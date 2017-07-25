using SharpDX;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isometric._3DObjects
{
    class Corner : Shape
    {
        #region members 

        public int TypeAngle { get; set; }//סוג זוית
        public eDimension axis { get; set; }//ציר

        #endregion
        #region c-tor

        public Corner(float x, float y, float z, eDimension axis, int TypeAngle)
            : base(x, y, z)
        {
            this.axis = axis;
            this.TypeAngle = TypeAngle;
        }
        #endregion
       
        #region func
        private Vector3 GetPosition(float t, float c, float rCurrent)
        {
            // בהתחלה מינוגס
            float xx=0;
            float zz=0;
            float yy=0;
            //
            switch (axis)
            {
                case eDimension.X://x
                     xx = x +AddToP1+ (float)(rCurrent * Math.Cos(t));
                     zz = z +AddToP2+ (float)(rCurrent * Math.Sin(t));
                     yy = y + c;
                     break;
                case eDimension.Y://y
                     xx = x + AddToP2 + (float)(rCurrent * Math.Sin(t));
                     zz = z + c;
                     yy = y + AddToP1 + (float)(rCurrent * Math.Cos(t));
                     break;
                case eDimension.Z://z
                     xx = x + c;
                     zz = z +  AddToP1 + (float)(rCurrent * Math.Cos(t));
                     yy = y + AddToP2 + (float)(rCurrent * Math.Sin(t));
                     break;
                default:
                    break;
            }
            return new Vector3(xx,yy,zz);
        }

        float AddToP1 = 1.5f;//לשנות למשהו יותר גנרי....
        float AddToP2 = 1.5f;
        public override List<VertexPositionNormalTexture> Draw()
        {
            List<VertexPositionNormalTexture> listVertexPositionColor = new List<VertexPositionNormalTexture>();
            int tDiv = 32;//מספר החלקים בגליל
            int yDiv = 8;//מספר החלקים בזוית
            float maxThetaY = (float)(2 * Math.PI);//עיגול שלם
            float maxThetaT = (float)(0.5 * Math.PI);//הזוית מורכבת מרבע עיגול
            float dt = maxThetaT / tDiv;//חלוקת המקסימום בכמות החלקים = כמה להתקדם בכל חלק
            float dy = maxThetaY / yDiv;//חלוקת המקסימום בכמות החלקים = כמה להתקדם בכל חלק
            float startZavit=0;
            //לפי סוג הזוית משתנה מקום התחלת הזוית
            //ומשתנה המרחק מצדדי הקוביה
            switch (TypeAngle)
            {
                case 1:
                    startZavit = 1;
                    break;
                case 2:
                    startZavit = 1.5f;
                    AddToP1 = -1.5f;
                    break;
                case 3:
                    startZavit = 2f;
                    AddToP1 = -1.5f;
                    AddToP2 = -1.5f;
                    break;
                case 4:
                    startZavit = 0.5f;
                    AddToP2 = -1.5f;
                    break;
                default:
                    break;
            }
           startZavit *= (float)(Math.PI);
            float zp = 1.5f;
            float yp = 0f;
            float zCenter = 3f;
            for (int yi = 0; yi <= yDiv; yi++)
            {
                //בכל שלב נחשב את הזוית הזו ואת הזוית הבאה 
                //כדי ליצור משולשים המתחילים בזוית ו ונגמרים בזוית הבאה
                float angle1 = yi * dy;
                float yc1 = yp + (float)(r * Math.Cos(angle1));
                float zc1 = zp + (float)(r * Math.Sin(angle1));
                float angle2 = (yi + 1) * dy;
                float yc2 = yp + (float)(r * Math.Cos(angle2));
                float zc2 = zp + (float)(r * Math.Sin(angle2));

                float rCurrent1 = zCenter - zc1;
                float rCurrent2 = zCenter - zc2;
                for (int ti = 0; ti <= tDiv; ti++)
                {
                    float t = startZavit + ti * dt;
                    float t1 = startZavit + (ti + 1) * dt;

                    listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t, yc1, rCurrent1), GetPosition(t, yc1, rCurrent1), new Vector2(0, 1)));
                    listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t1, yc1, rCurrent1),GetPosition(t1, yc1, rCurrent1), new Vector2(0, 1)));
                    listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t, yc2, rCurrent2),GetPosition(t, yc2, rCurrent2), new Vector2(0, 1)));
                    
                    listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t1, yc2, rCurrent2),GetPosition(t1, yc2, rCurrent2), new Vector2(0, 1)));
                    listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t, yc2, rCurrent2),GetPosition(t, yc2, rCurrent2), new Vector2(0, 1)));
                    listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t1, yc1, rCurrent1),GetPosition(t1, yc1, rCurrent1), new Vector2(0, 1)));

                }
            }
            return listVertexPositionColor;
        }
        #endregion
    }
}
