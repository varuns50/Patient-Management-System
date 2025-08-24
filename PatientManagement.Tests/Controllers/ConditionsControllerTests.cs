using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using PatientManagement.API.Controllers;
using PatientManagement.API.Models;
using PatientManagement.API.Services;

namespace PatientManagement.Tests.Controllers
{
    public class ConditionsControllerTests
    {
        private readonly Mock<IConditionService> _svc = new();
        private readonly ConditionsController _controller;

        public ConditionsControllerTests()
        {
            _controller = new ConditionsController(_svc.Object);
        }

        [Fact]
        public async Task AddCondition_ReturnsCreatedAt_WhenOk()
        {
            var c = new Condition { Name = "Web-Slinging", Description = "Shoots webs" };
            _svc.Setup(s => s.AddConditionAsync(c)).ReturnsAsync(17);

            var result = await _controller.AddCondition(c);

            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created!.ActionName.Should().Be(nameof(ConditionsController.GetConditionById));
            created.RouteValues!["id"].Should().Be(17);
            _svc.Verify(s => s.AddConditionAsync(c), Times.Once);
        }

        [Fact]
        public async Task GetAllConditions_ReturnsOk_List()
        {
            var list = new List<Condition> { new Condition { Id = 1, Name = "A" }, new Condition { Id = 2, Name = "B" } };
            _svc.Setup(s => s.GetAllConditionsAsync()).ReturnsAsync(list);

            var result = await _controller.GetAllConditions();

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeSameAs(list);
        }

        [Fact]
        public async Task GetConditionById_ReturnsNotFound_WhenNull()
        {
            _svc.Setup(s => s.GetConditionByIdAsync(99)).ReturnsAsync((Condition?)null);

            var result = await _controller.GetConditionById(99);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetConditionById_ReturnsOk_WhenFound()
        {
            var item = new Condition { Id = 5, Name = "Ice Powers" };
            _svc.Setup(s => s.GetConditionByIdAsync(5)).ReturnsAsync(item);

            var result = await _controller.GetConditionById(5);

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            (ok!.Value as Condition)!.Id.Should().Be(5);
        }
    }
}
