using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace Editor
{
    public partial class MainWindow : Window
    {
        private static void DragTreeviewItem(MouseEventArgs e, Vector diff, string myformat)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Get the dragged ListViewItem

                var listViewItem =
                    FindAnchestor<TreeViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem == null)
                    return;
                // Find the data behind the ListViewItem
                //XElement xe = ((XElementTreeViewItem)listViewItem).Element;
                XElement xe = null;
                // Initialize the drag & drop operation
                var dragData = new DataObject(myformat, xe);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }

        private static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            } while (current != null);
            return null;
        }

        private void GuiObjectTreePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }

        private void GuiObjectTreePreviewMouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            DragTreeviewItem(e, diff, "GUIFormat");
        }

    }
}
