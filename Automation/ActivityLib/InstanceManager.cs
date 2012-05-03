using System.Collections.Generic;
using System.Xml.Linq;
using ActivityLib.Activities;

namespace ActivityLib
{
    public class InstanceManager
    {
        private static InstanceManager Instance;
        private readonly Dictionary<string, TestInstance> InstanceList = new Dictionary<string, TestInstance>();


        private InstanceManager()
        {
        }

        public static InstanceManager GetInstance()
        {
            if (Instance == null)
                Instance = new InstanceManager();
            return Instance;
        }

        public void UpdateInstance(XElement instanceInfo)
        {
            foreach (XNode node2 in instanceInfo.Nodes())
            {
                var instanceInfoXElement = (XElement) node2;
                var testInstance = new TestInstance(instanceInfoXElement);
                string instanceId = testInstance.Id;
                if (InstanceList.ContainsKey(instanceId))
                    InstanceList[instanceId] = testInstance;
                else
                    InstanceList.Add(testInstance.Id, testInstance);
            }
            //return instanceId;
        }

        public TestInstance GetTestInstance(string id)
        {
            if (InstanceList.ContainsKey(id))
            {
                return InstanceList[id];
            }
            return null;
        }

        public string GetInstances()
        {
            var list = new XElement("Instances");
            foreach (TestInstance ti in InstanceList.Values)
            {
                list.Add(ti.Element);
            }
            return list.ToString();
        }

        public void RemoveTestInstance(string id)
        {
            if (InstanceList.ContainsKey(id))
            {
                InstanceList.Remove(id);
            }
        }
    }
}