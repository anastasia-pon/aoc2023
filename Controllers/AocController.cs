using aoc2023.Services;
using Microsoft.AspNetCore.Mvc;

namespace aoc2023.Controllers;

[ApiController]
[Route("[controller]")]
public class AocController : ControllerBase
{
    private readonly IInputService _inputService;
    private readonly ISolutionService _solutionService;

    public AocController(IInputService inputService, ISolutionService solutionService)
    {
        _inputService = inputService;
        _solutionService = solutionService;
    }
    
    [HttpGet("{day}/{part}")]
    public async Task<IActionResult> Get(int day, int part)
    {
        var input = await _inputService.GetInput(day);

        if (input == null) return BadRequest($"Could not get input for day {day}");
        
        switch (day)
        {
            case 1: return Ok(_solutionService.GetSolutionDay1(part, input));
            case 2: return Ok(_solutionService.GetSolutionDay2(part, input));
            default: return BadRequest($"There is no solution for day {day}");
        }
    }

}