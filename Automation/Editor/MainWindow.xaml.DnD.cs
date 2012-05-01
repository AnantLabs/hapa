using System;
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
                    FindAnchestor<TreeViewItem>((DependencyObject) e.OriginalSource);
                if (listViewItem == null)
                    return;
                // Find the data behind the ListViewItem
                //XElement xe = ((XElementTreeViewItem)listViewItem).Element;
                XElement xe = XElement.Parse("<TestSuite Id=\"1234567890\" Name=\"JustForTestNow\" />");
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
                    return (T) current;
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
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            DragTreeviewItem(e, diff, "GUIFormat");
        }
    }
}