using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LibraryManagementSystemAPI.Data
{
    public class Member
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } // Stored as hashed password

        public DateTime MembershipDate { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string StripeEmail { get; set; }
        
        public string? CustomerId { get; set; }

        [Required(ErrorMessage = "Late fee is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Late fee must be a non-negative value.")]
        public decimal LateFee { get; set; }
        public ICollection<Borrowing> Borrowings { get; set; }
        public ICollection<MemberRefreshToken>? MemberRefreshTokens { get; set; }
    }
}
