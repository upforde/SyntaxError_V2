using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.UI.Controls;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        public bool IsConnected;

        public MainPage()
        {
            InitializeComponent();
            
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            Loaded += MainPage_GameProfilesLoadedAsync;
        }

        private async void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MainPage_GameProfilesLoadedAsync());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ChangeButtonsEnabled(e.IsAvailable));
        }

        private void ChangeButtonsEnabled(bool access)
        {
            if (!access)
                listViewProgressBar.Visibility = Visibility.Collapsed;
            PlayButton.IsEnabled = access;
            AddButton.IsEnabled =  access;
            DeleteButton.IsEnabled = access;
        }

        /// <summary>  Loads the GameProfiles asyncronous.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void MainPage_GameProfilesLoadedAsync(object sender = null, RoutedEventArgs e = null)
        {
            var isInternetAvailable = NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
            if (isInternetAvailable)
            {
                GameProfilesList.SelectionMode = ListViewSelectionMode.None;

                listViewProgressBar.Visibility = Visibility.Visible;

                try
                {
                    await ViewModel.LoadGameProfilesFromDBAsync();
                } catch (System.Net.Http.HttpRequestException) { }

                listViewProgressBar.Visibility = Visibility.Collapsed;

                try
                {
                    await ViewModel.LoadChallengesFromDBAsync();
                } catch (System.Net.Http.HttpRequestException) { }
                
                GameProfilesList.SelectionMode = ListViewSelectionMode.Single;

                try
                {
                    await ViewModel.LoadMediaObjectsFromDBAsync();
                } catch (System.Net.Http.HttpRequestException) { } catch (InvalidOperationException) { }
            }
            ChangeButtonsEnabled(isInternetAvailable);
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
            var count = (audience.Children[0] as ListView).Items.Count;
            (audience.Children[0] as ListView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (audience.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;
            
            var crew = (ChallengeList.Items[1] as PivotItem).Content as Grid;
            count = (crew.Children[0] as ListView).Items.Count;
            (crew.Children[0] as ListView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (crew.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var multiple = (ChallengeList.Items[2] as PivotItem).Content as Grid;
            count = (multiple.Children[0] as ListView).Items.Count;
            (multiple.Children[0] as ListView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (multiple.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var music = (ChallengeList.Items[3] as PivotItem).Content as Grid;
            count = (music.Children[0] as ListView).Items.Count;
            (music.Children[0] as ListView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (music.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var quiz = (ChallengeList.Items[4] as PivotItem).Content as Grid;
            count = (quiz.Children[0] as ListView).Items.Count;
            (quiz.Children[0] as ListView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (quiz.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var screenshot = (ChallengeList.Items[5] as PivotItem).Content as Grid;
            count = (screenshot.Children[0] as ListView).Items.Count;
            (screenshot.Children[0] as ListView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (screenshot.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var silouette = (ChallengeList.Items[6] as PivotItem).Content as Grid;
            count = (silouette.Children[0] as ListView).Items.Count;
            (silouette.Children[0] as ListView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
            (silouette.Children[1] as TextBlock).Visibility = (count == 0) ? Visibility.Visible: Visibility.Collapsed;

            var sologame = (ChallengeList.Items[7] as PivotItem).Content as Grid;
            count = (sologame.Children[0] as ListView).Items.Count;
            (sologame.Children[0] as ListView).Visibility = (count == 0) ? Visibility.Collapsed: Visibility.Visible;
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
            ViewModel.AddGameProfileCommand.Execute(newProfile);
        }

        private async void AppBarButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            var gameProfileWithObjects = new GameObjectForSending
            {
                GameProfile = (GameProfilesList.SelectedItem as ListItemMainPage).GameProfile,
                Challenges = new List<ListItemMainPage>()
            };

            if (ViewModel.AudienceChallenges != null) gameProfileWithObjects.Challenges.AddRange(ViewModel.AudienceChallenges);
            if (ViewModel.CrewChallenges != null) gameProfileWithObjects.Challenges.AddRange(ViewModel.CrewChallenges);
            if (ViewModel.MultipleChoiceChallenges != null) gameProfileWithObjects.Challenges.AddRange(ViewModel.MultipleChoiceChallenges);
            if (ViewModel.MusicChallenges != null) gameProfileWithObjects.Challenges.AddRange(ViewModel.MusicChallenges);
            if (ViewModel.QuizChallenges != null) gameProfileWithObjects.Challenges.AddRange(ViewModel.QuizChallenges);
            if (ViewModel.ScreenshotChallenges != null) gameProfileWithObjects.Challenges.AddRange(ViewModel.ScreenshotChallenges);
            if (ViewModel.SilhouetteChallenges != null) gameProfileWithObjects.Challenges.AddRange(ViewModel.SilhouetteChallenges);
            if (ViewModel.SologameChallenges != null) gameProfileWithObjects.Challenges.AddRange(ViewModel.SologameChallenges);

            try{
                CoreApplicationView newView = CoreApplication.CreateNewView();
                int newViewId = 0;
                await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Frame frame = new Frame();
                    frame.Navigate(typeof(GamePage), gameProfileWithObjects);
                    Window.Current.Content = frame;
                    Window.Current.Activate();

                    newViewId = ApplicationView.GetForCurrentView().Id;
                });
                bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
            } catch (NullReferenceException){}
        }
    }
}
