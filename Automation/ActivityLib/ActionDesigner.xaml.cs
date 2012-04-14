using System;
using System.Activities.Presentation.Model;
using System.Windows;
using System.Windows.Controls;

namespace ActivityLib
{
    // Interaction logic for TestCaseActivityDesigner.xaml

    public partial class ActionDesigner
    {
        public ActionDesigner()
        {
            InitializeComponent();
            Loaded += ActionDesignerLoaded;
        }

        //private void ModelItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "UIObject")
        //    {
        //        if (ModelItem.Properties["UIObject"].ComputedValue != null)
        //        {
        //            const string dataContextName = "GUIData";
        //            XElement xe = DataContextManager.GetInstance().GetDataContext(dataContextName).Data;
        //            this.i
        //        }
        //    }
        //}

        protected void ActionDesignerLoaded(object sender, RoutedEventArgs e)
        {
            if (ModelItem != null)
            {
                //ModelItem.PropertyChanged += ModelItem_PropertyChanged;
                //2 parent to get current action: target: get the parent data context
                ModelItem parent = ModelItem.Parent.Parent;
                if (parent != null)
                {
                    if (parent.ItemType.IsSubclassOf(typeof(ActionSet)))
                    {
                        //DataContext dataContext = ((Action)parent.GetCurrentValue()).CurrentDataContext;
                        //if (dataContext != null)
                        //{
                        //    ((Action)ModelItem.GetCurrentValue()).CurrentDataContext = dataContext;
                        //}
                    }
                    //else throw new Exception("Some strange things happen: we load the activity but its parent is not an actionset, please check ActivityLib.ActionDesigner!");
                }
                else
                {
                    throw new Exception(
                        "Some strange things happen: we load the activity but its parent is null, please check ActivityLib.ActionDesigner!");
                }
            }
            else
            {
                throw new Exception(
                    "Some strange things happen: we load the activity but modelitem is null, please check ActivityLib.ActionDesigner!");
            }
        }

        private void TreeViewDrop(object sender, DragEventArgs e)
        {
            TreeViewDropped(e, "GUIFormat", "UIObject", (TreeView)sender);
        }

        private void TreeViewDragEnter(object sender, DragEventArgs e)
        {
            TreeViewDragEntered(e, sender, "GUIFormat");
        }
    }
}