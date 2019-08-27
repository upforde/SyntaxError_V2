using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using SyntaxError.V2.DataAccess;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;

namespace SyntaxError.V2.DatabaseConfig.ConsoleApp
{
    class Program
    {
        const string DONE = "Done!";

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome, user! Here are the available commands:\n");
            WriteHelpCommands();
            FindWhatToDo(Console.ReadLine());
        }

        /// <summary>Creates a new game profile.</summary>
        private static void NewGameProfile()
        {
            bool enough = false;
            List<int> idList = new List<int>(); 
            
            Console.WriteLine("What would you like to name your game profile?");
            string gameProfileName = Console.ReadLine();
            if (gameProfileName.Length == 0) gameProfileName = "SyntaxError game profile ";

            GameProfile newGame = new GameProfile()
            { 
                GameProfileName = gameProfileName,
                DateCreated = DateTime.Now,
                Profile = new Profile(),
                SaveGame = new SaveGame()
            };
            
            Console.WriteLine("Which challenges would you like to include?\n" +
                "(Use ID to identify the challenges. Type 'Stop' when you have enough challenges or 'All' for all challenges in the database)");
            ShowAllChallenges(new SyntaxErrorContext());
            int elementCount;
            using(var db = new SyntaxErrorContext()) { elementCount = db.Challenges.Count(); }
            string input = "undefined";
            
            while (!enough)
            {
                foreach (int i in idList) { Console.Write(" {0}", i.ToString()); };
                Console.WriteLine();
                input = Console.ReadLine().ToLower();
                if (input.Equals("stop")||input.Equals("all")||idList.Count() >= elementCount) enough = !enough;
                else
                {
                    if(int.TryParse(input, out int choice))
                    {
                        idList.Add(choice);
                        ClearTwoLines();
                    } else Console.WriteLine("Invalid input. Try again.");
                }
            }

            using (var db = new SyntaxErrorContext())
            {
                List<ChallengeBase> challenges;
                if (input.Equals("all"))
                {
                    challenges = db.Challenges.ToList();
                }
                else
                {
                    challenges = new List<ChallengeBase>();
                    foreach(int id in idList) challenges.Add(db.Challenges.Find(id));
                }
                foreach(var challenge in challenges) 
                {
                    if (challenge != null)
                    {
                        UsingChallenge usingChallenge = new UsingChallenge() { ChallengeID = challenge.ChallengeID };
                        newGame.Profile.Challenges.Add(usingChallenge);
                    }
                }

                db.GameProfiles.Add(newGame);
                db.SaveChanges();
            }
            Console.WriteLine(DONE);
        }

        /// <summary>Shows the game profiles.</summary>
        private static void ShowGameProfiles()
        {
            Console.WriteLine();

            using (var db = new SyntaxErrorContext())
            {
                var games = db.GameProfiles
                    .Include(g => g.Profile)
                    .ThenInclude(p => p.Challenges)
                    .Include(g => g.SaveGame)
                    .ThenInclude(sg => sg.Challenges)
                    .ToArray();

                if (games.Length != 0)
                {
                    foreach (var game in games)
                    {
                        Console.WriteLine("-----------------------------------------------\n" +
                            "Game ID: {0}\n" +
                            "GameProfile name: {1}\n" +
                            "Date of game created: {2}\n" +
                            "Number of challenges in the profile: {3}\n" +
                            "Number of those challenges completed: {4}\n" +
                            "-----------------------------------------------\n",
                            game.ID,
                            game.GameProfileName,
                            game.DateCreated.ToShortDateString(),
                            game.Profile.Challenges.Count,
                            game.SaveGame.Challenges.Count);
                    }
                } else Console.WriteLine("No games in the database.");
            }
        }

