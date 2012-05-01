using System.Reflection;
using System.Xml.Linq;

namespace Common
{
    public abstract class SuperData
    {
        //private string _id;
        public string Id { //get
            //{
            //    if (String.IsNullOrEmpty(_id))
            //        _id = Guid.NewGuid().ToString();
            //    return _id;
            //}
            //set { _id = value; }
            get; set; }

        public string ParentId { get; set; }
        public string DataType { get; set; }
        public string DisplayName { get; set; }
        public string DisplayIcon { get; set; }

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