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
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Isometric
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class InformationPage : Page
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


        public InformationPage()
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

        //private void Button_concept_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    string selectedValueName = "";
        //    Button btn_sender = (sender as Button);
        //    var senderName = btn_sender.Name.Split('_');
        //    if (senderName[0] == "ButtonZoomOut")
        //        selectedValueName = senderName[1];
        //    else
        //        if ((btn_sender.Parent is StackPanel))
        //            selectedValueName = (btn_sender.Parent as StackPanel).Name.Split('_')[1];
        //    selectedValueName = "Grid_" + selectedValueName;
        //    zoom.IsZoomedInViewActive = true;
        //    var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
        //    flipView.SelectedValue = selectedGrid;
        //    ListBox listBox = selectedGrid.Children.Where(c => c is ListBox).First() as ListBox;
        //    if (senderName[1] != "Title" || senderName[0] == "ButtonZoomOut")
        //        listBox.SelectedIndex = 0;
        //    else
        //    {
        //        //var selectedButton = "option_" + senderName[1];
        //        var selectedButton = senderName[1] + "_" + senderName[2];
        //        listBox.SelectedItem = listBox.Items.Where(i => (i as Button).Name == selectedButton).First();
        //    }

        //    //foreach (var item in flipView.Items)
        //    //{
        //    //    if (item.GetType() == typeof(Grid) && (item as Grid).Name == "grid_" + ((sender as Button).Parent as StackPanel).Name.Split('_')[1])
        //    //        flipView.SelectedValue = item;
        //    //}
        //}
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { //grid_desrption_concept_A3
            try
            {
                var selectedName = ((sender as ListBox).SelectedItem as Button).Name;//.Split('_')[1];
                var broder = ((sender as ListBox).Parent as Grid).Children.Where(c => c is Border).First() as Border;
                var canvas = broder.Child as Canvas;
                var el = canvas.Children.Where(c => c is Grid && (c as Grid).Name.Equals("grid_desrption_" + selectedName)).First();
                var d = canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1;
                Canvas.SetZIndex(el, canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1);
                //  canvas.
                //  var border = ((sender as ListBox).Parent as Grid).Children.Where(c => c is Border).First() as Border;
                //var o = this.Resources.Keys.ToArray();
                //   var b = this.Resources.Where(r => r.Key == "grid_desrption_" + selectedName).First();
                //foreach (var item in Resources)
                //{
                //    if (item.Key.Equals("grid_desrption_concept5"))
                //       border.Child = new Grid();// (item.Value as Grid);

                //     border.Child = new ;
                //     }
                //   border.Child =  b as Grid;
            }
            catch (Exception ex)
            {

            }
        }
        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedValueName = "";
            string str = sender.GetType().ToString();
            Button btn_sender = (sender as Button);
            var senderName = btn_sender.Name.Split('_');
            if (senderName[0] == "ButtonZoomOut")
                selectedValueName = senderName[1];
            else
                if ((btn_sender.Parent is StackPanel))
                    selectedValueName = (btn_sender.Parent as StackPanel).Name.Split('_')[1];
            selectedValueName = "Grid_" + selectedValueName;
            zoom.IsZoomedInViewActive = true;
            var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
            flipView.SelectedValue = selectedGrid;
            ListBox listBox = selectedGrid.Children.Where(c => c is ListBox).First() as ListBox;
            if (senderName[1] != "Title" || senderName[0] == "ButtonZoomOut")
                listBox.SelectedIndex = 0;
            else
            {
                //var selectedButton = "option_" + senderName[1];
                var selectedButton = senderName[1] + "_" + senderName[2];
                listBox.SelectedItem = listBox.Items.Where(i => (i as Button).Name == selectedButton).First();
            }

        }
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            string selectedValueName = "";
            string str = sender.GetType().ToString();
            Grid btn_sender = (sender as Grid);
            var senderName = btn_sender.Name.Split('_');
            if (senderName[0] == "ButtonZoomOut")
                selectedValueName = senderName[1];
            else
                if ((btn_sender.Parent is StackPanel))
                    selectedValueName = (btn_sender.Parent as StackPanel).Name.Split('_')[1];
            selectedValueName = "Grid_" + selectedValueName;
            zoom.IsZoomedInViewActive = true;
            var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
            flipView.SelectedValue = selectedGrid;
            ListBox listBox = selectedGrid.Children.Where(c => c is ListBox).First() as ListBox;
            if (senderName[1] != "Title" || senderName[0] == "ButtonZoomOut")
                listBox.SelectedIndex = 0;
            else
            {
                //var selectedButton = "option_" + senderName[1];
                var selectedButton = senderName[1] + "_" + senderName[2];
                listBox.SelectedItem = listBox.Items.Where(i => (i as Button).Name == selectedButton).First();
            }

        }
        private void ButtonZoomOut_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string selectedValueName = "";
            string str = sender.GetType().ToString();
            Grid rect_sender = (sender as Grid);
            var senderName = rect_sender.Name.Split('_');
            if (senderName[0] == "ButtonZoomOut")
                selectedValueName = senderName[1];
            else
                if ((rect_sender.Parent is StackPanel))
                    selectedValueName = (rect_sender.Parent as StackPanel).Name.Split('_')[1];
            selectedValueName = "Grid_" + selectedValueName;
            zoom.IsZoomedInViewActive = true;
            var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
            flipView.SelectedValue = selectedGrid;
            StackPanel stackPanel = selectedGrid.Children.Where(c => c is StackPanel).First() as StackPanel;
            //if (senderName[1] != "Title" || senderName[0] == "ButtonZoomOut")
            //  view(stackPanel,);  stackPanel.SelectedIndex = 0;
            //else
            //{
            //    //var selectedButton = "option_" + senderName[1];
            //    var selectedButton = senderName[1] + "_" + senderName[2];
            //    stackPanel.SelectedItem = stackPanel.Items.Where(i => (i as Button).Name == selectedButton).First();
            //}

        }
        private void concept_A_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedName = ((sender as Button)).Name;//.Split('_')[1];
                var broder = (((sender as Button).Parent as StackPanel).Parent as Grid).Children.Where(c => c is Border).First() as Border;
                var canvas = broder.Child as Canvas;
                var el = canvas.Children.Where(c => c is Grid && (c as Grid).Name.Equals("grid_desrption_" + selectedName)).First();
                var d = canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1;
                Canvas.SetZIndex(el, canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1);
                //  canvas.
                //  var border = ((sender as ListBox).Parent as Grid).Children.Where(c => c is Border).First() as Border;
                //var o = this.Resources.Keys.ToArray();
                //   var b = this.Resources.Where(r => r.Key == "grid_desrption_" + selectedName).First();
                //foreach (var item in Resources)
                //{
                //    if (item.Key.Equals("grid_desrption_concept5"))
                //       border.Child = new Grid();// (item.Value as Grid);

                //     border.Child = new ;
                //     }
                //   border.Child =  b as Grid;
            }
            catch (Exception ex)
            {

            }
        }
       
        private void Rect_concept_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            ((sender as Grid).Children[0] as Rectangle).Opacity = 0.83;

        }
        private void Rect_concept_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string selectedValueName = "";
            Grid Rect_sender = (sender as Grid);
            var senderName = Rect_sender.Name.Split('_');

            if ((Rect_sender.Parent is StackPanel))
                selectedValueName = (Rect_sender.Parent as StackPanel).Name.Split('_')[1];
            selectedValueName = "Grid_" + selectedValueName;
            zoom.IsZoomedInViewActive = true;
            var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
            flipView.SelectedValue = selectedGrid;//Rect_concept_E2  concept_B1
            StackPanel stackPanel = selectedGrid.Children.Where(c => c is StackPanel).First() as StackPanel;
            var consept = stackPanel.Children.Where(t => t is TextBlock && (t as TextBlock).Name == "concept_" + senderName[2]).First() as TextBlock;
            viewConcept(consept, stackPanel);
        }

        private void viewConcept(TextBlock textBlock, StackPanel stackPanel)
        {
            try
            {

                foreach (TextBlock tb in stackPanel.Children)
                {
                    tb.Foreground = textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 3 * 16 + 3, 3 * 16 + 3, 3 * 16 + 3));
                }
                var title = (stackPanel.Parent as Grid).Children[0] as TextBlock;
                textBlock.Foreground = title.Foreground;
            }
            catch (Exception)
            {


            } // textBlock.Tag = true;
            try
            {
                var selectedName = (textBlock).Name;//.Split('_')[1];
                var broder = (stackPanel.Parent as Grid).Children.Where(c => c is Border).First() as Border;
                var canvas = broder.Child as Canvas;
                var el = canvas.Children.Where(c => c is Grid && (c as Grid).Name.Equals("grid_desrption_" + selectedName)).First();
                var d = canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1;
                Canvas.SetZIndex(el, canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1);
            }
            catch (Exception)
            {

            }
        }
        private void Rect_concept_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            // (sender as Grid).Background = new SolidColorBrush(Color.FromArgb(255, 11 * 16 + 12, 12 * 16 + 2, 32 +7)); //9 * 16 + 7, 10 * 16 + 2, 16 + 9));97a219 aab246
            ((sender as Grid).Children[0] as Rectangle).Opacity = 0.6;
        }

        private void concept_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //    var gray = new SolidColorBrush(Color.FromArgb(255, 3 * 16 + 3, 3 * 16 + 3, 3 * 16 + 3));
            //    if ((sender as TextBlock).Foreground.Equals(gray))
            //        (sender as TextBlock).Foreground = new SolidColorBrush(Colors.Black);
        }
        private void concept_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //var black = new SolidColorBrush(Colors.Black);
            //if (!(sender as TextBlock).Foreground.Equals(black))
            //    (sender as TextBlock).Foreground = new SolidColorBrush(Color.FromArgb(255, 3 * 16 + 3, 3 * 16 + 3, 3 * 16 + 3));

        }

        private void concept_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var stackPanel = textBlock.Parent as StackPanel;
            viewConcept(textBlock, stackPanel);

        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

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
                        this.Frame.Navigate(typeof(SuccessedPage));
                        break;
                    case "buttonSP_Home":
                        this.Frame.Navigate(typeof(StartPage));
                        break;

                }
        }
    }

}


