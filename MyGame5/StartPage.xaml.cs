using Isometric.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Isometric
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class StartPage : Page
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


        public StartPage()
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
            A_Category.IsChecked = ManagerGame.Type;
            B_Category.IsChecked = !ManagerGame.Type;
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
   
        private  void StackPanel_PointerReleased(object sender, RoutedEventArgs e)
        {
            string name="";
            if(sender is StackPanel)name=((StackPanel)sender).Name;
            if(sender is Rectangle)name=((Rectangle)sender).Name;
            if (this.Frame != null)
                switch (name)
                {
                    case "ButtonStartGame":
                        ManagerGame.ExerciseOrTest = false;
                        this.Frame.Navigate(typeof(ExercisePage));
                        break;
                    case "ButtonLearn":
                        Frame.Navigate(typeof(InformationPage)); 
                        break;
                    case "ButtonTest":
                        ManagerGame.ExerciseOrTest = true;
                        Frame.Navigate(typeof(ExercisePage));
                        break;

                }
        }

        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
                switch (((Button)sender).Name)
                {
                    case "AppBar_Marks":
                      Frame.Navigate(typeof(StatisticsPage)); break;
                    case "AppBar_OpenSavedGame":
                        Frame.Navigate(typeof(OpensvedGamePage)); break;
                    case "AppBar_Level":
                        CreatePopupmenu(sender); break;
                }
        }
        //הצגת השלבים בתפריט קופץ
        private async void CreatePopupmenu(object sender)
        {
            PopupMenu menu = new PopupMenu();

            menu.Commands.Add(new UICommand("אלוף", new UICommandInvokedHandler(SelectLevel), 2));
            menu.Commands.Add(new UICommandSeparator());
            menu.Commands.Add(new UICommand("מתקדם", new UICommandInvokedHandler(SelectLevel), 1));
            menu.Commands.Add(new UICommandSeparator());
            menu.Commands.Add(new UICommand("מתחיל", new UICommandInvokedHandler(SelectLevel), 0));
            await menu.ShowAsync(((FrameworkElement)sender).TransformToVisual(null).TransformPoint(new Point()));
   
            Popup p = new Popup();
            p.Width = 100;
            p.Height = 100;
           p.IsOpen = true;
        }

     
        //בחירת שלב
        private void SelectLevel(IUICommand command)
        {
            ManagerGame.Level = (int)command.Id;
        }

        //בחירת קטגוריה
        private void Category_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (A_Category.IsChecked != null)
                    ManagerGame.Type = A_Category.IsChecked.Value;
            }
            catch (Exception)
            {
            }
            try
            {
                if (ManagerGame.Type)
                {
                    ImageA.Visibility = Visibility.Visible;
                    ImageB.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ImageB.Visibility = Visibility.Visible;
                    ImageA.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                
            
            }
        }

        private void pageTitle_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void toggleSwitchType_Toggled(object sender, RoutedEventArgs e)
        {
            //if (toggleSwitchType!=null)
            //ManagerGame.Type = toggleSwitchType.IsOn;
        }

        private void AppBar_Marks_PointerMoved(object sender, PointerRoutedEventArgs e)
        {

        }

        private void buttonSP_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush(Color.FromArgb(255, 34, 34, 34));
        }

        private void buttonSP_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush(Color.FromArgb(255,68,68,68));
        }

        private void buttonSP_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(Frame!=null)
            switch(((StackPanel)sender).Name)
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

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ManagerGame.ExerciseOrTest = false;
            this.Frame.Navigate(typeof(ExercisePage));
        }

    }
}
