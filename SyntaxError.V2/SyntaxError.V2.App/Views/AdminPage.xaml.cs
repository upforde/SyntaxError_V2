using System;
using System.Collections.Generic;
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
        /// <summary>The play state
        /// boolean</summary>
        private bool IsInPlayState = false;
        /// <summary>The boolean determening if syntax error
        /// is next</summary>
        private bool IsNextSyntaxError = false;
        /// <summary>The syntax error maximum value</summary>
        private readonly int SyntaxErrorMaxVal = SettingsPage._syntaxErrorMaxVal;
        /// <summary>The syntax error counter</summary>
        private int SyntaxErrorCounter = SettingsPage._syntaxErrorMaxVal;

        /// <summary>Gets the view model.</summary>
        /// <value>The view model.</value>
        public AdminViewModel ViewModel { get; } = new AdminViewModel();
        /// <summary>The game page</summary>
        public GamePage GamePage;
        /// <summary>The audience challenges</summary>
        public List<AudienceChallenge> AudienceChallenges = new List<AudienceChallenge>();
        /// <summary>The used audience challenges</summary>
        public List<AudienceChallenge> UsedAudienceChallenges = new List<AudienceChallenge>();
        /// <summary>The crew challenges</summary>
        public List<CrewChallenge> CrewChallenges = new List<CrewChallenge>();
        /// <summary>The used crew challenges</summary>
        public List<CrewChallenge> UsedCrewChallenges = new List<CrewChallenge>();
        /// <summary>The multiple choice challenges</summary>
        public List<MultipleChoiceChallenge> MultipleChoiceChallenges = new List<MultipleChoiceChallenge>();
        /// <summary>The used multiple choice challenges</summary>
        public List<MultipleChoiceChallenge> UsedMultipleChoiceChallenges = new List<MultipleChoiceChallenge>();
        /// <summary>The music challenges</summary>
        public List<MusicChallenge> MusicChallenges = new List<MusicChallenge>();
        /// <summary>The used music challenges</summary>
        public List<MusicChallenge> UsedMusicChallenges = new List<MusicChallenge>();
        /// <summary>The quiz challenges</summary>
        public List<QuizChallenge> QuizChallenges = new List<QuizChallenge>();
        /// <summary>The used quiz challenges</summary>
        public List<QuizChallenge> UsedQuizChallenges = new List<QuizChallenge>();
        /// <summary>The screenshot challenges</summary>
        public List<ScreenshotChallenge> ScreenshotChallenges = new List<ScreenshotChallenge>();
        /// <summary>The used screenshot challenges</summary>
        public List<ScreenshotChallenge> UsedScreenshotChallenges = new List<ScreenshotChallenge>();
        /// <summary>The silhouette challenges</summary>
        public List<SilhouetteChallenge> SilhouetteChallenges = new List<SilhouetteChallenge>();
        /// <summary>The used silhouette challenges</summary>
        public List<SilhouetteChallenge> UsedSilhouetteChallenges = new List<SilhouetteChallenge>();
        /// <summary>The sologame challenges</summary>
        public List<SologameChallenge> SologameChallenges = new List<SologameChallenge>();
        /// <summary>The used sologame challenges</summary>
        public List<SologameChallenge> UsedSologameChallenges = new List<SologameChallenge>();

        /// <summary>Gets or sets the current audience challenge.</summary>
        /// <value>The current audience challenge.</value>
        public AudienceChallenge CurrentAudienceChallenge { get; set; }
        /// <summary>Gets or sets the current crew challenge.</summary>
        /// <value>The current crew challenge.</value>
        public CrewChallenge CurrentCrewChallenge { get; set; }
        /// <summary>Gets or sets the current multiple choice challenge.</summary>
        /// <value>The current multiple choice challenge.</value>
        public MultipleChoiceChallenge CurrentMultipleChoiceChallenge { get; set; }
        /// <summary>Gets or sets the current music challenge.</summary>
        /// <value>The current music challenge.</value>
        public MusicChallenge CurrentMusicChallenge { get; set; }
        /// <summary>Gets or sets the current quiz challenge.</summary>
        /// <value>The current quiz challenge.</value>
        public QuizChallenge CurrentQuizChallenge { get; set; }
        /// <summary>Gets or sets the current screenshot challenge.</summary>
        /// <value>The current screenshot challenge.</value>
        public ScreenshotChallenge CurrentScreenshotChallenge { get; set; }
        /// <summary>Gets or sets the current silhouette challenge.</summary>
        /// <value>The current silhouette challenge.</value>
        public SilhouetteChallenge CurrentSilhouetteChallenge { get; set; }
        /// <summary>Gets or sets the current sologame challenge.</summary>
        /// <value>The current sologame challenge.</value>
        public SologameChallenge CurrentSologameChallenge { get; set; }

        public AdminPage()
        {
            InitializeComponent();
        }

        /// <summary>Invoked when the Page is loaded and becomes the current source of a parent Frame.</summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GamePage = (GamePage) e.Parameter;
            GamePage.RandomSelectDone += GamePage_RandomSelectDone;
            AddAllChallenges();
            RollAll();
            UpdateAllTextFields();
        }

        /// <summary>Handles the RandomSelectDone event of the GamePage control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs" /> instance containing the event data.</param>
        private async void GamePage_RandomSelectDone(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PlayButton.IsEnabled = true;
                DeselectButton.IsEnabled = true;
            });
        }

        /// <summary>Adds all challenges.</summary>
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

        /// <summary>Rolls for all challenges.</summary>
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

        /// <summary>Rolls for audience challenge recursive.</summary>
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
        /// <summary>Rolls for crew challenge recursive.</summary>
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
        /// <summary>Rolls for multiple choice challenge recursive.</summary>
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
        /// <summary>Rolls for music challenge recursive.</summary>
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
        /// <summary>Rolls for quiz challenge recursive.</summary>
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
        /// <summary>Rolls for screenshot challenge recursive.</summary>
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
        /// <summary>Rolls for silhouette challenge recursive.</summary>
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
        /// <summary>Rolls for solo game challenge recursive.</summary>
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

        /// <summary>Updates all text fields.</summary>
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

        /// <summary>Gets the audience challenge game from database asynchronous.</summary>
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
        /// <summary>Gets the crew challenge game and member from database asynchronous.</summary>
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
        /// <summary>Gets the multiple choice answers from database asynchronous.</summary>
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
        /// <summary>Gets the music challenge song from database asynchronous.</summary>
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
        /// <summary>Gets the quiz challenge answer from database asynchronous.</summary>
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
        /// <summary>Gets the screenshot challenge screenshot from database asynchronous.</summary>
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
        /// <summary>Gets the silhouette challenge silhouette from database asynchronous.</summary>
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
        /// <summary>Gets the sologame challenge game from database asynchronous.</summary>
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

        /// <summary>Handles the FixSyntaxError event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_FixSyntaxError(object sender, RoutedEventArgs e)
        {
            SyntaxErrorCounter = SyntaxErrorMaxVal;
            IsNextSyntaxError = false;
            SyntaxErrorFixButton.Visibility = Visibility.Collapsed;
            GamePage.ToggleSyntaxErrorFix();
        }

        /// <summary>Handles the Audience event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
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
        /// <summary>Handles the RerollAudience event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_RerollAudience(object sender, RoutedEventArgs e)
        {
            RollForAudienceChallengeRecursive();
            GetAudienceChallengeGameFromDBAsync();
        }
        /// <summary>Handles the Crew event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
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
        /// <summary>Handles the RerollCrew event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_RerollCrew(object sender, RoutedEventArgs e)
        {
            RollForCrewChallengeRecursive();
            GetCrewChallengeGameAndMemberFromDBAsync();
        }
        /// <summary>Handles the Multiple event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
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
        /// <summary>Handles the RerollMultiple event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_RerollMultiple(object sender, RoutedEventArgs e)
        {
            RollForMultipleChoiceChallengeRecursive();
            GetMultipleChoiceAnswersFromDBAsync();
        }
        /// <summary>Handles the Music event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
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
        /// <summary>Handles the RerollMusic event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_RerollMusic(object sender, RoutedEventArgs e)
        {
            RollForMusicChallengeRecursive();
            GetMusicChallengeSongFromDBAsync();
        }
        /// <summary>Handles the Quiz event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
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
        /// <summary>Handles the RerollQuiz event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_RerollQuiz(object sender, RoutedEventArgs e)
        {
            RollForQuizChallengeRecursive();
            GetQuizChallengeAnswerFromDBAsync();
        }
        /// <summary>Handles the Screenshot event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
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
        /// <summary>Handles the RerollScreenshot event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_RerollScreenshot(object sender, RoutedEventArgs e)
        {
            RollForScreenshotChallengeRecursive();
            GetScreenshotChallengeScreenshotFromDBAsync();
        }
        /// <summary>Handles the Silhouette event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
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
        /// <summary>Handles the RerollSilhouette event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_RerollSilhouette(object sender, RoutedEventArgs e)
        {
            RollForSilhouetteChallengeRecursive();
            GetSilhouetteChallengeSilhouetteFromDBAsync();
        }
        /// <summary>Handles the Sologame event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
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
        /// <summary>Handles the RerollSologame event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_RerollSologame(object sender, RoutedEventArgs e)
        {
            RollForSoloGameChallengeRecursive();
            GetSologameChallengeGameFromDBAsync();
        }

        /// <summary>Handles the Play event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
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
        /// <summary>Handles the Deselect event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_Deselect(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PlayButton.IsEnabled = false;
            DeselectButton.IsEnabled = false;
            RandomButton.IsEnabled = true;

            GamePage.ToggleDeselect();
        }
        /// <summary>Handles the Answer event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
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
        /// <summary>Handles the Done event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.RoutedEventArgs" /> instance containing the event data.</param>
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

        /// <summary>Handles the Random event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_Random(object sender, RoutedEventArgs e)
        {
            PlayButton.IsEnabled = false;
            DeselectButton.IsEnabled = false;
            RandomButton.IsEnabled = false;
            GamePage.ToggleRandomSelection();
        }

        /// <summary>Causes the syntax error.</summary>
        private void CauseSyntaxError()
        {
            SyntaxErrorFixButton.Visibility = Visibility.Visible;
            GamePage.ToggleSyntaxError();
        }
        /// <summary>Rolls for syntax error next .</summary>
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

        /// <summary>Updates the save game.</summary>
        /// <param name="currentChallenge">The current challenge.</param>
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
