using Microsoft.AspNetCore.Mvc;

namespace VehicleManagementService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleManagementController : ControllerBase
{
    public VehicleManagementController()
    {
    }
    
    [HttpGet]
    public ActionResult<bool> ValidateNumberPlate(string numberPlate)
    {
        var random = new Random();
        return Ok(random.Next() % 2 == 0);
    }
}