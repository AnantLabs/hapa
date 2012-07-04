using System;
using System.IO;
using System.Windows;
using AutoClient;
using Microsoft.Win32;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
            string envs = AutoClientManager.GetInstance().DoTest(LogPanel.Text);
            LogPanel.Text = envs;
        }

        private void SendResult(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OpenTestFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog().Value)
            {
                string fileName = openFile.FileName;
                string content = File.ReadAllText(fileName);
                LogPanel.Text = content;
            }
        }

        
    }
}