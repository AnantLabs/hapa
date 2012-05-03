using System;
using System.Activities;
using System.Xml.Linq;
using ActivityLib.Activities;
using Common;

namespace ActivityLib
{
    public class RequestCommand : LeafAction
    {
        protected override void Execute(NativeActivityContext context)
        {
            //if computer is not in the computer manager, add it;
            //if no command in it, return wait 10s, 
            string commandStr = GetContextValue(context, "command");

            try
            {
                XElement content = XElement.Parse(commandStr);
                string computerName = content.GetAttributeValue(Const.AttributeId);
                if (string.IsNullOrEmpty(computerName))
                {
                    SetReturnMessage(context,
                                     Common.Result.ErrorResult("Want to request Command,but no ComputerName:\n" +
                                                               commandStr));
                    return;
                }

                Computer computer = ComputersManager.GetInstance().GetComputer(computerName);
                if (computer == null)
                {
                    ComputersManager.GetInstance().Register(commandStr);
                    computer = ComputersManager.GetInstance().GetComputer(computerName);
                }
                string commandInfo = computer.GetCommand();
                Result r = Common.Result.SuccessResult();
                //r.Attach(commandInfo);
                SetReturnMessage(context, r);
            }
            catch (Exception ex)
            {
                SetReturnMessage(context, Common.Result.ErrorResult(ex));
            }
        }
    }
}