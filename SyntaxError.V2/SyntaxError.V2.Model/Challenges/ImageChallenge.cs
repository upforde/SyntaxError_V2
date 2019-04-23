using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.Modell.Challenges
{
    public abstract class ImageChallenge: ChallengeBase
    {
        public int? ImageID { get; set; }
        public Image Image { get; set; }
    }
}
