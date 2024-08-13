using System.ComponentModel.DataAnnotations;
using AnalyticsService.Enums;

namespace AnalyticsService.Models;

public class IVASettings
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CameraId {get; set;}

    [Required]
    public AnalyticsType AnalyticsType {get; set;}

    public List<ActionType> ActionTypes {get; set;} = [];
}
