using System;

using SyntaxError.V2.App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class CreateObjectsPage : Page
    {
        public CreateObjectsViewModel ViewModel { get; } = new CreateObjectsViewModel();

        public CreateObjectsPage()
        {
            InitializeComponent();
        }
    }
}
