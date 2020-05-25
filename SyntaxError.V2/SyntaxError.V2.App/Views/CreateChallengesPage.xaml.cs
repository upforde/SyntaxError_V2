using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
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

        /// <summary>Gets the view model.</summary>
        /// <value>The view model.</value>
        public CreateChallengesViewModel ViewModel { get; } = new CreateChallengesViewModel();

        /// <summary>The segoe md l2 assets</summary>
        public FontFamily segoeMDL2Assets = new FontFamily("Segoe MDL2 Assets");

        /// <summary>Gets or sets the stored challenge.</summary>
        /// <value>The stored challenge.</value>
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
                                                                CreateChallengesViewModel.AudienceChallenges.Insert(0, param as AudienceChallenge);
                                                                ViewModel.FilteredAudience.Insert(0, param as AudienceChallenge);
                                                                break;
                                                            case "CrewChallenge":
                                                                CreateChallengesViewModel.CrewChallenges.Insert(0, param as CrewChallenge);
                                                                ViewModel.FilteredCrew.Insert(0, param as CrewChallenge);
                                                                break;
                                                            case "MultipleChoiceChallenge":
                                                                CreateChallengesViewModel.MultipleChoiceChallenges.Insert(0, param as MultipleChoiceChallenge);
                                                                ViewModel.FilteredMulti.Insert(0, param as MultipleChoiceChallenge);
                                                                break;
                                                            case "MusicChallenge":
                                                                CreateChallengesViewModel.MusicChallenges.Insert(0, param as MusicChallenge);
                                                                ViewModel.FilteredMusic.Insert(0, param as MusicChallenge);
                                                                break;
                                                            case "QuizChallenge":
                                                                CreateChallengesViewModel.QuizChallenges.Insert(0, param as QuizChallenge);
                                                                ViewModel.FilteredQuiz.Insert(0, param as QuizChallenge);
                                                                break;
                                                            case "ScreenshotChallenge":
                                                                CreateChallengesViewModel.ScreenshotChallenges.Insert(0, param as ScreenshotChallenge);
                                                                ViewModel.FilteredScreen.Insert(0, param as ScreenshotChallenge);
                                                                break;
                                                            case "SilhouetteChallenge":
                                                                CreateChallengesViewModel.SilhouetteChallenges.Insert(0, param as SilhouetteChallenge);
                                                                ViewModel.FilteredSilhu.Insert(0, param as SilhouetteChallenge);
                                                                break;
                                                            case "SologameChallenge":
                                                                CreateChallengesViewModel.SologameChallenges.Insert(0, param as SologameChallenge);
                                                                ViewModel.FilteredSolo.Insert(0, param as SologameChallenge);
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

        /// <summary>Handles the Loaded event of the CreateChallengesPage control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void CreateChallengesPage_Loaded(object sender = null, RoutedEventArgs e = null)
        {
            RefreshButtonDeactivate();
            var isInternetAvailable = NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
            if (isInternetAvailable)
                try
                {
                    await ViewModel.LoadChallengesFromDBAsync();
                    ChangeButtonsEnabled(isInternetAvailable);
                    ViewModel.RefreshList("", ChallengeList.SelectedIndex);
                }
                catch (HttpRequestException) { RefreshButtonActive(); }
                catch (Newtonsoft.Json.JsonSerializationException) { RefreshButtonActive(); }
                catch (Newtonsoft.Json.JsonReaderException) { RefreshButtonActive(); }
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
                {
                    (collection.Children[1] as ListView).SelectionMode = ListViewSelectionMode.None;
                    (collection.Children[1] as ListView).Tapped += ChallengeList_Tapped;
                } else
                {
                    (collection.Children[1] as AdaptiveGridView).SelectionMode = ListViewSelectionMode.None;
                    (collection.Children[1] as AdaptiveGridView).Tapped += ChallengeList_Tapped;
                }
                deleteButton.Visibility = Visibility.Collapsed;
                return;
            }

            if (isList)
            {
                (collection.Children[1] as ListView).SelectionMode = ListViewSelectionMode.Multiple;
                (collection.Children[1] as ListView).Tapped -= ChallengeList_Tapped;
            } else {
                (collection.Children[1] as AdaptiveGridView).SelectionMode = ListViewSelectionMode.Multiple;
                (collection.Children[1] as AdaptiveGridView).Tapped -= ChallengeList_Tapped;
            }
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

            ViewModel.RefreshList((((((ChallengeList.SelectedItem as PivotItem)
                .Content as Grid).Children[0] as Grid).Children[0] as StackPanel)
                .Children[0] as AutoSuggestBox).Text.ToLower(),
                ChallengeList.SelectedIndex);
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
                    newChallenge = new AudienceChallenge{ ChallengeTask = "New challenge" };
                    break;
                case 1:
                    newChallenge = new CrewChallenge{ ChallengeTask = "New challenge" };
                    break;
                case 2:
                    newChallenge = new MultipleChoiceChallenge{ ChallengeTask = "New challenge" };
                    break;
                case 3:
                    newChallenge = new MusicChallenge{ ChallengeTask = "Guess the song" };
                    break;
                case 4:
                    newChallenge = new QuizChallenge{ ChallengeTask = "New challenge" };
                    break;
                case 5:
                    newChallenge = new ScreenshotChallenge{ ChallengeTask = "What game is this screenshot from?" };
                    break;
                case 6:
                    newChallenge = new SilhouetteChallenge{ ChallengeTask = "What character is this?" };
                    break;
                case 7:
                    newChallenge = new SologameChallenge{ ChallengeTask = "New challenge" };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            AddNewChallengeCommand.Execute(newChallenge);
        }

        /// <summary>Edits the command item clicked.</summary>
        /// <param name="clickedItem">The clicked item.</param>
        private async void EditCommand_ItemClicked(ChallengeBase clickedItem)
        {
            ConnectedAnimation animation;

            var collection = (ChallengeList.SelectedItem as PivotItem).Content as Grid;
            var smokeGrid = (collection.Children[2] as Grid);
            var children = ((smokeGrid.Children[0] as Grid).Children[0] as StackPanel).Children[0] as Grid;
            
            bool isList = (ChallengeList.SelectedIndex == 2 || ChallengeList.SelectedIndex == 4);
            bool isGame = (ChallengeList.SelectedIndex == 0 || ChallengeList.SelectedIndex == 1 || ChallengeList.SelectedIndex == 7);
            bool isImage = (ChallengeList.SelectedIndex == 5 || ChallengeList.SelectedIndex == 6);

            (children.Children[0] as TextBox).Text = _storedChallenge.ChallengeTask;
            
            if (isList)
            {
                var answerID = (_storedChallenge as QuestionChallenge).AnswersID;
                if(answerID != null){
                    (_storedChallenge as QuestionChallenge).Answers = await ViewModel.AnswersDataAccess.GetAnswersAsync(answerID);
                    if(ChallengeList.SelectedIndex == 2){
                        MultipleChoiceAnswer.Text = (_storedChallenge as QuestionChallenge).Answers.Answer;
                        MultipleChoiceDummy1.Text = (_storedChallenge as QuestionChallenge).Answers.DummyAnswer1;
                        MultipleChoiceDummy2.Text = (_storedChallenge as QuestionChallenge).Answers.DummyAnswer2;
                        MultipleChoiceDummy3.Text = (_storedChallenge as QuestionChallenge).Answers.DummyAnswer3;
                    }
                    else QuizAnswer.Text = (_storedChallenge as QuestionChallenge).Answers.Answer;
                }
                animation = (collection.Children[1] as ListView).PrepareConnectedAnimation("forwardAnimation", _storedChallenge, "connectedElement");
            }
            else
            {
                animation = (collection.Children[1] as AdaptiveGridView).PrepareConnectedAnimation("forwardAnimation", _storedChallenge, "connectedElement");
                var childGrid = (children.Children[2] as Grid).Children;
                
                if (isGame)
                {
                    var gameImage = ((((childGrid[0] as StackPanel).Children[0] as Grid).Children[0] as Grid).Children[0] as ImageEx);
                    if ((_storedChallenge as GameChallenge).Game != null)
                    {
                        gameImage.Source = (_storedChallenge as GameChallenge).Game.URI;
                        (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Games.IndexOf((_storedChallenge as GameChallenge).Game);
                        (childGrid[2] as TextBox).Text = (_storedChallenge as GameChallenge).Game.Name;
                    }
                    else
                    {
                        (_storedChallenge as GameChallenge).Game = ViewModel.Games[ViewModel.Games.Count-1];
                        gameImage.Source = null;
                        (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Games.Count-1;
                        (childGrid[2] as TextBox).Text = "";
                    }
                }
                else if (isImage)
                {
                    var image = ((((childGrid[0] as StackPanel).Children[0] as Grid).Children[0] as Grid).Children[0] as ImageEx);
                    if ((_storedChallenge as ImageChallenge).Image != null)
                    {
                        image.Source = (_storedChallenge as ImageChallenge).Image.URI;
                        (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Images.IndexOf((_storedChallenge as ImageChallenge).Image);
                        (childGrid[2] as TextBox).Text = (_storedChallenge as ImageChallenge).Image.Name;
                    }
                    else
                    {
                        (_storedChallenge as ImageChallenge).Image = ViewModel.Images[ViewModel.Images.Count-1];
                        image.Source = null;
                        (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Images.Count-1;
                        (childGrid[2] as TextBox).Text = "";
                    }
                }
                else
                {
                    var musicImage = ((((childGrid[0] as StackPanel).Children[0] as Grid).Children[0] as Grid).Children[0] as ImageEx);
                    if ((_storedChallenge as MusicChallenge).Song != null)
                    {
                        musicImage.Source = (_storedChallenge as MusicChallenge).Song.URI;
                        (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Music.IndexOf((_storedChallenge as MusicChallenge).Song);
                        (childGrid[2] as TextBox).Text = (_storedChallenge as MusicChallenge).Song.Name;
                    }
                    else
                    {
                        (_storedChallenge as MusicChallenge).Song = ViewModel.Music[ViewModel.Music.Count-1];
                        musicImage.Source = null;
                        (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Music.Count-1;
                        (childGrid[2] as TextBox).Text = "";
                    }
                }
            }

            smokeGrid.Visibility = Visibility.Visible;
            animation.TryStart(smokeGrid.Children[0]);
        }

        /// <summary>  Handles the suggest box text changed event.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxTextChangedEventArgs"/> instance containing the event data.</param>
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            ViewModel.RefreshList(sender.Text.ToLower(), ChallengeList.SelectedIndex);
        }

        /// <summary>  Handles the suggest box query submitted event.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxQuerySubmittedEventArgs"/> instance containing the event data.</param>
        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.RefreshList(sender.Text.ToLower(), ChallengeList.SelectedIndex);
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
                case 4:
                    if((_storedChallenge as QuestionChallenge).Answers == null){
                        Answers newAnswer;
                        if(ChallengeList.SelectedIndex == 2) newAnswer = new Answers{ Answer = MultipleChoiceAnswer.Text,
                                                                                       DummyAnswer1 = MultipleChoiceDummy1.Text,
                                                                                       DummyAnswer2 = MultipleChoiceDummy2.Text,
                                                                                       DummyAnswer3 = MultipleChoiceDummy3.Text };
                        else newAnswer = new Answers{Answer = QuizAnswer.Text};
                        (_storedChallenge as QuestionChallenge).Answers = newAnswer;
                        (_storedChallenge as QuestionChallenge).AnswersID = (await ViewModel.AnswersDataAccess.CreateAnswersAsync(newAnswer)).AnswersID;
                    }
                    else {
                        if(ChallengeList.SelectedIndex == 2) (_storedChallenge as QuestionChallenge).Answers = new Answers
                        {
                            Answer = MultipleChoiceAnswer.Text,
                            AnswersID = (int.TryParse((_storedChallenge as QuestionChallenge).AnswersID.ToString(), out int result))?result:0,
                            DummyAnswer1 = MultipleChoiceDummy1.Text,
                            DummyAnswer2 = MultipleChoiceDummy2.Text,
                            DummyAnswer3 = MultipleChoiceDummy3.Text
                        };
                        else (_storedChallenge as QuestionChallenge).Answers = new Answers
                        {
                            Answer = QuizAnswer.Text,
                            AnswersID = (int.TryParse((_storedChallenge as QuestionChallenge).AnswersID.ToString(), out int result))?result:0
                        };
                    }
                    break;
                case 3:
                    (_storedChallenge as MusicChallenge).Song.Name = (childGrid[2] as TextBox).Text;
                    if(((childGrid[1] as ComboBox).SelectedItem as Music).ID == 0){
                        var newSong = await ViewModel.ObjectsViewModel.MediaObjectsDataAccess.CreateMediaObjectAsync((_storedChallenge as MusicChallenge).Song);
                        (_storedChallenge as MusicChallenge).SongID = newSong.ID;
                    }
                    else (_storedChallenge as MusicChallenge).SongID = (_storedChallenge as MusicChallenge).Song.ID;
                    break;
                case 5:
                case 6:
                    (_storedChallenge as ImageChallenge).Image.Name = (childGrid[2] as TextBox).Text;
                    if(((childGrid[1] as ComboBox).SelectedItem as Modell.ChallengeObjects.Image).ID == 0){
                        var newImage = await ViewModel.ObjectsViewModel.MediaObjectsDataAccess.CreateMediaObjectAsync((_storedChallenge as ImageChallenge).Image);
                        (_storedChallenge as ImageChallenge).ImageID = newImage.ID;
                    }
                    else (_storedChallenge as ImageChallenge).ImageID = (_storedChallenge as ImageChallenge).Image.ID;
                    break;
            }
            
            ViewModel.EditCommand.Execute(_storedChallenge);

            switch (ChallengeList.SelectedIndex)
            {
                case 0:
                    var audienceIndex = CreateChallengesViewModel.AudienceChallenges.IndexOf(CreateChallengesViewModel.AudienceChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    CreateChallengesViewModel.AudienceChallenges[audienceIndex] = _storedChallenge as AudienceChallenge;
                    ViewModel.FilteredAudience[audienceIndex] = _storedChallenge as AudienceChallenge;
                    break;
                case 1:
                    var crewIndex = CreateChallengesViewModel.CrewChallenges.IndexOf(CreateChallengesViewModel.CrewChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    CreateChallengesViewModel.CrewChallenges[crewIndex] = _storedChallenge as CrewChallenge;
                    ViewModel.FilteredCrew[crewIndex] = _storedChallenge as CrewChallenge;
                    break;
                case 2:
                    var multipleChoiceIndex = CreateChallengesViewModel.MultipleChoiceChallenges.IndexOf(CreateChallengesViewModel.MultipleChoiceChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    CreateChallengesViewModel.MultipleChoiceChallenges[multipleChoiceIndex] = _storedChallenge as MultipleChoiceChallenge;
                    ViewModel.FilteredMulti[multipleChoiceIndex] = _storedChallenge as MultipleChoiceChallenge;
                    break;
                case 3:
                    var musicIndex = CreateChallengesViewModel.MusicChallenges.IndexOf(CreateChallengesViewModel.MusicChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    CreateChallengesViewModel.MusicChallenges[musicIndex] = _storedChallenge as MusicChallenge;
                    ViewModel.FilteredMusic[musicIndex] = _storedChallenge as MusicChallenge;
                    break;
                case 4:
                    var quizIndex = CreateChallengesViewModel.QuizChallenges.IndexOf(CreateChallengesViewModel.QuizChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    CreateChallengesViewModel.QuizChallenges[quizIndex] = _storedChallenge as QuizChallenge;
                    ViewModel.FilteredQuiz[quizIndex] = _storedChallenge as QuizChallenge;
                    break;
                case 5:
                    var screenshotIndex = CreateChallengesViewModel.ScreenshotChallenges.IndexOf(CreateChallengesViewModel.ScreenshotChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    CreateChallengesViewModel.ScreenshotChallenges[screenshotIndex] = _storedChallenge as ScreenshotChallenge;
                    ViewModel.FilteredScreen[screenshotIndex] = _storedChallenge as ScreenshotChallenge;
                    break;
                case 6:
                    var silhouetteIndex = CreateChallengesViewModel.SilhouetteChallenges.IndexOf(CreateChallengesViewModel.SilhouetteChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    CreateChallengesViewModel.SilhouetteChallenges[silhouetteIndex] = _storedChallenge as SilhouetteChallenge;
                    ViewModel.FilteredSilhu[silhouetteIndex] = _storedChallenge as SilhouetteChallenge;
                    break;
                case 7:
                    var sologameIndex = CreateChallengesViewModel.SologameChallenges.IndexOf(CreateChallengesViewModel.SologameChallenges.Where(obj => obj.ChallengeID == _storedChallenge.ChallengeID).First());
                    CreateChallengesViewModel.SologameChallenges[sologameIndex] = _storedChallenge as SologameChallenge;
                    ViewModel.FilteredSolo[sologameIndex] = _storedChallenge as SologameChallenge;
                    break;
            }

            ViewModel.CreateNewObjectPlaceholder();

            BackButton_Click(sender, e);
        }

        /// <summary>Handles the SelectionChanged event of the ChallengeList control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ChallengeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.RefreshList((((((ChallengeList.SelectedItem as PivotItem)
                .Content as Grid).Children[0] as Grid).Children[0] as StackPanel)
                .Children[0] as AutoSuggestBox).Text.ToLower(),
                ChallengeList.SelectedIndex);
        }

        /// <summary>Handles the SelectionChanged event of the ComboBox control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var collection = (ChallengeList.SelectedItem as PivotItem).Content as Grid;
            var smokeGrid = (collection.Children[2] as Grid);
            var children = ((smokeGrid.Children[0] as Grid).Children[0] as StackPanel).Children[0] as Grid;

            var childGrid = (children.Children[2] as Grid).Children;
            var image = ((((childGrid[0] as StackPanel).Children[0] as Grid).Children[0] as Grid).Children[0] as ImageEx);

            switch (ChallengeList.SelectedIndex)
            {
                case 0:
                case 1:
                case 7:
                    (_storedChallenge as GameChallenge).Game = ViewModel.Games[(sender as ComboBox).SelectedIndex];
                    image.Source = (_storedChallenge as GameChallenge).Game.URI;
                    (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Games.IndexOf((_storedChallenge as GameChallenge).Game);
                    (childGrid[2] as TextBox).Text = (_storedChallenge as GameChallenge).Game.Name;
                    break;
                case 3:
                    (_storedChallenge as MusicChallenge).Song = ViewModel.Music[(sender as ComboBox).SelectedIndex];
                    image.Source = (_storedChallenge as MusicChallenge).Song.URI;
                    (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Music.IndexOf((_storedChallenge as MusicChallenge).Song);
                    (childGrid[2] as TextBox).Text = (_storedChallenge as MusicChallenge).Song.Name;
                    break;
                case 5:
                case 6:
                    (_storedChallenge as ImageChallenge).Image = ViewModel.Images[(sender as ComboBox).SelectedIndex];
                    image.Source = (_storedChallenge as ImageChallenge).Image.URI;
                    (childGrid[1] as ComboBox).SelectedIndex = ViewModel.Images.IndexOf((_storedChallenge as ImageChallenge).Image);
                    (childGrid[2] as TextBox).Text = (_storedChallenge as ImageChallenge).Image.Name;
                    break;
            }
        }

        /// <summary>Handles the Tapped event of the StackPanel control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.Input.TappedRoutedEventArgs"/> instance containing the event data.</param>
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
                        var gameImage = ((((childGrid[0] as StackPanel).Children[0] as Grid).Children[0] as Grid).Children[0] as ImageEx);
                        gameImage.Source = (_storedChallenge as GameChallenge).Game.URI;
                        break;
                    case 3:
                        (_storedChallenge as MusicChallenge).Song.URI = await ViewModel.ObjectsViewModel.ImagesDataAccess.PostImageAsync(file);
                        var songImage = ((((childGrid[0] as StackPanel).Children[0] as Grid).Children[0] as Grid).Children[0] as ImageEx);
                        songImage.Source = (_storedChallenge as MusicChallenge).Song.URI;
                        break;
                    case 5:
                    case 6:
                        (_storedChallenge as ImageChallenge).Image.URI = await ViewModel.ObjectsViewModel.ImagesDataAccess.PostImageAsync(file);
                        var image = ((((childGrid[0] as StackPanel).Children[0] as Grid).Children[0] as Grid).Children[0] as ImageEx);
                        image.Source = (_storedChallenge as ImageChallenge).Image.URI;
                        break;
                }
            }
        }
        /// <summary>  Makes the refresh button avctive .</summary>
        private void RefreshButtonActive()
        {
            RefreshChallengePage.Visibility = Visibility.Visible;
        }
        /// <summary>  Deactivates the refresh button.</summary>
        private void RefreshButtonDeactivate()
        {
            RefreshChallengePage.Visibility = Visibility.Collapsed;
        }

        /// <summary>  Tries to reload the page when the <see cref="RefreshMainPage"/> button is pressed.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void RefreshChallengePage_Click(object sender, RoutedEventArgs e)
        {
            CreateChallengesPage_Loaded();
        }

    }
}
