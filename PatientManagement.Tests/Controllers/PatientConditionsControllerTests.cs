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
    public class PatientConditionsControllerTests
    {
        private readonly Mock<IPatientConditionService> _svc = new();
        private readonly PatientConditionsController _controller;

        public PatientConditionsControllerTests()
        {
            _controller = new PatientConditionsController(_svc.Object);
        }

        [Fact]
        public async Task AddPatientCondition_ReturnsOk_OnSuccess()
        {
            var pc = new PatientCondition { PatientId = 42, ConditionId = 3, DiagnosedDate = DateOnly.FromDateTime(System.DateTime.UtcNow) };
            _svc.Setup(s => s.AddPatientConditionAsync(pc)).ReturnsAsync(true);

            var result = await _controller.AddPatientCondition(pc);

            result.Should().BeOfType<OkObjectResult>();
            _svc.Verify(s => s.AddPatientConditionAsync(pc), Times.Once);
        }

        [Fact]
        public async Task RemovePatientCondition_ReturnsOk_OnSuccess()
        {
            _svc.Setup(s => s.RemovePatientConditionAsync(42, 3)).ReturnsAsync(true);

            var result = await _controller.RemovePatientCondition(42, 3);

            result.Should().BeOfType<OkObjectResult>();
            _svc.Verify(s => s.RemovePatientConditionAsync(42, 3), Times.Once);
        }

        [Fact]
        public async Task GetConditionsByPatientId_ReturnsOk_List()
        {
            var list = new List<Condition> { new Condition { Id = 1, Name = "A" }, new Condition { Id = 2, Name = "B" } };
            _svc.Setup(s => s.GetConditionsByPatientIdAsync(7)).ReturnsAsync(list);

            var result = await _controller.GetConditionsByPatientId(7);

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeSameAs(list);
        }
    }
}
