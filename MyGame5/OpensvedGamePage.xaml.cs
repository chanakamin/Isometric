using Isometric.Common;
using Isometric.SavedGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Isometric
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class OpensvedGamePage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public OpensvedGamePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            load();
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        async public void load()
        {
            //ManagerGame.ToXml("Rivka", "behezrathashemnasevenatzlizch", null);
            SavedGameData items = new SavedGameData();
            await items.openFiles();
            List<GroupInfoList<object>> groups = items.GetGroupsByUserName();
            collectionSource.Source = groups;
            groupGridView.ItemsSource = collectionSource.View.CollectionGroups;
        }
        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        private void SelectedGame(object sender, ItemClickEventArgs e)
        {
            string gameName = ((SavedGame.SavedGame)e.ClickedItem).GameName;
            if (this.Frame != null)
                this.Frame.Navigate(typeof(ExercisePage), gameName);
            //איך אפשר להעביר מספר פרמטרים
            // new { GameName = gameName, TypeGame = true });
            //ManagerGame.
        }
        private void buttonSP_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush(Color.FromArgb(255, 34, 34, 34));
        }

        private void buttonSP_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush(Color.FromArgb(255, 68, 68, 68));
        }

        private void buttonSP_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Frame != null)
                switch (((StackPanel)sender).Name)
                {
                    case "buttonSP_Info":
                        Frame.Navigate(typeof(InformationPage));
                        break;
                    case "buttonSP_Test":
                        ManagerGame.ExerciseOrTest = true;
                        Frame.Navigate(typeof(ExercisePage));
                        break;
                    case "buttonSP_Exp":
                        ManagerGame.ExerciseOrTest = false;
                        this.Frame.Navigate(typeof(ExercisePage));
                        break;
                    case "buttonSP_Home":
                        this.Frame.Navigate(typeof(StartPage));
                        break;

                }
        }
    }
}
