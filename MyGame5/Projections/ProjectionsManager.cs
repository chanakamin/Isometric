using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Isometric.Hints;
using Isometric._3DObjects;

namespace Isometric.Projections
{
    public delegate void OnFinishProjection(); 
    public class ProjectionsManager
    {
        const int N = Global.N;//////////
        public Projection projectoinX { get; set; }
        public Projection projectoinY { get; set; }
        public Projection projectoinZ { get; set; }
        public bool isFinishedX;
        public bool isFinishedY;
        public bool isFinishedZ;

        public static OnExerciseEnd onExercisEnd { get; set; }
        private static int countFinishProjection;
        public void FinishProjection()
        {
            if (++countFinishProjection == 3)
                if (onExercisEnd != null)
                    onExercisEnd(true);
        }
        public ProjectionsManager(Boolean[, ,] m)
        {
            projectoinX = new Projection();
            projectoinY = new Projection();
            projectoinZ = new Projection();
            init(m);
        }
        public ProjectionsManager(Boolean[, ,] m, bool b)
        {
            projectoinX = new Projection(b);
            projectoinY = new Projection(b);
            projectoinZ = new Projection(b);
            init(m);
            projectoinX.onFinishProjection = FinishProjection;
            projectoinY.onFinishProjection = FinishProjection;
            projectoinZ.onFinishProjection = FinishProjection;
            countFinishProjection = 0;
        }
        //public ProjectionsManager(Boolean[, ,] m, bool b, Grid gX, Grid gY, Grid gZ)
        //{
        //    projectoinX = new Projection(b);
        //    projectoinY = new Projection(b);
        //    projectoinZ = new Projection(b);
        //    setCanvases(gX,gY,gZ);
        //    init(m);
        //} 
        private void init(bool[, ,] m)
        {
            for (int i = 0; i < N; i += N / 2)
                for (int j = 0; j < N; j++)
                    for (int k = 0; k < N; k++)
                        if ((m[i, j, k]))
                        {
                            switch (i)
                            {
                                case 0: projectoinX.proj0[k, j] = true; break;
                                case (int)N / 2: projectoinX.proj1[k, j] = true; break;
                                case N - 1: projectoinX.proj2[k, j] = true; break;
                            }
                            projectoinX.realisticProj[k, j] = true;
                        }


            for (int i = 0; i < Global.N; i++)
                for (int j = 0; j < Global.N; j += N / 2)
                    for (int k = 0; k < Global.N; k++)
                        if ((m[i, j, k]))
                        {
                            projectoinY.realisticProj[i, k] = true;
                            switch (j)
                            {
                                case 0: projectoinY.proj0[i, k] = true; break;
                                case (int)N / 2: projectoinY.proj1[i, k] = true; break;
                                case N - 1: projectoinY.proj2[i, k] = true; break;
                                default: break;
                            }
                        }

            for (int i = 0; i < Global.N; i++)
                for (int j = 0; j < Global.N; j++)
                    for (int k = 0; k < Global.N; k += N / 2)
                        if ((m[i, j, k]))
                        {
                            projectoinZ.realisticProj[j, i] = true;
                            switch (k)
                            {
                                case 0: projectoinZ.proj0[j, i] = true; break;
                                case (int)N / 2: projectoinZ.proj1[j, i] = true; break;
                                case N - 1: projectoinZ.proj2[j, i] = true; break;
                                default: break;
                            }
                        }

        }//איך מעבירים מידע בין בנאים?
        public void showHint(Hint hint)
        {
            bool[,] matHintX = new bool[N, N];
            bool[,] matHintY = new bool[N, N];
            bool[,] matHintZ = new bool[N, N];
            int index = 0;
            Line l = hint.LineToBold;
            l.length = (l.length==5) ? 6 : l.length;
            switch (l.axis)
            {
                case (eDimension.X):
                    {
                        matHintX[(int)l.z, (int)l.y] = true;
                        while (index < l.length)
                        {
                            matHintY[(int)l.x + index, (int)l.z] = true;
                            matHintZ[(int)l.y, (int)l.x + index] = true;
                            index++;
                        }
                    }
                    break;
                case (eDimension.Y):
                    {
                        matHintY[(int)l.x, (int)l.z] = true;
                        while (index < l.length)
                        {
                            matHintX[(int)l.z, (int)l.y + index] = true;
                            matHintZ[(int)l.y + index, (int)l.x] = true;
                            index++;
                        }
                    }
                    break;
                case (eDimension.Z):
                    {
                        matHintZ[(int)l.y, (int)l.x] = true;
                        while (index < l.length)
                        {
                            matHintX[(int)l.z + index, (int)l.y] = true;
                            matHintY[(int)l.x, (int)l.z + index] = true;
                            index++;
                        }
                    }
                    break;
            }
            draw();
            Color silver =Color.FromArgb(220, 220, 200, 200);
            projectoinX.DrawLines(matHintX, silver);
            projectoinY.DrawLines(matHintY, silver);
            projectoinZ.DrawLines(matHintZ, silver);
        }
        private void DrawLines()
        {
            projectoinX.DrawLines();
            projectoinY.DrawLines();
            projectoinZ.DrawLines();
        }
        public bool IsFinished()
        {
            return isFinishedX & isFinishedY & isFinishedZ;
        }
        public void setCanvases(Grid gX, Grid gY, Grid gZ)
        {
            gX.Children.Clear();
            gX.Children.Add(projectoinX.getCanvas());
            gY.Children.Clear();
            gY.Children.Add(projectoinY.getCanvas());
            gZ.Children.Clear();
            gZ.Children.Add(projectoinZ.getCanvas());
        }
        public void draw()
        {
            projectoinX.DrawProjection();
            projectoinY.DrawProjection();
            projectoinZ.DrawProjection();
        }

        internal void draw(Color color)
        {
            projectoinX.color = color;
            projectoinY.color = color;
            projectoinZ.color = color;
            draw();
        }
    }
}

