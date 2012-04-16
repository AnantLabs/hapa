using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Common;

namespace AutoClient
{
    public class ActionsFactory
    {
        public ActionsFactory()
        {
            //read configuration.xml, load all assemblies
        }

        //receive command, find out the steps, call the actions, return result
        //TODO should return Result
        public Result DoCommand(XElement command)
        {
            var query = from o in command.DescendantsAndSelf()
                        where o.Name.Equals("Action")
                        select o;
            foreach (XElement action in query)
            {
                
            }

            return null;
        }
    }
}
