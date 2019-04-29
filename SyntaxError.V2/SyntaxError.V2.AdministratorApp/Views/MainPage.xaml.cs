using System;

using SyntaxError.V2.AdministratorApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SyntaxError.V2.AdministratorApp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
