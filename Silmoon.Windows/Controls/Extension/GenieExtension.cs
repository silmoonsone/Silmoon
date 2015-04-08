using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using Silmoon.Threading;

namespace Silmoon.Windows.Controls.Extension
{
    public class GenieExtension
    {
        Control control = null;
        /// <summary>
        /// 处理特效的控件的父控件
        /// </summary>
        /// <param name="parentControl"></param>
        public GenieExtension(Control parentControl)
        {
            this.control = parentControl;
        }
        public void ShowControl(Control control)
        {
            ShowControl(control, true, 0);
        }
        public void ShowControl(Control control, bool changColor)
        {
            ShowControl(control, changColor, 0);
        }
        public void ShowControl(Control control, bool changColor, int afterStart)
        {
            if (changColor)
                Threads.ExecAsync(colorProc, new object[] { control, afterStart }, null);
            Threads.ExecAsync(scrollProc, new object[] { control, afterStart }, null);
        }

        public void FocusSlide(Control control, int maxSize, int minSize)
        {
            FocusSlide(control, maxSize, minSize, 5);
        }
        public void FocusSlide(Control control, int maxSize, int minSize, int sleep)
        {
            object obj = (object)new object[] { control, maxSize, minSize, sleep };
            control.GotFocus += new EventHandler(delegate(object sender, EventArgs e)
            {
                Threads.ExecAsync(gotFocus, obj, null);
            });
            control.LostFocus += new EventHandler(delegate(object sender, EventArgs e)
            {
                Threads.ExecAsync(lostFocus, obj, null);
            });
        }

        void gotFocus(object obj)
        {
            Control control = (Control)((object[])obj)[0];
            int max = (int)((object[])obj)[1];
            int sleep = (int)((object[])obj)[3];
            while (control.Width < max)
            {
                this.control.Invoke(new EventHandler(delegate(object sender, EventArgs e)
                {
                    control.Width = control.Width + 5;
                }));
                Thread.Sleep(sleep);
            }
        }
        void lostFocus(object obj)
        {
            Control control = (Control)((object[])obj)[0];
            int min = (int)((object[])obj)[2];
            int sleep = (int)((object[])obj)[3];
            while (control.Width > min)
            {
                this.control.Invoke(new EventHandler(delegate(object sender, EventArgs e)
                {
                    control.Width = control.Width - 5;
                    Thread.Sleep(sleep);
                }));
            }
        }

        void colorProc(object obj)
        {
            object[] o = obj as object[];
            Control panel = o[0] as Control;
            int after = (int)o[1];
            Thread.Sleep(after);

            int a, r, g, b;
            a = panel.BackColor.A;
            r = panel.BackColor.R;
            g = panel.BackColor.G;
            b = panel.BackColor.B;

            control.Invoke(new Action<int>(delegate(int i) { panel.BackColor = Color.White; }), 0);

            bool stop = false;
            while (!stop)
            {
                if (panel.BackColor.A == a && panel.BackColor.R == r && panel.BackColor.G == g && panel.BackColor.B == b)
                    break;

                Color beforeColor = panel.BackColor;
                Color newColor = Color.Empty;
                int na = 0, nr = 0, ng = 0, nb = 0;

                if (panel.BackColor.A > a)
                {
                    na = beforeColor.A - 3;
                    if (na < 0) na = 0;
                }
                else na = beforeColor.A;

                if (panel.BackColor.R > r)
                {
                    nr = beforeColor.R - 3;
                    if (nr < 0) nr = 0;
                }
                else nr = beforeColor.R;

                if (panel.BackColor.G > g)
                {
                    ng = beforeColor.G - 3;
                    if (ng < 0) ng = 0;
                }
                else ng = beforeColor.G;

                if (panel.BackColor.B > b)
                {
                    nb = beforeColor.B - 3;
                    if (nb < 0) nb = 0;
                }
                else nb = beforeColor.B;

                newColor = Color.FromArgb(na, nr, ng, nb);

                if (control.IsDisposed) return;
                control.Invoke(new Action<int>(delegate(int i) { panel.BackColor = newColor; }), 0);
                Thread.Sleep(30);
            }
        }
        void scrollProc(object obj)
        {
            if (control.IsDisposed) return;

            object[] o = obj as object[];
            Control panel = o[0] as Control;
            int after = (int)o[1];
            Thread.Sleep(after);

            int w = panel.Width;
            int h = panel.Height;

            control.Invoke(new Action<int>(delegate(int i)
            {
                panel.Visible = true;
                panel.Width = 1;
                panel.Height = 20;
            }), 0);

            while (panel.Width < w)
            {
                if (control.IsDisposed) return;
                control.Invoke(new Action<int>(delegate(int i) { panel.Width += 5; }), 0);
                Thread.Sleep(5);
            }
            control.Invoke(new Action<int>(delegate(int i) { if (panel.Width > w) panel.Width = w; }), 0);

            control.Invoke(new Action<int>(delegate(int i) { panel.Height = 1; }), 0);
            while (panel.Height < h)
            {
                if (control.IsDisposed) return;
                control.Invoke(new Action<int>(delegate(int i) { panel.Height += 5; }), 0);
                Thread.Sleep(5);
            }
            control.Invoke(new Action<int>(delegate(int i) { if (panel.Height > w) panel.Height = h; }), 0);
        }
    }
}
