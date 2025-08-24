using Moq;
using PatientManagement.API.Models;
using PatientManagement.API.Repository;
using PatientManagement.API.Services;

namespace PatientManagement.Tests.Services
{
    public class PatientConditionServiceTests
    {
        private readonly Mock<IPatientConditionRepository> _repo = new();
        private readonly IPatientConditionService _svc;

        public PatientConditionServiceTests()
        {
            _svc = new PatientConditionService(_repo.Object);
        }

        [Fact]
        public async Task AddPatientCondition_ShouldReturnTrue_AndCallRepo()
        {
            var pc = new PatientCondition { PatientId = 42, ConditionId = 3, DiagnosedDate = DateOnly.FromDateTime(DateTime.UtcNow) };

            _repo.Setup(r => r.AddPatientConditionAsync(pc)).ReturnsAsync(true);

            var ok = await _svc.AddPatientConditionAsync(pc);

            Assert.True(ok);
            _repo.Verify(r => r.AddPatientConditionAsync(pc), Times.Once);
        }

        [Fact]
        public async Task RemovePatientCondition_ShouldReturnTrue_AndCallRepo()
        {
            _repo.Setup(r => r.RemovePatientConditionAsync(42, 3)).ReturnsAsync(true);

            var ok = await _svc.RemovePatientConditionAsync(42, 3);

            Assert.True(ok);
            _repo.Verify(r => r.RemovePatientConditionAsync(42, 3), Times.Once);
        }

        [Fact]
        public async Task GetConditionsByPatientId_ShouldReturnList_FromRepo()
        {
            var list = new List<Condition>
            {
                new Condition { Id = 1, Name = "Web-Slinging" },
                new Condition { Id = 4, Name = "Ice Powers" }
            };

            _repo.Setup(r => r.GetConditionsByPatientIdAsync(7)).ReturnsAsync(list);

            var result = await _svc.GetConditionsByPatientIdAsync(7);

            Assert.Same(list, result);
            _repo.Verify(r => r.GetConditionsByPatientIdAsync(7), Times.Once);
        }
    }
}
