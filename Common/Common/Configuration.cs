using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class Configuration
    {
        /// <summary>
        /// Now we have below settings now:
        /// DBConnectionString="mongodb://hostdb"
        /// DBName = "Automation"
        /// LogFormat="%newline%date [%thread] %-5level [%message%]%newline"
        /// LogPath = "c:\temp"
        /// LogName = "Automation.log"
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static string Settings(string setting, string defaultString = null)
        {
            string value = ConfigurationManager.AppSettings[setting];
            if (!string.IsNullOrEmpty(value))
                return value;
            return defaultString;
        }

}
