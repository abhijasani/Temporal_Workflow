using System.ComponentModel.DataAnnotations;

namespace CameraService.Models;

public class Camera
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Device name is required.")]
    [StringLength(100, ErrorMessage = "Device name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Connection address is required.")]
    [StringLength(255, ErrorMessage = "Connection address cannot exceed 255 characters.")]
    public string Address { get; set; } = string.Empty;


    [Required(ErrorMessage = "HTTP port is required.")]
    [Range(1, 65535, ErrorMessage = "HTTP port must be between 1 and 65535.")]
    public int HttpPort { get; set; } = 80;

    [Required(ErrorMessage = "RTSP port is required.")]
    [Range(1, 65535, ErrorMessage = "RTSP port must be between 1 and 65535.")]
    public int RtspPort { get; set; } = 554;

    [StringLength(50, ErrorMessage = "Protocol cannot exceed 50 characters.")]
    public string Protocol { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Version cannot exceed 50 characters.")]
    public string Version { get; set; } = string.Empty;
}
