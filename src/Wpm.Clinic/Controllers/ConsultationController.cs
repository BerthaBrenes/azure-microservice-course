using Microsoft.AspNetCore.Mvc;
using Wpm.Clinic.Aplication;

namespace Wpm.Clinic.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ConsultationController(ClinicApplicationService applicationService, ILogger<ConsultationController> logger) : ControllerBase
{
    [HttpPost("/start")]
    public async Task<IActionResult> Start(StartConsultationCommand command)
    {
        var result = await applicationService.Handle(command);
        if (result == null) {
            logger.LogWarning("Consultation not started");
            return NotFound();
        }
        return Ok(result);
    }


}
public record StartConsultationCommand(int PatiendID);
