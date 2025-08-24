using PatientManagement.API.Helpers;
using PatientManagement.API.Models;

namespace PatientManagement.API.Repository;

public class PatientConditionRepository : IPatientConditionRepository
{
    private readonly IDatabaseHelper _db;

    public PatientConditionRepository(IDatabaseHelper db)
    {
        _db = db;
    }

    public async Task<bool> AddPatientConditionAsync(PatientCondition pc)
    {
        var rows = await _db.ExecuteStoredProcNonQueryAsync(
            "sp_AddPatientCondition",
            new Dictionary<string, object?>
            {
                {"@PatientId", pc.PatientId},
                {"@ConditionId", pc.ConditionId},
                {"@DiagnosedDate", pc.DiagnosedDate.ToDateTime(TimeOnly.MinValue)}
            });

        return rows > 0;
    }

    public async Task<bool> RemovePatientConditionAsync(int patientId, int conditionId)
    {
        var rows = await _db.ExecuteStoredProcNonQueryAsync(
            "sp_RemovePatientCondition",
            new Dictionary<string, object?>
            {
                {"@PatientId", patientId},
                {"@ConditionId", conditionId}
            });

        return rows > 0;
    }

    public async Task<List<Condition>> GetConditionsByPatientIdAsync(int patientId)
    {
        return await _db.ExecuteStoredProcReaderAsync(
            "sp_GetConditionsByPatientId",
            new Dictionary<string, object?> { { "@PatientId", patientId } },
            reader => new Condition
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2)
            });
    }
}
