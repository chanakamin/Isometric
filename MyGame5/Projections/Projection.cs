using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Input;
using Windows.UI.Popups;

namespace Isometric.Projections
{
    //מצב ציור על ידי המשתמש:
    //להגביל את טווח הסמן עד לגבולות הקנבס////
    //מעבר בין ציור למחיקה- ע"י לחיצה על הסמן?
    //הופעת הסמן רק בעת כניסה לקנבס////
    //נעילת הקנבס בסיום הציור
    //
    public class Projection
    {
        public bool B { get; set; }
        //public Color color = Color.FromArgb(255, 186, 4 * 16 + 15, 6 * 16 + 4);//ba4f64   Color.FromArgb(255, 231, 20, 96);//e71460
        public Color color = Color.FromArgb(255, 231, 20, 96);//e71460
        #region CONSTS
        public bool IsHint = false;
        public const int N = Global.N;///////
        const double whole = 8.5;//6.35;
        const int NwidthOfCanvas = 132;
        const double NradiusOfCorner = 18.5; //18.13;
        const double NstrokeThickness = 10;
        const double N364 = NwidthOfCanvas - NradiusOfCorner * 2;
        const double N227 = NwidthOfCanvas / 2 - NstrokeThickness / 2;
        const double N136 = N364 - N227;
        #endregion
        #region properties
        bool[,] unitedProj;
        public bool[,] proj0 { get; set; }
        public bool[,] proj1 { get; set; }
        public bool[,] proj2 { get; set; }
        public bool[,] realisticProj { get; set; }
        public Canvas canvas { get; set; }

        double[] offsets = { 0.25 * whole, 0.5 * whole, 0.75 * whole, 0, 0.25 * whole };

        List<Thickness> marginInCorner = new List<Thickness>()
            {
                new Thickness(N364,0,0,N364),
                new Thickness(0,0,N364,N364),
                new Thickness(0,N364,N364,0),
                new Thickness(N364,N364,0,0)
            };
        List<Thickness> marginInCenter = new List<Thickness>()
            {
                new Thickness(N136,N227,N227,N136),
                new Thickness(N227,N227,N136,N136),
                new Thickness(N227,N136,N136,N227),
                new Thickness(N136,N136,N227,N227)
            };
        List<Thickness> marginInSide2 = new List<Thickness>()
            {
                new Thickness(N227,0,N136,N364),
                new Thickness(0,N136,N364,N227),
                new Thickness(N136,N364,N227,0),
                new Thickness(N364,N227,0,N136)
            };
        List<Thickness> marginInSide1 = new List<Thickness>()
            {
                new Thickness(N136,0,N227,N364),
                new Thickness(0,N227,N364,N136),
                new Thickness(N227,N364,N136,0),
                new Thickness(N364,N136,0,N227)
            };

