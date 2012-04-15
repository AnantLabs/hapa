using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClient
{
    public class AutoClientManager :IDisposable
    {
        private static AutoClientManager _instance;
        private List<IAutoObserver> observerList = new List<IAutoObserver>();
        //static CancellationTokenSource cancell = new CancellationTokenSource();
        //CancellationToken token = cancell.Token;
        Service.ServiceSoapClient client = new Service.ServiceSoapClient();
        Task task;
        
        private AutoClientManager()
        {
            client.Open();
            task = new Task(() =>
            {
                while (true)
                {
                    //bool cancelled = token.WaitHandle.WaitOne(5 * 60 * 1000);
                    //DateTime now = DateTime.Now;

                    

                    //if (cancelled)
                    //{
                    //    throw new OperationCanceledException(token);
                    //}

                    
                    if (client.State.Equals(CommunicationState.Closed))
                        client.Open();
                    //TODO connect and register here, get computer info and submit
                    client.Command("<Command Name=\"Register\" />");
                    while (true)
                    {
                        //TODO request command here, if communication error happen, break
                        client.Command("<Command Name=\"RequestCommand\" />");
                    }

                }
                //}, token);
            });
            task.Start();
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
