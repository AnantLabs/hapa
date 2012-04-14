using System;
using System.Activities;
using System.ComponentModel;
using System.Xml.Linq;


namespace ActivityLib
{
    //[ToolboxBitmap(typeof(Click),"Resources.Click.bmp"]
    public class Click : LeafAction
    {
        // Define an activity input argument of type string

        [DefaultValue(false)]
        public bool DoubleClick { get; set; }

        [DefaultValue(false)]
        public bool RightClick { get; set; }

        [DefaultValue(false)]
        public bool HoldControl { get; set; }

        [DefaultValue(false)]
        public bool HoldShift { get; set; }

        [DefaultValue(false)]
        public bool HoldAlt { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(NativeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
        }

        //public override XElement SetData(ActivityContext context, DataContext currentDataContext)
        //{
        //    //UIObject is the name of the object we want to click, we store it in the currentDataContext
        //    //get its value at runtime, then we can set our Xelement at this moment
        //    XElement xContent;
        //    if (Id != null)
        //        xContent = new XElement("Click",
        //                                new XAttribute(ConstString.AttributeId, Id),
        //                                new XAttribute("InstanceId", InstanceId),
        //                                new XAttribute("DoubleClick", DoubleClick),
        //                                new XAttribute("RightClick", RightClick),
        //                                new XAttribute("HoldControl", HoldControl),
        //                                new XAttribute("HoldShift", HoldShift),
        //                                new XAttribute("HoldAlt", HoldAlt)
        //            );
        //    else
        //        throw new Exception("Action Id is null. Fatal Error!");
        //    XElement guiObject = FindGuiObjectInDataContext(UIObject);

        //    xContent.Add(guiObject);
        //    return xContent;
        //}
    }
}