        public OnFinishProjection onFinishProjection { get; set; }
        #endregion
        public Projection()
        {
            initProjection();
        }
        private void initProjection()
        {
            canvas = new Canvas();
            canvas.Background = new SolidColorBrush(Colors.White);
            canvas.Margin = new Thickness(1);
            proj0 = new bool[N, N];
            proj1 = new bool[N, N];
            proj2 = new bool[N, N];
            realisticProj = new bool[N, N];
            unitedProj = new bool[N, N];
        }//איך מעבירים מידע בין בנאים?
        public Canvas getCanvas()
        {
            return canvas;
        }
        public void DrawProjection()
        {
            canvas.Children.Clear();
            {
                bool[,] ez0 = (bool[,])proj0.Clone();
                bool[,] ez1 = (bool[,])proj1.Clone();
                bool[,] ez2 = (bool[,])proj2.Clone();
                DrawCorners(ez0);
                DrawCorners(ez1);
                DrawCorners(ez2);
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                        unitedProj[i, j] = (ez0[i, j] | ez1[i, j] | ez2[i, j]);
                DrawLines(unitedProj, color);
            }
        }
        internal void DrawLines()
        {
            DrawLines(unitedProj, color);
            
        }
        internal void DrawLines(bool[,] pro, Color color)
        {
            IsHint = (color != this.color);
            if(IsHint)
            {
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                        pro[i, j] = (unitedProj[i, j]&&pro[i, j]);
            }
            List<int> points = new List<int>();
            listOflines(pro, points);
            double[] value = {   NstrokeThickness / 2,
                                 NstrokeThickness / 2+ NradiusOfCorner*0.7,
                                 NstrokeThickness / 2 + NradiusOfCorner*0.7,
                                 NwidthOfCanvas/2-NradiusOfCorner*0.7,
                                 NwidthOfCanvas/2-NradiusOfCorner*0.7,//לא טעות
                                 NwidthOfCanvas/2,//אמצע
                                 NwidthOfCanvas/2,//לא טעות, למקרה של אזן ואנך באותה שורה- מצטבר רצף של חצי פלוס אחד
                                 NwidthOfCanvas/2+NradiusOfCorner*0.7,
                                 NwidthOfCanvas-NstrokeThickness / 2-NradiusOfCorner*0.7,
                                 NwidthOfCanvas-NstrokeThickness / 2-NradiusOfCorner*0.7,
                                 NwidthOfCanvas-NstrokeThickness/2 ,
                             };
            for (int i = 0; i < points.Count - 4; i += 4)
            {
                Line p = new Line();
                p.X1 = value[points[i]];
                p.Y1 = value[points[i + 1]];
                p.X2 = value[points[i + 2]];
                p.Y2 = value[points[i + 3]];
                p.Stroke = new SolidColorBrush(color);
                //if(color!=this.color)
                //     p.Opacity=0.95;
                p.StrokeStartLineCap = PenLineCap.Round;
                p.StrokeEndLineCap = PenLineCap.Round;
                p.StrokeLineJoin = PenLineJoin.Round;
                p.StrokeThickness = NstrokeThickness;
                canvas.Children.Add(p);
            }
        }

