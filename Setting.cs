using System;

namespace DotNetNuke.SettingsRepository
{
    /// <summary>The definition of a setting</summary>
    /// <typeparam name="T">The type of the setting's value</typeparam>
    public struct Setting<T>
    {
        /// <summary>Backing field for <see cref="SettingName"/></summary>
        private readonly string _settingName;

        /// <summary>Backing field for <see cref="Scope"/></summary>
        private readonly SettingScope _scope;

        /// <summary>Backing field for <see cref="DefaultValue"/></summary>
        private readonly T _defaultValue;

        /// <summary>Initializes a new instance of the <see cref="Setting{T}"/> struct.</summary>
        /// <param name="settingName">The name of the setting. This name needs to be sufficiently unique when using a <paramref name="scope"/> which isn't owned by the module (e.g. <see cref="SettingScope.Tab"/>, <see cref="SettingScope.Portal"/>, or <see cref="SettingScope.Host"/>)</param>
        /// <param name="scope">The scope of the setting.</param>
        /// <param name="defaultValue">The default value of the setting to use when the setting has not yet been set.</param>
        public Setting(string settingName, SettingScope scope, T defaultValue)
            : this()
        {
            this._settingName = settingName;
            this._scope = scope;
            this._defaultValue = defaultValue;
        }

        /// <summary>Gets the default value to use if the setting does not have a value set.</summary>
        public T DefaultValue
        {
            get { return this._defaultValue; }
        }

        /// <summary>Gets the name of the setting. This name needs to be sufficiently unique when using a <see cref="Scope"/> which isn't owned by the module (e.g. <see cref="SettingScope.Tab"/>, <see cref="SettingScope.Portal"/>, or <see cref="SettingScope.Host"/>)</summary>
        public string SettingName
        {
            get { return this._settingName; }
        }

        /// <summary>Gets the scope of the setting.</summary>
        public SettingScope Scope
        {
            get { return this._scope; }
        }
    }
}
