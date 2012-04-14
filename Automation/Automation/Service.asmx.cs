using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Automation
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {

        /// <summary>
        /// It only accept the XML format command
        /// </summary>
        /// <param name="xmlFormatCommand"></param>
        /// <returns></returns>
        [WebMethod]
        public string Command(string xmlFormatCommand)
        {
            //XElement xe = XElement.Parse(xmlFormatCommand);
            //string name = xe.GetAttributeValue("name");
            //string id = xe.GetAttributeValue(Const.AttributeId);

            //MainFlow wf = new MainFlow();
            ////ActivityLib.MainWorkflow wf = new ActivityLib.MainWorkflow();
            //System.Collections.Generic.IDictionary<string, object> input = new System.Collections.Generic.Dictionary<string, object>();
            //input.Add("command", xmlFormatCommand);

            //if (name != null)
            //    input.Add("name", name);
            //if (id != null)
            //    input.Add(Const.AttributeId, id);

            //var result = WorkflowInvoker.Invoke(wf, input, new TimeSpan(0, 10, 10));

            //string outValue = (string)result["returnMessage"];
            //Logger.GetInstance().Log("Return:").Debug(outValue);
            //return outValue;
            return "<Nothing />";
        }
    }
}
