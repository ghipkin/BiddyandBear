namespace GenerateDataLayer
{
    partial class GenerateCode
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
            this.DataLayer = new System.Windows.Forms.Button();
            this.GenerateDataContracts = new System.Windows.Forms.Button();
            this.GenerateMapping = new System.Windows.Forms.Button();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.lblWarning2 = new System.Windows.Forms.Label();
            this.GenerateModel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DataLayer
            // 
            this.DataLayer.Location = new System.Drawing.Point(51, 107);
            this.DataLayer.Name = "DataLayer";
            this.DataLayer.Size = new System.Drawing.Size(139, 23);
            this.DataLayer.TabIndex = 0;
            this.DataLayer.Text = "GenerateDataLayer";
            this.DataLayer.UseVisualStyleBackColor = true;
            this.DataLayer.Click += new System.EventHandler(this.GenerateDataLayer_Click);
            // 
            // GenerateDataContracts
            // 
            this.GenerateDataContracts.Location = new System.Drawing.Point(51, 136);
            this.GenerateDataContracts.Name = "GenerateDataContracts";
            this.GenerateDataContracts.Size = new System.Drawing.Size(139, 23);
            this.GenerateDataContracts.TabIndex = 1;
            this.GenerateDataContracts.Text = "GenerateDataContracts";
            this.GenerateDataContracts.UseVisualStyleBackColor = true;
            this.GenerateDataContracts.Click += new System.EventHandler(this.GenerateDataContracts_Click);
            // 
            // GenerateMapping
            // 
            this.GenerateMapping.Location = new System.Drawing.Point(51, 165);
            this.GenerateMapping.Name = "GenerateMapping";
            this.GenerateMapping.Size = new System.Drawing.Size(139, 23);
            this.GenerateMapping.TabIndex = 0;
            this.GenerateMapping.Text = "Generate Mapping";
            this.GenerateMapping.Click += new System.EventHandler(this.GenerateMapping_Click);
            // 
            // lblInstruction
            // 
            this.lblInstruction.AutoSize = true;
            this.lblInstruction.Location = new System.Drawing.Point(12, 32);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(267, 13);
            this.lblInstruction.TabIndex = 2;
            this.lblInstruction.Text = "For best results only run on a newly published database";
            // 
            // lblWarning2
            // 
            this.lblWarning2.AutoSize = true;
            this.lblWarning2.Location = new System.Drawing.Point(12, 60);
            this.lblWarning2.MaximumSize = new System.Drawing.Size(300, 0);
            this.lblWarning2.Name = "lblWarning2";
            this.lblWarning2.Size = new System.Drawing.Size(251, 26);
            this.lblWarning2.TabIndex = 3;
            this.lblWarning2.Text = "CHECK AGAINST PREVIOUS VERSION BEFORE CHECKING INTO SOURCE CONTROL";
            // 
            // GenerateModel
            // 
            this.GenerateModel.Location = new System.Drawing.Point(51, 195);
            this.GenerateModel.Name = "GenerateModel";
            this.GenerateModel.Size = new System.Drawing.Size(139, 23);
            this.GenerateModel.TabIndex = 4;
            this.GenerateModel.Text = "GenerateModel";
            this.GenerateModel.UseVisualStyleBackColor = true;
            this.GenerateModel.Click += new System.EventHandler(this.GenerateModel_Click);
            // 
            // GenerateCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(355, 262);
            this.Controls.Add(this.GenerateModel);
            this.Controls.Add(this.lblWarning2);
            this.Controls.Add(this.lblInstruction);
            this.Controls.Add(this.GenerateMapping);
            this.Controls.Add(this.GenerateDataContracts);
            this.Controls.Add(this.DataLayer);
            this.Name = "GenerateCode";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DataLayer;
        private System.Windows.Forms.Button GenerateDataContracts;
        private System.Windows.Forms.Button GenerateMapping;
        private System.Windows.Forms.Label lblInstruction;
        private System.Windows.Forms.Label lblWarning2;
        private System.Windows.Forms.Button GenerateModel;
    }
}

