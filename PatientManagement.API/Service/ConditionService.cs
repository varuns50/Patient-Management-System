using PatientManagement.API.Models;
using PatientManagement.API.Repository;

namespace PatientManagement.API.Services;

public class ConditionService : IConditionService
{
    private readonly IConditionRepository _conditionRepo;

    public ConditionService(IConditionRepository conditionRepo)
    {
        _conditionRepo = conditionRepo;
    }

    public async Task<int> AddConditionAsync(Condition condition)
    {
        return await _conditionRepo.AddConditionAsync(condition);
    }

    public async Task<List<Condition>> GetAllConditionsAsync()
    {
        return await _conditionRepo.GetAllConditionsAsync();
    }

    public async Task<Condition?> GetConditionByIdAsync(int id)
    {
        return await _conditionRepo.GetConditionByIdAsync(id);
    }
}
