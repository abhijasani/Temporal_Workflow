using AnalyticsService.DTOs;
using AnalyticsService.Models;

namespace AnalyticsService;

public class IVASettingsService
{
       private readonly List<IVASettings> _ivaSettingsList = [];

        public List<IVASettings> GetAllSettings()
        {
            return _ivaSettingsList.ToList();
        }

        public IVASettings? GetSettingsById(Guid id)
        {
            return _ivaSettingsList.FirstOrDefault(s => s.Id == id);
        }

        public IVASettings CreateSettings(IVASettingsDTO settings)
        {
            var settingsId = Guid.NewGuid();
            IVASettings newSettings = settings.ToIVASettings(settingsId);
            
            _ivaSettingsList.Add(newSettings);
            return newSettings;
        }

        public IVASettings? UpdateSettings(Guid settingsId, IVASettingsDTO settings)
        {
            var existingSettings = GetSettingsById(settingsId);
            if (existingSettings == null)
            {
                return null;
            }

            existingSettings.CameraId = settings.CameraId;
            existingSettings.AnalyticsType = settings.AnalyticsType;
            existingSettings.ActionTypes = settings.ActionTypes;

            return existingSettings;
        }

        public bool DeleteSettings(Guid id)
        {
            var settings = GetSettingsById(id);
            if (settings == null)
            {
                return false;
            }

            return _ivaSettingsList.Remove(settings);
        }
}
