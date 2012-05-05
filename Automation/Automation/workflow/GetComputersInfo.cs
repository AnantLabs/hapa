using System;
using System.Activities;
using System.Xml.Linq;
using ActivityLib;
using ActivityLib.Activities;
using Common;

namespace Automation.workflow
{
    /// <summary>
    /// 
    /// Return Registered computer list
    /// </summary>
    public class GetComputersInfo : LeafAction
    {
        protected override void Execute(NativeActivityContext context)
        {
            string commandStr = GetContextValue(context, "command");

            try
            {
                XElement content = XElement.Parse(commandStr);

                Result r = Common.Result.SuccessResult();
                r.attach(ComputersManager.GetInstance().ToString());
                SetReturnMessage(context, r);
            }
            catch (Exception ex)
            {
                SetReturnMessage(context, Common.Result.ErrorResult(ex));
            }
        }
    }
}