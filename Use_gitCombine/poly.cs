using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Use_gitCombine
{
    public partial class poly : Form
    {
        private TextBox[] mytext;
        int step = 20;
        public poly()
        {
            InitializeComponent();
            x = this.Width;
            y = this.Height;
            setTag(this);
            mytext = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7 };
        }
        private float x;
        private float y;
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }
        private void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }
        public void Drawline(Panel pan, PointF p1, PointF p2)//连接两点

        {
            Graphics g = pan.CreateGraphics();
            Pen p = new Pen(Color.Red, 3);
            g.DrawLine(p, p1, p2);

        }
        public void DrawX(int step, Panel panel)
        {
            int a = (panel.Width / 2) / step;
            Graphics g = panel.CreateGraphics();//绘图工具
            Pen p = new Pen(Color.Blue, 3);//画笔
            Point[] pg = { new Point(0, panel.Height / 2), new Point(panel.Width, panel.Height / 2) };
            g.DrawLine(p, pg[0], pg[1]);
            for (int i = 0; i < a; i++)
            {
                Point[] pg1 = { new Point(panel.Width / 2 + step * i, panel.Height / 2),
                    new Point(panel.Width / 2 + step * i, panel.Height / 2 - 4),
                 new Point(panel.Width / 2 - step * i, panel.Height / 2),
                new Point(panel.Width / 2 - step * i, panel.Height / 2 - 4)};
                g.DrawLine(p, pg1[0], pg1[1]);//X轴正向描点

                g.DrawLine(p, pg1[2], pg1[3]);//X轴负向描点

                g.DrawString(i.ToString(), new Font("宋体", 8f), Brushes.Black, panel.Width / 2 + step * i,
                                                                      panel.Height / 2 + 4);//X轴正向数字
                if (i != 0)
                {
                    g.DrawString("-" + i.ToString(), new Font("宋体", 8f), Brushes.Black, panel.Width / 2 - step * i,
                                                                                          panel.Height / 2 + 4);//X轴负向数字
                }
            }
            g.DrawString("X轴", new Font("宋体", 10f), Brushes.Black, panel.Width - 30, panel.Height / 2 + 20);
        }
        public void DrawY(int step, Panel pan)
        {
            Graphics g = pan.CreateGraphics();
            Pen p = new Pen(Color.Blue, 3);
            Point[] pp = { new Point(pan.Width / 2, pan.Height), new Point(pan.Width / 2, 0) };
            g.DrawLine(p, pp[0], pp[1]);

            int a = (pan.Height / 2) / step;
            for (int i = 0; i < a; i++)
            {
                Point[] po = { new Point(pan.Width / 2, pan.Height / 2 - step * i),
                new Point(pan.Width / 2 + 4, pan.Height / 2 - step * i),
                new Point(pan.Width / 2, pan.Height / 2 + step * i),
                new Point(pan.Width / 2 + 4, pan.Height / 2 + step * i) };//定义需要做坐标系的点
                g.DrawLine(p, po[0], po[1]);
                g.DrawLine(p, po[2], po[3]);


                if (i != 0)
                {
                    g.DrawString(i.ToString(), new Font("宋体", 8f), Brushes.Black, pan.Width / 2 - 20,
                    pan.Height / 2 - step * i);
                    g.DrawString("-" + i.ToString(), new Font("宋体", 8f), Brushes.Black, pan.Width / 2 - 25,
                    pan.Height / 2 + step * i);
                }

            }
            g.DrawString("Y轴", new Font("宋体", 10f), Brushes.Black, pan.Width / 2, 10);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            DrawX(step, panel1);
            DrawY(step, panel1);//画二维坐标轴
        }
        public void Drawpoint(Panel pan, PointF p1, PointF p2)//引用方法，连接各点
        {
            Graphics g = pan.CreateGraphics();
            g.DrawLine(new Pen(Brushes.Red, 2), p1, p2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < mytext.Length; i++)//判断信息是否输入完整
            {

                if (mytext[i].Text.Trim() == "")
                {

                    MessageBox.Show("信息必须填写完整", "警告");
                    return;
                }
            }
            Pen blackPen = new Pen(Color.Black, 3);
            int width = panel1.Width;//包裹矩形的大小
            int height = panel1.Height;
            double x = 0;
            double y = 0;
            double xs, ys, xe, ye, r;
            xs = Convert.ToDouble(textBox1.Text);
            ys = Convert.ToDouble(textBox2.Text);
            xe = Convert.ToDouble(textBox3.Text);
            ye = Convert.ToDouble(textBox4.Text);
            r = Convert.ToDouble(textBox5.Text);
            if (xe == xs)
            {
                double t = Math.Sqrt(r * r - (ye - ys) * (ye - ys) / 4);
                y = (ye + ys) / 2;

                if (ye > ys)
                    x = xe - t;
                else
                    x = xe + t;//当斜率为0.时的圆心坐标
            }
            else
            {
                double c1 = (xe * xe - xs * xs + ye * ye - ys * ys) / (2 * (xe - xs));
                double c2 = (ye - ys) / (xe - xs);  //斜率
                double A = (c2 * c2 + 1);
                double B = (2 * xs * c2 - 2 * c1 * c2 - 2 * ys);
                double C = xs * xs - 2 * xs * c1 + c1 * c1 + ys * ys - r * r;
                y = (-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A);//圆弧圆心坐标
                x = c1 - c2 * y;
            }
            double LL = Math.Sqrt((xe - xs) * (xe - xs) + (ye - ys) * (ye - ys));//起点和终点的距离
            if (r < LL / 2)
            {
                MessageBox.Show("半径不能小于两点距离的一半", "警告");
                return;
            }
            ////判断半径与两点的距离
            if (xe == xs)
            {
                double t = Math.Sqrt(r * r - (ye - ys) * (ye - ys) / 4);
                y = (ye + ys) / 2;

                if (ye > ys)
                    x = xe - t;
                else
                    x = xe + t;//当斜率为0.时的圆心坐标           
            }
            else
            {
                double c1 = (xe * xe - xs * xs + ye * ye - ys * ys) / (2 * (xe - xs));
                double c2 = (ye - ys) / (xe - xs);  //斜率
                double A = (c2 * c2 + 1);
                double B = (2 * xs * c2 - 2 * c1 * c2 - 2 * ys);
                double C = xs * xs - 2 * xs * c1 + c1 * c1 + ys * ys - r * r;
                y = (-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A);//圆弧圆心坐标
                x = c1 - c2 * y;
            }
            if (radioButton1.Checked == true)//逆时针
            {
                double L1 = r;
                double L2 = r;
                double L3 = Math.Sqrt((xe - xs) * (xe - xs) + (ye - ys) * (ye - ys));//起点和终点的距离
                double angle = Math.Acos((L1 * L1 + L2 * L2 - L3 * L3) / (2 * L1 * L2)) * 180 / Math.PI;

                Graphics g = panel1.CreateGraphics();
                float a = panel1.Width / 2 + (float)(x - r) * 20;
                float b = panel1.Height / 2 - (float)(y + r) * 20;//定圆弧的矩形左上角坐标
                float c = 2 * (float)r * 20;//矩形大小
                float angle1 = Convert.ToSingle(angle);//两直线的夹角，用来指定圆弧划过的角度。          
                if (xs <= xe)
                {
                    if (xs < xe)
                    {
                        int angleOfLine = Convert.ToInt32(Math.Atan2(ys - y, xs - x) * 180 / Math.PI);//起始点与圆心连线与X轴的夹角。
                        g.DrawArc(blackPen, a, b, c, c, -angleOfLine, -angle1);
                    }
                    else
                    {

                        int angleOfLine = Convert.ToInt32(Math.Atan2(ys - y, xs - x) * 180 / Math.PI);//起始点与圆心连线与X轴的夹角。
                        g.DrawArc(blackPen, a, b, c, c, -angleOfLine, -angle1);
                    }
                }
                else
                {
                    int angleOfLine = Convert.ToInt32(Math.Atan2(ys - y, xs - x) * 180 / Math.PI);//起始点与圆心连线与X轴的夹角。
                    g.DrawArc(blackPen, a, b, c, c, -angleOfLine, -(360 - angle1));
                }
            }
            else//顺时针
            {
                double x0 = 0, y0 = 0;
                x0 = x;
                y0 = y;

                int angleOfLine1 = Convert.ToInt32(Math.Atan2(ys - y0, xs - x0) * 180 / Math.PI);//起始点与圆心连线与X轴的夹角。
                double L11 = r;
                double L21 = r;
                double L31 = Math.Sqrt((xe - xs) * (xe - xs) + (ye - ys) * (ye - ys));//起点和终点的距离
                double angle11 = Math.Acos((L11 * L11 + L21 * L21 - L31 * L31) / (2 * L11 * L21)) * 180 / Math.PI;
                float angle111 = Convert.ToSingle(angle11);//两直线的夹角，用来指定圆弧划过的角度。
                Graphics g1 = panel1.CreateGraphics();
                float a1 = panel1.Width / 2 + (float)(x0 - r) * 20;
                float b1 = panel1.Height / 2 - (float)(y0 + r) * 20;//定圆弧的矩形左上角坐标
                float cc = 2 * (float)r * 20;//矩形大小
                if (xe >= xs)
                {
                    g1.DrawArc(blackPen, a1, b1, cc, cc, -angleOfLine1, 360 - angle111);
                }
                else
                {
                    g1.DrawArc(blackPen, a1, b1, cc, cc, -angleOfLine1, angle111);
                }
            }
            /////////逆时针引用
            double xm, ym, Fm, n;
            int s;
            n = Convert.ToDouble(textBox6.Text);//加工步长
            s = Convert.ToInt32(textBox7.Text);//加工延迟
            xm = xs;
            ym = ys;
            float xi, yi, x2, y2;
            if (radioButton1.Checked == true)
            {
                while (Math.Abs(xm - xe) > 1E-6 || Math.Abs(ym - ye) > 1E-6)
                {
                    Fm = (xm - x) * (xm - x) + (ym - y) * (ym - y) - r * r;
                    xi = (float)xm;
                    yi = (float)ym;
                    if (xm > x && ym >= y)//Nr1
                    {
                        if (Math.Abs(xm - xe) <= 1E-6 && Math.Abs(ym - ye) <= 1E-6)
                        {
                            break;
                        }
                        else if (Fm >= 0)
                        {
                            xm = xm - n;
                        }
                        else
                        {
                            //Fm =Fm+ 1 + 2 * ym;
                            ym = ym + n;
                        }
                        Thread.Sleep(s);

                        x2 = (float)xm; y2 = (float)ym;
                        PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                        PointF p2 = new PointF(panel1.Width / 2 + x2 * step, panel1.Height / 2 - y2 * step);
                        Drawpoint(panel1, p1, p2);
                    }

                    xi = (float)xm;
                    yi = (float)ym;
                    if (xm <= x && ym > y)//Nr2
                    {
                        if (Math.Abs(xm - xe) <= 1E-6 && Math.Abs(ym - ye) <= 1E-6)
                        {
                            break;
                        }

                        else if (Fm >= 0)
                        {

                            ym -= n;
                        }
                        else
                        {

                            xm -= n;
                        }
                        Thread.Sleep(s);

                        x2 = (float)xm; y2 = (float)ym;
                        PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                        PointF p2 = new PointF(panel1.Width / 2 + x2 * step, panel1.Height / 2 - y2 * step);
                        Drawpoint(panel1, p1, p2);
                    }
                    xi = (float)xm;
                    yi = (float)ym;
                    if (xm < x && ym <= y)//Nr3
                    {

                        if (Math.Abs(xm - xe) <= 1E-6 && Math.Abs(ym - ye) <= 1E-6)
                        {
                            break;
                        }
                        else if (Fm >= 0)
                        {

                            xm += n;
                        }
                        else
                        {

                            ym -= n;
                        }
                        Thread.Sleep(s);

                        x2 = (float)xm; y2 = (float)ym;
                        PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                        PointF p2 = new PointF(panel1.Width / 2 + x2 * step, panel1.Height / 2 - y2 * step);
                        Drawpoint(panel1, p1, p2);
                    }
                    xi = (float)xm;
                    yi = (float)ym;
                    if (xm >= x && ym < y)//Nr4
                    {
                        if (Math.Abs(xm - xe) <= 1E-6 && Math.Abs(ym - ye) <= 1E-6)
                        {
                            break;
                        }
                        else if (Fm >= 0)
                        {
                            ym += n;
                        }
                        else
                        {
                            xm += n;
                        }
                        Thread.Sleep(s);

                        x2 = (float)xm; y2 = (float)ym;
                        PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                        PointF p2 = new PointF(panel1.Width / 2 + x2 * step, panel1.Height / 2 - y2 * step);
                        Drawpoint(panel1, p1, p2);
                    }
                }
            }
            else//顺时针引用
            {
                while (Math.Abs(xm - xe) > 1E-6 || Math.Abs(ym - ye) > 1E-6)
                {
                    Fm = (xm - x) * (xm - x) + (ym - y) * (ym - y) - r * r;
                    xi = (float)xm;
                    yi = (float)ym;
                    if (xm > x && ym <= y)//Sr4
                    {
                        if (Math.Abs(xm - xe) <= 1E-6 && Math.Abs(ym - ye) <= 1E-6)
                        {
                            break;
                        }
                        else if (Fm >= 0)
                        {
                            xm -= n;
                        }
                        else
                        {
                            ym -= n;
                        }
                        Thread.Sleep(s);
                        x2 = (float)xm; y2 = (float)ym;
                        PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                        PointF p2 = new PointF(panel1.Width / 2 + x2 * step, panel1.Height / 2 - y2 * step);
                        Drawpoint(panel1, p1, p2);
                    }
                    xi = (float)xm;
                    yi = (float)ym;
                    if (xm <= x && ym < y)//Sr3
                    {

                        if (Math.Abs(xm - xe) <= 1E-6 && Math.Abs(ym - ye) <= 1E-6)
                        {
                            break;
                        }
                        else if (Fm >= 0)
                        {

                            ym += n;
                        }
                        else
                        {

                            xm -= n;
                        }
                        Thread.Sleep(s);

                        x2 = (float)xm; y2 = (float)ym;
                        PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                        PointF p2 = new PointF(panel1.Width / 2 + x2 * step, panel1.Height / 2 - y2 * step);
                        Drawpoint(panel1, p1, p2);
                    }
                    xi = (float)xm;
                    yi = (float)ym;
                    if (xm < x && ym >= y)//Sr2
                    {
                        if (Math.Abs(xm - xe) <= 1E-6 && Math.Abs(ym - ye) <= 1E-6)
                        {
                            break;
                        }

                        else if (Fm >= 0)
                        {

                            xm += n;
                        }
                        else
                        {

                            ym += n;
                        }
                        Thread.Sleep(s);

                        x2 = (float)xm; y2 = (float)ym;
                        PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                        PointF p2 = new PointF(panel1.Width / 2 + x2 * step, panel1.Height / 2 - y2 * step);
                        Drawpoint(panel1, p1, p2);
                    }

                    xi = (float)xm;
                    yi = (float)ym;
                    if (xm >= x && ym > y)//sr1
                    {
                        if (Math.Abs(xm - xe) <= 1E-6 && Math.Abs(ym - ye) <= 1E-6)
                        {
                            break;
                        }
                        else if (Fm >= 0)
                        {
                            ym = ym - n;

                        }
                        else
                        {
                            xm = xm + n;
                        }
                        Thread.Sleep(s);

                        x2 = (float)xm; y2 = (float)ym;
                        PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                        PointF p2 = new PointF(panel1.Width / 2 + x2 * step, panel1.Height / 2 - y2 * step);
                        Drawpoint(panel1, p1, p2);
                    }

                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }

        private void poly_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();//清空文本框
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Refresh();//刷新图案
        }

        private void poly_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / x;
            float newy = (this.Height) / y;
            setControls(newx, newy, this);
        }
    }
}
