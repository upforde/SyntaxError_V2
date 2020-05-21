using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.Modell.Challenges
{
    /// <summary>Challenge where a contestant has to guess which game the music is from</summary>
    public class MusicChallenge: ChallengeBase
    {
        public int? SongID { get; set; }
        public Music Song { get; set; }
    }
}
