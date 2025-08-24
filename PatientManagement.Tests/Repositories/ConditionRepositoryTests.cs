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
    public class ConditionRepositoryTests
    {
        private readonly Mock<IDatabaseHelper> _db = new();
        private readonly ConditionRepository _repo;

        public ConditionRepositoryTests()
        {
            _repo = new ConditionRepository(_db.Object);
        }

        [Fact]
        public async Task AddConditionAsync_CallsSp_AndReturnsNewId()
        {
            var c = new Condition { Name = "Rich Genius", Description = "Tech + $$" };

            _db.Setup(d => d.ExecuteStoredProcScalarAsync<int>(
                "sp_AddCondition",
                It.IsAny<Dictionary<string, object?>>()))
               .ReturnsAsync(55);

            var id = await _repo.AddConditionAsync(c);

            id.Should().Be(55);

            _db.Verify(d => d.ExecuteStoredProcScalarAsync<int>(
                "sp_AddCondition",
                It.Is<Dictionary<string, object?>>(p =>
                    (string)p["@Name"]! == c.Name &&
                    (string?)p["@Description"] == c.Description)), Times.Once);
        }

        [Fact]
        public async Task GetAllConditionsAsync_MapsRows()
        {
            var rows = new List<Condition>
            {
                new Condition{ Id=1, Name="A", Description="x"},
                new Condition{ Id=2, Name="B", Description=null}
            };

            _db.Setup(d => d.ExecuteStoredProcReaderAsync(
                "sp_GetAllConditions",
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<System.Func<SqlDataReader, Condition>>()))
               .ReturnsAsync(rows);

            var result = await _repo.GetAllConditionsAsync();

            result.Should().HaveCount(2);
            _db.Verify(d => d.ExecuteStoredProcReaderAsync(
                "sp_GetAllConditions",
                It.Is<Dictionary<string, object?>>(p => p.Count == 0),
                It.IsAny<System.Func<SqlDataReader, Condition>>()), Times.Once);
        }

        [Fact]
        public async Task GetConditionByIdAsync_PassesId_AndMaps()
        {
            var row = new Condition { Id = 9, Name = "Detective Skills", Description = "World-class" };

            _db.Setup(d => d.ExecuteStoredProcReaderAsync(
                "sp_GetConditionById",
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<System.Func<SqlDataReader, Condition>>()))
               .ReturnsAsync(new List<Condition> { row });

            var result = await _repo.GetConditionByIdAsync(9);

            result.Should().NotBeNull();
            result!.Id.Should().Be(9);

            _db.Verify(d => d.ExecuteStoredProcReaderAsync(
                "sp_GetConditionById",
                It.Is<Dictionary<string, object?>>(p => (int)p["@Id"]! == 9),
                It.IsAny<System.Func<SqlDataReader, Condition>>()), Times.Once);
        }
    }
}
