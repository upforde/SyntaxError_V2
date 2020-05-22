using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SyntaxError.V2.App.Helpers
{
    /// <summary>
    ///   <para>Class meant for holding ListItem information. Wether it is one of the challenges of any type, a list containing many challenges of different types or a game profile.</para>
    ///   <para>This class was needed because when assigning the datatype of things inside of a ListView control in the XAML, I could only use one datatype. Since every challenge is it's own datatype, as well as the <see cref="GameProfile"/> being it's own datatype, I had no choice but to make a helper class that housed all of the relevant information for a list item in the main page.</para>
    /// </summary>
    public class ListItemMainPage
    {
        /// <summary>Gets or sets the game profile.</summary>
        /// <value>The game profile.</value>
        public GameProfile GameProfile { get; set; }
        /// <summary>Gets or sets the delete command game profile.</summary>
        /// <value>The delete command game profile.</value>
        public ICommand DeleteCommandGameProfile { get; set; }

        /// <summary>Gets or sets the challenges.</summary>
        /// <value>The challenges.</value>
        public ObservableCollection<ListItemMainPage> Challenges { get; set; }

        /// <summary>Gets or sets the challenge.</summary>
        /// <value>The challenge.</value>
        public ChallengeBase Challenge { get; set; }
        /// <summary>Gets or sets the is game challenge.</summary>
        /// <value>The is game challenge.</value>
        public Visibility IsGameChallenge { get; set; } = Visibility.Collapsed;
        /// <summary>Gets or sets the is question challenge.</summary>
        /// <value>The is question challenge.</value>
        public Visibility IsQuestionChallenge { get; set; } = Visibility.Collapsed;
        /// <summary>Gets or sets the is music challenge.</summary>
        /// <value>The is music challenge.</value>
        public Visibility IsMusicChallenge { get; set; } = Visibility.Collapsed;
        /// <summary>Gets or sets the is image challenge.</summary>
        /// <value>The is image challenge.</value>
        public Visibility IsImageChallenge { get; set; } = Visibility.Collapsed;

        /// <summary>Gets or sets a value indicating whether the challenge is completed.</summary>
        /// <value>
        ///   <c>true</c> if this instance is challenge completed; otherwise, <c>false</c>.</value>
        public bool IsChallengeCompleted { get; set; }

        /// <summary>Gets the is challenge completed brush.</summary>
        /// <returns></returns>
        public SolidColorBrush GetIsChallengeCompletedBrush()
        {
            if (IsChallengeCompleted)
                return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 105, 97)); // #FF6961
            return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 119, 221, 119)); // #77DD77 
        }

        /// <summary>Decides the correct text based on the type of challenge.</summary>
        /// <param name="challenge">The challenge.</param>
        /// <returns></returns>
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
