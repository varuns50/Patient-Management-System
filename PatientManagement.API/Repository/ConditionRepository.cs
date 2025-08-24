using PatientManagement.API.Helpers;
using PatientManagement.API.Models;
using PatientManagement.API.Repository;

namespace PatientManagement.API.Repository;

public class ConditionRepository : IConditionRepository
{
    private readonly IDatabaseHelper _db;

    public ConditionRepository(IDatabaseHelper db)
    {
        _db = db;
    }

    public async Task<int> AddConditionAsync(Condition condition)
    {
        return await _db.ExecuteStoredProcScalarAsync<int>(
            "sp_AddCondition",
            new Dictionary<string, object?>
            {
                {"@Name", condition.Name},
                {"@Description", condition.Description}
            });
    }

    public async Task<List<Condition>> GetAllConditionsAsync()
    {
        return await _db.ExecuteStoredProcReaderAsync(
            "sp_GetAllConditions",
            new Dictionary<string, object?>(),
            reader => new Condition
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2)
            });
    }

    public async Task<Condition?> GetConditionByIdAsync(int id)
    {
        var result = await _db.ExecuteStoredProcReaderAsync(
            "sp_GetConditionById",
            new Dictionary<string, object?> { { "@Id", id } },
            reader => new Condition
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2)
            });

        return result.FirstOrDefault();
    }
}
