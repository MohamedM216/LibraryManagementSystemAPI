using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemAPI.DTOModels;

public class DtoAdmin {
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#.])[A-Za-z\d@$!%*?&#.]{8,}$", 
        ErrorMessage = "Password must have at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
    public string Password { get; set; }
    public string AdminKey { get; set; }
}