        /// <summary>Removes the game profile.</summary>
        private static void RemoveGameProfile()
        {
            Console.WriteLine("Which game would you like to remove?\n" +
                "(Use ID to identify the game)");
            ShowGameProfiles();

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                using (var db = new SyntaxErrorContext())
                {
                    try 
                    {
                        var game = db.GameProfiles
                            .Where(gp => gp.ID == choice)
                            .Include(gp => gp.Profile)
                            .ThenInclude(p => p.Challenges)
                            .Include(gp => gp.SaveGame)
                            .ThenInclude(sg => sg.Challenges)
                            .Single();

                        db.UsingChallenges.RemoveRange(game.Profile.Challenges);
                        db.UsingChallenges.RemoveRange(game.SaveGame.Challenges);
                        db.UsingPanes.Remove(game.Profile);
                        db.UsingPanes.Remove(game.SaveGame);
                        db.GameProfiles.Remove(game);
                        db.SaveChanges();
                    } catch (InvalidOperationException)
                    {
                        Console.WriteLine("No such challenge\n");
                    }
                }
                Console.WriteLine(DONE);
            } else Console.WriteLine("Invalid input");
        }

        /// <summary>Simulates a session with a specified game profile.</summary>
        private static void Play()
        {
            GameProfile game;

            Console.WriteLine("Which game would you like to play?\n" +
                "(Use ID to identify the game)");

            ShowGameProfiles();

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                using (var db = new SyntaxErrorContext())
                {
                    try 
                    {
                        game = db.GameProfiles
                            .Where(gp => gp.ID == choice)
                            .Include(g => g.Profile)
                            .ThenInclude(p => p.Challenges)
                            .Include(g => g.SaveGame)
                            .ThenInclude(sg => sg.Challenges)
                            .Single();
                        
                    } catch (InvalidOperationException)
                    {
                        Console.WriteLine("No such challenge\n");
                        game = null;
                    }
                }
            } else { Console.WriteLine("Invalid input"); game = null; }

            if(game != null) 
            {
                Console.WriteLine("Game retrieved.\n");

                SimulateGame(game);

                using (var db = new SyntaxErrorContext())
                {
                    var dbChallenges = db.UsingChallenges
                        .Where(uc => uc.UsingID == game.SaveGameID)
                        .ToArray();

                    foreach(var challenge in game.SaveGame.Challenges)
                    {
                        if (IsDuplicateAbsent(dbChallenges, challenge)) db.UsingChallenges.Add(challenge);
                    }
                    
                    db.SaveChanges();
                }
                
                Console.WriteLine("Saved!");
            }
        }

        /// <summary>Adds a challenge to the database.</summary>
        private static void AddChallenge()
        {
            Console.WriteLine("What kind of challenge do you want to add?\nAvailable Challenges:\n" +
                "\t-AudienceChallenge\n" +
                "\t-CrewChallenge\n" +
                "\t-MultipleChoiceChallenge\n" +
                "\t-MusicChallenge\n" +
                "\t-QuizChallenge\n" +
                "\t-ScreenshotChallenge\n" +
                "\t-SilhouetteChallenge\n" +
                "\t-SologameChallenge\n\nType 'back' to go back");
            switch (Console.ReadLine().ToLower()) {
                case "audiencechallenge":
                    Console.Clear();
                    AddAudienceChallenge();
                    break;
                case "crewchallenge":
                    Console.Clear();
                    AddCrewChallenge();
                    break;
                case "multiplechoicechallenge":
                    Console.Clear();
                    AddMultipleChouceChallenge();
                    break;
                case "musicchallenge":
                    Console.Clear();
                    AddMusicChallenge();
                    break;
                case "quizchallenge":
                    Console.Clear();
                    AddQuizChallenge();
                    break;
                case "screenshotchallenge":
                    Console.Clear();
                    AddScreenshotChallenge();
                    break;
                case "silhouettechallenge":
                    Console.Clear();
                    AddSilhouetteChallenge();
                    break;
                case "sologamechallenge":
                    Console.Clear();
                    AddSologameChallenge();
                    break;
                case "back":
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("No such type of challenge\n");
                    AddChallenge();
                    break;
            }
        }

        /// <summary>Adds the audience challenge.</summary>
        private static void AddAudienceChallenge()
        {
            Console.WriteLine("Add a new game to database? (y/n)");
            bool isAddTrue = Console.ReadLine().Equals("y");
            Game game = ConvertToGame(GetObjectForChallenge(new SyntaxErrorContext(), isAddTrue, "Game"));

            if (game != null)
            {
                Console.WriteLine("What is the task?");
                var task = Console.ReadLine();

                using (var db = new SyntaxErrorContext())
                {
                    AudienceChallenge newChallenge = new AudienceChallenge() { ChallengeTask = task };

                    if (isAddTrue) newChallenge.Game = game;
                    else newChallenge.GameID = game.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
                Console.WriteLine(DONE);
            } else Console.WriteLine("No game selected.");
        }

        /// <summary>Adds the crew challenge.</summary>
        private static void AddCrewChallenge()
        {
            Console.WriteLine("Add a new crew member to the database? (y/n)");
            bool isAddTrueCrew = Console.ReadLine().Equals("y");
            CrewMember crewMember = GetCrewMemberForChallenge(new SyntaxErrorContext(), isAddTrueCrew);
            Console.WriteLine("Add a new game to the database? (y/n)");
            bool isAddTrueGame = Console.ReadLine().Equals("y");
            Game game = ConvertToGame(GetObjectForChallenge(new SyntaxErrorContext(), isAddTrueGame, "Game"));

            if (game == null)
                Console.WriteLine("No game selected");
            else if (crewMember == null)
                Console.WriteLine("No crew member selected");
            else
            {
                Console.WriteLine("What is the task?");
                var task = Console.ReadLine();

                using (var db = new SyntaxErrorContext())
                {
                    CrewChallenge newChallenge = new CrewChallenge() { ChallengeTask = task };

                    if (isAddTrueCrew) newChallenge.CrewMember = crewMember;
                    else newChallenge.CrewMemberID = crewMember.CrewMemberID;

                    if (isAddTrueGame) newChallenge.Game = game;
                    else newChallenge.GameID = game.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
                Console.WriteLine(DONE);
            }
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
            Console.WriteLine(DONE);
        }

        /// <summary>Adds the music challenge.</summary>
        private static void AddMusicChallenge()
        {
            Console.WriteLine("Add a new song to the database? (y/n)");
            bool isAddTrue = Console.ReadLine().Equals("y");
            Music music = ConvertToMusic(GetObjectForChallenge(new SyntaxErrorContext(), isAddTrue, "Music"));

            if (music != null)
            {
                using (var db = new SyntaxErrorContext())
                {
                    MusicChallenge newChallenge = new MusicChallenge() { ChallengeTask = "Guess the song" };

                    if (isAddTrue) newChallenge.Song = music;
                    else newChallenge.SongID = music.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
                Console.WriteLine(DONE);
            } else Console.WriteLine("No music selected.");
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
                Answers quizAnswer = new Answers() { Answer = answer };

                QuizChallenge newChallenge = new QuizChallenge()
                {
                    Answers = quizAnswer,
                    ChallengeTask = question
                };

                db.Challenges.Add(newChallenge);
                db.SaveChanges();
            }
            Console.WriteLine(DONE);
        }

        /// <summary>Adds the screenshot challenge.</summary>
        private static void AddScreenshotChallenge()
        {
            Console.WriteLine("Add a new screenshot to the database? (y/n)");
            bool isAddTrue = Console.ReadLine().Equals("y");
            Image image = ConvertToImage(GetObjectForChallenge(new SyntaxErrorContext(), isAddTrue, "Image"));

            if (image != null)
            {
                using (var db = new SyntaxErrorContext())
                {
                    ScreenshotChallenge newChallenge = new ScreenshotChallenge() { ChallengeTask = "What game is this screenshot from?" };
                    
                    if(isAddTrue) newChallenge.Image = image;
                    else newChallenge.ImageID = image.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
            Console.WriteLine(DONE);
            } else Console.WriteLine("No screenshots selected.");
        }

        /// <summary>Adds the silhouette challenge.</summary>
        private static void AddSilhouetteChallenge()
        {
            Console.WriteLine("Add a new silhouette to the database? (y/n)");
            bool isAddTrue = Console.ReadLine().Equals("y");
            Image image = ConvertToImage(GetObjectForChallenge(new SyntaxErrorContext(), isAddTrue, "Image"));

            if (image != null)
            {
                using (var db = new SyntaxErrorContext())
                {
                    SilhouetteChallenge newChallenge = new SilhouetteChallenge() { ChallengeTask = "What character is this?" };

                    if(isAddTrue) newChallenge.Image = image;
                    else newChallenge.ImageID = image.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
            Console.WriteLine(DONE);
            } else Console.WriteLine("No screenshots selected.");
        }

        /// <summary>Adds the sologame challenge.</summary>
        private static void AddSologameChallenge()
        {
            Console.WriteLine("Add a new game to database? (y/n)");
            bool isAddTrue = Console.ReadLine().Equals("y");
            Game game = ConvertToGame(GetObjectForChallenge(new SyntaxErrorContext(), isAddTrue, "Game"));

            if (game != null)
            {
                Console.WriteLine("What is the task?");
                var task = Console.ReadLine();

                using (var db = new SyntaxErrorContext())
                {
                    SologameChallenge newChallenge = new SologameChallenge() { ChallengeTask = task };

                    if (isAddTrue) newChallenge.Game = game;
                    else newChallenge.GameID = game.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
                Console.WriteLine(DONE);
            } else Console.WriteLine("No game selected.");
        }

        /// <summary>Shows the challenges specified by a terminal input.</summary>
        private static void ShowChallenges()
        {
            using (var db = new SyntaxErrorContext())
            {
                Console.WriteLine("What kind of challenges would you like to see?\nAvailable challenges");

                var types = db.Challenges
                    .Select(x => x.GetDiscriminator())
                    .Distinct();

                foreach(var type in types)
                {
                    Console.WriteLine("\t-{0}", type);
                }
                Console.WriteLine("\t-All");
                
                var typeOfChallenge = Console.ReadLine().ToLower();

                switch (typeOfChallenge)
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
                    challenge.Game = (Game) db.Objects
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

        /// <summary>Shows the crew challenges.</summary>
        /// <param name="db">The database.</param>
        private static void ShowCrewChallenges(SyntaxErrorContext db)
        {
            Console.WriteLine();
            var challenges = GetChallengesOfType(db, "CrewChallenge");

            if (challenges.Length != 0)
            {
                foreach (CrewChallenge challenge in challenges)
                {
                    challenge.CrewMember = db.CrewMembers
                        .Where(cm => cm.CrewMemberID == challenge.CrewMemberID)
                        .Single();
                    challenge.Game = (Game) db.Objects
                        .Where(g => g.ID == challenge.GameID)
                        .Single();

                    Console.WriteLine("-----------------------------------------------\n" +
                        "Challenge ID: {0}\n" +
                        "Challenge task: {1}\n" +
                        "Challenge Game: {2}\n" +
                        "Challenge Crewmember: {3}\n" +
                        "-----------------------------------------------\n",
                        challenge.ChallengeID,
                        challenge.ChallengeTask,
                        challenge.Game.Name,
                        challenge.CrewMember.CrewTag);
                }
            } else Console.WriteLine("No challenges of type AudienceChallenge in database.");
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
                        "Challenge task: {1}",
                        challenge.ChallengeID,
                        challenge.ChallengeTask);

                    foreach (var answer in challenge.Answers.GetAll())
                        Console.WriteLine("\t-{0}" + (answer.Equals(challenge.Answers.Answer)?" (answer)":""), answer);

                    Console.WriteLine("-----------------------------------------------\n");
                }
            } else Console.WriteLine("No challenges of type MultipleChoiceChallenge in database.");
        }

        /// <summary>Shows the music challenges.</summary>
        /// <param name="db">The database.</param>
        private static void ShowMusicChallenges(SyntaxErrorContext db)
        {
            Console.WriteLine();
            var challenges = GetChallengesOfType(db, "MusicChallenge");

            if (challenges.Length != 0)
            {
                foreach(MusicChallenge challenge in challenges)
                {
                    challenge.Song = (Music) db.Objects
                        .Where(m => m.ID == challenge.SongID)
                        .Single();

                    Console.WriteLine("-----------------------------------------------\n" +
                        "Challenge ID: {0}\n" +
                        "Challenge task: {1}\n" +
                        "Challenge song: {2}\n" +
                        "-----------------------------------------------\n",
                        challenge.ChallengeID,
                        challenge.ChallengeTask,
                        challenge.Song.Name);
                }
            } else Console.WriteLine("No challenges of type MusicChallenge in database.");
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

        /// <summary>Shows the screenshot challenges.</summary>
        /// <param name="db">The database.</param>
        private static void ShowScreenshotChallenges(SyntaxErrorContext db)
        {
            Console.WriteLine();
            var challenges = GetChallengesOfType(db, "ScreenshotChallenge");

            if (challenges.Length != 0)
            {
                foreach (ScreenshotChallenge challenge in challenges)
                    {
                    challenge.Image = (Image) db.Objects
                        .Where(i => i.ID == challenge.ImageID)
                        .Single();

                    Console.WriteLine("-----------------------------------------------\n" +
                        "Challenge ID: {0}\n" + 
                        "Challenge task: {1}\n" +
                        "Image: {2}\n" +
                        "-----------------------------------------------\n",
                        challenge.ChallengeID,
                        challenge.ChallengeTask,
                        challenge.Image.Name);
                }
            } else Console.WriteLine("No challenges of type ScreenshotChallenge in database.");
        }

        /// <summary>Shows the silhouette challenges.</summary>
        /// <param name="db">The database.</param>
        private static void ShowSilhouetteChallenges(SyntaxErrorContext db)
        {
            Console.WriteLine();
            var challenges = GetChallengesOfType(db, "SilhouetteChallenge");

            if (challenges.Length != 0)
            {
                foreach (SilhouetteChallenge challenge in challenges)
                    {
                    challenge.Image = (Image) db.Objects
                        .Where(i => i.ID == challenge.ImageID)
                        .Single();

                    Console.WriteLine("-----------------------------------------------\n" +
                        "Challenge ID: {0}\n" + 
                        "Challenge task: {1}\n" +
                        "Silhouette: {2}\n" +
                        "-----------------------------------------------\n",
                        challenge.ChallengeID,
                        challenge.ChallengeTask,
                        challenge.Image.Name);
                }
            } else Console.WriteLine("No challenges of type SilhouetteChallenge in database.");
        }

        /// <summary>Shows the sologame challenges.</summary>
        /// <param name="db">The database.</param>
        private static void ShowSologameChallenges(SyntaxErrorContext db)
        {
            Console.WriteLine();
            var challenges = GetChallengesOfType(db, "SologameChallenge");

            if (challenges.Length != 0)
            {
                foreach (SologameChallenge challenge in challenges)
                {
                    challenge.Game = (Game) db.Objects
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
            } else Console.WriteLine("No challenges of type SologameChallenge in database.");
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
        
        /// <summary>Removes the specified challenge from the database.</summary>
        private static void RemoveChallenge()
        {
            Console.WriteLine("Which challenge would you like to remove?\n" +
                "(Use ID to identify the challenge)");
            ShowAllChallenges(new SyntaxErrorContext());

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                using (var db = new SyntaxErrorContext())
                {
                    try 
                    {
                        var challenge = db.Challenges
                            .Where(c => c.ChallengeID == choice)
                            .Single();

                        db.Challenges.Remove(challenge);
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        Console.WriteLine("Challenge in use.\n");
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("No such challenge.\n");
                    }
                }
                Console.WriteLine(DONE);
            } else Console.WriteLine("Invalid input");
        }

        /// <summary>Accepts input commands through the terminal and determines what to do.</summary>
        /// <param name="argument">The argument.</param>
        static void FindWhatToDo(string argument)
        {
            switch (argument.ToLower())
            {
                case "new game":
                    NewGameProfile();
                    break;
                case "show games":
                    ShowGameProfiles();
                    break;
                case "play":
                    Play();
                    break;
                case "remove game":
                    RemoveGameProfile();
                    break;
                case "show challenges":
                    ShowChallenges();
                    break;
                case "add challenge":
                    AddChallenge();
                    break;
                case "remove challenge":
                    RemoveChallenge();
                    break;
                case "show objects":
                    ShowObjects();
                    break;
                case "add object":
                    AddObject();
                    break;
                case "remove object":
                    RemoveObject();
                    break;
                case "show crew members":
                    ShowCrewMembers();
                    break;
                case "register crew member":
                    RegisterCrewMember();
                    break;
                case "remove crew member":
                    RemoveCrewMember();
                    break;
                case "help":
                    WriteHelpCommands();
                    break;
                case "initialize database":
                    InitializeDatabase();
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

        /// <summary>Initializes the database.</summary>
        private static void InitializeDatabase()
        {
            var db = new SyntaxErrorContext();
            if (db.Database.EnsureCreated())
            {
                Console.WriteLine("No database found. Applying migrations.");
                db.Database.Migrate();
            }
            else Console.WriteLine("The database allready exists. No migrations applied.");
            Console.WriteLine(DONE);
        }

        /// <summary>Shows the objects specified by a terminal input.</summary>
        private static void ShowObjects()
        {
            using (var db = new SyntaxErrorContext())
            {
                Console.WriteLine("What kind of objects would you like to see?\nAvailable object types");

                var types = db.Objects
                    .Select(x => x.GetType().Name)
                    .Distinct();

                foreach(var type in types)
                {
                    Console.WriteLine("\t-{0}", type);
                }
                Console.WriteLine("\t-All");
                
                var typeOfObject = Console.ReadLine().ToLower();
                MediaObject[] objects;

                Console.WriteLine();
                switch (typeOfObject)
                {
                    case "game":
                        objects = GetObjectsOfType(db, "Game");
                        break;
                    case "image":
                        objects = GetObjectsOfType(db, "Image");
                        break;
                    case "music":
                        objects = GetObjectsOfType(db, "Music");
                        break;
                    case "all":
                        objects = db.Objects.ToArray();
                        break;
                    default:
                        objects = null;
                        break;
                }

                if(objects != null)
                {
                    if (objects.Length != 0)
                    {
                        foreach (var item in objects)
                        {
                            Console.WriteLine("-----------------------------------------------\n" +
                                "Object ID: {0}\n" +
                                "Object name: {1}\n" +
                                "Object URI: {2}\n" +
                                "-----------------------------------------------\n",
                                item.ID,
                                item.Name,
                                item.URI);
                        }
                    } else Console.WriteLine("No challenges of type {0} in database.", typeOfObject);
                } else Console.WriteLine("Invalid input");
            }
        }

        /// <summary>Adds the object.</summary>
        private static void AddObject()
        {
            MediaObject outerSourceObject;
            Console.WriteLine("What kind of object do you want to add to the database?\nAvailable types:\n" +
                "\t-Game\n" +
                "\t-Image\n" +
                "\t-Music\n");
            string type = Console.ReadLine().ToLower();

            switch (type)
            {
                case "game":
                    outerSourceObject = new Game();
                    break;
                case "image":
                    outerSourceObject = new Image();
                    break;
                case "music":
                    outerSourceObject = new Music();
                    break;
                default:
                    outerSourceObject = null;
                    break;
            }

            if (outerSourceObject != null)
            {
                Console.WriteLine("What is the name of the {0}?", type);
                outerSourceObject.Name = Console.ReadLine();
                
                Console.WriteLine("What is the path to the {0} picture?", type);
                outerSourceObject.URI = Console.ReadLine();

                using (var db = new SyntaxErrorContext())
                {
                    db.Objects.Add(outerSourceObject);
                    db.SaveChanges();
                }
                Console.WriteLine(DONE);
            } else Console.WriteLine("Invalid input");
        }

        /// <summary>Removes the specified object from the database.</summary>
        private static void RemoveObject()
        {
            Console.WriteLine("Choose the object you want to delete\n" +
                "(Use the ID to identify the object)");
            using (var db = new SyntaxErrorContext())
            {
                var objects = db.Objects.ToArray();

                foreach (var item in objects)
                {
                    Console.WriteLine("-----------------------------------------------\n" +
                                "Object ID: {0}\n" +
                                "Object name: {1}\n" +
                                "Object URI: {2}\n" +
                                "-----------------------------------------------\n",
                                item.ID,
                                item.Name,
                                item.URI);
                }
            }

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                using (var db = new SyntaxErrorContext())
                {
                    try
                    {
                        var objectToDelete = db.Objects
                            .Where(o => o.ID == choice)
                            .Single();
                        db.Objects.Remove(objectToDelete);
                        db.SaveChanges();
                    } catch (DbUpdateException)
                    {
                        var assignedChallenge = "undefined";
                        var challenges = db.Challenges
                            .Where(c => !c.GetDiscriminator().Equals("MultipleChoiceChallenge") ||
                                !c.GetDiscriminator().Equals("QuizChallenge"))
                            .ToArray();

                        foreach (var challenge in challenges)
                        {
                            if (challenge.GetDiscriminator().Equals("AudienceChallenge"))
                            {
                                var tempChallenge = (AudienceChallenge) challenge;
                                if (tempChallenge.GameID == choice) assignedChallenge = tempChallenge.ChallengeID.ToString();
                            }
                            else if (challenge.GetDiscriminator().Equals("CrewChallenge"))
                            {
                                var tempChallenge = (CrewChallenge) challenge;
                                if (tempChallenge.GameID == choice) assignedChallenge = tempChallenge.ChallengeID.ToString();
                            }
                            else if (challenge.GetDiscriminator().Equals("MusicChallenge"))
                            {
                                var tempChallenge = (MusicChallenge) challenge;
                                if (tempChallenge.SongID == choice) assignedChallenge = tempChallenge.ChallengeID.ToString();
                            }
                            else if (challenge.GetDiscriminator().Equals("ScreenshotChallenge"))
                            {
                                var tempChallenge = (ScreenshotChallenge) challenge;
                                if (tempChallenge.ImageID == choice) assignedChallenge = tempChallenge.ChallengeID.ToString();
                            }
                            else if (challenge.GetDiscriminator().Equals("SilhouetteChallenge"))
                            {
                                var tempChallenge = (SilhouetteChallenge) challenge;
                                if (tempChallenge.ImageID == choice) assignedChallenge = tempChallenge.ChallengeID.ToString();
                            }
                            else
                            {
                                var tempChallenge = (SologameChallenge) challenge;
                                if (tempChallenge.GameID == choice) assignedChallenge = tempChallenge.ChallengeID.ToString();
                            }
                        }
                        Console.WriteLine("Object in use in challenge ID {0}.\n" +
                            "Delete the challenge that is using the object first before deleting the object.", assignedChallenge);
                    }
                }
                Console.WriteLine(DONE);
            } else Console.WriteLine("Invalid Input");
        }

        /// <summary>Shows the crew members.</summary>
        private static void ShowCrewMembers()
        {
            using (var db = new SyntaxErrorContext())
            {
                var crewMembers = db.CrewMembers.ToArray();

                foreach (var member in crewMembers)
                {
                    Console.WriteLine("-----------------------------------------------\n" +
                        "Crew member ID: {0}\n" +
                        "Crew member tag: {1}\n" +
                        "-----------------------------------------------\n",
                        member.CrewMemberID,
                        member.CrewTag);
                }
            }
        }

        /// <summary>Registers the crew member.</summary>
        private static void RegisterCrewMember()
        {
            Console.WriteLine("What is the CrewTag of the crew member?");
            CrewMember newCrewMember = new CrewMember(){ CrewTag = Console.ReadLine() };

            using (var db = new SyntaxErrorContext())
            {
                db.CrewMembers.Add(newCrewMember);
                db.SaveChanges();
            }
            Console.WriteLine(DONE);
        }

        /// <summary>Removes the crew member.</summary>
        private static void RemoveCrewMember()
        {
            Console.WriteLine("Choose the crew member you want to delete\n" +
                "(Use the ID to identify the crew member)");

            ShowCrewMembers();

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                using (var db = new SyntaxErrorContext())
                {
                    try
                    {
                        var crewMemberToDelete = db.CrewMembers
                            .Where(cm => cm.CrewMemberID == choice)
                            .Single();
                        db.CrewMembers.Remove(crewMemberToDelete);
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        
                        var assignedChallenge = "undefined";
                        var challenges = db.Challenges
                            .Where(c => c.GetDiscriminator().Equals("CrewChallenge"))
                            .ToArray();

                        foreach (CrewChallenge item in challenges)
                        {
                            if(item.CrewMemberID == choice)
                            {
                                assignedChallenge = item.ChallengeID.ToString();
                            }
                        }
                        Console.WriteLine("Crew member assigned to a challenge with ID {0}.\n" +
                            "Delete the challenge that the crew member is assigned to first before deleting the crew member.",
                            assignedChallenge);
                    }
                }
                Console.WriteLine(DONE);
            } else Console.WriteLine("Invalid Input");
        }

        /// <summary>Writes the help commands.</summary>
        private static void WriteHelpCommands()
        {
            Console.WriteLine("New game: Lets the user create a new game profile and choose which challenges they want in it\n" +
                "Show games: Lets the user see the game profiless stored in the database\n" +
                "Play: Lets the user simulate a play session with a specified game profile\n" +
                "Remove game: Lets the user delete a specified game profile\n" +
                "Show challenges: Lets the user see the challenges stored in the database\n" +
                "Add challenge: Lets the user add a challenge to the database\n" +
                "Remove challenge: Lets the user choose and remove a challenge from the database\n" +
                "Show objects: Lets the user see the objects stored in the database\n" +
                "Add object: Lets the user add an object to the database\n" +
                "Remove object: Lets the user choose and remove an object from the database\n" +
                "Show crew members: Lets the user see the crew members registered in the database\n" +
                "Register crew member: Lets the user register a crew member into the database\n" +
                "Remove crew member: Lets the user choose and remove a crew member from the database\n" +
                "Help: Lets the user see all available commands\n" +
                "Initialize database: Creates the database if the database does not exist in the first place\n" +
                "cls: Clears the terminal\n" +
                "exit: closes the terminal");
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
        private static MediaObject[] GetObjectsOfType(SyntaxErrorContext db, string type)
        {
            return db.Objects
                .Where(g => g.GetType().Name.Equals(type))
                .ToArray();
        }

        /// <summary>Gets the object needed for challenge.
        /// The method either creates a new object, or gets one from the database</summary>
        /// <param name="db">The database.</param>
        /// <param name="isAddTrue">The yes no.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static MediaObject GetObjectForChallenge(SyntaxErrorContext db, bool isAddTrue, string type)
        {
            MediaObject outerSourceObject;
            if (isAddTrue)
            {
                switch (type)
                {
                    case "Game":
                        outerSourceObject = new Game();
                        break;
                    case "Image":
                        outerSourceObject = new Image();
                        break;
                    case "Music":
                        outerSourceObject = new Music();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), "Unknown type");
                }
                Console.WriteLine("What is the name of the {0}?", type);
                outerSourceObject.Name = Console.ReadLine();
                Console.WriteLine("What is the path to the {0} picture?", type);
                outerSourceObject.URI = Console.ReadLine();
                return outerSourceObject;
            }
            else {
                outerSourceObject = new MediaObject();
                Console.WriteLine("Which {0} do you want to have in this challenge?\n" +
                    "(Use the ID to choose the game)", type);

                using (db)
                {
                    var objects = GetObjectsOfType(db, type);

                    if (objects.Length != 0)
                    {
                        foreach(var item in objects)
                        {
                            Console.WriteLine("-----------------------------------------------\n" +
                                " ID: {0}\n" +
                                " Name: {1}\n" +
                                "-----------------------------------------------\n",
                                    
                                item.ID,
                                item.Name
                                );
                        }
                        if (int.TryParse(Console.ReadLine(), out int choice))
                        {
                            try
                            {
                                outerSourceObject = db.Objects
                                    .Where(g => g.ID == choice)
                                    .Single();
                            } catch(InvalidOperationException)
                            {
                                return null;
                            }
                        } else { Console.WriteLine("Wrong input"); outerSourceObject = null; }
                    } else { Console.WriteLine("No {0}s registered in the database.", type); outerSourceObject = null; }
                }
                return outerSourceObject;
            }
        }

        /// <summary>Gets the crew member for challenge.
        /// The method either creates a new crew member, or gets one from the database</summary>
        /// <param name="db">The database.</param>
        /// <param name="yesNo">The yes no.</param>
        /// <returns></returns>
        private static CrewMember GetCrewMemberForChallenge(SyntaxErrorContext db, bool isAddTrue)
        {
            CrewMember crewMember;
            if(isAddTrue)
            {
                crewMember = new CrewMember();
                Console.WriteLine("What is the CrewTag of the crew member?");
                crewMember.CrewTag = Console.ReadLine();
                return crewMember;
            }
            else
            {
                crewMember = new CrewMember();
                Console.WriteLine("Which crew member do you want to have in this challenge?\n" +
                    "(Use the ID to choose the game)");
                using (db)
                {
                    var crewMembers = db.CrewMembers
                        .ToArray();

                    if (crewMembers.Length != 0)
                    {
                        foreach (var member in crewMembers)
                        {
                            Console.WriteLine("-----------------------------------------------\n" +
                                "CrewMember ID: {0}\n" +
                                "CrewTag: {1}\n" +
                                "-----------------------------------------------\n",
                                member.CrewMemberID,
                                member.CrewTag);
                        }
                        if (int.TryParse(Console.ReadLine(), out int choice))
                        {
                            try
                            {
                                crewMember = db.CrewMembers
                                    .Where(cm => cm.CrewMemberID == choice)
                                    .Single();
                            } catch(InvalidOperationException)
                            {
                                crewMember = null;
                            }
                        } else Console.WriteLine("Wrong input");
                    } else Console.WriteLine("No crew members registered in the database.");
                }
                return crewMember;
            }
        }
        
        /// <summary>Simulates the game.</summary>
        /// <param name="game">The game.</param>
        private static void SimulateGame(GameProfile game)
        {
            Console.WriteLine("What to do?\nAvailable options:\n" +
                "\t-next round\n" +
                "\t-end game");
            switch (Console.ReadLine())
            {
                case "end game":
                    return;
                case "next round":
                    if (game.Profile.Challenges.Count != game.SaveGame.Challenges.Count)
                    {
                        var rndChallenge = RollForRandomChallengeRecursive(game);

                        if(rndChallenge != null)
                        {
                            game.SaveGame.Challenges.Add(rndChallenge);

                            Console.WriteLine("\nAdded challenge {0} to the SaveGame\n",
                                rndChallenge.ChallengeID.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("No more challenges. Game ended.");
                        return;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
            SimulateGame(game);
        }
        /// <summary>Rolls for random challenge recursive.</summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        private static UsingChallenge RollForRandomChallengeRecursive(GameProfile game)
        {
            var randomNumberGen = new Random();
            int randomNumber = randomNumberGen.Next(0, game.Profile.Challenges.Count);

            if(!(randomNumber < 0))
            {
                var rndChallenge = new UsingChallenge()
                {
                   ChallengeID = game.Profile.Challenges.ElementAt(randomNumber).ChallengeID,
                   UsingID = game.SaveGameID
                };
                if (!IsDuplicateAbsent(game.SaveGame.Challenges.ToArray(), rndChallenge))
                {
                    Console.WriteLine("Reroling...");
                    return RollForRandomChallengeRecursive(game);
                }
                else return rndChallenge;
            } else return null;
        }
        /// <summary>Determines whether there is a duplicate in the specified challenges.</summary>
        /// <param name="challenges">The challenges.</param>
        /// <param name="dupeTestChallenge">The dupe test challenge.</param>
        /// <returns>
        ///   <c>true</c> if no duplicates; otherwise, <c>false</c>.</returns>
        private static bool IsDuplicateAbsent(UsingChallenge[] challenges, UsingChallenge dupeTestChallenge)
        {
            bool noDupes = true;
            foreach(var dbChallenge in challenges)
            {
                if (dbChallenge.ChallengeID == dupeTestChallenge.ChallengeID &&
                    dbChallenge.UsingID == dupeTestChallenge.UsingID) noDupes = !noDupes;
            }
            return noDupes;
        }
        
        /// <summary>Converts to game.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>Game</returns>
        private static Game ConvertToGame(MediaObject obj) { return obj as Game; }
        /// <summary>Converts to image.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>Image</returns>
        private static Image ConvertToImage(MediaObject obj) { return obj as Image; }
        /// <summary>Converts to music.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>Music</returns>
        private static Music ConvertToMusic(MediaObject obj) { return obj as Music; }

        /// <summary>Clears the current console line.</summary>
        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, currentLineCursor);
        }
        /// <summary>Clears two console lines.</summary>
        private static void ClearTwoLines()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            ClearCurrentConsoleLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            ClearCurrentConsoleLine();
        }
    }
}
