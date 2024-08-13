using AnalyticsService.Enums;
using AnalyticsService.Models;

namespace AnalyticsService.DTOs;

public class IVASettingsDTO
{
    public Guid CameraId {get; set;}

    public AnalyticsType AnalyticsType {get; set;}

    public List<ActionType> ActionTypes {get; set;} = [];

    public IVASettings ToIVASettings(Guid settingsId)
    {
        return new IVASettings()
        {
            Id = settingsId,
            CameraId = CameraId,
            AnalyticsType = AnalyticsType,
            ActionTypes = ActionTypes,
        };
    }
}
