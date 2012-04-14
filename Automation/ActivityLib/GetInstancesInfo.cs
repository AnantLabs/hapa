using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Common;


namespace ActivityLib
{
    public class GetInstancesInfo : LeafAction
    {
        protected override void Execute(NativeActivityContext context)
        {
            
            try
            {
                string instancesInfo = InstanceManager.GetInstance().GetInstances();
                Result r = Common.Result.SuccessResult();
                r.attach(instancesInfo);
                SetReturnMessage(context, r );
            }
            catch (Exception ex)
            {
                SetReturnMessage(context, Common.Result.ErrorResult(ex));
            }
        }
    }
}
