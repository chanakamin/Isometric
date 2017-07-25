using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Isometric._3DObjects;
namespace Isometric
//לשנות את הxyz גם במחלקות האחרות לint
{
    class MatrixBase
    {

        #region members
        //אפשר להגדיר קבוע ברמת ניים ספייס?
        public const int N = 11;
        public bool[, ,] matrix { get; set; }
        #endregion
        #region c-tor
        public MatrixBase(bool b)
        {
            matrix = new bool[N, N, N];
            if (b) Init();

        }
        #endregion
        #region func
        //מילוי זמני
        private void miluy()
        {
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    for (int z = 0; z < N; z++)
                    {
                        matrix[x, y, z] = false;
                        if (z == 0 && y == 0) matrix[x, y, z] = true;
                        //if (x == 1 && y == 2 && z < N / 2) matrix[x, y, z] = true;
                      ///  if (z == 0 && y == 0) matrix[x, y, z] = true;
                        //הקוים על ציר הy
                        //יוצאים הפוך (את הבפנים של כל השאר רואים אצלם מבחוץ )
                        //בעקרון זה לא משנה אבל למה זה קורה?
                       // if (z == 1 && x == 2) matrix[x, y, z] = true;
                       // if (z == 6 && x == 1) matrix[x, y, z] = true;

                    }
                }
            }
        }
        public void Init()
        {
            IncDec incDecPos = incX;
            Random rand = new Random();
            int[] matEzPosition = new int[3] { 0, N / 2, N - 1 };
            //אפשר להגדיר ע"פ רמת קושי
            int CountLines = rand.Next(5, 8);
            // לאפשר לו להיות רק בהתחלה או בסוף
            int x = matEzPosition[rand.Next(0, 2) * 2];
            int y = matEzPosition[rand.Next(0, 2) * 2];
            int z = matEzPosition[rand.Next(0, 2) * 2];
            for (int i = 0; i < CountLines; i++)
            {
                eDimension axis = eDimension.X;
                bool direct = true;
                bool length = false;
                bool valid;
                do
                {
                    //if (!length)
                    //{
                    axis = (eDimension)rand.Next(0, 3);
                    direct = rand.Next(0, 2) == 1 ? true : false;
                    // length = matEzPosition[rand.Next(1, 3)];
                    // length = 5;
                    incDecPos = SetDelegateIncDec(axis, direct);
                    length = rand.Next(0, 2) != 1 ? true : false;/////////////////////////////////////////////////////////////////
                    //}
                    //else length = false;
                    valid = IsValidDirection(axis, direct, x, y, z, length);
                    valid = valid ? !IfFill(axis, direct, x, y, z, length) : valid;
                    valid = valid ? (SumLines(axis, direct, x, y, z, false) == 0) : valid;
                    if (length) valid = valid ? (SumLines(axis, direct, x, y, z, true) == 0) : valid;

                } while (!valid);
                int _length = (length) ? N - 1 : N / 2;
                //Hתכן שזה מיותר אולי צריך רק בפעם הראשונה
                matrix[x, y, z] = true;
                for (int j = 0; j < _length; j++)
                {
                    incDecPos(ref x, ref y, ref z);
                    matrix[x, y, z] = true;
                }
            }
        }
        public List<Shape> CreatateShape(int x, int y, int z, eDimension axis, int pointSstart)
        {
            List<Shape> listShape = new List<Shape>();
            bool flagIsCornerStart = false;
            bool flagIsCornerEnd = false;
            float UpdatepointSstart = 0;
            float UpdatepointEnd = 0;
            switch (axis)
            {
                case eDimension.X:
                    if (z < N - 1)
                    {
                        if (matrix[pointSstart, y, z + 1])
                        {
                            listShape.Add(new Corner(pointSstart, y, z, eDimension.X, 1));
                            flagIsCornerStart = true;
                        }
                        if (matrix[x - 1, y, z + 1])
                        {
                            listShape.Add(new Corner(x - 1, y, z, eDimension.X, 2));
                            flagIsCornerEnd = true;
                        }
                    }
                    if (z > 0)
                    {
                        if (matrix[pointSstart, y, z - 1])
                        {
                            listShape.Add(new Corner(pointSstart, y, z, eDimension.X, 4));
                            flagIsCornerStart = true;
                        }
                        if (matrix[x - 1, y, z - 1])
                        {
                            listShape.Add(new Corner(x - 1, y, z, eDimension.X, 3));
                            flagIsCornerEnd = true;
                        }
                    }
                    UpdatepointSstart = pointSstart;
                    UpdatepointEnd = x - 1;
                    if (flagIsCornerStart || (y > 0 && matrix[pointSstart, y - 1, z]) || (y < N - 1 && matrix[pointSstart, y + 1, z]))
                        UpdatepointSstart += (N / 11f) * 1.5f;
                    else
                        listShape.Add(new EndLine(pointSstart, y, z));
                    if (flagIsCornerEnd || (y > 0 && matrix[x - 1, y - 1, z]) || (y < N - 1 && matrix[x - 1, y + 1, z]))
                        UpdatepointEnd -= (N / 11f) * 1.5f;
                    else
                        listShape.Add(new EndLine(x - 1, y, z));
                    listShape.Add(new Line(UpdatepointSstart, y, z, eDimension.X, UpdatepointEnd - UpdatepointSstart));
                    break;
                case eDimension.Y:
                    //   listShape.Add(new Line(x, pointSstart, z, eAxis.y, y - 1 - pointSstart));
                    if (x < N - 1)
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
                    if (flagIsCornerStart || (z > 0 && matrix[x, pointSstart, z - 1]) || (z < N - 1 && matrix[x, pointSstart, z + 1]))
                        UpdatepointSstart += (N / 11f) * 1.5f;
                    else
                        listShape.Add(new EndLine(x, pointSstart, z));
                    if (flagIsCornerEnd || (z > 0 && matrix[x, y - 1, z - 1]) || (z < N - 1 && matrix[x, y - 1, z + 1]))
                        UpdatepointEnd -= (N / 11f) * 1.5f;
                    else
                        listShape.Add(new EndLine(x, y - 1, z));
                    listShape.Add(new Line(x, UpdatepointSstart, z, eDimension.Y, UpdatepointEnd - UpdatepointSstart));

                    break;
                case eDimension.Z:
                    //    listShape.Add(new Line(x, y, pointSstart, eAxis.z, z - 1 - pointSstart));
                    if (y < N - 1)
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
                    if (flagIsCornerStart || (x > 0 && matrix[x - 1, y, pointSstart]) || (x < N - 1 && matrix[x + 1, y, pointSstart]))
                        UpdatepointSstart += (N / 11f) * 1.5f;
                    else
                        listShape.Add(new EndLine(x, y, pointSstart));
                    if (flagIsCornerEnd || (x > 0 && matrix[x - 1, y, z - 1]) || (x < N - 1 && matrix[x + 1, y, z - 1]))
                        UpdatepointEnd -= (N / 11f) * 1.5f;
                    else
                        listShape.Add(new EndLine(x, y, z - 1));
                    listShape.Add(new Line(x, y, UpdatepointSstart, eDimension.Z, UpdatepointEnd - UpdatepointSstart));

                    break;
                default:
                    break;
            }
            return listShape;
        }
        public List<Shape> ToListShape()
        {
            List<Shape> listShape = new List<Shape>();
            int pointSstart;
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    pointSstart = -1;
                    for (int z = 0; z < N; z++)
                    {
                        if (matrix[x, y, z])
                        {
                            if (pointSstart == -1)
                                pointSstart = z;
                        }
                        else
                        {
                            if (pointSstart != -1 && pointSstart < z - 1)
                            {
                                listShape.AddRange(CreatateShape(x, y, z, eDimension.Z, pointSstart));
                            }
                            pointSstart = -1;
                        }
                    }

                    if (pointSstart != -1 && pointSstart < N - 1)
                    {
                        listShape.AddRange(CreatateShape(x, y, N, eDimension.Z, pointSstart));
                    }
                }
            }
            for (int z = 0; z < N; z++)
            {
                for (int x = 0; x < N; x++)
                {
                    pointSstart = -1;
                    for (int y = 0; y < N; y++)
                    {
                        if (matrix[x, y, z])
                        {
                            if (pointSstart == -1)
                                pointSstart = y;
                        }
                        else
                        {
                            if (pointSstart != -1 && pointSstart < y - 1)
                            {
                                listShape.AddRange(CreatateShape(x, y, z, eDimension.Y, pointSstart));
                            }
                            pointSstart = -1;
                        }
                    }
                    if (pointSstart != -1 && pointSstart < N - 1)
                        listShape.AddRange(CreatateShape(x, N, z, eDimension.Y, pointSstart));
                }
            }

            for (int z = 0; z < N; z++)
            {
                for (int y = 0; y < N; y++)
                {
                    pointSstart = -1;
                    for (int x = 0; x < N; x++)
                    {
                        if (matrix[x, y, z])
                        {
                            if (pointSstart == -1)
                                pointSstart = x;
                        }
                        else
                        {
                            if (pointSstart != -1 && pointSstart < x - 1)
                            {
                                listShape.AddRange(CreatateShape(x, y, z, eDimension.X, pointSstart));
                            }
                            pointSstart = -1;
                        }
                    }

                    if (pointSstart != -1 && pointSstart < N - 1)
                    {
                        listShape.AddRange(CreatateShape(N, y, z, eDimension.X, pointSstart));
                    }
                }
            }
            return listShape;
        }
        //האם הכיוון תקין
        public static bool IsValidDirection(eDimension axis, Boolean direct, int x, int y, int z, bool length = false)
        {
            int _length = length ? N - 1 : N / 2;
            switch (axis)
            {
                case eDimension.X:
                    if (direct)
                        return (x <= (length ? 0 : N / 2));
                    else
                        return (x >= (length ? N - 1 : N / 2));

                case eDimension.Y:
                    if (direct)
                        return (y <= (length ? 0 : N / 2));
                    else
                        return (y >= (length ? N - 1 : N / 2));

                case eDimension.Z:
                    if (direct)
                        return (z <= (length ? 0 : N / 2));
                    else
                        return (z >= (length ? N - 1 : N / 2));

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
                        return (this[x + 1, y, z] && (!length || this[x + N / 2 + 1, y, z]));
                    else
                        return (this[x - 1, y, z] && (!length || this[x - N / 2 + 1, y, z]));

                case eDimension.Y:
                    if (direct)
                        return (this[x, y + 1, z] && (!length || this[x, y + N / 2 + 1, z]));
                    else
                        return (this[x, y - 1, z] && (!length || this[x, y - N / 2 + 1, z]));

                case eDimension.Z:
                    if (direct)
                        return (this[x, y, z + 1] && (!length || this[x, y, z + N / 2 + 1]));
                    else
                        return (this[x, y, z - 1] && (!length || this[x, y, z - N / 2 + 1]));
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
            sum += (x < N - 1 && matrix[x + 1, y, z]) ? 1 : 0;
            sum += (y > 0 && matrix[x, y - 1, z]) ? 1 : 0;
            sum += (y < N - 1 && matrix[x, y + 1, z]) ? 1 : 0;
            sum += (z > 0 && matrix[x, y, z - 1]) ? 1 : 0;
            sum += (z < N - 1 && matrix[x, y, z + 1]) ? 1 : 0;
            return sum;
        }
        //צריך שם אחר לפונקציה -הםונקציה מחזירה את מספר הקוים שמתחילים במקום שבו הקו שאני רוצה צריך להסתיים
        //בדיקת תקינות אם אפשר לצייר קו
        public int SumLines(eDimension axis, bool direct, int x, int y, int z, bool length = false)
        {
            int _length = (length) ? N - 1 : N / 2;
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
        public static IncDec SetDelegateIncDec(eDimension axis, bool direct)
        {
            IncDec incDecPos;
            switch (axis)
            {
                case eDimension.X:
                    if (direct)
                        incDecPos = MatrixBase.incX;
                    else
                        incDecPos = MatrixBase.decX;
                    break;
                case eDimension.Y:
                    if (direct)
                        incDecPos = MatrixBase.incY;
                    else
                        incDecPos = MatrixBase.decY;
                    break;
                case eDimension.Z:
                    if (direct)
                        incDecPos = MatrixBase.incZ;
                    else
                        incDecPos = MatrixBase.decZ;
                    break;
                default: incDecPos = MatrixBase.incX;
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
        ///  GetDeviceRemovedReason
        //קידום אחד מהערכים תזוזה לאחד הכיוונים
        public static void incX(ref int x, ref int y, ref int z) { x++; }
        public static void decX(ref int x, ref int y, ref int z) { x--; }
        public static void incY(ref int x, ref int y, ref int z) { y++; }
        public static void decY(ref int x, ref int y, ref int z) { y--; }
        public static void incZ(ref int x, ref int y, ref int z) { z++; }
        public static void decZ(ref int x, ref int y, ref int z) { z--; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(MatrixBase))
            {
                for (int x = 0; x < N; x++)
                    for (int y = 0; y < N; y++)
                        for (int z = 0; z < N; z++)
                            if (matrix[x, y, z] != ((MatrixBase)obj)[x, y, z]) return false;
                return true;
            }
            return base.Equals(obj);
        }
        
        #endregion
    }
}