using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemAPI.Data;

public class AuthorsDue
{
    public int Id { get; set; }
    public Author Author { get; set; }
    public int AuthorId { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string PaypalEmail { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Due Author Payout must be a positive value.")]
    public decimal DueAuthorPayout { get; set; }
    public Transaction Transaction { get; set; }
    public int TransactionId { get; set; }
    public bool IsDuePaid { get; set; } = false;
}