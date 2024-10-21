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


public class AuthorAuthService(ApplicationDbContext dbContext, ILogger<AuthorAuthService> logger, IConfiguration configuration)
{

    private void SetAuthorSignUpRequest(ref Author author, DtoAuthor dtoAuthor)
    {
        author.FirstName = dtoAuthor.FirstName;
        author.LastName = dtoAuthor.LastName;
        author.Email = dtoAuthor.Email;
        author.Bio = dtoAuthor.Bio;
        author.Password = BCrypt.Net.BCrypt.HashPassword(dtoAuthor.Password);
        author.PaypalEmail = dtoAuthor.PaypalEmail;
    }
    public async Task<int> SignUpAsync(DtoAuthor signUpAuthorRequest, List<String> emailInfo) {
        if (signUpAuthorRequest == null)
            return 0;

        // validate signUpMemberRequest data
        if (string.IsNullOrEmpty(signUpAuthorRequest.Email)
            || string.IsNullOrEmpty(signUpAuthorRequest.Password)
            || string.IsNullOrEmpty(signUpAuthorRequest.Bio)
            || string.IsNullOrEmpty(signUpAuthorRequest.FirstName)
            || string.IsNullOrEmpty(signUpAuthorRequest.LastName)
            || string.IsNullOrEmpty(signUpAuthorRequest.PaypalEmail))
            return 0;

        var existingauthor = dbContext.Authors.SingleOrDefault(a => a.Email == signUpAuthorRequest.Email);
        if (existingauthor != null)
            return 0;

        Author author = new Author();
        using (var transaction = dbContext.Database.BeginTransaction()) {
            try
            {
                SetAuthorSignUpRequest(ref author, signUpAuthorRequest);
                dbContext.Add(author);
                await dbContext.SaveChangesAsync();

                emailInfo.Add(author.Email);
                emailInfo.Add(author.FirstName + " " + author.LastName);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                logger.LogCritical("problem to sign up an author, exception message: {x}", ex.Message);
            }
        }
        return author.Id;
    }

    public async Task<AuthModel> GetTokenAsync(AuthenticationRequest request) {
        if (request == null || string.IsNullOrEmpty(request.Identifier) || string.IsNullOrEmpty(request.Password))
            return new AuthModel { Message = "Invalid Credentials" };

        var user = dbContext.Authors?.SingleOrDefault(u => u.Email == request.Identifier);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return new AuthModel { Message = "Invalid Credentials" };
        
        if (string.IsNullOrEmpty(configuration["Jwt:SigningKey"]))
            return new AuthModel { Message = "Invalid Credentials" };

        try
        {
            var accessToken = await CreateJwtTokenAsync(user);
            var authModel = new AuthModel();


            if (user.AuthorRefreshTokens != null && user.AuthorRefreshTokens.Any(x => x.IsActive)) {
                var activeRefreshToken = user.AuthorRefreshTokens.FirstOrDefault(x => x.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else {
                var refreshToken = GenerateRefreshToken();
                refreshToken.AuthorId = user.Id;
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                dbContext.Add(refreshToken);
                await dbContext.SaveChangesAsync();
            }
            // track users
            var newLogin = new User {
                UserId = user.Id,
                UserType = "author",
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

    public async Task<string> CreateJwtTokenAsync(Author user)
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
                new Claim(ClaimTypes.Role, "author"),
                new Claim("Id", user.Id.ToString())
            })
        };

        // Create the token
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        // Serialize the token
        var accessToken = tokenHandler.WriteToken(securityToken);

        return accessToken;
    }

    public async Task<AuthModel> RefreshTokenAsync(string token) {
        var user = await dbContext.Authors
            .Include(a => a.AuthorRefreshTokens) // Ensure related tokens are loaded
            .SingleOrDefaultAsync(a => a.AuthorRefreshTokens.Any(t => t.Token == token));

        if (user == null) {
            return new AuthModel { Message = "token is required "};
        }

        var refreshToken = dbContext.AuthorRefreshTokens.Single(t => t.Token == token && t.AuthorId == user.Id);
        if (!refreshToken.IsActive) {
            return new AuthModel { Message = "token is required "};
        }

        refreshToken.RevokedOn = DateTime.UtcNow;
        dbContext.Update(refreshToken);

        var newRefreshToken = GenerateRefreshToken();
        newRefreshToken.AuthorId = user.Id;
        dbContext.Add(newRefreshToken);
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        var jwtToken = await CreateJwtTokenAsync(user);

        return new AuthModel {
            IsAuthenticated = true,
            Message = "Valid refresh token",
            AccessToken = jwtToken,
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpiration = newRefreshToken.ExpiresOn
        };
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token) {
        var user = await dbContext.Authors
            .Include(a => a.AuthorRefreshTokens) // Ensure related tokens are loaded
            .SingleOrDefaultAsync(a => a.AuthorRefreshTokens.Any(t => t.Token == token));
        if (user == null) {
            return false;
        }

        var refreshToken = dbContext.AuthorRefreshTokens.Single(t => t.Token == token && t.AuthorId == user.Id);
        if (!refreshToken.IsActive) {
            return false;
        }

        refreshToken.RevokedOn = DateTime.UtcNow;
        dbContext.Update(refreshToken);
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();
        
        return true;
    }

    private AuthorRefreshToken GenerateRefreshToken() {
        var randomNumber = new byte[32];
        using var rng = new RNGCryptoServiceProvider();
        rng.GetBytes(randomNumber);

        return new AuthorRefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow,
        };
    }

}