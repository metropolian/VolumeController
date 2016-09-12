namespace VolumeController
{
    partial class VolumeDisplayForm
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
            this.LbValue = new System.Windows.Forms.Label();
            this.ImIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ImIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // LbValue
            // 
            this.LbValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 56.14286F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.LbValue.ForeColor = System.Drawing.Color.Lime;
            this.LbValue.Location = new System.Drawing.Point(103, 13);
            this.LbValue.Name = "LbValue";
            this.LbValue.Size = new System.Drawing.Size(319, 172);
            this.LbValue.TabIndex = 0;
            this.LbValue.Text = "100";
            this.LbValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ImIcon
            // 
            this.ImIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ImIcon.Location = new System.Drawing.Point(13, 13);
            this.ImIcon.Name = "ImIcon";
            this.ImIcon.Size = new System.Drawing.Size(84, 169);
            this.ImIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ImIcon.TabIndex = 1;
            this.ImIcon.TabStop = false;
            // 
            // VolumeDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(434, 194);
            this.Controls.Add(this.ImIcon);
            this.Controls.Add(this.LbValue);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "VolumeDisplayForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VolumeDisplayForm_FormClosing);
            this.Shown += new System.EventHandler(this.VolumeDisplay_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.ImIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LbValue;
        private System.Windows.Forms.PictureBox ImIcon;
    }
}

