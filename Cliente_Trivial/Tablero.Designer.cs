
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
            this.movimientosLbl = new System.Windows.Forms.Label();
            this.ChatTxt = new System.Windows.Forms.TextBox();
            this.ChatBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ChatBox = new System.Windows.Forms.RichTextBox();
            this.tableroBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.playersGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableroBox)).BeginInit();
            this.SuspendLayout();
            // 
            // playersGridView
            // 
            this.playersGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.playersGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.playersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.playersGridView.Location = new System.Drawing.Point(40, 154);
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
            // movimientosLbl
            // 
            this.movimientosLbl.AutoSize = true;
            this.movimientosLbl.BackColor = System.Drawing.Color.Transparent;
            this.movimientosLbl.ForeColor = System.Drawing.Color.White;
            this.movimientosLbl.Location = new System.Drawing.Point(37, 107);
            this.movimientosLbl.Name = "movimientosLbl";
            this.movimientosLbl.Size = new System.Drawing.Size(154, 17);
            this.movimientosLbl.TabIndex = 18;
            this.movimientosLbl.Text = "Possibles movimientos:";
            // 
            // ChatTxt
            // 
            this.ChatTxt.Location = new System.Drawing.Point(40, 539);
            this.ChatTxt.Name = "ChatTxt";
            this.ChatTxt.Size = new System.Drawing.Size(297, 22);
            this.ChatTxt.TabIndex = 19;
            // 
            // ChatBtn
            // 
            this.ChatBtn.Location = new System.Drawing.Point(355, 539);
            this.ChatBtn.Name = "ChatBtn";
            this.ChatBtn.Size = new System.Drawing.Size(75, 23);
            this.ChatBtn.TabIndex = 20;
            this.ChatBtn.Text = "Enviar";
            this.ChatBtn.UseVisualStyleBackColor = true;
            this.ChatBtn.Click += new System.EventHandler(this.ChatBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(163, 355);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 18);
            this.label1.TabIndex = 22;
            this.label1.Text = "CHAT";
            // 
            // ChatBox
            // 
            this.ChatBox.Location = new System.Drawing.Point(40, 376);
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.ReadOnly = true;
            this.ChatBox.Size = new System.Drawing.Size(297, 157);
            this.ChatBox.TabIndex = 23;
            this.ChatBox.Text = "";
            // 
            // tableroBox
            // 
            this.tableroBox.BackColor = System.Drawing.Color.Transparent;
            this.tableroBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableroBox.Location = new System.Drawing.Point(393, 84);
            this.tableroBox.Name = "tableroBox";
            this.tableroBox.Size = new System.Drawing.Size(749, 433);
            this.tableroBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.tableroBox.TabIndex = 24;
            this.tableroBox.TabStop = false;
            this.tableroBox.Click += new System.EventHandler(this.tableroBox_Click);
            // 
            // Tablero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1351, 596);
            this.Controls.Add(this.tableroBox);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ChatBtn);
            this.Controls.Add(this.ChatTxt);
            this.Controls.Add(this.movimientosLbl);
            this.Controls.Add(this.partida_lbl);
            this.Controls.Add(this.username_lbl);
            this.Controls.Add(this.dadolbl);
            this.Controls.Add(this.dado);
            this.Controls.Add(this.playersGridView);
            this.Name = "Tablero";
            this.Text = "Tablero";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Tablero_FormClosing);
            this.Load += new System.EventHandler(this.Tablero_Load);
            ((System.ComponentModel.ISupportInitialize)(this.playersGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableroBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView playersGridView;
        private System.Windows.Forms.PictureBox dado;
        private System.Windows.Forms.Label dadolbl;
        private System.Windows.Forms.Label username_lbl;
        private System.Windows.Forms.Label partida_lbl;
        private System.Windows.Forms.Label movimientosLbl;
        private System.Windows.Forms.TextBox ChatTxt;
        private System.Windows.Forms.Button ChatBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox ChatBox;
        protected System.Windows.Forms.PictureBox tableroBox;
    }
}