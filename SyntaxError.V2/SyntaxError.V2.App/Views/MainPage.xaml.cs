using Microsoft.Toolkit.Uwp.UI.Controls;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.Utility;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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

        /// <summary>  Loads the GameProfiles asyncronous.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void MainPage_GameProfilesLoadedAsync(object sender, RoutedEventArgs e)
        {
            GameProfilesList.SelectionMode = ListViewSelectionMode.None;

            listViewProgressBar.Visibility = Visibility.Visible;

            await ViewModel.LoadGameProfilesFromDBAsync();
            listViewProgressBar.Visibility = Visibility.Collapsed;

            await ViewModel.LoadChallengesFromDBAsync();

            GameProfilesList.SelectionMode = ListViewSelectionMode.Single;
        }

        /// <summary>Handles the SelectionChanged event of the GameProfilesList control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void GameProfilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListItemMainPage listItem = ((sender as ListView).SelectedItem as ListItemMainPage);
            try
            {
                ViewModel.PutChallengesInLists(listItem.GameProfile);
                ChangeVisibility();
                PlayButton.IsEnabled = true;
            }
            catch (NullReferenceException)
            {
                foreach (var item in ViewModel.ChallengeListList)
                    item.Clear();
                ChangeVisibility();
            }
        }

        /// <summary>Swaps the visibility of List and TextBlock that says 'no items'.</summary>
        private void ChangeVisibility()
        {
            var audience = (ChallengeList.Items[0] as PivotItem).Content as Grid;
            var count = (audience.Children[0] as AdaptiveGridView).Items.Count;
            (audience.Children[0] as AdaptiveGridView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (audience.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var crew = (ChallengeList.Items[1] as PivotItem).Content as Grid;
            count = (crew.Children[0] as AdaptiveGridView).Items.Count;
            (crew.Children[0] as AdaptiveGridView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (crew.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var multiple = (ChallengeList.Items[2] as PivotItem).Content as Grid;
            count = (multiple.Children[0] as ListView).Items.Count;
            (multiple.Children[0] as ListView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (multiple.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var music = (ChallengeList.Items[3] as PivotItem).Content as Grid;
            count = (music.Children[0] as AdaptiveGridView).Items.Count;
            (music.Children[0] as AdaptiveGridView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (music.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var quiz = (ChallengeList.Items[4] as PivotItem).Content as Grid;
            count = (quiz.Children[0] as ListView).Items.Count;
            (quiz.Children[0] as ListView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (quiz.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var screenshot = (ChallengeList.Items[5] as PivotItem).Content as Grid;
            count = (screenshot.Children[0] as AdaptiveGridView).Items.Count;
            (screenshot.Children[0] as AdaptiveGridView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (screenshot.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var silouette = (ChallengeList.Items[6] as PivotItem).Content as Grid;
            count = (silouette.Children[0] as AdaptiveGridView).Items.Count;
            (silouette.Children[0] as AdaptiveGridView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (silouette.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var sologame = (ChallengeList.Items[7] as PivotItem).Content as Grid;
            count = (sologame.Children[0] as AdaptiveGridView).Items.Count;
            (sologame.Children[0] as AdaptiveGridView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (sologame.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;
        }
        
        private void AppBarButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var newProfile = new GameProfile
            {
                GameProfileName = "New Profile",
                DateCreated = DateTime.Now,
                Profile = new Profile(),
                SaveGame = new SaveGame()
            };
            ViewModel.AdDGameProfileCommand.Execute(newProfile);
        }

        private void AppBarButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            var param = (GameProfilesList.SelectedItem as ListItemMainPage).GameProfile;

            Frame.Navigate(typeof(GamePage), param);
        }
    }
}
