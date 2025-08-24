using System.ComponentModel.DataAnnotations;

namespace PatientManagement.API.Models
{
    public class PatientCondition
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int ConditionId { get; set; }

        [Required]
        public DateOnly DiagnosedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
