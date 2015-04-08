namespace Silmoon.Windows.Controls
{
    partial class SmTrackBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmTrackBar));
            this.Button = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Button)).BeginInit();
            this.SuspendLayout();
            // 
            // Button
            // 
            this.Button.BackColor = System.Drawing.Color.Transparent;
            this.Button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Button.BackgroundImage")));
            this.Button.Location = new System.Drawing.Point(0, -10);
            this.Button.Name = "Button";
            this.Button.Size = new System.Drawing.Size(12, 34);
            this.Button.TabIndex = 0;
            this.Button.TabStop = false;
            this.Button.MouseLeave += new System.EventHandler(this.S_MouseLeave);
            this.Button.MouseMove += new System.Windows.Forms.MouseEventHandler(this.S_MouseMove);
            this.Button.Click += new System.EventHandler(this.S_Click);
            this.Button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.S_MouseDown);
            this.Button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.S_MouseUp);
            this.Button.MouseEnter += new System.EventHandler(this.S_MouseEnter);
            // 
            // SmTrackBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.Button);
            this.Name = "SmTrackBar";
            this.Size = new System.Drawing.Size(243, 11);
            this.Load += new System.EventHandler(this.SmTrackBar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Button)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox Button;


    }
}