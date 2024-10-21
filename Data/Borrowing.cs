using System.ComponentModel.DataAnnotations;
using LibraryManagementSystemAPI.Data;

public class Borrowing
{
    public int Id { get; set; }

    public int MemberId { get; set; }
    public Member Member { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }

    [DataType(DataType.Date)]
    public DateTime BorrowDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? ReturnDate { get; set; }
}
