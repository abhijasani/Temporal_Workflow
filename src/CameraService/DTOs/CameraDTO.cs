using CameraService.Models;

namespace CameraService.DTOs;

public class CameraDTO
{
    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public int HttpPort { get; set; } = 80;

    public int RtspPort { get; set; } = 554;

    public string Protocol { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public Camera ToCamera(Guid cameraId) 
    {
        return new()
        {
            Id = cameraId,
            Name = Name,
            Address = Address,
            HttpPort = HttpPort,
            RtspPort = RtspPort,
            Protocol = Protocol,
            Version = Version
        };
    }
}
