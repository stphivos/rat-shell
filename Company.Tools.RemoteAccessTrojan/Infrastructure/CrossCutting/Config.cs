using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using Company.Tools.RemoteAccessTrojan.Infrastructure.Logging;
using Company.Tools.RemoteAccessTrojan.Application;

namespace Company.Tools.RemoteAccessTrojan.Infrastructure.CrossCutting
{
    public class Config
    {
        #region Properties

        // Static
        public static Config Current { get; set; }

        // Application
        public string ApplicationServiceTypeName { get; set; }

        // Logging
        public string LoggerTypeName { get; set; }
        public string EventSource { get; set; }

        // Communication
        public string IpAddress { get; set; }
        public int ListenPort { get; set; }
        public int MaxPendingConnections { get; set; }
        public int PollInterval { get; set; }

        #endregion

        #region Methods

        #region Constructors

        private Config()
        {
        }

        #endregion

        #region Exposed

        public static Config Parse(string[] args)
        {
            var config = Config.GetInstanceFromAppConfig().GetOverwritesFrom(Config.GetInstanceFromArgs(args));
            return config;
        }

        #endregion

        #region Loading

        private static Config GetInstanceFromAppConfig()
        {
            var config = new Config()
            {
                ApplicationServiceTypeName = Config.GetKeyValue<string>("ApplicationServiceTypeName", typeof(ApplicationService).FullName),
                LoggerTypeName = Config.GetKeyValue<string>("LoggerTypeName", typeof(WindowsEventLogger).FullName),
                EventSource = Config.GetKeyValue<string>("EventSource", Assembly.GetExecutingAssembly().GetName().Name),
                IpAddress = Config.GetKeyValue<string>("IpAddress", "127.0.0.1"),
                ListenPort = Config.GetKeyValue<int>("ListenPort", 9999),
                MaxPendingConnections = Config.GetKeyValue<int>("MaxPendingConnections", 10),
                PollInterval = Config.GetKeyValue<int>("PollInterval", 1000)
            };
            return config;
        }

        private static Config GetInstanceFromArgs(string[] args)
        {
            Config config = new Config();

            foreach (var arg in args)
            {
                var parts = arg.Split('=');
                var key = parts.First();
                var value = parts.Skip(1).First();

                switch (key.ToLower())
                {
                    case "a":
                    case "application":
                        config.ApplicationServiceTypeName = value;
                        break;
                    case "l":
                    case "logger":
                        config.LoggerTypeName = value;
                        break;
                    case "s":
                    case "event":
                        config.EventSource = value;
                        break;
                    case "i":
                    case "ip":
                        config.IpAddress = value;
                        break;
                    case "p":
                    case "port":
                        config.ListenPort = value.ConvertTo<int>();
                        break;
                    case "m":
                    case "connections":
                        config.MaxPendingConnections = value.ConvertTo<int>();
                        break;
                    case "n":
                    case "interval":
                        config.PollInterval = value.ConvertTo<int>();
                        break;
                }
            }

            return config;
        }

        private Config GetOverwritesFrom(Config source)
        {
            var config = new Config()
            {
                ApplicationServiceTypeName = source.ApplicationServiceTypeName.IsDefaultValue() ? this.ApplicationServiceTypeName : source.ApplicationServiceTypeName,
                LoggerTypeName = source.LoggerTypeName.IsDefaultValue() ? this.LoggerTypeName : source.LoggerTypeName,
                EventSource = source.EventSource.IsDefaultValue() ? this.EventSource : source.EventSource,
                IpAddress = source.IpAddress.IsDefaultValue() ? this.IpAddress : source.IpAddress,
                ListenPort = source.ListenPort.IsDefaultValue() ? this.ListenPort : source.ListenPort,
                MaxPendingConnections = source.MaxPendingConnections.IsDefaultValue() ? this.MaxPendingConnections : source.MaxPendingConnections,
                PollInterval = source.PollInterval.IsDefaultValue() ? this.PollInterval : source.PollInterval,
            };
            return config;
        }

        #endregion

        #region Helper

        private static T GetKeyValue<T>(string key, T valueIfNotFound)
        {
            T value = Config.GetKeyValue<T>(key, false);
            if (value.IsDefaultValue())
            {
                return valueIfNotFound;
            }
            else
            {
                return value;
            }
        }

        private static T GetKeyValue<T>(string key, bool isRequired = true)
        {
            var typedValue = default(T);

            var value = ConfigurationManager.AppSettings[key];
            if (value != null)
            {
                typedValue = value.ConvertTo<T>();
            }
            else if (isRequired)
            {
                throw new Exception(string.Format("Required key '{0}' not found in configuration.", key));
            }

            return typedValue;
        }

        #endregion

        #endregion
    }
}