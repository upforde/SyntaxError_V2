using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.UI.Controls;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
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
                ChallengesCVS.Source = GetChallengesGrouped();
                PlayButton.IsEnabled = true;
            }
            catch (NullReferenceException)
            {
                ViewModel.GameProfileChallenges.Clear();
            }
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
            if((GameProfilesList.SelectedItem as ListItemMainPage) == null) return;

            var gameProfileWithObjects = new ListItemMainPage
            {
                GameProfile = (GameProfilesList.SelectedItem as ListItemMainPage).GameProfile,
                Challenges = ViewModel.GameProfileChallenges
            };
            
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
        
        public ObservableCollection<GroupChallengesList> GetChallengesGrouped()
        {
            var query = from item in ViewModel.GameProfileChallenges
            group item by item.Challenge.GetDiscriminator() into g
            orderby g.Key
            select new GroupChallengesList(g) { Key = g.Key };
    
            return new ObservableCollection<GroupChallengesList>(query);
        }
    }
}
