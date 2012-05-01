using System.Windows;
using System.Windows.Controls;

namespace ActivityLib
{
    // Interaction logic for TestSuiteActivityDesigner.xaml
    public partial class ActionSetDesigner
    {
        public ActionSetDesigner()
        {
            InitializeComponent();
        }

        private void TreeViewPreviewDragEnter(object sender, DragEventArgs e)
        {
            TreeViewDragEntered(e, sender, "DataFormat");
            //TreeViewDragEntered(e, sender, "ClientFormat");
        }

        private void TreeViewPreviewDrop(object sender, DragEventArgs e)
        {
            TreeViewDropped(e, "DataFormat", "Data", (TreeView) sender);
            //TreeViewDropped(e, "ClientFormat", "Client", (TreeView)sender);
        }
    }
}