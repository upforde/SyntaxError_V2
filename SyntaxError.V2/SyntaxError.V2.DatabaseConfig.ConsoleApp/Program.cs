using Microsoft.EntityFrameworkCore;
using SyntaxError.V2.DataAccess;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;
using System;
using System.Linq;

namespace SyntaxError.V2.DatabaseConfig.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome, user! Here are the available commands:\n");
            WriteHelpCommands();
            FindWhatToDo(Console.ReadLine());
        }

        static void FindWhatToDo(string argument)
        {
            switch (argument.ToLower())
            {
                case "show challenges":
                    ShowChallenges();
                    break;
                case "add challenge":
                    AddChallenge();
                    break;
                case "help":
                    WriteHelpCommands();
                    break;
                case "cls":
                    Console.Clear();
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
            FindWhatToDo(Console.ReadLine());
        }

        /// <summary>Writes the help commands.</summary>
        private static void WriteHelpCommands()
        {
            Console.WriteLine("Show challenges: Lets the user see the challenges stored in the database\n" +
                "Add challenge: Lets the user add a challenge to the database\n" +
                "Help: Lets the user see all available commands\n" +
                "cls: Clears the terminal\n" +
                "exit: closes the terminal");
        }

        /// <summary>Adds a challenge to the database.</summary>
        private static void AddChallenge()
        {
            Console.WriteLine("What kind of challenge do you want to add?");
            switch (Console.ReadLine().ToLower()) {
                case "audiencechallenge":
                    AddAudienceChallenge();
                    break;
                case "crewchallenge":
                    AddCrewChallenge();
                    break;
                case "multiplechoicechallenge":
                    AddMultipleChouceChallenge();
                    break;
                case "musicchallenge":
                    AddMusicChallenge();
                    break;
                case "quizchallenge":
                    AddQuizChallenge();
                    break;
                case "screenshotchallenge":
                    AddScreenshotChallenge();
                    break;
                case "silhouettechallenge":
                    AddSilhouetteChallenge();
                    break;
                case "sologamechallenge":
                    AddSologameChallenge();
                    break;
                case "back":
                    break;
                default:
                    Console.WriteLine("No such type of challenge\n");
                    AddChallenge();
                    break;
            }
        }

        /// <summary>Adds the audience challenge.</summary>
        private static void AddAudienceChallenge()
        {
            Console.WriteLine("Add game to database? (y/n)");
            Game game = null;
            AudienceChallenge newChallenge = null;
            switch (Console.ReadLine().ToLower())
            {
                case "y":
                    game = new Game();
                    Console.WriteLine("What is the name of the game?");
                    game.Name = Console.ReadLine();
                    Console.WriteLine("What is the path to the game poster?");
                    game.URI = Console.ReadLine();

                    newChallenge = new AudienceChallenge() { Game = game };
                    break;
                case "n":
                    Console.WriteLine("Which game do you want to use for this challenge?\n" +
                        "(Use the ID to choose the game)");
                    using (var db = new SyntaxErrorContext())
                    {
                        var games = GetObjectsOfType(db, "Game");

                        if (games.Length != 0)
                        {
                            foreach(Game item in games)
                            {
                                Console.WriteLine("-----------------------------------------------\n" +
                                    "GameID: {0}\n" +
                                    "GameName: {1}\n" +
                                    "-----------------------------------------------\n",
                                    item.ID,
                                    item.Name
                                    );
                            }

                            var choice = int.Parse(Console.ReadLine());

                            game = (Game) db.Objects
                                .Where(g => g.ID == choice)
                                .Single();

                            newChallenge = new AudienceChallenge() { GameID = game.ID };

                        } else Console.WriteLine("No games in the database.");
                    }
                    break;
                default:
                    Console.WriteLine("Wrong input");
                    break;
            }

            if (game != null)
            {
                Console.WriteLine("What is the task?");
                var task = Console.ReadLine();

                using (var db = new SyntaxErrorContext())
                {
                    newChallenge.ChallengeTask = task;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
            }
            else Console.WriteLine("No game selected.");
        }
        
        private static void AddCrewChallenge()
        {
            throw new NotImplementedException();
        }

        /// <summary>Adds the multiple chouce challenge.</summary>
        private static void AddMultipleChouceChallenge()
        {
            Console.WriteLine("What is the question?");
            var question = Console.ReadLine();
            Console.WriteLine("What is the answer to the question?");
            var answer = Console.ReadLine();
            Console.WriteLine("Provide a dummy answer");
            var dummyAnswer1 = Console.ReadLine();
            Console.WriteLine("Provide a dummy answer");
            var dummyAnswer2 = Console.ReadLine();
            Console.WriteLine("Provide a dummy answer");
            var dummyAnswer3 = Console.ReadLine();

            using(var db = new SyntaxErrorContext())
            {
                Answers multipleChoiceAnswers = new Answers() 
                { 
                    Answer = answer,
                    DummyAnswer1 = dummyAnswer1,
                    DummyAnswer2 = dummyAnswer2,
                    DummyAnswer3 = dummyAnswer3
                };

                MultipleChoiceChallenge newChallenge = new MultipleChoiceChallenge()
                {
                    Answers = multipleChoiceAnswers,
                    ChallengeTask = question
                };

                db.Challenges.Add(newChallenge);
                db.SaveChanges();
            }
        }
        
        private static void AddMusicChallenge()
        {
            throw new NotImplementedException();
        }

        /// <summary>Adds the quiz challenge.</summary>
        private static void AddQuizChallenge()
        {
            Console.WriteLine("What is the question?");
            var question = Console.ReadLine();
            Console.WriteLine("What is the answer to the question?");
            var answer = Console.ReadLine();

            using(var db = new SyntaxErrorContext())
            {
                Answers quizAnswer = new Answers() {Answer = answer };

                QuizChallenge newChallenge = new QuizChallenge()
                {
                    Answers = quizAnswer,
                    ChallengeTask = question
                };

                db.Challenges.Add(newChallenge);
                db.SaveChanges();
            }
        }

        private static void AddScreenshotChallenge()
        {
            throw new NotImplementedException();
        }

        private static void AddSilhouetteChallenge()
        {
            throw new NotImplementedException();
        }

        private static void AddSologameChallenge()
        {
            throw new NotImplementedException();
        }

        /// <summary>Shows the challenges.</summary>
        private static void ShowChallenges()
        {
            using (var db = new SyntaxErrorContext())
            {
                Console.WriteLine("What kind of challenes would you like to see?");

                var types = db.Challenges
                    .Select(x => x.GetDiscriminator())
                    .Distinct();

                foreach(var type in types)
                {
                    Console.WriteLine("\t-{0}", type);
                }
                Console.WriteLine("\t-All");
                
                var typeOfChallenge = Console.ReadLine();

                switch (typeOfChallenge.ToLower())
                {
                    case "audiencechallenge":
                        ShowAudienceChallenges(db);
                        break;
                    case "crewchallenge":
                        ShowCrewChallenges(db);
                        break;
                    case "multiplechoicechallenge":
                        ShowMultipleChoiceChallenges(db);
                        break;
                    case "musicchallenge":
                        ShowMusicChallenges(db);
                        break;
                    case "quizchallenge":
                        ShowQuizChallenges(db);
                        break;
                    case "screenshotchallenge":
                        ShowScreenshotChallenges(db);
                        break;
                    case "silhouettechallenge":
                        ShowSilhouetteChallenges(db);
                        break;
                    case "sologamechallenge":
                        ShowSologameChallenges(db);
                        break;
                    case "all":
                        ShowAllChallenges(db);
                        break;
                    default:
                        Console.WriteLine("Wrong input. No such category of challenge in the database");
                        break;
                }
            }
        }

        /// <summary>Shows the audience challenges.</summary>
        /// <param name="db">The database.</param>
        private static void ShowAudienceChallenges(SyntaxErrorContext db)
        {
            Console.WriteLine();
            var challenges = GetChallengesOfType(db, "AudienceChallenge");

            if (challenges.Length != 0)
            {
                foreach (AudienceChallenge challenge in challenges)
                {
                    challenge.Game = (Game)db.Objects
                        .Where(g => g.ID == challenge.GameID)
                        .Single();

                    Console.WriteLine("-----------------------------------------------\n" +
                        "Challenge ID: {0}\n" +
                        "Challenge task: {1}\n" +
                        "Challenge Game: {2}\n" +
                        "-----------------------------------------------\n",
                        challenge.ChallengeID,
                        challenge.ChallengeTask,
                        challenge.Game.Name);
                }
            } else Console.WriteLine("No challenges of type AudienceChallenge in database.");
        }
        
        private static void ShowCrewChallenges(SyntaxErrorContext db)
        {
            throw new NotImplementedException();
        }

        /// <summary>Shows the multiple choice challenges.</summary>
        /// <param name="db">The database.</param>
        private static void ShowMultipleChoiceChallenges(SyntaxErrorContext db)
        {
            Console.WriteLine();
            var challenges = GetChallengesOfType(db, "MultipleChoiceChallenge");

            if (challenges.Length != 0)
            {
                foreach (MultipleChoiceChallenge challenge in challenges)
                {
                    challenge.Answers = db.Answers
                        .Where(a => a.AnswersID == challenge.AnswersID)
                        .Single();
                    
                    Console.WriteLine("-----------------------------------------------\n" +
                        "Challenge ID: {0}\n" +
                        "Challenge task: {0}",
                        challenge.ChallengeID,
                        challenge.ChallengeTask);

                    foreach (var answer in challenge.Answers.GetAll())
                        Console.WriteLine("\t-{0}" + ((answer.Equals(challenge.Answers.Answer))?" (answer)":""), answer);

                    Console.WriteLine("-----------------------------------------------\n");
                }
            } else Console.WriteLine("No challenges of type MultipleChoiceChallenge in database.");
        }

        private static void ShowMusicChallenges(SyntaxErrorContext db)
        {
            throw new NotImplementedException();
        }

        /// <summary>Shows the quiz challenges.</summary>
        /// <param name="db">The database.</param>
        private static void ShowQuizChallenges(SyntaxErrorContext db)
        {
            Console.WriteLine();
            var challenges = GetChallengesOfType(db, "QuizChallenge");

            if (challenges.Length != 0)
            {
                foreach (QuizChallenge challenge in challenges)
                    {
                    challenge.Answers = db.Answers
                        .Where(a => a.AnswersID == challenge.AnswersID)
                        .Single();

                    Console.WriteLine("-----------------------------------------------\n" +
                        "Challenge ID: {0}\n" + 
                        "Challenge task: {1}\n" +
                        "Answer: {2}\n" +
                        "-----------------------------------------------\n",
                        challenge.ChallengeID,
                        challenge.ChallengeTask,
                        challenge.Answers.Answer);
                }
            } else Console.WriteLine("No challenges of type QuizChallenge in database.");
        }

        private static void ShowScreenshotChallenges(SyntaxErrorContext db)
        {
            throw new NotImplementedException();
        }

        private static void ShowSilhouetteChallenges(SyntaxErrorContext db)
        {
            throw new NotImplementedException();
        }

        private static void ShowSologameChallenges(SyntaxErrorContext db)
        {
            throw new NotImplementedException();
        }

        /// <summary>Shows all challenges.</summary>
        /// <param name="db">The database.</param>
        private static void ShowAllChallenges(SyntaxErrorContext db)
        {
            Console.WriteLine();
            var challenges = db.Challenges
                .ToArray();

            if (challenges.Length != 0)
            {
                foreach (var challenge in challenges)
                {
                    Console.WriteLine("-----------------------------------------------\n" +
                        "Challenge ID: {0}\n" +
                        "Challenge type: {1}\n" +
                        "Challenge task: {2}\n" +
                        "-----------------------------------------------\n",
                        challenge.ChallengeID,
                        challenge.GetDiscriminator(),
                        challenge.ChallengeTask);
                }
            } else Console.WriteLine("No challenges in the database.");
        }

        /// <summary>Gets challenges of specific types.
        /// Type of challenges is specified in the parameters</summary>
        /// <param name="db">The database.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static ChallengeBase[] GetChallengesOfType(SyntaxErrorContext db, string type)
        {
            return db.Challenges
                .Where(c => c.GetDiscriminator() == type)
                .ToArray();
        }

        /// <summary>Gets objects of specific types. Types of objects is specified in the parameters</summary>
        /// <param name="db">The database.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static OuterSourceObject[] GetObjectsOfType(SyntaxErrorContext db, string type)
        {
            return db.Objects
                .Where(g => g.GetType().Name.Equals(type))
                .ToArray();
        }
    }
}
