using System;

using SyntaxError.V2.App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SyntaxError.V2.App.Views
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
