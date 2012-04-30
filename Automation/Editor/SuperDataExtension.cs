using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ActivityLib;
using Common;

namespace Editor
{
    public static class SuperDataExtension
    {
        public static TreeViewItem GetTreeViewItem(this SuperData superData)
        {
            TreeViewItem tvi = new TreeViewItem();
            var head = new StackPanel { Orientation = System.Windows.Controls.Orientation.Horizontal };

            var text = new TextBlock();
            text.Text = superData.DisplayName;
            text.ToolTip = new ToolTip { Content = "Need further programming" };
            
            BitmapImage bitmap = ImageList.GetInstance().Get(superData.DisplayIcon);
            
            if (bitmap != null)
            {
                // this is important, remove it, the tree will be 20 times slower
                
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Normal, (System.Action)(() =>
                {
                    Image image = new Image
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
            head.Children.Add(text);
            tvi.Header = head;
            return tvi;
        }
    }
}
