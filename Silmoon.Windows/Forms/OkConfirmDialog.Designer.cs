namespace Silmoon.Windows.Forms
{
    partial class OkConfirmDialog
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
            this.ConfirmTextBox = new System.Windows.Forms.TextBox();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ctlMessageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConfirmTextBox
            // 
            this.ConfirmTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.ConfirmTextBox.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ConfirmTextBox.ForeColor = System.Drawing.Color.Red;
            this.ConfirmTextBox.Location = new System.Drawing.Point(12, 12);
            this.ConfirmTextBox.Name = "ConfirmTextBox";
            this.ConfirmTextBox.Size = new System.Drawing.Size(132, 21);
            this.ConfirmTextBox.TabIndex = 0;
            this.ConfirmTextBox.TextChanged += new System.EventHandler(this.ConfirmTextBox_TextChanged);
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Enabled = false;
            this.ConfirmButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ConfirmButton.Location = new System.Drawing.Point(150, 11);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(81, 23);
            this.ConfirmButton.TabIndex = 1;
            this.ConfirmButton.Text = "确定！";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "请输入“OK”确认！";
            // 
            // ctlMessageLabel
            // 
            this.ctlMessageLabel.AutoSize = true;
            this.ctlMessageLabel.ForeColor = System.Drawing.Color.Red;
            this.ctlMessageLabel.Location = new System.Drawing.Point(12, 52);
            this.ctlMessageLabel.Name = "ctlMessageLabel";
            this.ctlMessageLabel.Size = new System.Drawing.Size(23, 12);
            this.ctlMessageLabel.TabIndex = 3;
            this.ctlMessageLabel.Text = "OK?";
            // 
            // OkConfirmDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 70);
            this.Controls.Add(this.ctlMessageLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.ConfirmTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OkConfirmDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OK 确认框";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ConfirmTextBox;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label ctlMessageLabel;
    }
}