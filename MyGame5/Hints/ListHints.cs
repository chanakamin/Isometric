using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Isometric._3DObjects;
namespace Isometric.Hints
{
   struct TypePoint
    {
        public eDimension axis;
        public int TypeAngle;
    }

    class ListHints
    {
        public List<Hint> list = new List<Hint>();
        public void Create(LogicalMatrix Matrix)
        {
            ConvertToMatrixTypePoint(Matrix);
            CreateListHint();
        }
        public Hint getHint()
        {
            if (list.Count > 0)
            {
                //אוסף זמני של הרמזים לשמירת הרמזים הניתנים להצגה בלבד
                List<Hint> tempListHint = new List<Hint>();
                foreach (var item in list)
                {
                    var lineHint = item.LineToBold;
                    //בדיקה אם הקו שאליו מכוון הרמז עדין לא צויר ע"י המשתמש
                    if (!ManagerGame.matrixUser.IfFill(lineHint.axis, true, (int)lineHint.x, (int)lineHint.y, (int)lineHint.z, lineHint.length == Global.N))
                    {
                        var LineCondition = item.LineCondition;
                        //בדיקה האם הקו המתנה את קיום הרמז כבר צויר 
                        if (LineCondition == null || ManagerGame.matrixUser.IfFill(LineCondition.axis, true, (int)LineCondition.x, (int)LineCondition.y, (int)LineCondition.z, LineCondition.length == Global.N))
                        {
                            //הוספת הרמז שמקיים את התנאים לרשימה הזמנית
                            tempListHint.Add(item);
                        }
                    }
                }
                Random r = new Random();
                //אם יש רמזים רלוונטים
                //בחירת רמז רנדומלי מתוך האוסף
               return tempListHint.Count > 0 ?  tempListHint[r.Next(tempListHint.Count())] : null ;
            }
            return null;
        }


