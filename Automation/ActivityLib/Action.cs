using System;
using System.Activities;
using System.ComponentModel;
using Common;

namespace ActivityLib
{
    [Designer(typeof (ActionDesigner))]
    public abstract class Action : NativeActivity
    {
        //use the ImageConverter to show the result image
        // set it to un-browsable, but now for debug reason, show it.
        //[Browsable(false)]

        protected Action()
        {
            MyId = Guid.NewGuid().ToString();
            AllowDropItemType = "GUIFormat";
        }

        public string Result { get; set; }

        // set it to un-browsable, but now for debug reason, show it.
        //[Browsable(false)]
        public string AllowDropItemType { get; set; }

        //[Browsable(false)]
        //public DataContext CurrentDataContext { get; set; }

        [RequiredArgument]
        [ReadOnly(true)]
        [Browsable(false)]
        public string MyId { get; set; }

        /// <summary>
        /// It only is used at runtime, the parent pass the instance id to it, then we can get data by this instance id.
        /// </summary>
        [Browsable(false)]
        public string InstanceId { get; set; }

        public Const.ResultReaction OnError { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(NativeActivityContext nativeActivityContext)
        {
            // Obtain the runtime value of the Text input argument
        }

        //public abstract XElement SetData(ActivityContext context, DataContext currentDataContext);

        public static string GetContextValue(NativeActivityContext context, string name)
        {
            WorkflowDataContext dc = context.DataContext;
            foreach (object p in dc.GetProperties())
            {
                if (((PropertyDescriptor) p).Name.Equals(name))
                {
                    string idValue = ((PropertyDescriptor) p).GetValue(dc).ToString();
                    return idValue;
                }
            }
            return null;
        }

        public static bool SetContextValue(NativeActivityContext context, string name, string value)
        {
            WorkflowDataContext dc = context.DataContext;
            foreach (object p in dc.GetProperties())
            {
                if (((PropertyDescriptor) p).Name.Equals(name))
                {
                    ((PropertyDescriptor) p).SetValue(dc, value);
                    return true;
                }
            }
            return false;
        }

        public static void SetReturnMessage(NativeActivityContext context, string message)
        {
            SetContextValue(context, "returnMessage", message);
        }

        public static void SetReturnMessage(NativeActivityContext context, Result r)
        {
            SetContextValue(context, "returnMessage", r.ToString());
        }
    }
}