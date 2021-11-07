using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Sockets;


namespace Trivial
{
    public partial class Acceso : Form
    {
        Socket server;
        int c = 0;
        int ask = 0;

        public Acceso()
        {

            InitializeComponent();
            Bitmap portada = new Bitmap(Application.StartupPath + @"\portada.png");
            this.BackgroundImage = portada;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        //Iniciacion del Form 
        private void Acceso_Load(object sender, EventArgs e)
        {
            //Partes ocultas al inicio
            consultaBox.Visible = false;
            consultasButton.Visible = false;
            dado.Visible = false;
            dadolbl.Visible = false;
            DameConectados.Visible = false;
            ConectadosGridView.Visible = false;
            labelConectados.Visible = false;

            //Fondo
            candadoBox.Image = Image.FromFile(".\\candadoCerrado.jpg");
            candadoBox.SizeMode = PictureBoxSizeMode.StretchImage;
            dado.Image = Image.FromFile("dado1.png");

        }


        //Botón de conexión/desconexión.
        private void conexion_Click(object sender, EventArgs e)
        {
            //En funcion del estado del sistema (desconectado/conectado), el boton permite conectarse/desconectarse respectivamente

            //Caso Desconectado --> Queremos conectarnos
            if (c == 0)
            {
                IPAddress direc = IPAddress.Parse("192.168.56.102");
                IPEndPoint ipep = new IPEndPoint(direc, 9080);

                //Creamos el socket 
                this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);
                    luz.BackColor = Color.Green;
                    conexion.Text = "Desconectar";
                    c = 1;
                }
                catch (SocketException)
                {
                    MessageBox.Show("No he podido conectar con el servidor");
                }
            }

            //Caso Conectado --> Queremos desconectarnos
            else
            {
                try
                {
                    //Mensaje de desconexion
                    string mensaje = "0/";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Desconexión del servidor
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    conexion.Text = "Conectar";
                    c = 0;

                    //Cambios de color de fondos
                    this.BackColor = Color.DarkSlateGray;
                    luz.BackColor = Color.DarkSlateGray;

                    //Establecemos pantalla inicial
                    consultaBox.Visible = false;
                    consultasButton.Visible = false;
                    dado.Visible = false;
                    dadolbl.Visible = false;
                    accederBox.Visible = true;
                    registroBox.Visible = true;
                    DameConectados.Visible = false;
                    ConectadosGridView.Visible = false;
                    labelConectados.Visible = false;

                    //Cambio de fondo
                    Bitmap portada = new Bitmap(Application.StartupPath + @"\portada.png");
                    this.BackgroundImage = portada;

                    //Vaciamos las casillas por si habian quedado rellenadas
                    NameBox.Clear();
                    PasswordBox.Clear();
                  
                }
                catch (Exception)
                {
                    MessageBox.Show("Ya estás desconectado.");
                }
            }

        }

