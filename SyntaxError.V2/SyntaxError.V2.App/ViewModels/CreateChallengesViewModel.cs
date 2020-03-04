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

        public ObservableCollection<AudienceChallenge> AudienceChallenges { get; set; } = new ObservableCollection<AudienceChallenge>();
        public ObservableCollection<CrewChallenge> CrewChallenges { get; set; } = new ObservableCollection<CrewChallenge>();
        public ObservableCollection<MultipleChoiceChallenge> MultipleChoiceChallenges { get; set; } = new ObservableCollection<MultipleChoiceChallenge>();
        public ObservableCollection<MusicChallenge> MusicChallenges { get; set; } = new ObservableCollection<MusicChallenge>();
        public ObservableCollection<QuizChallenge> QuizChallenges { get; set; } = new ObservableCollection<QuizChallenge>();
        public ObservableCollection<ScreenshotChallenge> ScreenshotChallenges { get; set; } = new ObservableCollection<ScreenshotChallenge>();
        public ObservableCollection<SilhouetteChallenge> SilhouetteChallenges { get; set; } = new ObservableCollection<SilhouetteChallenge>();
        public ObservableCollection<SologameChallenge> SologameChallenges { get; set; } = new ObservableCollection<SologameChallenge>();
        
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
        internal async Task LoadChallengesFromDBAsync()
        {
            var challenges = await ChallengesDataAccess.GetChallengesAsync();
            
            await ObjectsViewModel.LoadObjectsFromDBAsync();
            foreach (Game game in CreateObjectsViewModel.Games) Games.Add(game);
            foreach (Image image in CreateObjectsViewModel.Images) Images.Add(image);
            foreach (Music music in CreateObjectsViewModel.Music) Music.Add(music);

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
                    foreach (var filtered in MusicChallenges.Where(x => x.Song.Name.ToLower().StartsWith(filterText)))
                        FilteredMusic.Add(filtered);
                    break;
                case 4:
                    FilteredQuiz.Clear();
                    foreach (var filtered in QuizChallenges.Where(x => x.ChallengeTask.ToLower().StartsWith(filterText)))
                        FilteredQuiz.Add(filtered);
                    break;
                case 5:
                    FilteredScreen.Clear();
                    foreach (var filtered in ScreenshotChallenges.Where(x => x.Image.Name.ToLower().StartsWith(filterText)))
                        FilteredScreen.Add(filtered);
                    break;
                case 6:
                    FilteredSilhu.Clear();
                    foreach (var filtered in SilhouetteChallenges.Where(x => x.Image.Name.ToLower().StartsWith(filterText)))
                        FilteredSilhu.Add(filtered);
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
            if (!Games.Contains(new Game())) Games.Add(new Game{ Name="<Add a new game>" });
            if (!Images.Contains(new Image())) Images.Add(new Image{ Name="<Add a new image>" });
            if (!Music.Contains(new Music())) Music.Add(new Music{ Name="<Add a new song>" });
        }
    }
}
