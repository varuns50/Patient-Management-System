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
    public class PatientsControllerTests
    {
        private readonly Mock<IPatientService> _svc = new();
        private readonly PatientsController _controller;

        public PatientsControllerTests()
        {
            _controller = new PatientsController(_svc.Object);
        }

        private static Patient MakePatient(int id = 0) => new Patient
        {
            Id = id,
            FirstName = "Tony",
            LastName = "Stark",
            DOB = DateOnly.FromDateTime(new System.DateTime(1970, 5, 29)),
            Gender = "Male",
            City = "NY",
            Email = "tony@stark.com",
            Phone = "1234567890"
        };

        [Fact]
        public async Task AddPatient_ReturnsCreatedAt_WhenOk()
        {
            // Arrange
            var p = MakePatient();
            _svc.Setup(s => s.AddPatientAsync(p, It.IsAny<List<int>>())).ReturnsAsync(42);

            // Act
            var result = await _controller.AddPatient(p, new List<int>());

            // Assert
            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created!.ActionName.Should().Be(nameof(PatientsController.GetPatientById));
            created.RouteValues!["id"].Should().Be(42);
            _svc.Verify(s => s.AddPatientAsync(p, It.IsAny<List<int>>()), Times.Once);
        }

        [Fact]
        public async Task GetPatientById_ReturnsOk_WhenFound()
        {
            var p = MakePatient(7);
            _svc.Setup(s => s.GetPatientByIdAsync(7)).ReturnsAsync(p);

            var result = await _controller.GetPatientById(7);

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            (ok!.Value as Patient)!.Id.Should().Be(7);
        }

        [Fact]
        public async Task GetPatientById_ReturnsNotFound_WhenNull()
        {
            _svc.Setup(s => s.GetPatientByIdAsync(99)).ReturnsAsync((Patient?)null);

            var result = await _controller.GetPatientById(99);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task SearchPatients_ReturnsOk_WithList()
        {
            _svc.Setup(s => s.SearchPatientsAsync("NY", "Male", 20, 60, 2)).ReturnsAsync(new List<Patient>());

            var result = await _controller.SearchPatients("NY", "Male", 20, 60, 2);
            result.Should().BeOfType<OkObjectResult>();
            _svc.Verify(s => s.SearchPatientsAsync("NY", "Male", 20, 60, 2), Times.Once);
        }
    }
}
