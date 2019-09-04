using System;
using System.Net.NetworkInformation;
using Microsoft.Toolkit.Uwp.Connectivity;
using SyntaxError.V2.App.ViewModels;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace SyntaxError.V2.App.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            ApplicationView.PreferredLaunchViewSize = new Size(840, 600);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Loaded += ShellPage_Loaded;
        }

        private async void ShellPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => InAppNotification.Show("No network access", 2000));
            }
        }

        private async void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if(!e.IsAvailable)
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => InAppNotification.Show("No network access", 2000));
        }
    }
}
