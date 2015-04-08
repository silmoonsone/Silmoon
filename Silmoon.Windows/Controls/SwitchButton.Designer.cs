namespace Silmoon.Windows.Controls
{
    partial class SwitchButton
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SwitchButton));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ctlDisableBIMG = new System.Windows.Forms.PictureBox();
            this.ctlEnableBIMG = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlDisableBIMG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlEnableBIMG)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 20);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // ctlDisableBIMG
            // 
            this.ctlDisableBIMG.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ctlDisableBIMG.BackgroundImage")));
            this.ctlDisableBIMG.Location = new System.Drawing.Point(0, 0);
            this.ctlDisableBIMG.Name = "ctlDisableBIMG";
            this.ctlDisableBIMG.Size = new System.Drawing.Size(60, 20);
            this.ctlDisableBIMG.TabIndex = 1;
            this.ctlDisableBIMG.TabStop = false;
            this.ctlDisableBIMG.Click += new System.EventHandler(this.ctlDisableBIMG_Click);
            // 
            // ctlEnableBIMG
            // 
            this.ctlEnableBIMG.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ctlEnableBIMG.BackgroundImage")));
            this.ctlEnableBIMG.Location = new System.Drawing.Point(-30, 0);
            this.ctlEnableBIMG.Name = "ctlEnableBIMG";
            this.ctlEnableBIMG.Size = new System.Drawing.Size(60, 20);
            this.ctlEnableBIMG.TabIndex = 2;
            this.ctlEnableBIMG.TabStop = false;
            this.ctlEnableBIMG.Click += new System.EventHandler(this.ctlEnableBIMG_Click);
            // 
            // SwitchButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ctlEnableBIMG);
            this.Controls.Add(this.ctlDisableBIMG);
            this.Name = "SwitchButton";
            this.Size = new System.Drawing.Size(60, 20);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlDisableBIMG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlEnableBIMG)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox ctlDisableBIMG;
        private System.Windows.Forms.PictureBox ctlEnableBIMG;
    }
}
