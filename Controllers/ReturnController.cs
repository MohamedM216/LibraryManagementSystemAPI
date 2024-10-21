using System.Security.Claims;
using LibraryManagementSystemAPI.Data;
using LibraryManagementSystemAPI.Filters;
using LibraryManagementSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystemAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ReturnController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ReturnService _service;

    public ReturnController(ApplicationDbContext context, ReturnService service) {
        _context = context;
        _service = service;
    }

    [HttpPut]
    [Route("")]
    [Authorize(Roles = "member")]
    public ActionResult Return([FromBody]string ISBN) {
        if (!ModelState.IsValid || string.IsNullOrEmpty(ISBN))
            return BadRequest("invalid input");

        var memberIdClaim = User.FindFirstValue("currentMemberId");
        if (string.IsNullOrEmpty(memberIdClaim))
            return Unauthorized("Member ID is not present in claims");

        var currentMemberId = int.Parse(memberIdClaim);

        var returnCode = _service.ReturnBook(currentMemberId, ISBN);
        if (!returnCode)
            return NotFound();

        return Ok();
    }
}