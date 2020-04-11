using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.UI.Controls;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class MainPage : Page
    {
        private ListItemMainPage _storedGameProfile;
        private ObservableCollection<GroupChallengesList> _challengesSource;

        public MainViewModel ViewModel { get; } = new MainViewModel();
        public bool IsConnected;

        private ICommand _editCommand;
        private ICommand _resetSaveCommand;
        /// <summary>A command to edit the selected GameProfile.</summary>
        /// <value>The edit command.</value>
        public ICommand EditCommand => _editCommand ?? (_editCommand = new RelayCommand<ListItemMainPage>(EditCommand_ItemClicked));
        public ICommand ResetSaveCommand => _resetSaveCommand ?? (_resetSaveCommand = new RelayCommand<ListItemMainPage>(ResetSaveCommand_ItemClicked));

        public MainPage()
        {
            InitializeComponent();

            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            Loaded += MainPage_GameProfilesLoadedAsync;
            ViewModel.RefreshSaveDone += ViewModel_RefreshSaveDone;
        }

        private async void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MainPage_GameProfilesLoadedAsync());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ChangeButtonsEnabled(e.IsAvailable));
        }

        private async void ViewModel_RefreshSaveDone(object sender, PropertyChangedEventArgs e)
        {
            ViewModel.PutChallengesInLists(_storedGameProfile.GameProfile);
            _challengesSource = await GetChallengesGrouped(ViewModel.GameProfileChallenges);
            ChallengesCVS.Source = _challengesSource;
        }

        private void ChangeButtonsEnabled(bool access)
        {
            if (!access)
            {
                listViewProgressBar.Visibility = Visibility.Collapsed;
                PlayButton.IsEnabled = access;
            }
            AddButton.IsEnabled = access;
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
        private async void GameProfilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _storedGameProfile = ((sender as ListView).SelectedItem as ListItemMainPage);
            if (_storedGameProfile != null)
            {
                ViewModel.PutChallengesInLists(_storedGameProfile.GameProfile);
                _challengesSource = await GetChallengesGrouped(ViewModel.GameProfileChallenges);
                ChallengesCVS.Source = _challengesSource;
                PlayButton.IsEnabled = true;
                EditButton.IsEnabled = true;
                RefreshButton.IsEnabled = true;

                PrepareLists();
            }
            else
            {
                ViewModel.GameProfileChallenges.Clear();
                _challengesSource = await GetChallengesGrouped(ViewModel.GameProfileChallenges);
                ChallengesCVS.Source = _challengesSource;
                PlayButton.IsEnabled = false;
                EditButton.IsEnabled = false;
                RefreshButton.IsEnabled = false;

                PrepareLists();
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
            if ((GameProfilesList.SelectedItem as ListItemMainPage) == null) return;

            var gameProfileWithObjects = new ListItemMainPage
            {
                GameProfile = (GameProfilesList.SelectedItem as ListItemMainPage).GameProfile,
                Challenges = ViewModel.GameProfileChallenges
            };

            try {
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
            } catch (NullReferenceException) { }
        }

        private void EditCommand_ItemClicked(ListItemMainPage clickedGameProfile)
        {
            ConnectedAnimation animation = GameProfilesList.PrepareConnectedAnimation("forwardAnimation", _storedGameProfile, "connectedElement");
            SmokeGrid.Visibility = Visibility.Visible;

            SmokeGridText.Text = _storedGameProfile.GameProfile.GameProfileName;
            animation.TryStart(SmokeGrid.Children[0]);
        }

        private void ResetSaveCommand_ItemClicked(ListItemMainPage clickedGameProfile)
        {
            ViewModel.RefreshSaveGame.Execute(clickedGameProfile);
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", SmokeGrid.Children[0]);
            
            animation.Completed += Animation_Completed;
            
            GameProfilesList.ScrollIntoView(_storedGameProfile, ScrollIntoViewAlignment.Default);
            GameProfilesList.UpdateLayout();
            
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                animation.Configuration = new DirectConnectedAnimationConfiguration();
            
            await GameProfilesList.TryStartConnectedAnimationAsync(animation, _storedGameProfile, "connectedElement");
        }

        /// <summary>  Handles the Animation_Completed event of the ConnectedAnimation.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        private void Animation_Completed(ConnectedAnimation sender, object args)
        {
            SmokeGrid.Visibility = Visibility.Collapsed;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var newChallenges = new List<ChallengeBase>();
            newChallenges.AddRange(ViewModel.NewAudienceChallenges);
            newChallenges.AddRange(ViewModel.NewCrewChallenges);
            newChallenges.AddRange(ViewModel.NewMultipleChoiceChallenges);
            newChallenges.AddRange(ViewModel.NewMusicChallenges);
            newChallenges.AddRange(ViewModel.NewQuizChallenges);
            newChallenges.AddRange(ViewModel.NewScreenshotChallenges);
            newChallenges.AddRange(ViewModel.NewSilhouetteChallenges);
            newChallenges.AddRange(ViewModel.NewSologameChallenges);

            _storedGameProfile.GameProfile.GameProfileName = SmokeGridText.Text;

            var newProfile = new Profile { UsingID=(int)_storedGameProfile.GameProfile.ProfileID };

            foreach (var challenge in newChallenges)
                newProfile.Challenges.Add(
                    new UsingChallenge
                    {
                        Challenge=challenge,
                        ChallengeID=challenge.ChallengeID,
                        UsingID=_storedGameProfile.GameProfile.ProfileID
                    });

            _storedGameProfile.GameProfile.Profile = newProfile;

            ViewModel.EditCommand.Execute(_storedGameProfile.GameProfile);

            var index = ViewModel.GameProfiles.IndexOf(_storedGameProfile);

            ViewModel.GameProfiles[index] = _storedGameProfile;

            _storedGameProfile = ViewModel.GameProfiles[index];

            BackButton_Click(sender, e);
        }

        private void Expander_Expanded(object sender, EventArgs e)
        {
            ChangeExpanderVisibility(Visibility.Collapsed);

            switch ((sender as Expander).Header)
            {
                case "AudienceChallenges":
                    AudienceChallengesExpander.Visibility = Visibility.Visible;
                    AudienceChallengesExpander2.Visibility = Visibility.Visible;
                    AudienceChallengesExpander.IsExpanded = true;
                    AudienceChallengesExpander2.IsExpanded = true;
                    break;
                case "CrewChallenges":
                    CrewChallengesExpander.Visibility = Visibility.Visible;
                    CrewChallengesExpander2.Visibility = Visibility.Visible;
                    CrewChallengesExpander.IsExpanded = true;
                    CrewChallengesExpander2.IsExpanded = true;
                    break;
                case "MultipleChoiceChallenges":
                    MultipleChoiceChallengesExpander.Visibility = Visibility.Visible;
                    MultipleChoiceChallengesExpander2.Visibility = Visibility.Visible;
                    MultipleChoiceChallengesExpander.IsExpanded = true;
                    MultipleChoiceChallengesExpander2.IsExpanded = true;
                    break;
                case "MusicChallenges":
                    MusicChallengesExpander.Visibility = Visibility.Visible;
                    MusicChallengesExpander2.Visibility = Visibility.Visible;
                    MusicChallengesExpander.IsExpanded = true;
                    MusicChallengesExpander2.IsExpanded = true;
                    break;
                case "QuizChallenges":
                    QuizChallengesExpander.Visibility = Visibility.Visible;
                    QuizChallengesExpander2.Visibility = Visibility.Visible;
                    QuizChallengesExpander.IsExpanded = true;
                    QuizChallengesExpander2.IsExpanded = true;
                    break;
                case "ScreenshotChallenges":
                    ScreenshotChallengesExpander.Visibility = Visibility.Visible;
                    ScreenshotChallengesExpander2.Visibility = Visibility.Visible;
                    ScreenshotChallengesExpander.IsExpanded = true;
                    ScreenshotChallengesExpander2.IsExpanded = true;
                    break;
                case "SilhouetteChallenges":
                    SilhouetteChallengesExpander.Visibility = Visibility.Visible;
                    SilhouetteChallengesExpander2.Visibility = Visibility.Visible;
                    SilhouetteChallengesExpander.IsExpanded = true;
                    SilhouetteChallengesExpander2.IsExpanded = true;
                    break;
                case "SologameChallenges":
                    SologameChallengesExpander.Visibility = Visibility.Visible;
                    SologameChallengesExpander2.Visibility = Visibility.Visible;
                    SologameChallengesExpander.IsExpanded = true;
                    SologameChallengesExpander2.IsExpanded = true;
                    break;
            }
        }
        private void Expander_Collapsed(object sender, EventArgs e)
        {
            ChangeExpanderVisibility(Visibility.Visible);

            switch ((sender as Expander).Header)
            {
                case "AudienceChallenges":
                    AudienceChallengesExpander.IsExpanded = false;
                    AudienceChallengesExpander2.IsExpanded = false;
                    break;
                case "CrewChallenges":
                    CrewChallengesExpander.IsExpanded = false;
                    CrewChallengesExpander2.IsExpanded = false;
                    break;
                case "MultipleChoiceChallenges":
                    MultipleChoiceChallengesExpander.IsExpanded = false;
                    MultipleChoiceChallengesExpander2.IsExpanded = false;
                    break;
                case "MusicChallenges":
                    MusicChallengesExpander.IsExpanded = false;
                    MusicChallengesExpander2.IsExpanded = false;
                    break;
                case "QuizChallenges":
                    QuizChallengesExpander.IsExpanded = false;
                    QuizChallengesExpander2.IsExpanded = false;
                    break;
                case "ScreenshotChallenges":
                    ScreenshotChallengesExpander.IsExpanded = false;
                    ScreenshotChallengesExpander2.IsExpanded = false;
                    break;
                case "SilhouetteChallenges":
                    SilhouetteChallengesExpander.IsExpanded = false;
                    SilhouetteChallengesExpander2.IsExpanded = false;
                    break;
                case "SologameChallenges":
                    SologameChallengesExpander.IsExpanded = false;
                    SologameChallengesExpander2.IsExpanded = false;
                    break;
            }
        }

        public async Task<ObservableCollection<GroupChallengesList>> GetChallengesGrouped(ObservableCollection<ListItemMainPage> list)
        {
            var query = await Task.Run(() => from item in list
                                             group item by item.Challenge.GetDiscriminator() into g
                                             orderby g.Key
                                             select new GroupChallengesList(g) { Key = g.Key });

            return new ObservableCollection<GroupChallengesList>(query);
        }

        private void TotalChallenges_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var challengeType = (((sender as ListView).Parent as Grid).Parent as Expander).Header + ";";
            var challengeIDs = string.Join(",", e.Items.Cast<ChallengeBase>().Select(i => i.ChallengeID));
            e.Data.SetText(challengeType + challengeIDs);
            e.Data.RequestedOperation = DataPackageOperation.Copy;
        }
        private void GameProfileChallenges_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var challengeType = (((sender as ListView).Parent as Grid).Parent as Expander).Header + ";";
            var challengeIDs = string.Join(",", e.Items.Cast<ChallengeBase>().Select(i => i.ChallengeID));
            e.Data.SetText(challengeType + challengeIDs);
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }

        private void TotalChallenges_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = ((e.Data.RequestedOperation == DataPackageOperation.Move)?DataPackageOperation.Move:DataPackageOperation.None);

            e.DragUIOverride.IsGlyphVisible = false;

            if (e.AcceptedOperation == DataPackageOperation.Move)
            {
                e.DragUIOverride.Caption = "Drop challenge(s) here to remove from list";
            } else
            {
                e.DragUIOverride.IsCaptionVisible = false;
            }
        }
        private void GameProfileChallenges_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = ((e.Data.RequestedOperation==DataPackageOperation.Copy)?DataPackageOperation.Copy:DataPackageOperation.None);

            e.DragUIOverride.IsGlyphVisible = false;

            if (e.AcceptedOperation == DataPackageOperation.Copy)
            {
                e.DragUIOverride.Caption = "Drop challenge(s) here to add to list";
            } else
            {
                e.DragUIOverride.IsCaptionVisible = false;
            }
        }

        private async void TotalChallenges_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                var def = e.GetDeferral();
                var s = await e.DataView.GetTextAsync();
                var header = s.Split(";")[0];
                var challengeIDs = s.Split(";")[1].Split(",");

                switch (header)
                {
                    case "AudienceChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.NewAudienceChallenges.First(x => x.ChallengeID == index);
                            if (element != null) ViewModel.NewAudienceChallenges.Remove(element);
                        }
                        break;
                    case "CrewChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.NewCrewChallenges.First(x => x.ChallengeID == index);
                            if (element != null) ViewModel.NewCrewChallenges.Remove(element);
                        }
                        break;
                    case "MultipleChoiceChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.NewMultipleChoiceChallenges.First(x => x.ChallengeID == index);
                            if (element != null) ViewModel.NewMultipleChoiceChallenges.Remove(element);
                        }
                        break;
                    case "MusicChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.NewMusicChallenges.First(x => x.ChallengeID == index);
                            if (element != null) ViewModel.NewMusicChallenges.Remove(element);
                        }
                        break;
                    case "QuizChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.NewQuizChallenges.First(x => x.ChallengeID == index);
                            if (element != null) ViewModel.NewQuizChallenges.Remove(element);
                        }
                        break;
                    case "ScreenshotChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.NewScreenshotChallenges.First(x => x.ChallengeID == index);
                            if (element != null) ViewModel.NewScreenshotChallenges.Remove(element);
                        }
                        break;
                    case "SilhouetteChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.NewSilhouetteChallenges.First(x => x.ChallengeID == index);
                            if (element != null) ViewModel.NewSilhouetteChallenges.Remove(element);
                        }
                        break;
                    case "SologameChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.NewSologameChallenges.First(x => x.ChallengeID == index);
                            if (element != null) ViewModel.NewSologameChallenges.Remove(element);
                        }
                        break;
                }

                e.AcceptedOperation = DataPackageOperation.Copy;
                def.Complete();
            }
        }
        private async void GameProfileChallenges_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                var def = e.GetDeferral();
                var s = await e.DataView.GetTextAsync();
                var header = s.Split(";")[0];
                var challengeIDs = s.Split(";")[1].Split(",");

                switch (header)
                {
                    case "AudienceChallenges":
                        foreach(string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.AudienceChallenges.First(x => x.ChallengeID == index);
                            if (element != null && !ViewModel.NewAudienceChallenges.Contains(element)) ViewModel.NewAudienceChallenges.Add(element);
                        }
                        break;
                    case "CrewChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.CrewChallenges.First(x => x.ChallengeID == index);
                            if (element != null && !ViewModel.NewCrewChallenges.Contains(element)) ViewModel.NewCrewChallenges.Add(element);
                        }
                        break;
                    case "MultipleChoiceChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.MultipleChoiceChallenges.First(x => x.ChallengeID == index);
                            if (element != null && !ViewModel.NewMultipleChoiceChallenges.Contains(element)) ViewModel.NewMultipleChoiceChallenges.Add(element);
                        }
                        break;
                    case "MusicChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.MusicChallenges.First(x => x.ChallengeID == index);
                            if (element != null && !ViewModel.NewMusicChallenges.Contains(element)) ViewModel.NewMusicChallenges.Add(element);
                        }
                        break;
                    case "QuizChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.QuizChallenges.First(x => x.ChallengeID == index);
                            if (element != null && !ViewModel.NewQuizChallenges.Contains(element)) ViewModel.NewQuizChallenges.Add(element);
                        }
                        break;
                    case "ScreenshotChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.ScreenshotChallenges.First(x => x.ChallengeID == index);
                            if (element != null && !ViewModel.NewScreenshotChallenges.Contains(element)) ViewModel.NewScreenshotChallenges.Add(element);
                        }
                        break;
                    case "SilhouetteChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.SilhouetteChallenges.First(x => x.ChallengeID == index);
                            if (element != null && !ViewModel.NewSilhouetteChallenges.Contains(element)) ViewModel.NewSilhouetteChallenges.Add(element);
                        }
                        break;
                    case "SologameChallenges":
                        foreach (string i in challengeIDs)
                        {
                            var index = int.TryParse(i, out int result) ? result : -1;
                            var element = ViewModel.SologameChallenges.First(x => x.ChallengeID == index);
                            if (element != null && !ViewModel.NewSologameChallenges.Contains(element)) ViewModel.NewSologameChallenges.Add(element);
                        }
                        break;
                }

                e.AcceptedOperation = DataPackageOperation.Copy;
                def.Complete();
            }
        }

        internal void ChangeExpanderVisibility(Visibility visibility)
        {
            AudienceChallengesExpander.Visibility = visibility;
            CrewChallengesExpander.Visibility = visibility;
            MultipleChoiceChallengesExpander.Visibility = visibility;
            MusicChallengesExpander.Visibility = visibility;
            QuizChallengesExpander.Visibility = visibility;
            ScreenshotChallengesExpander.Visibility = visibility;
            SilhouetteChallengesExpander.Visibility = visibility;
            SologameChallengesExpander.Visibility = visibility;

            AudienceChallengesExpander2.Visibility = visibility;
            CrewChallengesExpander2.Visibility = visibility;
            MultipleChoiceChallengesExpander2.Visibility = visibility;
            MusicChallengesExpander2.Visibility = visibility;
            QuizChallengesExpander2.Visibility = visibility;
            ScreenshotChallengesExpander2.Visibility = visibility;
            SilhouetteChallengesExpander2.Visibility = visibility;
            SologameChallengesExpander2.Visibility = visibility;
        }
        private void PrepareLists()
        {
            ClearLists();

            foreach (ChallengeBase challenge in ViewModel.ChallengesFromDB)
            {
                switch (challenge.GetDiscriminator())
                {
                    case "AudienceChallenge":
                        ViewModel.AudienceChallenges.Add(challenge as AudienceChallenge);
                        break;
                    case "CrewChallenge":
                        ViewModel.CrewChallenges.Add(challenge as CrewChallenge);
                        break;
                    case "MultipleChoiceChallenge":
                        ViewModel.MultipleChoiceChallenges.Add(challenge as MultipleChoiceChallenge);
                        break;
                    case "MusicChallenge":
                        ViewModel.MusicChallenges.Add(challenge as MusicChallenge);
                        break;
                    case "QuizChallenge":
                        ViewModel.QuizChallenges.Add(challenge as QuizChallenge);
                        break;
                    case "ScreenshotChallenge":
                        ViewModel.ScreenshotChallenges.Add(challenge as ScreenshotChallenge);
                        break;
                    case "SilhouetteChallenge":
                        ViewModel.SilhouetteChallenges.Add(challenge as SilhouetteChallenge);
                        break;
                    case "SologameChallenge":
                        ViewModel.SologameChallenges.Add(challenge as SologameChallenge);
                        break;
                }
            }

            foreach (ListItemMainPage challenge in ViewModel.GameProfileChallenges)
            {
                switch (challenge.Challenge.GetDiscriminator())
                {
                    case "AudienceChallenge":
                        ViewModel.NewAudienceChallenges.Add(challenge.Challenge as AudienceChallenge);
                        break;
                    case "CrewChallenge":
                        ViewModel.NewCrewChallenges.Add(challenge.Challenge as CrewChallenge);
                        break;
                    case "MultipleChoiceChallenge":
                        ViewModel.NewMultipleChoiceChallenges.Add(challenge.Challenge as MultipleChoiceChallenge);
                        break;
                    case "MusicChallenge":
                        ViewModel.NewMusicChallenges.Add(challenge.Challenge as MusicChallenge);
                        break;
                    case "QuizChallenge":
                        ViewModel.NewQuizChallenges.Add(challenge.Challenge as QuizChallenge);
                        break;
                    case "ScreenshotChallenge":
                        ViewModel.NewScreenshotChallenges.Add(challenge.Challenge as ScreenshotChallenge);
                        break;
                    case "SilhouetteChallenge":
                        ViewModel.NewSilhouetteChallenges.Add(challenge.Challenge as SilhouetteChallenge);
                        break;
                    case "SologameChallenge":
                        ViewModel.NewSologameChallenges.Add(challenge.Challenge as SologameChallenge);
                        break;
                }
            }
        }
        private void ClearLists()
        {
            ViewModel.AudienceChallenges.Clear();
            ViewModel.CrewChallenges.Clear();
            ViewModel.MultipleChoiceChallenges.Clear();
            ViewModel.MusicChallenges.Clear();
            ViewModel.QuizChallenges.Clear();
            ViewModel.ScreenshotChallenges.Clear();
            ViewModel.SilhouetteChallenges.Clear();
            ViewModel.SologameChallenges.Clear();

            ViewModel.NewAudienceChallenges.Clear();
            ViewModel.NewCrewChallenges.Clear();
            ViewModel.NewMultipleChoiceChallenges.Clear();
            ViewModel.NewMusicChallenges.Clear();
            ViewModel.NewQuizChallenges.Clear();
            ViewModel.NewScreenshotChallenges.Clear();
            ViewModel.NewSilhouetteChallenges.Clear();
            ViewModel.NewSologameChallenges.Clear();
        }
    }
}
