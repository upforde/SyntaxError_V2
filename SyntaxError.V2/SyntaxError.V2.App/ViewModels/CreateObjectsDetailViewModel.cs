using System;
using System.Linq;

using SyntaxError.V2.App.Core.Models;
using SyntaxError.V2.App.Core.Services;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.Modell.ChallengeObjects;
using Windows.UI.Xaml.Controls;

namespace SyntaxError.V2.App.ViewModels
{
    public class CreateObjectsDetailViewModel : Observable
    {
        private MediaObject _item;

        public MediaObject Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public FontIcon ItemGlyph = new FontIcon();

        public CreateObjectsDetailViewModel()
        {
        }

        public void Initialize(MediaObject item)
        {
            Item = item;
            switch (Item.GetType().Name)
            {
                case "Game":
                    ItemGlyph.Glyph = "\uE7FC";
                    break;
                case "Image":
                    ItemGlyph.Glyph = "\uE8B9";
                    break;
                case "Music":
                    ItemGlyph.Glyph = "\uE8D6";
                    break;
            }
        }
    }
}