//using Isometric.Common;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Controls.Primitives;
//using Windows.UI.Xaml.Data;
//using Windows.UI.Xaml.Input;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Navigation;

//// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

//namespace Isometric
//{
//    /// <summary>
//    /// A basic page that provides characteristics common to most applications.
//    /// </summary>
//    public sealed partial class InformationPage : Page
//    {

//        private NavigationHelper navigationHelper;
//        private ObservableDictionary defaultViewModel = new ObservableDictionary();

//        /// <summary>
//        /// This can be changed to a strongly typed view model.
//        /// </summary>
//        public ObservableDictionary DefaultViewModel
//        {
//            get { return this.defaultViewModel; }
//        }

//        /// <summary>
//        /// NavigationHelper is used on each page to aid in navigation and 
//        /// process lifetime management
//        /// </summary>
//        public NavigationHelper NavigationHelper
//        {
//            get { return this.navigationHelper; }
//        }


//        public InformationPage()
//        {
//            this.InitializeComponent();
//            this.navigationHelper = new NavigationHelper(this);
//            this.navigationHelper.LoadState += navigationHelper_LoadState;
//            this.navigationHelper.SaveState += navigationHelper_SaveState;
//        }

//        /// <summary>
//        /// Populates the page with content passed during navigation. Any saved state is also
//        /// provided when recreating a page from a prior session.
//        /// </summary>
//        /// <param name="sender">
//        /// The source of the event; typically <see cref="NavigationHelper"/>
//        /// </param>
//        /// <param name="e">Event data that provides both the navigation parameter passed to
//        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
//        /// a dictionary of state preserved by this page during an earlier
//        /// session. The state will be null the first time a page is visited.</param>
//        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
//        {
//        }

