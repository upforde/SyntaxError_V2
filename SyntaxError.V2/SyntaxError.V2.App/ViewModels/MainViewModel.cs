using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SyntaxError.V2.App.DataAccess;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.Views;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using Windows.UI.Xaml.Controls;

namespace SyntaxError.V2.App.ViewModels
{
    public class MainViewModel : Observable
    {
        public ICommand AddGameProfileCommand { get; set; }
        public ICommand DeleteGameProfileCommand { get; set; }

        public ObservableCollection<ChallengeBase> ChallengesFromDB { get; set; } = new ObservableCollection<ChallengeBase>();

        public ObservableCollection<ListItemMainPage> GameProfiles { get; set; } = new ObservableCollection<ListItemMainPage>();
        public GameProfiles GameProfilesDataAccess { get; set; } = new GameProfiles();
        public Challenges ChallengesDataAccess { get; set; } = new Challenges();
        public MediaObjects MediaObjectsDataAccess { get; set; } = new MediaObjects();
        public DataAccess.Answers AnswersDataAccess { get; set; } = new DataAccess.Answers();
        
        public ObservableCollection<ListItemMainPage> AudienceChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> CrewChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> MultipleChoiceChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> MusicChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> QuizChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> ScreenshotChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> SilhouetteChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> SologameChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();


        public List<ObservableCollection<ListItemMainPage>> ChallengeListList { get; set; }

        public MainViewModel()
        {
            AddGameProfileCommand = new RelayCommand<GameProfile>(async param =>
                                                    {
                                                        param = await GameProfilesDataAccess.CreateNewGame(param);
                                                        GameProfiles.Add(new ListItemMainPage{ GameProfile = param });
                                                    }, param => param != null);

            DeleteGameProfileCommand = new RelayCommand<ListItemMainPage>(async param =>
                                                    {
                                                        if (await GameProfilesDataAccess.DeleteGameProfileAsync(param.GameProfile))
                                                            GameProfiles.Remove(param);
                                                    }, param => param != null);
        }

        internal async Task LoadMediaObjectsFromDBAsync()
        {
            foreach (var challenge in ChallengesFromDB)
                switch (challenge.GetDiscriminator())
                {
                    case "AudienceChallenge":
                    case "CrewChallenge":
                    case "SologameChallenge":
                        if((challenge as GameChallenge).GameID != 0 && (challenge as GameChallenge).Game == null)
                            try{
                                (challenge as GameChallenge).Game = await MediaObjectsDataAccess.GetMediaObjectAsync((challenge as GameChallenge).GameID, "Game") as Game;
                            }
                            catch (Exception e) { }
                        break;
                    case "MultipleChoiceChallenge":
                    case "QuizChallenge":
                        if((challenge as QuestionChallenge).AnswersID != 0 && (challenge as QuestionChallenge).Answers == null)
                            try {
                                (challenge as QuestionChallenge).Answers = await AnswersDataAccess.GetAnswersAsync((challenge as QuestionChallenge).AnswersID);
                            } catch (Exception e) { }
                        break;
                    case "MusicChallenge":
                        if((challenge as MusicChallenge).SongID != 0 && (challenge as MusicChallenge).Song == null)
                            try {
                                (challenge as MusicChallenge).Song = await MediaObjectsDataAccess.GetMediaObjectAsync((challenge as MusicChallenge).SongID, "Music") as Music;
                            } catch (Exception e){ }
                        break;
                    case "ScreenshotChallenge":
                    case "SilhouetteChallenge":
                        if((challenge as ImageChallenge).ImageID != 0 && (challenge as ImageChallenge).Image == null)
                            try
                            {
                                (challenge as ImageChallenge).Image = await MediaObjectsDataAccess.GetMediaObjectAsync((challenge as ImageChallenge).ImageID, "Image") as Modell.ChallengeObjects.Image;
                            } catch (Exception e) { }
                        break;
                }
        }

        /// <summary>Loads the game profiles from database asynchronous.</summary>
        /// <returns></returns>
        internal async Task LoadGameProfilesFromDBAsync()
        {
            if (GameProfiles.Count == 0)
            {
                var gameProfiles = await GameProfilesDataAccess.GetGameProfilesAsync();
                foreach(GameProfile gameProfile in gameProfiles)
                {
                    GameProfiles.Add(new ListItemMainPage
                    {
                        GameProfile = gameProfile,
                        DeleteCommandGameProfile = DeleteGameProfileCommand
                    });
                }
                ChallengeListList = new List<ObservableCollection<ListItemMainPage>>()
                {
                    AudienceChallenges,
                    CrewChallenges,
                    MultipleChoiceChallenges,
                    MusicChallenges,
                    QuizChallenges,
                    ScreenshotChallenges,
                    SilhouetteChallenges,
                    SologameChallenges
                };
            }
        }

        /// <summary>Loads the challenges from database asynchronous.</summary>
        /// <returns></returns>
        internal async Task LoadChallengesFromDBAsync()
        {
            var challenges = await ChallengesDataAccess.GetChallengesAsync();
            foreach (ChallengeBase challenge in challenges) ChallengesFromDB.Add(challenge);
        }

        /// <summary>Puts the challenges in their respective lists.</summary>
        /// <param name="profile">The profile.</param>
        internal void PutChallengesInLists(GameProfile profile)
        {
            foreach (var list in ChallengeListList)
                list.Clear();
            
            List<int?> saveGameChallengeIDs = new List<int?>();
            foreach (var saveGame in profile.SaveGame.Challenges) saveGameChallengeIDs.Add(saveGame.ChallengeID);
            
            foreach (var profileChallenge in profile.Profile.Challenges)
            {
                var challenge = ChallengesFromDB.Where(c => c.ChallengeID == profileChallenge.ChallengeID).First();
                ListItemMainPage listChallenge = new ListItemMainPage
                {
                    IsChallengeCompleted = (saveGameChallengeIDs.Contains(profileChallenge.ChallengeID)?true:false)
                };

                switch (challenge.GetDiscriminator())
                {
                    case "AudienceChallenge":
                        listChallenge.AudienceChallenge = (AudienceChallenge) challenge;
                        AudienceChallenges.Add(listChallenge);
                        break;
                    case "CrewChallenge":
                        listChallenge.CrewChallenge = (CrewChallenge) challenge;
                        CrewChallenges.Add(listChallenge);
                        break;
                    case "MultipleChoiceChallenge":
                        listChallenge.MultipleChoiceChallenge = (MultipleChoiceChallenge) challenge;
                        MultipleChoiceChallenges.Add(listChallenge);
                        break;
                    case "MusicChallenge":
                        listChallenge.MusicChallenge = (MusicChallenge) challenge;
                        MusicChallenges.Add(listChallenge);
                        break;
                    case "QuizChallenge":
                        listChallenge.QuizChallenge = (QuizChallenge) challenge;
                        QuizChallenges.Add(listChallenge);
                        break;
                    case "ScreenshotChallenge":
                        listChallenge.ScreenshotChallenge = (ScreenshotChallenge) challenge;
                        ScreenshotChallenges.Add(listChallenge);
                        break;
                    case "SilhouetteChallenge":
                        listChallenge.SilhouetteChallenge = (SilhouetteChallenge) challenge;
                        SilhouetteChallenges.Add(listChallenge);
                        break;
                    case "SologameChallenge":
                        listChallenge.SologameChallenge = (SologameChallenge) challenge;
                        SologameChallenges.Add(listChallenge);
                        break;
                }
            }
        }
    }
}
