using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Common;


namespace ActivityLib
{
    public class DeleteInstance : LeafAction
    {
        protected override void Execute(NativeActivityContext context)
        {
            string commandStr = GetContextValue(context, "command");

            try
            {
                string id = GetContextValue(context, Const.AttributeId);
                //XElement content = (XElement)XElement.Parse(commandStr);
                
                InstanceManager.GetInstance().RemoveTestInstance(id);

                SetReturnMessage(context, Common.Result.SuccessResult());
            }
            catch (Exception ex)
            {
                SetReturnMessage(context, Common.Result.ErrorResult(ex));
            }
        }
    }
}
