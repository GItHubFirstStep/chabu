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
    public partial class line : Form
    {
        private TextBox[] mytext;
        int step = 20;
        public line()
        {
            InitializeComponent();
            x = this.Width;
            y = this.Height;
            setTag(this);
            mytext = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6 };
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
            float count, xs, ys, xe, ye, xi, yi, F, n, x0, y0;
            int time;
            xs = float.Parse(textBox1.Text);
            ys = float.Parse(textBox2.Text);
            xe = float.Parse(textBox3.Text);
            ye = float.Parse(textBox4.Text);
            n = float.Parse(textBox5.Text);//步长
            time = Convert.ToInt32(textBox6.Text);
            count = (Math.Abs((xe - xs)) + Math.Abs((ye - ys))) / n;//总共需要插补的次数
            xi = xs;
            yi = ys;

            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Black, 3);
            PointF P1 = new PointF(panel1.Width / 2 + xs * step, panel1.Height / 2 - ys * step);
            PointF P2 = new PointF(panel1.Width / 2 + xe * step, panel1.Height / 2 - ye * step);
            //Drawline(panel1, P1, P2);
            g.DrawLine(p, P1, P2);
            for (int i = 0; i < count; i++)

            {
                x0 = xi;
                y0 = yi;
                F = (xe - xs) * (yi - ys) - (xi - xs) * (ye - ys);//偏差函数

                if (xe - xs == 0)//当直线垂直

                {
                    if (ye - ys > 0)

                    {
                        yi += n;
                    }
                    else
                    {
                        yi -= n;
                    }

                    Thread.Sleep(time);//延迟0.5S画图 

                    PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                    PointF p2 = new PointF(panel1.Width / 2 + x0 * step, panel1.Height / 2 - y0 * step);
                    Drawline(panel1, p1, p2);

                }
                if (ye - ys == 0)//当直线与X轴平行
                {
                    if (xe - xs > 0)

                    {

                        xi += n;

                    }
                    else

                    {

                        xi -= n;

                    }
                    Thread.Sleep(time);

                    PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                    PointF p2 = new PointF(panel1.Width / 2 + x0 * step, panel1.Height / 2 - y0 * step);
                    Drawline(panel1, p1, p2);
                }
                if ((xe - xs > 0) && (ye - ys > 0))//判断所有插补方向的四种情况 

                {

                    if (F >= 0)

                    {

                        xi += n;

                    }

                    else

                    {
                        yi += n;
                    }

                    Thread.Sleep(time);

                    PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                    PointF p2 = new PointF(panel1.Width / 2 + x0 * step, panel1.Height / 2 - y0 * step);
                    Drawline(panel1, p1, p2);
                }

                if ((xe - xs < 0) && (ye - ys > 0))
                {
                    if (F <= 0)

                    {


                        xi -= n;

                    }

                    else

                    {

                        yi += n;

                    }
                    Thread.Sleep(time);
                    PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                    PointF p2 = new PointF(panel1.Width / 2 + x0 * step, panel1.Height / 2 - y0 * step);
                    Drawline(panel1, p1, p2);
                }
                if ((xe - xs < 0) && (ye - ys < 0))
                {
                    if (F <= 0)
                    {
                        yi -= n;
                    }
                    else
                    {
                        xi -= n;
                    }
                    Thread.Sleep(time);
                    PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                    PointF p2 = new PointF(panel1.Width / 2 + x0 * step, panel1.Height / 2 - y0 * step);
                    Drawline(panel1, p1, p2);
                }
                if ((xe - xs > 0) && (ye - ys < 0))
                {
                    if (F >= 0)
                    {
                        yi -= n;
                    }
                    else
                    {
                        xi += n;
                    }
                    Thread.Sleep(time);
                    PointF p1 = new PointF(panel1.Width / 2 + xi * step, panel1.Height / 2 - yi * step);
                    PointF p2 = new PointF(panel1.Width / 2 + x0 * step, panel1.Height / 2 - y0 * step);
                    Drawline(panel1, p1, p2);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            DrawX(step, panel1);
            DrawY(step, panel1);//画二维坐标轴
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Refresh();//清空画板
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Refresh();//清空画板
        }

        private void line_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / x;
            float newy = (this.Height) / y;
            setControls(newx, newy, this);
        }
    }
}
