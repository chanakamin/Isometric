using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SharpDX;
using System.Xml.Linq;
using Windows.UI.Popups;
using System.IO;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Isometric._3DObjects;

namespace Isometric
{
    //דלגייט לפונקציה שמקדת את אחד האופרטרים
    delegate void IncDec(ref int x, ref int y, ref int z);
    // בקוביה התלת מימדית דלגייט לסוג פעולה יצירה ,מחיקה,תזוזה
    delegate void CommandCube3d(eDimension axis, bool direct);
    static class ManagerGame
    {
        #region members
        //מטריצת מערכת קוביה מקורית
        public static LogicalMatrix matrixSystem { get; set; }
        //מטריצת משתמש לשמירת צעדי משתמש
        public static LogicalMatrix matrixUser { get; set; }
        // מיקום הסמן -משמש לציור הסמן בקוביה
        public static Cursor cursor { get; set; }

        private static int xCursor;

        public static int XCursor
        {
            get { return ManagerGame.xCursor; }
            set { ManagerGame.xCursor = value; }
        }
        private static int yCursor;

        public static int YCursor
        {
            get { return ManagerGame.yCursor; }
            set { ManagerGame.yCursor = value; }
        }
        private static int zCursor;

        public static int ZCursor
        {
            get { return ManagerGame.zCursor; }
            set { ManagerGame.zCursor = value; }
        }
        //כדי לא לאפשר למשתמש ליצור שני קוים יחד או למחוק
        //אבל לאפשר את המשך התוכנית בצורה סינכרונית 
        //המשתנה הבוליאני הבא משמש לבדיקה האם יש קו שנוצר או נמחק 
        //ואם כן אין אשרות לבנות קו או למחוק
        static bool DrawNow = false;
        // דלגייט להפעלת אירוע סיום משחק
        public static OnExerciseEnd onExercisEnd { get; set; }

        public static CommandCube3d CommandCube = ManagerGame.CreateLine;

        private static int _level = 1;

        public static int Level
        {
            get { return ManagerGame._level; }
            set
            {
                if (ManagerGame._level != value)
                {
                    ManagerGame._level = value;
                    ManagerGame.ConfigurationChange();
                }
            }
        }
        //שלב ברירת מחדל 1
        //false-2->3 בנייה מהיטליםtrue-3->2 היטלים מבניה
        private static bool _type;

        public static bool Type
        {
            get { return ManagerGame._type; }
            set
            {
                if (ManagerGame._type != value)
                {
                    ManagerGame._type = value;
                    ManagerGame.ConfigurationChange();
                }
            }
        }
        //false for exec true for test
        private static bool _exerciseOrTest = false;

        public static bool ExerciseOrTest
        {
            get { return ManagerGame._exerciseOrTest; }
            set
            {
                if (ManagerGame._exerciseOrTest != value)
                {
                    ManagerGame._exerciseOrTest = value;
                    ManagerGame.ConfigurationChange();
                }
            }
        }


        public static bool ActiveExercise = false;//יש כרגע משחק פעיל
        public static int mark;

        public static Dictionary<string, string> arrowsKeys = new Dictionary<string, string>(){
            {"Up","U"},
            {"Down","D"},
            {"Right","R"},
            {"Left","L"},
            {"Backward","B"},
            {"Forward","F"},
        };
        #endregion

        #region c-tor

        static ManagerGame()
        {
            createNewExercise(false);

            //                    Windows.Storage.ApplicationDataContainer roamingSettings =
            //Windows.Storage.ApplicationData.Current.RoamingSettings;
            //foreach (var value in ManagerGame.arrowsKeys.Values)
            //{
            for (int i = 0; i < ManagerGame.arrowsKeys.Values.Count; i++)
            {
                var value = ManagerGame.arrowsKeys.Keys.ToList()[i];

                //if (roamingSettings.Values.ContainsKey(value))
                //{
                //    ManagerGame.arrowsKeys[value] = roamingSettings.Values[value].ToString();
                //}
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(value))
                {
                    ManagerGame.arrowsKeys[value] = ApplicationData.Current.RoamingSettings.Values[value].ToString();
                }

            }
        }

