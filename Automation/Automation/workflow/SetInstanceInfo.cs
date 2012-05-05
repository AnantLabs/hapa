using System;
using System.Activities;
using System.Xml.Linq;
using ActivityLib;
using ActivityLib.Activities;

namespace Automation.workflow
{
    public class SetInstanceInfo : LeafAction
    {
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string commandStr = GetContextValue(context, "command");

                XElement content = XElement.Parse(commandStr);

                InstanceManager.GetInstance().UpdateInstance(content);
                SetReturnMessage(context, Common.Result.SuccessResult());
            }
            catch (Exception ex)
            {
                SetReturnMessage(context, Common.Result.ErrorResult(ex));
            }
        }
    }
}