using System;
using System.Collections.Generic;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class AdminPage : Page
    {
        private bool IsInPlayState = false;
        private bool IsNextSyntaxError = false;
        const int SyntaxErrorMaxVal = 1;
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
            
            AddAllChallenges();

            RollAll();

            UpdateAllTextFields();
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
        }
        
        private void RollAll()
        {
            CurrentAudienceChallenge = RollForAudienceChallengeRecursive();
            CurrentCrewChallenge = RollForCrewChallengeRecursive();
            CurrentMultipleChoiceChallenge = RollForMultipleChoiceChallengeRecursive();
            CurrentMusicChallenge = RollForMusicChallengeRecursive();
            CurrentQuizChallenge = RollForQuizChallengeRecursive();
            CurrentScreenshotChallenge = RollForScreenshotChallengeRecursive();
            CurrentSilhouetteChallenge = RollForSilhouetteChallengeRecursive();
            CurrentSologameChallenge = RollForSoloGameChallengeRecursive();
        }

        private AudienceChallenge RollForAudienceChallengeRecursive()
        {
            if (AudienceChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, AudienceChallenges.Count-1);
                if (UsedAudienceChallenges.Count < AudienceChallenges.Count)
                {
                    if(!UsedAudienceChallenges.Contains(AudienceChallenges[rnd]))
                    {
                        return AudienceChallenges[rnd];
                    }
                    RollForAudienceChallengeRecursive();
                }
            }
            return null;
        }
        private CrewChallenge RollForCrewChallengeRecursive()
        {
            if (CrewChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, CrewChallenges.Count-1);
                if (UsedCrewChallenges.Count < CrewChallenges.Count)
                {
                    if (!UsedCrewChallenges.Contains(CrewChallenges[rnd]))
                    {
                        return CrewChallenges[rnd];
                    }
                    RollForCrewChallengeRecursive();
                }
            }
            return null;
        }
        private MultipleChoiceChallenge RollForMultipleChoiceChallengeRecursive()
        {
            if (MultipleChoiceChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, MultipleChoiceChallenges.Count-1);
                if (UsedMultipleChoiceChallenges.Count < MultipleChoiceChallenges.Count)
                {
                    if (!UsedMultipleChoiceChallenges.Contains(MultipleChoiceChallenges[rnd]))
                    {
                        return MultipleChoiceChallenges[rnd];
                    }
                    RollForMultipleChoiceChallengeRecursive();
                }
            }
            return null;
        }
        private MusicChallenge RollForMusicChallengeRecursive()
        {
            if (MusicChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, MusicChallenges.Count-1);
                if (UsedMusicChallenges.Count < MusicChallenges.Count)
                {
                    if (!UsedMusicChallenges.Contains(MusicChallenges[rnd]))
                    {
                        return MusicChallenges[rnd];
                    }
                    RollForMusicChallengeRecursive();
                }
            }
            return null;
        }
        private QuizChallenge RollForQuizChallengeRecursive()
        {
            if (QuizChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, QuizChallenges.Count-1);
                if (UsedQuizChallenges.Count < QuizChallenges.Count)
                {
                    if (!UsedQuizChallenges.Contains(QuizChallenges[rnd]))
                    {
                        return QuizChallenges[rnd];
                    }
                    RollForQuizChallengeRecursive();
                }
            }
            return null;
        }
        private ScreenshotChallenge RollForScreenshotChallengeRecursive()
        {
            if (ScreenshotChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, ScreenshotChallenges.Count-1);
                if (UsedScreenshotChallenges.Count < ScreenshotChallenges.Count)
                {
                    if (!UsedScreenshotChallenges.Contains(ScreenshotChallenges[rnd]))
                    {
                        return ScreenshotChallenges[rnd];
                    }
                    RollForScreenshotChallengeRecursive();
                }
            }
            return null;
        }
        private SilhouetteChallenge RollForSilhouetteChallengeRecursive()
        {
            if (SilhouetteChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, SilhouetteChallenges.Count-1);
                if (UsedSilhouetteChallenges.Count < SilhouetteChallenges.Count)
                {
                    if (!UsedSilhouetteChallenges.Contains(SilhouetteChallenges[rnd]))
                    {
                        return SilhouetteChallenges[rnd];
                    }
                    RollForSilhouetteChallengeRecursive();
                }
            }
            return null;
        }
        private SologameChallenge RollForSoloGameChallengeRecursive()
        {
            if (SologameChallenges.Count != 0)
            {
                int rnd = GamePage.RandomNumber(0, SologameChallenges.Count-1);
                if (UsedSologameChallenges.Count < SologameChallenges.Count)
                {
                    if (!UsedSologameChallenges.Contains(SologameChallenges[rnd]))
                    {
                        return SologameChallenges[rnd];
                    }
                    RollForSoloGameChallengeRecursive();
                }
            }
            return null;
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
                    CurrentAudienceChallenge.Game = await ViewModel.LoadObjectFromDBAsync(CurrentAudienceChallenge.GameID, "Game") as Game;
                    AudienceNextGame.Text = CurrentAudienceChallenge.Game.Name;
                }
            } else
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
                    CurrentCrewChallenge.Game = await ViewModel.LoadObjectFromDBAsync(CurrentCrewChallenge.GameID, "Game") as Game;
                    CrewNextGame.Text = CurrentCrewChallenge.Game.Name;
                }
                if (CurrentCrewChallenge.CrewMemberID != null)
                {
                    CurrentCrewChallenge.CrewMember = await ViewModel.LoadCrewMemberFromDBAsync(CurrentCrewChallenge.CrewMemberID);
                    CrewNextCrew.Text = CurrentCrewChallenge.CrewMember.CrewTag;
                }
            } else
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
                    CurrentMultipleChoiceChallenge.Answers = await ViewModel.LoadAnswersFromDBAsync(CurrentMultipleChoiceChallenge.AnswersID);
                    MultipleChoiceAnswer.Text = CurrentMultipleChoiceChallenge.Answers.Answer;
                }
            } else
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
                    CurrentMusicChallenge.Song = await ViewModel.LoadObjectFromDBAsync(CurrentMusicChallenge.SongID, "Music") as Music;
                    MusicNextSong.Text = CurrentMusicChallenge.Song.Name;
                }
            } else
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
                    CurrentQuizChallenge.Answers = await ViewModel.LoadAnswersFromDBAsync(CurrentQuizChallenge.AnswersID);
                    QuizAnswer.Text = CurrentQuizChallenge.Answers.Answer;
                }
            } else
            {
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
                    CurrentScreenshotChallenge.Image = await ViewModel.LoadObjectFromDBAsync(CurrentScreenshotChallenge.ImageID, "Image") as Modell.ChallengeObjects.Image;
                    ScreenshotNextScreenshot.Text = CurrentScreenshotChallenge.Image.Name;
                }
            } else
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
                    CurrentSilhouetteChallenge.Image = await ViewModel.LoadObjectFromDBAsync(CurrentSilhouetteChallenge.ImageID, "Image") as Modell.ChallengeObjects.Image;
                    SilhouetteNextSilhouette.Text = CurrentSilhouetteChallenge.Image.Name;
                }
            } else
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
                    CurrentSologameChallenge.Game = await ViewModel.LoadObjectFromDBAsync(CurrentSologameChallenge.GameID, "Game") as Game;
                    SologameNextGame.Text = CurrentSologameChallenge.Game.Name;
                }
            } else
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
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_Crew(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleCrewChallenge();
            if (!IsInPlayState)
            {
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_Multiple(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleMultipleChoiceChallenge();
            if (!IsInPlayState)
            {
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_Music(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleMusicChallenge();
            if (!IsInPlayState)
            {
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_Quiz(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleQuizChallenge();
            if (!IsInPlayState)
            {
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_Screenshot(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleScreenshotChallenge();
            if (!IsInPlayState)
            {
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_Silhouette(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleSilhouetteChallenge();
            if (!IsInPlayState)
            {
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        private void Button_Click_Sologame(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleSologameChallenge();
            if (!IsInPlayState)
            {
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            }
        }
        
        private void Button_Click_Play(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (GamePage.CurrentChallenge != null)
            {
                IsInPlayState = true;
            
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
                    CurrentAudienceChallenge = RollForAudienceChallengeRecursive();
                    GetAudienceChallengeGameFromDBAsync();
                    break;
                case 1:
                    UsedCrewChallenges.Add(CurrentCrewChallenge);
                    CurrentCrewChallenge = RollForCrewChallengeRecursive();
                    GetCrewChallengeGameAndMemberFromDBAsync();
                    break;
                case 2:
                    UsedMultipleChoiceChallenges.Add(CurrentMultipleChoiceChallenge);
                    CurrentMultipleChoiceChallenge = RollForMultipleChoiceChallengeRecursive();
                    GetMultipleChoiceAnswersFromDBAsync();
                    break;
                case 3:
                    UsedMusicChallenges.Add(CurrentMusicChallenge);
                    CurrentMusicChallenge = RollForMusicChallengeRecursive();
                    GetMusicChallengeSongFromDBAsync();
                    break;
                case 4:
                    UsedQuizChallenges.Add(CurrentQuizChallenge);
                    CurrentQuizChallenge = RollForQuizChallengeRecursive();
                    GetQuizChallengeAnswerFromDBAsync();
                    break;
                case 5:
                    UsedScreenshotChallenges.Add(CurrentScreenshotChallenge);
                    CurrentScreenshotChallenge = RollForScreenshotChallengeRecursive();
                    GetScreenshotChallengeScreenshotFromDBAsync();
                    break;
                case 6:
                    UsedSilhouetteChallenges.Add(CurrentSilhouetteChallenge);
                    CurrentSilhouetteChallenge = RollForSilhouetteChallengeRecursive();
                    GetSilhouetteChallengeSilhouetteFromDBAsync();
                    break;
                case 7:
                    UsedSologameChallenges.Add(CurrentSologameChallenge);
                    CurrentSologameChallenge = RollForSoloGameChallengeRecursive();
                    GetSologameChallengeGameFromDBAsync();
                    break;
                default:
                    break;
            }
            GamePage.ToggleMainScreen();
            
            IsInPlayState = false;

            if(IsNextSyntaxError)
            {
                SyntaxErrorShakeImage.Visibility = Visibility.Collapsed;
                CauseSyntaxError();
            }
            else RollForNextSyntaxError();
        }
        private void Button_Click_Random(object sender, RoutedEventArgs e)
        {

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

        /// <summary>Updates the game profile.
        /// INACTIVE Does not work yet</summary>
        private void UpdateGameProfile()
        {
            GameProfile profile = GamePage.GameProfile;
            
            foreach(var challenge in UsedAudienceChallenges)
                profile.SaveGame.Challenges.Add(new UsingChallenge{ Challenge = challenge, ChallengeID = challenge.ChallengeID  });
            foreach (var challenge in UsedCrewChallenges)
                profile.SaveGame.Challenges.Add(new UsingChallenge{ Challenge = challenge, ChallengeID = challenge.ChallengeID  });
            foreach (var challenge in UsedMultipleChoiceChallenges)
                profile.SaveGame.Challenges.Add(new UsingChallenge{ Challenge = challenge, ChallengeID = challenge.ChallengeID  });
            foreach (var challenge in UsedMusicChallenges)
                profile.SaveGame.Challenges.Add(new UsingChallenge{ Challenge = challenge, ChallengeID = challenge.ChallengeID  });
            foreach (var challenge in UsedQuizChallenges)
                profile.SaveGame.Challenges.Add(new UsingChallenge{ Challenge = challenge, ChallengeID = challenge.ChallengeID  });
            foreach (var challenge in UsedScreenshotChallenges)
                profile.SaveGame.Challenges.Add(new UsingChallenge{ Challenge = challenge, ChallengeID = challenge.ChallengeID  });
            foreach (var challenge in UsedSilhouetteChallenges)
                profile.SaveGame.Challenges.Add(new UsingChallenge{ Challenge = challenge, ChallengeID = challenge.ChallengeID  });
            foreach (var challenge in UsedSologameChallenges)
                profile.SaveGame.Challenges.Add(new UsingChallenge{ Challenge = challenge, ChallengeID = challenge.ChallengeID  });
            
            ViewModel.EditCommand.Execute(profile);
        }
    }
}
