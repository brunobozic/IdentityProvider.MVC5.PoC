using System;
using System.Collections.Generic;
using System.Configuration;

namespace IdentityProvider.Services.Logging.WCF
{
    public class ConfigFileConfigurationProvider : IConfigurationProvider
    {
        public Dictionary<string, string> FakeSettingsDictionary => throw new NotImplementedException();

        public T GetConfigurationValue<T>(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
                throw new KeyNotFoundException(string.Format(KeyNotFound, key));
            try
            {
                if (typeof(Enum).IsAssignableFrom(typeof(T)))
                    return (T)Enum.Parse(typeof(T), value);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T GetConfigurationValue<T>(string key, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
                return defaultValue;
            try
            {
                if (typeof(Enum).IsAssignableFrom(typeof(T)))
                    return (T)Enum.Parse(typeof(T), value);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        Dictionary<string, string> IConfigurationProvider.FakeSettingsDictionary
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public void Dispose()
        {
            Disposed = true;
        }

        public T GetConfigurationValueAndNotifyIfPropertyNotFound<T>(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
                throw new KeyNotFoundException(string.Format(KeyNotFound, key));
            try
            {
                if (typeof(Enum).IsAssignableFrom(typeof(T)))
                    return (T)Enum.Parse(typeof(T), value);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                // _log.LogFatal(this, KeyNotFound, ex);
                throw ex;
            }
        }

        public T GetConfigurationValueOrDefaultAndNotifyIfPropertyNotFound<T>(string key, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
                return defaultValue;
            try
            {
                if (typeof(Enum).IsAssignableFrom(typeof(T)))
                    return (T)Enum.Parse(typeof(T), value);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                // _log.LogWarning(this, string.Format(KeyNotFoundUsingDefaultValue, key, defaultValue));
                return defaultValue;
            }
        }

        #region Ctor

        #endregion Ctor

        #region Properties

        private const string KeyNotFoundUsingDefaultValue =
            "Key [ {0} ] not found in web.config (please add it), using default of [ {1} ].";

        private const string KeyNotFound = "Key [ {0} ] not found in web.config, please add it.";


        public bool Disposed { get; set; }

        #endregion Properties
    }
}