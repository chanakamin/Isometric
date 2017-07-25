using Isometric.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class SrartPage : Page
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


        public SrartPage()
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

        int Level;//שמירת רמת המשחק אותה בחר המשתמש
        bool CategorySelected;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog(" ");
            //•	הצגת ההודעה -

            if (this.Frame != null)
                switch (((Button)sender).Name)
                {
                    case "ButtonStartGame":
                        dialog = new MessageDialog("רוצה לשחק?-עדסוף השבוע יתחיל המשחק...  ..... ", "משחק"); break;
                    case "ButtonLearn":
                        dialog = new MessageDialog("רוצה ללמוד?- חכה שנכתוב חומר לימודי...  ..... ", "לימוד"); break;
                    case "ButtonTest":
                        dialog = new MessageDialog("רוצה להבחן?- לא תרגלת אז איך תבחן?  ..... ", "מבחן"); break;

                }
            await dialog.ShowAsync();
        }

        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
                switch (((Button)sender).Name)
                {
                    case "AppBar_Marks":
                     //   Frame.Navigate(typeof(MarksPage)); break;
                    case "AppBar_OpenSavedGame":
                    //    Frame.Navigate(typeof(OpenSavedGamePage)); break;
                    case "AppBar_Level":
                        CreatePopupmenu(sender); break;
                }
        }

        private async void CreatePopupmenu(object sender)
        {
            PopupMenu menu = new PopupMenu();

            menu.Commands.Add(new UICommand("מתחיל", new UICommandInvokedHandler(SelectLevel), 1));
            menu.Commands.Add(new UICommandSeparator());
            menu.Commands.Add(new UICommand("מתקדם", new UICommandInvokedHandler(SelectLevel), 2));
            menu.Commands.Add(new UICommandSeparator());
            menu.Commands.Add(new UICommand("אלוף", new UICommandInvokedHandler(SelectLevel), 3));
            // await menu.ShowForSelectionAsync(GetElementRect((FrameworkElement)sender));

            await menu.ShowAsync(((FrameworkElement)sender).TransformToVisual(null).TransformPoint(new Point()));
        }

        //public static Rect GetElementRect(FrameworkElement element)
        //{
        //    GeneralTransform buttonTransform = element.TransformToVisual(null);
        //    Point point = buttonTransform.TransformPoint(new Point());
        //    return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        //}

        private void SelectLevel(IUICommand command)
        {
            Level = (int)command.Id;
            //יש לשמור את הרמה שנבחרה נתוני יישום או נתוני מופע
            //או לשלוח בתור פרמטר
        }

        private void Category_Checked(object sender, RoutedEventArgs e)
        {
        //    CategorySelected = !CategorySelected;
            switch ((sender as RadioButton).Name)
            {
                case "A_Category": CategorySelected = true; break;
                case "B_Category": CategorySelected = false; break;
                default: CategorySelected = true; break;
            }
        }
    }
}
