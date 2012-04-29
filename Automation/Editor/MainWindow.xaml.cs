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

        #region Main Menu

        private void FileMenuSubmenuOpened(object sender, RoutedEventArgs e)
        {
            //if (ChangeManager.GetInstance().IsChanged())
            //    SaveMenuItem.IsEnabled = true;
            //else
            //    SaveMenuItem.IsEnabled = false;
        }

        private void ClickExitButton(object sender, RoutedEventArgs e)
        {
            //if (ChangeManager.GetInstance().IsChanged())
            //{
            //    string message = "Some Changes has not been saved! ";

            //    message += "\nAre you sure?";
            //    if (MessageBox.Show(message, "Exit Without Saved?", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
            //        MessageBoxResult.No)
            //        return;
            //}

            Close();
        }

        private void ClickSaveButton(object sender, RoutedEventArgs e)
        {
            //if (!ChangeManager.GetInstance().IsChanged())
            //{
            //    MessageBox.Show("Nothing Changed.");
            //    return;
            //}
            StartProgressBar();
            //ChangeManager.GetInstance().Save();
            StopProgressBar();
        }

        private void SaveAnElement(XElement rootXe)
        {
            //XAttribute idAttr = rootXe.Attribute(ConstString.AttributeId);
            //if (idAttr != null)
            //{
            //    string id = idAttr.Value;
            //    string content = rootXe.ToString(SaveOptions.None);
            //    _client.UpdateData(id, content);
            //}
        }

        private void ClickOpenButton(object sender, RoutedEventArgs e)
        {
            //TODO load the workflow from server
        }

        private void ClickNewButton(object sender, RoutedEventArgs e)
        {
            //TODO just for test now, will update to more complex logic

            string fileName = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetTempFileName());
            //var newTestSuite = new TestSuite { DisplayName = "new Test suite without name" };
            var newTestSuite = new Sequence();
            XamlServices.Save(fileName, newTestSuite);
            AddDesigner(fileName);
        }

        private void ClickUndoButton(object sender, RoutedEventArgs e)
        {
            redoMenu.IsEnabled = true;
            _workflowDesigner.Context.Services.GetService<UndoEngine>().Undo();
            if (_undoEngineService.GetUndoActions().Count() == 0)
                undoMenu.IsEnabled = false;
            if (_undoEngineService.GetRedoActions().Count() > 0)
                redoMenu.IsEnabled = true;
        }

        private void ClickRedoButton(object sender, RoutedEventArgs e)
        {
            _workflowDesigner.Context.Services.GetService<UndoEngine>().Redo();
            if (_undoEngineService.GetRedoActions().Count() == 0)
                redoMenu.IsEnabled = false;
            if (_undoEngineService.GetUndoActions().Count() > 0)
                undoMenu.IsEnabled = true;
        }

        #endregion Main Menu

        private void ProjectContextMenuOpened(object sender, RoutedEventArgs e)
        {
            //ContextMenuOpened(ProjectTreeView);
        }

        private void DataContextMenuOpened(object sender, RoutedEventArgs e)
        {
            //ContextMenuOpened(DataTree);
        }

        private void GuiObjectsContextMenuOpened(object sender, RoutedEventArgs e)
        {
            //ContextMenuOpened(GuiObjectTree);
        }

        private void ProjectTreeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //DoubleClickOnTree(ProjectTreeView);
        }

        private void DataTreeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //DoubleClickOnTree(DataTree);
        }

        private void GuiTreeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //DoubleClickOnTree(GuiObjectTree);
        }

        private void ProjectTreeMenuItemClicked(object sender, RoutedEventArgs e)
        {
            var tag = (string)((MenuItem)sender).Tag;
            TreeView currentTree = ProjectTreeView;

            if (currentTree.SelectedItem == null)
                return;
            //UpdateAndDeleteAndReload(currentTree, tag);

            //if (!tag.Equals("Delete") && !tag.Equals("Update") && !tag.Equals("Reload"))
            //{
            //    XElement toCreatedXe;
            //    if (tag.Equals("Project"))
            //        toCreatedXe = _template.CreateProject(_userName);
            //    else
            //        toCreatedXe = _template.GetInitalObject(tag, _userName);

            //    if (tag.Equals("TestSuite")
            //        || tag.Equals("TestCase")
            //        || tag.Equals("TestSteps"))
            //    {
            //        CreateActivity(toCreatedXe);
            //    }

            //    var tvi = new XElementTreeViewItem(toCreatedXe);
            //    ((XElementTreeViewItem)currentTree.SelectedItem).Add(tvi.GetSimpleVersion());
            //    ChangeManager.GetInstance().Changed(toCreatedXe);
            //    ChangeManager.GetInstance().Changed(GetNearestIndependentParent((TreeViewItem)currentTree.SelectedItem).Element);
            //}

            //currentTree.UpdateLayout();
        }

        private void DataTreePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }

        private void DataTreePreviewMouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            System.Windows.Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            DragTreeviewItem(e, diff, "DataFormat");
        }

        private void GuiObjectsTreeMenuItemClicked(object sender, RoutedEventArgs e)
        {
            var tag = (string)((MenuItem)sender).Tag;
            TreeView currentTree = GuiObjectTree;
            if (currentTree.SelectedItem == null)
                return;
            //UpdateAndDeleteAndReload(currentTree, tag);

            //if (!tag.Equals("Delete") && !tag.Equals("Update") && !tag.Equals("Reload"))
            //{
            //    XElement toCreateXe = AddNodeByTemplete(currentTree, tag);
            //}
        }

        private void DataTreeMenuItemClicked(object sender, RoutedEventArgs e)
        {
            var tag = (string)((MenuItem)sender).Tag;
            TreeView currentTree = DataTree;
            if (currentTree.SelectedItem == null)
                return;
            //UpdateAndDeleteAndReload(currentTree, tag);

            //if (!tag.Equals("Delete") && !tag.Equals("Update") && !tag.Equals("Reload"))
            //{
            //    XElement toCreateXe = AddNodeByTemplete(currentTree, tag);
            //}
        }

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

        private void InitWorkflowStyle()
        {
            string style = Configuration.Settings("WorkflowStyle","TestSuite");
            if (style.Equals("Sequence"))
            {
                _workflowDesigner.Load(new Sequence());
                return;
            }
            if (style.Equals("FlowChart"))
            {
                _workflowDesigner.Load(new Flowchart());
                return;
            }
            if (style.Equals("TestSuite"))
            {
                _workflowDesigner.Load(new ActivityLib.TestSuite());
                return;
            }
            //State machine is too different to others, it require special tracker ... etc, so we don't use it now
            if (style.Equals("StateMachine"))
            {
                //_workflowDesigner.Load(new StateMachine()); 
            }

            _workflowDesigner.Load(new Sequence());
        }

        protected void LoadToolBox()
        {
            var tbc = new ToolboxControl();
            ToolboxBorder.Child = tbc;
            tbc.CategoryItemStyle = new Style(typeof(TreeViewItem))
            {
                Setters =
                                                {
                                                    new Setter(TreeViewItem.IsExpandedProperty, false)
                                                }
            };
            LoadDefaultActivities(tbc);
            //Load the dll contain TestSuite, it means the ActivityLib.dll loaded here
            Assembly customAss = typeof(ActivityLib.TestSuite).Assembly;
            const string categoryTitle = "Built-in Automation";
            LoadCustomActivities(tbc, customAss, categoryTitle);

            //support the users create there own automation activities, just add the dll name to appsettings
            string ass = Configuration.Settings("Assemmblies", "ActivityLib.dll"); //ConfigurationManager.AppSettings["Assemblies"];
            var split = new[] { ';' };
            string[] asses = ass.Split(split);
            foreach (string a in asses)
            {
                if (!string.IsNullOrWhiteSpace(a))
                    LoadCustomActivities(tbc, Assembly.LoadFrom(a), "Custom Automation");
            }
        }

        private static void LoadCustomActivities(ToolboxControl tbc, Assembly customAss, string categoryTitle)
        {
            IEnumerable<Type> types = customAss.GetTypes().
                Where(t => (typeof(Activity).IsAssignableFrom(t) ||
                            typeof(IActivityTemplateFactory).IsAssignableFrom(t)) && !t.IsAbstract && t.IsPublic &&
                           !t.IsNested && HasDefaultConstructor(t));
            var cat = new ToolboxCategory(categoryTitle);
            foreach (Type type in types.OrderBy(t => t.Name))
            {
                //var w = new ToolboxItemWrapper(type, ToGenericTypeString(type));
                var w = new ToolboxItemWrapper(type, ImageList.GetInstance().GetFileName(type.Name.ToLower()), type.Name);
                cat.Add(w);
            }

            tbc.Categories.Add(cat);
        }

        private void LoadDefaultActivities(ToolboxControl tbc)
        {
            var dict = new ResourceDictionary
            {
                Source =
                    new Uri(
                    "pack://application:,,,/System.Activities.Presentation;component/themes/icons.xaml")
            };
            Resources.MergedDictionaries.Add(dict);

            IEnumerable<Type> standtypes = typeof(Activity).Assembly.GetTypes().
                Where(t => (typeof(Activity).IsAssignableFrom(t) ||
                            typeof(IActivityTemplateFactory).IsAssignableFrom(t)) && !t.IsAbstract && t.IsPublic &&
                           !t.IsNested && HasDefaultConstructor(t));

            var primary = new ToolboxCategory("Microsoft Primary");

            foreach (Type type in standtypes.OrderBy(t => t.Name))
            {
                var w = new ToolboxItemWrapper(type, ToGenericTypeString(type));
                if (AddIcon(type, _builder))
                {
                    //secondary.Add(w);
                }
                else
                {
                    primary.Add(w);
                }
            }

            MetadataStore.AddAttributeTable(_builder.CreateTable());
            tbc.Categories.Add(primary);
        }

        protected bool AddIcon(Type type, AttributeTableBuilder builder)
        {
            bool secondary = false;

            Type tbaType = typeof(ToolboxBitmapAttribute);
            Type imageType = typeof(System.Drawing.Image);
            ConstructorInfo constructor = tbaType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null,
                                                                 new[] { imageType, imageType }, null);

            string resourceKey = type.IsGenericType ? type.GetGenericTypeDefinition().Name : type.Name;
            int index = resourceKey.IndexOf('`');
            if (index > 0)
            {
                resourceKey = resourceKey.Remove(index);
            }
            if (resourceKey == "Flowchart")
            {
                resourceKey = "FlowChart"; // it appears that themes/icons.xaml has a typo here
            }
            resourceKey += "Icon";
            Bitmap small, large;
            object resource = TryFindResource(resourceKey);
            if (!(resource is DrawingBrush))
            {
                resource = FindResource("GenericLeafActivityIcon");
                secondary = true;
            }
            var dv = new DrawingVisual();
            using (DrawingContext context = dv.RenderOpen())
            {
                context.DrawRectangle(((DrawingBrush)resource), null, new Rect(0, 0, 32, 32));
                context.DrawRectangle(((DrawingBrush)resource), null, new Rect(32, 32, 16, 16));
            }
            var rtb = new RenderTargetBitmap(32, 32, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(rtb));
                enc.Save(outStream);
                outStream.Position = 0;
                large = new Bitmap(outStream);
            }
            rtb = new RenderTargetBitmap(16, 16, 96, 96, PixelFormats.Pbgra32);
            dv.Offset = new Vector(-32, -32);
            rtb.Render(dv);
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(rtb));
                enc.Save(outStream);
                outStream.Position = 0;
                small = new Bitmap(outStream);
            }
            var tba = constructor.Invoke(new object[] { small, large }) as ToolboxBitmapAttribute;
            builder.AddCustomAttributes(type, tba);

            return secondary;
        }

        public static string ToGenericTypeString(Type t)
        {
            if (!t.IsGenericType)
                return t.Name;

            string genericTypeName = t.GetGenericTypeDefinition().Name;
            genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));

            string genericArgs = string.Join(",", t.GetGenericArguments().Select(ToGenericTypeString));
            return genericTypeName + "<" + genericArgs + ">";
        }

        public static bool HasDefaultConstructor(Type t)
        {
            return t.GetConstructors().Where(c => c.GetParameters().Length <= 0).Count() > 0;
        }


        private void UndoEngineServiceUndoUnitAdded(object sender, UndoUnitEventArgs e)
        {
            undoMenu.IsEnabled = true;
        }

        private static void RegisterMetadata()
        {
            var metaData = new DesignerMetadata();
            metaData.Register();
        }

        private void AddDesigner(string fileName)
        {
            //Create an instance of WorkflowDesigner class
            _workflowDesigner = new WorkflowDesigner();

            //Place the WorkflowDesigner in the middle column of the grid

            Grid.SetColumn(_workflowDesigner.View, 1);

            if (string.IsNullOrWhiteSpace(fileName))
                InitWorkflowStyle();
            else
                _workflowDesigner.Load(fileName);
            //Add the WorkflowDesigner to the grid
            DesignerBorder.Child = _workflowDesigner.View;
            //grid1.Children.Add(this.workflowDesigner.View);

            // Add the Property Inspector
            UIElement propertyView = _workflowDesigner.PropertyInspectorView;

            PropertyBorder.Child = propertyView;

            // Flush the workflow when the model changes
            _workflowDesigner.ModelChanged += (s, e) =>
            {
                _workflowDesigner.Flush();
                debugInfo.Text = _workflowDesigner.Text;
                //var action = (ActivityLib.Action)XamlServices.Parse(_workflowDesigner.Text);
                //ChangeManager.GetInstance().Changed(_currentActivityId, _workflowDesigner.Text);
            };
            // services
            _undoEngineService = _workflowDesigner.Context.Services.GetService<UndoEngine>();
            _undoEngineService.UndoUnitAdded += UndoEngineServiceUndoUnitAdded;

            var designerView = _workflowDesigner.Context.Services.GetService<DesignerView>();
            //hide the shell bar of designer
            //designerView.WorkflowShellBarItemVisibility = ShellBarItemVisibility.None;
            designerView.WorkflowShellBarItemVisibility = ShellBarItemVisibility.MiniMap 
                | ShellBarItemVisibility.Zoom  | ShellBarItemVisibility.Arguments | ShellBarItemVisibility.Variables ;
        }
    }
}
