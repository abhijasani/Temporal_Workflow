using CameraService.DTOs;
using CameraService.Models;

namespace CameraService.Services;

public class CameraConfigurationService
{
    private readonly List<Camera> _cameras = [];

    public List<Camera>? GetAllCameras()
    {
        return _cameras.ToList();
    }

    public Camera? GetCameraById(Guid id)
    {
        return _cameras.FirstOrDefault(c => c.Id == id);
    }

    public Camera CreateCamera(CameraDTO camera)
    {
        var cameraId = Guid.NewGuid();
        Camera newCamera = camera.ToCamera(cameraId);

        _cameras.Add(newCamera);
        return newCamera;
    }

    public Camera? UpdateCamera(Guid camerId, CameraDTO camera)
    {
        var existingCamera = GetCameraById(camerId);
        
        if (existingCamera == null)
        {
            return null;
        }

        existingCamera.Name = camera.Name;
        existingCamera.Address = camera.Address;
        existingCamera.HttpPort = camera.HttpPort;
        existingCamera.RtspPort = camera.RtspPort;
        existingCamera.Protocol = camera.Protocol;
        existingCamera.Version = camera.Version;

        return existingCamera;
    }

    public bool DeleteCamera(Guid id)
    {
        var camera = GetCameraById(id);
        if (camera == null)
        {
            return false;
        }

        return _cameras.Remove(camera);
    }
}
