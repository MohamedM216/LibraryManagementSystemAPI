using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagementSystemAPI.Data;
using LibraryManagementSystemAPI.DTOModels;
using LibraryManagementSystemAPI.Options;
using LibraryManagementSystemAPI.Requests;
using LibraryManagementSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagementSystemAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly AuthorAuthService _service;
    private readonly IConfiguration _configuration;
    private readonly WelcomeEmailService _welcomeEmailService;

    public AuthorController(ApplicationDbContext dbContext, AuthorAuthService service, IConfiguration configuration, WelcomeEmailService welcomeEmailService) {
        _dbContext = dbContext;
        _service = service;
        _configuration = configuration;
        _welcomeEmailService = welcomeEmailService;
    }

    [HttpPost]
    [Route("signUp")]
    public async Task<IActionResult> SignUp(DtoAuthor request) {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var emailInfo = new List<string>();
        int returnCode = await _service.SignUpAsync(request, emailInfo);
        if (returnCode == 0)
            return BadRequest(returnCode);

        await _welcomeEmailService.SendWelcomeEmailAsync(emailInfo[0], emailInfo[1]);
        return Ok(returnCode);
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTokenAsync([FromBody]AuthenticationRequest request) {
        if (!ModelState.IsValid)
            return BadRequest("Invalid Credentials");

        var authModel = await _service.GetTokenAsync(request);
        if (authModel == null || !authModel.IsAuthenticated)
            return Unauthorized(authModel?.Message);

        if (!string.IsNullOrEmpty(authModel.RefreshToken))
            SetRefreshTokenInCookie(authModel.RefreshToken, authModel.RefreshTokenExpiration);

        return Ok(authModel);
    }

    [HttpPost]
    [Route("refreshToken")]
    public async Task<IActionResult> GetRefreshToken([FromBody] string requestRefreshToken) {
        var refreshToken = Request.Cookies["authorRefreshToken"] ?? requestRefreshToken;

        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest(new { Message = "token is required" });

        var result = await _service.RefreshTokenAsync(refreshToken);

        if (result == null || !result.IsAuthenticated)
            return BadRequest(new { Message = result?.Message ?? "Invalid token" });

        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
        return Ok(result);
    }


    [HttpPost]
    [Route("revokeToken")]
    public async Task<IActionResult> RevokeToken(string token) {
        var refreshToken = token ?? Request.Cookies["authorRefreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest("token is required");

        var result = await _service.RevokeRefreshTokenAsync(token);

        if (!result)
            return BadRequest("token is invalid");
        
        return Ok();
    }

    private void SetRefreshTokenInCookie(string refreshToken, DateTime expiresOn) {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expiresOn.ToLocalTime()
        };
        Response.Cookies.Append("authorRefreshToken", refreshToken, cookieOptions);
    }
    

    [HttpGet]
    [Route("")]
    [Authorize(Roles = "author")]
    public ActionResult<IEnumerable<Transaction>> GetAllAuthorTransactions() {
        var records = _dbContext.Transactions.Where(t => t.AuthorId == int.Parse(User.FindFirstValue("Id"))).ToList();
        if (records.Count < 1)
            return BadRequest("null or empty records");
        return Ok(records);
    }

}