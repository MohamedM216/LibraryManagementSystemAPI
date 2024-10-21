using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemAPI.Data
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Transaction date is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Transaction), nameof(ValidateTransactionDate))]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Book ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Book ID must be a positive integer.")]
        public int bookId { get; set; }

        [Required(ErrorMessage = "Member ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Member ID must be a positive integer.")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Customer ID must be a positive integer.")]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "Author ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Author ID must be a positive integer.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Delivery cost is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Delivery cost must be a non-negative value.")]
        public decimal DeliveryCost { get; set; }

        [Required(ErrorMessage = "Author payout is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Author payout must be a non-negative value.")]
        public decimal AuthorPayout { get; set; }

        [Required(ErrorMessage = "Patment Intent ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Payment Intent ID must be a positive integer.")]
        public string PaymentIntentId { get; set; }
        public AuthorsDue AuthorsDue { get; set; }

        // Custom validation method for Transaction Date
        public static ValidationResult ValidateTransactionDate(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Now)
            {
                return new ValidationResult("Transaction date cannot be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}
