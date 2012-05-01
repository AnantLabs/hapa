using System.Activities;

namespace ActivityLib
{
    //[ToolboxBitmap(typeof(Click),"Resources.Click.bmp"]
    public class Select : LeafAction
    {
        public bool IsMultiSelect { get; set; }

        public string Options { set; get; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(NativeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
        }

        //public override XElement SetData(ActivityContext context, DataContext currentDataContext)
        //{
        //    //UIObject is the name of the object we want to select, we store it in the currentDataContext
        //    //get its value at runtime, then we can set our Xelement at this moment
        //    var xContent = new XElement("Select",
        //                                new XAttribute(ConstString.AttributeId, Id),
        //                                new XAttribute("InstanceId", InstanceId),
        //                                new XAttribute("Options", Options),
        //                                new XAttribute("IsDoubleClick", IsMultiSelect)
        //        );
        //    XElement guiObject = FindGuiObjectInDataContext(UIObject);

        //    xContent.Add(guiObject);
        //    return xContent;
        //}
    }
}