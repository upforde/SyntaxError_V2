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

        public List<Game> Games = new List<Game>();
        public List<Image> Images = new List<Image>();
        public List<Music> Music = new List<Music>();
        
        public MediaObjects MediaObjectsDataAccess = new MediaObjects();
        public Images ImagesDataAccess = new DataAccess.Images();

        public CreateObjectsViewModel()
        {
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
                                                                    break;
                                                                case "Image":
                                                                    Images.Remove(param as Image);
                                                                    break;
                                                                case "Music":
                                                                    Music.Remove(param as Music);
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
            if (Images.Count == 0)
            {
                var images = await MediaObjectsDataAccess.GetMediaObjectsOfTypeAsync("Image");
                foreach (Image image in images)
                    Images.Add(image);
            }
            if (Music.Count == 0)
            {
                var songs = await MediaObjectsDataAccess.GetMediaObjectsOfTypeAsync("Music");
                foreach (Music song in songs)
                    Music.Add(song);
            }
            return true;
        }
    }
}
