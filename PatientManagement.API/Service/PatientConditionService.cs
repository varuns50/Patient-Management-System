using PatientManagement.API.Models;
using PatientManagement.API.Repository;

namespace PatientManagement.API.Services;

public class PatientConditionService : IPatientConditionService
{
    private readonly IPatientConditionRepository _pcRepo;

    public PatientConditionService(IPatientConditionRepository pcRepo)
    {
        _pcRepo = pcRepo;
    }

    public async Task<bool> AddPatientConditionAsync(PatientCondition pc)
    {
        return await _pcRepo.AddPatientConditionAsync(pc);
    }

    public async Task<bool> RemovePatientConditionAsync(int patientId, int conditionId)
    {
        return await _pcRepo.RemovePatientConditionAsync(patientId, conditionId);
    }

    public async Task<List<Condition>> GetConditionsByPatientIdAsync(int patientId)
    {
        return await _pcRepo.GetConditionsByPatientIdAsync(patientId);
    }
}
