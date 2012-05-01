using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common;

namespace AutoClient
{
    public class ActionsFactory
    {
        //receive command, find out the steps, call the actions, return result
        //TODO should return Result
        public Result DoCommand(XElement command)
        {
            IEnumerable<XElement> query = from o in command.DescendantsAndSelf()
                                          where o.Name.Equals("Action")
                                          select o;
            foreach (XElement action in query)
            {
            }

            return null;
        }
    }
}