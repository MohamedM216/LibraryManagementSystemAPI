using System.Security.Claims;
using LibraryManagementSystemAPI.Data;
using LibraryManagementSystemAPI.Filters;
using LibraryManagementSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystemAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BorrowingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly BorrowingService _service;
    public BorrowingController(ApplicationDbContext context, BorrowingService service) {
        _context = context;
        _service = service;
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "member")]
    public async Task<ActionResult> Borrow([FromBody]string ISBN) {
        if (!ModelState.IsValid || string.IsNullOrEmpty(ISBN))
            return BadRequest("invalid input");

        var memberIdClaim = User.FindFirstValue("currentMemberId");
        if (string.IsNullOrEmpty(memberIdClaim))
            return Unauthorized("Member ID is not present in claims");

        var currentMemberId = int.Parse(memberIdClaim);

        var returnCode = _service.BorrowBook(currentMemberId, ISBN);
        if (!returnCode) {
            return NotFound();

        }

        return Ok();
    }

    [HttpGet]
    [Route("")]
    [Authorize(Roles = "admin")]
    public ActionResult<IEnumerable<Borrowing>> GetAllBorrowings() {
        var records = _context.Borrowings.ToList();
        if (records == null || records.Count < 1)
            return NotFound();
        return Ok(records);
    }

    
    [HttpGet]
    [Route("GetByMemberId")]
    [Authorize(Roles = "member")]
    public ActionResult<IEnumerable<Borrowing>> GetBorrowedBooksByCurrentMemberId() {
        var memberIdClaim = User.FindFirstValue("currentMemberId");
        if (string.IsNullOrEmpty(memberIdClaim))
            return Unauthorized("Member ID is not present in claims");

        var currentMemberId = int.Parse(memberIdClaim);

        var records = _context.Borrowings
            .Where(borrow => borrow.MemberId == currentMemberId)
            .Include(b => b.Member) // Eager loading
            .Include(b => b.Book) // Eager loading
            .ToList();

        return Ok(records);
    }

}