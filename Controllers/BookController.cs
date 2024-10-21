using System.Security.Claims;
using LibraryManagementSystemAPI.Data;
using LibraryManagementSystemAPI.DTOModels;
using LibraryManagementSystemAPI.Filters;
using LibraryManagementSystemAPI.Services;
using LibraryManagementSystemAPI.ValidationClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystemAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly BookService _service ;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BookController> _logger;

    public BookController(BookService service, ApplicationDbContext context, ILogger<BookController> logger) {
        _service = service;
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "author")]
    public ActionResult<int> CreateNewBook(DtoBook book) {
        if (!ModelState.IsValid || book == null)
            return BadRequest();

        int authorId = int.Parse(User.FindFirstValue("Id"));

        var returnCode = _service.CreateBook(book, authorId);
        if (returnCode == -1)
            return BadRequest();
        return Ok(returnCode);
    }

    [HttpPut]
    [Route("")]
    [Authorize(Roles = "author")]
    // issue: i need to prevent author from changing the isbn of the book (i.e. he must 
    // insert books's isbn as it is or he willnot get the book or it will be like adding a new book in place of another one)
    public ActionResult UpdateBook(DtoBook book) {
        if (book == null)
            return NotFound();
        bool isUpdated = _service.UpdateBook(book);
        if (isUpdated)
            return Ok();
        return NotFound();
    }
    
    [HttpDelete]
    [Route("{id}")]
    [IdValidationFilter]
    [Authorize(Roles = "author")]
    public ActionResult RemoveBook(int id) {
        if (!ModelState.IsValid)
            return BadRequest("Invalid input");

        var isRemoved = _service.RemoveBook(id);
        if (isRemoved)
            return Ok();
        return NotFound();
    }


    [HttpGet]
    [Route("GetByTitle")]
    [IdValidationFilter]
    [Authorize(Roles = "admin, author, member")]
    public ActionResult<IEnumerable<Book>> GetBookByTitle([FromBody]string title) {
        if (!ModelState.IsValid || string.IsNullOrEmpty(title))
            return BadRequest("Invalid input");

        var existingBooks = _context.Books.Where(book => book.Title == title).ToList();
        if (existingBooks == null || existingBooks.Count < 1)
            return NotFound();
        return Ok(existingBooks);
    }

    [HttpGet]
    [Route("")]
    [Authorize(Roles = "member, author, admin")]
    public ActionResult<IEnumerable<Book>> GetAllBooks() {
        var books = _context.Books.ToList();
        if (books == null || books.Count < 1)
            return NotFound("no books available");
        return Ok(books);
    }

}