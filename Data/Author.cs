using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemAPI.Data
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } // Should be hashed

        [StringLength(500, ErrorMessage = "Bio must not exceed 500 characters.")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string PaypalEmail { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<AuthorsDue> AuthorsDues { get; set; }
        public ICollection<AuthorRefreshToken>? AuthorRefreshTokens { get; set; }
    }
}
