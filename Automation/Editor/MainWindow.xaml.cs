using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Metadata;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

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
            AddTestDesigner("TestSuite");

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
