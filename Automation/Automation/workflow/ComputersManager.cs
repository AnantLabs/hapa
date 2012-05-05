using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common;

namespace Automation.workflow
{
    public class ComputersManager
    {
        private static ComputersManager _instance;
        private readonly Dictionary<string, Computer> _computerList = new Dictionary<string, Computer>();

        private ComputersManager()
        {
            var cancell = new CancellationTokenSource();
            CancellationToken token = cancell.Token;
            var task = new Task(() =>
                                    {
                                        while (true)
                                        {
                                            bool cancelled = token.WaitHandle.WaitOne(5*60*1000);
                                            DateTime now = DateTime.Now;
                                            foreach (string nameOfComputer in _computerList.Keys)
                                            {
                                                Computer computer = _computerList[nameOfComputer];
                                                TimeSpan lostMessage = now - computer.LastAccess;
                                                if (lostMessage.Hours >= 1)
                                                    _computerList.Remove(nameOfComputer);
                                            }
                                            if (cancelled)
                                            {
                                                throw new OperationCanceledException(token);
                                            }
                                        }
                                    }, token);
            task.Start();
        }

        public void Register(string xComputerInfo)
        {
            XElement xElement = XElement.Parse(xComputerInfo);
            var computerElement = (XElement) (xElement.FirstNode);
            var computer = new Computer(computerElement);
            if (_computerList.ContainsKey(computer.Name))
            {
                _computerList.Remove(computer.Name);
            }
            _computerList.Add(computer.Name, computer);
        }

        public static ComputersManager GetInstance()
        {
            return _instance ?? (_instance = new ComputersManager());
        }

        public Computer GetComputer(string nameOfComputer)
        {
            foreach (string name in _computerList.Keys)
            {
                if (name.Contains(nameOfComputer))
                    return _computerList[name];
            }
            return null;
        }

        public override string ToString()
        {
            var cl = new XElement("ComputerList");
            cl.SetAttributeValue("Number", _computerList.Count);
            foreach (Computer computer in _computerList.Values)
            {
                cl.Add(computer.Element());
            }
            return cl.ToString(SaveOptions.None);
        }
    }

    public class Computer
    {
        private readonly ArrayList _commandList = new ArrayList();
        private readonly XElement _element;

        public Computer(XElement info)
        {
            _element = info;
            XAttribute xAttribute = info.Attribute(Const.AttributeId);
            if (xAttribute != null) Name = xAttribute.Value;
            //Role = info.Attribute("Role").Value;
            LastAccess = DateTime.Now;
        }

        public string Name { set; get; }
        public string Role { set; get; } //computer's role
        public DateTime LastAccess { set; get; }


        public XElement Element()
        {
            return _element;
        }

        public override string ToString()
        {
            return _element.ToString(SaveOptions.None);
        }

        public void SetCommand(string command)
        {
            lock (_commandList)
            {
                if (!string.IsNullOrEmpty(command))
                {
                    //Monitor.Wait(CommandList);                  
                    _commandList.Add(command);
                    Monitor.Pulse(_commandList);
                }
            }
        }

        public string GetCommand()
        {
            //Register last access time, for further clear use
            string retCommand;
            lock (_commandList)
            {
                //Monitor.Wait(CommandList);
                if (_commandList.Count == 0)
                {
                    //no command for this computer now, send back a Wait
                    retCommand = @"<Command Action='Wait' Data='10' />";
                }
                else
                {
                    retCommand = _commandList[0].ToString();
                    _commandList.RemoveAt(0);
                }
                LastAccess = DateTime.Now;
                Monitor.Pulse(_commandList);
            }
            return retCommand;
        }
    }
}