using PatientManagement.API.Models;
using PatientManagement.API.Repository;

namespace PatientManagement.API.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepo;
    private readonly IPatientConditionRepository _pcRepo;

    public PatientService(IPatientRepository patientRepo, IPatientConditionRepository pcRepo)
    {
        _patientRepo = patientRepo;
        _pcRepo = pcRepo;
    }

    public async Task<int> AddPatientAsync(Patient patient, List<int>? conditionIds = null)
    {
        // Ensure email and phone uniqueness
        var existing = await _patientRepo.SearchPatientsAsync(null, null, null, null, null);
        if (existing.Any(x => x.Email.Equals(patient.Email, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("Email already exists.");
        if (existing.Any(x => x.Phone.Equals(patient.Phone)))
            throw new ArgumentException("Phone number already exists.");

        // Add patient
        var newId = await _patientRepo.AddPatientAsync(patient);

        if (conditionIds != null && conditionIds.Any())
        {
            foreach (var condId in conditionIds)
            {
                await _pcRepo.AddPatientConditionAsync(new PatientCondition
                {
                    PatientId = newId,
                    ConditionId = condId,
                    DiagnosedDate = DateOnly.FromDateTime(DateTime.UtcNow)
                });
            }
        }

        return newId;
    }

    public async Task<bool> UpdatePatientAsync(Patient patient)
    {

        // Ensure email and phone uniqueness (excluding self)
        var existing = await _patientRepo.SearchPatientsAsync(null, null, null, null, null) ?? new List<Patient>();
        if (existing.Any(x => x.Email.Equals(patient.Email, StringComparison.OrdinalIgnoreCase) && x.Id != patient.Id))
            throw new ArgumentException("Email already exists.");
        if (existing.Any(x => x.Phone.Equals(patient.Phone) && x.Id != patient.Id))
            throw new ArgumentException("Phone number already exists.");

        return await _patientRepo.UpdatePatientAsync(patient);
    }

    public async Task<bool> DeletePatientAsync(int patientId)
    {
        return await _patientRepo.DeletePatientAsync(patientId);
    }

    public async Task<Patient?> GetPatientByIdAsync(int patientId)
    {
        return await _patientRepo.GetPatientByIdAsync(patientId);
    }

    public async Task<List<Patient>> SearchPatientsAsync(string? city, string? gender, int? minAge, int? maxAge, int? conditionId)
    {
        return await _patientRepo.SearchPatientsAsync(city, gender, minAge, maxAge, conditionId);
    }
}
