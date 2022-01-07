using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Trivial
{
    public partial class Tablero : Form
    {
        int partida;
        string userName;
        string rol;
        bool miTurno;
        Socket server;
        List<string> jugadores;
        ListaCasillas casillas;
        List<int> movimientos; //New, esperem que no doni problemes. Definida quan el dau. Usada again quan piques on vols anar
        int miCasilla;

        ListaPreguntas geografia;
        ListaPreguntas historia;
        ListaPreguntas ciencia;
        ListaPreguntas deportes;
        ListaPreguntas literatura;
        ListaPreguntas cultura;

        Queue<string> chat;
        int xorigen;
        int yorigen;

        public Tablero()
        {
            InitializeComponent();
            PictureBox dado = new PictureBox();

            Bitmap tablero = new Bitmap(Application.StartupPath + @"\Tablero.png");
            tableroBox.Image = (Image)tablero;
            tableroBox.SizeMode = PictureBoxSizeMode.AutoSize;
            tableroBox.Location = new Point(0, 0);
            this.Size = tableroBox.Size;

            this.xorigen = (tableroBox.Size.Width / 2) + tableroBox.Location.X;
            this.yorigen = tableroBox.Size.Height / 2 + tableroBox.Location.Y;

            casillas = new ListaCasillas(xorigen,yorigen);
            casillas.CalcularMovimientos();

            geografia = new ListaPreguntas(@".\\geografia.txt");
            historia = new ListaPreguntas(@".\\historia.txt");
            ciencia = new ListaPreguntas(@".\\ciencia.txt");
            deportes = new ListaPreguntas(@".\\deportes.txt");
            literatura = new ListaPreguntas(@".\\literatura.txt");
            cultura = new ListaPreguntas(@".\\cultura.txt");

            ChatBox.Multiline = true;
            chat = new Queue<string>();
        }

        private void Tablero_Load(object sender, EventArgs e)
        {
            dado.Image = Image.FromFile("dado1.png");
            username_lbl.Text = "Username: " + userName;
            partida_lbl.Text = "Partida: " + partida;
            playersGridView.ColumnCount = 2;
            playersGridView.RowCount = this.jugadores.Count;
            playersGridView.ColumnHeadersVisible = true;
            playersGridView.Columns[0].HeaderText = "Jugador";
            playersGridView.Columns[1].HeaderText = "Puntos";
            playersGridView.RowHeadersVisible = false;
            playersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            playersGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            playersGridView.RowsDefaultCellStyle.BackColor = Color.White;
            for (int i = 0; i < this.jugadores.Count; i++)
            {
                playersGridView.Rows[i].Cells[0].Value = jugadores[i];
                playersGridView.Rows[i].Cells[1].Value = "0";
                
            }
            playersGridView.Rows[0].DefaultCellStyle.BackColor = Color.Blue;

            playersGridView.Show();

            //Establecemos el turno inicial
            if (rol == "host")
                miTurno = true;
            else
                miTurno = false;

            //Empezamos en la casilla central
            miCasilla = 1000; //Casilla central
        }

        
        public void SetPartida(string mensaje,Socket server, string userName)
        {
            string[] trozos = mensaje.Split('*'); 
            this.partida = Convert.ToInt32(trozos[0]);

            jugadores = new List<string>();
            for (int i = 1; i < trozos.Length-1; i++)
                jugadores.Add(trozos[i]);
            this.rol = trozos[trozos.Length - 1];
            this.server = server;

            this.userName = userName;
        }

        //Recibimos un nuevo movimiento (el codigo nos indica el tipo de movimiento)
        public void NuevoMovimiento(string mensaje, int codigo) //"idPartida*resDado*nombreTirador*siguienteTurno(rol)"
        {
            //Resultado del dado
            if (codigo == 11)
            {
                string[] trozos = mensaje.Split('*');
                if (miTurno == true)
                    miTurno = false;
                else
                {
                    dadolbl.Text = trozos[2] + " avanza " + trozos[1] + " casillas";
                    dado.Image = Image.FromFile("dado"+trozos[1]+".png");
                    if (trozos[3] == rol)
                        miTurno = true;
                }

                ActualizarTurno(trozos[3]);

            }
        }

        //Actualizar turno 
        private void ActualizarTurno(string siguienteTurno)
        {
            for (int i = 0; i < playersGridView.RowCount; i++)
            {
                playersGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
            }
            switch (siguienteTurno)
            {
                case "host":
                    playersGridView.Rows[0].DefaultCellStyle.BackColor = Color.Blue;
                    break;
                case "jug2":
                    playersGridView.Rows[1].DefaultCellStyle.BackColor = Color.Blue;
                    break;
                case "jug3":
                    playersGridView.Rows[2].DefaultCellStyle.BackColor = Color.Blue;
                    break;
                case "jug4":
                    playersGridView.Rows[3].DefaultCellStyle.BackColor = Color.Blue;
                    break;
            }

        }

        //Obtener el numero de la partida de este tablero
        public int DameIdPartida()
        {
            return this.partida;
        }

        //Tirar el dado y mostrar el resultado
        private void dado_Click_1(object sender, EventArgs e)
        {
            if (miTurno == true)
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
                else
                    dado.Image = Image.FromFile("dado6.png");

                List<int> movimientos = casillas.DameMovimientosPosibles(miCasilla, num); 
                string texto = " ";
                foreach (int posicion in movimientos)
                {
                    texto = texto + posicion + ",";
                    Casilla c = new Casilla(posicion, 1, xorigen, yorigen);
                    Bitmap ubi = new Bitmap(Application.StartupPath + @"\ubicacion.png");
                    PictureBox p = new PictureBox {
                        Name = "pictureBox" + Convert.ToString(posicion),
                        Image = (Image)ubi,
                        Size = new Size(60, 90),
                        Location = new Point(c.GetX() - p.Size.Width / 2, c.GetY() - p.Size.Height),
                    };
                }
                texto.Remove(texto.Length - 1);
                movimientosLbl.Text = "Posibles movimientos: " + texto;
                dadolbl.Text = "Avanza " + num.ToString() + " casillas.";

                //Construimos el mensaje para enviar el resultado del dado
                string resDado = "8/" + partida + "/" + num + "/" + rol + "/" + userName;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(resDado);
                server.Send(msg);
            }
            else
                MessageBox.Show("No es tu turno");
            
        }

        private void Tablero_FormClosing(object sender, FormClosingEventArgs e)
        {
            string finpartida = "9/" + partida;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(finpartida);
            server.Send(msg);
        }

        //Enviar mensajes al chat
        private void ChatBtn_Click(object sender, EventArgs e)
        {
            if (ChatTxt.Text != "")
            {
                string mensaje = "12/" + partida + "/" + ChatTxt.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Escribimos en el chat lo que enviamos
                string mchat = "Yo: " + ChatTxt.Text;
                if (chat.Count >= 9)
                {
                    chat.Dequeue();
                    chat.Enqueue(mchat);

                    ChatBox.Clear();
                    foreach(string msgChat in chat)
                    {
                        ChatBox.Text = ChatBox.Text + msgChat + Environment.NewLine;
                    }
                }
                else if (chat.Count == 8)
                {
                    chat.Enqueue(mchat);
                    ChatBox.Text = ChatBox.Text + mchat;
                }
                else
                {
                    chat.Enqueue(mchat);
                    ChatBox.Text = ChatBox.Text + mchat + Environment.NewLine;
                }
                

                //RichTextBox bold = ChatBox;
                //foreach (string line in bold.Lines)
                //{
                //    string user = line.Split(' ')[0];
                //    int srt = bold.Find(user);
                //    bold.Select(srt, user.Length);
                //    bold.SelectionFont = new Font(bold.Font, FontStyle.Bold);
                //}

                //Borramos lo escrito una vez enviado
                ChatTxt.Clear();
            }
        }

        //Recibir mensajes chat
        public void NuevoMensajeChat(string datos)
        {
            string mensaje = datos.Split('*')[1] + ": "+ datos.Split('*')[2];
          
            if (chat.Count >= 9)
            {
                chat.Dequeue();
                chat.Enqueue(mensaje);

                ChatBox.Clear();
                foreach (string msgChat in chat)
                {
                    ChatBox.Text = ChatBox.Text + msgChat + Environment.NewLine;
                }
            }
            else if (chat.Count == 8)
            {
                chat.Enqueue(mensaje);
                ChatBox.Text = ChatBox.Text + mensaje;
            }
            else
            {
                chat.Enqueue(mensaje);
                ChatBox.Text = ChatBox.Text + mensaje + Environment.NewLine;
            }

            //ChatBox.ScrollToCaret(); //no lo hace

            //ChatBox -> 9 lineas

            //RichTextBox bold = ChatBox;
            //foreach (string line in bold.Lines)
            //{
            //    string user = line.Split(' ')[0];
            //    int srt = bold.Find(user);
            //    bold.Select(srt, user.Length);
            //    bold.SelectionFont = new Font(bold.Font, FontStyle.Bold);
            //}
        }

        private void tableroBox_Click(object sender, EventArgs e)
        {
            // Pasos:
            // 1. Comprobar si es tu turno
            // 2. Mirar en qué casilla has picado = idcasilla
            // 3. Comprobar si esa está en posibles movimientos
            // 4. If yes: Ubicar tu pieza en idcasilla.x;

            if (miTurno==true) //1
            {
                int idcasilla; //2

                MouseEventArgs me = (MouseEventArgs)e;
                Point coordinates = me.Location;
                double xclick = coordinates.X;
                double yclick = coordinates.Y;

                double a = xclick - xorigen;
                double b = yclick - yorigen;

                double distOrigen = Math.Sqrt(a * a + b * b);

                double espesor = 45; // Diferencia entre la 00 y la 01
                double ancho = 25;   // Como de anchas son las rieras
                double radi = 675 / 2; // Cuestiones esteticas
                double alpha = 8.57; // El incremento de angulo que debería haber entre casillas perimetro       
                double radians = Math.Atan2(a, b);
                double angle_pos = radians * (180 / Math.PI);  // Calculamos el angulo en el que estamos
                double angle_girat = 180 - angle_pos;   // El que realment és pel nostre sistema (siempre positivo)

                double r04 = 2 * espesor;
                double r03 = 3 * espesor;
                double r02 = 4 * espesor;
                double r01 = 5 * espesor;
                double r00 = 6 * espesor;
                double rtotal = radi; //Radio del tablero

                //Determinar casella 
                string piso;
                string riera;
                
                if (distOrigen < rtotal) // Determinar dentro tablero
                {
                    double multiple = angle_girat / alpha;  // Quantes vegades s'ha multiplicat per alpha determinarà quin numero és de casella

                    if ((distOrigen > r00) && (distOrigen <= rtotal)) // Determinar si estas en el perimetro
                    {
                        if (Math.Abs(a) <= ancho) //Determinar si es la de arriba (0) o abajo (21)
                        {
                            if (b > 0)
                                idcasilla = 21;
                            else
                                idcasilla = 0;
                        }
                        else
                        {
                            double m = Math.Round(multiple, 0);
                            idcasilla = Convert.ToInt32(m);
                        }
                    }
                    else
                    {
                        // Si no estàs en el perímetre, has de determinar si estàs en la central o a quina fila, i llavors determinar quina riera
                        if (distOrigen <= espesor)
                        {
                            idcasilla = 1000;
                        }
                        else
                        {
                            if ((distOrigen > espesor) && (distOrigen <= r04))
                            {
                                piso = "4";
                            }
                            else if ((distOrigen > r04) && (distOrigen <= r03))
                            {
                                piso = "3";
                            }
                            else if ((distOrigen > r03) && (distOrigen <= r02))
                            {
                                piso = "2";
                            }
                            else if ((distOrigen > r02) && (distOrigen <= r01))
                            {
                                piso = "1";
                            }
                            else
                            {
                                piso = "0";
                            }
                            if (Math.Abs(a) <= ancho) //Determinar si es la de arriba (0) o abajo (21)
                            {
                                if (b > 0)
                                    riera = "13";
                                else
                                    riera = "10";
                                string c = riera + piso;
                                idcasilla = Convert.ToInt32(c);
                            }
                            else
                            // Calcules els límits que ha de tenir en aquella x
                            {
                                if (((angle_pos > 0) && (angle_girat < 90)) || ((angle_pos < 0) && (angle_girat < 3 * 90)))
                                //Arriba derecha or abajo abajo izq
                                {
                                    double yupper = -Math.Tan(Math.PI / 6) * xclick + yorigen + Math.Tan(Math.PI / 6) * xorigen - ancho / Math.Sin(Math.PI / 3);
                                    double ydown = -Math.Tan(Math.PI / 6) * xclick + yorigen + Math.Tan(Math.PI / 6) * xorigen + ancho / Math.Sin(Math.PI / 3);

                                    if ((yclick > yupper) && (yclick < ydown))
                                    {
                                        if (angle_pos > 0)
                                            riera = "11";
                                        else
                                            riera = "14";
                                        string c = riera + piso;
                                        idcasilla = Convert.ToInt32(c);
                                    }
                                    else
                                        idcasilla = -1;
                                }
                                else
                                {
                                    double yupper = Math.Tan(Math.PI / 6) * xclick + yorigen - Math.Tan(Math.PI / 6) * xorigen + ancho / Math.Sin(Math.PI / 3);
                                    double ydown = Math.Tan(Math.PI / 6) * xclick + yorigen - Math.Tan(Math.PI / 6) * xorigen - ancho / Math.Sin(Math.PI / 3);

                                    if ((yclick < yupper) && (yclick > ydown))
                                    {
                                        if (angle_pos > 0)
                                            riera = "12";
                                        else
                                            riera = "15";
                                        string c = riera + piso;
                                        idcasilla = Convert.ToInt32(c);
                                    }
                                    else
                                        idcasilla = -1;
                                }

                            }


                        }

                    }
                    MessageBox.Show("Estás en la casilla: " + Convert.ToString(idcasilla));

                    bool encontrado = false; //3
                    foreach (int posicion in movimientos)
                    {
                        if (posicion == idcasilla)
                            encontrado = true;
                    }
                    if (encontrado == true)
                    {
                        Casilla c = new Casilla(idcasilla, 1, xorigen, yorigen);
                        // TOCA COLOCAR
                    }
                }
                else
                    MessageBox.Show("Fuera del tablero");
            
                
            }

        }

    }
}
