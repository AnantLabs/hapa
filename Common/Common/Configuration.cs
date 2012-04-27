﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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
        /// 
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
        public static void Set(string key, string value)
        {
            ConfigurationManager.AppSettings.Set(key, value);
        }

        public static void AddSettingsToXElement(XElement x)
        {
            foreach (var key in ConfigurationManager.AppSettings.Keys)
            {
                x.SetAttributeValue(key.ToString(), Configuration.Settings(key.ToString(),null));
            }
        }
    }
}

