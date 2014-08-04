using System;

namespace DotNetNuke.SettingsRepository
{
    public struct Setting<T>
    {
        private readonly string _settingName;

        private readonly SettingScope _scope;

        private readonly T _defaultValue;

        public Setting(string settingName, SettingScope scope, T defaultValue)
            : this()
        {
            this._settingName = settingName;
            this._scope = scope;
            this._defaultValue = defaultValue;
        }

        public T DefaultValue
        {
            get { return this._defaultValue; }
        }

        public string SettingName
        {
            get { return this._settingName; }
        }

        public SettingScope Scope
        {
            get { return this._scope; }
        }
    }
}
