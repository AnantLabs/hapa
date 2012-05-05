using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Metadata;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using Common;
using MongoDB;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AttributeTableBuilder _builder = new AttributeTableBuilder();
        private Point _startPoint;
        private UndoEngine _undoEngineService;
        private WorkflowDesigner _workflowDesigner = new WorkflowDesigner();

        public MainWindow()
        {
            InitializeComponent();
            StartProgressBar();
            
            LoadToolBox();
            RegisterMetadata();
            AddTestDesigner("TestSuite");

            InitializeProject();
            LoadProject();

            StopProgressBar();
        }

        private void LoadProject()
        {
            //load the project tree
            string projectId = Configuration.Settings("Project");
            SuperData project = SuperDataExtension.GetById(projectId);
            this.ProjectTreeView.Items.Add(project.GetTreeViewItem());
            //load the ui tree
            string dataId = Configuration.Settings("Data");
            SuperData data = SuperDataExtension.GetById(dataId);
            this.DataTree.Items.Add(data.GetTreeViewItem());
            //load the data tree
            String objectId = Configuration.Settings("Object");
            SuperData obj = SuperDataExtension.GetById(objectId);
            this.GuiObjectTree.Items.Add(obj.GetTreeViewItem());
            //TODO load the translation table

        }

        private void InitializeProject()
        {
            string projectId = CreateNewItem("Project", "0000000000000");
            CreateNewItem("Result", projectId);
            CreateNewItem("Data", projectId);
            CreateNewItem("Object", projectId);
            CreateNewItem("Translation", projectId);
        }
        
        private static string CreateNewItem(string itemNameInConfig, string parentId)
        {
            
            var mainId = SetIdInConfig(itemNameInConfig);

            SuperData superData = DB.GetInstance().Find<SuperData>(mainId);
            if (superData == null)
            {
                superData = new SuperData(mainId,parentId,itemNameInConfig);
                //superData = SuperData.CreateItem(itemNameInConfig);
                //superData.Id = mainId;
                //superData.ParentId = parentId;
                //superData.DisplayName = itemNameInConfig;
                superData.Save();
            }

            return mainId;
        }
        
        private static string SetIdInConfig(string itemNameInConfig)
        {
            string mainId = Configuration.Settings(itemNameInConfig, null);
            if (mainId == null)
            {
                string newId = Guid.NewGuid().ToString();
                Configuration.Set(itemNameInConfig, mainId);
                Configuration.SaveSettings();
            }
            return mainId;
        }

        #region Progress bar

        private void StartProgressBar()
        {
            Progressing.IsIndeterminate = true;
            Progressing.Visibility = Visibility.Visible;
            var duration = new Duration(TimeSpan.FromSeconds(1));
            var doubleanimation = new DoubleAnimation(10.0, duration);
            Progressing.BeginAnimation(RangeBase.ValueProperty, doubleanimation);
        }

        private void StopProgressBar()
        {
            Progressing.BeginAnimation(RangeBase.ValueProperty, null);
            Progressing.Visibility = Visibility.Collapsed;
        }

        #endregion Progress bar
    }
}