//        /// <summary>
//        /// Preserves state associated with this page in case the application is suspended or the
//        /// page is discarded from the navigation cache.  Values must conform to the serialization
//        /// requirements of <see cref="SuspensionManager.SessionState"/>.
//        /// </summary>
//        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
//        /// <param name="e">Event data that provides an empty dictionary to be populated with
//        /// serializable state.</param>
//        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
//        {
//        }

//        #region NavigationHelper registration

//        /// The methods provided in this section are simply used to allow
//        /// NavigationHelper to respond to the page's navigation methods.
//        /// 
//        /// Page specific logic should be placed in event handlers for the  
//        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
//        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
//        /// The navigation parameter is available in the LoadState method 
//        /// in addition to page state preserved during an earlier session.

//        protected override void OnNavigatedTo(NavigationEventArgs e)
//        {
//            navigationHelper.OnNavigatedTo(e);
//        }

//        protected override void OnNavigatedFrom(NavigationEventArgs e)
//        {
//            navigationHelper.OnNavigatedFrom(e);
//        }

//        #endregion

//        private void Button_concept_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            string selectedValueName="";
//            Button btn_senter = (sender as Button);
//             var senderName=btn_senter.Name.Split('_');
//                if(senderName[0]=="ButtonZoomOut")
//                    selectedValueName=senderName[1];
//                else 
//                    if((btn_senter.Parent is StackPanel))
//                        selectedValueName = (btn_senter.Parent as StackPanel).Name.Split('_')[1];
//                selectedValueName = "Grid_" + selectedValueName;
//            zoom.IsZoomedInViewActive = true;
//           var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
//            flipView.SelectedValue=selectedGrid;
//               ListBox listBox = selectedGrid.Children.Where(c => c is ListBox).First() as ListBox;
//            if (senderName[1] != "Title" || senderName[0]=="ButtonZoomOut")
//                listBox.SelectedIndex=0;
//          else
//            {
//                var selectedButton = "option_"+senderName[1];
//                listBox.SelectedItem = listBox.Items.Where(i => (i as Button).Name == selectedButton).First();
//            }