        #region draw Corners
        private void DrawCorners(bool[,] p)
        {
            sides(p);
            corners(p);
            center(p);
        }
        private void center(bool[,] log)
        {
            Point a, b, c, d, e;
            Point[] lp = new Point[] { a, c, d, e, a };
            bool b1, b2, b3, b0 = true;
            b1 = b2 = b3 = true;
            a = new Point(N / 2, N / 2 - 1);//שמאל
            b = new Point(N / 2, N / 2);//מרכז
            c = new Point(N / 2 + 1, N / 2);//למטה 
            d = new Point(N / 2, N / 2 + 1);//ימין
            e = new Point(N / 2 - 1, N / 2);//למעלה
            b0 = check(log, a, b, c);
            b1 = check(log, c, b, d);
            b2 = check(log, d, b, e);
            b3 = check(log, e, b, a);
            if (b0) addCorner(0, marginInCenter[0]);
            if (b2) addCorner(2, marginInCenter[2]);
            if (b1) addCorner(1, marginInCenter[1]);
            if (b3) addCorner(3, marginInCenter[3]);
            if (b0 || b3) log[(int)a.X, (int)a.Y] = false;
            if (b1 || b0) log[(int)c.X, (int)c.Y] = false;
            if (b2 || b1) log[(int)d.X, (int)d.Y] = false;
            if (b3 || b2) log[(int)e.X, (int)e.Y] = false;
            if (b0 || b1 || b2 || b3) log[(int)b.X, (int)b.Y] = false;
        }
        private void sides(bool[,] log)
        {
            Point a, b, c, d;
            bool b1, b2;///a///////////////b//////////////c//////////////////d
            double[] index = { 0, N / 2 - 1,      0,N / 2,      1 , N / 2,       0 ,N / 2 + 1,
                              N / 2 + 1, 0,       N / 2, 0,     N / 2, 1,        N / 2 - 1, 0,
                              N-1, N / 2 + 1,    N-1, N / 2,    N-2 ,N / 2,      N-1 , N / 2 - 1,
                              N / 2 - 1, N-1,    N / 2, N-1,    N / 2, N-2,      N / 2 + 1 ,N-1 };
            for (int i = 0, mini = 0; i < index.Length; i += 8, mini = 0)
            {
                a = new Point(index[i + mini++], index[i + mini++]);
                b = new Point(index[i + mini++], index[i + mini++]);
                c = new Point(index[i + mini++], index[i + mini++]);
                d = new Point(index[i + mini++], index[i + mini++]);
                b1 = check(log, a, b, c);
                if (b1)
                    addCorner(i / 8, marginInSide1[i / 8]);
                b2 = check(log, d, b, c);
                if (b2)
                    addCorner(i / 8 + 1, marginInSide2[i / 8]);
                if (b1)
                    log[(int)a.X, (int)a.Y] = !b1;
                if (b2)
                    log[(int)d.X, (int)d.Y] = !b2;
                if (b2 || b1)
                {
                    log[(int)b.X, (int)b.Y] = !(b2 || b1);
                    log[(int)c.X, (int)c.Y] = !(b2 || b1);
                }
            }
        }
        private void corners(bool[,] log)
        {
            Point a, b, c;
            bool b1;
            double[] index = {0 , N- 2,     0, N -1,      1, N -1,
                                  1,0,      0, 0,       0, 1,
                                 N-2, 0,    N-1, 0,     N-1, 1,
                            N-2,N-1,       N-1,N-1,     N-1,N-2,};
            for (int i = 0, mini = 0; i < index.Length; i += 6, mini = 0)
            {
                a = new Point(index[i + mini++], index[i + mini++]);
                b = new Point(index[i + mini++], index[i + mini++]);
                c = new Point(index[i + mini++], index[i + mini++]);
                b1 = check(log, a, b, c);
                if (b1)
                {
                    addCorner(i / 6, marginInCorner[i / 6]);
                    log[(int)a.X, (int)a.Y] = !(b1);
                    log[(int)b.X, (int)b.Y] = !b1;
                    log[(int)c.X, (int)c.Y] = !b1;
                }
            }
        }
        private bool check(bool[,] log, Point x, Point y, Point z)
        {
            bool a, b, c;
            a = log[(int)x.X, (int)x.Y];
            b = log[(int)y.X, (int)y.Y];
            c = log[(int)z.X, (int)z.Y];
            return (a && b && c);
        }
        void addCorner(int quarter, Thickness margin)
        {
            List<DoubleCollection> listOfCollections = new List<DoubleCollection>() {//לשנות לשם מהותי/למה לא ניתן להגדיר אותו מחוץ לפונקציה?
            new DoubleCollection { 0.25 * whole, 0.75 * whole },  //ראשון
            new DoubleCollection { 0.25 * whole, 0.75 * whole },//שני
            new DoubleCollection { 0, 0.25 * whole, 0.25 * whole , 0.26 * whole }, //שלישי
            new DoubleCollection { 0.25 * whole, 0.75 * whole },//רביעי
            new DoubleCollection { 0.25 * whole, 0.75 * whole },  //ראשון
            };
            Ellipse el = new Ellipse();
            el.Height = NradiusOfCorner * 2;
            el.Width = NradiusOfCorner * 2;
            el.Stroke = new SolidColorBrush(color);
            el.StrokeThickness = NstrokeThickness;
            el.StrokeDashArray = listOfCollections[quarter];
            el.StrokeDashOffset = offsets[quarter];
            el.Margin = margin;
            canvas.Children.Add(el);
        }
        #endregion

        #region drawByUser

