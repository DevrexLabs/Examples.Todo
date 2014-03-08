using System;
using System.Windows;

namespace Todo.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += (s, e) =>
                                {
                                    (this.DataContext as IDisposable).Dispose();
                                    App.Current.Shutdown();
                                };
        }
    }
}
