using PatientManagement.API.Models;

namespace PatientManagement.API.Services;

public interface IConditionService
{
    Task<int> AddConditionAsync(Condition condition);
    Task<List<Condition>> GetAllConditionsAsync();
    Task<Condition?> GetConditionByIdAsync(int id);
}
