using AnalyticsService.DTOs;
using AnalyticsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AnalyticsController : ControllerBase
{
    private readonly IVASettingsService _ivaSettingsService;

    public AnalyticsController(IVASettingsService ivaSettingsService)
    {
        _ivaSettingsService = ivaSettingsService;
    }

    [HttpGet]
    public ActionResult<List<IVASettings>> GetAllSettings()
    {
        var settingsList = _ivaSettingsService.GetAllSettings();
        if (settingsList == null || settingsList.Count == 0)
        {
            return NotFound("No settings found.");
        }
        return Ok(settingsList);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<IVASettings> GetSettingsById(Guid id)
    {
        var settings = _ivaSettingsService.GetSettingsById(id);
        if (settings == null)
        {
            return NotFound($"Settings with ID {id} not found.");
        }
        return Ok(settings);
    }

    [HttpPost]
    public ActionResult<IVASettings> CreateSettings(IVASettingsDTO settingsDto)
    {
        var newSettings = _ivaSettingsService.CreateSettings(settingsDto);
        return CreatedAtAction(nameof(GetSettingsById), new { id = newSettings.Id }, newSettings);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<IVASettings> UpdateSettings(Guid id, IVASettingsDTO settingsDto)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid Camera ID.");
        }
        
        var updatedSettings = _ivaSettingsService.UpdateSettings(id, settingsDto);
        if (updatedSettings == null)
        {
            return NotFound($"Settings with ID {id} not found.");
        }

        return CreatedAtAction(nameof(GetSettingsById), new { id = id }, updatedSettings);
    }

    [HttpDelete("{id:guid}")]
    public ActionResult DeleteSettings(Guid id)
    {
        var success = _ivaSettingsService.DeleteSettings(id);
        if (!success)
        {
            return NotFound($"Settings with ID {id} not found.");
        }
        return NoContent();
    }
}
