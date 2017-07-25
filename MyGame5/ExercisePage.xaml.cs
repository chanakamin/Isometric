using Isometric.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Isometric._3DObjects;
using Windows.UI.Popups;
using Isometric.Hints;
using Isometric.Projections;
using Windows.Storage;
using System.Xml.Linq;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Isometric
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ExercisePage : Page
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

        public ExercisePage()
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
            if (e.PageState != null && e.PageState.ContainsKey("Level"))
            {
                ManagerGame.Level = int.Parse(e.PageState["level"].ToString());
            }


            // Restore values stored in app data.
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
            //e.PageState["level"] = ManagerGame.Level;
            //e.PageState["matrixSystem"] = ManagerGame.matrixSystem;
            // e.PageState["level"] = ManagerGame.Level;
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
            var parameter = e.Parameter;
            if (parameter != null && parameter.GetType() == typeof(string))
                //אם נשלח פרמטר של שם המשחק - יש לפתוח משחק שמור
                playExercise(true, (string)parameter);
            else
                playExercise(false);


        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        ProjectionsManager projectionsManager;
        ListHints listHints = new ListHints();
        Draw3D _draw3d;
        //Arrows arrows;
        private SharpDX.Toolkit.Graphics.Image captuewscreen;
        private Popup message;
        private SaveGameMessageDialog _saveMessageDialog;
        DispatcherTimer timerTime;
        DispatcherTimer timerMark;
        private async void playExercise(bool openSaveGame, string gameName = "")
        {
            saveGame.Visibility = Visibility.Visible;
            newGame.Visibility = Visibility.Visible;
            if (timerMark!=null)
                timerMark.Stop();
            if (timerTime != null)
                timerTime.Stop();
            _draw3d = new Draw3D();
            _draw3d.InactiveSleepTime = TimeSpan.FromMilliseconds(1000);
            _draw3d.TargetElapsedTime = TimeSpan.FromMilliseconds(100);
            _draw3d.IsFixedTimeStep = true;
            _draw3d.Run(panelCube);
            getHint.IsEnabled = true;
            create.IsEnabled = true;
            delete.IsEnabled = true;
            move.IsEnabled = true;
            saveGame.IsEnabled = true;
            testPanel.Visibility = Visibility.Collapsed;
            //יצירת תרגיל חדש
            if (ManagerGame.ExerciseOrTest) ManagerGame.Type = false;
            await ManagerGame.createNewExercise(openSaveGame, gameName);
            if (ManagerGame.Type)//היטלים ממבנה
            {
                getHint.IsEnabled = false;
                create.IsEnabled = false;
                delete.IsEnabled = false;
                move.IsEnabled = false;
                saveGame.IsEnabled = false;

                getHintPanel.Visibility = Visibility.Collapsed;
                //Buttons3D.Visibility = Visibility.Collapsed;////////////////
                //הצגת ההטלים
                GridOfProjections.Style = App.Current.Resources["SkewGrid"] as Style;
                projectionsManager = new ProjectionsManager(ManagerGame.matrixSystem.matrix, true);
                projectionsManager.setCanvases(ProjectionX, ProjectionY, ProjectionZ);
                //projectionsManager.draw();//////////////////////////////////////////////////////////////////
                //הצגת סוג התרגיל והרמה שנבחרה
                Description_3D.Visibility = Visibility.Collapsed;
                Description_2D.Visibility = Visibility.Visible;
                LevelText2D.Text = new string[3] { "מתחיל", "מתקדם", "אלוף" }[ManagerGame.Level];
                ProjectionsManager.onExercisEnd = ExerciseEnd;
            }
            else//מבנה מהיטלים
            {
                //הצגת האלמנטים הנדרשים לסוג זה של משחק והסתרת אלו שלא נדרשים
                Buttons3D.Visibility = Visibility.Visible;
                projectionsManager = new ProjectionsManager(ManagerGame.matrixSystem.matrix);
                projectionsManager.setCanvases(ProjectionX, ProjectionY, ProjectionZ);
                projectionsManager.draw();
                Description_2D.Visibility = Visibility.Collapsed;
                Description_3D.Visibility = Visibility.Visible;
                LevelText3D.Text = new string[3] { "מתחיל", "מתקדם", "אלוף" }[ManagerGame.Level];
                if (ManagerGame.ExerciseOrTest)
                {
                    saveGame.Visibility = Visibility.Collapsed;
                    newGame.Visibility = Visibility.Collapsed;
                    projectionsManager.draw(Color.FromArgb(255, 0, 7 * 16 + 15, 6 * 16 + 10));//007f6a
                    getHint.IsEnabled = false;
                    getHintPanel.Visibility = Visibility.Collapsed;
                    testPanel.Visibility = Visibility.Visible;
                    ManagerGame.onExercisEnd = TestEnd;
                    //הפעלת הטיימרים
                    timerMark = new DispatcherTimer();
                    timerMark.Interval = TimeSpan.FromSeconds(0.7);
                    timerMark.Tick += timerMark_Tick;
                    timerMark.Start();
                    timerTime = new DispatcherTimer();
                    timerTime.Interval = TimeSpan.FromSeconds(1);
                    timerTime.Tick += timerTime_Tick;
                    timerTime.Start();
                    //חישוב הזמן למבחן ע"פ הרמה שנבחרה
                    TestTime = (5 - ManagerGame.Level * 2) * 60 * 1;
                    pageTitle.Text = "מבחן";// +new string[3] { "מתחיל", "מתקדם", "אלוף" }[ManagerGame.Level];
                    Description_2D.Visibility = Visibility.Collapsed;
                    Description_3D.Visibility = Visibility.Visible;
                    LevelText3D.Text = new string[3] { "מתחיל", "מתקדם", "אלוף" }[ManagerGame.Level];
                }
                else
                {
                    //יצירת מערך  רמזים
                    listHints.Create(ManagerGame.matrixSystem);
                    ManagerGame.onExercisEnd = ExerciseEnd;
                }
            }
        }

        /// <summary>
        /// סיום מבחן
        /// </summary>
        /// <param name="succsed">הצלחה/כשלון</param>
        private async void TestEnd(bool succsed)
        {
            try
            {
                timerMark.Stop();
                timerTime.Stop();
                if (succsed)
                {
                    //סיום בהצלחה -> שמירת הישג
                    ShowMessage("שמור את השגך", TestEndCB);
                }
                else
                {
                    // סיום בכישלון
                    //הצגת הודעה מתאימה וחזרה לדפ הבית
                    MessageDialog ms = new MessageDialog("", "גיים אובר");
                    await ms.ShowAsync();
                    if (Frame != null)
                        Frame.Navigate(typeof(StartPage));
                }
            }
            catch (Exception ex)
            {

            }
        }

        //סיום מבחן בעת לחיצה על אישור שמירת הישג
        private async void TestEndCB(object sender, RoutedEventArgs e)
        {
            message.IsOpen = false;
            //שמירת הערכים המתאימים
            XElement element = new XElement("item", new XAttribute("name", _saveMessageDialog.UserName), new XAttribute("gameName", _saveMessageDialog.GameName), new XAttribute("mark", ManagerGame.mark));
            //פתיחת קובץ הxml
            XDocument Document = new XDocument();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                var file = await storageFolder.GetFileAsync("markList.xml");
                if (file != null)
                {
                    //אם הקובץ קיים קריאתו לאובייקט ומחיקת הקובץ
                    Document = XDocument.Load(file.Path.ToString());
                    await file.DeleteAsync();
                }
            }
            catch (Exception ex)
            {
                //אם קובץ לא קיים יצירת אובייקט חדש
                Document.Add(new XElement("root"));
            }
            //הוספת האלמנט לאובייקט
            Document.Element("root").Add(element);
            //שמירה לקובץ
            Stream s = await storageFolder.OpenStreamForWriteAsync("markList.xml", new CreationCollisionOption());
            Document.Save(s);
            //העברה לדף ההשגים
            if (this.Frame != null)
                this.Frame.Navigate(typeof(StatisticsPage), Document);
        }

        int TestTime;
        //הצגת הזמן שנשאר למבחן 
        private void timerTime_Tick(object sender, object e)
        {
            //אם נגמר הזמן
            if (TestTime-- <= 0)
                //המבחן הסתיים בכשלון
                TestEnd(false);
            textBlockTimer.Text = ConvertToTime(TestTime);

        }
        //במבחן  
        private void timerMark_Tick(object sender, object e)
        {
            //עדכון הציון על פי הזמן שעבר
            ManagerGame.mark -= 23;
            textBlockMark.Text = ManagerGame.mark.ToString();

        }
        //המרת מספר לפורמט של זמן
        private string ConvertToTime(int pastTime)
        {
            var s1 = "00" + pastTime % 60;
            var s2 = "00" + pastTime / 60;
            return s2.Substring(s2.Length - 2) + ":" + s1.Substring(s1.Length - 2);
        }

        //סיום תרגיל
        public async void ExerciseEnd(bool success)
        {
            //אין תרגיל מופעל
            ManagerGame.ActiveExercise = false;
            //מעבר לדף הנצחון
            if (this.Frame != null)
                this.Frame.Navigate(typeof(SuccessedPage));
        }

        //קריאה לפעולה במבנה התלת מימדי
        private void pageRoot_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //מאופשר רק במבנה מהיטלים ורק כשיש משחק מופעל
            if (!ManagerGame.Type && ManagerGame.ActiveExercise)
            {
                //אם המקש הנוכחי ברשימת מקשי החיצים 
                //הפעלת הפונקציה המתאימה על פי הדלגייט
                //הכיוון על פי המקש
                switch (ManagerGame.arrowsKeys.FirstOrDefault(k => k.Value == e.Key.ToString()).Key)
                {
                    case "Left": ManagerGame.CommandCube(eDimension.X, true); break;
                    case "Right": ManagerGame.CommandCube(eDimension.X, false); break;
                    case "Up": ManagerGame.CommandCube(eDimension.Y, true); break;
                    case "Down": ManagerGame.CommandCube(eDimension.Y, false); break;
                    case "Backward": ManagerGame.CommandCube(eDimension.Z, false); break;
                    case "Forward": ManagerGame.CommandCube(eDimension.Z, true); break;
                    default:
                        break;
                }
            }
        }
        //הפעלת הםונקציה המתאימה על פי שם הפקד
        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string name = "";
            if (sender is Button)
                name = (sender as Button).Name;
            else if (sender is Grid)
                name = (sender as Grid).Name;
            switch (name)
            {
                case "create": ManagerGame.CommandCube = ManagerGame.CreateLine; break;
                case "delete": ManagerGame.CommandCube = ManagerGame.DeleteLine; break;
                case "move": ManagerGame.CommandCube = ManagerGame.Movement; break;
                case "getHint": GetHint(); break;
                case "saveGame": SaveGame(); break;
                case "_buttonShowArrowsKeys": ShowArrowsKeys(); break;
                case "isometric": ShowIsometric(); break;
                case "attach": Attach(); break;
                case "noAttach": NoAttach(); break;
                case "rotate120": Rotate120(); break;
                case "getHintPanel": GetHint(); break;
                case "newGame": NewGame();
                    break;
            }

        }

        private void NewGame()
        {
            ManagerGame.ActiveExercise = false;
            numHintsShow = 0;
            textHint.Text = "";
            texthintCount.Text = numHintsShow.ToString();
            playExercise(false);
        }


     
        private void RotationAnimation_Completed(object sender, object e)
        {
            IsometricShow.Visibility = (IsometricShow.Visibility.Equals(Visibility.Collapsed)) ? Visibility.Visible : Visibility.Collapsed;
        }


        //הצגת הערכים של מקשי החיצים בחלון קופץ
        private void ShowArrowsKeys()
        {
            var grid = _popupArrowsKeys.Child as Grid;
            foreach (StackPanel item in (grid.Children[0] as StackPanel).Children)
            {
                var tb = item.Children[1] as TextBlock;
                tb.Text = ManagerGame.arrowsKeys[tb.Name.Split('_')[1]];
            }
            _popupArrowsKeys.IsOpen = true;
        }


        #region saveGame//שמירת משחק
        public void SaveGame()
        {
            captuewscreen = _draw3d.SaveAsImage();
            ShowMessage("שמירת משחק", SaveGameCB);
        }
        /// <summary>
        /// הצגת הודעה מותאמת
        /// </summary>
        /// <param name="title">כותרת להודעה</param>
        /// <param name="CB">פונקציה שתופעל בסיום הצגת ההודעה בעת לחיצה על אישור</param>
        private void ShowMessage(string title, RoutedEventHandler CB)
        {
            message = new Popup();

            _saveMessageDialog = new SaveGameMessageDialog();
            //הצבת הפונקציה שתופעל בסיום ההודעה
            _saveMessageDialog.OnSave += CB;
            //הצגת הכותרת המתאימה
            _saveMessageDialog.Header = title;
            message.Child = _saveMessageDialog;
            //לאפשר יציאה אוטמטית בלחיצה בחוץ
            message.IsLightDismissEnabled = true;
            //מיקום ההודעה במקום המתאים
            message.Width = _saveMessageDialog.Width;
            message.Height = _saveMessageDialog.Height;
            //message.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width / 2 + _saveMessageDialog.Width / 2);
            //message.SetValue(Canvas.TopProperty, Window.Current.Bounds.Height / 2 - _saveMessageDialog.Height / 2);
            message.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width / 2 + _saveMessageDialog.Width / 2);
            message.SetValue(Canvas.TopProperty, 200);
            message.Closed += message_Closed;
            //עמעום התצוגה מאחור
            this.Content.Opacity = 0.6;
            OnSwapChainCubePanel.Visibility = Visibility.Visible;
            this.BottomAppBar.IsOpen = false;
            this.TopAppBar.IsOpen = false;
            message.IsOpen = true;
        }
        //בעת עזיבת ההודעה החזרת המסך למצב תצוגה רגיל
        private void message_Closed(object sender, object e)
        {
            this.Content.Opacity = 1;
            OnSwapChainCubePanel.Visibility = Visibility.Collapsed;

        }
        private void message_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Content.Opacity = 1;
            OnSwapChainCubePanel.Visibility = Visibility.Collapsed;
        }
        //הפונקציה נקראת בשמירת משחק לאחר הקשה על לחצן האיור
        private async void SaveGameCB(object sender, RoutedEventArgs e)
        {
            //שליפת שם המשתמש
            string Name = _saveMessageDialog.UserName;
            //שליפת שם המשחק
            string GameName = _saveMessageDialog.GameName;
            //סגירת ההודעה
            message.IsOpen = false;
            //  רגילהחזרת המסך למצב תצוגה 
            this.Content.Opacity = 1;
            //תקיית האפליקציה
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //כתיבה לקובץ 
            Stream s = await storageFolder.OpenStreamForWriteAsync(GameName + ".jpg", new CreationCollisionOption());
            //שמירת המבנה התלת מימדי כקובץ תמונה
            captuewscreen.Save(s, SharpDX.Toolkit.Graphics.ImageFileType.Wmp);
            //xmlשמירת המשחק בקובץ 
            ManagerGame.ToXml(Name, GameName, panelCube);

        }
        #endregion
        #region Hint //הצגת רמז
        int numHintsShow = 0;
        Hint h;
        async private void GetHint()
        {
            //קבלת רמז רלוונטי מרשימת ההטלים
            h = listHints.getHint();
            if (h != null)
            {
                //הצגת הרז בהיטלים
                //הצגת תאור הרמז שליפה מאוסף תאורי הרמזים על פי קוד הרמז
                textHint.Text = Global.DescreptionHints[h.idDescription] + ".";
                //הצגת מספר הרמזים שהוצגו
                numHintsShow++;
                texthintCount.Text = numHintsShow.ToString();

            }
            else
            {//במקרה שאין רמז רלוונטי להצגה
                MessageDialog md = new MessageDialog("מצטערים אין באפשרותינו לעזור לך  באפשרותך לחזור לדף הלימוד ", "הודעה");
                await md.ShowAsync();
            }
        }
        private void drawHint_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            draw_Hint();
        }
        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GetHint();
        }
        private void drawHint_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

            deleteHint();
        }
        private void TextBlock_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

            deleteHint();
        }
        private void draw_Hint()
        {
            if (h != null)
                projectionsManager.showHint(h);
        }
        private void deleteHint()
        {
            projectionsManager.draw();
        }
        private void TextBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            draw_Hint();
        }

        #endregion
        #region projectionTasks //משימות ההיטלים

        //הצגת האיזומטריה של ההיטלים
        private void ShowIsometric()
        {
            if(attach.Visibility == Visibility.Collapsed)
            {
                    NoAttach();
                    b = false;
            }
            IsoStoryboard.Stop();
            if (b)
            {
                RotationAnimation.From = 90;
                RotationAnimation.To = 180;
            }
            else
            {
               // NoAttach();///////////////
                RotationAnimation.From = 180;
                RotationAnimation.To = 90;
            }
            IsoStoryboard.Begin();
            b = !b;
        }
        bool b = false;
        public bool IsAttaching { get; set; }
        //הצמדת ההיטלים
        //הצגתם במבנה של קוביה כעזרה למשתמש
        private void Attach()
        {
            ProjectionZ.ClearValue(RenderTransformProperty);//////////////////
            ProjectionX.Style = App.Current.Resources["PX_Attaching"] as Style;
            ProjectionY.Style = App.Current.Resources["PY_Attaching"] as Style;
            ProjectionZ.Style = App.Current.Resources["PZ_Attaching"] as Style;
            IsometricShow.Visibility = Visibility.Collapsed;
            attach.Visibility = Visibility.Collapsed;
            noAttach.Visibility = Visibility.Visible;

        }
        //ביטול הצמדת ההיטלים
        private void NoAttach()
        {
            ProjectionX.Style = App.Current.Resources["PX_United"] as Style;
            ProjectionY.Style = App.Current.Resources["PY_United"] as Style;
            ProjectionZ.Style = App.Current.Resources["PZ_United"] as Style;
            attach.Visibility = Visibility.Visible;
            noAttach.Visibility = Visibility.Collapsed;
        }

        bool Xs;
        bool Ys;
        bool Zs;
        //הגדלת היטל בעת מעבר העכבר עליו
        //והחזרה למצב קודם בעת יציאה

        private void ProjectionX_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Xs = (ProjectionX.Style == App.Current.Resources["PX_Attaching"] as Style);
            var g = (ProjectionX.Parent as Grid);
            g.Children.Remove(ProjectionX);
            g.Children.Add(ProjectionX);
            ProjectionX.Style = App.Current.Resources["PX_UnitedSkew"] as Style;
        }
        private void ProjectionX_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (Xs)
                ProjectionX.Style = App.Current.Resources["PX_Attaching"] as Style;
            else
                ProjectionX.Style = App.Current.Resources["PX_United"] as Style;
        }
        private void ProjectionY_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Ys = (ProjectionY.Style == App.Current.Resources["PY_Attaching"] as Style);
            var g = (ProjectionY.Parent as Grid);
            g.Children.Remove(ProjectionY);
            g.Children.Add(ProjectionY);
            ProjectionY.Style = App.Current.Resources["PY_UnitedSkew"] as Style;
        }
        private void ProjectionY_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (Ys)
                ProjectionY.Style = App.Current.Resources["PY_Attaching"] as Style;
            else
                ProjectionY.Style = App.Current.Resources["PY_United"] as Style;
        }
        private void ProjectionZ_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Zs = (ProjectionZ.Style == App.Current.Resources["PZ_Attaching"] as Style);
            var g = (ProjectionZ.Parent as Grid);
            g.Children.Remove(ProjectionZ);
            g.Children.Add(ProjectionZ);
            ProjectionZ.Style = App.Current.Resources["PZ_UnitedSkew"] as Style;
        }
        private void ProjectionZ_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (Zs)
                ProjectionZ.Style = App.Current.Resources["PZ_Attaching"] as Style;
            else
                ProjectionZ.Style = App.Current.Resources["PZ_United"] as Style;
        }

        #endregion
        //החזרת המבנה התלת ממדי לצורת הסיבוב הדפולטיבי
        public void Rotate120()
        {
            Global.World = Global.defaultWorld;
            // Global.World= ApplicationData.Current.RoamingSettings.Values["angle120"] as SharpDX.Matrix;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string angle120 = "angle120";
            SharpDX.Matrix matrix = Global.World;
            ApplicationDataContainer Roaminigsetting = ApplicationData.Current.RoamingSettings;
            // Roaminigsetting.Values[angle120] = matrix.;

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
