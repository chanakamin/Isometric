using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isometric._3DObjects
{
    //מחלקה הבסיס צורה ממנה יורשות הצורות האחרות 
    public abstract class Shape
    {
        #region properties
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        //רדיוס סטטי משותף לכל הצורות
        public static float r = 0.5f;
        #endregion

        #region function
        public abstract List<VertexPositionNormalTexture> Draw();
        #endregion

        #region c-tor
        public Shape(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Shape()
        {

        }
        #endregion
    }
}
