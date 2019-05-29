using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SyntaxError.V2.App.DataAccess;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;

namespace SyntaxError.V2.App.ViewModels
{
    public class MainViewModel : Observable
    {

        public ICommand DeleteGameProfileCommand { get; set; }

        public List<ChallengeBase> ChallengesFromDB = new List<ChallengeBase>();

        public ObservableCollection<ListItemMainPage> GameProfiles { get; set; } = new ObservableCollection<ListItemMainPage>();
        private GameProfiles gameProfilesDataAccess = new GameProfiles();
        
        public ObservableCollection<ListItemMainPage> AudienceChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> CrewChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> MultipleChoiceChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> MusicChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> QuizChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> ScreenshotChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> SilhouetteChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        public ObservableCollection<ListItemMainPage> SologameChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();

        public Challenges challengesDataAccess = new Challenges();

        public List<ObservableCollection<ListItemMainPage>> ChallengeListList;

        public MainViewModel()
        {
            DeleteGameProfileCommand = new RelayCommand<ListItemMainPage>(async param =>
                                                    {
                                                        if (await gameProfilesDataAccess.DeleteGameProfileAsync(param.GameProfile))
                                                            GameProfiles.Remove(param);
                                                    }, param => param != null);
        }

        /// <summary>Loads the game profiles from database asynchronous.</summary>
        /// <returns></returns>
        internal async Task LoadGameProfilesFromDBAsync()
        {
            if (GameProfiles.Count == 0)
            {
                var gameProfiles = await gameProfilesDataAccess.GetGameProfilesAsync();
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
            var challenges = await challengesDataAccess.GetChallengesAsync();
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
                var challenge = ChallengesFromDB.Find(c => c.ChallengeID == profileChallenge.ChallengeID);
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
