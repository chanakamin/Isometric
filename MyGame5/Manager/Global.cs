using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Isometric
{
    public enum eDimension { X, Y, Z };
    public delegate  void OnExerciseEnd (bool sucssed); 
    static class Global
    {
        
        public const int N = 11;//גודל המטריצה התלת מימדית N*N*N
        //תאור הרמזים
        public static Dictionary<int, string> DescreptionHints = new Dictionary<int, string>(){
            
         //{10,"מקדימה לכל האורך אין בכלל קו"},
         //{11,"מלמעלה לכל הוחב אין בכלל קו "},
         //{12,"מהצד לכל האורך אין בכלל קו "},
         //{13,"מלמעלה לכל האורך אין בכלל קו "},
         //{14,"מהצד לכל הרוחב אין בכלל קו "},
         //{15,"מקדימה לכל הרוחב אין בכלל קו "},
         {10,"קו אנכי בהיטל y ניתן לזיהוי בהתבוננות בהיטל z , מופיע שם קו אופקי אחד בלבד  "},
         {11,"קו אופקי בהיטל z ניתן לזיהוי בהתבוננות בהיטל y , מופיע שם קו אופקי אחד בלבד"},//error
         {12,"קו אופקי בהיטל y ניתן לגילוי בהתבוננות בהיטל x , מופיע שם קו אופקי אחד בלבד"},//מהצד לכל האורך אין בכלל קו 
         {13,"קו אופקי בהיטל x ניתן לגילוי מהתבוננות מהיטל y , מופיע שם קו אופקי אחד בלבד"},
         {14,"קו אנכי בהיטל z ניתן לזיהוי בהתבוננות בהיטל x , מופיע שם קו אופקי אחד בלבד "},
         {15,"קו אנכי בהיטל x ניתן לגילוי בהתבוננות מהיבט z , מופיע שם קו אופקי אחד בלבד "},
         {20,"קו אנכי בהיטל y ניתן לזיהוי על פי התבוננות מהיטל z , אמנם מופיע שם קו אופקי נוסף אך על פי צורת הזוית ניתן לזהות את מיקום הקו "},//מקדימה לכל האורך יש קו אבל הוא שונה ולכן ברור שחייב להיות קו
         {21,"קו אופקי בהיטל z ניתן לזיהוי על פי התבוננות מהיטל y , אמנם מופיע שם קו אנכי נוסף אך על פי צורת הזוית ניתן לזהות את מיקום הקו "},
         {22,"קו אופקי בהיטל y ניתן לזיהוי על פי התבוננות מהיטל x , אמנם מופיע שם קו אופקי נוסף אך על פי צורת הזוית ניתן לזהות את מיקום הקו "},//מהצד לכל האורך יש קו אבל הוא שונה ולכן ברור שחייב להיות קו
         {23,"קו אופקי בהיטל x ניתן לזיהוי על פי התבוננות מהיטל y , אמנם מופיע שם קו אופקי נוסף אך על פי צורת הזוית ניתן לזהות את מיקום הקו "},//מלמעלה לכל האורך יש קו אבל הוא שונה ולכן ברור שחייב להיות קו
         {24,"קו אנכי בהיטל z ניתן לזיהוי על פי התבוננות מהיטל x , אמנם מופיע שם קו אנכי נוסף אך על פי צורת הזוית ניתן לזהות את מיקום הקו"},//מהצד לכל הרוחב יש קו אבל הוא שונה ולכן ברור שחייב להיות קו 
         {25,"קו אנכי בהיטל x ניתן לזיהוי על פי התבוננות מהיטל z , אמנם מופיע שם קו אנכי נוסף אך על פי צורת הזוית ניתן לזהות את מיקום הקו  "}, //מקדימה לכל הרוחב יש קו אבל הוא שונה ולכן ברור שחייב להיות קו 
         {0,"מקדימה לכל האורך אין כלום"},//מקדימה לכל האורך אין כלום
         {1,"מלמעלה לכל הוחב אין כלום "},//מלמעלה לכל הוחב אין כלום
         {2,"מהצד לכל האורך אין כלום "},//מהצד לכל האורך אין כלום
         {3,"מלמעלה לכל האורך אין כלום "},//
         {4,"מהצד לכל הרוחב אין כלום "},//
         {5,"מקדימה לכל הרוחב אין כלום "},//
         {50,"ניתן להמשיך את אחד הקוים הבנוים לפי צורת הזוית המסמלת את המשך הקו"}
    };
       // string angle120 = "angle120";
        //= Matrix.RotationX(-12);
        //const float angle = -0.67f;
        const float angle = 0.4f;
        const float Sangle = -0.1f;
        
         //if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("angle120"))
         //       {
         //           ManagerGame.arrowsKeys[value] = ApplicationData.Current.RoamingSettings.Values[value].ToString();
         //       }
        //if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("angle120"))
        //public static Matrix IsoWorld= ApplicationData.Current.RoamingSettings.Values["angle120"] as Matrix;
        public static Matrix defaultWorld = Matrix.RotationZ(-angle - Sangle) * Matrix.RotationX(angle + Sangle) * Matrix.RotationY(-angle);//* Matrix.RotationY(-12) ;//* Matrix.RotationZ(120);
        public static SharpDX.Matrix World = Matrix.Identity;//defaultWorld;
    }
}
