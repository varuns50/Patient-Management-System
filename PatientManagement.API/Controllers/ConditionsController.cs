using PatientManagement.API.Models;
using PatientManagement.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PatientManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConditionsController : ControllerBase
{
    private readonly IConditionService _conditionService;

    public ConditionsController(IConditionService conditionService)
    {
        _conditionService = conditionService;
    }

    // POST: api/conditions
    [HttpPost]
    public async Task<IActionResult> AddCondition([FromBody] Condition condition)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var id = await _conditionService.AddConditionAsync(condition);
        return CreatedAtAction(nameof(GetConditionById), new { id }, condition);
    }

    // GET: api/conditions
    [HttpGet]
    public async Task<IActionResult> GetAllConditions()
    {
        var conditions = await _conditionService.GetAllConditionsAsync();
        return Ok(conditions);
    }

    // GET: api/conditions/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConditionById(int id)
    {
        var condition = await _conditionService.GetConditionByIdAsync(id);
        return condition is null ? NotFound() : Ok(condition);
    }
}
