using System;
using System.Activities;
using System.Xml.Linq;
using ActivityLib.Activities;

//using MongoDB;

namespace Automation.workflow
{
    public class SetData : LeafAction
    {
        protected override void Execute(NativeActivityContext context)
        {
            string commandStr = GetContextValue(context, "command");

            try
            {
                var content = (XElement) XElement.Parse(commandStr).FirstNode;
                //MongoDB.MongoDB.GetInstance()["New"]=content.ToString();
                SetReturnMessage(context, Common.Result.SuccessResult());
            }
            catch (Exception ex)
            {
                SetReturnMessage(context, Common.Result.ErrorResult(ex));
            }
        }
    }
}