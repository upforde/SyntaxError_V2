﻿using SyntaxError.V2.DataAccess;
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

        private static void NewGame()
        {
            throw new NotImplementedException();
        }
        
        private static void Play()
        {
            throw new NotImplementedException();
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
            string yesNo = Console.ReadLine().ToLower();
            Game game = ConvertToGame(GetObjectForChallenge(new SyntaxErrorContext(), yesNo, "Game"));

            if (game != null)
            {
                Console.WriteLine("What is the task?");
                var task = Console.ReadLine();

                using (var db = new SyntaxErrorContext())
                {
                    AudienceChallenge newChallenge = new AudienceChallenge() { ChallengeTask = task };

                    if (yesNo.Equals("y")) newChallenge.Game = game;
                    else newChallenge.GameID = game.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
                Console.WriteLine("Done!");
            } else Console.WriteLine("No game selected.");
        }

        /// <summary>Adds the crew challenge.</summary>
        private static void AddCrewChallenge()
        {
            Console.WriteLine("Add a new crew member to the database? (y/n)");
            string yesNoCrew = Console.ReadLine().ToLower();
            CrewMember crewMember = GetCrewMemberForChallenge(new SyntaxErrorContext(), yesNoCrew);
            Console.WriteLine("Add a new game to the database? (y/n)");
            string yesNoGame = Console.ReadLine().ToLower();
            Game game = ConvertToGame(GetObjectForChallenge(new SyntaxErrorContext(), yesNoGame, "Game"));

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

                    if (yesNoCrew.Equals("y")) newChallenge.CrewMember = crewMember;
                    else newChallenge.CrewMemberID = crewMember.CrewMemberID;

                    if (yesNoGame.Equals("y")) newChallenge.Game = game;
                    else newChallenge.GameID = game.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
                Console.WriteLine("Done!");
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
            Console.WriteLine("Done!");
        }

        /// <summary>Adds the music challenge.</summary>
        private static void AddMusicChallenge()
        {
            Console.WriteLine("Add a new song to the database? (y/n)");
            string yesNo = Console.ReadLine().ToLower();
            Music music = ConvertToMusic(GetObjectForChallenge(new SyntaxErrorContext(), yesNo, "Music"));

            if (music != null)
            {
                using (var db = new SyntaxErrorContext())
                {
                    MusicChallenge newChallenge = new MusicChallenge() { ChallengeTask = "Guess the song" };

                    if (yesNo.Equals("y")) newChallenge.Song = music;
                    else newChallenge.SongID = music.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
                Console.WriteLine("Done!");
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
            Console.WriteLine("Done!");
        }

        /// <summary>Adds the screenshot challenge.</summary>
        private static void AddScreenshotChallenge()
        {
            Console.WriteLine("Add a new screenshot to the database? (y/n)");
            string yesNo = Console.ReadLine();
            Image image = ConvertToImage(GetObjectForChallenge(new SyntaxErrorContext(), yesNo, "Image"));

            if (image != null)
            {
                using (var db = new SyntaxErrorContext())
                {
                    ScreenshotChallenge newChallenge = new ScreenshotChallenge() { ChallengeTask = "What game is this screenshot from?" };

                    if(yesNo.Equals("y")) newChallenge.Image = image;
                    else newChallenge.ImageID = image.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
            Console.WriteLine("Done!");
            } else Console.WriteLine("No screenshots selected.");
        }

        /// <summary>Adds the silhouette challenge.</summary>
        private static void AddSilhouetteChallenge()
        {
            Console.WriteLine("Add a new silhouette to the database? (y/n)");
            string yesNo = Console.ReadLine();
            Image image = ConvertToImage(GetObjectForChallenge(new SyntaxErrorContext(), yesNo, "Image"));

            if (image != null)
            {
                using (var db = new SyntaxErrorContext())
                {
                    SilhouetteChallenge newChallenge = new SilhouetteChallenge() { ChallengeTask = "What character is this?" };

                    if(yesNo.Equals("y")) newChallenge.Image = image;
                    else newChallenge.ImageID = image.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
            Console.WriteLine("Done!");
            } else Console.WriteLine("No screenshots selected.");
        }

        /// <summary>Adds the sologame challenge.</summary>
        private static void AddSologameChallenge()
        {
            Console.WriteLine("Add a new game to database? (y/n)");
            string yesNo = Console.ReadLine().ToLower();
            Game game = ConvertToGame(GetObjectForChallenge(new SyntaxErrorContext(), yesNo, "Game"));

            if (game != null)
            {
                Console.WriteLine("What is the task?");
                var task = Console.ReadLine();

                using (var db = new SyntaxErrorContext())
                {
                    SologameChallenge newChallenge = new SologameChallenge() { ChallengeTask = task };

                    if (yesNo.Equals("y")) newChallenge.Game = game;
                    else newChallenge.GameID = game.ID;

                    db.Challenges.Add(newChallenge);
                    db.SaveChanges();
                }
                Console.WriteLine("Done!");
            } else Console.WriteLine("No game selected.");
        }

        /// <summary>Shows all of the challenges.</summary>
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
                        Console.WriteLine("\t-{0}" + ((answer.Equals(challenge.Answers.Answer))?" (answer)":""), answer);

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
                    var challenge = db.Challenges
                        .Where(c => c.ChallengeID == choice)
                        .Single();

                    db.Challenges.Remove(challenge);
                    db.SaveChanges();
                }
                Console.WriteLine("Done!");
            } else Console.WriteLine("Invalid input");
        }

        /// <summary>Accepts input commands through the terminal and determines what to do.</summary>
        /// <param name="argument">The argument.</param>
        static void FindWhatToDo(string argument)
        {
            switch (argument.ToLower())
            {
                case "new game":
                    NewGame();
                    break;
                case "play":
                    Play();
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

        private static void ShowObjects()
        {
            throw new NotImplementedException();
        }

        private static void AddObject()
        {
            throw new NotImplementedException();
        }

        private static void RemoveObject()
        {
            throw new NotImplementedException();
        }

        private static void ShowCrewMembers()
        {
            throw new NotImplementedException();
        }

        private static void RegisterCrewMember()
        {
            throw new NotImplementedException();
        }

        private static void RemoveCrewMember()
        {
            throw new NotImplementedException();
        }

        /// <summary>Writes the help commands.</summary>
        private static void WriteHelpCommands()
        {
            Console.WriteLine("Show challenges: Lets the user see the challenges stored in the database\n" +
                "Add challenge: Lets the user add a challenge to the database\n" +
                "Remove challenge: Lets the user choose and remove a challenge from the database\n" +
                "Help: Lets the user see all available commands\n" +
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
        private static OuterSourceObject[] GetObjectsOfType(SyntaxErrorContext db, string type)
        {
            return db.Objects
                .Where(g => g.GetType().Name.Equals(type))
                .ToArray();
        }

        /// <summary>Gets the object needed for challenge.
        /// The method either creates a new object, or gets one from the database</summary>
        /// <param name="db">The database.</param>
        /// <param name="yesNo">The yes no.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static OuterSourceObject GetObjectForChallenge(SyntaxErrorContext db, string yesNo, string type)
        {
            OuterSourceObject outerSourceObject;
            switch (yesNo)
            {
                case "y":
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
                            outerSourceObject = new OuterSourceObject();
                            break;
                    }
                    Console.WriteLine("What is the name of the {0}?", type);
                    outerSourceObject.Name = Console.ReadLine();
                    Console.WriteLine("What is the path to the {0} picture?", type);
                    outerSourceObject.URI = Console.ReadLine();
                    return outerSourceObject;
                case "n":
                    outerSourceObject = new OuterSourceObject();
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
                default:
                    Console.WriteLine("Wrong input");
                    return null;
            }
        }

        /// <summary>Gets the crew member for challenge.
        /// The method either creates a new crew member, or gets one from the database</summary>
        /// <param name="db">The database.</param>
        /// <param name="yesNo">The yes no.</param>
        /// <returns></returns>
        private static CrewMember GetCrewMemberForChallenge(SyntaxErrorContext db, string yesNo)
        {
            CrewMember crewMember;
            switch (yesNo)
            {
                case "y":
                    crewMember = new CrewMember();
                    Console.WriteLine("What is the CrewTag of the crew member?");
                    crewMember.CrewTag = Console.ReadLine();
                    return crewMember;
                case "n":
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
                default:
                    Console.WriteLine("Wrong input");
                    return null;
            }
        }

        /// <summary>Converts to game.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>Game</returns>
        private static Game ConvertToGame(OuterSourceObject obj) { return obj as Game; }
        /// <summary>Converts to image.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>Image</returns>
        private static Image ConvertToImage(OuterSourceObject obj) { return obj as Image; }
        /// <summary>Converts to music.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>Music</returns>
        private static Music ConvertToMusic(OuterSourceObject obj) { return obj as Music; }
    }
}
