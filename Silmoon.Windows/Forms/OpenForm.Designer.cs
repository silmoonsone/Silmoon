namespace Silmoon.Windows.Forms
{
    partial class OpenForm
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
            this.OpenTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.OpenOKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OpenTextBox
            // 
            this.OpenTextBox.Location = new System.Drawing.Point(12, 12);
            this.OpenTextBox.Name = "OpenTextBox";
            this.OpenTextBox.Size = new System.Drawing.Size(287, 21);
            this.OpenTextBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(305, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "浏览(&B)...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OpenOKButton
            // 
            this.OpenOKButton.Location = new System.Drawing.Point(384, 10);
            this.OpenOKButton.Name = "OpenOKButton";
            this.OpenOKButton.Size = new System.Drawing.Size(73, 23);
            this.OpenOKButton.TabIndex = 2;
            this.OpenOKButton.Text = "确定(&O)";
            this.OpenOKButton.UseVisualStyleBackColor = true;
            this.OpenOKButton.Click += new System.EventHandler(this.OpenOKButton_Click);
            // 
            // OpenWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 46);
            this.Controls.Add(this.OpenOKButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.OpenTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "OpenWindow";
            this.Text = "OpenWindow";
            this.Load += new System.EventHandler(this.OpenWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox OpenTextBox;
        public System.Windows.Forms.Button OpenOKButton;
    }
}