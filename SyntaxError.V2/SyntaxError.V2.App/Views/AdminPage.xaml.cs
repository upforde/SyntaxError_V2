using System;
using System.Linq;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class AdminPage : Page
    {
        public AdminViewModel ViewModel { get; } = new AdminViewModel();
        public GamePage GamePage;

        public AdminPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GamePage = (GamePage) e.Parameter;
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.AddToOtherList("Button clicked!");
        }
    }
}
