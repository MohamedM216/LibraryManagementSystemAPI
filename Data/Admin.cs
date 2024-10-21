using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemAPI.Data;

public class Admin {
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; }    // hashed
    public ICollection<AdminRefreshToken>? AdminRefreshTokens { get; set; }
}