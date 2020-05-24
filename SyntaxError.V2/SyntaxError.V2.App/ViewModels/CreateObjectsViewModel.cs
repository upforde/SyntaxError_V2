using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using SyntaxError.V2.App.DataAccess;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.App.ViewModels
{
    public class CreateObjectsViewModel : Observable
    {
        /// <summary>A command to edit a MediaObject in the database and list.</summary>
        /// <value>The edit command.</value>
        public ICommand EditCommand { get; set; }
        /// <summary>A command to delete a new MediaObject from the database and list.</summary>
        /// <value>The delete command.</value>
        public ICommand DeleteCommand { get; set; }

        /// <summary>Adds a new media object to the database and list.</summary>
        /// <value>The add new object command.</value>
        public ICommand AddNewObjectCommand { get; set; }

        /// <summary>The games</summary>
        public static List<Game> Games = new List<Game>();
        /// <summary>The images</summary>
        public static List<Image> Images = new List<Image>();
        /// <summary>The music</summary>
        public static List<Music> Music = new List<Music>();

        /// <summary>The media objects data access</summary>
        public MediaObjects MediaObjectsDataAccess = new MediaObjects();
        /// <summary>The images data access</summary>
        public Images ImagesDataAccess = new DataAccess.Images();

        /// <summary>Initializes a new instance of the <see cref="CreateObjectsViewModel" /> class.</summary>
        public CreateObjectsViewModel()
        {
            AddNewObjectCommand = new RelayCommand<MediaObject>(async param =>
                                                    {
                                                        param = await MediaObjectsDataAccess.CreateMediaObjectAsync(param);

                                                        switch (param.GetType().Name)
                                                        {
                                                            case "Game":
                                                                Games.Add(param as Game);
                                                                break;
                                                            case "Image":
                                                                Images.Add(param as Image);
                                                                break;
                                                            case "Music":
                                                                Music.Add(param as Music);
                                                                break;
                                                        }
                                                    }, param => param != null);

            EditCommand = new RelayCommand<MediaObject>(async param =>
                                                    {
                                                        await MediaObjectsDataAccess.EditMediaObjectAsync(param);
                                                    }, param => param != null);

            DeleteCommand = new RelayCommand<MediaObject>(async param =>
                                                    {
                                                        if (await MediaObjectsDataAccess.DeleteMediaObjectAsync(param))
                                                            switch (param.GetType().Name)
                                                            {
                                                                case "Game":
                                                                    ImagesDataAccess.DeleteImageAsync(param.URI);
                                                                    Games.Remove(param as Game);
                                                                    CreateChallengesViewModel.AudienceChallenges.RemoveAll(x => x.GameID == param.ID);
                                                                    CreateChallengesViewModel.CrewChallenges.RemoveAll(x => x.GameID == param.ID);
                                                                    CreateChallengesViewModel.SologameChallenges.RemoveAll(x => x.GameID == param.ID);
                                                                    break;
                                                                case "Image":
                                                                    ImagesDataAccess.DeleteImageAsync(param.URI);
                                                                    Images.Remove(param as Image);
                                                                    CreateChallengesViewModel.ScreenshotChallenges.RemoveAll(x => x.ImageID == param.ID);
                                                                    CreateChallengesViewModel.SilhouetteChallenges.RemoveAll(x => x.ImageID == param.ID);
                                                                    break;
                                                                case "Music":
                                                                    ImagesDataAccess.DeleteImageAsync(param.URI);
                                                                    Music.Remove(param as Music);
                                                                    CreateChallengesViewModel.MusicChallenges.RemoveAll(x => x.SongID == param.ID);
                                                                    break;
                                                            }
                                                    }, param => param != null);
        }

        /// <summary>Loads the objects from database asynchronous.</summary>
        /// <returns></returns>
        internal async Task<bool> LoadObjectsFromDBAsync()
        {
            if (Games.Count == 0)
            {
                var games = await MediaObjectsDataAccess.GetMediaObjectsOfTypeAsync("Game");
                foreach (Game game in games)
                    Games.Add(game);
            }
            else
            {
                var games = await MediaObjectsDataAccess.UpdateGameListAsync(Games);
                foreach (Game game in games) Games.Add(game);
            } 
            if (Images.Count == 0)
            {
                var images = await MediaObjectsDataAccess.GetMediaObjectsOfTypeAsync("Image");
                foreach (Image image in images)
                    Images.Add(image);
            }
            else
            {
                var images = await MediaObjectsDataAccess.UpdateImageListAsync(Images);
                foreach (Image image in images) Images.Add(image);
            }
            if (Music.Count == 0)
            {
                var songs = await MediaObjectsDataAccess.GetMediaObjectsOfTypeAsync("Music");
                foreach (Music song in songs)
                    Music.Add(song);
            }
            else
            {
                var music = await MediaObjectsDataAccess.UpdateMusicListAsync(Music);
                foreach (Music song in music) Music.Add(song);
            }
            return true;
        }

        /// <summary>Gets the games.</summary>
        /// <returns></returns>
        public List<Game> GetGames()
        {
            return Games;
        }
        /// <summary>Gets the images.</summary>
        /// <returns></returns>
        public List<Image> GetImages()
        {
            return Images;
        }
        /// <summary>Gets the musics.</summary>
        /// <returns></returns>
        public List<Music> GetMusics()
        {
            return Music;
        }
    }
}
