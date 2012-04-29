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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ActivityLib;
using Common;

namespace Editor
{
    public partial class MainWindow : Window
    {
        private void InitWorkflowStyle()
        {
            string style = Configuration.Settings("WorkflowStyle", "TestSuite");
            if (_workflowDesigner != null)
            {
                //TODO how to clean the current workflow designer ???
            }
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
            const string categoryTitle = "HaPa Automation";
            LoadCustomActivities(tbc, customAss, categoryTitle);

            //support the users create there own automation activities, just add the dll name to appsettings
            string ass = Configuration.Settings("Assemmblies", "ActivityLib.dll"); //ConfigurationManager.AppSettings["Assemblies"];
            var split = new[] { ';' };
            string[] asses = ass.Split(split);
            foreach (string a in asses)
            {
                if (!string.IsNullOrWhiteSpace(a))
                    LoadCustomActivities(tbc, Assembly.LoadFrom(a), "Custom Activities");
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

            var primary = new ToolboxCategory("Native Activities");

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
                | ShellBarItemVisibility.Zoom | ShellBarItemVisibility.Arguments | ShellBarItemVisibility.Variables;
        }
    }
}
