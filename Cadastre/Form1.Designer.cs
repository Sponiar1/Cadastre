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
            buttonSave = new Button();
            buttonLoad = new Button();
            buttonNew = new Button();
            buttonFindProperty = new Button();
            buttonFindLand = new Button();
            SuspendLayout();
            // 
            // buttonTest
            // 
            buttonTest.Location = new Point(919, 512);
            buttonTest.Name = "buttonTest";
            buttonTest.Size = new Size(124, 42);
            buttonTest.TabIndex = 0;
            buttonTest.Text = "Test structure";
            buttonTest.UseVisualStyleBackColor = true;
            buttonTest.Click += buttonTest_Click;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(47, 12);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(93, 23);
            buttonSave.TabIndex = 1;
            buttonSave.Text = "Save data";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(47, 41);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(93, 23);
            buttonLoad.TabIndex = 2;
            buttonLoad.Text = "Load data";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // buttonNew
            // 
            buttonNew.Location = new Point(47, 70);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(93, 23);
            buttonNew.TabIndex = 3;
            buttonNew.Text = "New Tree";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += buttonNew_Click;
            // 
            // buttonFindProperty
            // 
            buttonFindProperty.Location = new Point(270, 12);
            buttonFindProperty.Name = "buttonFindProperty";
            buttonFindProperty.Size = new Size(109, 34);
            buttonFindProperty.TabIndex = 4;
            buttonFindProperty.Text = "Find property";
            buttonFindProperty.UseVisualStyleBackColor = true;
            buttonFindProperty.Click += buttonFindProperty_Click;
            // 
            // buttonFindLand
            // 
            buttonFindLand.Location = new Point(270, 52);
            buttonFindLand.Name = "buttonFindLand";
            buttonFindLand.Size = new Size(109, 31);
            buttonFindLand.TabIndex = 5;
            buttonFindLand.Text = "Find land";
            buttonFindLand.UseVisualStyleBackColor = true;
            buttonFindLand.Click += buttonFindLand_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1055, 566);
            Controls.Add(buttonFindLand);
            Controls.Add(buttonFindProperty);
            Controls.Add(buttonNew);
            Controls.Add(buttonLoad);
            Controls.Add(buttonSave);
            Controls.Add(buttonTest);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button buttonTest;
        private Button buttonSave;
        private Button buttonLoad;
        private Button buttonNew;
        private Button buttonFindProperty;
        private Button buttonFindLand;
    }
}