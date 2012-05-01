using System;
using System.Windows;
using AutoClient;

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

        #region IAutoObserver Members

        public void update(string message)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SwitchBrowser(object sender, RoutedEventArgs e)
        {
            //Browser.GetInstance().SwitchToAnotherBrowser();
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
            LogPanel.Text = envs;
        }
    }
}