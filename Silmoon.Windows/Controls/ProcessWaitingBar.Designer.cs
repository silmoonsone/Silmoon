namespace Silmoon.Windows.Controls
{
    partial class ProcessWaitingBar
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
            this.ctlProcessPictrue = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ctlProcessPictrue)).BeginInit();
            this.SuspendLayout();
            // 
            // ctlProcessPictrue
            // 
            this.ctlProcessPictrue.BackColor = System.Drawing.Color.Transparent;
            this.ctlProcessPictrue.Location = new System.Drawing.Point(-110, 0);
            this.ctlProcessPictrue.Name = "ctlProcessPictrue";
            this.ctlProcessPictrue.Size = new System.Drawing.Size(120, 102);
            this.ctlProcessPictrue.TabIndex = 0;
            this.ctlProcessPictrue.TabStop = false;
            // 
            // ProcessWaitingBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.ctlProcessPictrue);
            this.Name = "ProcessWaitingBar";
            this.Size = new System.Drawing.Size(411, 22);
            this.Load += new System.EventHandler(this.ProcessWaitingBar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ctlProcessPictrue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox ctlProcessPictrue;

    }
}
