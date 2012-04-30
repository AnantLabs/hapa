using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using Common;


namespace ActivityLib
{
    public class ActionDesignerBase : ActivityDesigner
    {
        protected void SetIconInDesigner(object newItem)
        {
            if (newItem is ModelItem)
            {
                string iconName = ((ModelItem)newItem).ItemType.Name;
                Icon = new DrawingBrush
                           {
                               Drawing = new ImageDrawing
                                             {
                                                 Rect = new Rect(0, 0, FontSize, FontSize),
                                                 ImageSource = ImageList.GetInstance().Get(iconName)
                                             }
                           };
            }
        }

        protected override void OnModelItemChanged(object newItem)
        {
            base.OnModelItemChanged(newItem);
            //Get IconName, if it is null, don't set the icon
            SetIconInDesigner(newItem);
        }

        protected void ReloadTree(TreeView tree, XElementTreeViewItem item)
        {
            if (tree == null)
                return;
            //tree.Items.Clear();
            //tree.Items.Add(item);
        }

        /// <summary>
        /// keep it now, maybe will use it someday
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="item"></param>
        protected void ReloadTree(TreeView tree, XElement item)
        {
            var treeItem = new XElementTreeViewItem(item);
            ReloadTree(tree, treeItem);
        }

        //protected override void OnDragEnter(DragEventArgs e)
        //{
        //
        //    string allowDropType = this.ModelItem.Properties["AllowDropItemType"].Value.ToString();
        //    string draggedDataFormat = e.Data.GetFormats()[0];
        //    if (Regex.IsMatch(draggedDataFormat, allowDropType))
        //    {
        //        e.Effects = DragDropEffects.Move & e.AllowedEffects;
        //        e.Handled = true;

        //    }
        //    if (DragDropHelper.AllowDrop(e.Data, Context, typeof (Activity)))
        //    {
        //        e.Effects = DragDropEffects.Move & e.AllowedEffects;
        //        e.Handled = true;

        //    }
        //    base.OnDragEnter(e);
        //}

        protected void TreeViewDropped(DragEventArgs e, string format, string fieldName, TreeView tree)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                return;
            string id = GetDroppedItemId(e, format);
            if (string.IsNullOrWhiteSpace(id))
                return;

            if (ModelItem != null)
                if (ModelItem.Properties[fieldName] != null)
                {
                    // ReSharper disable PossibleNullReferenceException
                    ModelItem.Properties[fieldName].SetValue(id);
                    // ReSharper restore PossibleNullReferenceException
                    // use typeconverter to replace this, but keep it now, maybe someday will need it:
                    //ReloadTree(tree, xe);
                }


            //TODO for test, remove it later
            ModelItem myItem = this.ModelItem;
            do
            {
                myItem = myItem.Parent;
            }
            while (myItem.Parent.ItemType != typeof(TestSuite));

            myItem.Parent.Properties["Activities"].Collection.Add(new TestSuite());





            //if (e.Data.GetDataPresent(format))
            //{
            //    var xe = e.Data.GetData(format) as XElement;
            //    if (null != xe)
            //    {
            //        if (xe.Attribute(ConstString.AttributeId)==null)
            //            return;
            //        // ReSharper disable PossibleNullReferenceException
            //        string id = xe.Attribute(ConstString.AttributeId).Value;

            //        if (string.IsNullOrWhiteSpace(id))
            //            return;
            //
            //        if (ModelItem != null) if (ModelItem.Properties[fieldName] != null)
            //        {
            //            ModelItem.Properties[fieldName].SetValue(id);
            //            // use typeconverter to replace this, but keep it now, maybe someday will need it:
            //            //ReloadTree(tree, xe);
            //        }
            //        // ReSharper restore PossibleNullReferenceException
            //    }
            //}
        }

        private static string GetDroppedItemId(DragEventArgs e, string format)
        {
            if (e.Data.GetDataPresent(format))
            {
                var xe = e.Data.GetData(format) as XElement;
                if (null != xe)
                {
                    if (xe.Attribute(Const.AttributeId) == null)
                        return null;

                    // ReSharper disable PossibleNullReferenceException
                    string id = xe.Attribute(Const.AttributeId).Value;
                    // ReSharper restore PossibleNullReferenceException
                    return id;
                }
            }
            return null;
        }

        protected static void TreeViewDragEntered(DragEventArgs e, object sender, string format)
        {
            //only the item with Id field can be drag and drop
            if (string.IsNullOrWhiteSpace(GetDroppedItemId(e, format)))
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.All;
            }
            e.Handled = true;
        }
    }
}