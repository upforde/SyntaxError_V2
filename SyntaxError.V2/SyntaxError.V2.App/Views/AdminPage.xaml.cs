using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class AdminPage : Page
    {
        private bool IsInPlayState = false;
        private bool IsNextSyntaxError = false;
        const int SyntaxErrorMaxVal = 25;
        private int SyntaxErrorCounter = SyntaxErrorMaxVal;

        public AdminViewModel ViewModel { get; } = new AdminViewModel();
        public GamePage GamePage;
        public List<AudienceChallenge> AudienceChallenges = new List<AudienceChallenge>();
        public List<AudienceChallenge> UsedAudienceChallenges = new List<AudienceChallenge>();
        public List<CrewChallenge> CrewChallenges = new List<CrewChallenge>();
        public List<CrewChallenge> UsedCrewChallenges = new List<CrewChallenge>();
        public List<MultipleChoiceChallenge> MultipleChoiceChallenges = new List<MultipleChoiceChallenge>();
        public List<MultipleChoiceChallenge> UsedMultipleChoiceChallenges = new List<MultipleChoiceChallenge>();
        public List<MusicChallenge> MusicChallenges = new List<MusicChallenge>();
        public List<MusicChallenge> UsedMusicChallenges = new List<MusicChallenge>();
        public List<QuizChallenge> QuizChallenges = new List<QuizChallenge>();
        public List<QuizChallenge> UsedQuizChallenges = new List<QuizChallenge>();
        public List<ScreenshotChallenge> ScreenshotChallenges = new List<ScreenshotChallenge>();
        public List<ScreenshotChallenge> UsedScreenshotChallenges = new List<ScreenshotChallenge>();
        public List<SilhouetteChallenge> SilhouetteChallenges = new List<SilhouetteChallenge>();
        public List<SilhouetteChallenge> UsedSilhouetteChallenges = new List<SilhouetteChallenge>();
        public List<SologameChallenge> SologameChallenges = new List<SologameChallenge>();
        public List<SologameChallenge> UsedSologameChallenges = new List<SologameChallenge>();

        public AudienceChallenge CurrentAudienceChallenge { get; set; }
        public CrewChallenge CurrentCrewChallenge { get; set; }
        public MultipleChoiceChallenge CurrentMultipleChoiceChallenge { get; set; }
        public MusicChallenge CurrentMusicChallenge { get; set; }
        public QuizChallenge CurrentQuizChallenge { get; set; }
        public ScreenshotChallenge CurrentScreenshotChallenge { get; set; }
        public SilhouetteChallenge CurrentSilhouetteChallenge { get; set; }
        public SologameChallenge CurrentSologameChallenge { get; set; }

        public AdminPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GamePage = (GamePage) e.Parameter;
            GamePage.RandomSelectDone += GamePage_RandomSelectDone;
            AddAllChallenges();
            RollAll();
            UpdateAllTextFields();
        }

        private async void GamePage_RandomSelectDone(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            });
        }

        private void AddAllChallenges()
        {
            AudienceChallenges.AddRange(GamePage.AudienceChallenges);
            CrewChallenges.AddRange(GamePage.CrewChallenges);
            MultipleChoiceChallenges.AddRange(GamePage.MultipleChoiceChallenges);
            MusicChallenges.AddRange(GamePage.MusicChallenges);
            QuizChallenges.AddRange(GamePage.QuizChallenges);
            ScreenshotChallenges.AddRange(GamePage.ScreenshotChallenges);
            SilhouetteChallenges.AddRange(GamePage.SilhouetteChallenges);
            SologameChallenges.AddRange(GamePage.SologameChallenges);

            foreach (ListItemMainPage challenge in GamePage.GameProfileAndChallenges.Challenges)
            {
                if (challenge.Challenge != null)
                    if (challenge.IsChallengeCompleted)
                        switch (challenge.Challenge.GetDiscriminator())
                        {
                            case "AudienceChallenge":
                                UsedAudienceChallenges.Add(challenge.Challenge as AudienceChallenge);
                                break;
                            case "CrewChallenge":
                                UsedCrewChallenges.Add(challenge.Challenge as CrewChallenge);
                                break;
                            case "MultipleChoiceChallenge":
                                UsedMultipleChoiceChallenges.Add(challenge.Challenge as MultipleChoiceChallenge);
                                break;
                            case "MusicChallenge":
                                UsedMusicChallenges.Add(challenge.Challenge as MusicChallenge);
                                break;
                            case "QuizChallenge":
                                UsedQuizChallenges.Add(challenge.Challenge as QuizChallenge);
                                break;
                            case "ScreenshotChallenge":
                                UsedScreenshotChallenges.Add(challenge.Challenge as ScreenshotChallenge);
                                break;
                            case "SilhouetteChallenge":
                                UsedSilhouetteChallenges.Add(challenge.Challenge as SilhouetteChallenge);
                                break;
                            case "SologameChallenge":
                                UsedSologameChallenges.Add(challenge.Challenge as SologameChallenge);
                                break;
                        }
            }
        }
        
        private void RollAll()
        {
            RollForAudienceChallengeRecursive();
            RollForCrewChallengeRecursive();
            RollForMultipleChoiceChallengeRecursive();
            RollForMusicChallengeRecursive();
            RollForQuizChallengeRecursive();
            RollForScreenshotChallengeRecursive();
            RollForSilhouetteChallengeRecursive();
            RollForSoloGameChallengeRecursive();
        }

        private void RollForAudienceChallengeRecursive()
        {
            if (AudienceChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, AudienceChallenges.Count);
                if (UsedAudienceChallenges.Count < AudienceChallenges.Count)
                {
                    if (!UsedAudienceChallenges.Contains(AudienceChallenges[rnd])) CurrentAudienceChallenge = AudienceChallenges[rnd];
                    else RollForAudienceChallengeRecursive();
                }
                else CurrentAudienceChallenge = null;
            }
            else CurrentAudienceChallenge = null;
        }
        private void RollForCrewChallengeRecursive()
        {
            if (CrewChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, CrewChallenges.Count);
                if (UsedCrewChallenges.Count < CrewChallenges.Count)
                {
                    if (!UsedCrewChallenges.Contains(CrewChallenges[rnd])) CurrentCrewChallenge = CrewChallenges[rnd];
                    else RollForCrewChallengeRecursive();
                }
                else CurrentCrewChallenge = null;
            }
            else CurrentCrewChallenge = null;
        }
        private void RollForMultipleChoiceChallengeRecursive()
        {
            if (MultipleChoiceChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, MultipleChoiceChallenges.Count);
                if (UsedMultipleChoiceChallenges.Count < MultipleChoiceChallenges.Count)
                {
                    if (!UsedMultipleChoiceChallenges.Contains(MultipleChoiceChallenges[rnd])) CurrentMultipleChoiceChallenge = MultipleChoiceChallenges[rnd];
                    else RollForMultipleChoiceChallengeRecursive();
                }
                else CurrentMultipleChoiceChallenge = null;
            }
            else CurrentMultipleChoiceChallenge = null;
        }
        private void RollForMusicChallengeRecursive()
        {
            if (MusicChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, MusicChallenges.Count);
                if (UsedMusicChallenges.Count < MusicChallenges.Count)
                {
                    if (!UsedMusicChallenges.Contains(MusicChallenges[rnd])) CurrentMusicChallenge = MusicChallenges[rnd];
                    else RollForMusicChallengeRecursive();
                }
                else CurrentMusicChallenge = null;
            }
            else CurrentMusicChallenge = null;
        }
        private void RollForQuizChallengeRecursive()
        {
            if (QuizChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, QuizChallenges.Count);
                if (UsedQuizChallenges.Count < QuizChallenges.Count)
                {
                    if (!UsedQuizChallenges.Contains(QuizChallenges[rnd])) CurrentQuizChallenge = QuizChallenges[rnd];
                    else RollForQuizChallengeRecursive();
                }
                else CurrentQuizChallenge = null;
            }
            else CurrentQuizChallenge = null;
        }
        private void RollForScreenshotChallengeRecursive()
        {
            if (ScreenshotChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, ScreenshotChallenges.Count);
                if (UsedScreenshotChallenges.Count < ScreenshotChallenges.Count)
                {
                    if (!UsedScreenshotChallenges.Contains(ScreenshotChallenges[rnd])) CurrentScreenshotChallenge = ScreenshotChallenges[rnd];
                    else RollForScreenshotChallengeRecursive();
                }
                else CurrentScreenshotChallenge = null;
            }
            else CurrentScreenshotChallenge = null;
        }
        private void RollForSilhouetteChallengeRecursive()
        {
            if (SilhouetteChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, SilhouetteChallenges.Count);
                if (UsedSilhouetteChallenges.Count < SilhouetteChallenges.Count)
                {
                    if (!UsedSilhouetteChallenges.Contains(SilhouetteChallenges[rnd])) CurrentSilhouetteChallenge = SilhouetteChallenges[rnd];
                    else RollForSilhouetteChallengeRecursive();
                }
                else CurrentSilhouetteChallenge = null;
            }
            else CurrentSilhouetteChallenge = null;
        }
        private void RollForSoloGameChallengeRecursive()
        {
            if (SologameChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, SologameChallenges.Count);
                if (UsedSologameChallenges.Count < SologameChallenges.Count)
                {
                    if (!UsedSologameChallenges.Contains(SologameChallenges[rnd])) CurrentSologameChallenge = SologameChallenges[rnd];
                    else RollForSoloGameChallengeRecursive();
                }
                else CurrentSologameChallenge = null;
            }
            else CurrentSologameChallenge = null;
        }
        
        private void UpdateAllTextFields()
        {
            GetAudienceChallengeGameFromDBAsync();
            GetCrewChallengeGameAndMemberFromDBAsync();
            GetMultipleChoiceAnswersFromDBAsync();
            GetMusicChallengeSongFromDBAsync();
            GetQuizChallengeAnswerFromDBAsync();
            GetScreenshotChallengeScreenshotFromDBAsync();
            GetSilhouetteChallengeSilhouetteFromDBAsync();
            GetSologameChallengeGameFromDBAsync();
        }

        private async void GetAudienceChallengeGameFromDBAsync()
        {
            if (CurrentAudienceChallenge != null)
            {
                if (CurrentAudienceChallenge.GameID != null)
                {
                    if (CurrentAudienceChallenge.Game == null) CurrentAudienceChallenge.Game = await ViewModel.LoadObjectFromDBAsync(CurrentAudienceChallenge.GameID, "Game") as Game;
                    AudienceNextGame.Text = CurrentAudienceChallenge.Game.Name;
                }
            } 
            else
            {
                AudienceNextGame.Text = "No more challenges ;(";
                AudienceButton.IsEnabled = false;
                GamePage.ToggleAudienceSaturation();
            }
        }
        private async void GetCrewChallengeGameAndMemberFromDBAsync()
        {
            if (CurrentCrewChallenge != null)
            {
                if (CurrentCrewChallenge.GameID != null)
                {
                    if (CurrentCrewChallenge.Game == null)
                        CurrentCrewChallenge.Game = await ViewModel.LoadObjectFromDBAsync(CurrentCrewChallenge.GameID, "Game") as Game;
                    CrewNextGame.Text = CurrentCrewChallenge.Game.Name;
                } else CrewNextGame.Text = "";
                if (CurrentCrewChallenge.CrewMemberID != null)
                {
                    if (CurrentCrewChallenge.CrewMember == null)
                        CurrentCrewChallenge.CrewMember = await ViewModel.LoadCrewMemberFromDBAsync(CurrentCrewChallenge.CrewMemberID);
                    CrewNextCrew.Text = CurrentCrewChallenge.CrewMember.CrewTag;
                } else CrewNextCrew.Text = "";
            }
            else
            {
                CrewNextGame.Text = "No more challenges ;(";
                CrewNextCrew.Text = "";
                CrewButton.IsEnabled = false;
                GamePage.ToggleCrewSaturation();
            }
            
        }
        private async void GetMultipleChoiceAnswersFromDBAsync()
        {
            if (CurrentMultipleChoiceChallenge != null)
            {
                if (CurrentMultipleChoiceChallenge.AnswersID != null)
                {
                    if (CurrentMultipleChoiceChallenge.Answers == null)
                        CurrentMultipleChoiceChallenge.Answers = await ViewModel.LoadAnswersFromDBAsync(CurrentMultipleChoiceChallenge.AnswersID);
                    MultipleChoiceQuiestion.Text = CurrentMultipleChoiceChallenge.ChallengeTask;
                    MultipleChoiceAnswer.Text = CurrentMultipleChoiceChallenge.Answers.Answer;
                }
            }
            else
            {
                MultipleChoiceQuiestion.Text = "";
                MultipleChoiceAnswer.Text = "No more challenges ;(";
                MultipleChoiceButton.IsEnabled = false;
                GamePage.ToggleMultipleChoiceSaturation();
            }
        }
        private async void GetMusicChallengeSongFromDBAsync()
        {
            if (CurrentMusicChallenge != null)
            {
                if (CurrentMusicChallenge.SongID != null)
                {
                    if (CurrentMusicChallenge.Song == null)
                        CurrentMusicChallenge.Song = await ViewModel.LoadObjectFromDBAsync(CurrentMusicChallenge.SongID, "Music") as Music;
                    MusicNextSong.Text = CurrentMusicChallenge.Song.Name;
                }
            }
            else
            {
                MusicNextSong.Text = "No more challenges ;(";
                MusicButton.IsEnabled = false;
                GamePage.ToggleMusicSaturation();
            }
        }
        private async void GetQuizChallengeAnswerFromDBAsync()
        {
            if (CurrentQuizChallenge != null)
            {
                if (CurrentQuizChallenge.AnswersID != null)
                {
                    if (CurrentQuizChallenge.Answers == null)
                        CurrentQuizChallenge.Answers = await ViewModel.LoadAnswersFromDBAsync(CurrentQuizChallenge.AnswersID);
                    QuizQuiestion.Text = CurrentQuizChallenge.ChallengeTask;
                    QuizAnswer.Text = CurrentQuizChallenge.Answers.Answer;
                }
            }
            else
            {
                QuizQuiestion.Text = "";
                QuizAnswer.Text = "No more challenges ;(";
                QuizButton.IsEnabled = false;
                GamePage.ToggleQuizSaturation();
            }
        }
        private async void GetScreenshotChallengeScreenshotFromDBAsync()
        {
            if (CurrentScreenshotChallenge != null)
            {
                if (CurrentScreenshotChallenge.ImageID != null)
                {
                    if (CurrentScreenshotChallenge.Image == null)
                        CurrentScreenshotChallenge.Image = await ViewModel.LoadObjectFromDBAsync(CurrentScreenshotChallenge.ImageID, "Image") as Modell.ChallengeObjects.Image;
                    ScreenshotNextScreenshot.Text = CurrentScreenshotChallenge.Image.Name;
                }
            }
            else
            {
                ScreenshotNextScreenshot.Text = "No more challenges ;(";
                ScreenshotButton.IsEnabled = false;
                GamePage.ToggleScreenshotSaturation();
            }
        }
        private async void GetSilhouetteChallengeSilhouetteFromDBAsync()
        {
            if (CurrentSilhouetteChallenge != null)
            {
                if (CurrentSilhouetteChallenge.ImageID != null)
                {
                    if (CurrentSilhouetteChallenge.Image == null)
                        CurrentSilhouetteChallenge.Image = await ViewModel.LoadObjectFromDBAsync(CurrentSilhouetteChallenge.ImageID, "Image") as Modell.ChallengeObjects.Image;
                    SilhouetteNextSilhouette.Text = CurrentSilhouetteChallenge.Image.Name;
                }
            }
            else
            {
                SilhouetteNextSilhouette.Text = "No more challenges ;(";
                SilhouetteButton.IsEnabled = false;
                GamePage.ToggleSilhouetteSaturation();
            }
        }
        private async void GetSologameChallengeGameFromDBAsync()
        {
            if (CurrentSologameChallenge != null)
            {
                if (CurrentSologameChallenge.GameID != null)
                {
                    if (CurrentSologameChallenge.Game == null)
                        CurrentSologameChallenge.Game = await ViewModel.LoadObjectFromDBAsync(CurrentSologameChallenge.GameID, "Game") as Game;
                    SologameNextGame.Text = CurrentSologameChallenge.Game.Name;
                }
            }
            else
            {
                SologameNextGame.Text = "No more challenges ;(";
                SologameButton.IsEnabled = false;
                GamePage.ToggleSologameSaturation();
            }
        }
        
        private void Button_Click_FixSyntaxError(object sender, RoutedEventArgs e)
        {
            SyntaxErrorCounter = SyntaxErrorMaxVal;
            IsNextSyntaxError = false;
            SyntaxErrorFixButton.Visibility = Visibility.Collapsed;
            GamePage.ToggleSyntaxErrorFix();
        }

        private void Button_Click_Audience(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleAudienceChallenge();
            if (!IsInPlayState)
            {
                RandomButton.IsEnabled = false;
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_RerollAudience(object sender, RoutedEventArgs e)
        {
            RollForAudienceChallengeRecursive();
            GetAudienceChallengeGameFromDBAsync();
        }
        private void Button_Click_Crew(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleCrewChallenge();
            if (!IsInPlayState)
            {
                RandomButton.IsEnabled = false;
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_RerollCrew(object sender, RoutedEventArgs e)
        {
            RollForCrewChallengeRecursive();
            GetCrewChallengeGameAndMemberFromDBAsync();
        }
        private void Button_Click_Multiple(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleMultipleChoiceChallenge();
            if (!IsInPlayState)
            {
                RandomButton.IsEnabled = false;
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_RerollMultiple(object sender, RoutedEventArgs e)
        {
            RollForMultipleChoiceChallengeRecursive();
            GetMultipleChoiceAnswersFromDBAsync();
        }
        private void Button_Click_Music(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleMusicChallenge();
            if (!IsInPlayState)
            {
                RandomButton.IsEnabled = false;
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_RerollMusic(object sender, RoutedEventArgs e)
        {
            RollForMusicChallengeRecursive();
            GetMusicChallengeSongFromDBAsync();
        }
        private void Button_Click_Quiz(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleQuizChallenge();
            if (!IsInPlayState)
            {
                RandomButton.IsEnabled = false;
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_RerollQuiz(object sender, RoutedEventArgs e)
        {
            RollForQuizChallengeRecursive();
            GetQuizChallengeAnswerFromDBAsync();
        }
        private void Button_Click_Screenshot(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleScreenshotChallenge();
            if (!IsInPlayState)
            {
                RandomButton.IsEnabled = false;
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_RerollScreenshot(object sender, RoutedEventArgs e)
        {
            RollForScreenshotChallengeRecursive();
            GetScreenshotChallengeScreenshotFromDBAsync();
        }
        private void Button_Click_Silhouette(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleSilhouetteChallenge();
            if (!IsInPlayState)
            {
                RandomButton.IsEnabled = false;
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_RerollSilhouette(object sender, RoutedEventArgs e)
        {
            RollForSilhouetteChallengeRecursive();
            GetSilhouetteChallengeSilhouetteFromDBAsync();
        }
        private void Button_Click_Sologame(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleSologameChallenge();
            if (!IsInPlayState)
            {
                RandomButton.IsEnabled = false;
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_RerollSologame(object sender, RoutedEventArgs e)
        {
            RollForSoloGameChallengeRecursive();
            GetSologameChallengeGameFromDBAsync();
        }
        
        private void Button_Click_Play(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (GamePage.CurrentChallenge != null)
            {
                IsInPlayState = true;
            
                RandomButton.IsEnabled = false;
                PlayButton.IsEnabled = false;
                DeselectButton.IsEnabled = false;

                switch (GamePage.CurrentChallenge)
                {
                    case 0:
                        GamePage.ActuateAudienceChallenge(CurrentAudienceChallenge);
                        DoneButton.IsEnabled = true;
                        break;
                    case 1:
                        GamePage.ActuateCrewChallenge(CurrentCrewChallenge);
                        DoneButton.IsEnabled = true;
                        break;
                    case 2:
                        GamePage.ActuateMultipleChoiceChallenge(CurrentMultipleChoiceChallenge);
                        AnswerButton.IsEnabled = true;
                        break;
                    case 3:
                        GamePage.ActuateMusicChallenge(CurrentMusicChallenge);
                        AnswerButton.IsEnabled = true;
                        break;
                    case 4:
                        GamePage.ActuateQuizChallenge(CurrentQuizChallenge);
                        AnswerButton.IsEnabled = true;
                        break;
                    case 5:
                        GamePage.ActuateScreenshotChallenge(CurrentScreenshotChallenge);
                        AnswerButton.IsEnabled = true;
                        break;
                    case 6:
                        GamePage.ActuateSilhouetteChallenge(CurrentSilhouetteChallenge);
                        AnswerButton.IsEnabled = true;
                        break;
                    case 7:
                        GamePage.ActuateSologameChallenge(CurrentSologameChallenge);
                        DoneButton.IsEnabled = true;
                        break;
                    default:
                        break;
                }
                GamePage.TogglePlayScreen();
            }
        }
        private void Button_Click_Deselect(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PlayButton.IsEnabled = false;
            DeselectButton.IsEnabled = false;
            RandomButton.IsEnabled = true;

            GamePage.ToggleDeselect();
        }
        private void Button_Click_Answer(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AnswerButton.IsEnabled = false;
            DoneButton.IsEnabled = true;

            switch (GamePage.CurrentChallenge)
            {
                case 2:
                    GamePage.AnswerMultipleChoiceChallenge(CurrentMultipleChoiceChallenge);
                    break;
                case 3:
                    GamePage.AnswerMusicChallenge(CurrentMusicChallenge);
                    break;
                case 4:
                    GamePage.AnswerQuizChallenge(CurrentQuizChallenge);
                    break;
                case 5:
                    GamePage.AnswerScreenshotChallenge(CurrentScreenshotChallenge);
                    break;
                case 6:
                    GamePage.AnswerSilhouetteChallenge(CurrentSilhouetteChallenge);
                    break;
                default:
                    break;
            }
        }
        private void Button_Click_Done(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DoneButton.IsEnabled = false;

            switch (GamePage.CurrentChallenge)
            {
                case 0:
                    UsedAudienceChallenges.Add(CurrentAudienceChallenge);
                    UpdateSaveGame(CurrentAudienceChallenge);
                    RollForAudienceChallengeRecursive();
                    GetAudienceChallengeGameFromDBAsync();
                    break;
                case 1:
                    UsedCrewChallenges.Add(CurrentCrewChallenge);
                    UpdateSaveGame(CurrentCrewChallenge);
                    RollForCrewChallengeRecursive();
                    GetCrewChallengeGameAndMemberFromDBAsync();
                    break;
                case 2:
                    UsedMultipleChoiceChallenges.Add(CurrentMultipleChoiceChallenge);
                    UpdateSaveGame(CurrentMultipleChoiceChallenge);
                    RollForMultipleChoiceChallengeRecursive();
                    GetMultipleChoiceAnswersFromDBAsync();
                    break;
                case 3:
                    UsedMusicChallenges.Add(CurrentMusicChallenge);
                    UpdateSaveGame(CurrentMusicChallenge);
                    RollForMusicChallengeRecursive();
                    GetMusicChallengeSongFromDBAsync();
                    break;
                case 4:
                    UsedQuizChallenges.Add(CurrentQuizChallenge);
                    UpdateSaveGame(CurrentQuizChallenge);
                    RollForQuizChallengeRecursive();
                    GetQuizChallengeAnswerFromDBAsync();
                    break;
                case 5:
                    UsedScreenshotChallenges.Add(CurrentScreenshotChallenge);
                    UpdateSaveGame(CurrentScreenshotChallenge);
                    RollForScreenshotChallengeRecursive();
                    GetScreenshotChallengeScreenshotFromDBAsync();
                    break;
                case 6:
                    UsedSilhouetteChallenges.Add(CurrentSilhouetteChallenge);
                    UpdateSaveGame(CurrentSilhouetteChallenge);
                    RollForSilhouetteChallengeRecursive();
                    GetSilhouetteChallengeSilhouetteFromDBAsync();
                    break;
                case 7:
                    UsedSologameChallenges.Add(CurrentSologameChallenge);
                    UpdateSaveGame(CurrentSologameChallenge);
                    RollForSoloGameChallengeRecursive();
                    GetSologameChallengeGameFromDBAsync();
                    break;
                default:
                    break;
            }
            GamePage.ToggleMainScreen();
            
            IsInPlayState = false;
            RandomButton.IsEnabled = true;

            if(IsNextSyntaxError)
            {
                SyntaxErrorShakeImage.Visibility = Visibility.Collapsed;
                CauseSyntaxError();
            }
            else RollForNextSyntaxError();
        }

        private void Button_Click_Random(object sender, RoutedEventArgs e)
        {
            PlayButton.IsEnabled = false;
            DeselectButton.IsEnabled = false;
            RandomButton.IsEnabled = false;
            GamePage.ToggleRandomSelection();
        }

        private void CauseSyntaxError()
        {
            SyntaxErrorFixButton.Visibility = Visibility.Visible;
            GamePage.ToggleSyntaxError();
        }
        private void RollForNextSyntaxError()
        {
            var test = SyntaxErrorCounter;
            int rnd = GamePage.RandomNumber(1, SyntaxErrorCounter+1);
            if (rnd == SyntaxErrorCounter)
            {
                IsNextSyntaxError = true;
                SyntaxErrorShakeImage.Visibility = Visibility.Visible;
                ForeverRotAnim.Begin();
            }
            else SyntaxErrorCounter--;
        }

        private void UpdateSaveGame(ChallengeBase currentChallenge)
        {
            var newEntry = new UsingChallenge
            {
                ChallengeID = currentChallenge.ChallengeID,
                UsingID = GamePage.GameProfile.SaveGameID
            };

            GamePage.GameProfile.SaveGame.Challenges.Add(newEntry);
            ViewModel.UpdateSaveGame.Execute(newEntry);
        }
    }
}
