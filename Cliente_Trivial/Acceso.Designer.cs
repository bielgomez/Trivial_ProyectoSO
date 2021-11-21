namespace Trivial
{
    partial class Acceso
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Login = new System.Windows.Forms.Button();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.accederBox = new System.Windows.Forms.GroupBox();
            this.candadoBox = new System.Windows.Forms.PictureBox();
            this.conexion = new System.Windows.Forms.Button();
            this.luz = new System.Windows.Forms.Button();
            this.registroBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.password2Box = new System.Windows.Forms.TextBox();
            this.mailBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.userBox = new System.Windows.Forms.TextBox();
            this.Registrarme = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.consultaBox = new System.Windows.Forms.GroupBox();
            this.Preguntar = new System.Windows.Forms.Button();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.duracion = new System.Windows.Forms.RadioButton();
            this.Contraseña = new System.Windows.Forms.RadioButton();
            this.consultasButton = new System.Windows.Forms.Button();
            this.ConectadosGridView = new System.Windows.Forms.DataGridView();
            this.labelConectados = new System.Windows.Forms.Label();
            this.nameUserTxt = new System.Windows.Forms.Label();
            this.invitarButton = new System.Windows.Forms.Button();
            this.accederBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.candadoBox)).BeginInit();
            this.registroBox.SuspendLayout();
            this.consultaBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConectadosGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // Login
            // 
            this.Login.BackColor = System.Drawing.Color.Black;
            this.Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Login.ForeColor = System.Drawing.Color.White;
            this.Login.Location = new System.Drawing.Point(38, 220);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(128, 45);
            this.Login.TabIndex = 0;
            this.Login.Text = "Entrar";
            this.Login.UseVisualStyleBackColor = false;
            this.Login.Click += new System.EventHandler(this.Login_Click);
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(28, 151);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(128, 22);
            this.PasswordBox.TabIndex = 2;
            this.PasswordBox.TextChanged += new System.EventHandler(this.PasswordBox_TextChanged);
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(28, 59);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(128, 22);
            this.NameBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(24, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Nombre usuario";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(34, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Contraseña";
            // 
            // accederBox
            // 
            this.accederBox.BackColor = System.Drawing.Color.Black;
            this.accederBox.Controls.Add(this.candadoBox);
            this.accederBox.Controls.Add(this.PasswordBox);
            this.accederBox.Controls.Add(this.label2);
            this.accederBox.Controls.Add(this.Login);
            this.accederBox.Controls.Add(this.label1);
            this.accederBox.Controls.Add(this.NameBox);
            this.accederBox.ForeColor = System.Drawing.Color.White;
            this.accederBox.Location = new System.Drawing.Point(12, 331);
            this.accederBox.Name = "accederBox";
            this.accederBox.Size = new System.Drawing.Size(215, 319);
            this.accederBox.TabIndex = 6;
            this.accederBox.TabStop = false;
            this.accederBox.Text = "Acceder";
            // 
            // candadoBox
            // 
            this.candadoBox.BackColor = System.Drawing.Color.White;
            this.candadoBox.Location = new System.Drawing.Point(162, 139);
            this.candadoBox.Name = "candadoBox";
            this.candadoBox.Size = new System.Drawing.Size(31, 34);
            this.candadoBox.TabIndex = 6;
            this.candadoBox.TabStop = false;
            this.candadoBox.Click += new System.EventHandler(this.candadoBox_Click);
            // 
            // conexion
            // 
            this.conexion.AutoSize = true;
            this.conexion.BackColor = System.Drawing.Color.Black;
            this.conexion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conexion.ForeColor = System.Drawing.Color.White;
            this.conexion.Location = new System.Drawing.Point(1209, 20);
            this.conexion.Name = "conexion";
            this.conexion.Size = new System.Drawing.Size(153, 45);
            this.conexion.TabIndex = 8;
            this.conexion.Text = "Conectar";
            this.conexion.UseVisualStyleBackColor = false;
            this.conexion.Click += new System.EventHandler(this.conexion_Click);
            // 
            // luz
            // 
            this.luz.ForeColor = System.Drawing.Color.Teal;
            this.luz.Location = new System.Drawing.Point(1176, 32);
            this.luz.Name = "luz";
            this.luz.Size = new System.Drawing.Size(27, 26);
            this.luz.TabIndex = 9;
            this.luz.UseVisualStyleBackColor = true;
            // 
            // registroBox
            // 
            this.registroBox.BackColor = System.Drawing.Color.Black;
            this.registroBox.Controls.Add(this.label3);
            this.registroBox.Controls.Add(this.password2Box);
            this.registroBox.Controls.Add(this.mailBox);
            this.registroBox.Controls.Add(this.label4);
            this.registroBox.Controls.Add(this.userBox);
            this.registroBox.Controls.Add(this.Registrarme);
            this.registroBox.Controls.Add(this.label5);
            this.registroBox.ForeColor = System.Drawing.Color.White;
            this.registroBox.Location = new System.Drawing.Point(1192, 331);
            this.registroBox.Name = "registroBox";
            this.registroBox.Size = new System.Drawing.Size(194, 319);
            this.registroBox.TabIndex = 10;
            this.registroBox.TabStop = false;
            this.registroBox.Text = "Registrarse";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(61, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "E-mail";
            // 
            // password2Box
            // 
            this.password2Box.Location = new System.Drawing.Point(38, 126);
            this.password2Box.Name = "password2Box";
            this.password2Box.Size = new System.Drawing.Size(128, 22);
            this.password2Box.TabIndex = 12;
            // 
            // mailBox
            // 
            this.mailBox.Location = new System.Drawing.Point(38, 192);
            this.mailBox.Name = "mailBox";
            this.mailBox.Size = new System.Drawing.Size(128, 22);
            this.mailBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(45, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 20);
            this.label4.TabIndex = 16;
            this.label4.Text = "Contraseña";
            // 
            // userBox
            // 
            this.userBox.Location = new System.Drawing.Point(38, 59);
            this.userBox.Name = "userBox";
            this.userBox.Size = new System.Drawing.Size(128, 22);
            this.userBox.TabIndex = 13;
            // 
            // Registrarme
            // 
            this.Registrarme.BackColor = System.Drawing.Color.Black;
            this.Registrarme.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Registrarme.ForeColor = System.Drawing.Color.White;
            this.Registrarme.Location = new System.Drawing.Point(25, 244);
            this.Registrarme.Name = "Registrarme";
            this.Registrarme.Size = new System.Drawing.Size(153, 45);
            this.Registrarme.TabIndex = 14;
            this.Registrarme.Text = "Registrarme";
            this.Registrarme.UseVisualStyleBackColor = false;
            this.Registrarme.Click += new System.EventHandler(this.Registrarme_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(24, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "Nombre usuario";
            // 
            // consultaBox
            // 
            this.consultaBox.BackColor = System.Drawing.Color.Black;
            this.consultaBox.Controls.Add(this.Preguntar);
            this.consultaBox.Controls.Add(this.radioButton2);
            this.consultaBox.Controls.Add(this.duracion);
            this.consultaBox.Controls.Add(this.Contraseña);
            this.consultaBox.ForeColor = System.Drawing.Color.White;
            this.consultaBox.Location = new System.Drawing.Point(12, 12);
            this.consultaBox.Name = "consultaBox";
            this.consultaBox.Size = new System.Drawing.Size(470, 250);
            this.consultaBox.TabIndex = 11;
            this.consultaBox.TabStop = false;
            this.consultaBox.Text = "¿Qué quieres saber?";
            // 
            // Preguntar
            // 
            this.Preguntar.BackColor = System.Drawing.Color.Black;
            this.Preguntar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Preguntar.ForeColor = System.Drawing.Color.White;
            this.Preguntar.Location = new System.Drawing.Point(315, 95);
            this.Preguntar.Name = "Preguntar";
            this.Preguntar.Size = new System.Drawing.Size(135, 47);
            this.Preguntar.TabIndex = 17;
            this.Preguntar.Text = "Preguntar";
            this.Preguntar.UseVisualStyleBackColor = false;
            this.Preguntar.Click += new System.EventHandler(this.Preguntar_Click);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.BackColor = System.Drawing.Color.Transparent;
            this.radioButton2.ForeColor = System.Drawing.Color.White;
            this.radioButton2.Location = new System.Drawing.Point(31, 184);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(273, 21);
            this.radioButton2.TabIndex = 16;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "¿Quién es el jugador con más puntos?";
            this.radioButton2.UseVisualStyleBackColor = false;
            // 
            // duracion
            // 
            this.duracion.AutoSize = true;
            this.duracion.BackColor = System.Drawing.Color.Transparent;
            this.duracion.ForeColor = System.Drawing.Color.White;
            this.duracion.Location = new System.Drawing.Point(31, 118);
            this.duracion.Margin = new System.Windows.Forms.Padding(4);
            this.duracion.Name = "duracion";
            this.duracion.Size = new System.Drawing.Size(221, 21);
            this.duracion.TabIndex = 15;
            this.duracion.TabStop = true;
            this.duracion.Text = "¿Cuál es la partida más larga?";
            this.duracion.UseVisualStyleBackColor = false;
            // 
            // Contraseña
            // 
            this.Contraseña.AutoSize = true;
            this.Contraseña.BackColor = System.Drawing.Color.Transparent;
            this.Contraseña.ForeColor = System.Drawing.Color.White;
            this.Contraseña.Location = new System.Drawing.Point(31, 47);
            this.Contraseña.Margin = new System.Windows.Forms.Padding(4);
            this.Contraseña.Name = "Contraseña";
            this.Contraseña.Size = new System.Drawing.Size(185, 21);
            this.Contraseña.TabIndex = 14;
            this.Contraseña.TabStop = true;
            this.Contraseña.Text = "¿Cuál es mi contraseña?";
            this.Contraseña.UseVisualStyleBackColor = false;
            // 
            // consultasButton
            // 
            this.consultasButton.AutoSize = true;
            this.consultasButton.BackColor = System.Drawing.Color.Black;
            this.consultasButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consultasButton.ForeColor = System.Drawing.Color.White;
            this.consultasButton.Location = new System.Drawing.Point(1209, 82);
            this.consultasButton.Name = "consultasButton";
            this.consultasButton.Size = new System.Drawing.Size(153, 45);
            this.consultasButton.TabIndex = 13;
            this.consultasButton.Text = "Consultar";
            this.consultasButton.UseVisualStyleBackColor = false;
            this.consultasButton.Click += new System.EventHandler(this.consultasButton_Click);
            // 
            // ConectadosGridView
            // 
            this.ConectadosGridView.AllowUserToAddRows = false;
            this.ConectadosGridView.AllowUserToDeleteRows = false;
            this.ConectadosGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConectadosGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.ConectadosGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ConectadosGridView.GridColor = System.Drawing.SystemColors.Control;
            this.ConectadosGridView.Location = new System.Drawing.Point(12, 331);
            this.ConectadosGridView.Name = "ConectadosGridView";
            this.ConectadosGridView.RowHeadersWidth = 51;
            this.ConectadosGridView.RowTemplate.Height = 24;
            this.ConectadosGridView.Size = new System.Drawing.Size(230, 281);
            this.ConectadosGridView.TabIndex = 16;
            this.ConectadosGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ConectadosGridView_CellClick);
            // 
            // labelConectados
            // 
            this.labelConectados.AutoSize = true;
            this.labelConectados.Location = new System.Drawing.Point(14, 316);
            this.labelConectados.Name = "labelConectados";
            this.labelConectados.Size = new System.Drawing.Size(137, 17);
            this.labelConectados.TabIndex = 17;
            this.labelConectados.Text = "Lista de Conectados";
            // 
            // nameUserTxt
            // 
            this.nameUserTxt.AutoSize = true;
            this.nameUserTxt.BackColor = System.Drawing.Color.Black;
            this.nameUserTxt.Font = new System.Drawing.Font("Lucida Fax", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameUserTxt.Location = new System.Drawing.Point(264, 623);
            this.nameUserTxt.Name = "nameUserTxt";
            this.nameUserTxt.Size = new System.Drawing.Size(0, 27);
            this.nameUserTxt.TabIndex = 18;
            // 
            // invitarButton
            // 
            this.invitarButton.BackColor = System.Drawing.Color.Black;
            this.invitarButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.invitarButton.ForeColor = System.Drawing.Color.White;
            this.invitarButton.Location = new System.Drawing.Point(12, 241);
            this.invitarButton.Name = "invitarButton";
            this.invitarButton.Size = new System.Drawing.Size(139, 72);
            this.invitarButton.TabIndex = 19;
            this.invitarButton.Text = "Invitar";
            this.invitarButton.UseVisualStyleBackColor = false;
            this.invitarButton.Click += new System.EventHandler(this.invitarButton_Click);
            // 
            // Acceso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1398, 662);
            this.Controls.Add(this.invitarButton);
            this.Controls.Add(this.nameUserTxt);
            this.Controls.Add(this.labelConectados);
            this.Controls.Add(this.consultasButton);
            this.Controls.Add(this.consultaBox);
            this.Controls.Add(this.registroBox);
            this.Controls.Add(this.luz);
            this.Controls.Add(this.conexion);
            this.Controls.Add(this.accederBox);
            this.Controls.Add(this.ConectadosGridView);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Acceso";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Acceder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Acceso_FormClosing);
            this.Load += new System.EventHandler(this.Acceso_Load);
            this.accederBox.ResumeLayout(false);
            this.accederBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.candadoBox)).EndInit();
            this.registroBox.ResumeLayout(false);
            this.registroBox.PerformLayout();
            this.consultaBox.ResumeLayout(false);
            this.consultaBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConectadosGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Login;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox accederBox;
        private System.Windows.Forms.Button conexion;
        private System.Windows.Forms.Button luz;
        private System.Windows.Forms.GroupBox registroBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox password2Box;
        private System.Windows.Forms.TextBox mailBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox userBox;
        private System.Windows.Forms.Button Registrarme;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox consultaBox;
        private System.Windows.Forms.Button Preguntar;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton duracion;
        private System.Windows.Forms.RadioButton Contraseña;
        private System.Windows.Forms.PictureBox candadoBox;
        private System.Windows.Forms.Button consultasButton;
        private System.Windows.Forms.DataGridView ConectadosGridView;
        private System.Windows.Forms.Label labelConectados;
        private System.Windows.Forms.Label nameUserTxt;
        private System.Windows.Forms.Button invitarButton;
    }
}

