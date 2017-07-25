using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SharpDX;
using System.Xml.Linq;
//
using Windows.UI.Popups;
using System.IO;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;


using System.IO;
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
    //דלגייט לסוג פעולה יצירה ,מחיקה,תזוזה
    delegate void CommandCube3d(eDimension axis, bool direct);
    static class ManagerGame
    {
        #region members
        // const int Global.N = 11;
        //מטריצת מערכת קוביה מקורית
        public static MatrixBase matrixSystem { get; set; }
        //מטריצת משתמש לשמירת צעדי משתמש
        public static MatrixBase matrixUser { get; set; }
        //מיקום הסמן כעת -לתצוגה נמצא במחלקה זו כדי לעדכן קודות
        //אפשר אולי לאתחל אותו במחלקת גיים -ציור ולהשתמש כל פעם במשתנים מהמחלקה
        // יוצר בעיה כי כל פעם הוא צריך להיות מעל הקו האחרון לכיוון מסוים
        //אפשרות נוספת ליצורפה שני איקס שני הואי וכו. לתצוגה ולעדכון...

        public static Cursor cursor { get; set; }
        //public static int xCursor { get; set; }
        //public static int yCursor { get; set; }
        //public static int zCursor { get; set; }
        //א"א לאתחל בצורה רגילה כי כדי לשלוח ברף א"א להשתמש בפרופ
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
        //TypeGame true for 2D => 3D and false for 3D => 2D
        //  public static bool TypeGame { get; set; }
        #endregion
        #region c-tor
        static ManagerGame()
        {
            //הפרמטר מילוי דפולטיבי או לא
            //matrixSystem = new MatrixBase(false);
            //matrixUser = new MatrixBase(false);
            //cursor = new Cursor(0, 0, 0);
            //xCursor = 0;
            //yCursor = 0;
            //zCursor = 0;
            createNewExercise();
        }

        #endregion
        #region function
        public static void createNewExercise()
        {
            matrixSystem = new MatrixBase(true);
            matrixUser = new MatrixBase(false);
            cursor = new Cursor(0, 0, 0);
            xCursor = 0;
            yCursor = 0;
            zCursor = 0;
        }
        static bool DrawNow = false;
        public static OnExerciseEnd onExercisEnd { get; set; }
        public async static void CreateLine(eDimension axis, bool direct)
        {
            if (!DrawNow)
            {
                DrawNow = true;
                //תקינות מבחינת גבולות המערך
                if (!MatrixBase.IsValidDirection(axis, direct, XCursor, YCursor, ZCursor)) return;
                //אם בנקודת ההתחלה ישיותר מקו אחד א"א ליצור את הקו כי אז יצאו 3 קוים מנקודה אחת
                if (matrixUser.SumNeighborsLines(XCursor, YCursor, ZCursor) > 1) return;
                //אם המקום הרצוי ריק
                if (matrixUser.IfFill(axis, direct, XCursor, YCursor, ZCursor)) return;
                //ואם אפשר לצייר מבחינת סיום הקו (לא יכולים להיות 3 קוים באותה נקודה
                if (matrixUser.SumLines(axis, direct, XCursor, YCursor, ZCursor) > 1) return;
                //מאתחל את הדלגייט שייקדם כל פעם בצורה הרצויה כדי לא להצטרך לעשות איף
                IncDec incDecPos = MatrixBase.SetDelegateIncDec(axis, direct);
                matrixUser[xCursor, yCursor, zCursor] = true;
                int x, y, z;
                for (int i = 0; i < Global.N / 2; i++)
                {
                    //lock ("222")
                    //{
                    //אם משתמשים בפקודה זו מחכ עד שגומר ורק אז מצייר הכל 
                    //  Utilities.Sleep(new TimeSpan(10000000));
                    //בפקודה זו אפשר ליצור קו נוסף באמצע(נוצרות מדרגות)
                    await Task.Delay(500);// איך נועלים?
                    // Thread.Sleep(10000);
                    matrixUser[xCursor, yCursor, zCursor] = true;
                    incDecPos(ref xCursor, ref yCursor, ref zCursor);
                    //הסמן נמצא במקום הבא..
                    cursor.setPosition(xCursor, yCursor, zCursor);
                    //}
                    matrixUser[xCursor, yCursor, zCursor] = true;
                    //בפעם האחרונה מקדם רק בשביל הסמן המצויר
                    x = XCursor; y = YCursor; z = ZCursor;
                    incDecPos(ref x, ref y, ref z);
                    cursor.setPosition(x, y, z);
                }
                DrawNow = false;
                if (matrixSystem.Equals(matrixUser))
                    if (onExercisEnd != null) onExercisEnd(true);
            }
        }
        public async static void DeleteLine(eDimension axis, bool direct)
        {
            if (!DrawNow)
            {
                DrawNow = true;
            if (!MatrixBase.IsValidDirection(axis, direct, XCursor, YCursor, ZCursor)) return;
            if (!matrixUser.IfFill(axis, direct, XCursor, YCursor, ZCursor)) return;
            IncDec incDecPos = MatrixBase.SetDelegateIncDec(axis, direct);
            matrixUser[xCursor, yCursor, zCursor] = false;
                for (int i = 0; i < Global.N / 2 - 1; i++)
                {
                    //הסמן נמצא במקום האחרון שנמחק
                    cursor.setPosition(xCursor, yCursor, zCursor);
                    await Task.Delay(500);// איך נועלים?
                    // Thread.Sleep(10000);

                    incDecPos(ref xCursor, ref yCursor, ref zCursor);
                    matrixUser[xCursor, yCursor, zCursor] = false;
                }
                int x = XCursor, y = YCursor, z = ZCursor;
                cursor.setPosition(x, y, z);
                incDecPos(ref xCursor, ref yCursor, ref zCursor);
                //אם למחוק את האחרון ? אם יש לו עוד שכנים לא מוחקים!
                if (matrixUser.SumNeighborsLines(XCursor, YCursor, ZCursor) > 0)
                {
                    matrixUser[xCursor, yCursor, zCursor] = false;
                    // למה צריך לקדם??
                    incDecPos(ref x, ref y, ref z);
                    cursor.setPosition(x, y, z);
                }
                DrawNow = false;
                if (matrixSystem.Equals(matrixUser))
                    if (onExercisEnd != null) onExercisEnd(true);
            }
        }
        public async static void Movement(eDimension axis, bool direct)
        {
            if (!DrawNow)
            {
                DrawNow = true;
                if (!MatrixBase.IsValidDirection(axis, direct, XCursor, YCursor, ZCursor)) return;

                IncDec incDecPos = MatrixBase.SetDelegateIncDec(axis, direct);

                for (int i = 0; i < Global.N / 2; i++)
                {
                    await Task.Delay(500);// איך נועלים?
                    // Thread.Sleep(10000);
                    incDecPos(ref xCursor, ref yCursor, ref zCursor);
                    cursor.setPosition(xCursor, yCursor, zCursor);
                }
                DrawNow = false;
            }
        }

        public async static void ToXml(string UserName, string GameName, UIElement screen)
        {

            XDocument Document = new XDocument();

            XElement Root = new XElement("root");
            Document.Add(Root);
            Root.Add(new XElement("Details", new XElement("UserName", UserName),
                                                 new XElement("Date", DateTime.Today),
                                                 new XElement("GameName", GameName)),
                                                 new XElement("RotateX", DateTime.Today),
                                                 new XElement("RotateY", DateTime.Today),
                                                 new XElement("RotateZ", DateTime.Today),
                                                 new XElement("Date", DateTime.Today));
            //  XAttribute s = Root.Attributes().Where(aa => aa.Name != "vfgf").FirstOrDefault();
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
                        {//כדאי להפוך למשהו אחר חוץ מאטריביוט 
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
            //Check if Corremt Game Name already exists - If so...
            Stream s = await storageFolder.OpenStreamForWriteAsync(GameName + ".xml", new CreationCollisionOption());
            Document.Save(s);
            //  PrintScreenshot(screen, GameName+"_img");
        }

        async static void PrintScreenshot(UIElement element_to_print, string FileName)
        {
            // Render to an image at the current system scale and retrieve pixel contents
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(element_to_print);
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

            // Prompt the user to select a file
            // var saveFile = await savePicker.PickSaveFileAsync();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //Stream s = await storageFolder.OpenStreamForWriteAsync(GameName, new CreationCollisionOption());
            StorageFile saveFile = await storageFolder.CreateFileAsync(FileName + ".png");
            // Verify the user selected a file
            if (saveFile == null)
                return;

            // Encode the image to the selected file on disk
            using (var fileStream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);

                encoder.SetPixelData(
               BitmapPixelFormat.Bgra8,
               BitmapAlphaMode.Ignore,
               (uint)renderTargetBitmap.PixelWidth,
               (uint)renderTargetBitmap.PixelHeight,
               DisplayInformation.GetForCurrentView().LogicalDpi,
               DisplayInformation.GetForCurrentView().LogicalDpi,
               pixelBuffer.ToArray());

                await encoder.FlushAsync();
            }
        }

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
        async public static void OpenSavedGame(string NameGame)
        {
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
                            matrixSystem[x, y, z] = (bool)itemZ.Attribute("User");
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