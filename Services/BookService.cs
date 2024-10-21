using System.Runtime.CompilerServices;
using LibraryManagementSystemAPI.Data;
using LibraryManagementSystemAPI.DTOModels;
using LibraryManagementSystemAPI.ValidationClasses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystemAPI.Services;

public class BookService
{
    private readonly ApplicationDbContext _context;

    public BookService(ApplicationDbContext context) {
        _context = context;
    }

    private void SetDTOBookToBookObj(ref Book book, DtoBook dtoBook)
    {
        book.Title = dtoBook.Title;
        book.ISBN = dtoBook.ISBN;
        book.PublishedYear = dtoBook.PublishedYear;
        book.CopiesAvailable = dtoBook.CopiesAvailable;
        book.Genre = dtoBook.Genre;
    }
    public bool ValidateBookInfo(DtoBook book) {
        if (string.IsNullOrEmpty(book.ISBN) || string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.Genre))
            return false;
        
        if (book.CopiesAvailable < 1)
            return false;
        
        if (book.PublishedYear < 1450 || book.PublishedYear > DateTime.Now.Year)
            return false;
        return true;
    }

    public int CreateBook(DtoBook dtoBook, int authorId) {
        if (dtoBook == null || !ValidateBookInfo(dtoBook))
            return -1;

        var existingbook = _context.Books.SingleOrDefault(book => book.ISBN == dtoBook.ISBN);
        if (existingbook != null) {
            Console.WriteLine("this book is already found! Try later");
            return -1;
        }
        Book book = new Book();
        SetDTOBookToBookObj(ref book, dtoBook);
        book.AuthorId = authorId;
        _context.Add(book);
        _context.SaveChanges();
        return book.Id;
    }

    public bool UpdateBook(DtoBook dtoBook) {
        if (dtoBook == null || !ValidateBookInfo(dtoBook))
            return false;

        var book = _context.Books.SingleOrDefault(book => book.ISBN == dtoBook.ISBN);

        if (book == null) {
            Console.WriteLine("book not found");
            return false;
        }
        
        // what if we added new record has the same values in another record (the only difference between them is PK(id))
        SetDTOBookToBookObj(ref book, dtoBook);

        _context.Update(book);
        _context.SaveChanges();
        return true;
    }

    public bool RemoveBook(int bookId) {
        var existngBook = _context.Books.SingleOrDefault(book => book.Id == bookId);
        if (existngBook == null) {
            Console.WriteLine("book not found");
            return false;
        }
        _context.Remove(existngBook);
        _context.SaveChanges();
        return true;
    }

}