using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.Modell.Challenges
{
    public abstract class QuestionChallenge: ChallengeBase
    {
        public int? AnswersID { get; set; }
        public Answers Answers { get; set; }
    }
}
