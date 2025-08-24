using PatientManagement.API.Models;

namespace PatientManagement.API.Repository;

public interface IPatientConditionRepository
{
    /// <summary>
    /// Add a condition to a patient (many-to-many link).
    /// </summary>
    Task<bool> AddPatientConditionAsync(PatientCondition pc);

    /// <summary>
    /// Remove a condition link from a patient.
    /// </summary>
    Task<bool> RemovePatientConditionAsync(int patientId, int conditionId);

    /// <summary>
    /// Get all conditions assigned to a patient.
    /// </summary>
    Task<List<Condition>> GetConditionsByPatientIdAsync(int patientId);
}
