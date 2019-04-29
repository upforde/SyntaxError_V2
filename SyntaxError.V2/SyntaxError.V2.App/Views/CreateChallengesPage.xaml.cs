using System;

using SyntaxError.V2.App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SyntaxError.V2.App.Views
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
