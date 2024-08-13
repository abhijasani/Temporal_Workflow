using CameraService.DTOs;
using CameraService.Models;
using CameraService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CameraService.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CameraController : ControllerBase
{
    private readonly CameraConfigurationService _cameraService;

    public CameraController(CameraConfigurationService cameraService)
    {
        _cameraService = cameraService;
    }

    [HttpGet]
    public ActionResult<List<Camera>> GetAllCameras()
    {
        var cameras = _cameraService.GetAllCameras();
        if (cameras == null || cameras.Count == 0)
        {
            return NotFound("No cameras found.");
        }
        return Ok(cameras);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Camera> GetCameraById(Guid id)
    {
        var camera = _cameraService.GetCameraById(id);
        if (camera == null)
        {
            return NotFound($"Camera with ID {id} not found.");
        }
        return Ok(camera);
    }

    [HttpPost]
    public ActionResult<Camera> CreateCamera(CameraDTO cameraDto)
    {
        var newCamera = _cameraService.CreateCamera(cameraDto);
        return CreatedAtAction(nameof(GetCameraById), new { id = newCamera.Id }, newCamera);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<Camera> UpdateCamera(Guid id, CameraDTO camera)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid Camera ID.");
        }

        var updatedCamera = _cameraService.UpdateCamera(id, camera);
        if (updatedCamera == null)
        {
            return NotFound($"Camera with ID {id} not found.");
        }

        return CreatedAtAction(nameof(GetCameraById), new { id = id }, updatedCamera);
    }

    [HttpDelete("{id:guid}")]
    public ActionResult DeleteCamera(Guid id)
    {
        var success = _cameraService.DeleteCamera(id);
        if (!success)
        {
            return NotFound($"Camera with ID {id} not found.");
        }

        return NoContent();
    }
}