//            //foreach (var item in flipView.Items)
//       //{
//       //    if (item.GetType() == typeof(Grid) && (item as Grid).Name == "grid_" + ((sender as Button).Parent as StackPanel).Name.Split('_')[1])
//       //        flipView.SelectedValue = item;
//       //}
//        }

//        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            try
//            {
//                var selectedName = ((sender as ListBox).SelectedItem as Button).Name.Split('_')[1];
//                var canvas = ((sender as ListBox).Parent as Grid).Children.Where(c => c is Canvas).First() as Canvas;
//                var el =  canvas.Children.Where(c => c is Grid && (c as Grid).Name.Equals("grid_desrption_" + selectedName)).First();
//                var d = canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1;
//                Canvas.SetZIndex(el,canvas.Children.Max(c=>Canvas.GetZIndex(c))+1);
//              //  canvas.
//              //  var border = ((sender as ListBox).Parent as Grid).Children.Where(c => c is Border).First() as Border;
//                //var o = this.Resources.Keys.ToArray();
//             //   var b = this.Resources.Where(r => r.Key == "grid_desrption_" + selectedName).First();
//                //foreach (var item in Resources)
//                //{
//                //    if (item.Key.Equals("grid_desrption_concept5"))
//                //       border.Child = new Grid();// (item.Value as Grid);

//               //     border.Child = new ;
//           //     }
//            //   border.Child =  b as Grid;
//            }
//            catch(Exception ex)
//            {

//            }
//            }
//    }
//}

//using Isometric.Common;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
//using Windows.UI;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Controls.Primitives;
//using Windows.UI.Xaml.Data;
//using Windows.UI.Xaml.Input;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Navigation;
//using Windows.UI.Xaml.Shapes;

//// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

//namespace Isometric
//{
//    /// <summary>
//    /// A basic page that provides characteristics common to most applications.
//    /// </summary>
//    public sealed partial class InformationPage : Page
//    {

//        private NavigationHelper navigationHelper;
//        private ObservableDictionary defaultViewModel = new ObservableDictionary();

//        /// <summary>
//        /// This can be changed to a strongly typed view model.
//        /// </summary>
//        public ObservableDictionary DefaultViewModel
//        {
//            get { return this.defaultViewModel; }
//        }

//        /// <summary>
//        /// NavigationHelper is used on each page to aid in navigation and 
//        /// process lifetime management
//        /// </summary>
//        public NavigationHelper NavigationHelper
//        {
//            get { return this.navigationHelper; }
//        }


//        public InformationPage()
//        {
//            this.InitializeComponent();
//            this.navigationHelper = new NavigationHelper(this);
//            this.navigationHelper.LoadState += navigationHelper_LoadState;
//            this.navigationHelper.SaveState += navigationHelper_SaveState;
//        }

//        /// <summary>
//        /// Populates the page with content passed during navigation. Any saved state is also
//        /// provided when recreating a page from a prior session.
//        /// </summary>
//        /// <param name="sender">
//        /// The source of the event; typically <see cref="NavigationHelper"/>
//        /// </param>
//        /// <param name="e">Event data that provides both the navigation parameter passed to
//        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
//        /// a dictionary of state preserved by this page during an earlier
//        /// session. The state will be null the first time a page is visited.</param>
//        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
//        {
//        }

//        /// <summary>
//        /// Preserves state associated with this page in case the application is suspended or the
//        /// page is discarded from the navigation cache.  Values must conform to the serialization
//        /// requirements of <see cref="SuspensionManager.SessionState"/>.
//        /// </summary>
//        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
//        /// <param name="e">Event data that provides an empty dictionary to be populated with
//        /// serializable state.</param>
//        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
//        {
//        }

//        #region NavigationHelper registration

//        /// The methods provided in this section are simply used to allow
//        /// NavigationHelper to respond to the page's navigation methods.
//        /// 
//        /// Page specific logic should be placed in event handlers for the  
//        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
//        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
//        /// The navigation parameter is available in the LoadState method 
//        /// in addition to page state preserved during an earlier session.

//        protected override void OnNavigatedTo(NavigationEventArgs e)
//        {
//            navigationHelper.OnNavigatedTo(e);
//        }

//        protected override void OnNavigatedFrom(NavigationEventArgs e)
//        {
//            navigationHelper.OnNavigatedFrom(e);
//        }

