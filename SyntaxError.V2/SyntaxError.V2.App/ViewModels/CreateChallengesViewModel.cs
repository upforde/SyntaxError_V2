using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SyntaxError.V2.App.DataAccess;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;

namespace SyntaxError.V2.App.ViewModels
{
    public class CreateChallengesViewModel : Observable
    {
        /// <summary>A command to edit a Challenge in the database and list.</summary>
        /// <value>The edit command.</value>
        public ICommand EditCommand { get; set; }
        /// <summary>A command to delete a new Challenge from the database and list.</summary>
        /// <value>The delete command.</value>
        public ICommand DeleteCommand { get; set; }

        public ObservableCollection<AudienceChallenge> AudienceChallenges { get; set; } = new ObservableCollection<AudienceChallenge>();
        public ObservableCollection<CrewChallenge> CrewChallenges { get; set; } = new ObservableCollection<CrewChallenge>();
        public ObservableCollection<MultipleChoiceChallenge> MultipleChoiceChallenges { get; set; } = new ObservableCollection<MultipleChoiceChallenge>();
        public ObservableCollection<MusicChallenge> MusicChallenges { get; set; } = new ObservableCollection<MusicChallenge>();
        public ObservableCollection<QuizChallenge> QuizChallenges { get; set; } = new ObservableCollection<QuizChallenge>();
        public ObservableCollection<ScreenshotChallenge> ScreenshotChallenges { get; set; } = new ObservableCollection<ScreenshotChallenge>();
        public ObservableCollection<SilhouetteChallenge> SilhouetteChallenges { get; set; } = new ObservableCollection<SilhouetteChallenge>();
        public ObservableCollection<SologameChallenge> SologameChallenges { get; set; } = new ObservableCollection<SologameChallenge>();

        public Challenges ChallengesDataAccess = new Challenges();
        
        public List<ChallengeBase> ChallengesFromDB = new List<ChallengeBase>();

        public CreateObjectsViewModel ObjectsViewModel = new CreateObjectsViewModel();

        public ObservableCollection<Game> Games { get; set; } = new ObservableCollection<Game>();
        public ObservableCollection<Image> Images { get; set; } = new ObservableCollection<Image>();
        public ObservableCollection<Music> Music { get; set; } = new ObservableCollection<Music>();

        public CreateChallengesViewModel()
        {
            EditCommand = new RelayCommand<ChallengeBase>(async param =>
                                                    {
                                                        switch (param.GetDiscriminator())
                                                        {
                                                            case "AudienceChallenge":
                                                                if ((param as AudienceChallenge).GameID == null)
                                                                {
                                                                    (param as AudienceChallenge).GameID = (await ObjectsViewModel.MediaObjectsDataAccess.CreateMediaObjectAsync(new Game{ Name = "New Game" })).ID;
                                                                    if(int.TryParse((param as AudienceChallenge).GameID.ToString(), out int result)) (param as AudienceChallenge).Game.ID = result;
                                                                }
                                                                ObjectsViewModel.EditCommand.Execute((param as AudienceChallenge).Game);
                                                                break;
                                                            case "CrewChallenge":
                                                                if ((param as CrewChallenge).Game.ID == 0)
                                                                {
                                                                    (param as CrewChallenge).GameID = (await ObjectsViewModel.MediaObjectsDataAccess.CreateMediaObjectAsync(new Game{ Name = "New Game" })).ID;
                                                                    if(int.TryParse((param as CrewChallenge).GameID.ToString(), out int result)) (param as CrewChallenge).Game.ID = result;
                                                                }
                                                                ObjectsViewModel.EditCommand.Execute((param as CrewChallenge).Game);
                                                                break;
                                                            case "MultipleChoiceChallenge":
                                                                break;
                                                            case "MusicChallenge":
                                                                break;
                                                            case "QuizChallenge":
                                                                break;
                                                            case "ScreenshotChallenge":
                                                                break;
                                                            case "SilhouetteChallenge":
                                                                break;
                                                            case "SologameChallenge":
                                                                if ((param as SologameChallenge).Game.ID == 0)
                                                                {
                                                                    (param as SologameChallenge).GameID = (await ObjectsViewModel.MediaObjectsDataAccess.CreateMediaObjectAsync(new Game{ Name = "New Game" })).ID;
                                                                    if(int.TryParse((param as SologameChallenge).GameID.ToString(), out int result)) (param as SologameChallenge).Game.ID = result;
                                                                }
                                                                ObjectsViewModel.EditCommand.Execute((param as SologameChallenge).Game);
                                                                break;
                                                        }
                                                        await ChallengesDataAccess.EditChallengeAsync(param);
                                                    }, param => param != null);

            DeleteCommand = new RelayCommand<ChallengeBase>(async param =>
                                                    {
                                                        if (await ChallengesDataAccess.DeleteChallengeAsync(param))
                                                            switch (param.GetDiscriminator())
                                                            {
                                                                case "AudienceChallenge":
                                                                    AudienceChallenges.Remove(param as AudienceChallenge);
                                                                    break;
                                                                case "CrewChallenge":
                                                                    CrewChallenges.Remove(param as CrewChallenge);
                                                                    break;
                                                                case "MultipleChoiceChallenge":
                                                                    MultipleChoiceChallenges.Remove(param as MultipleChoiceChallenge);
                                                                    break;
                                                                case "MusicChallenge":
                                                                    MusicChallenges.Remove(param as MusicChallenge);
                                                                    break;
                                                                case "QuizChallenge":
                                                                    QuizChallenges.Remove(param as QuizChallenge);
                                                                    break;
                                                                case "ScreenshotChallenge":
                                                                    ScreenshotChallenges.Remove(param as ScreenshotChallenge);
                                                                    break;
                                                                case "SilhouetteChallenge":
                                                                    SilhouetteChallenges.Remove(param as SilhouetteChallenge);
                                                                    break;
                                                                case "SologameChallenge":
                                                                    SologameChallenges.Remove(param as SologameChallenge);
                                                                    break;
                                                            }
                                                    }, param => param != null);
        }

