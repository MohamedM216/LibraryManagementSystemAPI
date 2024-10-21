using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemAPI.Data
{
    // enable admin from tracking users (i.e. create UserTrackingController class)
    public class User
    {
        public int UserId { get; set; }

        // Consider adding data annotations for validation
        [Required]
        [MaxLength(50)] // Adjust max length as needed
        public string UserType { get; set; }
 
        public DateTime LoginTime { get; set; }
    }
}
