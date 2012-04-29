using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common;

namespace AutoClient
{
    public class AutoClientManager :IDisposable
    {
        private static AutoClientManager _instance;
        private List<IAutoObserver> observerList = new List<IAutoObserver>();
        //static CancellationTokenSource cancell = new CancellationTokenSource();
        //CancellationToken token = cancell.Token;
        Service.ServiceSoapClient client;
        //Service.ServiceSoapClient client = new Service.ServiceSoapClient();
        Task task;
        
        private AutoClientManager()
        {
            ActionsFactory factory = new ActionsFactory();
            //client.Open();
            //task = new Task(() =>
            //{
            //    while (true)
            //    {
            //        //bool cancelled = token.WaitHandle.WaitOne(5 * 60 * 1000);
            //        //DateTime now = DateTime.Now;

                    

            //        //if (cancelled)
            //        //{
            //        //    throw new OperationCanceledException(token);
            //        //}


            //        if(!Register())
            //            continue;
            //        while (true)
            //        {
            //            DoOneCommand(factory);
            //        }

            //    }
            //    //}, token);
            //});
            //task.Start();
        }

        private bool Register()
        {
            try
            {
                if (client.State.Equals(CommunicationState.Closed))
                    client.Open();
                //TODO connect and register here, get computer info and submit
                XElement x = XElement.Parse("<Command Name=\"Register\" />");
                AddEnvironmentVarsToXElement(x);
                Configuration.AddSettingsToXElement(x);
                string result = client.Command(x.ToString());
                //TODO check the result, it may bring back some configuration changes.
            }
            catch (Exception e)
            {
                Thread.Sleep(Const.PauseAfterRegisterFailure);
                return false;
            }
            return true;
        }

        public string DoTest()
        {
            XElement x = XElement.Parse("<Command Name=\"Register\" />");
            Configuration.Set("TestKey", "TestValue");
            Configuration.SaveSettings();
            AddEnvironmentVarsToXElement(x);
            Configuration.AddSettingsToXElement(x);
            return x.ToString();
        }

        private static void AddEnvironmentVarsToXElement(XElement x)
        {
            foreach (var v in System.Environment.GetEnvironmentVariables().Keys)
            {
                string key = v.ToString().Replace("(", "").Replace(")", "");
                string value = System.Environment.GetEnvironmentVariables()[v].ToString().Replace("(", "").Replace(")", "");
                x.SetAttributeValue(key, value);
                //result += v.ToString() + " = " + System.Environment.GetEnvironmentVariables()[v]+" \n";
            }
        }

        private void DoOneCommand(ActionsFactory factory)
        {
            //TODO request command here, if communication error happen, break
            string xmlformatCommand = client.Command("<Command Name=\"RequestCommand\" />");
            Result result = factory.DoCommand(XElement.Parse(xmlformatCommand));
            //TODO construct the result string
            client.Command("<Command Name=\"\" />");
        }

        public void ObserverRegister(IAutoObserver observer)
        {
            observerList.Add(observer);
        }

        public void ObserverUnRegister(IAutoObserver observer)
        {
            observerList.Remove(observer);
        }

        void NotifyObservers(String message)
        {
            foreach (IAutoObserver observer in observerList)
            {
                if(observer!=null)
                    observer.update(message);
            }
        }

        public static AutoClientManager GetInstance()
        {
            return _instance ?? (_instance = new AutoClientManager());
        }

        public void Dispose()
        {
            if(client!=null)
                if (!client.State.Equals(CommunicationState.Closed))
                    client.Close();
            if (task != null) 
                task.Dispose();
            _instance = null;
        }
    }
}
