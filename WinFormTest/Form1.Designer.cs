namespace WinFormTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ctlAesEncryptTestButton = new Button();
            textBox1 = new TextBox();
            ctlCompressTestButton = new Button();
            ctlSubStringTestButton = new Button();
            ctlCopyTestButton = new Button();
            ctlKeyFileEncryptoButton = new Button();
            ctlJsonHelperTestButton = new Button();
            ctlDownloadTestButton = new Button();
            label1 = new Label();
            ctlTestObjectRefButton = new Button();
            ctlJsonRequestTestButton = new Button();
            SuspendLayout();
            // 
            // ctlAesEncryptTestButton
            // 
            ctlAesEncryptTestButton.Location = new Point(495, 12);
            ctlAesEncryptTestButton.Name = "ctlAesEncryptTestButton";
            ctlAesEncryptTestButton.Size = new Size(163, 23);
            ctlAesEncryptTestButton.TabIndex = 1;
            ctlAesEncryptTestButton.Text = "AesEncrypt";
            ctlAesEncryptTestButton.UseVisualStyleBackColor = true;
            ctlAesEncryptTestButton.Click += ctlAesEncryptTestButton_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 12);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(477, 426);
            textBox1.TabIndex = 0;
            // 
            // ctlCompressTestButton
            // 
            ctlCompressTestButton.Location = new Point(495, 41);
            ctlCompressTestButton.Name = "ctlCompressTestButton";
            ctlCompressTestButton.Size = new Size(163, 23);
            ctlCompressTestButton.TabIndex = 2;
            ctlCompressTestButton.Text = "Compress";
            ctlCompressTestButton.UseVisualStyleBackColor = true;
            ctlCompressTestButton.Click += ctlCompressTestButton_Click;
            // 
            // ctlSubStringTestButton
            // 
            ctlSubStringTestButton.Location = new Point(495, 70);
            ctlSubStringTestButton.Name = "ctlSubStringTestButton";
            ctlSubStringTestButton.Size = new Size(163, 23);
            ctlSubStringTestButton.TabIndex = 3;
            ctlSubStringTestButton.Text = "EncodingSubString";
            ctlSubStringTestButton.UseVisualStyleBackColor = true;
            ctlSubStringTestButton.Click += ctlSubStringTestButton_Click;
            // 
            // ctlCopyTestButton
            // 
            ctlCopyTestButton.Location = new Point(495, 99);
            ctlCopyTestButton.Name = "ctlCopyTestButton";
            ctlCopyTestButton.Size = new Size(163, 23);
            ctlCopyTestButton.TabIndex = 4;
            ctlCopyTestButton.Text = "Copy";
            ctlCopyTestButton.UseVisualStyleBackColor = true;
            ctlCopyTestButton.Click += ctlCopyTestButton_Click;
            // 
            // ctlKeyFileEncryptoButton
            // 
            ctlKeyFileEncryptoButton.Location = new Point(495, 128);
            ctlKeyFileEncryptoButton.Name = "ctlKeyFileEncryptoButton";
            ctlKeyFileEncryptoButton.Size = new Size(163, 23);
            ctlKeyFileEncryptoButton.TabIndex = 5;
            ctlKeyFileEncryptoButton.Text = "KeyFileEncrypt";
            ctlKeyFileEncryptoButton.UseVisualStyleBackColor = true;
            ctlKeyFileEncryptoButton.Click += ctlKeyFileEncryptoButton_Click;
            // 
            // ctlJsonHelperTestButton
            // 
            ctlJsonHelperTestButton.Location = new Point(495, 157);
            ctlJsonHelperTestButton.Name = "ctlJsonHelperTestButton";
            ctlJsonHelperTestButton.Size = new Size(163, 23);
            ctlJsonHelperTestButton.TabIndex = 6;
            ctlJsonHelperTestButton.Text = "JsonHelperTest";
            ctlJsonHelperTestButton.UseVisualStyleBackColor = true;
            ctlJsonHelperTestButton.Click += ctlJsonHelperTestButton_Click;
            // 
            // ctlDownloadTestButton
            // 
            ctlDownloadTestButton.Location = new Point(495, 186);
            ctlDownloadTestButton.Name = "ctlDownloadTestButton";
            ctlDownloadTestButton.Size = new Size(163, 23);
            ctlDownloadTestButton.TabIndex = 7;
            ctlDownloadTestButton.Text = "Download";
            ctlDownloadTestButton.UseVisualStyleBackColor = true;
            ctlDownloadTestButton.Click += ctlDownloadTestButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 441);
            label1.Name = "label1";
            label1.Size = new Size(43, 17);
            label1.TabIndex = 8;
            label1.Text = "label1";
            // 
            // ctlTestObjectRefButton
            // 
            ctlTestObjectRefButton.Location = new Point(495, 215);
            ctlTestObjectRefButton.Name = "ctlTestObjectRefButton";
            ctlTestObjectRefButton.Size = new Size(163, 23);
            ctlTestObjectRefButton.TabIndex = 9;
            ctlTestObjectRefButton.Text = "TestObjectRefButton";
            ctlTestObjectRefButton.UseVisualStyleBackColor = true;
            ctlTestObjectRefButton.Click += ctlTestObjectRefButton_Click;
            // 
            // ctlJsonRequestTestButton
            // 
            ctlJsonRequestTestButton.Location = new Point(495, 244);
            ctlJsonRequestTestButton.Name = "ctlJsonRequestTestButton";
            ctlJsonRequestTestButton.Size = new Size(163, 23);
            ctlJsonRequestTestButton.TabIndex = 10;
            ctlJsonRequestTestButton.Text = "JsonRequest Test";
            ctlJsonRequestTestButton.UseVisualStyleBackColor = true;
            ctlJsonRequestTestButton.Click += ctlJsonRequestTestButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(670, 466);
            Controls.Add(ctlJsonRequestTestButton);
            Controls.Add(ctlTestObjectRefButton);
            Controls.Add(label1);
            Controls.Add(ctlDownloadTestButton);
            Controls.Add(ctlJsonHelperTestButton);
            Controls.Add(ctlKeyFileEncryptoButton);
            Controls.Add(ctlCopyTestButton);
            Controls.Add(ctlSubStringTestButton);
            Controls.Add(ctlCompressTestButton);
            Controls.Add(textBox1);
            Controls.Add(ctlAesEncryptTestButton);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ctlAesEncryptTestButton;
        private TextBox textBox1;
        private Button ctlCompressTestButton;
        private Button ctlSubStringTestButton;
        private Button ctlCopyTestButton;
        private Button ctlKeyFileEncryptoButton;
        private Button ctlJsonHelperTestButton;
        private Button ctlDownloadTestButton;
        private Label label1;
        private Button ctlTestObjectRefButton;
        private Button ctlJsonRequestTestButton;
    }
}