﻿ using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.UI.Animations;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using Windows.ApplicationModel.Core;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class GamePage : Page
    {
        public bool IsAnimationActive = false;

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

        public async void AddToOtherList(string newMessage)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                if (!IsAnimationActive) HighlightCrew(); else DeHighlightCrew();
                                ViewModel.List.Add(newMessage);
                            });
        }
        
        public async void ToggleAudienceChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) HighlightAudience(); });
        }
        public async void ToggleAudienceChallengeOff()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) DeHighlightAudience(); });
        }
        
        public async void ToggleCrewChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) HighlightCrew(); });
        }
        public async void ToggleCrewChallengeOff()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) DeHighlightCrew(); });
        }
        
        public async void ToggleMultipleChoiceChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) HighlightMultipleChoice(); });
        }
        public async void ToggleMultipleChoiceChallengeOff()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) DeHighlightMultipleChoice(); });
        }
        
        public async void ToggleMusicChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) HighlightMusic(); });
        }
        public async void ToggleMusicChallengeOff()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) DeHighlightMusic(); });
        }

        public async void ToggleQuizChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) HighlightQuiz(); });
        }
        public async void ToggleQuizChallengeOff()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) DeHighlightQuiz(); });
        }

        public async void ToggleScreenshotChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) HighlightScreenshot(); });
        }

        public async void ToggleSilhouetteChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) HighlightSilhouette(); });
        }

        public async void ToggleSologameChallenge()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { if (!IsAnimationActive) HighlightSologame(); });
        }

        private async void HighlightAudience()
        {
            if (!IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                {
                    AudienceHighLight.TranslateY = -i;
                    AudienceGlow.BlurRadius = i;
                    AudienceGlow.OffsetY = -i;
                    await Task.Delay(1);
                }
                IsAnimationActive = true;
            }
        }
        private async void DeHighlightAudience()
        {
            if (IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                    {
                        AudienceHighLight.TranslateY = -20+i;
                        AudienceGlow.BlurRadius = 20-i;
                        AudienceGlow.OffsetY = -20+i;
                        await Task.Delay(1);
                    }
                IsAnimationActive = false;
            }
        }
        
        private async void HighlightCrew()
        {
            if (!IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                {
                    CrewChallengeHighLight.TranslateY = -i;
                    CrewGlow.BlurRadius = i;
                    CrewGlow.OffsetY = -i;
                    await Task.Delay(1);
                }
                IsAnimationActive = true;
            }
        }
        private async void DeHighlightCrew()
        {
            if (IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                    {
                        CrewChallengeHighLight.TranslateY = -20+i;
                        CrewGlow.BlurRadius = 20-i;
                        CrewGlow.OffsetY = -20+i;
                        await Task.Delay(1);
                    }
                IsAnimationActive = false;
            }
        }

        private async void HighlightMultipleChoice()
        {
            if (!IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                {
                    MultipleChoiceChallengeHighLight.TranslateY = -i;
                    MultipleChoiceGlow.BlurRadius = i;
                    MultipleChoiceGlow.OffsetY = -i;
                    await Task.Delay(1);
                }
                IsAnimationActive = true;
            }
        }
        private async void DeHighlightMultipleChoice()
        {
            if (IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                    {
                        MultipleChoiceChallengeHighLight.TranslateY = -20+i;
                        MultipleChoiceGlow.BlurRadius = 20-i;
                        MultipleChoiceGlow.OffsetY = -20+i;
                        await Task.Delay(1);
                    }
                IsAnimationActive = false;
            }
        }
        
        private async void HighlightMusic()
        {
            if (!IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                {
                    MusicChallengeHighLight.TranslateY = -i;
                    MusicGlow.BlurRadius = i;
                    MusicGlow.OffsetY = -i;
                    await Task.Delay(1);
                }
                IsAnimationActive = true;
            }
        }
        private async void DeHighlightMusic()
        {
            if (IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                    {
                        MusicChallengeHighLight.TranslateY = -20+i;
                        MusicGlow.BlurRadius = 20-i;
                        MusicGlow.OffsetY = -20+i;
                        await Task.Delay(1);
                    }
                IsAnimationActive = false;
            }
        }

        private async void HighlightQuiz()
        {
            if (!IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                {
                    QuizChallengeHighLight.TranslateY = -i;
                    QuizGlow.BlurRadius = i;
                    QuizGlow.OffsetY = -i;
                    await Task.Delay(1);
                }
                IsAnimationActive = true;
            }
        }
        private async void DeHighlightQuiz()
        {
            if (IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                    {
                        QuizChallengeHighLight.TranslateY = -20+i;
                        QuizGlow.BlurRadius = 20-i;
                        QuizGlow.OffsetY = -20+i;
                        await Task.Delay(1);
                    }
                IsAnimationActive = false;
            }
        }
        
        private async void HighlightScreenshot()
        {
            if (!IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                {
                    ScreenshotChallengeHighLight.TranslateY = -i;
                    ScreenshotGlow.BlurRadius = i;
                    ScreenshotGlow.OffsetY = -i;
                    await Task.Delay(1);
                }
                IsAnimationActive = true;
            }
        }
        private async void DeHighlightScreenshot()
        {
            if (IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                    {
                        ScreenshotChallengeHighLight.TranslateY = -20+i;
                        ScreenshotGlow.BlurRadius = 20-i;
                        ScreenshotGlow.OffsetY = -20+i;
                        await Task.Delay(1);
                    }
                IsAnimationActive = false;
            }
        }
        
        private async void HighlightSilhouette()
        {
            if (!IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                {
                    SilhouetteChallengeHighLight.TranslateY = -i;
                    SilhouetteGlow.BlurRadius = i;
                    SilhouetteGlow.OffsetY = -i;
                    await Task.Delay(1);
                }
                IsAnimationActive = true;
            }
        }
        private async void DeHighlightSilhouette()
        {
            if (IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                    {
                        SilhouetteChallengeHighLight.TranslateY = -20+i;
                        SilhouetteGlow.BlurRadius = 20-i;
                        SilhouetteGlow.OffsetY = -20+i;
                        await Task.Delay(1);
                    }
                IsAnimationActive = false;
            }
        }
        
        private async void HighlightSologame()
        {
            if (!IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                {
                    SologameChallengeHighLight.TranslateY = -i;
                    SologameGlow.BlurRadius = i;
                    SologameGlow.OffsetY = -i;
                    await Task.Delay(1);
                }
                IsAnimationActive = true;
            }
        }
        private async void DeHighlightSologame()
        {
            if (IsAnimationActive)
            {
                for (int i = 0; i < 20; i++)
                    {
                        SologameChallengeHighLight.TranslateY = -20+i;
                        SologameGlow.BlurRadius = 20-i;
                        SologameGlow.OffsetY = -20+i;
                        await Task.Delay(1);
                    }
                IsAnimationActive = false;
            }
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
