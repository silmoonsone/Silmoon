namespace Silmoon.Windows.Forms
{
    partial class GenieForm
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
            this.ctlTitleLabel = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.关闭XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctlClosePanelButton = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctlTitleLabel
            // 
            this.ctlTitleLabel.AutoSize = true;
            this.ctlTitleLabel.ContextMenuStrip = this.contextMenuStrip1;
            this.ctlTitleLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ctlTitleLabel.ForeColor = System.Drawing.Color.White;
            this.ctlTitleLabel.Location = new System.Drawing.Point(2, 2);
            this.ctlTitleLabel.Name = "ctlTitleLabel";
            this.ctlTitleLabel.Size = new System.Drawing.Size(68, 12);
            this.ctlTitleLabel.TabIndex = 1;
            this.ctlTitleLabel.Text = "GenieForm";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关闭XToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(117, 26);
            // 
            // 关闭XToolStripMenuItem
            // 
            this.关闭XToolStripMenuItem.Name = "关闭XToolStripMenuItem";
            this.关闭XToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.关闭XToolStripMenuItem.Text = "关闭(&X)";
            this.关闭XToolStripMenuItem.Click += new System.EventHandler(this.关闭XToolStripMenuItem_Click);
            // 
            // ctlClosePanelButton
            // 
            this.ctlClosePanelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.ctlClosePanelButton.Location = new System.Drawing.Point(267, 0);
            this.ctlClosePanelButton.Name = "ctlClosePanelButton";
            this.ctlClosePanelButton.Size = new System.Drawing.Size(17, 17);
            this.ctlClosePanelButton.TabIndex = 2;
            this.ctlClosePanelButton.Click += new System.EventHandler(this.ctlClosePanelButton_Click);
            // 
            // GenieForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.ctlTitleLabel);
            this.Controls.Add(this.ctlClosePanelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GenieForm";
            this.Opacity = 0D;
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ctlTitleLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 关闭XToolStripMenuItem;
        private System.Windows.Forms.Panel ctlClosePanelButton;
    }
}
