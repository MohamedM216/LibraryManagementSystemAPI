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
public class MemberController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly MemberAuthService _service;
    private readonly IConfiguration _configuration;
    private readonly WelcomeEmailService _welcomeEmailService;

    public MemberController(ApplicationDbContext dbContext, MemberAuthService service, IConfiguration configuration, WelcomeEmailService welcomeEmailService) {
        _dbContext = dbContext;
        _service = service;
        _configuration = configuration;
        _welcomeEmailService = welcomeEmailService;
    }

    [HttpPost]
    [Route("signUp")]
    public async Task<IActionResult> SignUp(DtoMember request) {
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
            return Unauthorized(authModel.Message);

        if (!string.IsNullOrEmpty(authModel.RefreshToken))
            SetRefreshTokenInCookie(authModel.RefreshToken, authModel.RefreshTokenExpiration);

        return Ok(authModel);
    }

    [HttpGet]
    [Route("refreshToken")]
    public async Task<IActionResult> GetRefreshToken() {
        var refreshToken = Request.Cookies["memberRefreshToken"];
        var result = await _service.RefreshTokenAsync(refreshToken);

        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest(result);

        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
        return Ok(result);
    }

    [HttpPost]
    [Route("revokeToken")]
    public async Task<IActionResult> RevokeToken(string token) {
        var refreshToken = token ?? Request.Cookies["memberRefreshToken"];
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
        Response.Cookies.Append("memberRefreshToken", refreshToken, cookieOptions);
    }

    [HttpGet]
    [Route("")]
    [Authorize(Roles = "member")]
    public ActionResult<IEnumerable<Transaction>> GetAllMemberTransactions() {
        var records = _dbContext.Transactions.Where(t => t.MemberId == int.Parse(User.FindFirstValue("currentMemberId"))).ToList();
        if (records.Count < 1)
            return BadRequest("null or empty records");
        return Ok(records);
    }

}