//        #endregion

//        //private void Button_concept_Tapped(object sender, TappedRoutedEventArgs e)
//        //{
//        //    string selectedValueName = "";
//        //    Button btn_sender = (sender as Button);
//        //    var senderName = btn_sender.Name.Split('_');
//        //    if (senderName[0] == "ButtonZoomOut")
//        //        selectedValueName = senderName[1];
//        //    else
//        //        if ((btn_sender.Parent is StackPanel))
//        //            selectedValueName = (btn_sender.Parent as StackPanel).Name.Split('_')[1];
//        //    selectedValueName = "Grid_" + selectedValueName;
//        //    zoom.IsZoomedInViewActive = true;
//        //    var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
//        //    flipView.SelectedValue = selectedGrid;
//        //    ListBox listBox = selectedGrid.Children.Where(c => c is ListBox).First() as ListBox;
//        //    if (senderName[1] != "Title" || senderName[0] == "ButtonZoomOut")
//        //        listBox.SelectedIndex = 0;
//        //    else
//        //    {
//        //        //var selectedButton = "option_" + senderName[1];
//        //        var selectedButton = senderName[1] + "_" + senderName[2];
//        //        listBox.SelectedItem = listBox.Items.Where(i => (i as Button).Name == selectedButton).First();
//        //    }

//        //    //foreach (var item in flipView.Items)
//        //    //{
//        //    //    if (item.GetType() == typeof(Grid) && (item as Grid).Name == "grid_" + ((sender as Button).Parent as StackPanel).Name.Split('_')[1])
//        //    //        flipView.SelectedValue = item;
//        //    //}
//        //}
//        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        { //grid_desrption_concept_A3
//            try
//            {
//                var selectedName = ((sender as ListBox).SelectedItem as Button).Name;//.Split('_')[1];
//                var broder = ((sender as ListBox).Parent as Grid).Children.Where(c => c is Border).First() as Border;
//                var canvas = broder.Child as Canvas;
//                var el = canvas.Children.Where(c => c is Grid && (c as Grid).Name.Equals("grid_desrption_" + selectedName)).First();
//                var d = canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1;
//                Canvas.SetZIndex(el, canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1);
//                //  canvas.
//                //  var border = ((sender as ListBox).Parent as Grid).Children.Where(c => c is Border).First() as Border;
//                //var o = this.Resources.Keys.ToArray();
//                //   var b = this.Resources.Where(r => r.Key == "grid_desrption_" + selectedName).First();
//                //foreach (var item in Resources)
//                //{
//                //    if (item.Key.Equals("grid_desrption_concept5"))
//                //       border.Child = new Grid();// (item.Value as Grid);

//                //     border.Child = new ;
//                //     }
//                //   border.Child =  b as Grid;
//            }
//            catch (Exception ex)
//            {

//            }
//        }
//        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            string selectedValueName = "";
//            string str = sender.GetType().ToString();
//            Button btn_sender = (sender as Button);
//            var senderName = btn_sender.Name.Split('_');
//            if (senderName[0] == "ButtonZoomOut")
//                selectedValueName = senderName[1];
//            else
//                if ((btn_sender.Parent is StackPanel))
//                    selectedValueName = (btn_sender.Parent as StackPanel).Name.Split('_')[1];
//            selectedValueName = "Grid_" + selectedValueName;
//            zoom.IsZoomedInViewActive = true;
//            var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
//            flipView.SelectedValue = selectedGrid;
//            ListBox listBox = selectedGrid.Children.Where(c => c is ListBox).First() as ListBox;
//            if (senderName[1] != "Title" || senderName[0] == "ButtonZoomOut")
//                listBox.SelectedIndex = 0;
//            else
//            {
//                //var selectedButton = "option_" + senderName[1];
//                var selectedButton = senderName[1] + "_" + senderName[2];
//                listBox.SelectedItem = listBox.Items.Where(i => (i as Button).Name == selectedButton).First();
//            }

//        }
//        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            string selectedValueName = "";
//            string str = sender.GetType().ToString();
//            Grid btn_sender = (sender as Grid);
//            var senderName = btn_sender.Name.Split('_');
//            if (senderName[0] == "ButtonZoomOut")
//                selectedValueName = senderName[1];
//            else
//                if ((btn_sender.Parent is StackPanel))
//                    selectedValueName = (btn_sender.Parent as StackPanel).Name.Split('_')[1];
//            selectedValueName = "Grid_" + selectedValueName;
//            zoom.IsZoomedInViewActive = true;
//            var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
//            flipView.SelectedValue = selectedGrid;
//            ListBox listBox = selectedGrid.Children.Where(c => c is ListBox).First() as ListBox;
//            if (senderName[1] != "Title" || senderName[0] == "ButtonZoomOut")
//                listBox.SelectedIndex = 0;
//            else
//            {
//                //var selectedButton = "option_" + senderName[1];
//                var selectedButton = senderName[1] + "_" + senderName[2];
//                listBox.SelectedItem = listBox.Items.Where(i => (i as Button).Name == selectedButton).First();
//            }

