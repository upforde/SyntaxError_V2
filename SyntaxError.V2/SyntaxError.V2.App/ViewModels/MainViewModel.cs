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
        public ICommand EdigGameProfileCommand { get; set; }

        public GameProfiles GameProfilesDataAccess { get; set; } = new GameProfiles();
        public Challenges ChallengesDataAccess { get; set; } = new Challenges();
        public MediaObjects MediaObjectsDataAccess { get; set; } = new MediaObjects();
        public DataAccess.Answers AnswersDataAccess { get; set; } = new DataAccess.Answers();
        
        public ObservableCollection<ListItemMainPage> GameProfiles { get; set; } = new ObservableCollection<ListItemMainPage>();
        
        public ObservableCollection<ChallengeBase> ChallengesFromDB { get; set; } = new ObservableCollection<ChallengeBase>();

        public ObservableCollection<ListItemMainPage> GameProfileChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();

        /*
        public ObservableCollection<AudienceChallenge> AudienceChallenges { get; set; } = new ObservableCollection<AudienceChallenge>();
        public ObservableCollection<CrewChallenge> CrewChallenges { get; set; } = new ObservableCollection<CrewChallenge>();
        public ObservableCollection<MultipleChoiceChallenge> MultipleChoiceChallenges { get; set; } = new ObservableCollection<MultipleChoiceChallenge>();
        public ObservableCollection<MusicChallenge> MusicChallenges { get; set; } = new ObservableCollection<MusicChallenge>();
        public ObservableCollection<QuizChallenge> QuizChallenges { get; set; } = new ObservableCollection<QuizChallenge>();
        public ObservableCollection<ScreenshotChallenge> ScreenshotChallenges { get; set; } = new ObservableCollection<ScreenshotChallenge>();
        public ObservableCollection<SilhouetteChallenge> SilhouetteChallenges { get; set; } = new ObservableCollection<SilhouetteChallenge>();
        public ObservableCollection<SologameChallenge> SologameChallenges { get; set; } = new ObservableCollection<SologameChallenge>();
        */

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
                        DeleteCommandGameProfile = DeleteGameProfileCommand,
                        EditCommandGameProfile = EdigGameProfileCommand
                    });
                }
            }
        }

        /// <summary>Loads the challenges from database asynchronous.</summary>
        /// <returns></returns>
        internal async Task LoadChallengesFromDBAsync()
        {
            var challenges = await ChallengesDataAccess.GetChallengesAsync();
            foreach (ChallengeBase challenge in challenges) ChallengesFromDB.Add(challenge);
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
                            catch (Exception) { }
                        break;
                    case "MultipleChoiceChallenge":
                    case "QuizChallenge":
                        if((challenge as QuestionChallenge).AnswersID != 0 && (challenge as QuestionChallenge).Answers == null)
                            try {
                                (challenge as QuestionChallenge).Answers = await AnswersDataAccess.GetAnswersAsync((challenge as QuestionChallenge).AnswersID);
                            } catch (Exception) { }
                        break;
                    case "MusicChallenge":
                        if((challenge as MusicChallenge).SongID != 0 && (challenge as MusicChallenge).Song == null)
                            try {
                                (challenge as MusicChallenge).Song = await MediaObjectsDataAccess.GetMediaObjectAsync((challenge as MusicChallenge).SongID, "Music") as Music;
                            } catch (Exception){ }
                        break;
                    case "ScreenshotChallenge":
                    case "SilhouetteChallenge":
                        if((challenge as ImageChallenge).ImageID != 0 && (challenge as ImageChallenge).Image == null)
                            try
                            {
                                (challenge as ImageChallenge).Image = await MediaObjectsDataAccess.GetMediaObjectAsync((challenge as ImageChallenge).ImageID, "Image") as Modell.ChallengeObjects.Image;
                            } catch (Exception) { }
                        break;
                }
        }

        /// <summary>Puts the challenges in their respective lists.</summary>
        /// <param name="profile">The profile.</param>
        internal void PutChallengesInLists(GameProfile profile)
        {
            //ClearLists();
            GameProfileChallenges.Clear();
            
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
                    case"AudienceChallenge":
                        listChallenge.Challenge = challenge as AudienceChallenge;
                        listChallenge.IsGameChallenge = Windows.UI.Xaml.Visibility.Visible;
                        GameProfileChallenges.Add(listChallenge);
                        break;
                    case"CrewChallenge":
                        listChallenge.Challenge = challenge as CrewChallenge;
                        listChallenge.IsGameChallenge = Windows.UI.Xaml.Visibility.Visible;
                        GameProfileChallenges.Add(listChallenge);
                        break;
                    case"MultipleChoiceChallenge":
                        listChallenge.Challenge = challenge as MultipleChoiceChallenge;
                        listChallenge.IsQuestionChallenge = Windows.UI.Xaml.Visibility.Visible;
                        GameProfileChallenges.Add(listChallenge);
                        break;
                    case"MusicChallenge":
                        listChallenge.Challenge = challenge as MusicChallenge;
                        listChallenge.IsMusicChallenge = Windows.UI.Xaml.Visibility.Visible;
                        GameProfileChallenges.Add(listChallenge);
                        break;
                    case"QuizChallenge":
                        listChallenge.Challenge = challenge as QuizChallenge;
                        listChallenge.IsQuestionChallenge = Windows.UI.Xaml.Visibility.Visible;
                        GameProfileChallenges.Add(listChallenge);
                        break;
                    case"ScreenshotChallenge":
                        listChallenge.Challenge = challenge as ScreenshotChallenge;
                        listChallenge.IsImageChallenge = Windows.UI.Xaml.Visibility.Visible;
                        GameProfileChallenges.Add(listChallenge);
                        break;
                    case"SilhouetteChallenge":
                        listChallenge.Challenge = challenge as SilhouetteChallenge;
                        listChallenge.IsImageChallenge = Windows.UI.Xaml.Visibility.Visible;
                        GameProfileChallenges.Add(listChallenge);
                        break;
                    case"SologameChallenge":
                        listChallenge.Challenge = challenge as SologameChallenge;
                        listChallenge.IsGameChallenge = Windows.UI.Xaml.Visibility.Visible;
                        GameProfileChallenges.Add(listChallenge);
                        break;
                }

                /*switch (challenge.GetDiscriminator())
                {
                    case "AudienceChallenge":
                        listChallenge.AudienceChallenge = (AudienceChallenge) challenge;
                        AudienceChallenges.Add(challenge as AudienceChallenge);
                        break;
                    case "CrewChallenge":
                        listChallenge.CrewChallenge = (CrewChallenge) challenge;
                        CrewChallenges.Add(challenge as CrewChallenge);
                        break;
                    case "MultipleChoiceChallenge":
                        listChallenge.MultipleChoiceChallenge = (MultipleChoiceChallenge) challenge;
                        MultipleChoiceChallenges.Add(challenge as MultipleChoiceChallenge);
                        break;
                    case "MusicChallenge":
                        listChallenge.MusicChallenge = (MusicChallenge) challenge;
                        MusicChallenges.Add(challenge as MusicChallenge);
                        break;
                    case "QuizChallenge":
                        listChallenge.QuizChallenge = (QuizChallenge) challenge;
                        QuizChallenges.Add(challenge as QuizChallenge);
                        break;
                    case "ScreenshotChallenge":
                        listChallenge.ScreenshotChallenge = (ScreenshotChallenge) challenge;
                        ScreenshotChallenges.Add(challenge as ScreenshotChallenge);
                        break;
                    case "SilhouetteChallenge":
                        listChallenge.SilhouetteChallenge = (SilhouetteChallenge) challenge;
                        SilhouetteChallenges.Add(challenge as SilhouetteChallenge);
                        break;
                    case "SologameChallenge":
                        listChallenge.SologameChallenge = (SologameChallenge) challenge;
                        SologameChallenges.Add(challenge as SologameChallenge);
                        break;
                }*/
            }
        }

        /*private void ClearLists()
        {
            AudienceChallenges.Clear();
            CrewChallenges.Clear();
            MultipleChoiceChallenges.Clear();
            MusicChallenges.Clear();
            QuizChallenges.Clear();
            ScreenshotChallenges.Clear();
            SilhouetteChallenges.Clear();
            SologameChallenges.Clear();
        }*/
    }
}
