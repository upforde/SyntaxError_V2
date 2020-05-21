using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.Modell.Challenges
{
    /// <summary>Challange that has something to do with an <see cref="Modell.ChallengeObjects.Image"/></summary>
    public abstract class ImageChallenge: ChallengeBase
    {
        public int? ImageID { get; set; }
        public Image Image { get; set; }
    }
}
