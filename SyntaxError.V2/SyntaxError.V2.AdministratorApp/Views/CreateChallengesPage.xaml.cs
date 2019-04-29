using System;

using SyntaxError.V2.AdministratorApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SyntaxError.V2.AdministratorApp.Views
{
    public sealed partial class CreateChallengesPage : Page
    {
        public CreateChallengesViewModel ViewModel { get; } = new CreateChallengesViewModel();

        public CreateChallengesPage()
        {
            InitializeComponent();
        }
    }
}
