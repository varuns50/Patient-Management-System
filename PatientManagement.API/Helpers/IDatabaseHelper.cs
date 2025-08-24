using Microsoft.Data.SqlClient;

namespace PatientManagement.API.Helpers;

public interface IDatabaseHelper
{
    Task<int> ExecuteStoredProcNonQueryAsync(string procName, Dictionary<string, object?> parameters);
    Task<T?> ExecuteStoredProcScalarAsync<T>(string procName, Dictionary<string, object?> parameters);
    Task<List<T>> ExecuteStoredProcReaderAsync<T>(
        string procName,
        Dictionary<string, object?> parameters,
        Func<SqlDataReader, T> mapper);
}
