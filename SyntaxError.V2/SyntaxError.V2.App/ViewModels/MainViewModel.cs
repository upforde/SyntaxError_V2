using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SyntaxError.V2.App.DataAccess;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;

namespace SyntaxError.V2.App.ViewModels
{
    public class MainViewModel : Observable
    {
        public ICommand AddGameProfileCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand RefreshSaveGame { get; set; }
        public ICommand DeleteGameProfileCommand { get; set; }

        public event PropertyChangedEventHandler RefreshSaveDone;

        public GameProfiles GameProfilesDataAccess { get; set; } = new GameProfiles();
        public Challenges ChallengesDataAccess { get; set; } = new Challenges();
        public MediaObjects MediaObjectsDataAccess { get; set; } = new MediaObjects();
        public DataAccess.Answers AnswersDataAccess { get; set; } = new DataAccess.Answers();

        public CreateChallengesViewModel ChallengesViewModel = new CreateChallengesViewModel();
        
        public ObservableCollection<ListItemMainPage> GameProfiles { get; set; } = new ObservableCollection<ListItemMainPage>();
        public List<ChallengeBase> ChallengesFromDB { get; set; } = new List<ChallengeBase>();
        public ObservableCollection<ListItemMainPage> GameProfileChallenges { get; set; } = new ObservableCollection<ListItemMainPage>();
        
        public ObservableCollection<AudienceChallenge> AudienceChallenges { get; set; } = new ObservableCollection<AudienceChallenge>();
        public ObservableCollection<CrewChallenge> CrewChallenges { get; set; } = new ObservableCollection<CrewChallenge>();
        public ObservableCollection<MultipleChoiceChallenge> MultipleChoiceChallenges { get; set; } = new ObservableCollection<MultipleChoiceChallenge>();
        public ObservableCollection<MusicChallenge> MusicChallenges { get; set; } = new ObservableCollection<MusicChallenge>();
        public ObservableCollection<QuizChallenge> QuizChallenges { get; set; }  = new ObservableCollection<QuizChallenge>();
        public ObservableCollection<ScreenshotChallenge> ScreenshotChallenges { get; set; } = new ObservableCollection<ScreenshotChallenge>();
        public ObservableCollection<SilhouetteChallenge> SilhouetteChallenges { get; set; } = new ObservableCollection<SilhouetteChallenge>();
        public ObservableCollection<SologameChallenge> SologameChallenges { get; set; } = new ObservableCollection<SologameChallenge>();

        public ObservableCollection<AudienceChallenge> NewAudienceChallenges { get; set; } = new ObservableCollection<AudienceChallenge>();
        public ObservableCollection<CrewChallenge> NewCrewChallenges { get; set; } = new ObservableCollection<CrewChallenge>();
        public ObservableCollection<MultipleChoiceChallenge> NewMultipleChoiceChallenges { get; set; } = new ObservableCollection<MultipleChoiceChallenge>();
        public ObservableCollection<MusicChallenge> NewMusicChallenges { get; set; } = new ObservableCollection<MusicChallenge>();
        public ObservableCollection<QuizChallenge> NewQuizChallenges { get; set; } = new ObservableCollection<QuizChallenge>();
        public ObservableCollection<ScreenshotChallenge> NewScreenshotChallenges { get; set; } = new ObservableCollection<ScreenshotChallenge>();
        public ObservableCollection<SilhouetteChallenge> NewSilhouetteChallenges { get; set; } = new ObservableCollection<SilhouetteChallenge>();
        public ObservableCollection<SologameChallenge> NewSologameChallenges { get; set; } = new ObservableCollection<SologameChallenge>();

        public MainViewModel()
        {
            AddGameProfileCommand = new RelayCommand<GameProfile>(async param =>
                                                    {
                                                        param = await GameProfilesDataAccess.CreateNewGame(param);
                                                        GameProfiles.Add(new ListItemMainPage{ GameProfile = param });
                                                    }, param => param != null);

            EditCommand = new RelayCommand<GameProfile>(async param =>
                                                    {
                                                        await GameProfilesDataAccess.EditGameProfileAsync(param);
                                                    }, param => param != null);

            RefreshSaveGame = new RelayCommand<ListItemMainPage>(async param =>
                                                    {
                                                        var success = await GameProfilesDataAccess.RefreshSaveGameAsync(param.GameProfile.ID);
                                                        if (success) param.GameProfile.SaveGame.Challenges.Clear();
                                                        RefreshSaveDone.Invoke(this, new PropertyChangedEventArgs("RefreshSaveDone"));
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
                        DeleteCommandGameProfile = DeleteGameProfileCommand
                    });
                }
            }
        }

        /// <summary>Loads the challenges from database asynchronous.</summary>
        /// <returns></returns>
        internal async Task LoadChallengesFromDBAsync()
        {
            if (ChallengesFromDB.Count == 0) await ChallengesViewModel.LoadChallengesFromDBAsync();
            ChallengesFromDB.AddRange(CreateChallengesViewModel.AudienceChallenges);
            ChallengesFromDB.AddRange(CreateChallengesViewModel.CrewChallenges);
            ChallengesFromDB.AddRange(CreateChallengesViewModel.MultipleChoiceChallenges);
            ChallengesFromDB.AddRange(CreateChallengesViewModel.MusicChallenges);
            ChallengesFromDB.AddRange(CreateChallengesViewModel.QuizChallenges);
            ChallengesFromDB.AddRange(CreateChallengesViewModel.ScreenshotChallenges);
            ChallengesFromDB.AddRange(CreateChallengesViewModel.SilhouetteChallenges);
            ChallengesFromDB.AddRange(CreateChallengesViewModel.SologameChallenges);
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
            }
        }
    }
}
