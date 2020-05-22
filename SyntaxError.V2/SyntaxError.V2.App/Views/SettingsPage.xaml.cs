using SyntaxError.V2.App.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel { get; } = new SettingsViewModel();
        /// <summary>The syntax error maximum value</summary>
        public static int _syntaxErrorMaxVal = 25;

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.InitializeAsync();
        }

        /// <summary>Handles the TextChanged event of the SyntaxErrorMaxValue control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void SyntaxErrorMaxValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            _syntaxErrorMaxVal = (int.TryParse(syntaxErrorMaxValue.Text, out int result))?result:25;
        }
    }
}
