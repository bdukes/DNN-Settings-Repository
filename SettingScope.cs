using System;

namespace DotNetNuke.SettingsRepository
{
    /// <summary>The scope of a <see cref="Setting{T}"/></summary>
    public enum SettingScope
    {
        /// <summary>A setting that applied to an instance of a module on a particular page</summary>
        TabModule = 0,

        /// <summary>A setting that applies to a module instance (including on any page to which a reference of it is shared)</summary>
        Module = 1,

        /// <summary>A setting that applies to a page</summary>
        Tab = 2,

        /// <summary>A setting that applies to a site</summary>
        Portal = 3,

        /// <summary>A setting that applies to the entire DNN installation</summary>
        Host = 4,
    }
}
