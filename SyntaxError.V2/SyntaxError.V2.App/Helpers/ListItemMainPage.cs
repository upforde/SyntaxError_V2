using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using System.Drawing;
using System.Windows.Input;

namespace SyntaxError.V2.App.Helpers
{
    public class ListItemMainPage
    {
        public GameProfile GameProfile { get; set; }
        public ICommand DeleteCommandGameProfile { get; set; }
        public ICommand EditCommandGameProfile { get; set; }

        public AudienceChallenge AudienceChallenge { get; set; }
        public CrewChallenge CrewChallenge { get; set; }
        public MultipleChoiceChallenge MultipleChoiceChallenge { get; set; }
        public MusicChallenge MusicChallenge { get; set; }
        public QuizChallenge QuizChallenge { get; set; }
        public ScreenshotChallenge ScreenshotChallenge { get; set; }
        public SilhouetteChallenge SilhouetteChallenge { get; set; }
        public SologameChallenge SologameChallenge { get; set; }
        
        public bool IsChallengeCompleted { get; set; }
        public string GetIsChallengeCompletedString()
        {
            if (IsChallengeCompleted)
                return "Completed";

            return "Not yet";
        }
    }
}
