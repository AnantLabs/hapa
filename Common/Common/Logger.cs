using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Common
{
    /// <summary>
    /// put below info into app.config or web.config
    /// <example>
    /*
<configuration>
<configSections>
<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net" requirePermission="false"/>
</configSections>
<log4net debug="false">
<appender name="LogFileAppender" type="log4net.Appender.FileAppender,log4net" >
  <param name="File" value="c:\\temp\\log.txt" />
  <param name="AppendToFile" value="true" />
  <layout type="log4net.Layout.PatternLayout,log4net">
    <param name="ConversionPattern" value="%-23d %-5p %c - %m%n" />
  </layout>
</appender>
<root>
  <priority value="ALL" /><!-- set leve here:FATAL,ERROR,WARN,INFO,DEBUG,ALL-->
  <appender-ref ref="LogFileAppender" />
</root>
    
</log4net>
<!--<startup>
<supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5"/>
</startup>-->
</configuration>

     */
    /// </example>
    /// </summary>
    public class Logger
    {
        private static Logger _instance;
        private Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static Logger GetInstance()
        {
            return _instance ?? (_instance = new Logger());
        }

        public ILog Log(Type type)
        {
            return type == null ? Log() : LogManager.GetLogger(type);
        }
        public ILog Log()
        {
            return LogManager.GetLogger("Message:");
        }
        public ILog Log(string category)
        {
            return string.IsNullOrEmpty(category) ? Log() : LogManager.GetLogger(category);
        }

        public ILog Log(object obj)
        {
            return obj == null ? Log() : LogManager.GetLogger(obj.GetType());
        }
    }
}
