using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Common;
//using MongoDB;

namespace ActivityLib
{
    public class SetData : LeafAction
    {
        protected override void Execute(NativeActivityContext context)
        {

            string commandStr = GetContextValue(context, "command");
            
            try
            {
                XElement content = (XElement)XElement.Parse(commandStr).FirstNode;
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
