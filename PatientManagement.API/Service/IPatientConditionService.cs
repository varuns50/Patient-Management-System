using PatientManagement.API.Models;

namespace PatientManagement.API.Services;

public interface IPatientConditionService
{
    Task<bool> AddPatientConditionAsync(PatientCondition pc);
    Task<bool> RemovePatientConditionAsync(int patientId, int conditionId);
    Task<List<Condition>> GetConditionsByPatientIdAsync(int patientId);
}
