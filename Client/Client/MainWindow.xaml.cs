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

using AutoClient;
using SeleniumActions;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAutoObserver
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SwitchBrowser(object sender, RoutedEventArgs e)
        {
            Browser.GetInstance().SwitchToAnotherBrowser();
        }

        private void GetUIObjects(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RequestCommand(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DoActions(object sender, RoutedEventArgs e)
        {
            string envs = AutoClientManager.GetInstance().DoTest();
            this.LogPanel.Text = envs;
        }

        public void update(string message)
        {
            
            throw new NotImplementedException();
        }
    }
}
