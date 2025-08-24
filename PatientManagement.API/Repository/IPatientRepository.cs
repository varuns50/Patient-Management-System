using PatientManagement.API.Models;

namespace PatientManagement.API.Repository
{
    public interface IPatientRepository
    {
        Task<int> AddPatientAsync(Patient patient);
        Task<bool> UpdatePatientAsync(Patient patient);
        Task<bool> DeletePatientAsync(int patientId);
        Task<Patient?> GetPatientByIdAsync(int patientId);
        Task<List<Patient>> SearchPatientsAsync(string? city, string? gender, int? minAge, int? maxAge, int? conditionId);
    }
}
