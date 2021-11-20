
namespace Trivial
{
    partial class Tablero
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
            this.playersGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.playersGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // playersGridView
            // 
            this.playersGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.playersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.playersGridView.Location = new System.Drawing.Point(42, 220);
            this.playersGridView.Name = "playersGridView";
            this.playersGridView.RowHeadersWidth = 51;
            this.playersGridView.RowTemplate.Height = 24;
            this.playersGridView.Size = new System.Drawing.Size(240, 150);
            this.playersGridView.TabIndex = 0;
            // 
            // Tablero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.playersGridView);
            this.Name = "Tablero";
            this.Text = "Tablero";
            this.Load += new System.EventHandler(this.Tablero_Load);
            ((System.ComponentModel.ISupportInitialize)(this.playersGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView playersGridView;
    }
}