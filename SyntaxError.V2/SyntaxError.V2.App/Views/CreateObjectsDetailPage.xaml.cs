using System;

using Microsoft.Toolkit.Uwp.UI.Animations;

using SyntaxError.V2.App.Services;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.ChallengeObjects;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class CreateObjectsDetailPage : Page
    {
        public CreateObjectsDetailViewModel ViewModel { get; } = new CreateObjectsDetailViewModel();

        public CreateObjectsDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is MediaObject mediaObject)
            {
                ViewModel.Initialize(mediaObject);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
