
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
            this.dado = new System.Windows.Forms.PictureBox();
            this.dadolbl = new System.Windows.Forms.Label();
            this.username_lbl = new System.Windows.Forms.Label();
            this.partida_lbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.playersGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dado)).BeginInit();
            this.SuspendLayout();
            // 
            // playersGridView
            // 
            this.playersGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.playersGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.playersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.playersGridView.Location = new System.Drawing.Point(40, 360);
            this.playersGridView.Name = "playersGridView";
            this.playersGridView.RowHeadersWidth = 51;
            this.playersGridView.RowTemplate.Height = 24;
            this.playersGridView.Size = new System.Drawing.Size(240, 150);
            this.playersGridView.TabIndex = 0;
            // 
            // dado
            // 
            this.dado.Location = new System.Drawing.Point(1160, 464);
            this.dado.Name = "dado";
            this.dado.Size = new System.Drawing.Size(101, 97);
            this.dado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.dado.TabIndex = 13;
            this.dado.TabStop = false;
            this.dado.Click += new System.EventHandler(this.dado_Click_1);
            // 
            // dadolbl
            // 
            this.dadolbl.AutoSize = true;
            this.dadolbl.BackColor = System.Drawing.Color.Transparent;
            this.dadolbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dadolbl.Location = new System.Drawing.Point(1148, 443);
            this.dadolbl.Name = "dadolbl";
            this.dadolbl.Size = new System.Drawing.Size(113, 18);
            this.dadolbl.TabIndex = 15;
            this.dadolbl.Text = "Lanza el dado";
            // 
            // username_lbl
            // 
            this.username_lbl.AutoSize = true;
            this.username_lbl.BackColor = System.Drawing.Color.Transparent;
            this.username_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username_lbl.ForeColor = System.Drawing.Color.White;
            this.username_lbl.Location = new System.Drawing.Point(37, 22);
            this.username_lbl.Name = "username_lbl";
            this.username_lbl.Size = new System.Drawing.Size(90, 18);
            this.username_lbl.TabIndex = 16;
            this.username_lbl.Text = "Username:";
            // 
            // partida_lbl
            // 
            this.partida_lbl.AutoSize = true;
            this.partida_lbl.BackColor = System.Drawing.Color.Transparent;
            this.partida_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.partida_lbl.ForeColor = System.Drawing.Color.White;
            this.partida_lbl.Location = new System.Drawing.Point(37, 60);
            this.partida_lbl.Name = "partida_lbl";
            this.partida_lbl.Size = new System.Drawing.Size(66, 18);
            this.partida_lbl.TabIndex = 17;
            this.partida_lbl.Text = "Partida:";
            // 
            // Tablero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1351, 596);
            this.Controls.Add(this.partida_lbl);
            this.Controls.Add(this.username_lbl);
            this.Controls.Add(this.dadolbl);
            this.Controls.Add(this.dado);
            this.Controls.Add(this.playersGridView);
            this.Name = "Tablero";
            this.Text = "Tablero";
            this.Load += new System.EventHandler(this.Tablero_Load);
            ((System.ComponentModel.ISupportInitialize)(this.playersGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dado)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView playersGridView;
        private System.Windows.Forms.PictureBox dado;
        private System.Windows.Forms.Label dadolbl;
        private System.Windows.Forms.Label username_lbl;
        private System.Windows.Forms.Label partida_lbl;
    }
}