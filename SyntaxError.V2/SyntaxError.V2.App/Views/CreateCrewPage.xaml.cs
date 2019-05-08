using System;

using SyntaxError.V2.App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class CreateCrewPage : Page
    {
        public CreateCrewViewModel ViewModel { get; } = new CreateCrewViewModel();

        public CreateCrewPage()
        {
            InitializeComponent();
        }
    }
}
