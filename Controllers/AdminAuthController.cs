using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
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
using Newtonsoft.Json;

namespace LibraryManagementSystemAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly AdminAuthService _service;

    public AdminController(ApplicationDbContext dbContext, IConfiguration configuration, AdminAuthService service) {
        _dbContext = dbContext;
        _configuration = configuration;
        _service = service;
    }

    [HttpPost]
    [Route("signUp")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp(DtoAdmin request) {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        int returnCode = await _service.SignUpAsync(request);
        if (returnCode == 0)
            return BadRequest(returnCode);

        // send welcome email
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
    public async Task<IActionResult> GetRefreshToken([FromBody]string requestRefreshToken) {
        var refreshToken = Request.Cookies["adminRefreshToken"] ?? requestRefreshToken;
        
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest("token is Required");

        var result = await _service.RefreshTokenAsync(refreshToken);

        
        if (result == null)
            return BadRequest("invalid model");

        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
        return Ok(result);
    }

    [HttpPost]
    [Route("revokeToken")]
    public async Task<IActionResult> RevokeToken(string token) {
        var refreshToken = token ?? Request.Cookies["adminRefreshToken"];
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
        Response.Cookies.Append("adminRefreshToken", refreshToken, cookieOptions);
    }
    

}