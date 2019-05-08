using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.Utility;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_GameProfilesLoadedAsync;
        }

        private async void MainPage_GameProfilesLoadedAsync(object sender, RoutedEventArgs e)
        {
            GameProfilesList.SelectionMode = ListViewSelectionMode.None;

            listViewProgressBar.Visibility = Visibility.Visible;

            await ViewModel.LoadGameProfilesFromDBAsync();
            listViewProgressBar.Visibility = Visibility.Collapsed;

            await ViewModel.LoadChallengesFromDBAsync();

            GameProfilesList.SelectionMode = ListViewSelectionMode.Single;
        }

        private void GameProfilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListItemMainPage listItem = ((sender as ListView).SelectedItem as ListItemMainPage);
            try
            {
                ViewModel.PutChallengesInLists(listItem.GameProfile);
                ChangeVisibility();
            }
            catch (NullReferenceException)
            {
                ViewModel.AudienceChallenges.Clear();
            }
        }

        private void ChangeVisibility()
        {
            foreach (PivotItem item in ChallengeList.Items)
            {
                Grid grid = item.Content as Grid;
                if ((grid.Children[0] as ListView).Items.Count == 0)
                {
                    (grid.Children[0] as ListView).Visibility = Visibility.Collapsed;
                    (grid.Children[1] as TextBlock).Visibility = Visibility.Visible;
                }
            }
        }
    }
}
