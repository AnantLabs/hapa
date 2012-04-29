using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Common;
using System.Threading.Tasks;
using System.Activities;
using System.Activities.Hosting;

namespace ActivityLib
{
    public class TestInstance
    {
        private XElement instanceInfoXElement;
        private WorkflowInstance workflow = null;
        public void Start()
        {
            Status = Const.InstanceStatus.Running;
            //start or resume workflow
            
        }

        public void Stop()
        {
            Status = Const.InstanceStatus.Stopped;
            //suspend or pause workflow
        }

        public string Id { get; set; }

        private Const.InstanceStatus _status = Const.InstanceStatus.Ready;
        public Const.InstanceStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                lock (this)
                {
                    _status = value;
                }
                instanceInfoXElement.SetAttributeValue("Status", _status.ToString());
            }
        }

        public XElement Element
        {
            get
            {
                return instanceInfoXElement;
            }
            set
            {
                instanceInfoXElement = value;
            }
        }

        public TestInstance(XElement instanceInfoXElement)
        {

            this.instanceInfoXElement = instanceInfoXElement;
            XAttribute xa = instanceInfoXElement.Attribute(Const.AttributeId);
            if (xa != null)
                Id = xa.Value;
            
            //TODO load workflow here
            //get workflow store id
            //get string content of workflow
            //load workflow
            Status = Const.InstanceStatus.Ready;

        }
        
    }
}
