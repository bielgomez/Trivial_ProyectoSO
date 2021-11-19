namespace Trivial
{
    partial class Invitacion
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
            this.siButton = new System.Windows.Forms.Button();
            this.noButton = new System.Windows.Forms.Button();
            this.invitationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // siButton
            // 
            this.siButton.AutoSize = true;
            this.siButton.BackColor = System.Drawing.Color.Black;
            this.siButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siButton.ForeColor = System.Drawing.Color.White;
            this.siButton.Location = new System.Drawing.Point(65, 194);
            this.siButton.Name = "siButton";
            this.siButton.Size = new System.Drawing.Size(68, 51);
            this.siButton.TabIndex = 14;
            this.siButton.Text = "SÍ";
            this.siButton.UseVisualStyleBackColor = false;
            this.siButton.Click += new System.EventHandler(this.siButton_Click);
            // 
            // noButton
            // 
            this.noButton.AutoSize = true;
            this.noButton.BackColor = System.Drawing.Color.Black;
            this.noButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noButton.ForeColor = System.Drawing.Color.White;
            this.noButton.Location = new System.Drawing.Point(184, 194);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(72, 46);
            this.noButton.TabIndex = 15;
            this.noButton.Text = "NO";
            this.noButton.UseVisualStyleBackColor = false;
            this.noButton.Click += new System.EventHandler(this.noButton_Click);
            // 
            // invitationLabel
            // 
            this.invitationLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.invitationLabel.Location = new System.Drawing.Point(55, 49);
            this.invitationLabel.Name = "invitationLabel";
            this.invitationLabel.Size = new System.Drawing.Size(221, 117);
            this.invitationLabel.TabIndex = 16;
            this.invitationLabel.Text = "tuputamadre";
            // 
            // Invitacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 289);
            this.Controls.Add(this.invitationLabel);
            this.Controls.Add(this.noButton);
            this.Controls.Add(this.siButton);
            this.Name = "Invitacion";
            this.Text = "Invitacion";
            this.Load += new System.EventHandler(this.Invitacion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button siButton;
        private System.Windows.Forms.Button noButton;
        private System.Windows.Forms.Label invitationLabel;
    }
}