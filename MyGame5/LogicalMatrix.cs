using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Isometric._3DObjects;
namespace Isometric

{
    class LogicalMatrix
    {

        #region members
        
        //מטריצה בוליאנית לשמירת מיקומי הקוים
        public bool[, ,] matrix { get; set; }
        
        #endregion
       
        #region c-tor
       
        /// <summary>
        /// קונסטרקטור
        /// </summary>
        /// <param name="b"> משתנה בוליאני האם למלאות את הקוביה או להשאיר אותה ריקה</param>
        public LogicalMatrix(bool b)
        {
            matrix = new bool[Global.N,Global.N,Global.N];
            if (b) Init();

        }
        #endregion
        #region func

        public void Init()
        {
            IncDec incDecPos = incX;
            Random rand = new Random();
            //הנקודות בהם אפשר להתחיל קו התחלה/אמצע/סיום
            int[] matEzPosition = new int[3] { 0, Global.N / 2, Global.N - 1 };
            //מספר הקוים ע"פ הרמה שנבחרה
            int CountLines = (ManagerGame.Level * 2) + 4;
            //קו ראשון יכול להתחיל רק בהצחלה או בסוף ולא באמצע
            // לאפשר לו להיות רק בהתחלה או בסוף
            int x = matEzPosition[rand.Next(0, 2) * 2];
            int y = matEzPosition[rand.Next(0, 2) * 2];
            int z = matEzPosition[rand.Next(0, 2) * 2];
            //יצירת קוים ע"פ הכמות הנדרשת
            for (int i = 0; i < CountLines; i++)
            {
                eDimension axis = eDimension.X;
                bool direct = true;
                bool length = false;
                bool valid;
                //ממשיך את הלולאה עד שמוצא קו תקין
                do
                {
                    //מימד 
                    axis = (eDimension)rand.Next(0, 3);
                    //כיוון עולה/יורד
                    direct = rand.Next(0, 2) == 1 ? true : false;
                    //הצבת דלגייט ע"פ כיווון ומימד
                    incDecPos = SetDelegateIncDec(axis, direct);
                    //אורך 
                    //0-4 כדי שיןןצרו יותר קוים ארוכים
                    length = rand.Next(0, 4) != 1 ? true : false;
                    //בדיקת תקינות הקו
                    //בולות מערך
                    valid = IsValidDirection(axis, direct, x, y, z, length);
                    //אם המקום ריק
                    valid = valid ? !IfFill(axis, direct, x, y, z, length) : valid;
                    //בדיקה שאין שום קו שמסתיים במקום שבו הקו צריך להסתיים כדי שלאתהיה הצנגשות בין הקוים
                    valid = valid ? (SumLines(axis, direct, x, y, z, false) == 0) : valid;
                    if (length) valid = valid ? (SumLines(axis, direct, x, y, z, true) == 0) : valid;
                    //ממשיך כל עוד לא מצא קו תקין
                } while (!valid);
                //הצבת האורך המוחלט
                int _length = (length) ? Global.N - 1 : Global.N / 2;
                //בניית הקו בפועל במטריצה ע"פ אורך כיוון ומימד
                matrix[x, y, z] = true;
                for (int j = 0; j < _length; j++)
                {
                    incDecPos(ref x, ref y, ref z);
                    matrix[x, y, z] = true;
                }
            }
        }

