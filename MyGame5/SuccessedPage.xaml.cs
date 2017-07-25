using Isometric._3DObjects;
using Isometric.Common;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Isometric
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SuccessedPage : Page
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


        public SuccessedPage()
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
        Draw3D draw3d;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            draw3d = new Draw3D(true);
            draw3d.EndPage = true;
            draw3d.Run(panelCube);
            StartAnimation();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        //החלפת צבעים ברקע על ידי הגדרת טיימר
        DispatcherTimer colortimer = new DispatcherTimer();
        List<Color> colors = new List<Color>() {
                                               Color.FromArgb(255,145,249,145),//ירוק FF91F991
                                              Color.FromArgb(255,138,231,247),//תכלת  FF8AE7F7
                                              Color.FromArgb(255,123,117,245),//כחול  FF7B75F5
                                               Color.FromArgb(255,159,51,243),//סגול  FF9F79F3
                                              Color.FromArgb(255,249,156,232),//ורוד  FFF99CE8
                                              Color.FromArgb(255,253,99,99),//אדום  FFFD6D6D
                                              Color.FromArgb(255,245,192,115),//כתום  FFF5C073
                                             Color.FromArgb(255,249,255,117),//צהוב  FFF9FF75
                                             Color.FromArgb(255,145,249,145),//ירוק FF91F991
                                          
                                                };
        int i = 0;
        ColorAnimation ca = new ColorAnimation();
        private void Func()
        {
            colorStoryboard.Stop();
            ca.Duration = new Duration(TimeSpan.FromSeconds(2.5));
            ca.From = colors[i];
            ca.To = colors[i + 1];
            Storyboard.SetTargetProperty(ca, "(Panel.Background).(SolidColorBrush.Color)");
            Storyboard.SetTargetName(ca, "myStackPanel");
            try
            {
                Storyboard yy = (myStackPanel.Resources["colorStoryboard"] as Storyboard);
                yy.Children.Clear();
                yy.Children.Add(ca);
                colorStoryboard.Begin();
                i = (i == colors.Count - 2) ? 0 : i + 1;

            }
            catch (Exception e)
            {
            }
        }

        private void StartAnimation()
        {
            colortimer.Interval = TimeSpan.FromSeconds(2.5);
            colortimer.Tick += color_Tick;
            colortimer.Start();
            DispatcherTimer backgroundCube = new DispatcherTimer();
            backgroundCube.Interval = TimeSpan.FromMilliseconds(1);
            backgroundCube.Tick += background_Tick;
            backgroundCube.Start();
        }
        int count = 0;
        private void color_Tick(object sender, object e)
        {
            if (count++ < 20)
                Func();
            else
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(StartPage));
                    colortimer.Stop();
                }
        }
        private void background_Tick(object sender, object e)
        {

            //draw3d.color.A = colors[i].A;
            //draw3d.color.B = colors[i].B;
            //draw3d.color.R = colors[i].R;
            //draw3d.color.G = colors[i].G; 
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExercisePage)); 
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
