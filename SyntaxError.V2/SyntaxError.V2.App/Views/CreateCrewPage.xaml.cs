using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.Connectivity;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.ChallengeObjects;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class CreateCrewPage : Page
    {
        /// <summary>The stored crew member</summary>
        public CrewMember _storedMember;

        /// <summary>The segoe md l2 assets</summary>
        public FontFamily segoeMDL2Assets = new FontFamily("Segoe MDL2 Assets");
        
        /// <summary>  A command to add a new CrewMember to the database and list.</summary>
        /// <value>The add new object command.</value>
        public ICommand AddNewCrewMemberCommand { get; set; }
        private ICommand _editCommand;
        /// <summary>A command to edit the selected CewMember.</summary>
        /// <value>The edit command.</value>
        public ICommand EditCommand => _editCommand ?? (_editCommand = new RelayCommand<CrewMember>(EditCommand_ItemClicked));

        /// <summary>The filtered
        /// crew members</summary>
        public ObservableCollection<CrewMember> Filtered = new ObservableCollection<CrewMember>();
        /// <summary>Gets the view model.</summary>
        /// <value>The view model.</value>
        public CreateCrewViewModel ViewModel { get; } = new CreateCrewViewModel();

        public CreateCrewPage()
        {
            InitializeComponent();

            AddNewCrewMemberCommand = new RelayCommand<CrewMember>(async param =>
                                                    {
                                                        param = await ViewModel.CrewMembersDataAccess.CreateCrewMemberAsync(param);
                                                        
                                                        ViewModel.CrewMembers.Add(param);
                                                        Filtered.Add(param);
                                                        
                                                    }, param => param != null);

            Loaded += LoadCrewMembersFromDBAsync;
            
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChangedAsync;
        }

        /// <summary>Handles the NetworkAvailabilityChangedAsync event of the NetworkChange control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NetworkAvailabilityEventArgs" /> instance containing the event data.</param>
        private async void NetworkChange_NetworkAvailabilityChangedAsync(object sender, NetworkAvailabilityEventArgs e)
        {
            if (!e.IsAvailable)
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => LoadCrewMembersFromDBAsync());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ChangeButtonsEnabled(e.IsAvailable));
        }

        /// <summary>Changes which buttons are enabled.</summary>
        /// <param name="access">if set to <c>true</c> [access].</param>
        private void ChangeButtonsEnabled(bool access)
        {
            if (!access)
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            AddButton.IsEnabled = access;
            SearchBar.IsEnabled = access;
        }

        /// <summary>Refreshes the Filtered list.</summary>
        private void RefreshList()
        {
            Filtered.Clear();
            foreach (CrewMember crewMember in ViewModel.CrewMembers)
                Filtered.Add(crewMember);
        }

        /// <summary>Updates the edited entry in the Filtered list.</summary>
        private void UpdateList()
        {
            var index = Filtered.IndexOf(Filtered.Where(cm => cm.CrewMemberID == _storedMember.CrewMemberID).First());
            Filtered[index] = _storedMember;
        }

        /// <summary>Removes from Filtered list.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RemoveFromList(object sender, RoutedEventArgs e)
        {
            Filtered.RemoveAt(Filtered.IndexOf(Filtered.Where(x=>x.CrewMemberID == _storedMember.CrewMemberID).First()));
        }

        /// <summary>Loads the crew members from database asynchronous.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void LoadCrewMembersFromDBAsync(object sender = null, RoutedEventArgs e = null)
        {
            var isInternetAvailable = NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
            if (isInternetAvailable)
            {
                LoadingProgressBar.Visibility = Visibility.Visible;
                CrewGrid.Visibility = Visibility.Collapsed;

                try{
                    if (await ViewModel.LoadCrewMembersFromDBAsync())
                    {
                        LoadingProgressBar.Visibility = Visibility.Collapsed;
                        CrewGrid.Visibility = Visibility.Visible;
                        RefreshList();
                    }
                } catch (System.Net.Http.HttpRequestException){}
            }
            ChangeButtonsEnabled(isInternetAvailable);
        }

        /// <summary>Handles the Tapped event of the AdaptiveGridView control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedRoutedEventArgs"/> instance containing the event data.</param>
        private void AdaptiveGridView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var element = ((e.OriginalSource as FrameworkElement).DataContext as CrewMember);
            _storedMember = element;
            var myFlyout = new MenuFlyout();

            var editItem = new MenuFlyoutItem
            {
                Text = "Edit",
                Icon = new FontIcon()
                {
                    FontFamily = segoeMDL2Assets,
                    Glyph = "\uE70F"
                },
                Command = EditCommand,
                CommandParameter = element
            };
            var deleteItem = new MenuFlyoutItem
            {
                Text = "Delete",
                Icon = new FontIcon()
                {
                    FontFamily = segoeMDL2Assets,
                    Glyph = "\uE74D"
                },
                Command = ViewModel.DeleteCommand,
                CommandParameter = element
            };
            
            deleteItem.Click += RemoveFromList;
            myFlyout.Items.Add(editItem);
            myFlyout.Items.Add(deleteItem);
            
            if (element != null)
                myFlyout.ShowAt((sender as UIElement), e.GetPosition(sender as UIElement));
        }

        /// <summary>Edits the clicked CrewMember inside of the AdaptiveGridView.</summary>
        /// <param name="clickedItem">The clicked item.</param>
        private void EditCommand_ItemClicked(CrewMember clickedCrew)
        {
            ConnectedAnimation animation = CrewGrid.PrepareConnectedAnimation("forwardAnimation", _storedMember, "connectedElement");
            SmokeGrid.Visibility = Visibility.Visible;

            SmokeGridText.Text = _storedMember.CrewTag;
            animation.TryStart(SmokeGrid.Children[0]);
        }

        /// <summary>Handles the Click event of the AddNewCrewMemberButton control.
        /// The button adds a new CrewMember to the database</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AddNewCrewMemberButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewCrewMemberCommand.Execute(new CrewMember{ CrewTag="New Member" });
        }

        /// <summary>Handles the Click event of the BackButton control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", SmokeGrid.Children[0]);
            
            animation.Completed += Animation_Completed;
            
            CrewGrid.ScrollIntoView(_storedMember, ScrollIntoViewAlignment.Default);
            CrewGrid.UpdateLayout();
            
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                animation.Configuration = new DirectConnectedAnimationConfiguration();
            
            await CrewGrid.TryStartConnectedAnimationAsync(animation, _storedMember, "connectedElement");
        }
        
        /// <summary>  Handles the Animation_Completed event of the ConnectedAnimation.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        private void Animation_Completed(ConnectedAnimation sender, object args)
        {
            SmokeGrid.Visibility = Visibility.Collapsed;
            UpdateList();
        }

        /// <summary>Handles the Click event of the SaveButton control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _storedMember.CrewTag = SmokeGridText.Text;

            ViewModel.EditCommand.Execute(_storedMember);

            var index = ViewModel.CrewMembers.IndexOf(ViewModel.CrewMembers.Where(c => c.CrewMemberID == _storedMember.CrewMemberID).First());
            ViewModel.CrewMembers[index] = _storedMember;

            BackButton_Click(sender, e);
        }

        /// <summary>Filters the list.</summary>
        /// <param name="theText">The text.</param>
        private void FilterTheList(string theText)
        {
            Filtered.Clear();
            
            var filterText = theText.ToLower();
            if (filterText == null || filterText == "")
                RefreshList();
            else
            {
                foreach (var filtered in ViewModel.CrewMembers.Where(c => c.CrewTag.ToLower().StartsWith(filterText)).ToList())
                    Filtered.Add(filtered);
            }
        }

        /// <summary>Handles the TextChanged event of the AutoSuggestBox control.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxTextChangedEventArgs"/> instance containing the event data.</param>
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            FilterTheList(sender.Text);
        }

        /// <summary>andles the QuerySubmitted event of the AutoSuggestBox control.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxQuerySubmittedEventArgs"/> instance containing the event data.</param>
        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            FilterTheList(sender.Text);
        }
    }
}
