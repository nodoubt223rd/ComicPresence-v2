using System.Collections.Generic;

namespace ComicPresence.Common.Config
{
    /// <summary>
    /// Full application settings data
    /// </summary>
    public class AppSettingsData
    {
        public IList<BaseAppSetting> BaseAppSettings { get; set; }
        public IList<ApplicationAppSettingOverride> ApplicationOverrideAppSettings { get; set; }
    }
}