//        }
//        private void ButtonZoomOut_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            string selectedValueName = "";
//            string str = sender.GetType().ToString();
//            Grid rect_sender = (sender as Grid);
//            var senderName = rect_sender.Name.Split('_');
//            if (senderName[0] == "ButtonZoomOut")
//                selectedValueName = senderName[1];
//            else
//                if ((rect_sender.Parent is StackPanel))
//                    selectedValueName = (rect_sender.Parent as StackPanel).Name.Split('_')[1];
//            selectedValueName = "Grid_" + selectedValueName;
//            zoom.IsZoomedInViewActive = true;
//            var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
//            flipView.SelectedValue = selectedGrid;
//            StackPanel stackPanel = selectedGrid.Children.Where(c => c is StackPanel).First() as StackPanel;
//            //if (senderName[1] != "Title" || senderName[0] == "ButtonZoomOut")
//            //  view(stackPanel,);  stackPanel.SelectedIndex = 0;
//            //else
//            //{
//            //    //var selectedButton = "option_" + senderName[1];
//            //    var selectedButton = senderName[1] + "_" + senderName[2];
//            //    stackPanel.SelectedItem = stackPanel.Items.Where(i => (i as Button).Name == selectedButton).First();
//            //}

//        }
//        private void concept_A_Click(object sender, RoutedEventArgs e)
//        {
//            try
//            {
//                var selectedName = ((sender as Button)).Name;//.Split('_')[1];
//                var broder = (((sender as Button).Parent as StackPanel).Parent as Grid).Children.Where(c => c is Border).First() as Border;
//                var canvas = broder.Child as Canvas;
//                var el = canvas.Children.Where(c => c is Grid && (c as Grid).Name.Equals("grid_desrption_" + selectedName)).First();
//                var d = canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1;
//                Canvas.SetZIndex(el, canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1);
//                //  canvas.
//                //  var border = ((sender as ListBox).Parent as Grid).Children.Where(c => c is Border).First() as Border;
//                //var o = this.Resources.Keys.ToArray();
//                //   var b = this.Resources.Where(r => r.Key == "grid_desrption_" + selectedName).First();
//                //foreach (var item in Resources)
//                //{
//                //    if (item.Key.Equals("grid_desrption_concept5"))
//                //       border.Child = new Grid();// (item.Value as Grid);

//                //     border.Child = new ;
//                //     }
//                //   border.Child =  b as Grid;
//            }
//            catch (Exception ex)
//            {

//            }
//        }
//        private void GridView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
//        {

//        }
//        private void Rect_concept_PointerMoved(object sender, PointerRoutedEventArgs e)
//        {
//            ((sender as Grid).Children[0] as Rectangle).Opacity = 0.83;
          
//        }
//        private void Rect_concept_Tapped(object sender, TappedRoutedEventArgs e)
//        { 
//            string selectedValueName = "";
//            Grid Rect_sender = (sender as Grid);
//            var senderName = Rect_sender.Name.Split('_');
            
//                if ((Rect_sender.Parent is StackPanel))
//                    selectedValueName = (Rect_sender.Parent as StackPanel).Name.Split('_')[1];
//            selectedValueName = "Grid_" + selectedValueName;
//            zoom.IsZoomedInViewActive = true;
//            var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
//            flipView.SelectedValue = selectedGrid;//Rect_concept_E2  concept_B1
//            StackPanel stackPanel = selectedGrid.Children.Where(c => c is StackPanel).First() as StackPanel;
//            var consept = stackPanel.Children.Where(t => t is TextBlock && (t as TextBlock).Name =="concept_"+senderName[2]).First() as TextBlock;
//            viewConcept(consept);
//        }
//        private void viewConcept(TextBlock textBlock)
//        {
//            var stackPanel = textBlock.Parent as StackPanel;
//            foreach (TextBlock tb in stackPanel.Children)
//            {
//                tb.Foreground = textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 3 * 16 + 3, 3 * 16 + 3, 3 * 16 + 3));
//            }
//            var title = (stackPanel.Parent as Grid).Children[0] as TextBlock;
//            textBlock.Foreground = title.Foreground;
//            // textBlock.Tag = true;
//            try
//            {
//                var selectedName = (textBlock).Name;//.Split('_')[1];
//                var broder = ((textBlock.Parent as StackPanel).Parent as Grid).Children.Where(c => c is Border).First() as Border;
//                var canvas = broder.Child as Canvas;
//                var el = canvas.Children.Where(c => c is Grid && (c as Grid).Name.Equals("grid_desrption_" + selectedName)).First();
//                var d = canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1;
//                Canvas.SetZIndex(el, canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1);
//            }
//            catch (Exception)
//            {

