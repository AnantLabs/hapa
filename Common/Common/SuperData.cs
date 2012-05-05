using System.Reflection;
using System.Xml.Linq;

namespace Common
{
    public class SuperData
    {
        private string _id;
        public string Id { get { return _id; } set { _id = value; } }
        public string ParentId { get; set; }
        //protected string DataType { get; set; }
        public string DisplayName { get; set; }
        public string DisplayIcon { get; set; }

        public SuperData(){}
        public SuperData(string id, string parentId,string dispalyName)
        {
            Id = id;
            ParentId = parentId;
            //DataType = dataType;
            DisplayName = dispalyName;
        }

        //public static SuperData CreateItem(string itemType)
        //{
        //    SuperData superData = null;
        //    if(itemType.Equals("Project"))
        //    {
        //        superData = new Project();
        //    }
        //    if(itemType.Equals("Result"))
        //    {
        //        superData = new Result();
        //    }
        //    if(itemType.Equals("Data"))
        //    {
        //        superData = new InputData();
        //    }
        //    if(itemType.Equals("Object"))
        //    {
        //        superData = new UIObject();
        //    }
        //    if(itemType.Equals("TestSuite"))
        //    {
        //        superData = new TestSuite();
        //    }
        //    if(itemType.Equals("TestCase"))
        //    {
        //        superData = new TestCase();
        //    }
        //    if(itemType.Equals("TestSteps"))
        //    {
        //        superData = new TestSteps();
        //    }
        //    if(itemType.Equals("Folder"))
        //    {
        //        superData = new Folder();
        //    }
        //    if(itemType.Equals("Environment"))
        //    {
        //        superData = new Environment();
        //    }

        //    if (itemType.Equals("Translation"))
        //    {
        //        superData = new Translation();
        //    }

        //    return superData;
        //}

        public static XElement ToXML(SuperData data)
        {
            if (data == null)
                return null;
            var element = new XElement(data.GetType().Name);
            FieldInfo[] fields = data.GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                string fName = field.Name;
                object fValue = field.GetValue(data);
                element.SetAttributeValue(fName, fValue);

                // remember, 2 way link is hard to maintain.
                //TODO if field is SuperData, just put a id link here "Id:xxxx-xxx...-xxxx" (In theory, it should not happen, we use parentId to point to Parent, don't use this way to point out children)
                //TODO if field is Collection, add them as children element( In theory, should not happen. we use parentId to point to Parent, don't use this way to point out children)
            }
            return element;
        }

        public static SuperData GetBaseData(SuperData data)
        {
            if (data == null)
                return null;
            return (SuperData) data.MemberwiseClone();
        }
    }
}