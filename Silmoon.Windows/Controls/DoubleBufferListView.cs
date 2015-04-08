using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Silmoon.Windows.Controls
{
    /// <summary>
    /// 启用了双缓冲和优化缓冲等方法减少闪烁的ListView
    /// </summary>
    public class DoubleBufferListView : ListView
    {
        public DoubleBufferListView()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
        public DoubleBufferListView(bool noFastShow)
        {
            if (noFastShow)
                SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            else
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }
        //public DoubleBufferListView()
        //{
        //    SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        //    UpdateStyles();
        //}
        //public DoubleBufferListView()
        //{
        //    SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        //    UpdateStyles();
        //}
    }
}
