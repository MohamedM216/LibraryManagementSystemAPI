using LibraryManagementSystemAPI.Data;

namespace LibraryManagementSystemAPI.Services;

public class ReturnService {
    private readonly ApplicationDbContext _context;

    public ReturnService(ApplicationDbContext context) {
        _context = context;
    }

    public bool ReturnBook(int currentMemberId, string ISBN) {
        var book = _context.Books.FirstOrDefault(book => book.ISBN == ISBN);
        if (book == null)
            return false;

        var existingBorrowing = _context.Borrowings
            .OrderBy(borrow => borrow.Id)
            .LastOrDefault(borrow => borrow.BookId == book.Id && borrow.MemberId == currentMemberId);
        
        if (existingBorrowing == null) {
            Console.WriteLine("borrowing record is not found");
            return false;
        }

        // ckeck if returned before or on due date, or the member must pay fees
        existingBorrowing.ReturnDate = DateTime.Now;
        ++book.CopiesAvailable;
        _context.Update(book);
        _context.SaveChanges();
        Console.WriteLine("return book process succeedded");
        
        return true;
    }

}