using LibraryManagementSystemAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystemAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "admin")]
public class TrackUsersController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [Route("")]
    public ActionResult<IEnumerable<User>> GetAllLogins() {
        var records = dbContext.Users.ToList();
        if (records.Count < 1)
            return BadRequest("null or empty records");
        return Ok(records);
    }

    [HttpGet]
    [Route("GetTransactions")]
    public ActionResult<IEnumerable<Transaction>> GetAllTransaction() {
        var records = dbContext.Transactions.ToList();
        if (records.Count < 1)
            return BadRequest("null or empty records");
        return Ok(records);
    }

    [HttpGet]
    [Route("{transactionId}")]
    public ActionResult<IEnumerable<Transaction>> GetTransactionById(int transactionId) {
        var record = dbContext.Transactions.FirstOrDefault(t => t.Id == transactionId);
        if (record == null)
            return BadRequest("null or empty record");
        return Ok(record);
    }
}