        //המרת המטריצה לליסט של צורות קו/ זוית/ סיום קו
        public List<Shape> ToListShape()
        {
            List<Shape> listShape = new List<Shape>();
            int pointSstart;
            //מימד z
            for (int x = 0; x < Global.N; x++)
            {
                for (int y = 0; y < Global.N; y++)
                {
                    pointSstart = -1;
                    for (int z = 0; z < Global.N; z++)
                    {//בדיקת רצף
                        if (matrix[x, y, z])
                        {
                            //אם עדיין לא הוצבה נקודת התחלה
                            if (pointSstart == -1)
                                //נקודת ההתחלה שווה לנוכחי
                                pointSstart = z;
                        }
                        else
                        {// נמצאה נקודה ריקה
                            //אם מוצבת נקודת התחלה ואורך הקו גדול מ1
                            if (pointSstart != -1 && pointSstart < z - 1)
                            {//מוסיף קו
                                listShape.AddRange(CreatateShape(x, y, z, eDimension.Z, pointSstart));
                            }
                            pointSstart = -1;//אתחול לקו חדש
                        }
                    }
                    //בדיקת נוספת לרצף אחרון
                    if (pointSstart != -1 && pointSstart < Global.N - 1)
                    {
                        listShape.AddRange(CreatateShape(x, y, Global.N, eDimension.Z, pointSstart));
                    }
                }
            }
            //מימד Y
            for (int z = 0; z < Global.N; z++)
            {
                for (int x = 0; x < Global.N; x++)
                {
                    pointSstart = -1;
                    for (int y = 0; y < Global.N; y++)
                    {//בדיקת רצף
                        if (matrix[x, y, z])
                        {
                            //אם עדיין לא הוצבה נקודת התחלה
                            if (pointSstart == -1)
                                pointSstart = y;
                        }
                        else
                        {// נמצאה נקודה ריקה
                            //אם מוצבת נקודת התחלה ואורך הקו גדול מ1
                            if (pointSstart != -1 && pointSstart < y - 1)
                            {
                                listShape.AddRange(CreatateShape(x, y, z, eDimension.Y, pointSstart));
                            }
                            pointSstart = -1;
                        }
                    }
                    //בדיקת נוספת לרצף אחרון
                    if (pointSstart != -1 && pointSstart < Global.N - 1)
                        listShape.AddRange(CreatateShape(x, Global.N, z, eDimension.Y, pointSstart));
                }
            }
            //מימד X
            for (int z = 0; z < Global.N; z++)
            {
                for (int y = 0; y < Global.N; y++)
                {
                    pointSstart = -1;
                    for (int x = 0; x < Global.N; x++)
                    {//בדיקת רצף
                        if (matrix[x, y, z])
                        {
                            //אם עדיין לא הוצבה נקודת התחלה
                            if (pointSstart == -1)
                                pointSstart = x;
                        }
                        else
                        {// נמצאה נקודה ריקה
                            //אם מוצבת נקודת התחלה ואורך הקו גדול מ1
                            if (pointSstart != -1 && pointSstart < x - 1)
                            {
                                listShape.AddRange(CreatateShape(x, y, z, eDimension.X, pointSstart));
                            }
                            pointSstart = -1;
                        }
                    }
                    //בדיקת נוספת לרצף אחרון
                    if (pointSstart != -1 && pointSstart < Global.N - 1)
                    {
                        listShape.AddRange(CreatateShape(Global.N, y, z, eDimension.X, pointSstart));
                    }
                }
            }
            return listShape;
        }

