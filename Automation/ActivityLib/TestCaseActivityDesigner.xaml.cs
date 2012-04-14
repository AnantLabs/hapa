using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Activities.Presentation;
using System.Activities;

namespace ActivityLib
{
    // Interaction logic for TestCaseActivityDesigner.xaml
    public partial class TestCaseActivityDesigner
    {
        public TestCaseActivityDesigner()
        {
            InitializeComponent();
            
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            if (DragDropHelper.AllowDrop(e.Data, this.Context, typeof(Activity)))
            {
                e.Effects = DragDropEffects.Move & e.AllowedEffects;
                e.Handled = true;
            }
            base.OnDragEnter(e);
        }
        
    }
}
