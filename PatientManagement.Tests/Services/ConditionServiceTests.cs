using Moq;
using PatientManagement.API.Models;
using PatientManagement.API.Repository;
using PatientManagement.API.Services;

namespace PatientManagement.Tests.Services
{
    public class ConditionServiceTests
    {
        private readonly Mock<IConditionRepository> _repo = new();
        private readonly IConditionService _svc;

        public ConditionServiceTests()
        {
            _svc = new ConditionService(_repo.Object);
        }

        [Fact]
        public async Task AddCondition_ShouldReturnNewId_AndCallRepo()
        {
            var c = new Condition { Name = "Super Strength", Description = "Boom" };

            _repo.Setup(r => r.AddConditionAsync(c)).ReturnsAsync(7);

            var id = await _svc.AddConditionAsync(c);

            Assert.Equal(7, id);
            _repo.Verify(r => r.AddConditionAsync(c), Times.Once);
        }

        [Fact]
        public async Task GetAllConditions_ShouldReturnList_FromRepo()
        {
            var list = new List<Condition>
            {
                new Condition { Id = 1, Name = "A" },
                new Condition { Id = 2, Name = "B" }
            };

            _repo.Setup(r => r.GetAllConditionsAsync()).ReturnsAsync(list);

            var result = await _svc.GetAllConditionsAsync();

            Assert.Same(list, result);
            _repo.Verify(r => r.GetAllConditionsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetConditionById_ShouldReturnNull_WhenRepoReturnsNull()
        {
            _repo.Setup(r => r.GetConditionByIdAsync(99)).ReturnsAsync((Condition?)null);

            var result = await _svc.GetConditionByIdAsync(99);

            Assert.Null(result);
            _repo.Verify(r => r.GetConditionByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task GetConditionById_ShouldReturnValue_WhenExists()
        {
            var item = new Condition { Id = 5, Name = "Detective Skills" };
            _repo.Setup(r => r.GetConditionByIdAsync(5)).ReturnsAsync(item);

            var result = await _svc.GetConditionByIdAsync(5);

            Assert.NotNull(result);
            Assert.Equal(5, result!.Id);
        }
    }
}
