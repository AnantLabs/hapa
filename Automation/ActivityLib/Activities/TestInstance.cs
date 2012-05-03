using System.Activities.Hosting;
using System.Xml.Linq;
using Common;

namespace ActivityLib.Activities
{
    public class TestInstance
    {
        private Const.InstanceStatus _status = Const.InstanceStatus.Ready;
        private XElement instanceInfoXElement;
        private WorkflowInstance workflow = null;

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

        public string Id { get; set; }

        public Const.InstanceStatus Status
        {
            get { return _status; }
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
            get { return instanceInfoXElement; }
            set { instanceInfoXElement = value; }
        }

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
    }
}