using System.Configuration;
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
            System.Configuration.Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection app = config.AppSettings;
            ConfigurationManager.AppSettings.Set(key, value);
        }

        public static void AddSettingsToXElement(XElement x)
        {
            foreach (object key in ConfigurationManager.AppSettings.Keys)
            {
                x.SetAttributeValue(key.ToString(), Settings(key.ToString(), null));
            }
        }


        /// <summary>
        /// we can only save the settings of editor, client; cannot save the settings of server side--web.config
        /// </summary>
        public static void SaveSettings()
        {
            System.Configuration.Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection app = config.AppSettings;
            foreach (object key in ConfigurationManager.AppSettings.Keys)
            {
                if (app.Settings[key.ToString()] == null)
                    app.Settings.Add(key.ToString(), Settings(key.ToString(), key.ToString()));
                app.Settings[key.ToString()].Value = Settings(key.ToString(), key.ToString());
            }
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //AppSettingsSection app = config.AppSettings;
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}