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
            Status = "Start";
            //start or resume workflow
            
        }

        public void Stop()
        {
            Status = "STOP";
            //suspend or pause workflow
        }

        public string Id { get; set; }

        private string _status = "STOP";
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                lock (_status)
                {
                    _status = value;
                }
                instanceInfoXElement.SetAttributeValue("Status", _status);
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
            Status = "STOP";
            //TODO load workflow here
            //get workflow store id
            //get string content of workflow
            //load workflow

        }
        
    }
}
