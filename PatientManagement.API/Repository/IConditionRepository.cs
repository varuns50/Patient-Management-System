using PatientManagement.API.Models;

namespace PatientManagement.API.Repository
{
    public interface IConditionRepository
    {
        Task<int> AddConditionAsync(Condition condition);
        Task<List<Condition>> GetAllConditionsAsync();
        Task<Condition?> GetConditionByIdAsync(int id);
    }
}
