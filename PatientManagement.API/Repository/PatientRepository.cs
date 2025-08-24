using PatientManagement.API.Helpers;
using PatientManagement.API.Models;
namespace PatientManagement.API.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IDatabaseHelper _db;

        public PatientRepository(IDatabaseHelper db)
        {
            _db = db;
        }

        public async Task<int> AddPatientAsync(Patient patient)
        {
            var newId = await _db.ExecuteStoredProcScalarAsync<int>(
                "sp_AddPatient",
                new Dictionary<string, object?>
                {
                {"@FirstName", patient.FirstName},
                {"@LastName", patient.LastName},
                {"@DOB", patient.DOB.ToDateTime(TimeOnly.MinValue)},
                {"@Gender", patient.Gender},
                {"@City", patient.City},
                {"@Email", patient.Email},
                {"@Phone", patient.Phone}
                });

            patient.Id = newId;
            return newId;
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            var rows = await _db.ExecuteStoredProcNonQueryAsync("sp_UpdatePatient", new Dictionary<string, object?>
        {
            {"@Id", patient.Id},
            {"@FirstName", patient.FirstName},
            {"@LastName", patient.LastName},
            {"@DOB", patient.DOB.ToDateTime(TimeOnly.MinValue)},
            {"@Gender", patient.Gender},
            {"@City", patient.City},
            {"@Email", patient.Email},
            {"@Phone", patient.Phone}
        });

            return rows > 0;
        }

        public async Task<bool> DeletePatientAsync(int patientId)
        {
            var rows = await _db.ExecuteStoredProcNonQueryAsync("sp_DeletePatient", new Dictionary<string, object?>
        {
            {"@Id", patientId}
        });

            return rows > 0;
        }

        public async Task<Patient?> GetPatientByIdAsync(int patientId)
        {
            var result = await _db.ExecuteStoredProcReaderAsync("sp_GetPatientById",
                new Dictionary<string, object?> { { "@Id", patientId } },
                reader => new Patient
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    DOB = DateOnly.FromDateTime(reader.GetDateTime(3)),
                    Gender = reader.IsDBNull(4) ? null : reader.GetString(4),
                    City = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Email = reader.GetString(6),
                    Phone = reader.GetString(7)
                });

            return result.FirstOrDefault();
        }

        public async Task<List<Patient>> SearchPatientsAsync(string? city, string? gender, int? minAge, int? maxAge, int? conditionId)
        {
            return await _db.ExecuteStoredProcReaderAsync("sp_SearchPatients",
                new Dictionary<string, object?>
                {
                {"@City", city},
                {"@Gender", gender},
                {"@MinAge", minAge},
                {"@MaxAge", maxAge},
                {"@ConditionId", conditionId}
                },
                reader => new Patient
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    DOB = DateOnly.FromDateTime(reader.GetDateTime(3)),
                    Gender = reader.IsDBNull(4) ? null : reader.GetString(4),
                    City = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Email = reader.GetString(6),
                    Phone = reader.GetString(7)
                });
        }
    }
}
