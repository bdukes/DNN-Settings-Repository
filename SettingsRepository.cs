// <copyright file="SettingsRepository.cs" company="Engage Software">
// DotNetNuke.SettingsRepository
// Copyright (c) 2004-2014
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
namespace DotNetNuke.SettingsRepository
{
    using System;
    using System.Collections;

    using DotNetNuke.Collections;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Controllers;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.UI.Modules;

    public class SettingsRepository : ISettingsRepository
    {
        private readonly ModuleInstanceContext moduleContext;

        public SettingsRepository(ModuleInstanceContext moduleContext)
        {
            this.moduleContext = moduleContext;
        }

        public T GetValue<T>(Setting<T> setting)
        {
            Requires.PropertyNotNullOrEmpty("setting", "SettingName", setting.SettingName);
            return this.GetSettings(setting.Scope).GetValueOrDefault(setting.SettingName, setting.DefaultValue);
        }

        public T GetValue<T>(Setting<T> setting, Func<string, T> converter)
        {
            Requires.PropertyNotNullOrEmpty("setting", "SettingName", setting.SettingName);
            return this.GetSettings(setting.Scope).GetValueOrDefault(setting.SettingName, setting.DefaultValue, converter);
        }

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