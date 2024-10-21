using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagementSystemAPI.Data;
using LibraryManagementSystemAPI.DTOModels;
using LibraryManagementSystemAPI.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace LibraryManagementSystemAPI.Services;

public class MemberAuthService(ApplicationDbContext dbContext, ILogger<MemberAuthService> logger, IConfiguration configuration)
{
    private void SetMemberSignUpRequest(ref Member member, DtoMember dtoMember)
    {
        member.FirstName = dtoMember.FirstName;
        member.LastName = dtoMember.LastName;
        member.Email = dtoMember.Email;
        member.MembershipDate = DateTime.Now;
        member.Password = BCrypt.Net.BCrypt.HashPassword(dtoMember.Password);
        member.StripeEmail = dtoMember.StripeEmail;
    }

    public async Task<int> SignUpAsync(DtoMember signUpMemberRequest, List<string> emailInfo) {
        if (signUpMemberRequest == null)
            return 0;

        // validate signUpMemberRequest data
        if (string.IsNullOrEmpty(signUpMemberRequest.Email)
            || string.IsNullOrEmpty(signUpMemberRequest.FirstName)
            || string.IsNullOrEmpty(signUpMemberRequest.LastName)
            || string.IsNullOrEmpty(signUpMemberRequest.Password)
            || string.IsNullOrEmpty(signUpMemberRequest.StripeEmail))
            return 0;

        var existingMember = dbContext.Members.SingleOrDefault(member => member.Email == signUpMemberRequest.Email);
        if (existingMember != null)
            return 0;

        Member member = new Member();

        using (var transaction = dbContext.Database.BeginTransaction()) {
            try
            {
                SetMemberSignUpRequest(ref member, signUpMemberRequest);
                dbContext.Add(member);
                dbContext.SaveChanges();
                
                transaction.Commit();
                emailInfo.Add(member.Email);
                emailInfo.Add(member.FirstName + " " + member.LastName);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                logger.LogCritical("problem to sign up a member, exception message: {x}", ex.Message);
            }
        }
        return member.Id;
    }

    public async Task<AuthModel> GetTokenAsync(AuthenticationRequest request) {
        if (request == null || string.IsNullOrEmpty(request.Identifier) || string.IsNullOrEmpty(request.Password))
            return new AuthModel { Message = "Invalid Credentials" };

        var user = dbContext.Members?.SingleOrDefault(u => u.Email == request.Identifier);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return new AuthModel { Message = "Invalid Credentials" };
        
        if (string.IsNullOrEmpty(configuration["Jwt:SigningKey"]))
            return new AuthModel { Message = "Invalid Credentials" };

        try
        {
            var accessToken = await CreateJwtTokenAsync(user);
            var authModel = new AuthModel();

            if (user.MemberRefreshTokens != null && user.MemberRefreshTokens.Any(x => x.IsActive)) {
                var activeRefreshToken = user.MemberRefreshTokens.FirstOrDefault(x => x.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                dbContext.Update(refreshToken);
                await dbContext.SaveChangesAsync();
            }
            // track users
            var newLogin = new User {
                UserId = user.Id,
                UserType = "member",
                LoginTime = DateTime.Now
            };
            dbContext.Add(newLogin);
            await dbContext.SaveChangesAsync();
            authModel.Message = "jwt token generated successfully";
            authModel.IsAuthenticated = true;
            authModel.AccessToken = accessToken;

            return authModel; 
        }
        catch (Exception ex)
        {
            return new AuthModel { Message = ex.Message };
        }
    }

    public async Task<string> CreateJwtTokenAsync(Member user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            Expires = DateTime.Now.AddMinutes(int.Parse(configuration["Jwt:LifeTime"])),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SigningKey"])), SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Role, "member"),
                new Claim("currentMemberId", user.Id.ToString())
            })
        };

        // Create the token
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        // Serialize the token
        var accessToken = tokenHandler.WriteToken(securityToken);

        return accessToken;
    }

    public async Task<AuthModel> RefreshTokenAsync(string token) {
        var authModel = new AuthModel();
        var user = await dbContext.Members.SingleOrDefaultAsync(a => a.MemberRefreshTokens.Any(t => t.Token == token));
        if (user == null) {
            authModel.Message = "invalid token";
            return authModel;
        }

        var refreshToken = user.MemberRefreshTokens.Single(t => t.Token == token);
        if (!refreshToken.IsActive) {
            authModel.Message = "invalid token";
            return authModel;
        }

        refreshToken.RevokedOn = DateTime.UtcNow;
        dbContext.Update(refreshToken);

        var newRefreshToken = GenerateRefreshToken();
        user.MemberRefreshTokens.Add(newRefreshToken);
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        var jwtToken = await CreateJwtTokenAsync(user);

        authModel.IsAuthenticated = true;
        authModel.Message = "valid refresh token";
        authModel.AccessToken = jwtToken;
        authModel.RefreshToken = newRefreshToken.Token;
        authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

        return authModel;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token) {
        var user = await dbContext.Members.SingleOrDefaultAsync(a => a.MemberRefreshTokens.Any(t => t.Token == token));
        if (user == null) {
            return false;
        }

        var refreshToken = user.MemberRefreshTokens.Single(t => t.Token == token);
        if (!refreshToken.IsActive) {
            return false;
        }

        refreshToken.RevokedOn = DateTime.UtcNow;
        dbContext.Update(refreshToken);
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();
        
        return true;
    }

    private MemberRefreshToken GenerateRefreshToken() {
        var randomNumber = new byte[32];
        using var rng = new RNGCryptoServiceProvider();
        rng.GetBytes(randomNumber);

        return new MemberRefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow,
        };
    }


}