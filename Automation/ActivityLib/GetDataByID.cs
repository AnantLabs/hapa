using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Common;


namespace ActivityLib
{
    public class GetDataByID : LeafAction
    {
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string id = GetContextValue(context, Const.AttributeId);

                string data = "";// MongoDB.MongoDB.GetInstance()[id];
                if (string.IsNullOrEmpty(data))
                {
                    SetReturnMessage(context, Common.Result.ErrorResult("No such a data in DB, id=" + id));
                    return;
                }
                Result r = Common.Result.SuccessResult();
                r.attach(data);
                SetReturnMessage(context, r);
            }
            catch (Exception ex)
            {
                SetReturnMessage(context, Common.Result.ErrorResult(ex));
            }
        }

        
    }
}
