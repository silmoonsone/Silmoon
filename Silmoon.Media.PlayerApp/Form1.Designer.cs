using Silmoon.Media.Controls;
namespace Silmoon.Media.Video.Player
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.smTraceBar1 = new Silmoon.Windows.Controls.SmTrackBar();
            this.ToolMenu = new System.Windows.Forms.ToolStrip();
            this.Tool_WMPOpenButton = new System.Windows.Forms.ToolStripButton();
            this.Menu_ProcLabel = new System.Windows.Forms.ToolStripLabel();
            this.TopMenu = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.使用WindowsMediaPlayer打开WToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.退出XToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.视图VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具栏ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playerControl1 = new Silmoon.Media.Controls.VideoControl();
            this.ToolMenu.SuspendLayout();
            this.TopMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // smTraceBar1
            // 
            this.smTraceBar1.BackColor = System.Drawing.Color.Transparent;
            this.smTraceBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.smTraceBar1.Location = new System.Drawing.Point(0, 385);
            this.smTraceBar1.Name = "smTraceBar1";
            this.smTraceBar1.ProgressPercentage = 0;
            this.smTraceBar1.Size = new System.Drawing.Size(585, 11);
            this.smTraceBar1.TabIndex = 1;
            this.smTraceBar1.Value = 0D;
            this.smTraceBar1.Visible = false;
            // 
            // ToolMenu
            // 
            this.ToolMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tool_WMPOpenButton,
            this.Menu_ProcLabel});
            this.ToolMenu.Location = new System.Drawing.Point(0, 24);
            this.ToolMenu.Name = "ToolMenu";
            this.ToolMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolMenu.Size = new System.Drawing.Size(585, 25);
            this.ToolMenu.TabIndex = 2;
            this.ToolMenu.Text = "toolStrip1";
            this.ToolMenu.Visible = false;
            // 
            // Tool_WMPOpenButton
            // 
            this.Tool_WMPOpenButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Tool_WMPOpenButton.Image = global::Silmoon.Media.PlayerApp.Properties.Resources.wmp_1;
            this.Tool_WMPOpenButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Tool_WMPOpenButton.Name = "Tool_WMPOpenButton";
            this.Tool_WMPOpenButton.Size = new System.Drawing.Size(23, 22);
            this.Tool_WMPOpenButton.Text = "使用Windows Media Player打开。";
            this.Tool_WMPOpenButton.Click += new System.EventHandler(this.Tool_WMPOpenButton_Click);
            // 
            // Menu_ProcLabel
            // 
            this.Menu_ProcLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Menu_ProcLabel.Name = "Menu_ProcLabel";
            this.Menu_ProcLabel.Size = new System.Drawing.Size(26, 22);
            this.Menu_ProcLabel.Text = "0%";
            // 
            // TopMenu
            // 
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.视图VToolStripMenuItem});
            this.TopMenu.Location = new System.Drawing.Point(0, 0);
            this.TopMenu.Name = "TopMenu";
            this.TopMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.TopMenu.Size = new System.Drawing.Size(585, 25);
            this.TopMenu.TabIndex = 3;
            this.TopMenu.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开OToolStripMenuItem,
            this.使用WindowsMediaPlayer打开WToolStripMenuItem1,
            this.退出XToolStripMenuItem1});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // 打开OToolStripMenuItem
            // 
            this.打开OToolStripMenuItem.Name = "打开OToolStripMenuItem";
            this.打开OToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.打开OToolStripMenuItem.Text = "打开(&O)";
            this.打开OToolStripMenuItem.Click += new System.EventHandler(this.打开OToolStripMenuItem_Click);
            // 
            // 使用WindowsMediaPlayer打开WToolStripMenuItem1
            // 
            this.使用WindowsMediaPlayer打开WToolStripMenuItem1.Image = global::Silmoon.Media.PlayerApp.Properties.Resources.wmp_1;
            this.使用WindowsMediaPlayer打开WToolStripMenuItem1.Name = "使用WindowsMediaPlayer打开WToolStripMenuItem1";
            this.使用WindowsMediaPlayer打开WToolStripMenuItem1.Size = new System.Drawing.Size(277, 22);
            this.使用WindowsMediaPlayer打开WToolStripMenuItem1.Text = "使用Windows Media Player打开(&W)";
            this.使用WindowsMediaPlayer打开WToolStripMenuItem1.Click += new System.EventHandler(this.使用WindowsMediaPlayer打开WToolStripMenuItem1_Click);
            // 
            // 退出XToolStripMenuItem1
            // 
            this.退出XToolStripMenuItem1.Name = "退出XToolStripMenuItem1";
            this.退出XToolStripMenuItem1.Size = new System.Drawing.Size(277, 22);
            this.退出XToolStripMenuItem1.Text = "退出(&X)";
            // 
            // 视图VToolStripMenuItem
            // 
            this.视图VToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.工具栏ToolStripMenuItem});
            this.视图VToolStripMenuItem.Name = "视图VToolStripMenuItem";
            this.视图VToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.视图VToolStripMenuItem.Text = "视图(&V)";
            // 
            // 工具栏ToolStripMenuItem
            // 
            this.工具栏ToolStripMenuItem.Name = "工具栏ToolStripMenuItem";
            this.工具栏ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.工具栏ToolStripMenuItem.Text = "工具栏";
            this.工具栏ToolStripMenuItem.Click += new System.EventHandler(this.工具栏ToolStripMenuItem_Click);
            // 
            // playerControl1
            // 
            this.playerControl1.A = null;
            this.playerControl1.BackColor = System.Drawing.SystemColors.ControlText;
            this.playerControl1.CurrentPosition = 0D;
            this.playerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playerControl1.FilePath = null;
            this.playerControl1.Location = new System.Drawing.Point(0, 25);
            this.playerControl1.Name = "playerControl1";
            this.playerControl1.ShowTrackBar = true;
            this.playerControl1.Size = new System.Drawing.Size(585, 360);
            this.playerControl1.TabIndex = 0;
            this.playerControl1.V = null;
            this.playerControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.playerControl1_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(585, 396);
            this.Controls.Add(this.playerControl1);
            this.Controls.Add(this.ToolMenu);
            this.Controls.Add(this.TopMenu);
            this.Controls.Add(this.smTraceBar1);
            this.MainMenuStrip = this.TopMenu;
            this.Name = "Form1";
            this.Text = "SmPlayer 播放器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ToolMenu.ResumeLayout(false);
            this.ToolMenu.PerformLayout();
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VideoControl playerControl1;
        private Silmoon.Windows.Controls.SmTrackBar smTraceBar1;
        private System.Windows.Forms.ToolStrip ToolMenu;
        private System.Windows.Forms.ToolStripButton Tool_WMPOpenButton;
        private System.Windows.Forms.ToolStripLabel Menu_ProcLabel;
        private System.Windows.Forms.MenuStrip TopMenu;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 使用WindowsMediaPlayer打开WToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 退出XToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 打开OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 视图VToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具栏ToolStripMenuItem;
    }
}

