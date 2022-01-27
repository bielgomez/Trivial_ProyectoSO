namespace Trivial
{
    partial class Prueba
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Prueba));
            this.Enviar = new System.Windows.Forms.Button();
            this.opcion0 = new System.Windows.Forms.RadioButton();
            this.opcion1 = new System.Windows.Forms.RadioButton();
            this.opcion2 = new System.Windows.Forms.RadioButton();
            this.opcion3 = new System.Windows.Forms.RadioButton();
            this.pregunta_lbl = new System.Windows.Forms.Label();
            this.cat_label = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Enviar
            // 
            this.Enviar.BackColor = System.Drawing.Color.White;
            this.Enviar.Font = new System.Drawing.Font("Ravie", 7.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Enviar.Location = new System.Drawing.Point(400, 424);
            this.Enviar.Name = "Enviar";
            this.Enviar.Size = new System.Drawing.Size(132, 49);
            this.Enviar.TabIndex = 0;
            this.Enviar.Text = "ENVIAR  RESPUESTA";
            this.Enviar.UseVisualStyleBackColor = false;
            this.Enviar.Click += new System.EventHandler(this.Enviar_Click);
            // 
            // opcion0
            // 
            this.opcion0.AutoSize = true;
            this.opcion0.BackColor = System.Drawing.Color.Transparent;
            this.opcion0.Font = new System.Drawing.Font("Ravie", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opcion0.ForeColor = System.Drawing.Color.White;
            this.opcion0.Location = new System.Drawing.Point(216, 192);
            this.opcion0.MaximumSize = new System.Drawing.Size(500, 0);
            this.opcion0.Name = "opcion0";
            this.opcion0.Size = new System.Drawing.Size(97, 23);
            this.opcion0.TabIndex = 1;
            this.opcion0.TabStop = true;
            this.opcion0.Text = "OpcionA";
            this.opcion0.UseVisualStyleBackColor = false;
            // 
            // opcion1
            // 
            this.opcion1.AutoSize = true;
            this.opcion1.BackColor = System.Drawing.Color.Transparent;
            this.opcion1.Font = new System.Drawing.Font("Ravie", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opcion1.ForeColor = System.Drawing.Color.White;
            this.opcion1.Location = new System.Drawing.Point(216, 252);
            this.opcion1.MaximumSize = new System.Drawing.Size(500, 0);
            this.opcion1.Name = "opcion1";
            this.opcion1.Size = new System.Drawing.Size(96, 23);
            this.opcion1.TabIndex = 2;
            this.opcion1.TabStop = true;
            this.opcion1.Text = "OpcionB";
            this.opcion1.UseVisualStyleBackColor = false;
            // 
            // opcion2
            // 
            this.opcion2.AutoSize = true;
            this.opcion2.BackColor = System.Drawing.Color.Transparent;
            this.opcion2.Font = new System.Drawing.Font("Ravie", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opcion2.ForeColor = System.Drawing.Color.White;
            this.opcion2.Location = new System.Drawing.Point(216, 308);
            this.opcion2.MaximumSize = new System.Drawing.Size(500, 0);
            this.opcion2.Name = "opcion2";
            this.opcion2.Size = new System.Drawing.Size(95, 23);
            this.opcion2.TabIndex = 3;
            this.opcion2.TabStop = true;
            this.opcion2.Text = "OpcionC";
            this.opcion2.UseVisualStyleBackColor = false;
            // 
            // opcion3
            // 
            this.opcion3.AutoSize = true;
            this.opcion3.BackColor = System.Drawing.Color.Transparent;
            this.opcion3.Font = new System.Drawing.Font("Ravie", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opcion3.ForeColor = System.Drawing.Color.White;
            this.opcion3.Location = new System.Drawing.Point(216, 363);
            this.opcion3.MaximumSize = new System.Drawing.Size(500, 0);
            this.opcion3.Name = "opcion3";
            this.opcion3.Size = new System.Drawing.Size(96, 23);
            this.opcion3.TabIndex = 4;
            this.opcion3.TabStop = true;
            this.opcion3.Text = "OpcionD";
            this.opcion3.UseVisualStyleBackColor = false;
            // 
            // pregunta_lbl
            // 
            this.pregunta_lbl.AutoSize = true;
            this.pregunta_lbl.BackColor = System.Drawing.Color.Transparent;
            this.pregunta_lbl.Font = new System.Drawing.Font("Ravie", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pregunta_lbl.ForeColor = System.Drawing.Color.White;
            this.pregunta_lbl.Location = new System.Drawing.Point(180, 80);
            this.pregunta_lbl.MaximumSize = new System.Drawing.Size(600, 100);
            this.pregunta_lbl.Name = "pregunta_lbl";
            this.pregunta_lbl.Size = new System.Drawing.Size(170, 22);
            this.pregunta_lbl.TabIndex = 5;
            this.pregunta_lbl.Text = "PREGUNTA_lbl";
            // 
            // cat_label
            // 
            this.cat_label.AutoSize = true;
            this.cat_label.BackColor = System.Drawing.Color.Transparent;
            this.cat_label.Font = new System.Drawing.Font("Ravie", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cat_label.ForeColor = System.Drawing.Color.White;
            this.cat_label.Location = new System.Drawing.Point(348, 18);
            this.cat_label.MaximumSize = new System.Drawing.Size(1000, 0);
            this.cat_label.Name = "cat_label";
            this.cat_label.Size = new System.Drawing.Size(230, 24);
            this.cat_label.TabIndex = 6;
            this.cat_label.Text = "CATEGORIA_label";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer_label
            // 
            this.timer_label.AutoSize = true;
            this.timer_label.BackColor = System.Drawing.Color.Transparent;
            this.timer_label.Font = new System.Drawing.Font("Ravie", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timer_label.ForeColor = System.Drawing.Color.White;
            this.timer_label.Location = new System.Drawing.Point(634, 435);
            this.timer_label.Name = "timer_label";
            this.timer_label.Size = new System.Drawing.Size(151, 38);
            this.timer_label.TabIndex = 7;
            this.timer_label.Text = "TIMER";
            // 
            // Prueba
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(932, 553);
            this.ControlBox = false;
            this.Controls.Add(this.timer_label);
            this.Controls.Add(this.cat_label);
            this.Controls.Add(this.pregunta_lbl);
            this.Controls.Add(this.opcion3);
            this.Controls.Add(this.opcion2);
            this.Controls.Add(this.opcion1);
            this.Controls.Add(this.opcion0);
            this.Controls.Add(this.Enviar);
            this.DoubleBuffered = true;
            this.Name = "Prueba";
            this.Text = "Pregunta";
            this.Load += new System.EventHandler(this.Prueba_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Enviar;
        private System.Windows.Forms.RadioButton opcion0;
        private System.Windows.Forms.RadioButton opcion1;
        private System.Windows.Forms.RadioButton opcion2;
        private System.Windows.Forms.RadioButton opcion3;
        private System.Windows.Forms.Label pregunta_lbl;
        private System.Windows.Forms.Label cat_label;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label timer_label;
    }
}