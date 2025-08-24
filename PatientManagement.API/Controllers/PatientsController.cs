using PatientManagement.API.Models;
using PatientManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PatientManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    // POST: api/patients
    [HttpPost]
    public async Task<IActionResult> AddPatient([FromBody] Patient patient, [FromQuery] List<int>? conditionIds)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newId = await _patientService.AddPatientAsync(patient, conditionIds);
        return CreatedAtAction(nameof(GetPatientById), new { id = newId }, patient);
    }

    // PUT: api/patients/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient patient)
    {
        if (id != patient.Id) return BadRequest("Id mismatch.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _patientService.UpdatePatientAsync(patient);
        return success ? Ok("Patient updated.") : NotFound();
    }

    // DELETE: api/patients/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var success = await _patientService.DeletePatientAsync(id);
        return success ? Ok("Patient deleted.") : NotFound();
    }

    // GET: api/patients/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        var patient = await _patientService.GetPatientByIdAsync(id);
        return patient is null ? NotFound() : Ok(patient);
    }

    // GET: api/patients/search
    [HttpGet("search")]
    public async Task<IActionResult> SearchPatients([FromQuery] string? city, [FromQuery] string? gender,
        [FromQuery] int? minAge, [FromQuery] int? maxAge, [FromQuery] int? conditionId)
    {
        var patients = await _patientService.SearchPatientsAsync(city, gender, minAge, maxAge, conditionId);
        return Ok(patients);
    }
}
