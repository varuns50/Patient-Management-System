using PatientManagement.API.Models;

namespace PatientManagement.API.Services;

public interface IPatientService
{
    Task<int> AddPatientAsync(Patient patient, List<int>? conditionIds = null);
    Task<bool> UpdatePatientAsync(Patient patient);
    Task<bool> DeletePatientAsync(int patientId);
    Task<Patient?> GetPatientByIdAsync(int patientId);
    Task<List<Patient>> SearchPatientsAsync(string? city, string? gender, int? minAge, int? maxAge, int? conditionId);
}
