using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.Toolbox;
using System.Activities.Presentation.View;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.Xml.Linq;
using ActivityLib;
using Common;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AttributeTableBuilder _builder = new AttributeTableBuilder();
        private UndoEngine _undoEngineService;
        private WorkflowDesigner _workflowDesigner = new WorkflowDesigner();

        private System.Windows.Point _startPoint;

        public MainWindow()
        {
            InitializeComponent();
            StartProgressBar();
            LoadToolBox();
            RegisterMetadata();
            AddDesigner(null);

            StopProgressBar();
        }

       
        

        #region Progress bar

        void StartProgressBar()
        {
            Progressing.IsIndeterminate = true;
            Progressing.Visibility = Visibility.Visible;
            var duration = new Duration(TimeSpan.FromSeconds(1));
            var doubleanimation = new DoubleAnimation(10.0, duration);
            Progressing.BeginAnimation(RangeBase.ValueProperty, doubleanimation);
        }

        void StopProgressBar()
        {
            Progressing.BeginAnimation(RangeBase.ValueProperty, null);
            Progressing.Visibility = Visibility.Collapsed;
        }

        #endregion Progress bar

        
    }
}
