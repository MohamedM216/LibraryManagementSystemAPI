using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemAPI.DTOModels
{
    public class DtoBook
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [Isbn]
        public string ISBN { get; set; }

        [Range(1450, 2100, ErrorMessage = "Published year must be between 1450 and 2100.")]
        public int PublishedYear { get; set; }

        [StringLength(100)]
        public string Genre { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Copies available cannot be negative.")]
        public int CopiesAvailable { get; set; }
    }
}
