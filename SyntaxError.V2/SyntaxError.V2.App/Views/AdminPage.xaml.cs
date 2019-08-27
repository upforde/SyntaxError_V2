using System;
using System.Collections.Generic;
using System.Linq;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class AdminPage : Page
    {
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
            
            AudienceChallenges.AddRange(GamePage.AudienceChallenges);
            CrewChallenges.AddRange(GamePage.CrewChallenges);
            MultipleChoiceChallenges.AddRange(GamePage.MultipleChoiceChallenges);
            MusicChallenges.AddRange(GamePage.MusicChallenges);
            QuizChallenges.AddRange(GamePage.QuizChallenges);
            ScreenshotChallenges.AddRange(GamePage.ScreenshotChallenges);
            SilhouetteChallenges.AddRange(GamePage.SilhouetteChallenges);
            SologameChallenges.AddRange(GamePage.SologameChallenges);
        }

        private AudienceChallenge RollForAudienceChallengeRecursive()
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
            return null;
        }
        private CrewChallenge RollForCrewChallengeRecursive()
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
            return null;
        }
        private MultipleChoiceChallenge RollForMultipleChoiceChallengeRecursive()
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
            return null;
        }
        private MusicChallenge RollForMusicChallengeRecursive()
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
            return null;
        }
        private QuizChallenge RollForQuizChallengeRecursive()
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
            return null;
        }
        private ScreenshotChallenge RollForScreenshotChallengeRecursive()
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
            return null;
        }
        private SilhouetteChallenge RollForSilhouetteChallengeRecursive()
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
            return null;
        }
        private SologameChallenge RollForSoloGameChallengeRecursive()
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
            return null;
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string text = "";
            int rndNum = GamePage.RandomNumber(0, 7);

            switch (rndNum)
            {
                case 0:
                    if(AudienceChallenges.Count > 0)
                    {
                        AudienceChallenge rnd = AudienceChallenges[GamePage.RandomNumber(0, AudienceChallenges.Count-1)];
                        text = rnd.GetDiscriminator() + ": " + rnd.ChallengeTask;
                    } else text = "No AudienceChallenges";
                    break;
                case 1:
                    if(CrewChallenges.Count > 0)
                    {
                        CrewChallenge rnd = CrewChallenges[GamePage.RandomNumber(0, CrewChallenges.Count-1)];
                        text = rnd.GetDiscriminator() + ": " + rnd.ChallengeTask;
                    } else text = "No CrewChallenges";
                    break;
                case 2:
                    if (MultipleChoiceChallenges.Count > 0)
                    {
                        MultipleChoiceChallenge rnd = MultipleChoiceChallenges[GamePage.RandomNumber(0, MultipleChoiceChallenges.Count-1)];
                        text = rnd.GetDiscriminator() + ": " + rnd.ChallengeTask;
                    } else text = "No MultipleChoiceChallenges";
                    break;
                case 3:
                    if (MusicChallenges.Count > 0)
                    {
                        MusicChallenge rnd = MusicChallenges[GamePage.RandomNumber(0, MusicChallenges.Count-1)];
                        text = rnd.GetDiscriminator() + ": " + rnd.ChallengeTask;
                    } else text = "No MusicChallenges";
                    break;
                case 4:
                    if (QuizChallenges.Count > 0)
                    {
                        QuizChallenge rnd = QuizChallenges[GamePage.RandomNumber(0, QuizChallenges.Count-1)];
                        text = rnd.GetDiscriminator() + ": " + rnd.ChallengeTask;
                    } else text = "No QuizChallenges";
                    break;
                case 5:
                    if (ScreenshotChallenges.Count > 0)
                    {
                        ScreenshotChallenge rnd = ScreenshotChallenges[GamePage.RandomNumber(0, ScreenshotChallenges.Count-1)];
                        text = rnd.GetDiscriminator() + ": " + rnd.ChallengeTask;
                    } else text = "No ScreenshotChallenges";
                    break;
                case 6:
                    if (SilhouetteChallenges.Count > 0)
                    {
                        SilhouetteChallenge rnd = SilhouetteChallenges[GamePage.RandomNumber(0, SilhouetteChallenges.Count-1)];
                        text = rnd.GetDiscriminator() + ": " + rnd.ChallengeTask;
                    } else text = "No SilhouetteChallenges";
                    break;
                case 7:
                    if (SologameChallenges.Count > 0)
                    {
                        SologameChallenge rnd = SologameChallenges[GamePage.RandomNumber(0, SologameChallenges.Count-1)];
                        text = rnd.GetDiscriminator() + ": " + rnd.ChallengeTask;
                    } else text = "No SologameChallenges";
                    break;
                default:
                    text = rndNum.ToString();
                    break;
            }

            GamePage.AddToOtherList(text);
        }
        
        private void Button_Click_Audience(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleAudienceChallenge();
        }

        private void Button_Click_Crew(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleCrewChallenge();
        }

        private void Button_Click_Multiple(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleMultipleChoiceChallenge();
        }

        private void Button_Click_Music(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleMusicChallenge();
        }

        private void Button_Click_Quiz(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleQuizChallenge();
        }

        private void Button_Click_Screenshot(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleScreenshotChallenge();
        }

        private void Button_Click_Silhouette(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleSilhouetteChallenge();
        }

        private void Button_Click_Sologame(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GamePage.ToggleSologameChallenge();
        }
    }
}
