using LibraryManagementSystemAPI.Data;
using LibraryManagementSystemAPI.DTOModels;
using LibraryManagementSystemAPI.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystemAPI.Services;


public class AdminAuthService(ApplicationDbContext dbContext, ILogger<AdminAuthService> logger, IConfiguration configuration)
{
    public async Task<int> SignUpAsync(DtoAdmin signUpAdminRequest) {
        if (signUpAdminRequest == null)
            return 0;

        // validate signUpMemberRequest data
        if (string.IsNullOrEmpty(signUpAdminRequest.Name)
            || string.IsNullOrEmpty(signUpAdminRequest.Password)
            || string.IsNullOrEmpty(signUpAdminRequest.AdminKey))
            return 0;

        if (signUpAdminRequest.AdminKey != configuration["AdminKey"])
            return 0;

        var existingAdmin = dbContext.Admins.SingleOrDefault(admin => admin.Name == signUpAdminRequest.Name);
        if (existingAdmin != null)
            return 0;

        Admin admin = new Admin();
        using (var transaction = dbContext.Database.BeginTransaction()) {
            try
            {
                admin.Name = signUpAdminRequest.Name;
                admin.Password = BCrypt.Net.BCrypt.HashPassword(signUpAdminRequest.Password);
                dbContext.Add(admin);
                dbContext.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                logger.LogCritical("problem to sign up an admin, exception message: {x}", ex.Message);
            }
        }
        return admin.Id;
    }

    public async Task<AuthModel> GetTokenAsync(AuthenticationRequest request) {
        if (request == null || string.IsNullOrEmpty(request.Identifier) || string.IsNullOrEmpty(request.Password))
            return new AuthModel { Message = "Invalid Credentials" };

        var user = dbContext.Admins?.SingleOrDefault(u => u.Name == request.Identifier);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return new AuthModel { Message = "Invalid Credentials" };
        
        if (string.IsNullOrEmpty(configuration["Jwt:SigningKey"]))
            return new AuthModel { Message = "Invalid Credentials" };

        try
        {
            var accessToken = await CreateJwtTokenAsync(user);
            var authModel = new AuthModel();

            if (user.AdminRefreshTokens != null && user.AdminRefreshTokens.Any(x => x.IsActive)) {
                var activeRefreshToken = user.AdminRefreshTokens.FirstOrDefault(x => x.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                dbContext.Add(refreshToken);
                await dbContext.SaveChangesAsync();
            }
            // track users
            var newLogin = new User {
                UserId = user.Id,
                UserType = "admin",
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
            return new AuthModel 
            { 
                Message = $"Exception: {ex.Message}, InnerException: {ex.InnerException?.Message}, Source: {ex.Source}" 
            };
        }
    }

    public async Task<string> CreateJwtTokenAsync(Admin user)
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
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Role, "admin"),
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
        var user = dbContext.Admins.SingleOrDefault(a => a.AdminRefreshTokens.Any(t => t.Token == token));
        if (user == null) {
            authModel.Message = "invalid token";
            return authModel;
        }

        var refreshToken = user.AdminRefreshTokens.Single(t => t.Token == token);
        if (!refreshToken.IsActive) {
            authModel.Message = "invalid token";
            return authModel;
        }

        refreshToken.RevokedOn = DateTime.UtcNow;
        dbContext.Update(refreshToken);

        var newRefreshToken = GenerateRefreshToken();
        user.AdminRefreshTokens.Add(newRefreshToken);
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
        var user = await dbContext.Admins.SingleOrDefaultAsync(a => a.AdminRefreshTokens.Any(t => t.Token == token));
        if (user == null) {
            return false;
        }

        var refreshToken = user.AdminRefreshTokens.Single(t => t.Token == token);
        if (!refreshToken.IsActive) {
            return false;
        }

        refreshToken.RevokedOn = DateTime.UtcNow;
        dbContext.Update(refreshToken);
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();
        
        return true;
    }

    private AdminRefreshToken GenerateRefreshToken() {
        var randomNumber = new byte[32];
        using var rng = new RNGCryptoServiceProvider();
        rng.GetBytes(randomNumber);

        return new AdminRefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow,
        };
    }

}