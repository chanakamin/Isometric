using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isometric
{
    public enum eDimension { X, Y, Z };
    delegate  void OnExerciseEnd (bool sucssed); 
    static class Global
    {
        
        public static int Level = 1;
        public static bool Type = false;//false-2->3 true-3->2
        public static bool ActiveExercise = false;
        public const int N = 11;
        public static Dictionary<int, string> DescreptionHints = new Dictionary<int, string>(){
         {0,"מקדימה לכל האורך אין בכלל קו"},
         {1,"מלמעלה לכל האורך אין בכלל קו "},
         {2,"מהצד לכל האורך אין בכלל קו "},
         {3,"מלמעלה לכל הרוחב אין בכלל קו "},
         {4,"מהצד לכל הרוחב אין בכלל קו "},
         {5,"מקדימה לכל הרוחב אין בכלל קו "},
         {10,"מקדימה לכל האורך יש קו אבל הוא שונה ולכן ברור שחייב להיות קו "},
         {11,"מלמעלה לכל האורך יש קו אבל הוא שונה ולכן ברור שחייב להיות קו "},
         {12,"מהצד לכל האורך יש קו אבל הוא שונה ולכן ברור שחייב להיות קו "},
         {13,"מלמעלה לכל הרוחב יש קו אבל הוא שונה ולכן ברור שחייב להיות קו "},
         {14,"מהצד לכל הרוחב יש קו אבל הוא שונה ולכן ברור שחייב להיות קו "},
         {15,"מקדימה לכל הרוחב יש קו אבל הוא שונה ולכן ברור שחייב להיות קו "}
    };
        public static SharpDX.Matrix World = Matrix.Identity;
    }
}
//if ((flags[0, 0] || flags[0, 1]) && (mat[i].axis == eDimension.X || mat[i].axis == eDimension.Z))
//    AddHint(new Hint()
//    {
//        idDescription = flags[0, 0] ? 0 : 10,
//        LineToBold = mat[i].Clone()
//    });
//if ((flags[1, 0] || flags[1, 1]) && (mat[i].axis == eDimension.Y || mat[i].axis == eDimension.X))
//    AddHint(new Hint()
//    {
//        idDescription = flags[1, 0] ? 1 : 11,
//        LineToBold = mat[i].Clone()
//    });
//if ((flags[2, 0] || flags[2, 1]) && (mat[i].axis == eDimension.X || mat[i].axis == eDimension.Z))
//    AddHint(new Hint()
//    {
//        idDescription = flags[2, 0] ? 2 : 12,
//        LineToBold = mat[i].Clone()
//    });
//if ((flags[3, 0] || flags[3, 1]) && (mat[i].axis == eDimension.Z || mat[i].axis == eDimension.Y))
//    AddHint(new Hint()
//    {
//        idDescription = flags[3, 0] ? 3 : 13,
//        LineToBold = mat[i].Clone()
//    });
//if ((flags[4, 0] || flags[4, 1]) && (mat[i].axis == eDimension.X || mat[i].axis == eDimension.Y))
//    AddHint(new Hint()
//    {
//        idDescription = flags[4, 0] ? 4 : 14,
//        LineToBold = mat[i].Clone()
//    });
//if ((flags[5, 0] || flags[5, 1]) && (mat[i].axis == eDimension.Z || mat[i].axis == eDimension.Y))
//    AddHint(new Hint()
//    {
//        idDescription = flags[5, 0] ? 5 : 15,
//        LineToBold = mat[i].Clone()
//    });