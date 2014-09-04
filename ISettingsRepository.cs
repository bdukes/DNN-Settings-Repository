// <copyright file="ISettingsRepository.cs" company="Engage Software">
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

    /// <summary>A contract specifying the ability to get and set settings</summary>
    public interface ISettingsRepository
    {
        /// <summary>Gets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <returns>The setting's value (returns the default value of the setting if it hasn't been set yet)</returns>
        T GetValue<T>(Setting<T> setting);

        /// <summary>Gets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <param name="converter">A function which converts the string representation of the value into <typeparamref name="T"/>.</param>
        /// <returns>The setting's value (returns the default value of the setting if it hasn't been set yet)</returns>
        T GetValue<T>(Setting<T> setting, Func<string, T> converter);
    }
}