        //יצירת קו וזויות מתאימות בהתאם לקו
        public List<Shape> CreatateShape(int x, int y, int z, eDimension axis, int pointSstart)
        {
            List<Shape> listShape = new List<Shape>();
            bool flagIsCornerStart = false;//דגל לבדיקה האם כבר הוצבה זוית בתחילת הקו
            bool flagIsCornerEnd = false;//דגל לבדיקה האם כבר הוצבה זוית בסופף הקו
            float UpdatepointSstart = 0;
            float UpdatepointEnd = 0;
            switch (axis)
            {//בודק עבור כל מימד את 4 סוגי הזויות
              //יוצר זוית בהתאם לצורך
                case eDimension.X:
                    if (z < Global.N - 1)
                    {
                        if (matrix[pointSstart, y, z + 1])//eDimension.X, 1
                        {
                            listShape.Add(new Corner(pointSstart, y, z, eDimension.X, 1));
                            flagIsCornerStart = true;
                        }
                        if (matrix[x - 1, y, z + 1])//eDimension.X, 2
                        {
                            listShape.Add(new Corner(x - 1, y, z, eDimension.X, 2));
                            flagIsCornerEnd = true;
                        }
                    }
                    if (z > 0)
                    {
                        if (matrix[pointSstart, y, z - 1])//eDimension.X, 4
                        {
                            listShape.Add(new Corner(pointSstart, y, z, eDimension.X, 4));
                            flagIsCornerStart = true;
                        }
                        if (matrix[x - 1, y, z - 1])//eDimension.X, 3
                        {
                            listShape.Add(new Corner(x - 1, y, z, eDimension.X, 3));
                            flagIsCornerEnd = true;
                        }
                    }
                    //אם קיימת זוית באחד הצדדים יש צורך לקצר את הקו במקום המתאים
                    UpdatepointSstart = pointSstart;
                    UpdatepointEnd = x - 1;
                    //UpdatepointSstart אם קייימת זוית בהתחלה נעדכן את 
                    if (flagIsCornerStart || (y > 0 && matrix[pointSstart, y - 1, z]) || (y < Global.N - 1 && matrix[pointSstart, y + 1, z]))
                        UpdatepointSstart += (Global.N / 11f) * 1.5f;
                    else// EndLineבמקרה שלא קיימת זוית תציב 
                        listShape.Add(new EndLine(pointSstart, y, z));

                    //UpdatepointEnd אם קייימת זוית בסוף נעדכן את 
                    if (flagIsCornerEnd || (y > 0 && matrix[x - 1, y - 1, z]) || (y < Global.N - 1 && matrix[x - 1, y + 1, z]))
                        UpdatepointEnd -= (Global.N / 11f) * 1.5f;
                    else// EndLineבמקרה שלא קיימת זוית תציב 
                        listShape.Add(new EndLine(x - 1, y, z));
                    //יצירת הקו ע"פ העדכונים
                    listShape.Add(new Line(UpdatepointSstart, y, z, eDimension.X, UpdatepointEnd - UpdatepointSstart));
                    break;

                case eDimension.Y:
                    if (x < Global.N - 1)
                    {
                        if (matrix[x + 1, pointSstart, z])
                        {
                            listShape.Add(new Corner(x, pointSstart, z, eDimension.Y, 1));
                            flagIsCornerStart = true;
                        }
                        if (matrix[x + 1, y - 1, z])
                        {
                            listShape.Add(new Corner(x, y - 1, z, eDimension.Y, 2));
                            flagIsCornerEnd = true;
                        }
                    }
                    if (x > 0)
                    {
                        if (matrix[x - 1, pointSstart, z])
                        {
                            listShape.Add(new Corner(x, pointSstart, z, eDimension.Y, 4));
                            flagIsCornerStart = true;
                        }
                        if (matrix[x - 1, y - 1, z])
                        {
                            listShape.Add(new Corner(x, y - 1, z, eDimension.Y, 3));
                            flagIsCornerEnd = true;
                        }
                    }
                    UpdatepointSstart = pointSstart;
                    UpdatepointEnd = y - 1;
                    if (flagIsCornerStart || (z > 0 && matrix[x, pointSstart, z - 1]) || (z < Global.N - 1 && matrix[x, pointSstart, z + 1]))
                        UpdatepointSstart += (Global.N / 11f) * 1.5f;
                    else
                        listShape.Add(new EndLine(x, pointSstart, z));
                    if (flagIsCornerEnd || (z > 0 && matrix[x, y - 1, z - 1]) || (z < Global.N - 1 && matrix[x, y - 1, z + 1]))
                        UpdatepointEnd -= (Global.N / 11f) * 1.5f;
                    else
                        listShape.Add(new EndLine(x, y - 1, z));
                    listShape.Add(new Line(x, UpdatepointSstart, z, eDimension.Y, UpdatepointEnd - UpdatepointSstart));

                    break;
                case eDimension.Z:
                    if (y < Global.N - 1)
                    {
                        if (matrix[x, y + 1, pointSstart])
                        {
                            listShape.Add(new Corner(x, y, pointSstart, eDimension.Z, 1));
                            flagIsCornerStart = true;
                        }
                        if (matrix[x, y + 1, z - 1])
                        {
                            listShape.Add(new Corner(x, y, z - 1, eDimension.Z, 2));
                            flagIsCornerEnd = true;
                        }
                    }
                    if (y > 0)
                    {
                        if (matrix[x, y - 1, pointSstart])
                        {
                            listShape.Add(new Corner(x, y, pointSstart, eDimension.Z, 4));
                            flagIsCornerStart = true;
                        }
                        if (matrix[x, y - 1, z - 1])
                        {
                            listShape.Add(new Corner(x, y, z - 1, eDimension.Z, 3));
                            flagIsCornerEnd = true;
                        }
                    }

                    UpdatepointSstart = pointSstart;
                    UpdatepointEnd = z - 1;
                    if (flagIsCornerStart || (x > 0 && matrix[x - 1, y, pointSstart]) || (x < Global.N - 1 && matrix[x + 1, y, pointSstart]))
                        UpdatepointSstart += (Global.N / 11f) * 1.5f;
                    else
                        listShape.Add(new EndLine(x, y, pointSstart));
                    if (flagIsCornerEnd || (x > 0 && matrix[x - 1, y, z - 1]) || (x < Global.N - 1 && matrix[x + 1, y, z - 1]))
                        UpdatepointEnd -= (Global.N / 11f) * 1.5f;
                    else
                        listShape.Add(new EndLine(x, y, z - 1));
                    listShape.Add(new Line(x, y, UpdatepointSstart, eDimension.Z, UpdatepointEnd - UpdatepointSstart));

                    break;
                default:
                    break;
            }
            return listShape;
        }


