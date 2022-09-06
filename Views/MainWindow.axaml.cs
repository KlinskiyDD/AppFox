using AppFox.ViewModels;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace AppFox.Views
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContextChanged += SetDataContextMethod;
        }

        private void SetDataContextMethod(object? sender, EventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.Height = this.Screens.Primary.Bounds.Height;
                ViewModel.Width = this.Screens.Primary.Bounds.Width;
            }
        }
    }
}