        Polyline line;
        Polyline Deleteline;
        bool[,] userBoolienProj = new bool[N, N];
        public bool is_swiping;
        bool draw_status = true;
        int N_2 = (N - 1) / 2;
        public Ellipse myPointer;
        double NwidthOfPointer;
        public Projection(bool b)
        {
            B = b;
            userBoolienProj = new bool[N, N];
            initProjection();
            DrawProjection();
            canvas.PointerEntered += new PointerEventHandler(SwipePointerEntered);
            canvas.PointerPressed += new PointerEventHandler(SwipePointerPressed);
            canvas.PointerMoved += new PointerEventHandler(SwipePointerMoved);
            canvas.PointerReleased += new PointerEventHandler(SwipePointerReleased);
            //canvas.RightTapped += new RightTappedEventHandler(SwipeRightTapped);
            canvas.Tapped += new TappedEventHandler(SwipeTapped);
            canvas.PointerExited += new PointerEventHandler(SwipePointerExited);
            initMyPointer();
        }
        private void SwipePointerEntered(object sender, PointerRoutedEventArgs e)
        {
            myPointer.Visibility = Visibility.Visible;
        }
        private void SwipeTapped(object sender, TappedRoutedEventArgs e)
        {
            PointerStatusChange();
        }
        private void initMyPointer()
        {
            myPointer = new Ellipse();
            myPointer.Style = App.Current.Resources["Draw_Pointer"] as Style;
            myPointer.Tapped += myPointer_Tapped;
            myPointer.RightTapped += myPointer_RightTapped;
            NwidthOfPointer = myPointer.Width;
            canvas.Children.Add(myPointer);
        }
        private void myPointer_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            PointerStatusChange();
        }
        //private void DrawProjection(bool[,] pro)
        //{
        //    ImageGrid.Children.Add(myPointer);
        // }
        private void DrawUserLines(bool[,] pro)
        {
            canvas.Children.Clear();
            List<int> points = new List<int>();
            listOflines(pro, points);
            double[] value = {   NstrokeThickness / 2,
                                 NwidthOfCanvas*1/N+NstrokeThickness / 2,
                                 NwidthOfCanvas*2/N+NstrokeThickness / 2,
                                 NwidthOfCanvas*3/N+NstrokeThickness / 2,
                                 NwidthOfCanvas*4/N+NstrokeThickness / 2,
                                 NwidthOfCanvas/2,//אמצע
                                 NwidthOfCanvas*7/N-NstrokeThickness / 2,
                                 NwidthOfCanvas*8/N-NstrokeThickness / 2,
                                 NwidthOfCanvas*9/N-NstrokeThickness / 2,
                                 NwidthOfCanvas*10/N-NstrokeThickness / 2,
                                 NwidthOfCanvas-NstrokeThickness / 2,
                              };
            for (int i = 0; i < points.Count - 4; i += 4)
            {
                Line p = new Line();
                try
                {
                    p.X1 = value[points[i]];
                    p.Y1 = value[points[i + 1]];
                    p.X2 = value[points[i + 2]];
                    p.Y2 = value[points[i + 3]];
                    //    p.RightTapped += new RightTappedEventHandler(Line_Tapped);
                    p.Stroke = new SolidColorBrush(color);
                    p.StrokeStartLineCap = PenLineCap.Round;
                    p.StrokeEndLineCap = PenLineCap.Round;
                    p.StrokeLineJoin = PenLineJoin.Round;
                    p.Opacity = 0.9;
                    p.StrokeThickness = NstrokeThickness;
                    p.Tapped += new TappedEventHandler(PointerTapped);
                    canvas.Children.Add(p);
                    // massege.Background = new SolidColorBrush(Colors.Black);
                    //massege.Content = "צייר";
                }
                catch (Exception)//לא חושב לעבוד
                {
                    // massege.Background = new SolidColorBrush(Colors.Red);
                    //massege.Content = "חוץ לטווח";
                }

            }
            canvas.Children.Remove(myPointer);
            canvas.Children.Add(myPointer);
        }
        //המעודכן
        private void listOflines(bool[,] M, List<int> pl)
        {
            int i = 0;
            int j = 0;
            int iStart = 0;
            int jStart = 0;
            int sequence = 0;
            for (i = 0; i < N; i += N / 2)//מעבר אופקי
            {
                for (j = 0; j < N; j++)
                    if (M[i, j])
                    {
                        if (sequence == 0) { iStart = i; jStart = j; }//שמור את נק' ההתחלה
                        sequence++;
                    }
                         //else if ((sequence > 0 && !IsHint)||(sequence > 1 && IsHint))
                    else if (sequence > 0)
                    {
                        //if(B)intactHorizontal(ref sequence, ref iStart, ref jStart, ref M);
                        pl.Add(jStart);
                        pl.Add(iStart);
                        pl.Add(jStart + sequence - 1);
                        pl.Add(i);
                        sequence = 0;
                    }
                if (sequence > 0)
                {
                    //if (B) intactHorizontal(ref sequence, ref iStart, ref jStart, ref M);
                    pl.Add(jStart);
                    pl.Add(iStart);
                    pl.Add(jStart + sequence - 1);
                    pl.Add(i);
                    sequence = 0;
                }
            }

            for (j = 0; j < N; j += N / 2)//מעבר אנכי
            {
                for (i = 0, sequence = 0; i < N; i++)
                    if (M[i, j])
                    {
                        if (sequence == 0) { iStart = i; jStart = j; }//שמור את נק' ההתחלה
                        sequence++;
                    }
                    else if (sequence > 0)
                    {
                        //if(B)intactVertical(ref sequence, ref iStart, ref jStart, ref M);
                        pl.Add(jStart);
                        pl.Add(iStart);
                        pl.Add(j);
                        pl.Add(iStart + sequence - 1);
                        sequence = 0;
                    }
                if (sequence > 0)
                {
                    //if(B)intactVertical(ref sequence, ref iStart, ref jStart, ref M);
                    pl.Add(jStart);
                    pl.Add(iStart);
                    pl.Add(j);
                    pl.Add(iStart + sequence - 1);
                }
            }
            pl.AddRange(new List<int>() { 0, 0, 0, 0 });
        }
        //private void intactHorizontal(ref int sequence, ref int iStart, ref int jStart, ref bool[,] M)//פונקציה המתקנת רצפים
        //{
        //    try
        //    {
        //    if (sequence == 1 && (jStart % N_2 != 0))//למקרה קצה נדיר- ניתן להוסיף תנאי על  נק' התחלה תקינה , ואת התנאי האחרון לשנות לאלס ברירת מחדל
        //    {
        //        if ((jStart - 1) % N_2 == 0) { M[iStart, jStart] = false; M[iStart, --jStart] = true; }
        //        if ((jStart + 1) % N_2 == 0) { M[iStart, jStart] = false; M[iStart, ++jStart] = true; }
        //    }
        //    if ((sequence) % (N_2) == 0 || (jStart % N_2 != 0 && (jStart + sequence - 1) % N_2 != 0))//ובציורי אמצע (sequence + 1) % N_2 == 0)||10 9 4 טיפול ברצפים של
        //    {
        //        if (jStart % N_2 != 0) {jStart--; M[iStart, jStart] = true; }
        //        if (jStart % N_2 == 0) { M[iStart, jStart + sequence] = true; sequence++; }//אם הוא המיקום הנכון
        //    } 
        //    if (sequence != 1 && sequence % N_2 == 2 || (sequence == 3&&jStart==N/2-1))//2 3 7 טיפול ברצפים של
        //    {
        //        sequence--;
        //        if (jStart % N_2 == 0) M[iStart, jStart + sequence] = false;//אם הוא המיקום הנכון
        //        else { M[iStart, jStart] = false; jStart++; }
        //    }  }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void intactVertical(ref int sequence, ref int iStart, ref int jStart, ref bool[,] M)//פונקציה המתקנת רצפים
        //{
        //    try
        //    {if (sequence == 1 && (iStart % N_2 != 0))
        //    {
        //        if ((iStart - 1) % N_2 == 0) { M[iStart, jStart] = false; M[--iStart, jStart] = true; }
        //        if ((iStart + 1) % N_2 == 0) { M[iStart, jStart] = false; M[++iStart, jStart] = true; }  
        //    }
        //    if ((sequence) % (N_2) == 0 || (iStart % N_2 != 0 && (iStart + sequence != N)))//||10 9 4 טיפול ברצפים של
        //    {
        //        if (iStart % N_2 == 0) M[iStart + sequence, jStart] = true;//אם הוא המיקום הנכון
        //        else { iStart--; M[iStart, jStart] = true; }
        //        sequence++;
        //    }
        //    if (sequence != 1 && sequence % N_2 == 2 || (sequence == 3 && iStart == N / 2 - 1))//2 3 7 טיפול ברצפים של
        //    {
        //        sequence--;
        //        if (iStart % N_2 == 0) M[iStart + sequence, jStart] = false;//אם הוא המיקום הנכון
        //        else { M[iStart, jStart] = false; iStart++; }
        //    }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        public void SwipePointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pt = e.GetCurrentPoint(canvas);
            Point currentContact = pt.RawPosition;
            myPointer.Margin = new Thickness(currentContact.X - NwidthOfPointer / 2,
               currentContact.Y - NwidthOfPointer / 2, 0, 0);
            is_swiping = true;
            if (draw_status)
            {
                line = new Polyline()
                {
                    StrokeThickness = NstrokeThickness,
                    Stroke = new SolidColorBrush(color),
                    Opacity = 0.9,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeLineJoin = PenLineJoin.Round
                };
                line.Tapped += new TappedEventHandler(PointerTapped);
                canvas.Children.Add(line);
                line.Points.Add(new Point(currentContact.X, currentContact.Y));
            }
            else
            {
                Deleteline = new Polyline()
                {
                    StrokeThickness = NwidthOfPointer,
                    Stroke = new SolidColorBrush(Colors.PowderBlue),
                    Opacity = 0.65,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeLineJoin = PenLineJoin.Round
                };
                canvas.Children.Add(Deleteline);
                Deleteline.Points.Add(new Point(currentContact.X, currentContact.Y));
            }
            canvas.Children.Remove(myPointer);
            canvas.Children.Add(myPointer);
        }
        private void PointerTapped(object sender, TappedRoutedEventArgs e)
        {
            PointerStatusChange();
        }
        public void SwipePointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //if (myPointer.Margin.Bottom < canvas.Margin.) { ;};//אם הסמן בתוך הקנבס
            PointerPoint pt = e.GetCurrentPoint(canvas);
            Point currentContact = pt.RawPosition;
            if (is_swiping)
            {
                myPointer.Margin = new Thickness(currentContact.X - NwidthOfPointer / 2,
                currentContact.Y - NwidthOfPointer / 2, 0, 0);
                if (draw_status)
                {
                    line.Points.Add(new Point(currentContact.X, currentContact.Y));
                    addBool(currentContact.X, currentContact.Y);
                }
                else
                {
                    Deleteline.Points.Add(new Point(currentContact.X, currentContact.Y));
                    addBool(currentContact.X + NwidthOfPointer * 0.3, currentContact.Y);
                    addBool(currentContact.X, currentContact.Y + NwidthOfPointer * 0.3);
                    addBool(currentContact.X - NwidthOfPointer * 0.3, currentContact.Y);
                    addBool(currentContact.X, currentContact.Y - NwidthOfPointer * 0.3);
                }
            }
            //ImageGrid.Children.Add(myPointer);
        }
        public void SwipePointerReleased(object sender, PointerRoutedEventArgs e)
        {
            is_swiping = false;
            DrawUserLines(userBoolienProj);
        }
        private void SwipePointerExited(object sender, PointerRoutedEventArgs e)
        {
            is_swiping = false;
            if (!draw_status)
                PointerStatusChange();
            myPointer.Visibility = Visibility.Collapsed;
            DrawUserLines(userBoolienProj);

        }
        private void SwipeRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            PointerStatusChange();
        }
        private void myPointer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PointerStatusChange();
        }
        private void PointerStatusChange()
        {
            draw_status = !draw_status;
            if (draw_status)
                myPointer.Style = App.Current.Resources["Draw_Pointer"] as Style;
            else
                myPointer.Style = App.Current.Resources["Del_Pointer"] as Style;
            NwidthOfPointer = myPointer.Width;
            // StatusOfPointer.Text = b ? "מצב ציור" : "מצב מחיקה"; 
        }

        private async void addBool(double X, double Y)
        {
            X = (X < NwidthOfCanvas) ? X : NwidthOfCanvas - 10;//10
            X = (X > 0) ? X : 0;
            Y = (Y < NwidthOfCanvas) ? Y : NwidthOfCanvas - 10;//10
            Y = (Y > 0) ? Y : 0;
            X = X / NwidthOfCanvas * N;
            Y = Y / NwidthOfCanvas * N;
            // b0 = Range(ref X, ref Y);//אם לא מדובר במצב מחיקה
            // b0 = Range( X,  Y);//אם לא מדובר במצב מחיקה
            if (!draw_status || ((int)Y % (N / 2) == 0 || (int)X % (N / 2) == 0))
                userBoolienProj[(int)Y, (int)X] = draw_status;
            MessageDialog dialog;
            if (IsFinish())
            {
                if (canvas.IsTapEnabled && onFinishProjection != null) onFinishProjection();
                this.DrawProjection();
                canvas.IsTapEnabled = false;
                canvas.Children.Remove(myPointer);
                canvas.PointerEntered -= SwipePointerEntered;
                canvas.PointerPressed -=SwipePointerPressed;
                canvas.PointerMoved -= SwipePointerMoved;
                canvas.PointerReleased -= SwipePointerReleased;
                //canvas.RightTapped += new RightTappedEventHandler(SwipeRightTapped);
                canvas.Tapped -=SwipeTapped;
                canvas.PointerExited-= SwipePointerExited;
                //dialog = new MessageDialog(" זהו");
                //try
                //{
                //    dialog.ShowAsync();
                //}
                //catch (Exception)
                //{

                //}
            }
        }
        private bool IsFinish()
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (realisticProj[i, j] != userBoolienProj[i, j])
                        return false;
            return true;
        }


        #endregion

    }

    //private void listOflines(bool[,] M, List<int> pl)
    //{
    //    int i = 0;
    //    int j = 0;
    //    int iStart = 0;
    //    int jStart = 0;
    //    int sequence = 0;
    //    for (i = 0; i < N; i += N / 2)//מעבר אופקי
    //    {
    //        for (j = 0; j < N; j++)
    //            if (M[i, j])// % NIndex != 0)
    //            {
    //                if (sequence == 0) { iStart = i; jStart = j; }//שמור את נק' ההתחלה
    //                sequence++;
    //            }
    //            else
    //                sequence = (sequence == 1) ? 0 : sequence;
    //        if (sequence > 1)//ניתן לשמור רצף אחד בלבד בכל שורה
    //        {
    //            pl.Add(jStart);
    //            pl.Add(iStart);
    //            pl.Add(jStart + sequence - 1);
    //            pl.Add(i);
    //        }
    //        sequence = 0;
    //    }

    //    for (j = 0; j < N; j += N / 2)//מעבר אנכי
    //    {
    //        for (i = 0; i < N; i++)
    //            if (M[i, j])// % NIndex != 0)
    //            {
    //                if (sequence == 0) { iStart = i; jStart = j; }//שמור את נק' ההתחלה
    //                sequence++;
    //            }
    //            else
    //                sequence = (sequence == 1) ? 0 : sequence;
    //        if (sequence > 1)//ניתן לשמור רצף אחד בלבד בכל שורה
    //        {
    //            pl.Add(jStart);
    //            pl.Add(iStart);
    //            pl.Add(j);
    //            //                   pl.Add(iStart + sequence);
    //            pl.Add(iStart + sequence - 1);
    //        }
    //        sequence = 0;
    //    }
    //    pl.AddRange(new List<int>() { 0, 0, 0, 0 });
    //}
}
