using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Toolkit.Uwp.UI.Animations;

using SyntaxError.V2.App.Core.Models;
using SyntaxError.V2.App.Core.Services;
using SyntaxError.V2.App.DataAccess;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.Services;
using SyntaxError.V2.App.Views;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;
using Windows.UI.Xaml.Media.Animation;

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

        public ICommand AddNewObjectCommand { get; set; }

        public static List<Game> Games = new List<Game>();
        public static List<Image> Images = new List<Image>();
        public static List<Music> Music = new List<Music>();
        
        public MediaObjects MediaObjectsDataAccess = new MediaObjects();
        public Images ImagesDataAccess = new DataAccess.Images();

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
                                                                    Games.Remove(param as Game);
                                                                    CreateChallengesViewModel.AudienceChallenges.RemoveAll(x => x.GameID == param.ID);
                                                                    CreateChallengesViewModel.CrewChallenges.RemoveAll(x => x.GameID == param.ID);
                                                                    CreateChallengesViewModel.SologameChallenges.RemoveAll(x => x.GameID == param.ID);
                                                                    break;
                                                                case "Image":
                                                                    Images.Remove(param as Image);
                                                                    CreateChallengesViewModel.ScreenshotChallenges.RemoveAll(x => x.ImageID == param.ID);
                                                                    CreateChallengesViewModel.SilhouetteChallenges.RemoveAll(x => x.ImageID == param.ID);
                                                                    break;
                                                                case "Music":
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

        public List<Game> GetGames()
        {
            return Games;
        }
        public List<Image> GetImages()
        {
            return Images;
        }
        public List<Music> GetMusics()
        {
            return Music;
        }
    }
}
