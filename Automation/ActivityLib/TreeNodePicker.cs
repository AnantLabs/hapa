using System.Activities.Presentation.Converters;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.PropertyEditing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace ActivityLib
{
    internal class TreeNodePicker : DialogPropertyValueEditor
    {
        public TreeNodePicker()
        {
            InlineEditorTemplate = new DataTemplate();

            var stack = new FrameworkElementFactory(typeof(StackPanel));
            stack.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            var label = new FrameworkElementFactory(typeof(Label));
            var labelBinding = new Binding("Value");
            label.SetValue(ContentControl.ContentProperty, labelBinding);
            //label.SetValue(Label.MaxWidthProperty, 90.0);

            stack.AppendChild(label);

            var editModeSwitch = new FrameworkElementFactory(typeof(EditModeSwitchButton));

            editModeSwitch.SetValue(EditModeSwitchButton.TargetEditModeProperty, PropertyContainerEditMode.Dialog);

            stack.AppendChild(editModeSwitch);

            InlineEditorTemplate.VisualTree = stack;
        }

        public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
        {
            var converter = new ModelPropertyEntryToOwnerActivityConverter();
            var modelItem = (ModelItem)converter.Convert(propertyValue.ParentProperty, typeof(ModelItem), false, null);
            if (modelItem != null)
            {
                //var xp = new XTreePicker();

                //string propertyName = propertyValue.ParentProperty.PropertyName;
                //if (propertyName.Equals("UIObject"))
                //{
                //    //xp.LoadContent(DataContextManager.GetInstance().GetDataContext("GUIData").Data);
                //}
                //else
                //{
                //    //xp.LoadContent(DataContextManager.GetInstance().GetDataContext("MainData").Data);
                //}

                ////TODO expend the tree to certain node
                //xp.ShowDialog();
                //if (xp.DialogResult == true)
                //{
                //    string pickPath = xp.GetSelectPath();
                //    if (propertyName.Equals("UIObject"))
                //    {
                //        //((LeafAction) modelItem.GetCurrentValue()).UIObject = pickPath;
                //        modelItem.Properties["UIObject"].SetValue(pickPath);
                //    }
                //    if (propertyName.Equals("Data"))
                //    {
                //        //((ActionSet) modelItem.GetCurrentValue()).Data = pickPath;
                //        modelItem.Properties["Data"].SetValue(pickPath);
                //    }
                //}
            }
        }
    }
}