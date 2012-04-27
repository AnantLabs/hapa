using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Selenium;
using Common;
using System.Diagnostics;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System.Collections;
using System.Collections.ObjectModel;

namespace SeleniumActions
{
    public class Browser
    {
        private static Browser Instance = null;
        private IWebDriver browser;
        private Browser()
        {
        }
        public static Browser GetInstance()
        {
            return Instance ?? (Instance = new Browser());
        }

        public IWebDriver GetCurrentBrowser()
        {
            if (browser == null)
            {
                CloseBrowser();
                StartBrowser();
            }
            DismissUnexpectedAlert();
            ReadOnlyCollection<string> bs = browser.WindowHandles;
            if (bs.Count == 1)
            {
                browser.SwitchTo().Window(bs[0]);
            }
            else
            {
                browser.SwitchTo().Window(bs[bs.Count - 1]);
            }

            return browser;
        }
        //CSStype subpool<fingerprint string, webelement>
        Hashtable pool = new Hashtable();
        //CSS types that we care
        string CSSXPath = Configuration.Settings("CSSType","//*[@class='ROW1' or @class='ROW2' or @class='' or @class='' or @class='' or @class='' or @class='' or @class='' or @class='' or @class='' or @class='' or @class='']");
        public void Prepare()
        {

        }

        public void Clear()
        {
            foreach (var item in pool.Values)
            {
                if(item.GetType().Name.Contains("Hashtable")){
                    ((Hashtable)item).Clear();
                }
            }
            pool.Clear();
        }

        public IWebElement GetWebElement(string tag, params string[] key_value)
        {
            //TODO get object from object pool? Multithread?
        }

        public string Snapshot()
        {
            return ((ITakesScreenshot)GetCurrentBrowser()).GetScreenshot().AsBase64EncodedString;
            //IJavaScriptExecutor js = GetCurrentBrowser() as IJavaScriptExecutor;
            //Response screenshotResponse = js.ExecuteScript(DriverCommand.Screenshot, null);
            //return screenshotResponse.Value.ToString();
        }

        private void DismissUnexpectedAlert()
        {
            IAlert alert = GetAlert();
            if (alert != null)
            {
                alert.Dismiss();
                Logger.GetInstance().Log().Warn("Close an unexpected dialog, please check.");
            }
        }

        private IAlert GetAlert()
        {
            IAlert alert = null;
            try
            {
                alert = browser.SwitchTo().Alert();
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Log().Debug("Suppress get alert");
            }
            return alert;
        }

        private void StartBrowser()
        {
            string browserType = Configuration.Settings("BrowserType", "IE");
            if (browserType.Equals("IE"))
                browser = new InternetExplorerDriver();
            if (browserType.Equals("Firefox"))
                browser = new FirefoxDriver();
            if (browserType.Equals("Chrome"))
                browser = new ChromeDriver();

            MaximiseBrowser();
        }

        private void MaximiseBrowser()
        {
            browser.Manage().Window.Maximize();
        }

        private void CloseBrowser()
        {
            if (browser != null)
                browser.Quit();
            browser = null;

            string browserType = Configuration.Settings("BrowserType", "IE");
            if (browserType.Equals("IE"))
                Process.Start("taskkill /IM iexplore.exe");
            if (browserType.Equals("Firefox"))
                Process.Start("taskkill /IM firefox.exe");
            if (browserType.Equals("Chrome"))
                Process.Start("taskkill /IM chrome.exe");

        }
    }
}
