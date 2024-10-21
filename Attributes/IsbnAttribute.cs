using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class IsbnAttribute : ValidationAttribute
{
        //     Valid ISBN-10:

        //     0-306-40615-2
        //     0 943396 04 2
        //     0-330-28777-8
        //     0 7167 0344 0

        // Valid ISBN-13:

        //     978-3-16-148410-0
        //     979-0-345-53392-4
        //     978 1 56619 909 4
        //     979 12 3456 78901
    private const string IsbnRegexPattern = @"^(?:97[89][- ]?)?\d{1,5}[- ]?\d{1,7}[- ]?\d{1,6}[- ]?[\dX]$";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var isbn = value as string;
        if (string.IsNullOrEmpty(isbn))
        {
            return new ValidationResult("ISBN is required.");
        }

        // Check the basic format using regex
        if (!Regex.IsMatch(isbn, IsbnRegexPattern))
        {
            return new ValidationResult("Invalid ISBN format.");
        }

        // Remove hyphens and spaces for validation
        isbn = isbn.Replace("-", "").Replace(" ", "");

        // Validate ISBN-10 or ISBN-13 check digit
        if (isbn.Length == 10)
        {
            if (!IsValidIsbn10(isbn))
            {
                return new ValidationResult("Invalid ISBN-10 check digit.");
            }
        }
        else if (isbn.Length == 13)
        {
            if (!IsValidIsbn13(isbn))
            {
                return new ValidationResult("Invalid ISBN-13 check digit.");
            }
        }
        else
        {
            return new ValidationResult("ISBN must be either 10 or 13 digits long.");
        }

        return ValidationResult.Success;
    }

    private bool IsValidIsbn10(string isbn)
    {
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            if (!char.IsDigit(isbn[i]))
            {
                return false; // Invalid character
            }
            sum += (i + 1) * (isbn[i] - '0');
        }

        char lastChar = isbn[9];
        int checkDigit = (lastChar == 'X') ? 10 : lastChar - '0';

        sum += 10 * checkDigit;

        return sum % 11 == 0;
    }

    private bool IsValidIsbn13(string isbn)
    {
        int sum = 0;

        for (int i = 0; i < 12; i++)
        {
            int digit = isbn[i] - '0';

            if (i % 2 == 0)
            {
                sum += digit; // Add 1x for even index positions
            }
            else
            {
                sum += 3 * digit; // Add 3x for odd index positions
            }
        }

        int checkDigit = (10 - (sum % 10)) % 10;

        return checkDigit == (isbn[12] - '0');
    }
}
