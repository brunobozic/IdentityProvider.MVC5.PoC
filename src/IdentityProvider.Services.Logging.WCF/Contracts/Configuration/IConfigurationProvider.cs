using System;
using System.Collections.Generic;

namespace HAC.Helpdesk.Services.Logging.WCF.Contracts.Configuration
{
    public interface IConfigurationProvider : IDisposable
    {
        Dictionary<string, string> FakeSettingsDictionary { get; set; }

        #region Properties

        T GetConfigurationValue<T>(string key);

        /// <summary>
        ///     Fetches a property from an underlying provider (usually web.config).
        ///     Logs the problem into a provided log provider.
        ///     Throws an error if the property is not found specifying which property was not found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetConfigurationValueAndNotifyIfPropertyNotFound<T>(string key);

        T GetConfigurationValue<T>(string key, T defaultValue);
        T GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound<T>(string key, T defaultValue);

        #endregion
    }
}