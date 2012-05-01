using System.Activities.Presentation.PropertyEditing;
using System.ComponentModel;
using System.Xml.Linq;

namespace ActivityLib
{
    public abstract class LeafAction : Action
    {
        [DisplayName(@"GUI Object")]
        [Editor(typeof (TreeNodePicker), typeof (DialogPropertyValueEditor))]
        public string UIObject { get; set; }

        public XElement FindGuiObjectInDataContext(string content)
        {
            //return CurrentDataContext.GetElement(content);
            return null;
        }
    }
}