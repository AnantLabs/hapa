using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Common;


namespace ActivityLib
{
    public class SetInstanceInfo : LeafAction
    {
        protected override void Execute(NativeActivityContext context)
        {
            
            try
            {
                string commandStr = GetContextValue(context, "command");

                XElement content = (XElement)XElement.Parse(commandStr);
                
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
