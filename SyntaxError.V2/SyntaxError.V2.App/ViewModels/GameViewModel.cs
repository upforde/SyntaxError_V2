using System;
using System.Collections.ObjectModel;
using SyntaxError.V2.App.Helpers;

namespace SyntaxError.V2.App.ViewModels
{
    public class GameViewModel : Observable
    {
        public ObservableCollection<string> List = new ObservableCollection<string>();

        public GameViewModel()
        {
        }
    }
}
