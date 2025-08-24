using System.ComponentModel.DataAnnotations;

namespace PatientManagement.API.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public required string FirstName { get; set; }

        [Required, StringLength(100)]
        public required string LastName { get; set; }

        [Required, NotInFuture(ErrorMessage = "DOB cannot be in the future.")]
        public DateOnly DOB { get; set; }

        [StringLength(10)]
        [RegularExpression("Male|Female|Other", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string? Gender { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [Required, StringLength(255), EmailAddress]
        public required string Email { get; set; }

        [Required, StringLength(20)]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public required string Phone { get; set; }
    }
}
