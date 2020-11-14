using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;

namespace Silmoon.Imaging
{
    public class ChartUtil
    {/// <summary>
        /// 根据数据表制作饼图，需要getcolor()方法的支持。。
        /// </summary>
        /// <param name="title">饼图名称。。</param>
        /// <param name="width">饼图宽度，推荐600。。</param>
        /// <param name="height">饼图高度，推荐500。。</param>
        /// <param name="left">圆距左边的距离，推荐100。。</param>
        /// <param name="top">圆距顶部的距离，推荐40。。</param>
        /// <param name="gdt">数据表，2列，多行。。</param>
        /// <returns>Bitmap实例。。</returns>
        public static Bitmap GetPieGraph(string title, int width, int height, int left, int top, DataTable gdt)
        {
            Bitmap objbitmap = new Bitmap(width, height);
            Graphics objgraphics;
            objgraphics = Graphics.FromImage(objbitmap);
            objgraphics.Clear(Color.White);
            StringFormat drawformat = new System.Drawing.StringFormat(StringFormatFlags.DirectionVertical);
            StringFormat drawformat1 = new System.Drawing.StringFormat(StringFormatFlags.DisplayFormatControl);
            objgraphics.DrawString(title, new Font("宋体", 16), Brushes.Black, width / 2 - title.Length * 10, 5, drawformat1);
            PointF symbolleg = new PointF(left, height - top - 45);
            PointF descleg = new PointF(left + 20, height - top - 45);
            //画边框
            objgraphics.DrawRectangle(Pens.Black, 0, 0, width - 1, height - 1);
            //画内小框
            int h = gdt.Rows.Count / 4;
            if (gdt.Rows.Count % 4 > 0)
            {
                h = h + 1;
            }
            objgraphics.DrawRectangle(Pens.Black, left - 10, height - top - 50, (gdt.Rows.Count > 4 ? 4 : gdt.Rows.Count) * 100 + 10, 20 * h);
            //显示什么颜色代表什么的 
            for (int i = 0; i < gdt.Rows.Count; i++)
            {
                if (i >= 4 && (i + 1) % 4 == 1)
                {
                    symbolleg.Y += 20;
                    descleg.Y += 20;
                    symbolleg.X = left;
                    descleg.X = left + 20;
                }
                objgraphics.FillRectangle(new SolidBrush(getcolor(i)), symbolleg.X, symbolleg.Y, 12, 10);
                objgraphics.DrawRectangle(Pens.Black, symbolleg.X, symbolleg.Y, 12, 10);
                objgraphics.DrawString(gdt.Rows[i][0].ToString().Trim(), new Font("宋体", 10), Brushes.Black, descleg);
                symbolleg.X += 100;
                descleg.X += 100;
            }
            float sglcurrentangle = 0;
            float sgltotalangle = 0;
            float sgltotalvalues = 0;
            for (int i = 0; i < gdt.Rows.Count; i++)
            {
                sgltotalvalues += float.Parse(gdt.Rows[i][1].ToString().Trim());
            }
            for (int i = 0; i < gdt.Rows.Count; i++)
            {
                sglcurrentangle = float.Parse(gdt.Rows[i][1].ToString().Trim()) / sgltotalvalues * 360;
                objgraphics.FillPie(new SolidBrush(getcolor(i)), left + 50, top + 30, 300, 300, sgltotalangle, sglcurrentangle);
                objgraphics.DrawPie(Pens.Black, left + 50, top + 30, 300, 300, sgltotalangle, sglcurrentangle);
                //半径 r
                double r = 300 / 2;
                //圆心位置：
                double cX = left + 50 + r;
                double cY = top + 30 + r;
                //圆上点的坐标：
                double dX = r * Math.Cos((360 - sgltotalangle - sglcurrentangle / 2) * 3.14 / 180);
                double dY = r * Math.Sin((360 - sgltotalangle - sglcurrentangle / 2) * 3.14 / 180);
                //圆上位置:
                double dcX = cX + dX;
                double dcY = cY - dY;
                //半径 r
                double r1 = 350 / 2;
                //圆心位置：
                double cX1 = left + 50 + r;
                double cY1 = top + 30 + r;
                //圆上点的坐标：
                double dX1 = r1 * Math.Cos((360 - sgltotalangle - sglcurrentangle / 2) * 3.14 / 180);
                double dY1 = r1 * Math.Sin((360 - sgltotalangle - sglcurrentangle / 2) * 3.14 / 180);
                //圆上位置:
                double dcX1 = cX1 + dX1;
                double dcY1 = cY1 - dY1;

                objgraphics.DrawLine(Pens.Black, Convert.ToInt32(dcX), Convert.ToInt32(dcY), Convert.ToInt32(dcX1), Convert.ToInt32(dcY1));
                if (dX1 >= 0 && dY1 >= 0)
                {
                    objgraphics.DrawString(gdt.Rows[i][0].ToString().Trim() + "(" + Math.Round((float.Parse(gdt.Rows[i][1].ToString().Trim()) / sgltotalvalues * 100), 2).ToString() + "%)", new Font("宋体", 10), Brushes.Black, Convert.ToInt32(dcX1), Convert.ToInt32(dcY1 - 5));
                }
                if (dX1 <= 0 && dY1 >= 0)
                {
                    objgraphics.DrawString(gdt.Rows[i][0].ToString().Trim() + "(" + Math.Round((float.Parse(gdt.Rows[i][1].ToString().Trim()) / sgltotalvalues * 100), 2).ToString() + "%)", new Font("宋体", 10), Brushes.Black, Convert.ToInt32(dcX1 - 25), Convert.ToInt32(dcY1 - 15));
                }
                if (dX1 <= 0 && dY1 <= 0)
                {
                    objgraphics.DrawString(gdt.Rows[i][0].ToString().Trim() + "(" + Math.Round((float.Parse(gdt.Rows[i][1].ToString().Trim()) / sgltotalvalues * 100), 2).ToString() + "%)", new Font("宋体", 10), Brushes.Black, Convert.ToInt32(dcX1 - 30), Convert.ToInt32(dcY1));
                }
                if (dX1 >= 0 && dY1 <= 0)
                {
                    objgraphics.DrawString(gdt.Rows[i][0].ToString().Trim() + "(" + Math.Round((float.Parse(gdt.Rows[i][1].ToString().Trim()) / sgltotalvalues * 100), 2).ToString() + "%)", new Font("宋体", 10), Brushes.Black, Convert.ToInt32(dcX1), Convert.ToInt32(dcY1));
                }
                sgltotalangle += sglcurrentangle;
            }
            return objbitmap;
        }