        #endregion
        //when change configuration execshould be restart to load change
        private static void ConfigurationChange()
        {
            ActiveExercise = false;
        }
        #region function
        /// <summary>
        /// יצירת משחק חדש
        /// </summary>
        public async static Task createNewExercise(bool openSaveGame = false, string gameName = "")
        {

            if (!ManagerGame.ActiveExercise || openSaveGame || ManagerGame.ExerciseOrTest)
            {
                Global.World = Global.defaultWorld;
                ManagerGame.ActiveExercise = true;
                if (openSaveGame)
                {
                    await OpenSavedGame(gameName);
                    ManagerGame.Type = false;
                }
                else
                {
                    //מטריצת מערכת מאתחלים עם נתונים
                    matrixSystem = new LogicalMatrix(true);
                    //מטריצת משתמש ריקה
                    matrixUser = new LogicalMatrix(false);

                }
                //מיקום הסמנים בתחילת 0,0,0
                mark = 1000 + 1000 * (ManagerGame.Level + 1);
                cursor = new Cursor(0, 0, 0);
                xCursor = 0;
                yCursor = 0;
                zCursor = 0;
            }
        }
        /// <summary>
        /// בניית קו בקוביה
        /// </summary>
        /// <param name="axis">הציר שעליו נבנה הקו</param>
        /// <param name="direct">כיוון עולה/יורד</param>
        public async static void CreateLine(eDimension axis, bool direct)
        {

            //תקינות מבחינת גבולות המטריצה
            if (!LogicalMatrix.IsValidDirection(axis, direct, XCursor, YCursor, ZCursor)) return;
            //בודק שאין יותר מקו אחד בנקודת ההתחלה כדי לא לאפשר יותר משני קוים מאותה נקודה
            if (matrixUser.SumNeighborsLines(XCursor, YCursor, ZCursor) > 1) return;
            //בודק אם יעד הקו הוא ריק
            if (matrixUser.IfFill(axis, direct, XCursor, YCursor, ZCursor)) return;
            //בודק את מקום סיום הקו לא לאפשר יותר משני קוים באותה נקודה
            if (matrixUser.SumLines(axis, direct, XCursor, YCursor, ZCursor) > 1) return;
            //כדי שלא נצטרך לשאול בכל פעם איזה פרמטר צריך לקדם או לחסר 
            //נשתמש בדלגייט בכדי לעדכן את הפרמטר הנדרש
            //מאתחל את הדלגייט לפונקציה המתאימה
            if (!DrawNow)
            {
                DrawNow = true;
                IncDec incDecPos = LogicalMatrix.SetDelegateIncDec(axis, direct);
                matrixUser[xCursor, yCursor, zCursor] = true;
                int x, y, z;
                for (int i = 0; i < Global.N / 2; i++)
                {
                    mark += 200;
                    //הקו נוצר חלק חלק במעין אנימציה ולא בבת אחת
                    await Task.Delay(500);
                    matrixUser[xCursor, yCursor, zCursor] = true;
                    incDecPos(ref xCursor, ref yCursor, ref zCursor);
                    //הסמן נמצא במקום הבא..
                    cursor.setPosition(xCursor, yCursor, zCursor);
                    matrixUser[xCursor, yCursor, zCursor] = true;
                    //בפעם האחרונה מקדם רק בשביל הסמן המצויר
                    x = XCursor; y = YCursor; z = ZCursor;
                    incDecPos(ref x, ref y, ref z);
                    cursor.setPosition(x, y, z);
                }
                DrawNow = false;
                //בודק אם המשחק נגמר אם כן מופעל הדלגייט של סיום משחק
                if (matrixSystem.Equals(matrixUser))
                    if (onExercisEnd != null) onExercisEnd(true);
            }
        }