//            }   
//        }
//        private void Rect_concept_PointerExited(object sender, PointerRoutedEventArgs e)
//        {
//           // (sender as Grid).Background = new SolidColorBrush(Color.FromArgb(255, 11 * 16 + 12, 12 * 16 + 2, 32 +7)); //9 * 16 + 7, 10 * 16 + 2, 16 + 9));97a219 aab246
//            ((sender as Grid).Children[0] as Rectangle).Opacity = 0.6;
//        }

//        private void concept_PointerEntered(object sender, PointerRoutedEventArgs e)
//        {
//            //    var gray = new SolidColorBrush(Color.FromArgb(255, 3 * 16 + 3, 3 * 16 + 3, 3 * 16 + 3));
//            //    if ((sender as TextBlock).Foreground.Equals(gray))
//            //        (sender as TextBlock).Foreground = new SolidColorBrush(Colors.Black);
//        }
//         private void concept_PointerExited(object sender, PointerRoutedEventArgs e)
//        {
//            //var black = new SolidColorBrush(Colors.Black);
//            //if (!(sender as TextBlock).Foreground.Equals(black))
//            //    (sender as TextBlock).Foreground = new SolidColorBrush(Color.FromArgb(255, 3 * 16 + 3, 3 * 16 + 3, 3 * 16 + 3));
        
//        }

//        private void concept_PointerPressed(object sender, PointerRoutedEventArgs e)
//        {
//            var textBlock=sender as TextBlock;
//            viewConcept(textBlock);
           
//        }

//        private void buttonSP_PointerMoved(object sender, PointerRoutedEventArgs e)
//        {
//            (sender as StackPanel).Background = new SolidColorBrush(Color.FromArgb(255, 34, 34, 34));
//        }

//        private void buttonSP_PointerExited(object sender, PointerRoutedEventArgs e)
//        {
//            (sender as StackPanel).Background = new SolidColorBrush(Color.FromArgb(255, 68, 68, 68));
//        }

//        private void buttonSP_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            if (Frame != null)
//                switch (((StackPanel)sender).Name)
//                {
//                    case "buttonSP_Info":
//                        Frame.Navigate(typeof(InformationPage));
//                        break;
//                    case "buttonSP_Test":
//                        ManagerGame.ExerciseOrTest = true;
//                        Frame.Navigate(typeof(ExercisePage));
//                        break;
//                    case "buttonSP_Exp":
//                        ManagerGame.ExerciseOrTest = false;
//                        this.Frame.Navigate(typeof(ExercisePage));
//                        break;
//                    case "buttonSP_Home":
//                        this.Frame.Navigate(typeof(StartPage));
//                        break;

//                }
//        }

//    }
//}


////using Isometric.Common;
////using System;
////using System.Collections.Generic;
////using System.IO;
////using System.Linq;
////using System.Runtime.InteropServices.WindowsRuntime;
////using Windows.Foundation;
////using Windows.Foundation.Collections;
////using Windows.UI.Xaml;
////using Windows.UI.Xaml.Controls;
////using Windows.UI.Xaml.Controls.Primitives;
////using Windows.UI.Xaml.Data;
////using Windows.UI.Xaml.Input;
////using Windows.UI.Xaml.Media;
////using Windows.UI.Xaml.Navigation;

////// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

////namespace Isometric
////{
////    /// <summary>
////    /// A basic page that provides characteristics common to most applications.
////    /// </summary>
////    public sealed partial class InformationPage : Page
////    {

////        private NavigationHelper navigationHelper;
////        private ObservableDictionary defaultViewModel = new ObservableDictionary();

////        /// <summary>
////        /// This can be changed to a strongly typed view model.
////        /// </summary>
////        public ObservableDictionary DefaultViewModel
////        {
////            get { return this.defaultViewModel; }
////        }

////        /// <summary>
////        /// NavigationHelper is used on each page to aid in navigation and 
////        /// process lifetime management
////        /// </summary>
////        public NavigationHelper NavigationHelper
////        {
////            get { return this.navigationHelper; }
////        }


