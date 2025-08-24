using Moq;
using PatientManagement.API.Models;
using PatientManagement.API.Repository;
using PatientManagement.API.Services;
using System.ComponentModel.DataAnnotations;

namespace PatientManagement.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepoMock = new();
        private readonly Mock<IPatientConditionRepository> _pcRepoMock = new();
        private readonly IPatientService _svc;

        public PatientServiceTests()
        {
            _svc = new PatientService(_patientRepoMock.Object, _pcRepoMock.Object);
        }

        private static Patient MakeValidPatient() => new Patient
        {
            FirstName = "Tony",
            LastName = "Stark",
            DOB = DateOnly.FromDateTime(new DateTime(1970, 5, 29)),
            Gender = "Male",
            City = "New York",
            Email = "tony.stark@avengers.com",
            Phone = "1234567890"
        };

        [Fact]
        public async Task AddPatient_ShouldThrow_WhenEmailExists()
        {
            var p = MakeValidPatient();

            _patientRepoMock
                .Setup(r => r.SearchPatientsAsync(null, null, null, null, null))
                .ReturnsAsync(new List<Patient> {
                    new Patient { Id = 99, FirstName="X", LastName="Y", DOB = p.DOB, Email = p.Email, Phone="999" }
                });

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _svc.AddPatientAsync(p, new List<int>()));

            Assert.Equal("Email already exists.", ex.Message);
            _patientRepoMock.Verify(r => r.AddPatientAsync(It.IsAny<Patient>()), Times.Never);
            _pcRepoMock.Verify(r => r.AddPatientConditionAsync(It.IsAny<PatientCondition>()), Times.Never);
        }

        [Fact]
        public async Task AddPatient_ShouldThrow_WhenPhoneExists()
        {
            var p = MakeValidPatient();

            _patientRepoMock
                .Setup(r => r.SearchPatientsAsync(null, null, null, null, null))
                .ReturnsAsync(new List<Patient> {
                    new Patient { Id = 101, FirstName="A", LastName="B", DOB = p.DOB, Email = "other@mail.com", Phone=p.Phone }
                });

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _svc.AddPatientAsync(p, new List<int>()));

            Assert.Equal("Phone number already exists.", ex.Message);
            _patientRepoMock.Verify(r => r.AddPatientAsync(It.IsAny<Patient>()), Times.Never);
            _pcRepoMock.Verify(r => r.AddPatientConditionAsync(It.IsAny<PatientCondition>()), Times.Never);
        }

        [Fact]
        public async Task AddPatient_ShouldInsertPatient_AndNotCallPcRepo_WhenNoConditions()
        {
            var p = MakeValidPatient();

            _patientRepoMock
                .Setup(r => r.SearchPatientsAsync(null, null, null, null, null))
                .ReturnsAsync(new List<Patient>()); // no dupes

            _patientRepoMock
                .Setup(r => r.AddPatientAsync(It.IsAny<Patient>()))
                .ReturnsAsync(1);

            var id = await _svc.AddPatientAsync(p, new List<int>()); // empty list

            Assert.Equal(1, id);
            _patientRepoMock.Verify(r => r.AddPatientAsync(It.IsAny<Patient>()), Times.Once);
            _pcRepoMock.Verify(r => r.AddPatientConditionAsync(It.IsAny<PatientCondition>()), Times.Never);
        }

        [Fact]
        public async Task AddPatient_ShouldInsertPatient_AndCallPcRepo_ForEachCondition()
        {
            var p = MakeValidPatient();
            var conditionIds = new List<int> { 10, 20, 30 };

            _patientRepoMock
                .Setup(r => r.SearchPatientsAsync(null, null, null, null, null))
                .ReturnsAsync(new List<Patient>());

            _patientRepoMock
                .Setup(r => r.AddPatientAsync(It.IsAny<Patient>()))
                .ReturnsAsync(42);

            _pcRepoMock
                .Setup(r => r.AddPatientConditionAsync(It.IsAny<PatientCondition>()))
                .ReturnsAsync(true);

            var id = await _svc.AddPatientAsync(p, conditionIds);

            Assert.Equal(42, id);
            _patientRepoMock.Verify(r => r.AddPatientAsync(It.IsAny<Patient>()), Times.Once);
            _pcRepoMock.Verify(r => r.AddPatientConditionAsync(It.Is<PatientCondition>(pc => pc.PatientId == 42)), Times.Exactly(conditionIds.Count));
        }

        [Fact]
        public async Task UpdatePatient_ShouldThrow_WhenEmailOrPhoneTakenByOthers()
        {
            var p = MakeValidPatient();
            p.Id = 7;

            _patientRepoMock
                .Setup(r => r.SearchPatientsAsync(null, null, null, null, null))
                .ReturnsAsync(new List<Patient> {
                    new Patient { Id = 8, FirstName="Dup", LastName="User", DOB=p.DOB, Email=p.Email, Phone="555" },
                    new Patient { Id = 9, FirstName="Dup2", LastName="User2", DOB=p.DOB, Email="x@mail.com", Phone=p.Phone }
                });

            await Assert.ThrowsAsync<ArgumentException>(() => _svc.UpdatePatientAsync(p));
            _patientRepoMock.Verify(r => r.UpdatePatientAsync(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePatient_ShouldCallRepo_WhenValid()
        {
            var p = MakeValidPatient();
            p.Id = 11;

            _patientRepoMock
                .Setup(r => r.SearchPatientsAsync(null, null, null, null, null))
                .ReturnsAsync(new List<Patient>());
            _patientRepoMock
                .Setup(r => r.UpdatePatientAsync(p))
                .ReturnsAsync(true);

            var ok = await _svc.UpdatePatientAsync(p);

            Assert.True(ok);
            _patientRepoMock.Verify(r => r.UpdatePatientAsync(p), Times.Once);
        }

        [Fact]
        public async Task DeletePatient_ShouldCallRepo()
        {
            _patientRepoMock.Setup(r => r.DeletePatientAsync(15)).ReturnsAsync(true);

            var ok = await _svc.DeletePatientAsync(15);

            Assert.True(ok);
            _patientRepoMock.Verify(r => r.DeletePatientAsync(15), Times.Once);
        }

        [Fact]
        public async Task GetPatientById_ShouldReturnValue_FromRepo()
        {
            var expected = MakeValidPatient();
            expected.Id = 77;

            _patientRepoMock.Setup(r => r.GetPatientByIdAsync(77)).ReturnsAsync(expected);

            var result = await _svc.GetPatientByIdAsync(77);

            Assert.NotNull(result);
            Assert.Equal(77, result!.Id);
        }

        [Fact]
        public async Task SearchPatients_ShouldPassThrough_ToRepo()
        {
            _patientRepoMock
                .Setup(r => r.SearchPatientsAsync("NY", "Male", 20, 60, 2))
                .ReturnsAsync(new List<Patient>());

            var list = await _svc.SearchPatientsAsync("NY", "Male", 20, 60, 2);

            Assert.NotNull(list);
            _patientRepoMock.Verify(r => r.SearchPatientsAsync("NY", "Male", 20, 60, 2), Times.Once);
        }
    }
}