        /// <summary>Loads the challenges from database asynchronous.</summary>
        /// <returns></returns>
        internal async Task LoadChallengesFromDBAsync()
        {
            var challenges = await ChallengesDataAccess.GetChallengesAsync();
            
            await ObjectsViewModel.LoadObjectsFromDBAsync();
            foreach (Game game in ObjectsViewModel.Games) Games.Add(game);
            foreach (Image image in ObjectsViewModel.Images) Images.Add(image);
            foreach (Music music in ObjectsViewModel.Music) Music.Add(music);

            foreach (ChallengeBase challenge in challenges)
            {
                switch (challenge.GetDiscriminator())
                {
                    case "AudienceChallenge":
                        foreach (Game game in Games)
                            if (game.ID.Equals((challenge as AudienceChallenge).GameID)) {
                                (challenge as AudienceChallenge).Game = game;
                                break;
                            }
                        AudienceChallenges.Add(challenge as AudienceChallenge);
                        break;
                    case "CrewChallenge":
                        foreach (Game game in Games)
                            if (game.ID.Equals((challenge as CrewChallenge).GameID)) {
                                (challenge as CrewChallenge).Game = game;
                                break;
                            }
                        CrewChallenges.Add(challenge as CrewChallenge);
                        break;
                    case "MultipleChoiceChallenge":
                        MultipleChoiceChallenges.Add(challenge as MultipleChoiceChallenge);
                        break;
                    case "MusicChallenge":
                        foreach (Music music in Music)
                            if (music.ID.Equals((challenge as MusicChallenge).SongID)){
                                (challenge as MusicChallenge).Song = music;
                                break;
                            }
                        MusicChallenges.Add(challenge as MusicChallenge);
                        break;
                    case "QuizChallenge":
                        QuizChallenges.Add(challenge as QuizChallenge);
                        break;
                    case "ScreenshotChallenge":
                        foreach (Image image in Images)
                            if (image.ID.Equals((challenge as ScreenshotChallenge).ImageID)){
                                (challenge as ScreenshotChallenge).Image = image;
                                break;
                            }
                        ScreenshotChallenges.Add(challenge as ScreenshotChallenge);
                        break;
                    case "SilhouetteChallenge":
                        foreach (Image image in Images)
                            if (image.ID.Equals((challenge as SilhouetteChallenge).ImageID)){
                                (challenge as SilhouetteChallenge).Image = image;
                                break;
                            }
                        SilhouetteChallenges.Add(challenge as SilhouetteChallenge);
                        break;
                    case "SologameChallenge":
                        foreach (Game game in Games)
                            if (game.ID.Equals((challenge as SologameChallenge).GameID)){
                                (challenge as SologameChallenge).Game = game;
                                break;
                            }
                        SologameChallenges.Add(challenge as SologameChallenge);
                        break;
                }
            }

            CreateNewObjectPlaceholder();
        }

        internal void CreateNewObjectPlaceholder()
        {
            if (!Games.Contains(new Game())) Games.Add(new Game{ Name="<Add a new game>" });
            if (!Images.Contains(new Image())) Images.Add(new Image{ Name="<Add a new image>" });
            if (!Music.Contains(new Music())) Music.Add(new Music{ Name="<Add a new song>" });
        }
    }
}
