using System;
using System.Collections.Generic;
using System.Linq;

using ComicPresence.Common.Caching;
using ComicPresence.Common.Data;
using ComicPresence.Common.Patterns;

namespace ComicPresence.Common.Config
{
    /// <summary>
    /// Caches using CacheManager.LocalInstance
    /// </summary>
    public class AppSettingsMgr : Singleton<AppSettingsMgr>
    {
        private bool Configured { get; set; }
        public ApplicationId ApplicationId { get; protected set; }

        private AppSettingsMgr()
        {
        }

        public void Configure(ApplicationId applicationId)
        {
            if (Configured)
                throw new InvalidOperationException("Already configured");
            ApplicationId = applicationId;
            Configured = true;
        }

        public string GetString(AppSettingId id)
        {
            CheckConfigured();
            return GetValue<string>(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Must be a type supported by Convert.ChangeType</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetValue<T>(AppSettingId id)
        {
            CheckConfigured();
            return GetAppSettings()[id].GetValue<T>();
        }

        public AppSetting GetAppSetting(AppSettingId id)
        {
            CheckConfigured();
            return GetAppSettings()[id];
        }

        public void ClearCache()
        {
            CheckConfigured();
            CacheManager.LocalInstance.Remove(CacheRegion.AppSettings, null);
        }

        /// <summary>
        /// Updates an app setting and clears the cache
        /// </summary>
        /// <param name="appSettingId"></param>
        /// <param name="applicationId"></param>
        /// <param name="value"></param>
        public void UpdateAppSetting(AppSettingId appSettingId, ApplicationId? applicationId, string value)
        {
            CheckConfigured();
            SqlOrm.AppInstance.ExecuteProc("[dbo].[cp_UpdateAppSetting]",
                new { applicationId = applicationId, appSettingId = appSettingId, value = value }, logPerf: false);
            ClearCache();
        }

        public Dictionary<AppSettingId, AppSetting> GetAppSettings()
        {
            CheckConfigured();
            // don't log performance stats - in order to break cycle/infinite loop
            return CacheManager.LocalInstance.Get(CacheRegion.AppSettings,
                null,
                CacheSettings.cDefaultNonUserExpiration,
                () => SqlOrm.AppInstance.QueryProc<AppSetting>("[dbo].[cp_GetAppSettings]",
                    new { applicationId = ApplicationId }, logPerf: false).ToDictionary<AppSetting, AppSettingId>(appSetting => appSetting.Id));
        }

        public AppSettingsData GetFullAppSettings()
        {
            CheckConfigured();
            AppSettingsData data = CacheManager.LocalInstance.Get<AppSettingsData>(CacheRegion.AppSettings, null);

            if (data == null)
            {
                data = new AppSettingsData();

                data.BaseAppSettings = SqlOrm.AppInstance.QueryProc<BaseAppSetting>(
                    "[dbo].[cp_GetBaseAppSettings]", logPerf: false);

                data.ApplicationOverrideAppSettings = SqlOrm.AppInstance.QueryProc<ApplicationAppSettingOverride>(
                    "[dbo].[cp_GetApplicationOverrideAppSettings]", logPerf: false);

                CacheManager.LocalInstance.Put(CacheRegion.AppSettings, null, data, CacheSettings.cDefaultNonUserExpiration);
            }

            return data;
        }

        protected void CheckConfigured()
        {
            if (!Configured)
            {
                throw new InvalidOperationException("AppSettingsMgr instance has not been configured");
            }
        }
    }
}
