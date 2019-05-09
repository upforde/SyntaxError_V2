using System;
using Microsoft.Toolkit.Uwp.UI.Controls;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.ChallengeObjects;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class CreateObjectsPage : Page
    {
        public CreateObjectsViewModel ViewModel { get; } = new CreateObjectsViewModel();

        public CreateObjectsPage()
        {
            InitializeComponent();

            Loaded += LoadMediaObjectsFromDBAsync;
        }

        private async void LoadMediaObjectsFromDBAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadObjectsFromDBAsync();
        }

        private void AdaptiveGridView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var element = ((e.OriginalSource as FrameworkElement).DataContext as MediaObject);
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem firstItem = new MenuFlyoutItem
            {
                Text = "Edit",
                Icon = new FontIcon(){ Glyph = "" },
                Command = ViewModel.ItemClickCommand,
                CommandParameter = element
            };
            myFlyout.Items.Add(firstItem);
            
            if (element != null)
                myFlyout.ShowAt((sender as UIElement), e.GetPosition(sender as UIElement));
        }
    }
}
