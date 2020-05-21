using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.Modell.Challenges
{
    /// <summary>Challange that has something to do with an <see cref="Modell.ChallengeObjects.Answers"/></summary>
    public abstract class QuestionChallenge: ChallengeBase
    {
        public int? AnswersID { get; set; }
        public Answers Answers { get; set; }
    }
}
