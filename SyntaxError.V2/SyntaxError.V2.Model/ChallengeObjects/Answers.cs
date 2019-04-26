using SyntaxError.V2.Modell.Utility;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxError.V2.Modell.ChallengeObjects
{
    [Table("Answers")]
    public class Answers
    {
        [Key]
        public int AnswersID { get; set; }
        public string Answer { get; set; }
        public string DummyAnswer1 { get; set; }
        public string DummyAnswer2 { get; set; }
        public string DummyAnswer3 { get; set; }

        public List<string> GetAll()
        {
            List<string> AllAnswers = new List<string>
            {
                Answer,
                DummyAnswer1,
                DummyAnswer2,
                DummyAnswer3
            };

            AllAnswers.Shuffle();

            return AllAnswers;
        }
    }
}
