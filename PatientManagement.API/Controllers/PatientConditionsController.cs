using PatientManagement.API.Models;
using PatientManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PatientManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientConditionsController : ControllerBase
{
    private readonly IPatientConditionService _pcService;

    public PatientConditionsController(IPatientConditionService pcService)
    {
        _pcService = pcService;
    }

    // POST: api/patientconditions
    [HttpPost]
    public async Task<IActionResult> AddPatientCondition([FromBody] PatientCondition pc)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _pcService.AddPatientConditionAsync(pc);
        return success ? Ok("Condition added to patient.") : BadRequest("Failed to add.");
    }

    // DELETE: api/patientconditions/{patientId}/{conditionId}
    [HttpDelete("{patientId}/{conditionId}")]
    public async Task<IActionResult> RemovePatientCondition(int patientId, int conditionId)
    {
        var success = await _pcService.RemovePatientConditionAsync(patientId, conditionId);
        return success ? Ok("Condition removed.") : NotFound();
    }

    // GET: api/patientconditions/{patientId}
    [HttpGet("{patientId}")]
    public async Task<IActionResult> GetConditionsByPatientId(int patientId)
    {
        var conditions = await _pcService.GetConditionsByPatientIdAsync(patientId);
        return Ok(conditions);
    }
}