        private void CreateListHint()
        {
            list.Clear();
            //עבור כל זוית את שני מימדי הקוים הנוצרים ממנה
            Dictionary<eDimension, List<eDimension>> DimensionsForAngle = new Dictionary<eDimension, List<eDimension>>(){
            {eDimension.X,new List<eDimension>(){eDimension.X, eDimension.Z}},
            {eDimension.Z,new List<eDimension>(){eDimension.Z, eDimension.Y}},
            {eDimension.Y,new List<eDimension>(){eDimension.Y, eDimension.X}}};
            //עבור כל סוג זוית מאילו כיוונים רואים אותה ומה מעניין מכיוון זה
            Dictionary<eDimension, List<int>> indexFlagsForAngle = new Dictionary<eDimension, List<int>>(){
            {eDimension.X,new List<int>(){0,1,2,4}},
            {eDimension.Y,new List<int>(){4,5,1,3}},
            {eDimension.Z,new List<int>(){2,3,0,5}}};

            for (int x = 0; x < Global.N; x += Global.N / 2)
            {
                for (int y = 0; y < Global.N; y += Global.N / 2)
                {
                    for (int z = 0; z < Global.N; z += Global.N / 2)
                    {
                        if (MatrixValues[x, y, z] != null && MatrixValues[x, y, z].Value.TypeAngle > 0)
                        {
                            int SubFromFirstLine = 0;
                            int AddToLastLine = 0;
                            switch (MatrixValues[x, y, z].Value.TypeAngle)
                            {
                                case 1: break;
                                case 2:
                                    SubFromFirstLine = Global.N / 2; break;
                                case 3:
                                    SubFromFirstLine = Global.N / 2;
                                    AddToLastLine = Global.N / 2; break;
                                case 4:
                                    AddToLastLine = Global.N / 2; break;
                                default:
                                    break;
                            }
                            Line[] mat = new Line[2];
                            switch (MatrixValues[x, y, z].Value.axis)
                            {//עבור כל זוית ניצור מערך המכל את שני הקוים שמרכיבים את אותה הזוית 
                                case eDimension.X: mat = new Line[2] { new Line(x - SubFromFirstLine, y, z, eDimension.X, Global.N / 2), new Line(x, y, z - AddToLastLine, eDimension.Z, Global.N / 2) }; break;
                                case eDimension.Y: mat = new Line[2] { new Line(x, y - SubFromFirstLine, z, eDimension.Y, Global.N / 2), new Line(x - AddToLastLine, y, z, eDimension.X, Global.N / 2) }; break;
                                case eDimension.Z: mat = new Line[2] { new Line(x, y, z - SubFromFirstLine, eDimension.Z, Global.N / 2), new Line(x, y - AddToLastLine, z, eDimension.Y, Global.N / 2) }; break;
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                bool flagLineCondition = true;
                                bool[,] flags = new bool[6, 3];
                                for (int d = 0; d < 4; d++)
                                {
                                    if (d < 2)
                                    {
                                        flags[indexFlagsForAngle[mat[i].axis][d], 1] = true;
                                        flags[indexFlagsForAngle[mat[i].axis][d], 2] = true;
                                    }
                                    else
                                        flags[indexFlagsForAngle[mat[i].axis][d], 0] = true;
                                }
                                for (int xx = 0; xx < Global.N; xx += Global.N / 2)
                                {
                                    for (int yy = 0; yy < Global.N; yy += Global.N / 2)
                                    {
                                        for (int zz = 0; zz < Global.N; zz += Global.N / 2)
                                        {
                                            bool[] flagTemp = new bool[3] { true, true, true };
                                            if (MatrixValues[xx, yy, zz] != null && (xx != x || yy != y || zz != z))//&& 
                                            {
                                                flagLineCondition &= !(MatrixValues[x, y, z].Value.axis == eDimension.X && xx == x && zz == z) || (MatrixValues[x, y, z].Value.axis == eDimension.Y && yy == y && xx == x) || (MatrixValues[x, y, z].Value.axis == eDimension.Z && zz == z && yy == y);
                                                flagTemp[0] = false;
                                                if (MatrixValues[xx, yy, zz].Value.TypeAngle > 0)
                                                {
                                                    if ((DimensionsForAngle[MatrixValues[xx, yy, zz].Value.axis]).Any(m => m == mat[i].axis))
                                                        flagTemp[1] = false;
                                                    if (MatrixValues[xx, yy, zz].Value.axis == MatrixValues[x, y, z].Value.axis && MatrixValues[xx, yy, zz].Value.TypeAngle > 0)
                                                        flagTemp[2] = false;
                                                }
                                                else
                                                    if (MatrixValues[xx, yy, zz].Value.axis == mat[i].axis)
                                                    {
                                                        flagTemp[1] = false;
                                                        flagTemp[2] = false;
                                                    }
                                                if (xx == x && yy != y)
                                                {
                                                    setFlags(ref flags, flagTemp, 0);
                                                }
                                                if (xx == x && zz != z)
                                                {
                                                    setFlags(ref flags, flagTemp, 1);
                                                }
                                                if (zz == z && yy != y)
                                                {
                                                    setFlags(ref flags, flagTemp, 2);
                                                }
                                                if (zz == z && xx != x)
                                                {
                                                    setFlags(ref flags, flagTemp, 3);
                                                }
                                                if (yy == y && zz != z)
                                                {
                                                    setFlags(ref flags, flagTemp, 4);
                                                }
                                                if (yy == y && xx != x)
                                                {
                                                    setFlags(ref flags, flagTemp, 5);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (flagLineCondition)
                                {
                                    list.Add(new Hint()
                                        {
                                            idDescription = 50,
                                            LineCondition = mat[(i + 1) % 2],
                                            LineToBold = mat[i]
                                        });
                                }
                                for (int m = 0; m < 6; m++)
                                {
                                    for (int n = 0; n < 3; n++)
                                    {
                                        if (flags[m, n])
                                        {
                                            AddHint(new Hint()
                                            {
                                                idDescription = m + n * 10,
                                                LineToBold = mat[i].Clone()
                                            });
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void setFlags(ref bool[,] flags, bool[] flagTemp, int g)
        {
            for (int w = 0; w < 3; w++)
            {
                flags[g, w] &= flagTemp[w];
            }
        }


      
        /// <summary>
        /// בודק אם הרמז ממשיך רמז אחר
        /// </summary>
        /// <param name="l"></param>
        private void AddHint(Hint HintToAdd)
        {
            Line l = HintToAdd.LineToBold;
            foreach (var item in list)
            {
                if (item.idDescription == HintToAdd.idDescription)
                {
                    Line lineHint = item.LineToBold;
                    if (lineHint.axis == l.axis)
                    {
                        var axis = l.axis;
                        //אם כל הערכים שוים חוץ מהערך ששווה לציר
                        if ((lineHint.x == l.x ^ axis == eDimension.X) && (lineHint.y == l.y ^ axis == eDimension.Y) && (lineHint.z == l.z ^ axis == eDimension.Z))

                            item.LineToBold.length = Global.N;
                        return;
                    }
                }

            }
            list.Add(HintToAdd);
        }
        TypePoint?[, ,] MatrixValues;
        private void ConvertToMatrixTypePoint(LogicalMatrix Matrix)
        {
            //TypePoint הגדרת מטריצה חדשה מסוג 
            //עם אפשרות להצבת ערך null
           //המטריצה בגודלה של המטרימ=צה המקורית כדי לשמור על אינדקס שווה בין המטריצות
            MatrixValues = new TypePoint?[Global.N, Global.N, Global.N];
            //מעבר על המטריצה  רק בנקודות בהם קיימות זויות תחילה סוף ואמצע 
            for (int x = 0; x < Global.N; x += Global.N / 2)
            {
                for (int y = 0; y < Global.N; y += Global.N / 2)
                {
                    for (int z = 0; z < Global.N; z += Global.N / 2)
                    {
                        //הצבת ערך דיפולטיבי ריק 
                        MatrixValues[x, y, z] = null;
                        //אם התא הנוכחי בעל ערך יש לבצע בדיקה על שכנוי
                        if (Matrix[x, y, z])
                        {
                            //Right
                            if (x < Global.N - 1 && Matrix[x + 1, y, z])
                            {
                                if (z < Global.N - 1 && Matrix[x, y, z + 1])//Right Forward
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.X, TypeAngle = 1 };//.CornerX1;
                                if (z > 0 && Matrix[x, y, z - 1])//Right Backward
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.X, TypeAngle = 4 };//eTypePoint.CornerX4;
                                //End line Right
                                if (MatrixValues[x, y, z] == null) MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.X, TypeAngle = 0 };

                            }
                            //Left
                            if (x > 0 && Matrix[x - 1, y, z])
                            {
                                if (z < Global.N - 1 && Matrix[x, y, z + 1])//Left Forward
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.X, TypeAngle = 2 };//eTypePoint.CornerX2;
                                if (z > 0 && Matrix[x, y, z - 1])//Left Backward
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.X, TypeAngle = 3 };// eTypePoint.CornerX3;
                                //End line Left
                                if (MatrixValues[x, y, z] == null) MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.X, TypeAngle = 0 };

                            }
                            //Up
                            if (y < Global.N - 1 && Matrix[x, y + 1, z])
                            {
                                if (x < Global.N - 1 && Matrix[x + 1, y, z])//up Right
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Y, TypeAngle = 1 };// eTypePoint.CornerY1;
                                if (x > 0 && Matrix[x - 1, y, z])//Up Left
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Y, TypeAngle = 4 };// eTypePoint.CornerY4;
                                //End line Up
                                if (MatrixValues[x, y, z] == null) MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Y, TypeAngle = 0 };

                            }
                            //Down
                            if (y > 0 && Matrix[x, y - 1, z])
                            {
                                if (x < Global.N - 1 && Matrix[x + 1, y, z])//Down Right
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Y, TypeAngle = 2 };// eTypePoint.CornerY2;
                                if (x > 0 && Matrix[x - 1, y, z])//Down Left
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Y, TypeAngle = 3 };// eTypePoint.CornerY3;
                                //End line Down
                                if (MatrixValues[x, y, z] == null) MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Y, TypeAngle = 0 };

                            }
                            //Forward
                            if (z < Global.N - 1 && Matrix[x, y, z + 1])
                            {
                                if (y < Global.N - 1 && Matrix[x, y + 1, z])//Forward Up
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Z, TypeAngle = 1 };// eTypePoint.CornerZ1;
                                if (y > 0 && Matrix[x, y - 1, z])//Forward Down
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Z, TypeAngle = 4 };// eTypePoint.CornerZ4;
                                //End line Forward
                                if (MatrixValues[x, y, z] == null) MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Z, TypeAngle = 0 };

                            }
                            //Backward
                            if (z > 0 && Matrix[x, y, z - 1])
                            {
                                if (y < Global.N - 1 && Matrix[x, y + 1, z])//Backward Up
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Z, TypeAngle = 2 };// eTypePoint.CornerZ2;
                                if (y > 0 && Matrix[x, y - 1, z])//Backward Down
                                    MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Z, TypeAngle = 3 };// eTypePoint.CornerZ3;
                                //End line Backward
                                if (MatrixValues[x, y, z] == null) MatrixValues[x, y, z] = new TypePoint() { axis = eDimension.Z, TypeAngle = 0 };
                            }
                        }
                    }
                }
            }
        }
    }
}