        //饼图绘制所需方法。。
        protected static Color getcolor(int itemindex)
        {
            Color objcolor;
            if (itemindex >= 14)
            {
                itemindex = itemindex % 14;
            }
            if (itemindex == 0)
            {
                objcolor = Color.DarkMagenta;
            }
            else if (itemindex == 1)
            {
                objcolor = Color.MediumAquamarine;
            }
            else if (itemindex == 2)
            {
                objcolor = Color.DeepSkyBlue;
            }
            else if (itemindex == 3)
            {
                objcolor = Color.DarkRed;
            }
            else if (itemindex == 4)
            {
                objcolor = Color.Pink;
            }
            else if (itemindex == 5)
            {
                objcolor = Color.Salmon;
            }
            else if (itemindex == 6)
            {
                objcolor = Color.Khaki;
            }
            else if (itemindex == 7)
            {
                objcolor = Color.Maroon;
            }
            else if (itemindex == 8)
            {
                objcolor = Color.LawnGreen;
            }
            else if (itemindex == 9)
            {
                objcolor = Color.LightGoldenrodYellow;
            }
            else if (itemindex == 10)
            {
                objcolor = Color.Moccasin;
            }
            else if (itemindex == 11)
            {
                objcolor = Color.YellowGreen;
            }
            else if (itemindex == 12)
            {
                objcolor = Color.DarkCyan;
            }
            else if (itemindex == 13)
            {
                objcolor = Color.SteelBlue;
            }
            else if (itemindex == 14)
            {
                objcolor = Color.Tomato;
            }
            else
            {
                objcolor = Color.SlateGray;
            }
            return objcolor;
        }
    }
}