        //Botón para acceder (LogIn)
        private void Login_Click(object sender, EventArgs e)
        {
            try
            {
                //Construimos el mensaje y lo enviamos (Codigo 1/ --> LogIn)
                string mensaje = "1/" + NameBox.Text + "/" + PasswordBox.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];


                if (mensaje == "0") //Login correcto
                {
                    //Cambio de fondo
                    Bitmap tablero = new Bitmap(Application.StartupPath + @"\tablero.png");
                    this.BackgroundImage = tablero;

                    //Establecemos pantalla del juego
                    consultasButton.Visible = true;
                    dado.Visible = true;
                    dadolbl.Visible = true;
                    DameConectados.Visible = true;
                    accederBox.Visible = false;
                    registroBox.Visible = false;
                }
                //Errores
                else if (mensaje == "1")
                    MessageBox.Show("Este usuario no existe");
                else if (mensaje == "2")
                    MessageBox.Show("Contraseña incorrecta");
                else
                    MessageBox.Show("Error de consulta. Pruebe otra vez.");
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR: Compruebe que está conectado al servidor.");
            }

        }

        //Botón para registrarse 
        private void Registrarme_Click(object sender, EventArgs e)
        {
            try
            {
                //Construimos el mensaje y lo enviamos (Codigo 2/ --> Registrarse)
                string mensaje = "2/" + userBox.Text + "/" + password2Box.Text + "/" + mailBox.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];


                if (mensaje == "0")
                {
                    MessageBox.Show("Se ha registrado correctamente.");
                }

                //Errores
                else if (mensaje == "1")
                {
                    MessageBox.Show("Este nombre de usuario ya existe.");
                }
                else
                    MessageBox.Show("Error de consulta, pruebe otra vez.");

                userBox.Clear();
                password2Box.Clear();
                mailBox.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR: Compruebe que está conectado al servidor.");
            }

        }

        //Botón para hacer consultas a la BBDD a traves del servidor
        private void Preguntar_Click(object sender, EventArgs e)
        {
            try
            {
                //Queremos saber nuestra contraseña
                if (Contraseña.Checked) 
                {
                    //Construimos el mensaje y lo enviamos (Codigo 3/ --> Dame Contraseña)
                    string mensaje = "3/" + NameBox.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                    if (mensaje == "-1")
                        MessageBox.Show("Error de consulta. Prueba otra vez.");

                    else
                        MessageBox.Show("Tu contraseña es: " + mensaje);

                }

                //Queremos saber cuanto dura la partida mas larga
                else if (duracion.Checked)
                {
                    //Construimos el mensaje y lo enviamos (Codigo 4/ --> Dame tiempo partida + larga)
                    string mensaje = "4/";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];


                    if (mensaje == "-1")
                        MessageBox.Show("Error de consulta. Prueba otra vez.");
                    else if (mensaje == "-2")
                        MessageBox.Show("No se ha encontrado ninguna partida en la base de datos");
                    else
                        MessageBox.Show("La partida más larga ha durado: " + mensaje + " segundos.");

                }

                //Queremos saber el jugador con mas puntos
                else
                {
                    //Construimos el mensaje y lo enviamos (Codigo 5/ --> Dame jugador con + puntos)
                    string mensaje = "5/";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                    if (mensaje == "-1")
                        MessageBox.Show("Error de consulta. Prueba otra vez");
                    else if (mensaje == "-2")
                        MessageBox.Show("No se ha encontrado ningún jugador en la base de datos.");
                    else
                        MessageBox.Show("El jugador con más puntos es: " + mensaje + ".");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR: Compruebe que está conectado al servidor.");
            }
        }

       
        //Mostrar y ocultar las contraseñas.
        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
            PasswordBox.UseSystemPasswordChar = true;
            candadoBox.Image = Image.FromFile(".\\candadoCerrado.jpg");
        }

        //Cambio del pictureBox Candado en funcion si desea mostrar u ocultar la contraseña
        private void candadoBox_Click(object sender, EventArgs e)
        {
            //Desea poder ver la contraseña
            if (PasswordBox.UseSystemPasswordChar == true)
            {
                PasswordBox.UseSystemPasswordChar = false;
                candadoBox.Image = Image.FromFile(".\\candadoAbierto.jpg");
            }

            //Desea ocultar la contraseña
            else
            {
                PasswordBox.UseSystemPasswordChar = true;
                candadoBox.Image = Image.FromFile(".\\candadoCerrado.jpg");
            }
        }

        //Tirar el dado y mostrar el resultado
        private void dado_Click(object sender, EventArgs e)
        {
            Random dice = new Random();
            int num = dice.Next(1, 7);
            if (num == 1)
                dado.Image = Image.FromFile("dado1.png");
            else if (num == 2)
                dado.Image = Image.FromFile("dado2.png");
            else if (num == 3)
                dado.Image = Image.FromFile("dado3.png");
            else if (num == 4)
                dado.Image = Image.FromFile("dado4.png");
            else if (num == 5)
                dado.Image = Image.FromFile("dado5.png");
            else if (num == 6)
                dado.Image = Image.FromFile("dado6.png");

            dadolbl.Text = "Avanza " +num.ToString()+ " casillas.";

        }

        //Boton para desplegar/esconder las posibles consultas
        private void consultasButton_Click(object sender, EventArgs e)
        {
            //Si las consultas estan desplegadas --> Queremos esconderlas
            if (ask==0)
            {
                consultaBox.Visible = false;
                consultasButton.Text = "Mostrar\n consultas";
                ask = 1;
            }
            //Si las consultas estas escondidas --> Queremos desplegarlas
            else
            {
                consultaBox.Visible = true;
                consultasButton.Text = "Ocultar\n consultas";
                ask = 0;
            }

        }

        //Queremos la lista de conectados
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Construimos el mensaje y lo enviamos (Codigo 6/ --> Dame lista conectados)
                string mensaje = "6/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                if (mensaje == "-1")
                    MessageBox.Show("No hay usuarios conectados.");
                else
                {
                    //Procesamos el mensaje para obtener un vector de conectados
                    string[] conectados = mensaje.Split('/');

                    //Queremos mostrar los datos en un Data Grid View
                        //Configuracion
                    labelConectados.Visible = true;
                    ConectadosGridView.Visible = true;
                    ConectadosGridView.ColumnCount = 1;
                    ConectadosGridView.RowCount = conectados.Length;
                    ConectadosGridView.ColumnHeadersVisible = false;
                    ConectadosGridView.RowHeadersVisible = false;
                    ConectadosGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                        //Introduccion de los datos
                    for (int i=0; i<conectados.Length; i++)
                        ConectadosGridView.Rows[i].Cells[0].Value = conectados[i];

                }

            }
            catch (SocketException)
            {
                MessageBox.Show("Ha habido un error.");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("No se puede mostrar el Data Grid View.");
            }
        }

        //Si cerramos el form directamente tambien tenemos que desconectar el socket
        private void Acceso_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //Mensaje de desconexion
                string mensaje = "0/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            catch (SocketException)
            {
                MessageBox.Show("Error al desconectar.");
            }
            catch (ObjectDisposedException) 
            {
            
            }
            catch (NullReferenceException)
            {

            }
        }

        private void ConectadosGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

   
    }
}