        // האם הכיוון תקין מבחינת גבולות מערך 
        public static bool IsValidDirection(eDimension axis, Boolean direct, int x, int y, int z, bool length = false)
        {
            int _length = length ? Global.N - 1 : Global.N / 2;
            switch (axis)
            {
                case eDimension.X:
                    if (direct)
                        return (x <= (length ? 0 : Global.N / 2));
                    else
                        return (x >= (length ? Global.N - 1 : Global.N / 2));

                case eDimension.Y:
                    if (direct)
                        return (y <= (length ? 0 : Global.N / 2));
                    else
                        return (y >= (length ? Global.N - 1 : Global.N / 2));

                case eDimension.Z:
                    if (direct)
                        return (z <= (length ? 0 : Global.N / 2));
                    else
                        return (z >= (length ? Global.N - 1 : Global.N / 2));

                default:
                    return false;
            }
        }
        //האם הצעד הבא מלא
        public bool IfFill(eDimension axis, Boolean direct, int x, int y, int z, bool length = false)
        {

            switch (axis)
            {
                case eDimension.X:
                    if (direct)
                        return (this[x + 1, y, z] && (!length || this[x + Global.N / 2 + 1, y, z]));
                    else
                        return (this[x - 1, y, z] && (!length || this[x - Global.N / 2 + 1, y, z]));

                case eDimension.Y:
                    if (direct)
                        return (this[x, y + 1, z] && (!length || this[x, y + Global.N / 2 + 1, z]));
                    else
                        return (this[x, y - 1, z] && (!length || this[x, y - Global.N / 2 + 1, z]));

                case eDimension.Z:
                    if (direct)
                        return (this[x, y, z + 1] && (!length || this[x, y, z + Global.N / 2 + 1]));
                    else
                        return (this[x, y, z - 1] && (!length || this[x, y, z - Global.N / 2 + 1]));
                default:
                    return false;
            }
        }

        //מספר הקוים השכנים לנקודה מסוימת
        public int SumNeighborsLines(int x, int y, int z)
        {
            int sum = 0;
            if (!matrix[x, y, z]) return 0;
            sum += (x > 0 && matrix[x - 1, y, z]) ? 1 : 0;
            sum += (x < Global.N - 1 && matrix[x + 1, y, z]) ? 1 : 0;
            sum += (y > 0 && matrix[x, y - 1, z]) ? 1 : 0;
            sum += (y < Global.N - 1 && matrix[x, y + 1, z]) ? 1 : 0;
            sum += (z > 0 && matrix[x, y, z - 1]) ? 1 : 0;
            sum += (z < Global.N - 1 && matrix[x, y, z + 1]) ? 1 : 0;
            return sum;
        }

        //-הפונקציה מחזירה את מספר הקוים שמתחילים במקום שבו הקו שאני רוצה צריך להסתיים
        //בדיקת תקינות אם אפשר לצייר קו
        public int SumLines(eDimension axis, bool direct, int x, int y, int z, bool length = false)
        {
            int _length = (length) ? Global.N - 1 : Global.N / 2;
            switch (axis)
            {
                case eDimension.X:
                    if (direct)
                        return SumNeighborsLines(x + _length, y, z);
                    else
                        return SumNeighborsLines(x - _length, y, z);

                case eDimension.Y:
                    if (direct)
                        return SumNeighborsLines(x, y + _length, z);
                    else
                        return SumNeighborsLines(x, y - _length, z);

                case eDimension.Z:
                    if (direct)
                        return SumNeighborsLines(x, y, z + _length);
                    else
                        return SumNeighborsLines(x, y, z - _length);
                default:
                    return 0;
            }
        }

        //פונקציה להצבת הדלגגיט המתאים ע"פ מימד וכיוון
        public static IncDec SetDelegateIncDec(eDimension axis, bool direct)
        {
            IncDec incDecPos;
            switch (axis)
            {
                case eDimension.X:
                    if (direct)
                        incDecPos = LogicalMatrix.incX;
                    else
                        incDecPos = LogicalMatrix.decX;
                    break;
                case eDimension.Y:
                    if (direct)
                        incDecPos = LogicalMatrix.incY;
                    else
                        incDecPos = LogicalMatrix.decY;
                    break;
                case eDimension.Z:
                    if (direct)
                        incDecPos = LogicalMatrix.incZ;
                    else
                        incDecPos = LogicalMatrix.decZ;
                    break;
                default: incDecPos = LogicalMatrix.incX;
                    break;
            }
            return incDecPos;
        }
        //indexer
        public bool this[int x, int y, int z]
        {
            set { matrix[x, y, z] = value; }
            get { return matrix[x, y, z]; }
        }

    
        //כל אחת מהפונקציות מקדמת או מחסרת ערך מסוים משמש לדלגייט קידום /חיסור
        public static void incX(ref int x, ref int y, ref int z) { x++; }
        public static void decX(ref int x, ref int y, ref int z) { x--; }
        public static void incY(ref int x, ref int y, ref int z) { y++; }
        public static void decY(ref int x, ref int y, ref int z) { y--; }
        public static void incZ(ref int x, ref int y, ref int z) { z++; }
        public static void decZ(ref int x, ref int y, ref int z) { z--; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(LogicalMatrix))
            {
                for (int x = 0; x < Global.N; x++)
                    for (int y = 0; y < Global.N; y++)
                        for (int z = 0; z < Global.N; z++)
                            if (matrix[x, y, z] != ((LogicalMatrix)obj)[x, y, z]) return false;
                return true;
            }
            return base.Equals(obj);
        }
        
        #endregion
    }
}