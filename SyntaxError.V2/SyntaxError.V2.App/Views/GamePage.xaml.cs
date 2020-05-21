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
using System.ComponentModel;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class GamePage : Page
    {
        /// <summary>The current challenge</summary>
        public int? CurrentChallenge;
        
        public GameViewModel ViewModel { get; } = new GameViewModel();
        public ListItemMainPage GameProfileAndChallenges { get; set; }
        public GameProfile GameProfile { get; set; }

        /// <summary>The bitmap image</summary>
        public BitmapImage bitmapImage;

        /// <summary>The audience challenges</summary>
        public List<AudienceChallenge> AudienceChallenges = new List<AudienceChallenge>();
        /// <summary>The crew challenges</summary>
        public List<CrewChallenge> CrewChallenges = new List<CrewChallenge>();
        /// <summary>The multiple choice challenges</summary>
        public List<MultipleChoiceChallenge> MultipleChoiceChallenges = new List<MultipleChoiceChallenge>();
        /// <summary>The music challenges</summary>
        public List<MusicChallenge> MusicChallenges =  new List<MusicChallenge>();
        /// <summary>The quiz challenges</summary>
        public List<QuizChallenge> QuizChallenges = new List<QuizChallenge>();
        /// <summary>The screenshot challenges</summary>
        public List<ScreenshotChallenge> ScreenshotChallenges = new List<ScreenshotChallenge>();
        /// <summary>The silhouette challenges</summary>
        public List<SilhouetteChallenge> SilhouetteChallenges = new List<SilhouetteChallenge>();
        /// <summary>The sologame challenges</summary>
        public List<SologameChallenge> SologameChallenges = new List<SologameChallenge>();

        private bool _audienceAvailable = true;
        private bool _crewAvailable = true;
        private bool _multipleChoiceAvailable = true;
        private bool _musicAvailable = true;
        private bool _quizAvailable = true;
        private bool _screenshotAvailable = true;
        private bool _silhuetteAvailable = true;
        private bool _sologameAvailable = true;

        /// <summary>Occurs when random selection is done.</summary>
        public event PropertyChangedEventHandler RandomSelectDone;

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

        /// <summary>Invoked when the Page is loaded and becomes the current source of a parent Frame.</summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.
        /// </param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            GameProfileAndChallenges = e.Parameter as ListItemMainPage;

            GameProfile = (e.Parameter as ListItemMainPage).GameProfile;

            foreach (ListItemMainPage challenge in ((ListItemMainPage)e.Parameter).Challenges)
            {
                if (challenge.Challenge != null)
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

        /// <summary>Collapses the challenge windows.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
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

        /// <summary>  Collapses the music answer area.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void MusicOpacityUp_Completed(object sender, object e)
        {
            MusicAnswerArea.Visibility = Visibility.Collapsed;
        }
        /// <summary>  Collapses the quiz answer plane.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void QuizOpacityUp_Completed(object sender, object e)
        {
            QuizAnswerPlane.Visibility = Visibility.Collapsed;
        }
        /// <summary>  Collapses the screenshot answer area.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void ScreenshotOpacityUp_Completed(object sender, object e)
        {
            ScreenshotAnswerArea.Visibility = Visibility.Collapsed;
        }
        /// <summary>  Shows the silhouette action area.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void SilhouetteOpacityUp_Completed(object sender, object e)
        {
            SilhouetteActionArea.Visibility = Visibility.Visible;
        }
        /// <summary>  Stops the images when opacity change has completed.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OpacityCompleted(object sender, object e)
        {
            ImgMove.Stop();
        }

        /// <summary>Toggles the play screen.</summary>
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
        /// <summary>Toggles the main screen.</summary>
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

        /// <summary>Toggles the moving images at the top and bottom of the game screen.</summary>
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

        /// <summary>Toggles the syntax error.</summary>
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
        /// <summary>Toggles the syntax error fix.</summary>
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

        /// <summary>Animates the syntax error for logo.</summary>
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
        /// <summary>Fixes the syntax error for logo.</summary>
        private void FixSyntaxErrorLogo()
        {
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorLogo.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorLogo.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorLogo.Begin();
        }
        /// <summary>Animates the syntax error for audience challenge.</summary>
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
        /// <summary>Fixes the syntax error for audience.</summary>
        private void FixSyntaxErrorAudience()
        {
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorAudience.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorAudience.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorAudience.Begin();
        }
        /// <summary>Animates the syntax error for crew challenge.</summary>
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
        /// <summary>Fixes the syntax error for crew.</summary>
        private void FixSyntaxErrorCrew()
        {
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorCrew.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorCrew.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorCrew.Begin();
        }
        /// <summary>Animates the syntax error for multiple choice challenge.</summary>
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
        /// <summary>Fixes the syntax error for multiple choice.</summary>
        private void FixSyntaxErrorMultipleChoice()
        {
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMultipleChoice.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMultipleChoice.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorMultipleChoice.Begin();
        }
        /// <summary>Animates the syntax error for music challenge.</summary>
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
        /// <summary>Fixes the syntax error for music.</summary>
        private void FixSyntaxErrorMusic()
        {
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMusic.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorMusic.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorMusic.Begin();
        }
        /// <summary>Animates the syntax error for quiz challenge.</summary>
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
        /// <summary>Fixes the syntax error for quiz.</summary>
        private void FixSyntaxErrorQuiz()
        {
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorQuiz.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorQuiz.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorQuiz.Begin();
        }
        /// <summary>Animates the syntax error for screenshot challenge.</summary>
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
        /// <summary>Fixes the syntax error for screenshot.</summary>
        private void FixSyntaxErrorScreenshot()
        {
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorScreenshot.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorScreenshot.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorScreenshot.Begin();
        }
        /// <summary>Animates the syntax error for silhouette challenge.</summary>
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
        /// <summary>Fixes the syntax error for silhouette.</summary>
        private void FixSyntaxErrorSilhouette()
        {
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSilhouette.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSilhouette.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorSilhouette.Begin();
        }
        /// <summary>Animates the syntax error for sologame challenge.</summary>
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
        /// <summary>Fixes the syntax error for sologame.</summary>
        private void FixSyntaxErrorSologame()
        {
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSologame.Children[1].SetValue(DoubleAnimation.ToProperty, 0);
            SyntaxErrorSologame.Children[0].SetValue(DoubleAnimation.DurationProperty, new Duration(TimeSpan.FromMilliseconds(2500)));
            SyntaxErrorSologame.Begin();
        }

        /// <summary>Actuates the audience challenge.</summary>
        /// <param name="challenge">The challenge.</param>
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
        /// <summary>Actuates the crew challenge.</summary>
        /// <param name="challenge">The challenge.</param>
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
        /// <summary>Actuates the multiple choice challenge.</summary>
        /// <param name="challenge">The challenge.</param>
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
        /// <summary>Actuates the music challenge.</summary>
        /// <param name="challenge">The challenge.</param>
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
        /// <summary>Actuates the quiz challenge.</summary>
        /// <param name="challenge">The challenge.</param>
        public async void ActuateQuizChallenge(QuizChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                QuizTask.Text = challenge.ChallengeTask;
                                QuizAnswer.Text = challenge.Answers.Answer;
                            });
        }
        /// <summary>Actuates the screenshot challenge.</summary>
        /// <param name="challenge">The challenge.</param>
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
        /// <summary>Actuates the silhouette challenge.</summary>
        /// <param name="challenge">The challenge.</param>
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
        /// <summary>Actuates the sologame challenge.</summary>
        /// <param name="challenge">The challenge.</param>
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

        /// <summary>Answers the multiple choice challenge.</summary>
        /// <param name="challenge">The challenge.</param>
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
        /// <summary>Answers the music challenge.</summary>
        /// <param name="challenge">The challenge.</param>
        public async void AnswerMusicChallenge(MusicChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                MusicAnswerArea.Visibility = Visibility.Visible;
                            });
        }
        /// <summary>Answers the quiz challenge.</summary>
        /// <param name="challenge">The challenge.</param>
        public async void AnswerQuizChallenge(QuizChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                QuizAnswerPlane.Visibility = Visibility.Visible;
                            });
        }
        /// <summary>Answers the screenshot challenge.</summary>
        /// <param name="challenge">The challenge.</param>
        public async void AnswerScreenshotChallenge(ScreenshotChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                ScreenshotAnswerArea.Visibility = Visibility.Visible;
                            });
        }
        /// <summary>Answers the silhouette challenge.</summary>
        /// <param name="challenge">The challenge.</param>
        public async void AnswerSilhouetteChallenge(SilhouetteChallenge challenge)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                Silhouette.Visibility = Visibility.Collapsed;
                                SilhouetteAnswer.Text = challenge.Image.Name;
                            });
        }

        /// <summary>Toggles the audience challenge.</summary>
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
        /// <summary>Toggles the crew challenge.</summary>
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
        /// <summary>Toggles the multiple choice challenge.</summary>
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
        /// <summary>Toggles the music challenge.</summary>
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
        /// <summary>Toggles the quiz challenge.</summary>
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
        /// <summary>Toggles the screenshot challenge.</summary>
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
        /// <summary>Toggles the silhouette challenge.</summary>
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
        /// <summary>Toggles the sologame challenge.</summary>
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
        /// <summary>Toggles the deselect.</summary>
        public async void ToggleDeselect()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => DeselectChallenge());
        }

        /// <summary>Toggles the audience saturation.</summary>
        public async void ToggleAudienceSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await audienceChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                                _audienceAvailable = false;
                            });
        }
        /// <summary>Toggles the crew saturation.</summary>
        public async void ToggleCrewSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await crewChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                                _crewAvailable = false;
                            });
        }
        /// <summary>Toggles the multiple choice saturation.</summary>
        public async void ToggleMultipleChoiceSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await multipleChoiceChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                                _multipleChoiceAvailable = false;
                            });
        }
        /// <summary>Toggles the music saturation.</summary>
        public async void ToggleMusicSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await musicChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                                _musicAvailable = false;
                            });
        }
        /// <summary>Toggles the quiz saturation.</summary>
        public async void ToggleQuizSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await quizChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                                _quizAvailable = false;
                            });
        }
        /// <summary>Toggles the screenshot saturation.</summary>
        public async void ToggleScreenshotSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await screenshotChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                                _screenshotAvailable = false;
                            });
        }
        /// <summary>Toggles the silhouette saturation.</summary>
        public async void ToggleSilhouetteSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await silhouetteChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                                _silhuetteAvailable = false;
                            });
        }
        /// <summary>Toggles the sologame saturation.</summary>
        public async void ToggleSologameSaturation()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                await sologameChallenge.Saturation(value:0, duration:1000, delay:1000).StartAsync();
                                _sologameAvailable = false;
                            });
        }

        /// <summary>Toggles the random selection.</summary>
        public async void ToggleRandomSelection()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                RandomSelectChallenge();
                            });
        }

        /// <summary>Highlights audience.</summary>
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
        /// <summary> Dehighlights audience.</summary>
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

        /// <summary>Highlights crew.</summary>
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
        /// <summary>Dehighlights crew.</summary>
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

        /// <summary>Highlights multiplechoice.</summary>
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
        /// <summary>Dehighlights multiplechoice.</summary>
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

        /// <summary>Highlights music.</summary>
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
        /// <summary>Dehighlights music.</summary>
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

        /// <summary>Highlights quiz.</summary>
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
        /// <summary>Dehighlights quiz.</summary>
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

        /// <summary>Highlights screenshot.</summary>
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
        /// <summary>Dehighlights screenshot.</summary>
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

        /// <summary>Highlights silhouette.</summary>
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
        /// <summary>Dehighlights silhouette.</summary>
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

        /// <summary>Highlights sologame.</summary>
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
        /// <summary>  Dehighlights sologame.</summary>
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

        /// <summary>Selects random challenge.</summary>
        private async void RandomSelectChallenge()
        {
            int rnd = RandomNumber(16, 33);
            int delay = 200;
            DeselectChallenge();
            if (_audienceAvailable)
            {
                HighlightAudience();
                CurrentChallenge = 0;
            }
            await Task.Delay(delay);
            for (int i = 0; i < rnd; i++)
            {
                DeselectChallenge();
                switch (i % 8)
                {
                    case 0:
                        if (_crewAvailable)
                        {
                            HighlightCrew();
                            CurrentChallenge = 1;
                            await Task.Delay((i < 33 && i > 16) ? delay + 100 + i * 10 : (i <= 16 && i > 8) ? delay + 100 + i : delay);
                        }
                        break;
                    case 1:
                        if (_multipleChoiceAvailable)
                        {
                            HighlightMultipleChoice();
                            CurrentChallenge = 2;
                            await Task.Delay((i < 33 && i > 16) ? delay + 100 + i * 10 : (i <= 16 && i > 8) ? delay + 100 + i : delay);
                        }
                        break;
                    case 2:
                        if (_musicAvailable)
                        {
                            HighlightMusic();
                            CurrentChallenge = 3;
                            await Task.Delay((i < 33 && i > 16) ? delay + 100 + i * 10 : (i <= 16 && i > 8) ? delay + 100 + i : delay);
                        }
                        break;
                    case 3:
                        if (_quizAvailable)
                        {
                            HighlightQuiz();
                            CurrentChallenge = 4;
                            await Task.Delay((i < 33 && i > 16) ? delay + 100 + i * 10 : (i <= 16 && i > 8) ? delay + 100 + i : delay);
                        }
                        break;
                    case 4:
                        if (_screenshotAvailable)
                        {
                            HighlightScreenshot();
                            CurrentChallenge = 5;
                            await Task.Delay((i < 33 && i > 16) ? delay + 100 + i * 10 : (i <= 16 && i > 8) ? delay + 100 + i : delay);
                        }
                        break;
                    case 5:
                        if (_silhuetteAvailable)
                        {
                            HighlightSilhouette();
                            CurrentChallenge = 6;
                            await Task.Delay((i < 33 && i > 16) ? delay + 100 + i * 10 : (i <= 16 && i > 8) ? delay + 100 + i : delay);
                        }
                        break;
                    case 6:
                        if (_sologameAvailable)
                        {
                            HighlightSologame();
                            CurrentChallenge = 7;
                            await Task.Delay((i < 33 && i > 16) ? delay + 100 + i * 10 : (i <= 16 && i > 8) ? delay + 100 + i : delay);
                        }
                        break;
                    case 7:
                        if (_audienceAvailable)
                        {
                            HighlightAudience();
                            CurrentChallenge = 0;
                            await Task.Delay((i < 33 && i > 16) ? delay + 100 + i * 10 : (i <= 16 && i > 8) ? delay + 100 + i : delay);
                        }
                        break;
                }
            }
            if (CurrentChallenge == null)
            {
                DeselectChallenge();
                await Task.Delay(delay);
                if (_audienceAvailable)
                {
                    HighlightAudience();
                    CurrentChallenge = 0;
                }
                else if (_crewAvailable)
                {
                    HighlightCrew();
                    CurrentChallenge = 1;
                }
                else if (_multipleChoiceAvailable)
                {
                    HighlightMultipleChoice();
                    CurrentChallenge = 2;
                }
                else if (_musicAvailable)
                {
                    HighlightMusic();
                    CurrentChallenge = 3;
                }
                else if (_quizAvailable)
                {
                    HighlightQuiz();
                    CurrentChallenge = 4;
                }
                else if (_screenshotAvailable)
                {
                    HighlightScreenshot();
                    CurrentChallenge = 5;
                }
                else if (_silhuetteAvailable)
                {
                    HighlightSilhouette();
                    CurrentChallenge = 6;
                }
                else if (_sologameAvailable)
                {
                    HighlightSologame();
                    CurrentChallenge = 7;
                }
            }
            RandomSelectDone.Invoke(this, new PropertyChangedEventArgs("RandomSelectDone"));
        }
        /// <summary>Deselects the challenge.</summary>
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
        /// <summary>Toggles the shadow opacity.</summary>
        /// <param name="level">The level.</param>
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

        /// <summary>  Returns a random number between the given minimum and maximum values.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
