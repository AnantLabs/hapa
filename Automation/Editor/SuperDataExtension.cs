using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ActivityLib;
using ActivityLib.Activities;
using Common;
using MongoDB;
using Action = System.Action;

namespace Editor
{
    public static class SuperDataExtension
    {
        public static void Save(this SuperData superData)
        {
            DB.GetInstance().Save(superData);
        }

        public static void Delete(this SuperData superData)
        {
            DB.GetInstance().Delete<SuperData>(superData.Id);
        }

        public static TreeViewItem GetTreeViewItem(this SuperData superData)
        {
            var tvi = new TreeViewItem();
            var head = new StackPanel {Orientation = Orientation.Horizontal};

            var text = new TextBlock
                           {Text = superData.DisplayName, ToolTip = new ToolTip {Content = "Need further programming"}};

            AddImageToTreeViewItem(superData, head, text);
            head.Children.Add(text);
            tvi.Header = head;
            return tvi;
        }

        private static void AddImageToTreeViewItem(SuperData superData, StackPanel head, TextBlock text)
        {
            BitmapImage bitmap = ImageList.GetInstance().Get(superData.DisplayIcon);

            if (bitmap != null)
            {
                // this is important, remove it, the tree will be 20 times slower

                Dispatcher.CurrentDispatcher.Invoke(
                    DispatcherPriority.Normal,
                    (Action)(() =>
                    {
                        var image = new Image
                        {
                            Source = bitmap,
                            Stretch = Stretch.Uniform,
                            Width = text.FontSize,
                            Height = text.FontSize,
                            MinHeight = 16,
                            MinWidth = 16
                        };
                        string id = superData.Id;

                        //ToolTip = new ToolTip();
                        image.ToolTip = id;
                        head.Children.Add(image);
                    }
                             ));
            }
        }
    }
}