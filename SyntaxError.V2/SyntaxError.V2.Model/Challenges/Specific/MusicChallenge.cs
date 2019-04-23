using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.Modell.Challenges
{
    public class MusicChallenge: ChallengeBase
    {
        public int? SongID { get; set; }
        public Music Song { get; set; }
    }
}
