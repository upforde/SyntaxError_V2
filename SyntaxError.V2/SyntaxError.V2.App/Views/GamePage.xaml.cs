using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class GamePage : Page
    {
        public int? CurrentChallenge;

        public GameViewModel ViewModel { get; } = new GameViewModel();
        public GameProfile GameProfile { get; set; }

        public List<AudienceChallenge> AudienceChallenges = new List<AudienceChallenge>();
        public List<CrewChallenge> CrewChallenges = new List<CrewChallenge>();
        public List<MultipleChoiceChallenge> MultipleChoiceChallenges = new List<MultipleChoiceChallenge>();
        public List<MusicChallenge> MusicChallenges =  new List<MusicChallenge>();
        public List<QuizChallenge> QuizChallenges = new List<QuizChallenge>();
        public List<ScreenshotChallenge> ScreenshotChallenges = new List<ScreenshotChallenge>();
        public List<SilhouetteChallenge> SilhouetteChallenges = new List<SilhouetteChallenge>();
        public List<SologameChallenge> SologameChallenges = new List<SologameChallenge>();

        public GamePage()
        {
            InitializeComponent();
            CurrentChallenge = null;
            PlayButtonAreaOpacityUp.Completed += OpacityCompleted;
            AudienceOpacityDown.Completed += CollapseChallengeWindows;
            CrewOpacityDown.Completed += CollapseChallengeWindows;
            MultipleChoiceOpacityDown.Completed += CollapseChallengeWindows;
            MusicOpacityDown.Completed += CollapseChallengeWindows;
            QuizOpacityDown.Completed += CollapseChallengeWindows;
            ScreenshotOpacityDown.Completed += CollapseChallengeWindows;
            SilhouetteOpacityDown.Completed += CollapseChallengeWindows;
            SologameOpacityDown.Completed += CollapseChallengeWindows;
        }

        private void CollapseChallengeWindows(object sender, object e)
        {
            switch (CurrentChallenge)
            {
                case 0:
                    AudienceChallengeArea.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    CrewChallengeArea.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    MultipleChoiceChallengeArea.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    MusicChallengeArea.Visibility = Visibility.Collapsed;
                    break;
                case 4:
                    QuizChallengeArea.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    ScreenshotChallengeArea.Visibility = Visibility.Collapsed;
                    break;
                case 6:
                    SilhouetteChallengeArea.Visibility = Visibility.Collapsed;
                    break;
                case 7:
                    SologameChallengeArea.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        private void OpacityCompleted(object sender, object e)
        {
            ToggleShadowOpacity(0.5);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            GameProfile = (e.Parameter as GameObjectForSending).GameProfile;
            
            foreach (ListItemMainPage challenge in (e.Parameter as GameObjectForSending).Challenges)
            {
                if(challenge.AudienceChallenge != null) AudienceChallenges.Add(challenge.AudienceChallenge);
                else if(challenge.CrewChallenge != null) CrewChallenges.Add(challenge.CrewChallenge);
                else if(challenge.MultipleChoiceChallenge != null) MultipleChoiceChallenges.Add(challenge.MultipleChoiceChallenge);
                else if(challenge.MusicChallenge != null) MusicChallenges.Add(challenge.MusicChallenge);
                else if(challenge.QuizChallenge != null) QuizChallenges.Add(challenge.QuizChallenge);
                else if(challenge.ScreenshotChallenge != null) ScreenshotChallenges.Add(challenge.ScreenshotChallenge);
                else if(challenge.SilhouetteChallenge != null) SilhouetteChallenges.Add(challenge.SilhouetteChallenge);
                else if(challenge.SologameChallenge != null) SologameChallenges.Add(challenge.SologameChallenge);
            }

            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(AdminPage), this);
                Window.Current.Content = frame;
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        public async void TogglePlayScreen()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                if (CurrentChallenge != null)
                                {
                                    ToggleShadowOpacity(0);
                                    PlayButtonAreaOpacityDown.Begin();
                                }
                                
                                switch (CurrentChallenge)
                                {
                                    case 0:
                                        AudienceChallengeArea.Visibility = Visibility.Visible;
                                        AudienceOpacityUp.Begin();
                                        break;
                                    case 1:
                                        CrewChallengeArea.Visibility = Visibility.Visible;
                                        CrewOpacityUp.Begin();
                                        break;
                                    case 2:
                                        MultipleChoiceChallengeArea.Visibility = Visibility.Visible;
                                        MultipleChoiceOpacityUp.Begin();
                                        break;
                                    case 3:
                                        MusicChallengeArea.Visibility = Visibility.Visible;
                                        MusicOpacityUp.Begin();
                                        break;
                                    case 4:
                                        QuizChallengeArea.Visibility = Visibility.Visible;
                                        QuizOpacityUp.Begin();
                                        break;
                                    case 5:
                                        ScreenshotChallengeArea.Visibility = Visibility.Visible;
                                        ScreenshotOpacityUp.Begin();
                                        break;
                                    case 6:
                                        SilhouetteChallengeArea.Visibility = Visibility.Visible;
                                        SilhouetteOpacityUp.Begin();
                                        break;
                                    case 7:
                                        SologameChallengeArea.Visibility = Visibility.Visible;
                                        SologameOpacityUp.Begin();
                                        break;
                                    default:
                                        break;
                                }
                            });
        }
        public async void ToggleMainScreen()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                PlayButtonAreaOpacityUp.Begin();

                                switch (CurrentChallenge)
                                {
                                    case 0:
                                        AudienceOpacityDown.Begin();
                                        break;
                                    case 1:
                                        CrewOpacityDown.Begin();
                                        break;
                                    case 2:
                                        MultipleChoiceOpacityDown.Begin();
                                        break;
                                    case 3:
                                        MusicOpacityDown.Begin();
                                        break;
                                    case 4:
                                        QuizOpacityDown.Begin();
                                        break;
                                    case 5:
                                        ScreenshotOpacityDown.Begin();
                                        break;
                                    case 6:
                                        SilhouetteOpacityDown.Begin();
                                        break;
                                    case 7:
                                        SologameOpacityDown.Begin();
                                        break;
                                    default:
                                        break;
                                }
                                DeselectChallenge();
                            });
        }

        public async void ToggleAudienceChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                if (CurrentChallenge != 0)
                                {
                                    DeselectChallenge();
                                    HighlightAudience();
                                    CurrentChallenge = 0;
                                }
                            });
        }
        public async void ToggleCrewChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                if (CurrentChallenge != 1)
                                {
                                    DeselectChallenge();
                                    HighlightCrew();
                                    CurrentChallenge = 1;
                                }
                            });
        }
        public async void ToggleMultipleChoiceChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                if (CurrentChallenge != 2)
                                {
                                    DeselectChallenge();
                                    HighlightMultipleChoice();
                                    CurrentChallenge = 2;
                                }
                            });
        }
        public async void ToggleMusicChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                if (CurrentChallenge != 3)
                                {
                                    DeselectChallenge();
                                    HighlightMusic();
                                    CurrentChallenge = 3;
                                }
                            });
        }
        public async void ToggleQuizChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                if (CurrentChallenge != 4)
                                {
                                    DeselectChallenge();                           
                                    HighlightQuiz();
                                    CurrentChallenge = 4;
                                }
                            });
        }
        public async void ToggleScreenshotChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                if (CurrentChallenge != 5)
                                {
                                    DeselectChallenge();                                 
                                    HighlightScreenshot();
                                    CurrentChallenge = 5;
                                }
                            });
        }
        public async void ToggleSilhouetteChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                if (CurrentChallenge != 6)
                                {
                                    DeselectChallenge();                                   
                                    HighlightSilhouette();
                                    CurrentChallenge = 6;
                                }
                            });
        }
        public async void ToggleSologameChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                if (CurrentChallenge != 7)
                                {
                                    DeselectChallenge();
                                    HighlightSologame();
                                    CurrentChallenge = 7;
                                }
                            });
        }
        public async void ToggleDeselect()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => DeselectChallenge());
        }

        private async void HighlightAudience()
        {
            for (int i = 0; i < 20; i++)
            {
                AudienceHighLight.TranslateY = -i;
                AudienceGlow.BlurRadius = i;
                AudienceGlow.OffsetY = -i;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightAudience()
        {
            for (int i = 0; i < 20; i++)
            {
                AudienceHighLight.TranslateY = -20+i;
                AudienceGlow.BlurRadius = 20-i;
                AudienceGlow.OffsetY = -20+i;
                await Task.Delay(1);
            }
        }
        
        private async void HighlightCrew()
        {
            for (int i = 0; i < 20; i++)
            {
                CrewChallengeHighLight.TranslateY = -i;
                CrewGlow.BlurRadius = i;
                CrewGlow.OffsetY = -i;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightCrew()
        {
            for (int i = 0; i < 20; i++)
            {
                CrewChallengeHighLight.TranslateY = -20+i;
                CrewGlow.BlurRadius = 20-i;
                CrewGlow.OffsetY = -20+i;
                await Task.Delay(1);
            }
        }

        private async void HighlightMultipleChoice()
        {
            for (int i = 0; i < 20; i++)
            {
                MultipleChoiceChallengeHighLight.TranslateY = -i;
                MultipleChoiceGlow.BlurRadius = i;
                MultipleChoiceGlow.OffsetY = -i;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightMultipleChoice()
        {
            for (int i = 0; i < 20; i++)
            {
                MultipleChoiceChallengeHighLight.TranslateY = -20+i;
                MultipleChoiceGlow.BlurRadius = 20-i;
                MultipleChoiceGlow.OffsetY = -20+i;
                await Task.Delay(1);
            }
        }
        
        private async void HighlightMusic()
        {
            for (int i = 0; i < 20; i++)
            {
                MusicChallengeHighLight.TranslateY = -i;
                MusicGlow.BlurRadius = i;
                MusicGlow.OffsetY = -i;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightMusic()
        {
            for (int i = 0; i < 20; i++)
            {
                MusicChallengeHighLight.TranslateY = -20+i;
                MusicGlow.BlurRadius = 20-i;
                MusicGlow.OffsetY = -20+i;
                await Task.Delay(1);
            }
        }

        private async void HighlightQuiz()
        {
            for (int i = 0; i < 20; i++)
            {
                QuizChallengeHighLight.TranslateY = -i;
                QuizGlow.BlurRadius = i;
                QuizGlow.OffsetY = -i+5;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightQuiz()
        {
            for (int i = 0; i < 20; i++)
            {
                QuizChallengeHighLight.TranslateY = -20+i;
                QuizGlow.BlurRadius = 20-i;
                QuizGlow.OffsetY = -15+i;
                await Task.Delay(1);
            }
        }
        
        private async void HighlightScreenshot()
        {
            for (int i = 0; i < 20; i++)
            {
                ScreenshotChallengeHighLight.TranslateY = -i;
                ScreenshotGlow.BlurRadius = i;
                ScreenshotGlow.OffsetY = -i+5;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightScreenshot()
        {
            for (int i = 0; i < 20; i++)
            {
                ScreenshotChallengeHighLight.TranslateY = -20+i;
                ScreenshotGlow.BlurRadius = 20-i;
                ScreenshotGlow.OffsetY = -15+i;
                await Task.Delay(1);
            }
        }
        
        private async void HighlightSilhouette()
        {
            for (int i = 0; i < 20; i++)
            {
                SilhouetteChallengeHighLight.TranslateY = -i;
                SilhouetteGlow.BlurRadius = i;
                SilhouetteGlow.OffsetY = -i+5;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightSilhouette()
        {
            for (int i = 0; i < 20; i++)
            {
                SilhouetteChallengeHighLight.TranslateY = -20+i;
                SilhouetteGlow.BlurRadius = 20-i;
                SilhouetteGlow.OffsetY = -15+i;
                await Task.Delay(1);
            }
        }
        
        private async void HighlightSologame()
        {
            for (int i = 0; i < 20; i++)
            {
                SologameChallengeHighLight.TranslateY = -i;
                SologameGlow.BlurRadius = i;
                SologameGlow.OffsetY = -i+5;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightSologame()
        {
            for (int i = 0; i < 20; i++)
                {
                    SologameChallengeHighLight.TranslateY = -20+i;
                    SologameGlow.BlurRadius = 20-i;
                    SologameGlow.OffsetY = -15+i;
                    await Task.Delay(1);
                }
        }
        
        public void DeselectChallenge()
        {
            switch (CurrentChallenge)
            {
                case 0:
                    DeHighlightAudience();
                    break;
                case 1:
                    DeHighlightCrew();
                    break;
                case 2:
                    DeHighlightMultipleChoice();
                    break;
                case 3:
                    DeHighlightMusic();
                    break;
                case 4:
                    DeHighlightQuiz();
                    break;
                case 5:
                    DeHighlightScreenshot();
                    break;
                case 6:
                    DeHighlightSilhouette();
                    break;
                case 7:
                    DeHighlightSologame();
                    break;
            }
            CurrentChallenge = null;
        }
        public void ToggleShadowOpacity(double level)
        {
            AudienceGlow.ShadowOpacity = level;
            CrewGlow.ShadowOpacity = level;
            MultipleChoiceGlow.ShadowOpacity = level;
            MusicGlow.ShadowOpacity = level;
            QuizGlow.ShadowOpacity = level;
            ScreenshotGlow.ShadowOpacity = level;
            SilhouetteGlow.ShadowOpacity = level;
            SologameGlow.ShadowOpacity = level;
        }
        
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