        /// <summary>
        ///     מחיקת קו מהקוביה
        /// </summary>
        /// <param name="axis"> ציר עליו בנוי הקו</param>
        /// <param name="direct">כיוון עולה/יורד</param>
        public async static void DeleteLine(eDimension axis, bool direct)
        {


            //בדיקת כיוון ואורך מבחינת גבולות המטריצה
            if (!LogicalMatrix.IsValidDirection(axis, direct, XCursor, YCursor, ZCursor)) return;
            //בדיקה אם יש קו במקום הנוכחי 
            if (!matrixUser.IfFill(axis, direct, XCursor, YCursor, ZCursor)) return;
            //הצבת דלגייט קידום או חיסור
            IncDec incDecPos = LogicalMatrix.SetDelegateIncDec(axis, direct);
            matrixUser[xCursor, yCursor, zCursor] = false;
            if (!DrawNow)
            {
                DrawNow = true;
                for (int i = 0; i < Global.N / 2 - 1; i++)
                {
                    mark -= 100;
                    //הסמן נמצא במקום האחרון שנמחק
                    cursor.setPosition(xCursor, yCursor, zCursor);
                    await Task.Delay(500);
                    incDecPos(ref xCursor, ref yCursor, ref zCursor);
                    matrixUser[xCursor, yCursor, zCursor] = false;
                }
                int x = XCursor, y = YCursor, z = ZCursor;
                cursor.setPosition(x, y, z);
                incDecPos(ref xCursor, ref yCursor, ref zCursor);
                //מחיקת המקום האחון רק אם אין לו עוד שכנים
                if (matrixUser.SumNeighborsLines(XCursor, YCursor, ZCursor) > 0)
                {
                    matrixUser[xCursor, yCursor, zCursor] = false;
                    incDecPos(ref x, ref y, ref z);
                    cursor.setPosition(x, y, z);
                }
                DrawNow = false;
                //בדיקה אם המטריצות שוות המשחק הסתיים 
                //הפעלת פונקציית סיום
                if (matrixSystem.Equals(matrixUser))
                    if (onExercisEnd != null) onExercisEnd(true);
            }
        }
        /// <summary>
        /// תזוזה בקוביה
        /// </summary>
        /// <param name="axis">ציר</param>
        /// <param name="direct">כיוון עולה/יורד</param>
        public async static void Movement(eDimension axis, bool direct)
        {
            //בדיקת כיוון ואורך מבחינת גבולות המטריצה
            if (!LogicalMatrix.IsValidDirection(axis, direct, XCursor, YCursor, ZCursor)) return;
            //הצבת דלגייט קידום או חיסור
            IncDec incDecPos = LogicalMatrix.SetDelegateIncDec(axis, direct);
            if (!DrawNow)
            {
                DrawNow = true;
                for (int i = 0; i < Global.N / 2; i++)
                {
                    await Task.Delay(500);
                    incDecPos(ref xCursor, ref yCursor, ref zCursor);
                    cursor.setPosition(xCursor, yCursor, zCursor);
                }
                DrawNow = false;
            }
        }

        //new XElement("RotateX", DateTime.Today),
        //new XElement("RotateY", DateTime.Today),
        //new XElement("RotateZ", DateTime.Today),
        /// <summary>
        ///שמירת המשחק בקובץ xml
        /// </summary>
        /// <param name="UserName">שם המשתמש</param>
        /// <param name="GameName">שם המשחק</param>
        /// <param name="screen"></param>
        public async static void ToXml(string UserName, string GameName, UIElement screen)
        {

            XDocument Document = new XDocument();

            XElement Root = new XElement("root");
            Document.Add(Root);
            //נתונים כללים
            Root.Add(new XElement("Details", new XElement("UserName", UserName),
                                                 new XElement("Date", DateTime.Today),
                                                 new XElement("GameName", GameName)),
                                                 new XElement("Date", DateTime.Today));
            //מטריצת מערכת מול מטריצת משתמש
            XElement XMatrix = new XElement("Matrix");
            for (int x = 0; x < Global.N; x++)
            {
                XElement xelement = new XElement("x", new XAttribute("index", x));
                for (int y = 0; y < Global.N; y++)
                {
                    XElement yelement = new XElement("y", new XAttribute("index", y));
                    for (int z = 0; z < Global.N; z++)
                    {
                        if (matrixSystem[x, y, z] || matrixUser[x, y, z])
                        {
                            XElement element = new XElement("point", new XAttribute("index", z));
                            if (matrixSystem[x, y, z]) element.Add(new XAttribute("System", true));
                            if (matrixUser[x, y, z]) element.Add(new XAttribute("User", true));
                            yelement.Add(element);
                        }
                    }
                    if (yelement.Elements().Count() > 0) xelement.Add(yelement);
                }
                if (xelement.Elements().Count() > 0) XMatrix.Add(xelement);
            }
            Root.Add(XMatrix);

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            Stream s = await storageFolder.OpenStreamForWriteAsync(GameName + ".xml", new CreationCollisionOption());
            Document.Save(s);
        }

