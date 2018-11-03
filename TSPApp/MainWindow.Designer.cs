namespace TSPApp
{
    partial class MainWindow
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
            this.canvasBox = new System.Windows.Forms.PictureBox();
            this.startButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.canvasBox)).BeginInit();
            this.SuspendLayout();
            // 
            // canvasBox
            // 
            this.canvasBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.canvasBox.Location = new System.Drawing.Point(12, 12);
            this.canvasBox.Name = "canvasBox";
            this.canvasBox.Size = new System.Drawing.Size(726, 540);
            this.canvasBox.TabIndex = 0;
            this.canvasBox.TabStop = false;
            this.canvasBox.Paint += new System.Windows.Forms.PaintEventHandler(this.canvasBox_Paint);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(744, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(68, 23);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 564);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.canvasBox);
            this.Name = "MainWindow";
            this.Text = "TSP App";
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.canvasBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox canvasBox;
        private System.Windows.Forms.Button startButton;
    }
}

