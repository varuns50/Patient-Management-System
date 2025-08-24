using Xunit;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PatientManagement.API.Helpers;
using PatientManagement.API.Models;
using PatientManagement.API.Repository;

namespace PatientManagement.Tests.Repositories
{
    public class PatientConditionRepositoryTests
    {
        private readonly Mock<IDatabaseHelper> _db = new();
        private readonly PatientConditionRepository _repo;

        public PatientConditionRepositoryTests()
        {
            _repo = new PatientConditionRepository(_db.Object);
        }

        [Fact]
        public async Task AddPatientConditionAsync_CallsSp_AndReturnsTrue_WhenRowsAffected()
        {
            var pc = new PatientCondition { PatientId = 42, ConditionId = 3, DiagnosedDate = DateOnly.FromDateTime(System.DateTime.UtcNow) };

            _db.Setup(d => d.ExecuteStoredProcNonQueryAsync(
                "sp_AddPatientCondition",
                It.IsAny<Dictionary<string, object?>>()))
               .ReturnsAsync(1);

            var ok = await _repo.AddPatientConditionAsync(pc);

            ok.Should().BeTrue();

            _db.Verify(d => d.ExecuteStoredProcNonQueryAsync(
                "sp_AddPatientCondition",
                It.Is<Dictionary<string, object?>>(p =>
                    (int)p["@PatientId"]! == pc.PatientId &&
                    (int)p["@ConditionId"]! == pc.ConditionId &&
                    p.ContainsKey("@DiagnosedDate"))), Times.Once);
        }

        [Fact]
        public async Task RemovePatientConditionAsync_CallsSp_AndReturnsTrue_WhenRowsAffected()
        {
            _db.Setup(d => d.ExecuteStoredProcNonQueryAsync(
                "sp_RemovePatientCondition",
                It.IsAny<Dictionary<string, object?>>()))
               .ReturnsAsync(1);

            var ok = await _repo.RemovePatientConditionAsync(42, 3);

            ok.Should().BeTrue();

            _db.Verify(d => d.ExecuteStoredProcNonQueryAsync(
                "sp_RemovePatientCondition",
                It.Is<Dictionary<string, object?>>(p =>
                    (int)p["@PatientId"]! == 42 &&
                    (int)p["@ConditionId"]! == 3)), Times.Once);
        }

        [Fact]
        public async Task GetConditionsByPatientIdAsync_CallsSp_AndMapsList()
        {
            var rows = new List<Condition>
            {
                new Condition{ Id=1, Name="A"},
                new Condition{ Id=2, Name="B"}
            };

            _db.Setup(d => d.ExecuteStoredProcReaderAsync(
                "sp_GetConditionsByPatientId",
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<System.Func<SqlDataReader, Condition>>()))
               .ReturnsAsync(rows);

            var result = await _repo.GetConditionsByPatientIdAsync(7);

            result.Should().HaveCount(2);

            _db.Verify(d => d.ExecuteStoredProcReaderAsync(
                "sp_GetConditionsByPatientId",
                It.Is<Dictionary<string, object?>>(p => (int)p["@PatientId"]! == 7),
                It.IsAny<System.Func<SqlDataReader, Condition>>()), Times.Once);
        }
    }
}
