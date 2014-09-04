using System;
using System.Collections;
using System.Globalization;

using DotNetNuke.Collections;
using DotNetNuke.Common;
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.UI.Modules;

namespace DotNetNuke.SettingsRepository
{
    /// <summary>An <see cref="ISettingsRepository"/> implementation which uses a <see cref="ModuleInfo"/> instance for context</summary>
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ModuleInstanceContext moduleContext;

        /// <summary>Initializes a new instance of the <see cref="SettingsRepository"/> class.</summary>
        /// <param name="moduleContext">The module context.</param>
        public SettingsRepository(ModuleInstanceContext moduleContext)
        {
            this.moduleContext = moduleContext;
        }

        /// <summary>Determines whether the specified setting has any value.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <returns><c>true</c> if the specified setting has a value; otherwise, <c>false</c>.</returns>
        public bool HasValue<T>(Setting<T> setting)
        {
            Requires.PropertyNotNullOrEmpty("setting", "SettingName", setting.SettingName);
            return this.GetSettings(setting.Scope).Contains(setting.SettingName);
        }

        /// <summary>Gets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <returns>The setting's value (returns the default value of the setting if it hasn't been set yet)</returns>
        public T GetValue<T>(Setting<T> setting)
        {
            Requires.PropertyNotNullOrEmpty("setting", "SettingName", setting.SettingName);
            return this.GetSettings(setting.Scope).GetValueOrDefault(setting.SettingName, setting.DefaultValue);
        }

        /// <summary>Gets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <param name="converter">A function which converts the string representation of the value into <typeparamref name="T" />.</param>
        /// <returns>The setting's value (returns the default value of the setting if it hasn't been set yet)</returns>
        public T GetValue<T>(Setting<T> setting, Func<string, T> converter)
        {
            Requires.PropertyNotNullOrEmpty("setting", "SettingName", setting.SettingName);
            return this.GetSettings(setting.Scope).GetValueOrDefault(setting.SettingName, setting.DefaultValue, converter);
        }

        /// <summary>Gets the settings collection for the given <paramref name="settingScope"/>.</summary>
        /// <param name="settingScope">The scope of settings to get.</param>
        /// <returns>A <see cref="IDictionary"/> instance mapping between setting names and setting values as (both as <see cref="string"/> values).</returns>
        /// <exception cref="InvalidOperationException"><paramref name="settingScope"/> was an invalid <see cref="SettingScope"/> value</exception>
        private IDictionary GetSettings(SettingScope settingScope)
        {
            switch (settingScope)
            {
                case SettingScope.TabModule:
                    return this.moduleContext.Configuration.TabModuleSettings;
                case SettingScope.Module:
                    return this.moduleContext.Configuration.ModuleSettings;
                case SettingScope.Host:
                    return HostController.Instance.GetSettingsDictionary();
                case SettingScope.Portal:
                    return PortalController.GetPortalSettingsDictionary(this.moduleContext.PortalId);
                case SettingScope.Tab:
                    return this.moduleContext.PortalSettings.ActiveTab.TabSettings;
                default:
                    throw new InvalidOperationException("Invalid SettingScope");
            }
        }
    }
}