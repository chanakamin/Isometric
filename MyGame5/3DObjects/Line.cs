using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Isometric;

namespace Isometric._3DObjects
{
     public class Line : Shape //,ICloneable
    {
        #region members

        public eDimension axis { get; set; }
        public float length { get; set; }

        #endregion

        #region c-tor
        public Line(float x, float y, float z, eDimension axis, float length)
            : base(x, y, z)
        {
            this.axis = axis;
            this.length = length;
        }
        public Line()
        {

        }
        public Line(eDimension axis)
        {
            this.axis = axis;
        }
        #endregion

        #region function
        /// <summary>
        /// הפונקציה ימחשבת נקודה במעגל ע"פ זוית וערך אחד מהצירים  
        ///  </summary>
        /// <param name="t">זוית</param>
        /// <param name="len">אורך לפעמים אורך ולפעמים 0</param>
        /// <returns>ווקטור 3 המכיל את הנקודה על המעגל</returns>
        private Vector3 GetPosition(float t, float len)
        {
            float xx = 0, yy = 0, zz = 0;
            switch (axis)
            {
                case eDimension.X:
                    zz = z + (float)(r * Math.Cos(t));
                    yy = y + (float)(r * Math.Sin(t));
                    xx = x + len;
                    break;
                case eDimension.Y:
                    zz = z + (float)(r * Math.Cos(t));
                    xx = x + (float)(r * Math.Sin(t));
                    yy = y + len;
                    break;
                case eDimension.Z:
                    yy = y + (float)(r * Math.Cos(t));
                    xx = x + (float)(r * Math.Sin(t));
                    zz = z + len;
                    break;
                default:
                    break;
            }
            return new Vector3(xx, yy, zz); ;
        }

        public override List<VertexPositionNormalTexture> Draw()
        {
            float pointStart = 0f;
            if (length < 0) return null;
            List<VertexPositionNormalTexture> listVertexPositionColor = new List<VertexPositionNormalTexture>();
            int tDiv = 32;//מספר החלקים בעיגול
            float maxTheta = (float)(2 * Math.PI);//
            float dt = maxTheta / tDiv;//    קידום הזוית בכל חלק
            for (int ti = 0; ti <= tDiv; ti++)
            {
                float t = ti * dt;
                float t1 = (ti + 1) * dt;//הזוית הבאה
                // בכל אינטרקציה של הלולאה יוצר שני משולשים ששניהם:י /
                //מקצה אחד של הגלית לקצה השני 
                //ומזוית זו לזוית הבאה 
                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t, pointStart), GetPosition(t, pointStart), new Vector2(0, 1)));
                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t1, pointStart), GetPosition(t1, pointStart), new Vector2(0, 1)));
                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t, length), GetPosition(t, length), new Vector2(0, 1)));

                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t1, length), GetPosition(t1, length), new Vector2(0, 1)));
                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t, length), GetPosition(t, length), new Vector2(0, 1)));
                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t1, pointStart), GetPosition(t1, pointStart), new Vector2(0, 1)));

                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t, length), GetPosition(t, length), new Vector2(0, 1)));
                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t1, pointStart), GetPosition(t1, pointStart), new Vector2(0, 1)));
                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t, pointStart), GetPosition(t, pointStart), new Vector2(0, 1)));

                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t1, pointStart), GetPosition(t1, pointStart), new Vector2(0, 1)));
                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t, length), GetPosition(t, length), new Vector2(0, 1)));
                listVertexPositionColor.Add(new VertexPositionNormalTexture(GetPosition(t1, length), GetPosition(t1, length), new Vector2(0, 1)));

            }
            return listVertexPositionColor;
        }
        public Line Clone()
        {
            return new Line(x, y, z, axis, length);
        }
        #endregion
    }
}
