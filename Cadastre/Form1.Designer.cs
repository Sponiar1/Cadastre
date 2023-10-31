namespace Cadastre
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
            buttonTest = new Button();
            SuspendLayout();
            // 
            // buttonTest
            // 
            buttonTest.Location = new Point(656, 402);
            buttonTest.Name = "buttonTest";
            buttonTest.Size = new Size(102, 23);
            buttonTest.TabIndex = 0;
            buttonTest.Text = "Test structure";
            buttonTest.UseVisualStyleBackColor = true;
            buttonTest.Click += buttonTest_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonTest);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button buttonTest;
    }
}