using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public static List<AudienceChallenge> AudienceChallenges { get; set; } = new List<AudienceChallenge>();
        public static List<CrewChallenge> CrewChallenges { get; set; } = new List<CrewChallenge>();
        public static List<MultipleChoiceChallenge> MultipleChoiceChallenges { get; set; } = new List<MultipleChoiceChallenge>();
        public static List<MusicChallenge> MusicChallenges { get; set; } = new List<MusicChallenge>();
        public static List<QuizChallenge> QuizChallenges { get; set; } = new List<QuizChallenge>();
        public static List<ScreenshotChallenge> ScreenshotChallenges { get; set; } = new List<ScreenshotChallenge>();
        public static List<SilhouetteChallenge> SilhouetteChallenges { get; set; } = new List<SilhouetteChallenge>();
        public static List<SologameChallenge> SologameChallenges { get; set; } = new List<SologameChallenge>();
        
        public ObservableCollection<AudienceChallenge> FilteredAudience { get; set; } = new ObservableCollection<AudienceChallenge>();
        public ObservableCollection<CrewChallenge> FilteredCrew { get; set; } = new ObservableCollection<CrewChallenge>();
        public ObservableCollection<MultipleChoiceChallenge> FilteredMulti { get; set; } = new ObservableCollection<MultipleChoiceChallenge>();
        public ObservableCollection<MusicChallenge> FilteredMusic { get; set; } = new ObservableCollection<MusicChallenge>();
        public ObservableCollection<QuizChallenge> FilteredQuiz { get; set; } = new ObservableCollection<QuizChallenge>();
        public ObservableCollection<ScreenshotChallenge> FilteredScreen { get; set; } = new ObservableCollection<ScreenshotChallenge>();
        public ObservableCollection<SilhouetteChallenge> FilteredSilhu { get; set; } = new ObservableCollection<SilhouetteChallenge>();
        public ObservableCollection<SologameChallenge> FilteredSolo { get; set; } = new ObservableCollection<SologameChallenge>();

        public Challenges ChallengesDataAccess { get; set; } = new Challenges();
        public DataAccess.Answers AnswersDataAccess { get; set; } = new DataAccess.Answers();
        
        public List<ChallengeBase> ChallengesFromDB { get; set; } = new List<ChallengeBase>();

        public CreateObjectsViewModel ObjectsViewModel { get; set; } = new CreateObjectsViewModel();

        public ObservableCollection<Game> Games { get; set; } = new ObservableCollection<Game>();
        public ObservableCollection<Image> Images { get; set; } = new ObservableCollection<Image>();
        public ObservableCollection<Music> Music { get; set; } = new ObservableCollection<Music>();

        /// <summary>Initializes a new instance of the <see cref="CreateChallengesViewModel"/> class.</summary>
        public CreateChallengesViewModel()
        {
            EditCommand = new RelayCommand<ChallengeBase>(async param =>
                                                    {
                                                        switch (param.GetDiscriminator())
                                                        {
                                                            case "AudienceChallenge":
                                                            case "CrewChallenge":
                                                            case "SologameChallenge":
                                                                ObjectsViewModel.EditCommand.Execute((param as GameChallenge).Game);
                                                                break;
                                                            case "MultipleChoiceChallenge":
                                                            case "QuizChallenge":
                                                                await AnswersDataAccess.EditAnswersAsync((param as QuestionChallenge).Answers);
                                                                break;
                                                            case "MusicChallenge":
                                                                ObjectsViewModel.EditCommand.Execute((param as MusicChallenge).Song);
                                                                break;
                                                            case "ScreenshotChallenge":
                                                            case "SilhouetteChallenge":
                                                                ObjectsViewModel.EditCommand.Execute((param as ImageChallenge).Image);
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
                                                                    FilteredAudience.Remove(param as AudienceChallenge);
                                                                    break;
                                                                case "CrewChallenge":
                                                                    CrewChallenges.Remove(param as CrewChallenge);
                                                                    FilteredCrew.Remove(param as CrewChallenge);
                                                                    break;
                                                                case "MultipleChoiceChallenge":
                                                                    MultipleChoiceChallenges.Remove(param as MultipleChoiceChallenge);
                                                                    FilteredMulti.Remove(param as MultipleChoiceChallenge);
                                                                    break;
                                                                case "MusicChallenge":
                                                                    MusicChallenges.Remove(param as MusicChallenge);
                                                                    FilteredMusic.Remove(param as MusicChallenge);
                                                                    break;
                                                                case "QuizChallenge":
                                                                    QuizChallenges.Remove(param as QuizChallenge);
                                                                    FilteredQuiz.Remove(param as QuizChallenge);
                                                                    break;
                                                                case "ScreenshotChallenge":
                                                                    ScreenshotChallenges.Remove(param as ScreenshotChallenge);
                                                                    FilteredScreen.Remove(param as ScreenshotChallenge);
                                                                    break;
                                                                case "SilhouetteChallenge":
                                                                    SilhouetteChallenges.Remove(param as SilhouetteChallenge);
                                                                    FilteredSilhu.Remove(param as SilhouetteChallenge);
                                                                    break;
                                                                case "SologameChallenge":
                                                                    SologameChallenges.Remove(param as SologameChallenge);
                                                                    FilteredSolo.Remove(param as SologameChallenge);
                                                                    break;
                                                            }
                                                    }, param => param != null);
        }

        /// <summary>Loads the challenges from database asynchronous.</summary>
        /// <returns></returns>
        public async Task LoadChallengesFromDBAsync()
        {
            if (AudienceChallenges.Count == 0)
                AudienceChallenges.AddRange(await ChallengesDataAccess.GetAudienceChallengesAsync());
            if (CrewChallenges.Count == 0)
                CrewChallenges.AddRange(await ChallengesDataAccess.GetCrewChallengesAsync());
            if (MultipleChoiceChallenges.Count == 0)
                MultipleChoiceChallenges.AddRange(await ChallengesDataAccess.GetMultipleChoiceChallengesAsync());
            if (MusicChallenges.Count == 0)
                MusicChallenges.AddRange(await ChallengesDataAccess.GetMusicChallengesAsync());
            if (QuizChallenges.Count == 0)
                QuizChallenges.AddRange(await ChallengesDataAccess.GetQuizChallengesAsync());
            if (ScreenshotChallenges.Count == 0)
                ScreenshotChallenges.AddRange(await ChallengesDataAccess.GetScreenshotChallengesAsync());
            if (SilhouetteChallenges.Count == 0)
                SilhouetteChallenges.AddRange(await ChallengesDataAccess.GetSilhouetteChallengesAsync());
            if (SologameChallenges.Count == 0)
                SologameChallenges.AddRange(await ChallengesDataAccess.GetSologameChallengesAsync());

            await ObjectsViewModel.LoadObjectsFromDBAsync();
            foreach (Game game in CreateObjectsViewModel.Games) Games.Add(game);
            foreach (Image image in CreateObjectsViewModel.Images) Images.Add(image);
            foreach (Music music in CreateObjectsViewModel.Music) Music.Add(music);

            foreach (AudienceChallenge challenge in AudienceChallenges)
                foreach (Game game in Games)
                    if (game.ID.Equals(challenge.GameID)) {
                        challenge.Game = game;
                        break;
                    }
            foreach (CrewChallenge challenge in CrewChallenges)
                foreach (Game game in Games)
                        if (game.ID.Equals(challenge.GameID)) {
                            challenge.Game = game;
                            break;
                        }
            foreach (MusicChallenge challenge in MusicChallenges)
                foreach (Music music in Music)
                    if (music.ID.Equals(challenge.SongID)){
                        challenge.Song = music;
                        break;
                    }
            foreach (ScreenshotChallenge challenge in ScreenshotChallenges)
                foreach (Image image in Images)
                    if (image.ID.Equals(challenge.ImageID)){
                        challenge.Image = image;
                        break;
                    }
            foreach (SilhouetteChallenge challenge in SilhouetteChallenges)
                foreach (Image image in Images)
                    if (image.ID.Equals(challenge.ImageID)){
                        challenge.Image = image;
                        break;
                    }
            foreach (SologameChallenge challenge in SologameChallenges)
                foreach (Game game in Games)
                    if (game.ID.Equals(challenge.GameID)){
                        challenge.Game = game;
                        break;
                    }

            CreateNewObjectPlaceholder();
        }

        /// <summary>Refreshes a filtered list based on selectedIndex.</summary>
        /// <param name="filterText">The filter text.</param>
        /// <param name="selectedIndex">Index of the selected.</param>
        internal void RefreshList(string filterText, int selectedIndex)
        {
            if (filterText == null) filterText = "";
            switch (selectedIndex)
            {
                case 0:
                    FilteredAudience.Clear();
                    foreach (var filtered in AudienceChallenges)
                        if(filtered.Game != null && filtered.Game.Name.ToLower().StartsWith(filterText)) FilteredAudience.Add(filtered);
                        else if (filterText.Equals("")) FilteredAudience.Add(filtered);
                    break;
                case 1:
                    FilteredCrew.Clear();
                    foreach (var filtered in CrewChallenges)
                        if(filtered.Game != null && filtered.Game.Name.ToLower().StartsWith(filterText)) FilteredCrew.Add(filtered);
                        else if (filterText.Equals("")) FilteredCrew.Add(filtered);
                    break;
                case 2:
                    FilteredMulti.Clear();
                    foreach (var filtered in MultipleChoiceChallenges.Where(x => x.ChallengeTask.ToLower().StartsWith(filterText)))
                        FilteredMulti.Add(filtered);
                    break;
                case 3:
                    FilteredMusic.Clear();
                    foreach (var filtered in MusicChallenges)
                        if (filtered.Song != null && filtered.Song.Name.ToLower().StartsWith(filterText)) FilteredMusic.Add(filtered);
                        else if (filterText.Equals("")) FilteredMusic.Add(filtered);
                    break;
                case 4:
                    FilteredQuiz.Clear();
                    foreach (var filtered in QuizChallenges.Where(x => x.ChallengeTask.ToLower().StartsWith(filterText)))
                        FilteredQuiz.Add(filtered);
                    break;
                case 5:
                    FilteredScreen.Clear();
                    foreach (var filtered in ScreenshotChallenges)
                        if (filtered.Image != null && filtered.Image.Name.ToLower().StartsWith(filterText)) FilteredScreen.Add(filtered);
                        else if (filterText.Equals("")) FilteredScreen.Add(filtered);
                    break;
                case 6:
                    FilteredSilhu.Clear();
                    foreach (var filtered in SilhouetteChallenges)
                        if (filtered.Image != null && filtered.Image.Name.ToLower().StartsWith(filterText)) FilteredSilhu.Add(filtered);
                        else if (filterText.Equals("")) FilteredSilhu.Add(filtered);
                    break;
                case 7:
                    FilteredSolo.Clear();
                    foreach (var filtered in SologameChallenges)
                        if(filtered.Game != null && filtered.Game.Name.ToLower().StartsWith(filterText)) FilteredSolo.Add(filtered);
                        else if (filterText.Equals("")) FilteredSolo.Add(filtered);
                    break;
            }
        }

        /// <summary>Creates the new object placeholder.</summary>
        internal void CreateNewObjectPlaceholder()
        {
            if (!Games.Last().Name.Equals("<Add a new game>")) Games.Add(new Game{ Name="<Add a new game>" });
            if (!Images.Last().Name.Equals("<Add a new image>")) Images.Add(new Image{ Name="<Add a new image>" });
            if (!Music.Last().Name.Equals("<Add a new song>")) Music.Add(new Music{ Name="<Add a new song>" });
        }
    }
}
