
namespace New_DIrection
{
    partial class OptionsDialogue
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TimeInterval = new System.Windows.Forms.NumericUpDown();
            this.UniWidth = new System.Windows.Forms.NumericUpDown();
            this.UniHeight = new System.Windows.Forms.NumericUpDown();
            this.OK_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TimeInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UniWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UniHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Timer Interval in Milliseconds";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Universe Width in Cells";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(98, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Universe Height in Cells";
            // 
            // TimeInterval
            // 
            this.TimeInterval.Location = new System.Drawing.Point(246, 42);
            this.TimeInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.TimeInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TimeInterval.Name = "TimeInterval";
            this.TimeInterval.Size = new System.Drawing.Size(120, 20);
            this.TimeInterval.TabIndex = 3;
            this.TimeInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // UniWidth
            // 
            this.UniWidth.Location = new System.Drawing.Point(246, 74);
            this.UniWidth.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.UniWidth.Name = "UniWidth";
            this.UniWidth.Size = new System.Drawing.Size(120, 20);
            this.UniWidth.TabIndex = 4;
            this.UniWidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // UniHeight
            // 
            this.UniHeight.Location = new System.Drawing.Point(246, 106);
            this.UniHeight.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.UniHeight.Name = "UniHeight";
            this.UniHeight.Size = new System.Drawing.Size(120, 20);
            this.UniHeight.TabIndex = 5;
            this.UniHeight.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // OK_Button
            // 
            this.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_Button.Location = new System.Drawing.Point(135, 163);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(75, 23);
            this.OK_Button.TabIndex = 6;
            this.OK_Button.Text = "OK";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new System.Drawing.Point(251, 163);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Button.TabIndex = 7;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            // 
            // OptionsDialogue
            // 
            this.AcceptButton = this.OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_Button;
            this.ClientSize = new System.Drawing.Size(460, 214);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.UniHeight);
            this.Controls.Add(this.UniWidth);
            this.Controls.Add(this.TimeInterval);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "OptionsDialogue";
            this.Text = "OptionsDialogue";
            this.Shown += new System.EventHandler(this.OptionsDialogue_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.TimeInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UniWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UniHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown TimeInterval;
        private System.Windows.Forms.NumericUpDown UniWidth;
        private System.Windows.Forms.NumericUpDown UniHeight;
        private System.Windows.Forms.Button OK_Button;
        private System.Windows.Forms.Button Cancel_Button;
    }
}