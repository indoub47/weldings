namespace Weldings
{
    partial class StartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnVerifyData = new System.Windows.Forms.Button();
            this.btnWriteData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnVerifyData
            // 
            this.btnVerifyData.Location = new System.Drawing.Point(52, 30);
            this.btnVerifyData.Name = "btnVerifyData";
            this.btnVerifyData.Size = new System.Drawing.Size(75, 23);
            this.btnVerifyData.TabIndex = 0;
            this.btnVerifyData.Text = "Patikrinti";
            this.btnVerifyData.UseVisualStyleBackColor = true;
            this.btnVerifyData.Click += new System.EventHandler(this.btnVerifyData_Click);
            // 
            // btnWriteData
            // 
            this.btnWriteData.Location = new System.Drawing.Point(52, 72);
            this.btnWriteData.Name = "btnWriteData";
            this.btnWriteData.Size = new System.Drawing.Size(75, 23);
            this.btnWriteData.TabIndex = 1;
            this.btnWriteData.Text = "Surašyti į DB";
            this.btnWriteData.UseVisualStyleBackColor = true;
            this.btnWriteData.Click += new System.EventHandler(this.btnWriteData_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnWriteData);
            this.Controls.Add(this.btnVerifyData);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnVerifyData;
        private System.Windows.Forms.Button btnWriteData;
    }
}

