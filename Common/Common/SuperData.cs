using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Common
{
    public abstract class SuperData
    {
        public string id
        {
            get
            {
                if (String.IsNullOrEmpty(id))
                    id = Guid.NewGuid().ToString();
                return id;
            }
            private set { id = value; }
        }

        public static XElement ToXML(SuperData data)
        {
            throw new NotImplementedException();
        }

        public static SuperData GetBaseData(SuperData data)
        {
            throw new NotImplementedException();
        }
        
    }
}
