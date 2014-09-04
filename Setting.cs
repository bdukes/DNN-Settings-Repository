using System;
using DotNetNuke.Common;

namespace DotNetNuke.SettingsRepository
{
    /// <summary>The definition of a setting</summary>
    /// <typeparam name="T">The type of the setting's value</typeparam>
    public struct Setting<T> : IEquatable<Setting<T>>
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
            Requires.NotNull("settingName", settingName);

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

        /// <summary>Implements the <c>==</c> operator.</summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>A value indicating whether <paramref name="left"/> equals <paramref name="right"/>.</returns>
        public static bool operator ==(Setting<T> left, Setting<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>Implements the <c>!=</c> operator.</summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>A value indicating whether <paramref name="left"/> doesn't equal <paramref name="right"/>.</returns>
        public static bool operator !=(Setting<T> left, Setting<T> right)
        {
            return !left.Equals(right);
        }

        /// <summary>Indicates whether the current object is equal to another <see cref="Setting{T}"/> instance.</summary>
        /// <param name="other">A <see cref="Setting{T}"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the current instance is equal to the <paramref name="other" /> instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Setting<T> other)
        {
            return string.Equals(this._settingName, other._settingName) && this._scope == other._scope;
        }

        /// <summary>Indicates whether the current object is equal to another <see cref="object"/>.</summary>
        /// <param name="obj">An <see cref="object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the current instance is equal to the <paramref name="obj" /> instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Setting<T> && this.Equals((Setting<T>)obj);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((this._settingName != null ? this._settingName.GetHashCode() : 0) * 397) ^ (int)this._scope;
            }
        }
    }
}