        //  PrintScreenshot(screen, GameName+"_img");
        //////async static void PrintScreenshot(UIElement element_to_print, string FileName)
        //////{
        //////    // Render to an image at the current system scale and retrieve pixel contents
        //////    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
        //////    await renderTargetBitmap.RenderAsync(element_to_print);
        //////    var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

        //////    // Prompt the user to select a file
        //////    // var saveFile = await savePicker.PickSaveFileAsync();
        //////    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        //////    //Stream s = await storageFolder.OpenStreamForWriteAsync(GameName, new CreationCollisionOption());
        //////    StorageFile saveFile = await storageFolder.CreateFileAsync(FileName + ".png");
        //////    // Verify the user selected a file
        //////    if (saveFile == null)
        //////        return;

        //////    // Encode the image to the selected file on disk
        //////    using (var fileStream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
        //////    {
        //////        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);

        //////        encoder.SetPixelData(
        //////       BitmapPixelFormat.Bgra8,
        //////       BitmapAlphaMode.Ignore,
        //////       (uint)renderTargetBitmap.PixelWidth,
        //////       (uint)renderTargetBitmap.PixelHeight,
        //////       DisplayInformation.GetForCurrentView().LogicalDpi,
        //////       DisplayInformation.GetForCurrentView().LogicalDpi,
        //////       pixelBuffer.ToArray());

        //////        await encoder.FlushAsync();
        //////    }
        //////}

        //public static void StartGame(bool TypeGame, string NameGame)
        //{
        //    if (NameGame != "")
        //    {
        //        OpenSavedGame(NameGame);
        //        ManagerGame.TypeGame = true;
        //    }
        //    else
        //    {
        //        ManagerGame.TypeGame = TypeGame;
        //        //if(TypeGame)
        //        matrixSystem.Init();
        //    }
        //}
        async public static Task OpenSavedGame(string NameGame)
        {
            matrixSystem = new LogicalMatrix(false);
            matrixUser = new LogicalMatrix(false);
            cursor = new Cursor(0, 0, 0);
            xCursor = 0;
            yCursor = 0;
            zCursor = 0;
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            var Files = await storageFolder.GetFilesAsync();
            var File = Files.ToList().Where(f => f.Name == NameGame).First();
            XDocument Document = XDocument.Load(File.Path.ToString());

            foreach (var itemX in Document.Descendants("Matrix").Elements("x"))
            {
                int x = (int)itemX.Attribute("index");
                foreach (var itemY in itemX.Elements("y"))
                {
                    int y = (int)itemY.Attribute("index");
                    foreach (var itemZ in itemY.Elements("point"))
                    {
                        int z = (int)itemZ.Attribute("index");
                        if (itemZ.Attribute("System") != null)
                            matrixSystem[x, y, z] = (bool)itemZ.Attribute("System");
                        if (itemZ.Attribute("User") != null)
                            matrixUser[x, y, z] = (bool)itemZ.Attribute("User");
                    }
                }
            }
        }
        #endregion
    }
}



//Document.Save(@"H:\wpf\ClientsBinding\ClientsBinding\XMLFile1.xml");
// System.IO.StringWriter str = new StringWriter();
//        Document.Save(str);
//      //  StorageFile s = new StorageFile();

//            //System.IO.StreamWriter dd = new StreamWriter();
//// //       StreamWriter sw = new StreamWriter("CDriveDirs.txt"))
//        StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

//            var dataFolder = await local.CreateFolderAsync("DataFolder",
//    CreationCollisionOption.OpenIfExists);

//// Create a new file named DataFile.txt.
//var file = await dataFolder.CreateFileAsync("DataFile.txt",
//CreationCollisionOption.ReplaceExisting);
//        File
//             System.IO.StreamWriter file1 = new System.IO.StreamWriter()
//        @"c:\temp\SerializationOverview.xml");

//// Write the data from the textbox.
////using (var s = await file.OpenStreamForWriteAsync())
////{
////    s.Write(fileBytes, 0, fileBytes.Length);
////}
//var s = await file.OpenStreamForWriteAsync();
//        s.Write()
// MessageDialog dialog;
// dialog = new MessageDialog(str.ToString());
// await dialog.ShowAsync();
//return new XElement("dd");