////        public InformationPage()
////        {
////            this.InitializeComponent();
////            this.navigationHelper = new NavigationHelper(this);
////            this.navigationHelper.LoadState += navigationHelper_LoadState;
////            this.navigationHelper.SaveState += navigationHelper_SaveState;
////        }

////        /// <summary>
////        /// Populates the page with content passed during navigation. Any saved state is also
////        /// provided when recreating a page from a prior session.
////        /// </summary>
////        /// <param name="sender">
////        /// The source of the event; typically <see cref="NavigationHelper"/>
////        /// </param>
////        /// <param name="e">Event data that provides both the navigation parameter passed to
////        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
////        /// a dictionary of state preserved by this page during an earlier
////        /// session. The state will be null the first time a page is visited.</param>
////        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
////        {
////        }

////        /// <summary>
////        /// Preserves state associated with this page in case the application is suspended or the
////        /// page is discarded from the navigation cache.  Values must conform to the serialization
////        /// requirements of <see cref="SuspensionManager.SessionState"/>.
////        /// </summary>
////        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
////        /// <param name="e">Event data that provides an empty dictionary to be populated with
////        /// serializable state.</param>
////        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
////        {
////        }

////        #region NavigationHelper registration

////        /// The methods provided in this section are simply used to allow
////        /// NavigationHelper to respond to the page's navigation methods.
////        /// 
////        /// Page specific logic should be placed in event handlers for the  
////        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
////        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
////        /// The navigation parameter is available in the LoadState method 
////        /// in addition to page state preserved during an earlier session.

////        protected override void OnNavigatedTo(NavigationEventArgs e)
////        {
////            navigationHelper.OnNavigatedTo(e);
////        }

////        protected override void OnNavigatedFrom(NavigationEventArgs e)
////        {
////            navigationHelper.OnNavigatedFrom(e);
////        }

////        #endregion

////        private void Button_concept_Tapped(object sender, TappedRoutedEventArgs e)
////        {
////            string selectedValueName="";
////            Button btn_senter = (sender as Button);
////             var senderName=btn_senter.Name.Split('_');
////                if(senderName[0]=="ButtonZoomOut")
////                    selectedValueName=senderName[1];
////                else 
////                    if((btn_senter.Parent is StackPanel))
////                        selectedValueName = (btn_senter.Parent as StackPanel).Name.Split('_')[1];
////                selectedValueName = "Grid_" + selectedValueName;
////            zoom.IsZoomedInViewActive = true;
////           var selectedGrid = (flipView.Items.Where(b => b is Grid && (b as Grid).Name == selectedValueName).First() as Grid);
////            flipView.SelectedValue=selectedGrid;
////               ListBox listBox = selectedGrid.Children.Where(c => c is ListBox).First() as ListBox;
////            if (senderName[1] != "Title" || senderName[0]=="ButtonZoomOut")
////                listBox.SelectedIndex=0;
////          else
////            {
////                var selectedButton = "option_"+senderName[1];
////                listBox.SelectedItem = listBox.Items.Where(i => (i as Button).Name == selectedButton).First();
////            }
     
////            //foreach (var item in flipView.Items)
////       //{
////       //    if (item.GetType() == typeof(Grid) && (item as Grid).Name == "grid_" + ((sender as Button).Parent as StackPanel).Name.Split('_')[1])
////       //        flipView.SelectedValue = item;
////       //}
////        }

////        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
////        {
////            try
////            {
////                var selectedName = ((sender as ListBox).SelectedItem as Button).Name.Split('_')[1];
////                var canvas = ((sender as ListBox).Parent as Grid).Children.Where(c => c is Canvas).First() as Canvas;
////                var el =  canvas.Children.Where(c => c is Grid && (c as Grid).Name.Equals("grid_desrption_" + selectedName)).First();
////                var d = canvas.Children.Max(c => Canvas.GetZIndex(c)) + 1;
////                Canvas.SetZIndex(el,canvas.Children.Max(c=>Canvas.GetZIndex(c))+1);
////              //  canvas.
////              //  var border = ((sender as ListBox).Parent as Grid).Children.Where(c => c is Border).First() as Border;
////                //var o = this.Resources.Keys.ToArray();
////             //   var b = this.Resources.Where(r => r.Key == "grid_desrption_" + selectedName).First();
////                //foreach (var item in Resources)
////                //{
////                //    if (item.Key.Equals("grid_desrption_concept5"))
////                //       border.Child = new Grid();// (item.Value as Grid);
                    
////               //     border.Child = new ;
////           //     }
////            //   border.Child =  b as Grid;
////            }
////            catch(Exception ex)
////            {

////            }
////            }
////    }
////}
