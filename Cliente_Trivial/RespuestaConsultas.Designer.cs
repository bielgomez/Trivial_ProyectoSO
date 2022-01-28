
namespace Trivial
{
    partial class RespuestaConsultas
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
            this.infoLabel = new System.Windows.Forms.Label();
            this.infoGridView = new System.Windows.Forms.DataGridView();
            this.askLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.infoGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.Location = new System.Drawing.Point(40, 74);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(77, 20);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "infoLabel";
            // 
            // infoGridView
            // 
            this.infoGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.infoGridView.Location = new System.Drawing.Point(43, 120);
            this.infoGridView.Name = "infoGridView";
            this.infoGridView.RowHeadersWidth = 51;
            this.infoGridView.RowTemplate.Height = 24;
            this.infoGridView.Size = new System.Drawing.Size(240, 150);
            this.infoGridView.TabIndex = 1;
            // 
            // askLbl
            // 
            this.askLbl.AutoSize = true;
            this.askLbl.Font = new System.Drawing.Font("Ravie", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.askLbl.Location = new System.Drawing.Point(40, 33);
            this.askLbl.Name = "askLbl";
            this.askLbl.Size = new System.Drawing.Size(72, 22);
            this.askLbl.TabIndex = 2;
            this.askLbl.Text = "label1";
            // 
            // RespuestaConsultas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.askLbl);
            this.Controls.Add(this.infoGridView);
            this.Controls.Add(this.infoLabel);
            this.Name = "RespuestaConsultas";
            this.Text = "RespuestaConsultas";
            this.Load += new System.EventHandler(this.RespuestaConsultas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.infoGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.DataGridView infoGridView;
        private System.Windows.Forms.Label askLbl;
    }
}