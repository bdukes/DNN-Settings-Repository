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
        /// <summary>The module info</summary>
        private readonly ModuleInfo _moduleInfo;

        /// <summary>Initializes a new instance of the <see cref="SettingsRepository"/> class.</summary>
        /// <param name="moduleContext">The module context.</param>
        public SettingsRepository(ModuleInstanceContext moduleContext)
        {
            this._moduleInfo = moduleContext.Configuration;
        }

        /// <summary>Initializes a new instance of the <see cref="SettingsRepository"/> class.</summary>
        /// <param name="moduleInfo">The module info.</param>
        public SettingsRepository(ModuleInfo moduleInfo)
        {
            this._moduleInfo = moduleInfo;
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

        /// <summary>Sets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <param name="value">The value.</param>
        public void SetValue<T>(Setting<T> setting, T value)
        {
            this.SetValue(setting, value, DefaultStringifier);
        }

        /// <summary>Sets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <param name="value">The value.</param>
        /// <param name="converter">A function which converts the value into its string representation.</param>
        public void SetValue<T>(Setting<T> setting, T value, Func<T, string> converter)
        {
            var convertedValue = converter(value);
            var setSettingValue = this.GetSetter(setting);
            setSettingValue(convertedValue);
        }

        /// <summary>The default conversion algorithm from a value to a <see cref="string"/> (for storing a setting).</summary>
        /// <typeparam name="T">The type of the value to convert</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>A <see cref="string"/> representation of the <paramref name="value"/></returns>
        private static string DefaultStringifier<T>(T value)
        {
            if (ReferenceEquals(value, null))
            {
                return null;
            }

            var convertibleValue = value as IConvertible;
            if (convertibleValue != null)
            {
                return convertibleValue.ToString(CultureInfo.InvariantCulture);
            }

            return value.ToString();
        }

        /// <summary>Gets a function which will update the setting's value.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <returns>A function which takes the <see cref="string"/> value to which to update the setting's value</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="setting"/> has a <see cref="Setting{T}.Scope"/> which is an invalid <see cref="SettingScope"/> value</exception>
        private Action<string> GetSetter<T>(Setting<T> setting)
        {
            switch (setting.Scope)
            {
                case SettingScope.TabModule:
                    return value => new ModuleController().UpdateTabModuleSetting(this._moduleInfo.TabModuleID, setting.SettingName, value);
                case SettingScope.Module:
                    return value => new ModuleController().UpdateModuleSetting(this._moduleInfo.ModuleID, setting.SettingName, value);
                case SettingScope.Tab:
                    return value => new TabController().UpdateTabSetting(this._moduleInfo.TabID, setting.SettingName, value);
                case SettingScope.Portal:
                    return value => PortalController.UpdatePortalSetting(this._moduleInfo.PortalID, setting.SettingName, value);
                case SettingScope.Host:
                    return value => HostController.Instance.Update(setting.SettingName, value);
                default:
                    throw new InvalidOperationException("Invalid SettingScope");
            }
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
                    return this._moduleInfo.TabModuleSettings;
                case SettingScope.Module:
                    return this._moduleInfo.ModuleSettings;
                case SettingScope.Tab:
                    return this._moduleInfo.ParentTab.TabSettings;
                case SettingScope.Portal:
                    return PortalController.GetPortalSettingsDictionary(this._moduleInfo.PortalID);
                case SettingScope.Host:
                    return HostController.Instance.GetSettingsDictionary();
                default:
                    throw new InvalidOperationException("Invalid SettingScope");
            }
        }
    }
}