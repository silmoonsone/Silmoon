namespace Silmoon.Media.Controls
{
    partial class VideoControl
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.RightKeyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.播放PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.暂停PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sILMOONCOMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smWebPlayer10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.VideoPanel = new System.Windows.Forms.Panel();
            this.ReplayButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.smTrackBar1 = new Silmoon.Windows.Controls.SmTrackBar();
            this.RightKeyMenu.SuspendLayout();
            this.VideoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // RightKeyMenu
            // 
            this.RightKeyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.播放PToolStripMenuItem,
            this.暂停PToolStripMenuItem,
            this.停止SToolStripMenuItem,
            this.sILMOONCOMToolStripMenuItem,
            this.smWebPlayer10ToolStripMenuItem});
            this.RightKeyMenu.Name = "contextMenuStrip1";
            this.RightKeyMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.RightKeyMenu.Size = new System.Drawing.Size(137, 114);
            // 
            // 播放PToolStripMenuItem
            // 
            this.播放PToolStripMenuItem.Name = "播放PToolStripMenuItem";
            this.播放PToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.播放PToolStripMenuItem.Text = "播放(&P)";
            this.播放PToolStripMenuItem.Click += new System.EventHandler(this.播放PToolStripMenuItem_Click);
            // 
            // 暂停PToolStripMenuItem
            // 
            this.暂停PToolStripMenuItem.Name = "暂停PToolStripMenuItem";
            this.暂停PToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.暂停PToolStripMenuItem.Text = "暂停(&P)";
            this.暂停PToolStripMenuItem.Click += new System.EventHandler(this.暂停PToolStripMenuItem_Click);
            // 
            // 停止SToolStripMenuItem
            // 
            this.停止SToolStripMenuItem.Name = "停止SToolStripMenuItem";
            this.停止SToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.停止SToolStripMenuItem.Text = "停止(&S)";
            this.停止SToolStripMenuItem.Click += new System.EventHandler(this.停止SToolStripMenuItem_Click);
            // 
            // sILMOONCOMToolStripMenuItem
            // 
            this.sILMOONCOMToolStripMenuItem.Name = "sILMOONCOMToolStripMenuItem";
            this.sILMOONCOMToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.sILMOONCOMToolStripMenuItem.Text = "SILMOON.COM";
            // 
            // smWebPlayer10ToolStripMenuItem
            // 
            this.smWebPlayer10ToolStripMenuItem.Enabled = false;
            this.smWebPlayer10ToolStripMenuItem.Name = "smWebPlayer10ToolStripMenuItem";
            this.smWebPlayer10ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.smWebPlayer10ToolStripMenuItem.Text = "SmPlayer1.0";
            // 
            // VideoPanel
            // 
            this.VideoPanel.BackColor = System.Drawing.Color.Black;
            this.VideoPanel.Controls.Add(this.ReplayButton);
            this.VideoPanel.Controls.Add(this.label1);
            this.VideoPanel.Location = new System.Drawing.Point(23, 52);
            this.VideoPanel.Name = "VideoPanel";
            this.VideoPanel.Size = new System.Drawing.Size(502, 284);
            this.VideoPanel.TabIndex = 2;
            // 
            // ReplayButton
            // 
            this.ReplayButton.BackColor = System.Drawing.Color.Lime;
            this.ReplayButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ReplayButton.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.ReplayButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.ReplayButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Cyan;
            this.ReplayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReplayButton.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ReplayButton.ForeColor = System.Drawing.Color.White;
            this.ReplayButton.Location = new System.Drawing.Point(221, 126);
            this.ReplayButton.Name = "ReplayButton";
            this.ReplayButton.Size = new System.Drawing.Size(52, 23);
            this.ReplayButton.TabIndex = 1;
            this.ReplayButton.Text = "Replay";
            this.ReplayButton.UseVisualStyleBackColor = false;
            this.ReplayButton.Visible = false;
            this.ReplayButton.Click += new System.EventHandler(this.Replay_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(218, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // smTrackBar1
            // 
            this.smTrackBar1.BackColor = System.Drawing.Color.Black;
            this.smTrackBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.smTrackBar1.Location = new System.Drawing.Point(0, 385);
            this.smTrackBar1.Name = "smTrackBar1";
            this.smTrackBar1.ProgressPercentage = 0;
            this.smTrackBar1.Size = new System.Drawing.Size(554, 11);
            this.smTrackBar1.TabIndex = 3;
            this.smTrackBar1.Value = 0;
            this.smTrackBar1.OnPercentageChange += new Silmoon.Windows.Controls.SmTrackBarHander(this.smTrackBar1_OnPercentageChange);
            // 
            // VideoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ContextMenuStrip = this.RightKeyMenu;
            this.Controls.Add(this.smTrackBar1);
            this.Controls.Add(this.VideoPanel);
            this.Name = "VideoControl";
            this.Size = new System.Drawing.Size(554, 396);
            this.SizeChanged += new System.EventHandler(this.PlayerControl_SizeChanged);
            this.RightKeyMenu.ResumeLayout(false);
            this.VideoPanel.ResumeLayout(false);
            this.VideoPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip RightKeyMenu;
        private System.Windows.Forms.ToolStripMenuItem sILMOONCOMToolStripMenuItem;
        private System.Windows.Forms.Panel VideoPanel;
        private System.Windows.Forms.ToolStripMenuItem 播放PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 暂停PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smWebPlayer10ToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ReplayButton;
        private Silmoon.Windows.Controls.SmTrackBar smTrackBar1;

    }
}
