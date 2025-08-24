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
    public class PatientRepositoryTests
    {
        private readonly Mock<IDatabaseHelper> _db = new();
        private readonly PatientRepository _repo;

        public PatientRepositoryTests()
        {
            _repo = new PatientRepository(_db.Object);
        }

        private static Patient MakePatient() => new Patient
        {
            FirstName = "Peter",
            LastName = "Parker",
            DOB = DateOnly.FromDateTime(new System.DateTime(1995, 8, 10)),
            Gender = "Male",
            City = "Queens",
            Email = "peter@bugle.com",
            Phone = "9999999999"
        };

        [Fact]
        public async Task AddPatientAsync_CallsSp_AddPatient_AndReturnsNewId()
        {
            var p = MakePatient();

            _db.Setup(d => d.ExecuteStoredProcScalarAsync<int>(
                    "sp_AddPatient",
                    It.IsAny<Dictionary<string, object?>>()))
               .ReturnsAsync(77);

            var id = await _repo.AddPatientAsync(p);

            id.Should().Be(77);

            _db.Verify(d => d.ExecuteStoredProcScalarAsync<int>(
                "sp_AddPatient",
                It.Is<Dictionary<string, object?>>(dict =>
                    dict.ContainsKey("@FirstName") &&
                    dict.ContainsKey("@LastName") &&
                    dict.ContainsKey("@DOB") &&
                    dict.ContainsKey("@Gender") &&
                    dict.ContainsKey("@City") &&
                    dict.ContainsKey("@Email") &&
                    dict.ContainsKey("@Phone"))), Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_MapsReader_ToModel()
        {
            // Arrange: simulate reader -> one row mapped to Patient
            _db.Setup(d => d.ExecuteStoredProcReaderAsync(
                    "sp_GetPatientById",
                    It.IsAny<Dictionary<string, object?>>(),
                    It.IsAny<System.Func<SqlDataReader, Patient>>()))
               .ReturnsAsync(new List<Patient> { new Patient { Id = 5, FirstName = "Mickey", LastName = "Mouse", Email="micky.mouse@avengers.com", Phone = "556464644" } });

            // Act
            var p = await _repo.GetPatientByIdAsync(5);

            // Assert
            p.Should().NotBeNull();
            p!.Id.Should().Be(5);

            _db.Verify(d => d.ExecuteStoredProcReaderAsync(
                "sp_GetPatientById",
                It.Is<Dictionary<string, object?>>(dict => (int)dict["@Id"]! == 5),
                It.IsAny<System.Func<SqlDataReader, Patient>>()), Times.Once);
        }
    }
}
