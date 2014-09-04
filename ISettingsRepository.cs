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
        /// <summary>Determines whether the specified setting has any value.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <returns><c>true</c> if the specified setting has a value; otherwise, <c>false</c>.</returns>
        bool HasValue<T>(Setting<T> setting);

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

        /// <summary>Sets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <param name="value">The value.</param>
        void SetValue<T>(Setting<T> setting, T value);

        /// <summary>Sets the value of the setting.</summary>
        /// <typeparam name="T">The type of the setting's value</typeparam>
        /// <param name="setting">The setting.</param>
        /// <param name="value">The value.</param>
        /// <param name="converter">A function which converts the value into its string representation.</param>
        void SetValue<T>(Setting<T> setting, T value, Func<T, string> converter);
    }
}