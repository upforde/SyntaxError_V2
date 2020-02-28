﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.UI.Controls;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class CreateChallengesPage : Page
    {
        /// <summary>  A command to add a new Challenge to the database and list.</summary>
        /// <value>The add new object command.</value>
        public ICommand AddNewChallengeCommand { get; set; }
        
        private ICommand _editCommand;
        /// <summary>Edit command for the MediaObject.</summary>
        /// <value>The edit command.</value>
        public ICommand EditCommand => _editCommand ?? (_editCommand = new RelayCommand<ChallengeBase>(EditCommand_ItemClicked));

        public CreateChallengesViewModel ViewModel { get; } = new CreateChallengesViewModel();
        
        public FontFamily segoeMDL2Assets = new FontFamily("Segoe MDL2 Assets");
        public ChallengeBase _storedChallenge { get; set; }

        /// <summary>Initializes a new instance of the <see cref="CreateChallengesPage"/> class.</summary>
        public CreateChallengesPage()
        {
            InitializeComponent();

            AddNewChallengeCommand = new RelayCommand<ChallengeBase>(async param =>
                                                    {
                                                        param = await ViewModel.ChallengesDataAccess.CreateChallengeAsync(param);

                                                        switch (param.GetDiscriminator())
                                                        {
                                                            case "AudienceChallenge":
                                                                ViewModel.AudienceChallenges.Add(param as AudienceChallenge);
                                                                break;
                                                            case "CrewChallenge":
                                                                ViewModel.CrewChallenges.Add(param as CrewChallenge);
                                                                break;
                                                            case "MultipleChoiceChallenge":
                                                                ViewModel.MultipleChoiceChallenges.Add(param as MultipleChoiceChallenge);
                                                                break;
                                                            case "MusicChallenge":
                                                                ViewModel.MusicChallenges.Add(param as MusicChallenge);
                                                                break;
                                                            case "QuizChallenge":
                                                                ViewModel.QuizChallenges.Add(param as QuizChallenge);
                                                                break;
                                                            case "ScreenshotChallenge":
                                                                ViewModel.ScreenshotChallenges.Add(param as ScreenshotChallenge);
                                                                break;
                                                            case "SilhouetteChallenge":
                                                                ViewModel.SilhouetteChallenges.Add(param as SilhouetteChallenge);
                                                                break;
                                                            case "SologameChallenge":
                                                                ViewModel.SologameChallenges.Add(param as SologameChallenge);
                                                                break;
                                                        }
                                                    }, param => param != null);

            Loaded += CreateChallengesPage_Loaded;
            
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChangedAsync;
        }

        /// <summary>Handles the NetworkAvailabilityChangedAsync event of the NetworkChange control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NetworkAvailabilityEventArgs"/> instance containing the event data.</param>
        private async void NetworkChange_NetworkAvailabilityChangedAsync(object sender, NetworkAvailabilityEventArgs e)
        {
            if (!e.IsAvailable)
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => CreateChallengesPage_Loaded());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ChangeButtonsEnabled(e.IsAvailable));
        }

        /// <summary>Changes the enabled attribute of buttons.</summary>
        /// <param name="access">if set to <c>true</c> [access].</param>
        private void ChangeButtonsEnabled(bool access)
        {
            var actionGrid = (((ChallengeList.SelectedItem as PivotItem).Content as Grid).Children[0] as Grid);

            ((actionGrid.Children[0] as StackPanel).Children[1] as AppBarButton).IsEnabled = access;
            ((actionGrid.Children[0] as StackPanel).Children[2] as AppBarButton).IsEnabled = access;
            (actionGrid.Children[1] as AppBarButton).IsEnabled = access;
        }

        //private void RefreshList()
        //{
        //    Filtered.Clear();

        //        switch (ChallengeList.SelectedIndex)
        //        {
        //            case 0:
        //                foreach(var item in ViewModel.AudienceChallenges)
        //                    Filtered.Add(item as ChallengeBase);
        //                break;
        //            case 1:
        //                foreach(var item in ViewModel.CrewChallenges)
        //                    Filtered.Add(item as ChallengeBase);
        //                break;
        //            case 2:
        //                foreach(var item in ViewModel.MultipleChoiceChallenges)
        //                    Filtered.Add(item as ChallengeBase);
        //                break;
        //            case 3:
        //                foreach(var item in ViewModel.MusicChallenges)
        //                    Filtered.Add(item as ChallengeBase);
        //                break;
        //            case 4:
        //                foreach(var item in ViewModel.QuizChallenges)
        //                    Filtered.Add(item as ChallengeBase);
        //                break;
        //            case 5:
        //                foreach(var item in ViewModel.ScreenshotChallenges)
        //                    Filtered.Add(item as ChallengeBase);
        //                break;
        //            case 6:
        //                foreach(var item in ViewModel.SilhouetteChallenges)
        //                    Filtered.Add(item as ChallengeBase);
        //                break;
        //            case 7:
        //                foreach(var item in ViewModel.SologameChallenges)
        //                    Filtered.Add(item as ChallengeBase);
        //                break;
        //        }
        //}

        /// <summary>Handles the Loaded event of the CreateChallengesPage control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void CreateChallengesPage_Loaded(object sender = null, RoutedEventArgs e = null)
        {
            var isInternetAvailable = NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
            if(isInternetAvailable)
                await ViewModel.LoadChallengesFromDBAsync();
            ChangeButtonsEnabled(isInternetAvailable);
        }

        /// <summary>Handles the Click event of the AppBarButton_SelectionMode control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.</param>
        private void AppBarButton_SelectionMode_Click(object sender, RoutedEventArgs e)
        {
            var collection = (ChallengeList.SelectedItem as PivotItem).Content as Grid;
            var deleteButton = ((collection.Children[0] as Grid)
                .Children[0] as StackPanel)
                .Children[2] as AppBarButton;
            bool isList = (ChallengeList.SelectedIndex == 2 || ChallengeList.SelectedIndex == 4)?true:false;

            if(deleteButton.Visibility == Visibility.Visible)
            {
                if (isList)
                    (collection.Children[1] as ListView).SelectionMode = ListViewSelectionMode.None;
                else (collection.Children[1] as AdaptiveGridView).SelectionMode = ListViewSelectionMode.None;
                deleteButton.Visibility = Visibility.Collapsed;
                return;
            }

            if (isList)
                (collection.Children[1] as ListView).SelectionMode = ListViewSelectionMode.Multiple;
            else (collection.Children[1] as AdaptiveGridView).SelectionMode = ListViewSelectionMode.Multiple;
            deleteButton.Visibility = Visibility.Visible;
        }

        /// <summary>Handles the Click event of the AppBarButton_DeleteSelected control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AppBarButton_DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var selectedContext = (ChallengeList.SelectedItem as PivotItem).Content as Grid;
            bool isList = (ChallengeList.SelectedIndex == 2 || ChallengeList.SelectedIndex == 4)?true:false;
            IList<object> itemsToDelete = new List<object>();
            if (isList)
                itemsToDelete = (selectedContext.Children[1] as ListView).SelectedItems;
            else itemsToDelete = (selectedContext.Children[1] as AdaptiveGridView).SelectedItems;

            foreach(var item in itemsToDelete)
            {
                ViewModel.DeleteCommand.Execute(item as ChallengeBase);
            }
        }

        /// <summary> Creates a new Challenge in the database.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void AddNewChallengeButton_Click(object sender, RoutedEventArgs e)
        {
            ChallengeBase newChallenge;
            switch (ChallengeList.SelectedIndex)
            {
                case 0:
                    newChallenge = new AudienceChallenge();
                    break;
                case 1:
                    newChallenge = new CrewChallenge();
                    break;
                case 2:
                    newChallenge = new MultipleChoiceChallenge();
                    break;
                case 3:
                    newChallenge = new MusicChallenge();
                    break;
                case 4:
                    newChallenge = new QuizChallenge();
                    break;
                case 5:
                    newChallenge = new ScreenshotChallenge();
                    break;
                case 6:
                    newChallenge = new SilhouetteChallenge();
                    break;
                case 7:
                    newChallenge = new SologameChallenge();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            newChallenge.ChallengeTask = "New Challenge";
            AddNewChallengeCommand.Execute(newChallenge);
        }

        /// <summary>Edits the command item clicked.</summary>
        /// <param name="clickedItem">The clicked item.</param>
        private void EditCommand_ItemClicked(ChallengeBase clickedItem)
        {
            ConnectedAnimation animation;

            var collection = (ChallengeList.SelectedItem as PivotItem).Content as Grid;
            var smokeGrid = (collection.Children[2] as Grid);
            var children = ((smokeGrid.Children[0] as Grid).Children[0] as StackPanel).Children[0] as Grid;
            
            bool isList = (ChallengeList.SelectedIndex == 2 || ChallengeList.SelectedIndex == 4)?true:false;

            (children.Children[0] as TextBox).Text = _storedChallenge.ChallengeTask;
            
            if (isList)
            {
                animation = (collection.Children[1] as ListView).PrepareConnectedAnimation("forwardAnimation", _storedChallenge, "connectedElement");
            }
            else
            {
                animation = (collection.Children[1] as AdaptiveGridView).PrepareConnectedAnimation("forwardAnimation", _storedChallenge, "connectedElement");
                var childGrid = (children.Children[2] as Grid).Children;
                
                if (ChallengeList.SelectedIndex == 0 || ChallengeList.SelectedIndex == 1 || ChallengeList.SelectedIndex == 7)
                {
                    if ((_storedChallenge as GameChallenge).Game != null)
                    {
                        ((((childGrid[0] as StackPanel).Children[0] as Grid).Children[0] as Grid).Children[0] as ImageEx).Source = (_storedChallenge as GameChallenge).Game.URI;
                        (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Games.IndexOf((_storedChallenge as GameChallenge).Game);
                        (childGrid[2] as TextBox).Text = (_storedChallenge as GameChallenge).Game.Name;
                    }
                    else
                    {
                        (_storedChallenge as GameChallenge).Game = ViewModel.Games[ViewModel.Games.Count-1];
                        ((((childGrid[0] as StackPanel).Children[0] as Grid).Children[0] as Grid).Children[0] as ImageEx).Source = null;
                        (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Games.Count-1;
                        (childGrid[2] as TextBox).Text = "";
                    }
                }
                else if (ChallengeList.SelectedIndex == 5 || ChallengeList.SelectedIndex == 6)
                {
                    (childGrid[0] as ImageEx).Source = (_storedChallenge as ImageChallenge).Image.URI;
                    (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Images.IndexOf((_storedChallenge as ImageChallenge).Image);
                    (childGrid[2] as TextBox).Text = (_storedChallenge as ImageChallenge).Image.Name;
                }
                else
                {
                    (childGrid[0] as ImageEx).Source = (_storedChallenge as MusicChallenge).Song.URI;
                    (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Music.IndexOf((_storedChallenge as MusicChallenge).Song);
                    (childGrid[2] as TextBox).Text = (_storedChallenge as MusicChallenge).Song.Name;
                }
            }

            smokeGrid.Visibility = Visibility.Visible;
            animation.TryStart(smokeGrid.Children[0]);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            throw new NotImplementedException();
        }

        /// <summary>Handles the Tapped event of the ChallengeList control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.Input.TappedRoutedEventArgs"/> instance containing the event data.</param>
        private void ChallengeList_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var element = (e.OriginalSource as FrameworkElement).DataContext as ChallengeBase;
            _storedChallenge = element;
            var myFlyout = new MenuFlyout();

            var editItem = new MenuFlyoutItem
            {
                Text = "Edit",
                Icon = new FontIcon()
                {
                    FontFamily = segoeMDL2Assets,
                    Glyph = "\uE70F"
                },
                Command = EditCommand,
                CommandParameter = element
            };
            var deleteItem = new MenuFlyoutItem
            {
                Text = "Delete",
                Icon = new FontIcon()
                {
                    FontFamily = segoeMDL2Assets,
                    Glyph = "\uE74D"
                },
                Command = ViewModel.DeleteCommand,
                CommandParameter = element
            };
            myFlyout.Items.Add(editItem);
            myFlyout.Items.Add(deleteItem);
            
            if (element != null)
                myFlyout.ShowAt((sender as UIElement), e.GetPosition(sender as UIElement));
        }

        /// <summary>Handles the Click event of the BackButton control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var collection = (ChallengeList.SelectedItem as PivotItem).Content as Grid;
            var smokeGrid = collection.Children[2] as Grid;
            
            bool isList = (ChallengeList.SelectedIndex == 2 || ChallengeList.SelectedIndex == 4)?true:false;

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", smokeGrid.Children[0]);
            
            animation.Completed += Animation_Completed;

            if (isList)
            {
                var challengeList = (collection.Children[1] as ListView);
                
                challengeList.ScrollIntoView(_storedChallenge, ScrollIntoViewAlignment.Default);
                challengeList.UpdateLayout();
            }
            else
            {
                var challengeList = (collection.Children[1] as AdaptiveGridView);
                
                challengeList.ScrollIntoView(_storedChallenge, ScrollIntoViewAlignment.Default);
                challengeList.UpdateLayout();
            }
            
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                animation.Configuration = new DirectConnectedAnimationConfiguration();

            if (isList)
                await (collection.Children[1] as ListView).TryStartConnectedAnimationAsync(animation, _storedChallenge, "connectedElement");
            else
                await (collection.Children[1] as AdaptiveGridView).TryStartConnectedAnimationAsync(animation, _storedChallenge, "connectedElement");
        }

        /// <summary>  Handles the Animation_Completed event of the ConnectedAnimation.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        private void Animation_Completed(ConnectedAnimation sender, object args)
        {
            var smokeGrid = ((ChallengeList.SelectedItem as PivotItem).Content as Grid).Children[2] as Grid;
            smokeGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>Handles the Click event of the SaveButton control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var collection = (ChallengeList.SelectedItem as PivotItem).Content as Grid;
            var smokeGrid = collection.Children[2] as Grid;
            var smokeGridChild = smokeGrid.Children[0] as Grid;
            var children = ((smokeGrid.Children[0] as Grid).Children[0] as StackPanel).Children[0] as Grid;
            var childGrid = (children.Children[2] as Grid).Children;
            
            _storedChallenge.ChallengeTask = (((smokeGridChild.Children[0] as StackPanel).Children[0] as Grid).Children[0] as TextBox).Text;

            switch (ChallengeList.SelectedIndex)
            {
                case 0:
                case 1:
                case 7:
                    (_storedChallenge as GameChallenge).Game.Name = (childGrid[2] as TextBox).Text;
                    if(((childGrid[1] as ComboBox).SelectedItem as Game).ID == 0){
                        var newGame = await ViewModel.ObjectsViewModel.MediaObjectsDataAccess.CreateMediaObjectAsync((_storedChallenge as GameChallenge).Game);
                        (_storedChallenge as GameChallenge).GameID = newGame.ID;
                    }
                    else (_storedChallenge as GameChallenge).GameID = (_storedChallenge as GameChallenge).Game.ID;
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
            }
            
            ViewModel.EditCommand.Execute(_storedChallenge);

            switch (ChallengeList.SelectedIndex)
            {
                case 0:
                    var audienceIndex = ViewModel.AudienceChallenges.IndexOf(ViewModel.AudienceChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    ViewModel.AudienceChallenges[audienceIndex] = _storedChallenge as AudienceChallenge;
                    break;
                case 1:
                    var crewIndex = ViewModel.CrewChallenges.IndexOf(ViewModel.CrewChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    ViewModel.CrewChallenges[crewIndex] = _storedChallenge as CrewChallenge;
                    break;
                case 2:
                    var multipleChoiceIndex = ViewModel.MultipleChoiceChallenges.IndexOf(ViewModel.MultipleChoiceChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    ViewModel.MultipleChoiceChallenges[multipleChoiceIndex] = _storedChallenge as MultipleChoiceChallenge;
                    break;
                case 3:
                    var musicIndex = ViewModel.MusicChallenges.IndexOf(ViewModel.MusicChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    ViewModel.MusicChallenges[musicIndex] = _storedChallenge as MusicChallenge;
                    break;
                case 4:
                    var quizIndex = ViewModel.QuizChallenges.IndexOf(ViewModel.QuizChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    ViewModel.QuizChallenges[quizIndex] = _storedChallenge as QuizChallenge;
                    break;
                case 5:
                    var screenshotIndex = ViewModel.ScreenshotChallenges.IndexOf(ViewModel.ScreenshotChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    ViewModel.ScreenshotChallenges[screenshotIndex] = _storedChallenge as ScreenshotChallenge;
                    break;
                case 6:
                    var silhouetteIndex = ViewModel.SilhouetteChallenges.IndexOf(ViewModel.SilhouetteChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    ViewModel.SilhouetteChallenges[silhouetteIndex] = _storedChallenge as SilhouetteChallenge;
                    break;
                case 7:
                    var sologameIndex = ViewModel.SologameChallenges.IndexOf(ViewModel.SologameChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    ViewModel.SologameChallenges[sologameIndex] = _storedChallenge as SologameChallenge;
                    break;
            }

            ViewModel.CreateNewObjectPlaceholder();
            
            BackButton_Click(sender, e);
        }

        private void GameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (_storedChallenge as GameChallenge).Game = ViewModel.Games[(sender as ComboBox).SelectedIndex];

            var collection = (ChallengeList.SelectedItem as PivotItem).Content as Grid;
            var smokeGrid = (collection.Children[2] as Grid);
            var children = ((smokeGrid.Children[0] as Grid).Children[0] as StackPanel).Children[0] as Grid;

            var childGrid = (children.Children[2] as Grid).Children;
            ((((childGrid[0] as StackPanel).Children[0] as Grid).Children[0] as Grid).Children[0] as ImageEx).Source = (_storedChallenge as GameChallenge).Game.URI;
            (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Games.IndexOf((_storedChallenge as GameChallenge).Game);
            (childGrid[2] as TextBox).Text = (_storedChallenge as GameChallenge).Game.Name;
        }

        private async void StackPanel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var collection = (ChallengeList.SelectedItem as PivotItem).Content as Grid;
                var smokeGrid = (collection.Children[2] as Grid);
                var children = ((smokeGrid.Children[0] as Grid).Children[0] as StackPanel).Children[0] as Grid;
                var childGrid = (children.Children[2] as Grid).Children;

                switch (ChallengeList.SelectedIndex)
                {
                    case 0:
                    case 1:
                    case 7:
                        (_storedChallenge as GameChallenge).Game.URI = await ViewModel.ObjectsViewModel.ImagesDataAccess.PostImageAsync(file);
                        ((((childGrid[0] as StackPanel).Children[0] as Grid).Children[0] as Grid).Children[0] as ImageEx).Source = (_storedChallenge as GameChallenge).Game.URI;
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                }
            }
        }
    }
}
