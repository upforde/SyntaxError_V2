using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.Foundation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class GamePage : Page
    {
        public int? CurrentChallenge;
        
        public GameViewModel ViewModel { get; } = new GameViewModel();
        public GameProfile GameProfile { get; set; }

        public BitmapImage bitmapImage;

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
            MusicOpacityUp.Completed += MusicOpacityUp_Completed;
            MusicOpacityDown.Completed += CollapseChallengeWindows;
            QuizOpacityUp.Completed += QuizOpacityUp_Completed;
            QuizOpacityDown.Completed += CollapseChallengeWindows;
            ScreenshotOpacityUp.Completed += ScreenshotOpacityUp_Completed;
            ScreenshotOpacityDown.Completed += CollapseChallengeWindows;
            SilhouetteOpacityUp.Completed += SilhouetteOpacityUp_Completed;
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
                    MusicAnswerArea.Visibility = Visibility.Collapsed;
                    break;
                case 4:
                    QuizChallengeArea.Visibility = Visibility.Collapsed;
                    QuizAnswerPlane.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    ScreenshotChallengeArea.Visibility = Visibility.Collapsed;
                    ScreenshotAnswerArea.Visibility = Visibility.Collapsed;
                    break;
                case 6:
                    Silhouette.Visibility = Visibility.Visible;
                    SilhouetteAnswer.Text = "Who's that character?";
                    SilhouetteActionArea.Visibility = Visibility.Collapsed;
                    SilhouetteChallengeArea.Visibility = Visibility.Collapsed;
                    break;
                case 7:
                    SologameChallengeArea.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
            DeselectChallenge();
        }
        
        private void MusicOpacityUp_Completed(object sender, object e)
        {
            MusicAnswerArea.Visibility = Visibility.Collapsed;
        }
        private void QuizOpacityUp_Completed(object sender, object e)
        {
            QuizAnswerPlane.Visibility = Visibility.Collapsed;
        }
        private void ScreenshotOpacityUp_Completed(object sender, object e)
        {
            ScreenshotAnswerArea.Visibility = Visibility.Collapsed;
        }
        private void SilhouetteOpacityUp_Completed(object sender, object e)
        {
            SilhouetteActionArea.Visibility = Visibility.Visible;
        }

        private void OpacityCompleted(object sender, object e)
        {
            ImgMove.Stop();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            GameProfile = (e.Parameter as ListItemMainPage).GameProfile;
            
            foreach (ListItemMainPage challenge in ((ListItemMainPage)e.Parameter).Challenges)
            {
                if(challenge.Challenge != null)
                    switch (challenge.Challenge.GetDiscriminator())
                    {
                        case "AudienceChallenge":
                            AudienceChallenges.Add(challenge.Challenge as AudienceChallenge);
                            break;
                        case "CrewChallenge":
                            CrewChallenges.Add(challenge.Challenge as CrewChallenge);
                            break;
                        case "MultipleChoiceChallenge":
                            MultipleChoiceChallenges.Add(challenge.Challenge as MultipleChoiceChallenge);
                            break;
                        case "MusicChallenge":
                            MusicChallenges.Add(challenge.Challenge as MusicChallenge);
                            break;
                        case "QuizChallenge":
                            QuizChallenges.Add(challenge.Challenge as QuizChallenge);
                            break;
                        case "ScreenshotChallenge":
                            ScreenshotChallenges.Add(challenge.Challenge as ScreenshotChallenge);
                            break;
                        case "SilhouetteChallenge":
                            SilhouetteChallenges.Add(challenge.Challenge as SilhouetteChallenge);
                            break;
                        case "SologameChallenge":
                            SologameChallenges.Add(challenge.Challenge as SologameChallenge);
                            break;
                    }
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
                                    ToggleImgMove();
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
                            });
        }

        public async void ToggleImgMove()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                switch (CurrentChallenge)
                                {
                                    case 0:
                                        ImgMove.Children[0].SetValue(Storyboard.TargetNameProperty, "frontAudienceTopImg");
                                        ImgMove.Children[1].SetValue(Storyboard.TargetNameProperty, "sideAudienceTopImg");
                                        ImgMove.Children[2].SetValue(Storyboard.TargetNameProperty, "frontAudienceBottomImg");
                                        ImgMove.Children[3].SetValue(Storyboard.TargetNameProperty, "sideAudienceBottomImg");
                                        break;
                                    case 1:
                                        ImgMove.Children[0].SetValue(Storyboard.TargetNameProperty, "frontCrewTopImg");
                                        ImgMove.Children[1].SetValue(Storyboard.TargetNameProperty, "sideCrewTopImg");
                                        ImgMove.Children[2].SetValue(Storyboard.TargetNameProperty, "frontCrewBottomImg");
                                        ImgMove.Children[3].SetValue(Storyboard.TargetNameProperty, "sideCrewBottomImg");
                                        break;
                                    case 2:
                                        ImgMove.Children[0].SetValue(Storyboard.TargetNameProperty, "frontMultipleChoiceTopImg");
                                        ImgMove.Children[1].SetValue(Storyboard.TargetNameProperty, "sideMultipleChoiceTopImg");
                                        ImgMove.Children[2].SetValue(Storyboard.TargetNameProperty, "frontMultipleChoiceBottomImg");
                                        ImgMove.Children[3].SetValue(Storyboard.TargetNameProperty, "sideMultipleChoiceBottomImg");
                                        break;
                                    case 3:
                                        ImgMove.Children[0].SetValue(Storyboard.TargetNameProperty, "frontMusicTopImg");
                                        ImgMove.Children[1].SetValue(Storyboard.TargetNameProperty, "sideMusicTopImg");
                                        ImgMove.Children[2].SetValue(Storyboard.TargetNameProperty, "frontMusicBottomImg");
                                        ImgMove.Children[3].SetValue(Storyboard.TargetNameProperty, "sideMusicBottomImg");
                                        break;
                                    case 4:
                                        ImgMove.Children[0].SetValue(Storyboard.TargetNameProperty, "frontQuizTopImg");
                                        ImgMove.Children[1].SetValue(Storyboard.TargetNameProperty, "sideQuizTopImg");
                                        ImgMove.Children[2].SetValue(Storyboard.TargetNameProperty, "frontQuizBottomImg");
                                        ImgMove.Children[3].SetValue(Storyboard.TargetNameProperty, "sideQuizBottomImg");
                                        break;
                                    case 5:
                                        ImgMove.Children[0].SetValue(Storyboard.TargetNameProperty, "frontScreenshotTopImg");
                                        ImgMove.Children[1].SetValue(Storyboard.TargetNameProperty, "sideScreenshotTopImg");
                                        ImgMove.Children[2].SetValue(Storyboard.TargetNameProperty, "frontScreenshotBottomImg");
                                        ImgMove.Children[3].SetValue(Storyboard.TargetNameProperty, "sideScreenshotBottomImg");
                                        break;
                                    case 6:
                                        ImgMove.Children[0].SetValue(Storyboard.TargetNameProperty, "frontSilhouetteTopImg");
                                        ImgMove.Children[1].SetValue(Storyboard.TargetNameProperty, "sideSilhouetteTopImg");
                                        ImgMove.Children[2].SetValue(Storyboard.TargetNameProperty, "frontSilhouetteBottomImg");
                                        ImgMove.Children[3].SetValue(Storyboard.TargetNameProperty, "sideSilhouetteBottomImg");
                                        break;
                                    case 7:
                                        ImgMove.Children[0].SetValue(Storyboard.TargetNameProperty, "frontSologameTopImg");
                                        ImgMove.Children[1].SetValue(Storyboard.TargetNameProperty, "sideSologameTopImg");
                                        ImgMove.Children[2].SetValue(Storyboard.TargetNameProperty, "frontSologameBottomImg");
                                        ImgMove.Children[3].SetValue(Storyboard.TargetNameProperty, "sideSologameBottomImg");
                                        break;
                                    default:
                                        break;
                                }
                                var imageWidth = PlayButtonArea.ActualWidth * 0.975;
                                ImgMove.Children[0].SetValue(DoubleAnimation.ToProperty, -imageWidth);
                                ImgMove.Children[1].SetValue(DoubleAnimation.FromProperty, imageWidth);
                                ImgMove.Children[2].SetValue(DoubleAnimation.FromProperty, -imageWidth);
                                ImgMove.Children[3].SetValue(DoubleAnimation.ToProperty, imageWidth);
                                ImgMove.Begin();
                            });
        }

        public async void ToggleSyntaxError()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await Task.Delay(3000);
                                AnimateSyntaxErrorQuizChallenge();
                                await Task.Delay(RandomNumber(1, 10));
                                AnimateSyntaxErrorScreenshotChallenge();
                                await Task.Delay(RandomNumber(1, 10));
                                AnimateSyntaxErrorSilhouetteChallenge();
                                await Task.Delay(RandomNumber(1, 10));
                                AnimateSyntaxErrorSologameChallenge();
                                await Task.Delay(RandomNumber(1, 10));
                                AnimateSyntaxErrorAudienceChallenge();
                                await Task.Delay(RandomNumber(1, 10));
                                AnimateSyntaxErrorCrewChallenge();
                                await Task.Delay(RandomNumber(1, 10));
                                AnimateSyntaxErrorMultipleChoiceChallenge();
                                await Task.Delay(RandomNumber(1, 10));
                                AnimateSyntaxErrorMusicChallenge();
                                await Task.Delay(RandomNumber(1, 10));
                                AnimateSyntaxErrorLogo();
                            });
        }
        public async void ToggleSyntaxErrorFix()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                FixSyntaxErrorAudience();
                                FixSyntaxErrorCrew();
                                FixSyntaxErrorMultipleChoice();
                                FixSyntaxErrorMusic();
                                FixSyntaxErrorQuiz();
                                FixSyntaxErrorScreenshot();
                                FixSyntaxErrorSilhouette();
                                FixSyntaxErrorSologame();
                                FixSyntaxErrorLogo();
                            });
        }

        private async void AnimateSyntaxErrorLogo()
        {
            var ttv = SyntaxErrorLogoImg.TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            var actualHeight = Window.Current.Bounds.Height;
            var absoluteHeight = actualHeight-screenCoords.Y - SyntaxErrorLogoImg.ActualHeight;
            
            double start = 0;
            double slutt = 100;
            double startX = 0;
            double sluttX = -40;
            double itteratingFactor = -5;
            var rnd = RandomNumber(1,100);
            if(rnd % 2 == 0){ sluttX = 40; itteratingFactor = 5; }
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.FromProperty, start);
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorLogo.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
            SyntaxErrorLogo.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorLogo.Children[1].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorLogo.Begin();
            start = slutt;
            slutt = slutt * 1.27;
            startX = sluttX;
            sluttX+=itteratingFactor;
            await Task.Delay(80);
            while(slutt < absoluteHeight)
            {
                SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.FromProperty, start);
                SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
                SyntaxErrorLogo.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
                SyntaxErrorLogo.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
                SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(10)));
                SyntaxErrorLogo.Begin();
                start = slutt;
                slutt = (slutt*1.18 >= absoluteHeight)?absoluteHeight:slutt * 1.18;
                startX = sluttX;
                sluttX+=itteratingFactor;
                await Task.Delay(10);
            }
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.FromProperty, slutt);
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.ToProperty, slutt-13);
            SyntaxErrorLogo.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorLogo.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor*2));
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorLogo.Begin();
            await Task.Delay(68);
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.FromProperty, slutt-13);
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorLogo.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorLogo.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor * 2));
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorLogo.Begin();
        }
        private void FixSyntaxErrorLogo()
        {
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorLogo.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorLogo.Begin();
        }
        private async void AnimateSyntaxErrorAudienceChallenge()
        {
            AudienceGlow.ShadowOpacity = 0;
            var ttv = audienceChallenge.TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            var actualHeight = Window.Current.Bounds.Height;
            var absoluteHeight = actualHeight-screenCoords.Y - audienceChallenge.ActualHeight;
            
            double start = 0;
            double slutt = 100;
            double startX = 0;
            double sluttX = -40;
            double itteratingFactor = -5;
            var rnd = RandomNumber(1,100);
            if(rnd % 2 == 0){ sluttX = 40; itteratingFactor = 5; }
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.FromProperty, start);
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorAudience.Begin();
            start = slutt;
            slutt = slutt * 1.27;
            startX = sluttX;
            sluttX+=itteratingFactor;
            await Task.Delay(80);
            while(slutt < absoluteHeight)
            {
                SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.FromProperty, start);
                SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
                SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
                SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
                SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(10)));
                SyntaxErrorAudience.Begin();
                start = slutt;
                slutt = (slutt*1.18 >= absoluteHeight)?absoluteHeight:slutt * 1.18;
                startX = sluttX;
                sluttX+=itteratingFactor;
                await Task.Delay(10);
            }
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.FromProperty, slutt);
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.ToProperty, slutt-13);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor*2));
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorAudience.Begin();
            await Task.Delay(68);
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.FromProperty, slutt-13);
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor * 2));
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorAudience.Begin();
        }
        private void FixSyntaxErrorAudience()
        {
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorAudience.Begin();
        }
        private async void AnimateSyntaxErrorCrewChallenge()
        {
            CrewGlow.ShadowOpacity = 0;
            var ttv = crewChallenge.TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            var actualHeight = Window.Current.Bounds.Height;
            var absoluteHeight = actualHeight-screenCoords.Y - crewChallenge.ActualHeight;
            
            double start = 0;
            double slutt = 100;
            double startX = 0;
            double sluttX = -40;
            double itteratingFactor = -5;
            var rnd = RandomNumber(1,100);
            if(rnd % 2 == 0){ sluttX = 40; itteratingFactor = 5; }
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.FromProperty, start);
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorCrew.Begin();
            start = slutt;
            slutt = slutt * 1.27;
            startX = sluttX;
            sluttX+=itteratingFactor;
            await Task.Delay(80);
            while(slutt < absoluteHeight)
            {
                SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.FromProperty, start);
                SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
                SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
                SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
                SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(10)));
                SyntaxErrorCrew.Begin();
                start = slutt;
                slutt = (slutt*1.18 >= absoluteHeight)?absoluteHeight:slutt * 1.18;
                startX = sluttX;
                sluttX+=itteratingFactor;
                await Task.Delay(10);
            }
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.FromProperty, slutt);
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.ToProperty, slutt-13);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor*2));
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorCrew.Begin();
            await Task.Delay(68);
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.FromProperty, slutt-13);
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor * 2));
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorCrew.Begin();
        }
        private void FixSyntaxErrorCrew()
        {
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorCrew.Begin();
        }
        private async void AnimateSyntaxErrorMultipleChoiceChallenge()
        {
            MultipleChoiceGlow.ShadowOpacity = 0;
            var ttv = multipleChoiceChallenge.TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            var actualHeight = Window.Current.Bounds.Height;
            var absoluteHeight = actualHeight-screenCoords.Y - multipleChoiceChallenge.ActualHeight;
            
            double start = 0;
            double slutt = 100;
            double startX = 0;
            double sluttX = -40;
            double itteratingFactor = -5;
            var rnd = RandomNumber(1,100);
            if(rnd % 2 == 0){ sluttX = 40; itteratingFactor = 5; }
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.FromProperty, start);
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorMultipleChoice.Begin();
            start = slutt;
            slutt = slutt * 1.27;
            startX = sluttX;
            sluttX+=itteratingFactor;
            await Task.Delay(80);
            while(slutt < absoluteHeight)
            {
                SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.FromProperty, start);
                SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
                SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
                SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
                SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(10)));
                SyntaxErrorMultipleChoice.Begin();
                start = slutt;
                slutt = (slutt*1.18 >= absoluteHeight)?absoluteHeight:slutt * 1.18;
                startX = sluttX;
                sluttX+=itteratingFactor;
                await Task.Delay(10);
            }
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.FromProperty, slutt);
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.ToProperty, slutt-13);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor*2));
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorMultipleChoice.Begin();
            await Task.Delay(68);
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.FromProperty, slutt-13);
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor * 2));
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorMultipleChoice.Begin();
        }
        private void FixSyntaxErrorMultipleChoice()
        {
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorMultipleChoice.Begin();
        }
        private async void AnimateSyntaxErrorMusicChallenge()
        {
            MusicGlow.ShadowOpacity = 0;
            var ttv = musicChallenge.TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            var actualHeight = Window.Current.Bounds.Height;
            var absoluteHeight = actualHeight-screenCoords.Y - musicChallenge.ActualHeight;
            
            double start = 0;
            double slutt = 100;
            double startX = 0;
            double sluttX = -40;
            double itteratingFactor = -5;
            var rnd = RandomNumber(1,100);
            if(rnd % 2 == 0){ sluttX = 40; itteratingFactor = 5; }
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.FromProperty, start);
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorMusic.Begin();
            start = slutt;
            slutt = slutt * 1.27;
            startX = sluttX;
            sluttX+=itteratingFactor;
            await Task.Delay(80);
            while(slutt < absoluteHeight)
            {
                SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.FromProperty, start);
                SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
                SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
                SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
                SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(10)));
                SyntaxErrorMusic.Begin();
                start = slutt;
                slutt = (slutt*1.18 >= absoluteHeight)?absoluteHeight:slutt * 1.18;
                startX = sluttX;
                sluttX+=itteratingFactor;
                await Task.Delay(10);
            }
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.FromProperty, slutt);
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.ToProperty, slutt-13);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor*2));
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorMusic.Begin();
            await Task.Delay(68);
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.FromProperty, slutt-13);
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor * 2));
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorMusic.Begin();
        }
        private void FixSyntaxErrorMusic()
        {
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorMusic.Begin();
        }
        private async void AnimateSyntaxErrorQuizChallenge()
        {
            QuizGlow.ShadowOpacity = 0;
            var ttv = quizChallenge.TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            var actualHeight = Window.Current.Bounds.Height;
            var absoluteHeight = actualHeight-screenCoords.Y - quizChallenge.ActualHeight;
            
            double start = 0;
            double slutt = 100;
            double startX = 0;
            double sluttX = -40;
            double itteratingFactor = -5;
            var rnd = RandomNumber(1,100);
            if(rnd % 2 == 0){ sluttX = 40; itteratingFactor = 5; }
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.FromProperty, start);
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorQuiz.Begin();
            start = slutt;
            slutt = slutt * 1.27;
            startX = sluttX;
            sluttX+=itteratingFactor;
            await Task.Delay(80);
            while(slutt < absoluteHeight)
            {
                SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.FromProperty, start);
                SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
                SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
                SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
                SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(10)));
                SyntaxErrorQuiz.Begin();
                start = slutt;
                slutt = (slutt*1.18 >= absoluteHeight)?absoluteHeight:slutt * 1.18;
                startX = sluttX;
                sluttX+=itteratingFactor;
                await Task.Delay(10);
            }
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.FromProperty, slutt);
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.ToProperty, slutt-13);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor*2));
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorQuiz.Begin();
            await Task.Delay(68);
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.FromProperty, slutt-13);
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor * 2));
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorQuiz.Begin();
        }
        private void FixSyntaxErrorQuiz()
        {
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorQuiz.Begin();
        }
        private async void AnimateSyntaxErrorScreenshotChallenge()
        {
            ScreenshotGlow.ShadowOpacity = 0;
            var ttv = screenshotChallenge.TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            var actualHeight = Window.Current.Bounds.Height;
            var absoluteHeight = actualHeight-screenCoords.Y - screenshotChallenge.ActualHeight;
            
            double start = 0;
            double slutt = 100;
            double startX = 0;
            double sluttX = -40;
            double itteratingFactor = -5;
            var rnd = RandomNumber(1,100);
            if(rnd % 2 == 0){ sluttX = 40; itteratingFactor = 5; }
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.FromProperty, start);
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorScreenshot.Begin();
            start = slutt;
            slutt = slutt * 1.27;
            startX = sluttX;
            sluttX+=itteratingFactor;
            await Task.Delay(80);
            while(slutt < absoluteHeight)
            {
                SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.FromProperty, start);
                SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
                SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
                SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
                SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(10)));
                SyntaxErrorScreenshot.Begin();
                start = slutt;
                slutt = (slutt*1.18 >= absoluteHeight)?absoluteHeight:slutt * 1.18;
                startX = sluttX;
                sluttX+=itteratingFactor;
                await Task.Delay(10);
            }
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.FromProperty, slutt);
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.ToProperty, slutt-13);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor*2));
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorScreenshot.Begin();
            await Task.Delay(68);
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.FromProperty, slutt-13);
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor * 2));
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorScreenshot.Begin();
        }
        private void FixSyntaxErrorScreenshot()
        {
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorScreenshot.Begin();
        }
        private async void AnimateSyntaxErrorSilhouetteChallenge()
        {
            SilhouetteGlow.ShadowOpacity = 0;
            var ttv = silhouetteChallenge.TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            var actualHeight = Window.Current.Bounds.Height;
            var absoluteHeight = actualHeight-screenCoords.Y - silhouetteChallenge.ActualHeight;
            
            double start = 0;
            double slutt = 100;
            double startX = 0;
            double sluttX = -40;
            double itteratingFactor = -5;
            var rnd = RandomNumber(1,100);
            if(rnd % 2 == 0){ sluttX = 40; itteratingFactor = 5; }
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.FromProperty, start);
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorSilhouette.Begin();
            start = slutt;
            slutt = slutt * 1.27;
            startX = sluttX;
            sluttX+=itteratingFactor;
            await Task.Delay(80);
            while(slutt < absoluteHeight)
            {
                SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.FromProperty, start);
                SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
                SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
                SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
                SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(10)));
                SyntaxErrorSilhouette.Begin();
                start = slutt;
                slutt = (slutt*1.18 >= absoluteHeight)?absoluteHeight:slutt * 1.18;
                startX = sluttX;
                sluttX+=itteratingFactor;
                await Task.Delay(10);
            }
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.FromProperty, slutt);
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.ToProperty, slutt-13);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor*2));
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorSilhouette.Begin();
            await Task.Delay(68);
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.FromProperty, slutt-13);
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor * 2));
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorSilhouette.Begin();
        }
        private void FixSyntaxErrorSilhouette()
        {
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorSilhouette.Begin();
        }
        private async void AnimateSyntaxErrorSologameChallenge()
        {
            SologameGlow.ShadowOpacity = 0;
            var ttv = sologameChallenge.TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            var actualHeight = Window.Current.Bounds.Height;
            var absoluteHeight = actualHeight-screenCoords.Y - sologameChallenge.ActualHeight;
            
            double start = 0;
            double slutt = 100;
            double startX = 0;
            double sluttX = -40;
            double itteratingFactor = -5;
            var rnd = RandomNumber(1,100);
            if(rnd % 2 == 0){ sluttX = 40; itteratingFactor = 5; }
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.FromProperty, start);
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(80)));
            SyntaxErrorSologame.Begin();
            start = slutt;
            slutt = slutt * 1.27;
            startX = sluttX;
            sluttX+=itteratingFactor;
            await Task.Delay(80);
            while(slutt < absoluteHeight)
            {
                SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.FromProperty, start);
                SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
                SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.FromProperty, startX);
                SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX);
                SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(10)));
                SyntaxErrorSologame.Begin();
                start = slutt;
                slutt = (slutt*1.18 >= absoluteHeight)?absoluteHeight:slutt * 1.18;
                startX = sluttX;
                sluttX+=itteratingFactor;
                await Task.Delay(10);
            }
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.FromProperty, slutt);
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.ToProperty, slutt-13);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor*2));
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorSologame.Begin();
            await Task.Delay(68);
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.FromProperty, slutt-13);
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.ToProperty, slutt);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.FromProperty, startX + itteratingFactor);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.ToProperty, sluttX + (itteratingFactor * 2));
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(68)));
            SyntaxErrorSologame.Begin();
        }
        private void FixSyntaxErrorSologame()
        {
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorSologame.Begin();
        }
        
        public async void ActuateAudienceChallenge(AudienceChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                bitmapImage = new BitmapImage
                                {
                                    UriSource = new Uri(challenge.Game.URI)
                                };
                                AudienceGameImg.Source = bitmapImage;
                                AudienceGameText.Text = challenge.Game.Name;
                                AudienceTaskText.Text = challenge.ChallengeTask;
                            });
        }
        public async void ActuateCrewChallenge(CrewChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                bitmapImage = new BitmapImage
                                {
                                    UriSource = new Uri(challenge.Game.URI)
                                };
                                CrewGameImg.Source = bitmapImage;
                                CrewGameName.Text = challenge.Game.Name;
                                CrewTask.Text = challenge.ChallengeTask;
                                //CrewCrewMember.Text = challenge.CrewMember.CrewTag;
                            });
        }
        public async void ActuateMultipleChoiceChallenge(MultipleChoiceChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                var answers = challenge.Answers.GetAll();
                                MultipleChoiceTask.Text = challenge.ChallengeTask;
                                MultipleChoiceTopLeft.Text = answers[0];
                                MultipleChoiceTopRight.Text = answers[1];
                                MultipleChoiceBottomLeft.Text = answers[2];
                                MultipleChoiceBottomRight.Text = answers[3];

                                MultipleChoiceTopLeftBorder.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                                MultipleChoiceTopRightBorder.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                                MultipleChoiceBottomLeftBorder.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                                MultipleChoiceBottomRightBorder.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                            });
        }
        public async void ActuateMusicChallenge(MusicChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                bitmapImage = new BitmapImage
                                {
                                    UriSource = new Uri(challenge.Song.URI)
                                };
                                MusicImg.Source = bitmapImage;
                                MusicAnswer.Text = challenge.Song.Name;
                            });
        }
        public async void ActuateQuizChallenge(QuizChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                QuizTask.Text = challenge.ChallengeTask;
                                QuizAnswer.Text = challenge.Answers.Answer;
                            });
        }
        public async void ActuateScreenshotChallenge(ScreenshotChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                bitmapImage = new BitmapImage
                                {
                                    UriSource = new Uri(challenge.Image.URI)
                                };
                                ScreenshotImg.Source = bitmapImage;
                                ScreenshotAnswer.Text = challenge.Image.Name;
                            });
        }
        public async void ActuateSilhouetteChallenge(SilhouetteChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                bitmapImage = new BitmapImage
                                {
                                    UriSource = new Uri(challenge.Image.URI)
                                };
                                Img.Source = bitmapImage;
                                SilhouetteBlendBrush.Source = bitmapImage;
                            });
            
        }
        public async void ActuateSologameChallenge(SologameChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                bitmapImage = new BitmapImage
                                {
                                    UriSource = new Uri(challenge.Game.URI)
                                };
                                SologameGameImg.Source = bitmapImage;
                                SologameGameText.Text = challenge.Game.Name;
                                SologameTaskText.Text = challenge.ChallengeTask;
                            });
        }

        public async void AnswerMultipleChoiceChallenge(MultipleChoiceChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                string answer = challenge.Answers.Answer;
                                int[] queue = {0, 1, 2, 3};
                                int queueAnswer = -1;
                                queue.Shuffle();

                                for (var i = 0; i <= 3; i++)
                                {
                                    switch (queue[i])
                                    {
                                        case 0:
                                            if (MultipleChoiceTopLeft.Text != answer)
                                            {
                                                for (byte c = 0; c <= 254; c++)
                                                {
                                                    MultipleChoiceTopLeftBorder.Background = new SolidColorBrush(Color.FromArgb(c, 255, 0, 0));
                                                    await Task.Delay(3);
                                                }
                                                MultipleChoiceTopLeftBorder.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                                            } else queueAnswer = i;
                                            break;
                                        case 1:
                                            if (MultipleChoiceTopRight.Text != answer)
                                            {
                                                for (byte c = 0; c <= 254; c++)
                                                {
                                                    MultipleChoiceTopRightBorder.Background = new SolidColorBrush(Color.FromArgb(c, 255, 0, 0));
                                                    await Task.Delay(3);
                                                }
                                                MultipleChoiceTopRightBorder.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                                            } else queueAnswer = i;
                                            break;
                                        case 2:
                                            if (MultipleChoiceBottomLeft.Text != answer)
                                            {
                                                for (byte c = 0; c <= 254; c++)
                                                {
                                                    MultipleChoiceBottomLeftBorder.Background = new SolidColorBrush(Color.FromArgb(c, 255, 0, 0));
                                                    await Task.Delay(3);
                                                }
                                                MultipleChoiceBottomLeftBorder.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                                            } else queueAnswer = i;
                                            break;
                                        case 3:
                                            if (MultipleChoiceBottomRight.Text != answer)
                                            {
                                                for (byte c = 0; c <= 254; c++)
                                                {
                                                    MultipleChoiceBottomRightBorder.Background = new SolidColorBrush(Color.FromArgb(c, 255, 0, 0));
                                                    await Task.Delay(3);
                                                }
                                                MultipleChoiceBottomRightBorder.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                                            } else queueAnswer = i;
                                            break;
                                    }
                                    await Task.Delay(235);
                                }
                                switch (queue[queueAnswer])
                                {
                                    case 0:
                                        for(int i = 0; i <= 50; i++)
                                        {
                                            if (i % 2 == 0) MultipleChoiceTopLeftBorder.Background = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                                            else MultipleChoiceTopLeftBorder.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                                            await Task.Delay(333);
                                        }
                                        break;
                                    case 1:
                                        for(int i = 0; i <= 50; i++)
                                        {
                                            if (i % 2 == 0) MultipleChoiceTopRightBorder.Background = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                                            else MultipleChoiceTopRightBorder.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                                            await Task.Delay(333);
                                        }
                                        break;
                                    case 2:
                                        for(int i = 0; i <= 50; i++)
                                        {
                                            if (i % 2 == 0) MultipleChoiceBottomLeftBorder.Background = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                                            else MultipleChoiceBottomLeftBorder.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                                            await Task.Delay(333);
                                        }
                                        break;
                                    case 3:
                                        for(int i = 0; i <= 50; i++)
                                        {
                                            if (i % 2 == 0) MultipleChoiceBottomRightBorder.Background = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                                            else MultipleChoiceBottomRightBorder.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                                            await Task.Delay(333);
                                        }
                                        break;
                                }
                            });
        }
        public async void AnswerMusicChallenge(MusicChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                MusicAnswerArea.Visibility = Visibility.Visible;
                            });
        }
        public async void AnswerQuizChallenge(QuizChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                QuizAnswerPlane.Visibility = Visibility.Visible;
                            });
        }
        public async void AnswerScreenshotChallenge(ScreenshotChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                ScreenshotAnswerArea.Visibility = Visibility.Visible;
                            });
        }
        public async void AnswerSilhouetteChallenge(SilhouetteChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                Silhouette.Visibility = Visibility.Collapsed;
                                SilhouetteAnswer.Text = challenge.Image.Name;
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

        public async void ToggleAudienceSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await audienceChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                            });
        }
        public async void ToggleCrewSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await crewChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                            });
        }
        public async void ToggleMultipleChoiceSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await multipleChoiceChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                            });
        }
        public async void ToggleMusicSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await musicChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                            });
        }
        public async void ToggleQuizSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await quizChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                            });
        }
        public async void ToggleScreenshotSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await screenshotChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                            });
        }
        public async void ToggleSilhouetteSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await silhouetteChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                            });
        }
        public async void ToggleSologameSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await sologameChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                            });
        }

        public async void ToggleRandomSelection()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                RandomSelectChallenge();
                            });
        }

        private async void HighlightAudience()
        {
            AudienceGlow.ShadowOpacity = 0.5;
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.ToProperty, -20);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorAudience.Begin();
            for (int i = 0; i < 20; i++)
            {
                AudienceGlow.BlurRadius = i;
                AudienceGlow.OffsetY = -i;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightAudience()
        {
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.FromProperty, -20);
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(120)));
            SyntaxErrorAudience.Begin();
            for (int i = 0; i < 20; i++)
            {
                AudienceGlow.BlurRadius = 20-i;
                AudienceGlow.OffsetY = -20+i;
                await Task.Delay(1);
            }
            AudienceGlow.ShadowOpacity = 0;
        }
        
        private async void HighlightCrew()
        {
            CrewGlow.ShadowOpacity = 0.5;
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.ToProperty, -20);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorCrew.Begin();
            for (int i = 0; i < 20; i++)
            {
                CrewGlow.BlurRadius = i;
                CrewGlow.OffsetY = -i;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightCrew()
        {
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.FromProperty, -20);
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorCrew.Begin();
            for (int i = 0; i < 20; i++)
            {
                CrewChallengeHighLight.TranslateY = -20+i;
                CrewGlow.BlurRadius = 20-i;
                CrewGlow.OffsetY = -20+i;
                await Task.Delay(1);
            }
            CrewGlow.ShadowOpacity = 0;
        }

        private async void HighlightMultipleChoice()
        {
            MultipleChoiceGlow.ShadowOpacity = 0.5;
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.ToProperty, -20);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorMultipleChoice.Begin();
            for (int i = 0; i < 20; i++)
            {
                MultipleChoiceGlow.BlurRadius = i;
                MultipleChoiceGlow.OffsetY = -i;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightMultipleChoice()
        {
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.FromProperty, -20);
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorMultipleChoice.Begin();
            for (int i = 0; i < 20; i++)
            {
                MultipleChoiceGlow.BlurRadius = 20-i;
                MultipleChoiceGlow.OffsetY = -20+i;
                await Task.Delay(1);
            }
            MultipleChoiceGlow.ShadowOpacity = 0;
        }
        
        private async void HighlightMusic()
        {
            MusicGlow.ShadowOpacity = 0.5;
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.ToProperty, -20);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorMusic.Begin();
            for (int i = 0; i < 20; i++)
            {
                MusicGlow.BlurRadius = i;
                MusicGlow.OffsetY = -i;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightMusic()
        {
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.FromProperty, -20);
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorMusic.Begin();
            for (int i = 0; i < 20; i++)
            {
                MusicGlow.BlurRadius = 20-i;
                MusicGlow.OffsetY = -20+i;
                await Task.Delay(1);
            }
            MusicGlow.ShadowOpacity = 0;
        }

        private async void HighlightQuiz()
        {
            QuizGlow.ShadowOpacity = 0.5;
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.ToProperty, -20);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorQuiz.Begin();
            for (int i = 0; i < 20; i++)
            {
                QuizGlow.BlurRadius = i;
                QuizGlow.OffsetY = -i+5;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightQuiz()
        {
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.FromProperty, -20);
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorQuiz.Begin();
            for (int i = 0; i < 20; i++)
            {
                QuizGlow.BlurRadius = 20-i;
                QuizGlow.OffsetY = -15+i;
                await Task.Delay(1);
            }
            QuizGlow.ShadowOpacity = 0;
        }
        
        private async void HighlightScreenshot()
        {
            ScreenshotGlow.ShadowOpacity = 0.5;
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.ToProperty, -20);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorScreenshot.Begin();
            for (int i = 0; i < 20; i++)
            {
                ScreenshotGlow.BlurRadius = i;
                ScreenshotGlow.OffsetY = -i+5;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightScreenshot()
        {
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.FromProperty, -20);
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorScreenshot.Begin();
            for (int i = 0; i < 20; i++)
            {
                ScreenshotGlow.BlurRadius = 20-i;
                ScreenshotGlow.OffsetY = -15+i;
                await Task.Delay(1);
            }
            ScreenshotGlow.ShadowOpacity = 0;
        }
        
        private async void HighlightSilhouette()
        {
            SilhouetteGlow.ShadowOpacity = 0.5;
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.ToProperty, -20);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorSilhouette.Begin();
            for (int i = 0; i < 20; i++)
            {
                SilhouetteGlow.BlurRadius = i;
                SilhouetteGlow.OffsetY = -i+5;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightSilhouette()
        {
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.FromProperty, -20);
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorSilhouette.Begin();
            for (int i = 0; i < 20; i++)
            {
                SilhouetteGlow.BlurRadius = 20-i;
                SilhouetteGlow.OffsetY = -15+i;
                await Task.Delay(1);
            }
            SilhouetteGlow.ShadowOpacity = 0;
        }
        
        private async void HighlightSologame()
        {
            SologameGlow.ShadowOpacity = 0.5;
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.ToProperty, -20);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorSologame.Begin();
            for (int i = 0; i < 20; i++)
            {
                SologameGlow.BlurRadius = i;
                SologameGlow.OffsetY = -i+5;
                await Task.Delay(1);
            }
        }
        private async void DeHighlightSologame()
        {
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.FromProperty, -20);
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.FromProperty, 0);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(200)));
            SyntaxErrorSologame.Begin();
            for (int i = 0; i < 20; i++)
            {
                SologameGlow.BlurRadius = 20-i;
                SologameGlow.OffsetY = -15+i;
                await Task.Delay(1);
            }
            SologameGlow.ShadowOpacity = 0;
        }

        private async void RandomSelectChallenge()
        {
            int rnd = RandomNumber(16, 33);
            int delay = 200;
            DeselectChallenge();
            HighlightAudience();
            CurrentChallenge = 0;
            await Task.Delay(delay);
            for (int i = 0; i < rnd; i++)
            {
                switch (i % 8)
                {
                    case 0:
                        DeHighlightAudience();
                        HighlightCrew();
                        CurrentChallenge = 1;
                        break;
                    case 1:
                        DeHighlightCrew();
                        HighlightMultipleChoice();
                        CurrentChallenge = 2;
                        break;
                    case 2:
                        DeHighlightMultipleChoice();
                        HighlightMusic();
                        CurrentChallenge = 3;
                        break;
                    case 3:
                        DeHighlightMusic();
                        HighlightQuiz();
                        CurrentChallenge = 4;
                        break;
                    case 4:
                        DeHighlightQuiz();
                        HighlightScreenshot();
                        CurrentChallenge = 5;
                        break;
                    case 5:
                        DeHighlightScreenshot();
                        HighlightSilhouette();
                        CurrentChallenge = 6;
                        break;
                    case 6:
                        DeHighlightSilhouette();
                        HighlightSologame();
                        CurrentChallenge = 7;
                        break;
                    case 7:
                        DeHighlightSologame();
                        HighlightAudience();
                        CurrentChallenge = 0;
                        break;
                }
                await Task.Delay((i<33 && i>16)?delay+100+i*10:(i<=16 && i>8)?delay+100:delay);
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
                default:
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
