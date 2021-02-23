
namespace LCTB
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.outputPathTb = new System.Windows.Forms.TextBox();
            this.outputBrowseBtn = new System.Windows.Forms.Button();
            this.inputBrowseBtn = new System.Windows.Forms.Button();
            this.inputPathTb = new System.Windows.Forms.TextBox();
            this.serverCombo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.processBtn = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Input:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Output:";
            // 
            // outputPathTb
            // 
            this.outputPathTb.Location = new System.Drawing.Point(54, 45);
            this.outputPathTb.Name = "outputPathTb";
            this.outputPathTb.Size = new System.Drawing.Size(114, 20);
            this.outputPathTb.TabIndex = 2;
            // 
            // outputBrowseBtn
            // 
            this.outputBrowseBtn.Location = new System.Drawing.Point(173, 44);
            this.outputBrowseBtn.Name = "outputBrowseBtn";
            this.outputBrowseBtn.Size = new System.Drawing.Size(24, 22);
            this.outputBrowseBtn.TabIndex = 3;
            this.outputBrowseBtn.Text = "...";
            this.outputBrowseBtn.UseVisualStyleBackColor = true;
            this.outputBrowseBtn.Click += new System.EventHandler(this.OutputBrowseClick);
            // 
            // inputBrowseBtn
            // 
            this.inputBrowseBtn.Location = new System.Drawing.Point(173, 11);
            this.inputBrowseBtn.Name = "inputBrowseBtn";
            this.inputBrowseBtn.Size = new System.Drawing.Size(24, 22);
            this.inputBrowseBtn.TabIndex = 1;
            this.inputBrowseBtn.Text = "...";
            this.inputBrowseBtn.UseVisualStyleBackColor = true;
            this.inputBrowseBtn.Click += new System.EventHandler(this.InputBrowseClick);
            // 
            // inputPathTb
            // 
            this.inputPathTb.Location = new System.Drawing.Point(54, 12);
            this.inputPathTb.Name = "inputPathTb";
            this.inputPathTb.Size = new System.Drawing.Size(114, 20);
            this.inputPathTb.TabIndex = 0;
            // 
            // serverCombo
            // 
            this.serverCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverCombo.FormattingEnabled = true;
            this.serverCombo.Location = new System.Drawing.Point(250, 11);
            this.serverCombo.Name = "serverCombo";
            this.serverCombo.Size = new System.Drawing.Size(105, 21);
            this.serverCombo.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Server:";
            // 
            // processBtn
            // 
            this.processBtn.Location = new System.Drawing.Point(203, 44);
            this.processBtn.Name = "processBtn";
            this.processBtn.Size = new System.Drawing.Size(152, 22);
            this.processBtn.TabIndex = 8;
            this.processBtn.Text = "Process!";
            this.processBtn.UseVisualStyleBackColor = true;
            this.processBtn.Click += new System.EventHandler(this.ProcessButtonClick);
            // 
            // infoLabel
            // 
            this.infoLabel.Enabled = false;
            this.infoLabel.Location = new System.Drawing.Point(206, 45);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(147, 21);
            this.infoLabel.TabIndex = 9;
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.infoLabel.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 74);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.processBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.serverCombo);
            this.Controls.Add(this.inputBrowseBtn);
            this.Controls.Add(this.inputPathTb);
            this.Controls.Add(this.outputBrowseBtn);
            this.Controls.Add(this.outputPathTb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoL Custom Teams Balancer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox outputPathTb;
        private System.Windows.Forms.Button outputBrowseBtn;
        private System.Windows.Forms.Button inputBrowseBtn;
        private System.Windows.Forms.TextBox inputPathTb;
        private System.Windows.Forms.ComboBox serverCombo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button processBtn;
        private System.Windows.Forms.Label infoLabel;
    }
}

