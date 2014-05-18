namespace Timetable_v2
{
    partial class SelectionSummary
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
            this.btGetTimetable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btGetTimetable
            // 
            this.btGetTimetable.Location = new System.Drawing.Point(595, 277);
            this.btGetTimetable.Name = "btGetTimetable";
            this.btGetTimetable.Size = new System.Drawing.Size(104, 23);
            this.btGetTimetable.TabIndex = 19;
            this.btGetTimetable.Text = "Get Timetable";
            this.btGetTimetable.UseVisualStyleBackColor = true;
            this.btGetTimetable.Click += new System.EventHandler(this.btGetTimetable_Click);
            // 
            // SelectionSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 314);
            this.Controls.Add(this.btGetTimetable);
            this.Name = "SelectionSummary";
            this.Text = "Module Selection Summary";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SelectionSummary_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btGetTimetable;
    }
}