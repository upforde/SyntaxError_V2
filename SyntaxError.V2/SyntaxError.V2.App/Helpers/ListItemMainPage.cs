using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SyntaxError.V2.App.Helpers
{
    public class ListItemMainPage
    {
        public GameProfile GameProfile { get; set; }
        public ICommand DeleteCommandGameProfile { get; set; }
        
        public ObservableCollection<ListItemMainPage> Challenges { get; set; }
        
        public ChallengeBase Challenge { get; set; }
        public Visibility IsGameChallenge { get; set; } = Visibility.Collapsed;
        public Visibility IsQuestionChallenge { get; set; } = Visibility.Collapsed;
        public Visibility IsMusicChallenge { get; set; } = Visibility.Collapsed;
        public Visibility IsImageChallenge { get; set; } = Visibility.Collapsed;

        public bool IsChallengeCompleted { get; set; }

        public SolidColorBrush GetIsChallengeCompletedBrush()
        {
            if (IsChallengeCompleted)
                return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 105, 97)); // #FF6961

            return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 119, 221, 119)); // #77DD77 
        }

        public string DecideCorrectText(ChallengeBase challenge)
        {
            switch (challenge.GetDiscriminator())
            {
                case "AudienceChallenge":
                case "CrewChallenge":
                case "SologameChallenge":
                    return ((challenge as GameChallenge).Game != null)?(challenge as GameChallenge).Game.Name:"";
                case "MultipleChoiceChallenge":
                case "QuizChallenge":
                    return ((challenge as QuestionChallenge).Answers != null)?(challenge as QuestionChallenge).Answers.Answer:"";
                case "MusicChallenge":
                    return ((challenge as MusicChallenge).Song != null)?(challenge as MusicChallenge).Song.Name:"";
                case "ScreenshotChallenge":
                case "SilhouetteChallenge":
                    return ((challenge as ImageChallenge).Image != null)?(challenge as ImageChallenge).Image.Name:"";
                default:
                    return "";
            }
        }
    }
}
