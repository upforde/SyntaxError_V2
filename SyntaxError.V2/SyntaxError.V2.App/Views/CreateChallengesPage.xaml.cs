using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class CreateChallengesPage : Page
    {
        /// <summary>  A command to add a new Challenge to the database and list.</summary>
        /// <value>The add new object command.</value>
        public ICommand AddNewChallengeCommand { get; set; }

        public CreateChallengesViewModel ViewModel { get; } = new CreateChallengesViewModel();

        public CreateChallengesPage()
        {
            InitializeComponent();

            AddNewChallengeCommand = new RelayCommand<ChallengeBase>(async param =>
                                                    {
                                                        param = await ViewModel.challengesDataAccess.CreateChallengeAsync(param);

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
        }

        private async void CreateChallengesPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.LoadChallengesFromDBAsync();
        }

        private void AppBarButton_SelectionMode_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AppBarButton_DeleteSelected_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

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

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
