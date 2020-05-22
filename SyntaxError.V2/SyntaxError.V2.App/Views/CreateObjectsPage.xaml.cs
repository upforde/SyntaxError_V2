using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.UI.Controls;
using SyntaxError.V2.App.DataAccess;
using SyntaxError.V2.App.Helpers;
using SyntaxError.V2.App.ViewModels;
using SyntaxError.V2.Modell.ChallengeObjects;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace SyntaxError.V2.App.Views
{
    public sealed partial class CreateObjectsPage : Page
    {
        /// <summary>The stored item</summary>
        public MediaObject _storedItem;
        /// <summary>The stored item index</summary>
        public int _storedItemIndex = -1;
        private ICommand _editCommand;
        /// <summary>Edit command for the MediaObject.</summary>
        /// <value>The edit command.</value>
        public ICommand EditCommand => _editCommand ?? (_editCommand = new RelayCommand<MediaObject>(EditCommand_ItemClicked));
        /// <summary>  A command to add a new MediaObject to the database and list.</summary>
        /// <value>The add new object command.</value>
        public ICommand AddNewObjectCommand { get; set; }

        /// <summary>The filtered
        /// list of MediaObjects</summary>
        public ObservableCollection<MediaObject> Filtered = new ObservableCollection<MediaObject>();
        /// <summary>The segoe md l2 assets</summary>
        public FontFamily segoeMDL2Assets = new FontFamily("Segoe MDL2 Assets");
        /// <summary>Gets the view model.</summary>
        /// <value>The view model.</value>
        public CreateObjectsViewModel ViewModel { get; } = new CreateObjectsViewModel();

        /// <summary>The grid</summary>
        public UIElementCollection TheGrid;
        /// <summary>The action grid</summary>
        public Grid ActionGrid;
        /// <summary>The loading progress bar</summary>
        public ProgressBar LoadingProgressBar;
        /// <summary>The collection</summary>
        public AdaptiveGridView Collection;
        /// <summary>The smoke grid</summary>
        public Grid SmokeGrid;
        /// <summary>The smoke grid child</summary>
        public Grid SmokeGridChild;

        /// <summary>Initializes a new instance of the <see cref="CreateObjectsPage" /> class.</summary>
        public CreateObjectsPage()
        {
            InitializeComponent();

            Loaded += LoadMediaObjectsFromDBAsync;

            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChangedAsync;
        }

        /// <summary>Handles the NetworkAvailabilityChangedAsync event of the NetworkChange control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NetworkAvailabilityEventArgs" /> instance containing the event data.</param>
        private async void NetworkChange_NetworkAvailabilityChangedAsync(object sender, NetworkAvailabilityEventArgs e)
        {
            if (!e.IsAvailable)
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => LoadMediaObjectsFromDBAsync());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ChangeButtonsEnabled(e.IsAvailable));
        }

        /// <summary>Changes which buttons are enabled.</summary>
        /// <param name="access">if set to <c>true</c> [access].</param>
        private void ChangeButtonsEnabled(bool access)
        {
            GetCurrentGrid();
            
            ((ActionGrid.Children[0] as StackPanel).Children[0] as AutoSuggestBox).IsEnabled = access;
            ((ActionGrid.Children[0] as StackPanel).Children[1] as AppBarButton).IsEnabled = access;
            ((ActionGrid.Children[0] as StackPanel).Children[2] as AppBarButton).IsEnabled = access;
            (ActionGrid.Children[1] as AppBarButton).IsEnabled = access;
        }

        /// <summary>Refreshes the Filtered list.</summary>
        private void RefreshList()
        {
            Filtered.Clear();
            switch (CreateObjectsPivot.SelectedIndex)
            {
                case 0:
                    foreach (var item in CreateObjectsViewModel.Games)
                        Filtered.Add(item);
                    break;
                case 1:
                    foreach (var item in CreateObjectsViewModel.Images)
                        Filtered.Add(item);
                    break;
                case 2:
                    foreach (var item in CreateObjectsViewModel.Music)
                        Filtered.Add(item);
                    break;
            }
        }

        /// <summary>Updates the edited entry in the Filtered list.</summary>
        private void UpdateList()
        {
            Filtered.Where(x=>x.ID == _storedItem.ID).First().Name = _storedItem.Name;
        }

        /// <summary>Removes from Filtered list.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RemoveFromList(object sender, RoutedEventArgs e)
        {
            Filtered.RemoveAt(Filtered.IndexOf(Filtered.Where(x=>x.ID == _storedItem.ID).First()));
        }

        /// <summary>Loads the media objects from database asynchronous.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void LoadMediaObjectsFromDBAsync(object sender = null, RoutedEventArgs e = null)
        {
            var isInternetAvailable = NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
            if (isInternetAvailable)
            {
                GetCurrentGrid();
            
                LoadingProgressBar.Visibility = Visibility.Visible;
                Collection.Visibility = Visibility.Collapsed;

                if (await ViewModel.LoadObjectsFromDBAsync())
                {
                    LoadingProgressBar.Visibility = Visibility.Collapsed;
                    Collection.Visibility = Visibility.Visible;
                    RefreshList();
                }
            }
            ChangeButtonsEnabled(isInternetAvailable);
        }

        /// <summary>Handles the SelectionChanged event of the CreateObjectsPivot control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void CreateObjectsPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }

        /// <summary>Handles the Tapped event of the AdaptiveGridView control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedRoutedEventArgs"/> instance containing the event data.</param>
        private void AdaptiveGridView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var element = (e.OriginalSource as FrameworkElement).DataContext as MediaObject;
            _storedItem = element;
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

        /// <summary>Edits the clicked MediaObject inside of the AdaptiveGridView.</summary>
        /// <param name="clickedItem">The clicked item.</param>
        private void EditCommand_ItemClicked(MediaObject clickedItem)
        {
            GetCurrentGrid();

            switch (_storedItem.GetType().Name)
            {
                case "Game":
                    _storedItemIndex = CreateObjectsViewModel.Games.IndexOf(clickedItem as Game);
                    break;
                case "Image":
                    _storedItemIndex = CreateObjectsViewModel.Images.IndexOf(clickedItem as Modell.ChallengeObjects.Image);
                    break;
                case "Music":
                    _storedItemIndex = CreateObjectsViewModel.Music.IndexOf(clickedItem as Music);
                    break;
            }
            
            ConnectedAnimation animation = Collection.PrepareConnectedAnimation("forwardAnimation", _storedItem, "connectedElement");
            SmokeGrid.Visibility = Visibility.Visible;
            ((SmokeGridChild.Children[0] as Grid).Children[0] as ImageEx).Source = _storedItem.URI;
            (SmokeGridChild.Children[1] as TextBox).Text = _storedItem.Name;
            animation.TryStart(SmokeGrid.Children[0]);
        }

        /// <summary>Handles the Click event of the SaveButton control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _storedItem.Name = (SmokeGridChild.Children[1] as TextBox).Text;

            ViewModel.EditCommand.Execute(_storedItem);

            switch (_storedItem.GetType().Name)
            {
                case "Game":
                    var gameIndex = CreateObjectsViewModel.Games.IndexOf(CreateObjectsViewModel.Games.Where(obj => obj.ID == _storedItem.ID).First());
                    CreateObjectsViewModel.Games[gameIndex] = _storedItem as Game;
                    if (Filtered.Count != 0)
                        Filtered[Filtered.IndexOf(Filtered.Where(obj => obj.ID == _storedItem.ID).First())] = _storedItem as Game;
                    break;
                case "Image":
                    var imgIndex = CreateObjectsViewModel.Images.IndexOf(CreateObjectsViewModel.Images.Where(obj => obj.ID == _storedItem.ID).First());
                    CreateObjectsViewModel.Images[imgIndex] = _storedItem as Modell.ChallengeObjects.Image;
                    if (Filtered.Count != 0)
                        Filtered[Filtered.IndexOf(Filtered.Where(obj => obj.ID == _storedItem.ID).First())] = _storedItem as Modell.ChallengeObjects.Image;
                    break;
                case "Music":
                    var musicIndex = CreateObjectsViewModel.Music.IndexOf(CreateObjectsViewModel.Music.Where(obj => obj.ID == _storedItem.ID).First());
                    CreateObjectsViewModel.Music[musicIndex] = _storedItem as Music;
                    if (Filtered.Count != 0)
                        Filtered[Filtered.IndexOf(Filtered.Where(obj => obj.ID == _storedItem.ID).First())] = _storedItem as Music;
                    break;
            }

            BackButton_Click(sender, e);
        }

        /// <summary>Handles the Click event of the BackButton control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("backwardsAnimation", SmokeGrid.Children[0]);
            
            animation.Completed += Animation_Completed;
            
            Collection.ScrollIntoView(_storedItem, ScrollIntoViewAlignment.Default);
            Collection.UpdateLayout();
            
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
                animation.Configuration = new DirectConnectedAnimationConfiguration();
            
            await Collection.TryStartConnectedAnimationAsync(animation, _storedItem, "connectedElement");
        }

        /// <summary>  Handles the Animation_Completed event of the ConnectedAnimation.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        private void Animation_Completed(ConnectedAnimation sender, object args)
        {
            SmokeGrid.Visibility = Visibility.Collapsed;
            UpdateList();
        }

        /// <summary>Gets the current grid.</summary>
        private void GetCurrentGrid()
        {
            TheGrid = ((CreateObjectsPivot.SelectedItem as PivotItem).Content as Grid).Children;
            ActionGrid = TheGrid[0] as Grid;
            LoadingProgressBar = TheGrid[1] as ProgressBar;
            Collection = TheGrid[2] as AdaptiveGridView;
            SmokeGrid = TheGrid[3] as Grid;
            SmokeGridChild = ((SmokeGrid.Children[0] as Grid).Children[0] as StackPanel).Children[0] as Grid;
        }

        /// <summary>Handles the Click event of the AddNewObjectButton control.
        /// The button adds a new MediaObject to the database</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void AddNewObjectButton_Click(object sender, RoutedEventArgs e)
        {
            MediaObject newObject;
            switch (CreateObjectsPivot.SelectedIndex)
            {
                case 0:
                    newObject = new Game(){ Name="New Game" };
                    break;
                case 1:
                    newObject = new Modell.ChallengeObjects.Image(){ Name="New Image" };
                    break;
                case 2:
                    newObject = new Music(){ Name="New Song" };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ViewModel.AddNewObjectCommand.Execute(newObject);
            Filtered.Add(newObject);
        }

        /// <summary>Filters the list.</summary>
        /// <param name="theText">The text.</param>
        private void FilterTheList(string theText)
        {
            Filtered.Clear();
            GetCurrentGrid();
            
            var filterText = theText.ToLower();
            if (filterText == null || filterText == "") RefreshList();
            else
            {
                switch (CreateObjectsPivot.SelectedIndex)
                {
                    case 0:
                        foreach (var filtered in CreateObjectsViewModel.Games.Where(mo => mo.Name.ToLower().StartsWith(filterText)).ToList())
                            Filtered.Add(filtered);
                        break;
                    case 1:
                        foreach (var filtered in CreateObjectsViewModel.Images.Where(mo => mo.Name.ToLower().StartsWith(filterText)).ToList())
                            Filtered.Add(filtered);
                        break;
                    case 2:
                        foreach (var filtered in CreateObjectsViewModel.Music.Where(mo => mo.Name.ToLower().StartsWith(filterText)).ToList())
                            Filtered.Add(filtered);
                        break;
                }
            }
        }

        /// <summary>  Handles the QuerySubmitted event of the AutoSuggestBox.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxQuerySubmittedEventArgs"/> instance containing the event data.</param>
        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            FilterTheList(sender.Text);
        }

        /// <summary>  Handles the TextChanged event of the AutoSuggestBox.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxTextChangedEventArgs"/> instance containing the event data.</param>
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            FilterTheList(sender.Text);
        }

        /// <summary>Handles the Click event of the AppBarButton_SelectionMode control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AppBarButton_SelectionMode_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentGrid();
            var button = (ActionGrid.Children[0] as StackPanel).Children[2] as AppBarButton;
            button.Visibility = (button.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            if (button.Visibility == Visibility.Visible)
            {
                Collection.Tapped -= AdaptiveGridView_Tapped;
                Collection.SelectionMode = ListViewSelectionMode.Multiple;
            }
            else
            {
                Collection.Tapped += AdaptiveGridView_Tapped;
                Collection.SelectionMode = ListViewSelectionMode.None;
            }
        }

        /// <summary>Handles the Click event of the AppBarButton_DeleteSelected control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void AppBarButton_DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var deletionList = Collection.SelectedItems.ToList();
            foreach (var itemToDelete in deletionList)
            {
                await Task.Run(() => ViewModel.DeleteCommand.Execute(itemToDelete));
                Filtered.Remove(itemToDelete as MediaObject);
            }
        }

        /// <summary>Handles the Tapped event of the StackPanel control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedRoutedEventArgs" /> instance containing the event data.</param>
        private async void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                _storedItem.URI = await ViewModel.ImagesDataAccess.PostImageAsync(file);
                if (_storedItem.URI != null)
                    ((SmokeGridChild.Children[0] as Grid).Children[0] as ImageEx).Source = _storedItem.URI;
            }
        }
    }
}
