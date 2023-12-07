namespace Cadastre
{
    partial class FormBinary
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
            buttonSave = new Button();
            label1 = new Label();
            button2 = new Button();
            buttonLoad = new Button();
            buttonNew = new Button();
            buttonFindProperty = new Button();
            buttonFindLand = new Button();
            buttonAddpropland = new Button();
            buttonDeleteProperty = new Button();
            buttonEditLand = new Button();
            buttonEditProperty = new Button();
            buttonGenerate = new Button();
            SuspendLayout();
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(24, 24);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 0;
            buttonSave.Text = "Save data";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(151, 32);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // button2
            // 
            button2.Location = new Point(24, 517);
            button2.Name = "button2";
            button2.Size = new Size(103, 35);
            button2.TabIndex = 2;
            button2.Text = "Test structure";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(24, 53);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(75, 23);
            buttonLoad.TabIndex = 3;
            buttonLoad.Text = "Load data";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // buttonNew
            // 
            buttonNew.Location = new Point(24, 82);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(75, 23);
            buttonNew.TabIndex = 4;
            buttonNew.Text = "New file";
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += buttonNew_Click;
            // 
            // buttonFindProperty
            // 
            buttonFindProperty.Location = new Point(24, 133);
            buttonFindProperty.Name = "buttonFindProperty";
            buttonFindProperty.Size = new Size(102, 29);
            buttonFindProperty.TabIndex = 5;
            buttonFindProperty.Text = "Find property";
            buttonFindProperty.UseVisualStyleBackColor = true;
            buttonFindProperty.Click += buttonFindProperty_Click;
            // 
            // buttonFindLand
            // 
            buttonFindLand.Location = new Point(24, 168);
            buttonFindLand.Name = "buttonFindLand";
            buttonFindLand.Size = new Size(102, 28);
            buttonFindLand.TabIndex = 6;
            buttonFindLand.Text = "Find land";
            buttonFindLand.UseVisualStyleBackColor = true;
            buttonFindLand.Click += buttonFindLand_Click;
            // 
            // buttonAddpropland
            // 
            buttonAddpropland.Location = new Point(24, 202);
            buttonAddpropland.Name = "buttonAddpropland";
            buttonAddpropland.Size = new Size(102, 26);
            buttonAddpropland.TabIndex = 7;
            buttonAddpropland.Text = "Add prop/land";
            buttonAddpropland.UseVisualStyleBackColor = true;
            buttonAddpropland.Click += buttonAddpropland_Click;
            // 
            // buttonDeleteProperty
            // 
            buttonDeleteProperty.Location = new Point(24, 234);
            buttonDeleteProperty.Name = "buttonDeleteProperty";
            buttonDeleteProperty.Size = new Size(102, 26);
            buttonDeleteProperty.TabIndex = 8;
            buttonDeleteProperty.Text = "Delete property";
            buttonDeleteProperty.UseVisualStyleBackColor = true;
            buttonDeleteProperty.Click += buttonDeleteProperty_Click;
            // 
            // buttonEditLand
            // 
            buttonEditLand.Location = new Point(24, 266);
            buttonEditLand.Name = "buttonEditLand";
            buttonEditLand.Size = new Size(102, 23);
            buttonEditLand.TabIndex = 9;
            buttonEditLand.Text = "Edit land";
            buttonEditLand.UseVisualStyleBackColor = true;
            buttonEditLand.Click += buttonEditLand_Click;
            // 
            // buttonEditProperty
            // 
            buttonEditProperty.Location = new Point(24, 295);
            buttonEditProperty.Name = "buttonEditProperty";
            buttonEditProperty.Size = new Size(102, 25);
            buttonEditProperty.TabIndex = 10;
            buttonEditProperty.Text = "Edit Property";
            buttonEditProperty.UseVisualStyleBackColor = true;
            buttonEditProperty.Click += buttonEditProperty_Click;
            // 
            // buttonGenerate
            // 
            buttonGenerate.Location = new Point(24, 477);
            buttonGenerate.Name = "buttonGenerate";
            buttonGenerate.Size = new Size(103, 34);
            buttonGenerate.TabIndex = 11;
            buttonGenerate.Text = "Generate data";
            buttonGenerate.UseVisualStyleBackColor = true;
            buttonGenerate.Click += buttonGenerate_Click;
            // 
            // FormBinary
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1187, 564);
            Controls.Add(buttonGenerate);
            Controls.Add(buttonEditProperty);
            Controls.Add(buttonEditLand);
            Controls.Add(buttonDeleteProperty);
            Controls.Add(buttonAddpropland);
            Controls.Add(buttonFindLand);
            Controls.Add(buttonFindProperty);
            Controls.Add(buttonNew);
            Controls.Add(buttonLoad);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(buttonSave);
            Name = "FormBinary";
            Text = "FormBinary";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonSave;
        private Label label1;
        private Button button2;
        private Button buttonLoad;
        private Button buttonNew;
        private Button buttonFindProperty;
        private Button buttonFindLand;
        private Button buttonAddpropland;
        private Button buttonDeleteProperty;
        private Button buttonEditLand;
        private Button buttonEditProperty;
        private Button buttonGenerate;
    }
}