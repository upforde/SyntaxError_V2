using System;
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

namespace SyntaxError.V2.App.ViewModels
{
    public class CreateObjectsViewModel : Observable
    {
        private ICommand _itemClickCommand;

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<MediaObject>(OnItemClick));
        
        public ObservableCollection<Game> Games = new ObservableCollection<Game>();
        public ObservableCollection<Image> Images = new ObservableCollection<Image>();
        public ObservableCollection<Music> Music = new ObservableCollection<Music>();

        public MediaObjects MediaObjectsDataAccess = new MediaObjects();

        public CreateObjectsViewModel()
        {
        }

        private void OnItemClick(MediaObject clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate<CreateObjectsDetailPage>(clickedItem);
            }
        }

        internal async Task LoadObjectsFromDBAsync()
        {
            if (Games.Count == 0)
            {
                var games = await MediaObjectsDataAccess.GetMediaObjectsOfTypeAsync("Game");
                foreach (Game game in games)
                {
                    Games.Add(game);
                }
            }
            if (Images.Count == 0)
            {
                var images = await MediaObjectsDataAccess.GetMediaObjectsOfTypeAsync("Image");
                foreach (Image image in images)
                {
                    Images.Add(image);
                }
            }
            if (Music.Count == 0)
            {
                var songs = await MediaObjectsDataAccess.GetMediaObjectsOfTypeAsync("Music");
                foreach (Music song in songs)
                {
                    Music.Add(song);
                }
            }
        }
    }
}
