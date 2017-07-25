using Isometric.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class StatisticsPage : Page
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


        public StatisticsPage()
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
            if (e.NavigationParameter != null)
                loadItems(e.NavigationParameter as XDocument);
            else
                loadItems();
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
        public async void loadItems()
        {
            try
            {
                XElement element = new XElement("item", new XAttribute("name", "1234"), new XAttribute("mark", ManagerGame.mark));
                XDocument Document = new XDocument();
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                Stream s = await storageFolder.OpenStreamForWriteAsync("markList.xml", new CreationCollisionOption());
                var file = await storageFolder.GetFileAsync("markList.xml");
                if (file != null)
                    loadItems(XDocument.Load(file.Path.ToString()));
            }
            catch
            {
                if (Frame != null)
                    Frame.Navigate(typeof(StartPage));
            }
        }
        public async void loadItems(XDocument Document)
        {
            //XElement element = new XElement("item", new XAttribute("name", "1234"), new XAttribute("mark", ManagerGame.mark));
            //XDocument Document = new XDocument();
            //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Stream s = await storageFolder.OpenStreamForWriteAsync("markList.xml", new CreationCollisionOption());
            try
            {
                //var file = await storageFolder.GetFileAsync("markList.xml");

                //if (file != null)
                //{
                //Document = XDocument.Load(file.Path.ToString());
                bool b = false;
                var col = new SolidColorBrush(Colors.Gainsboro);
                var Vcol = new SolidColorBrush(Color.FromArgb(255,4*16+15,9*16,7*16+15));//007f6a 4f907f cbd8d3
                var list = Document.Descendants("item").Select(
                   x => new
                   {
                       Rect = Vcol,
                       UserName = x.Attribute("name").Value,
                       Mark = x.Attribute("mark").Value,

                   }).ToList();
                // var Clist = list.CopyTo(Clist);
                //foreach (var item in list)
                //{
                //    item.Rect =(b)?col:Vcol;
                //    b = !b;
                //}
                //להציג רק 10 ראשןנים
                int limit = 10;
                peakGridView.ItemsSource = list.OrderBy(x => int.Parse(x.Mark)).TakeWhile(a => limit-- > 0).Reverse();

                // ControlTemplate t = (peakGridView.Items.First() as ListViewItem).Template;

                //foreach (var item in peakGridView.Items)
                //{
                //    if (b)
                //        ((item as ListViewItem)).Template.SetValue(ContentControl.BackgroundProperty,new SolidColorBrush(Colors.Gainsboro));
                //    else
                //        (item as ListViewItem).Background = new SolidColorBrush(Colors.GhostWhite);
                //    b = !b;
                //}
                countTest.Text = list.Count.ToString();
                Average.Text = ((int)(list.Average(x => int.Parse(x.Mark)))).ToString();//)//..ToString();
                countTester.Text = list.GroupBy(x => x.UserName).Count().ToString();
                //    l.Count
                //var r9 = (from u in l

                //          select new { u.Attribute("name"), parent = p1.name });
            }
            // }
            catch (Exception e)
            {
                var c = "ooooo faild.........";
            }
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