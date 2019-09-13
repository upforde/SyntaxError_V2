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
                    break;
                case 4:
                    QuizChallengeArea.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    ScreenshotChallengeArea.Visibility = Visibility.Collapsed;
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
            ToggleShadowOpacity(0.5);
            ImgMove.Stop();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            GameProfile = (e.Parameter as GameObjectForSending).GameProfile;
            
            foreach (ListItemMainPage challenge in ((GameObjectForSending)e.Parameter).Challenges)
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
                                CrewCrewMember.Text = challenge.CrewMember.CrewTag;
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

        public async void ToggleSaturationQuiz()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                for (double i = 1; i >= 0; i -= 0.1)
                                {
                                    QuizSaturationBrush.Saturation = i;
                                    await Task.Delay(1);
                                }
                            });
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
