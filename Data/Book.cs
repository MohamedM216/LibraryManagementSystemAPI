using System.ComponentModel.DataAnnotations;
using LibraryManagementSystemAPI.Data;

public class Book
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Title { get; set; }

    [Required]
    [Isbn]
    public string ISBN { get; set; }

    [Range(1450, 2024)]
    public int PublishedYear { get; set; }

    [StringLength(100)]
    public string Genre { get; set; }

    [Range(0, int.MaxValue)]
    public int CopiesAvailable { get; set; }
    
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}
