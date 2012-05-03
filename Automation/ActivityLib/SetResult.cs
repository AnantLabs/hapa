using System;
using System.Activities;
using System.Xml.Linq;
using ActivityLib.Activities;

namespace ActivityLib
{
    public class SetResult : LeafAction
    {
        protected override void Execute(NativeActivityContext context)
        {
            string commandStr = GetContextValue(context, "command");

            try
            {
                XElement content = XElement.Parse(commandStr);
                //TODO not implemented yet, related to BookMark

                SetReturnMessage(context, Common.Result.SuccessResult());
            }
            catch (Exception ex)
            {
                SetReturnMessage(context, Common.Result.ErrorResult(ex));
            }
        }
    }
}