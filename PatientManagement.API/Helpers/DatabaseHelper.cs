using Microsoft.Data.SqlClient;
using System.Data;

namespace PatientManagement.API.Helpers
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PatientDBConnection")!;
        }

        public async Task<int> ExecuteStoredProcNonQueryAsync(string procName, Dictionary<string, object?> parameters)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure };
            AddParameters(cmd, parameters);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<T?> ExecuteStoredProcScalarAsync<T>(string procName, Dictionary<string, object?> parameters)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure };
            AddParameters(cmd, parameters);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            if (result == DBNull.Value || result == null)
                return default;

            return (T)Convert.ChangeType(result, typeof(T));
        }

        public async Task<List<T>> ExecuteStoredProcReaderAsync<T>(
            string procName,
            Dictionary<string, object?> parameters,
            Func<SqlDataReader, T> mapper)
        {
            var results = new List<T>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure };
            AddParameters(cmd, parameters);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(mapper(reader));
            }

            return results;
        }

        private static void AddParameters(SqlCommand cmd, Dictionary<string, object?> parameters)
        {
            foreach (var kvp in parameters)
            {
                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value ?? DBNull.Value);
            }
        }
    }
}
