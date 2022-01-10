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
        int miCasilla; 

        Jugador miJugador;
        // Para perimtir y bloquear la manipulacion de cada funcion
        bool miTurno;
        bool dadoClick;
        bool tableroClick;

        Socket server;

        Queue<string> chat;

        List<Jugador> jugadors;   //Jugadores de la partida: host, jug2, (jug3, jug4)
        ListaCasillas casillas;   //Casillas tablero
        List<PictureBox> ubicaciones;  //Indicaciones posibles movimientso
        List<PictureBox> piezas;       //Pieza de cada jugador. En el mismo orden que "jugadors"
        
        ListaPreguntas geografia;
        ListaPreguntas historia;
        ListaPreguntas ciencia;
        ListaPreguntas deportes;
        ListaPreguntas literatura;
        ListaPreguntas cultura;
        // Quesitos
        Bitmap qV; 
        Bitmap qB;
        Bitmap qA;
        Bitmap qL;
        Bitmap qN;
        Bitmap qR;
        List<Bitmap> quesitos;

        int xorigen;
        int yorigen;

        int x;
        int y;

        public Tablero()
        {
            InitializeComponent();
            PictureBox dado = new PictureBox();
            // El fondo del Form es la imagen del tablero
            Bitmap tablero = new Bitmap(Application.StartupPath + @"\Tablero.png");
            this.BackgroundImage = (Image)tablero;
            this.BackgroundImageLayout = ImageLayout.Center;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            // Tablero
            tableroBox.Image = (Image)tablero;
            tableroBox.BackColor = Color.Transparent;
            tableroBox.SizeMode = PictureBoxSizeMode.AutoSize;
            tableroBox.Location = new Point(0, 0);
            this.Size = tableroBox.Size;
            // Imagenes para mostrar posibles movimientos
            Bitmap ubi = new Bitmap(Application.StartupPath + @"\ubicacion.png");
            this.ubicaciones = new List<PictureBox>();
            PictureBox[] ubilist = new PictureBox[] { ubi1Box, ubi2Box, ubi3Box, ubi4Box, ubi5Box, ubi6Box, ubi7Box };
            ubicaciones.AddRange(ubilist);
            foreach(PictureBox pBox in ubicaciones)
            {
                pBox.Image = (Image)ubi;
                pBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pBox.Visible = false;
            }
            // Fichas
            qV = new Bitmap(Application.StartupPath + @"\quesitoVerde.png");
            qB = new Bitmap(Application.StartupPath + @"\quesitoAzul.png");
            qA = new Bitmap(Application.StartupPath + @"\quesitoAmarillo.png");
            qL = new Bitmap(Application.StartupPath + @"\quesitoLila.png");
            qN = new Bitmap(Application.StartupPath + @"\quesitoNaranja.png");
            qR = new Bitmap(Application.StartupPath + @"\quesitoRojo.png");
            this.quesitos = new List<Bitmap>();
            Bitmap[] bitlist = new Bitmap[] { qV, qB, qA, qL, qN, qR };
            quesitos.AddRange(bitlist);
            // Piezas                        
            this.piezas = new List<PictureBox>();
            PictureBox[] emlist = new PictureBox[] { hostBox, jug2Box, jug3Box, jug4Box };
            piezas.AddRange(emlist);

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
            username_lbl.Text = "Username: " + miJugador.GetNombre();
            partida_lbl.Text = "Partida: " + partida;
            playersGridView.ColumnCount = 2;
            playersGridView.RowCount = this.jugadors.Count;
            playersGridView.ColumnHeadersVisible = true;
            playersGridView.Columns[0].HeaderText = "Jugador";
            playersGridView.Columns[1].HeaderText = "Puntos";
            playersGridView.RowHeadersVisible = false;
            playersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            playersGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            playersGridView.RowsDefaultCellStyle.BackColor = Color.White;
            for (int q = 0; q < this.jugadors.Count; q++)
            {
                playersGridView.Rows[q].Cells[0].Value = jugadors[q].GetNombre();
                playersGridView.Rows[q].Cells[1].Value = "0";
            }
            playersGridView.Rows[0].DefaultCellStyle.BackColor = Color.Blue;
            playersGridView.Show();

            //Establecemos el turno inicial
            if (miJugador.GetRol() == "host")
            {
                miTurno = true;
                dadoClick = true;
            }
            else
            {
                miTurno = false;
                dadoClick = false;
            }
            tableroClick = false;
            //Empezamos en la casilla central
            miCasilla = 1000; 
            // Colocar las piezas de todos los jugadores
            int i = 0;
            while(i<piezas.Count)
            {
                if(i < jugadors.Count)
                {
                    Bitmap bitmap = MakeNewImage(jugadors[i]);
                    piezas[i].Image = (Image)bitmap;
                    piezas[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    piezas[i].Visible = true;
                    piezas[i].Location = new Point(Convert.ToInt32(xorigen - hostBox.Size.Width / 2), Convert.ToInt32(yorigen - hostBox.Size.Height / 2));
                }                
                else
                {
                    piezas[i].Visible = false;
                }
                i++;
            }
        }        
        public void SetPartida(string mensaje, Socket server, string userName)
        {
            string[] trozos = mensaje.Split('*'); 
            this.partida = Convert.ToInt32(trozos[0]);
            
            jugadors = new List<Jugador>();
            List<string> roles = new List<string>();
            string[] r = new string[] { "host", "jug2", "jug3", "jug4" };
            roles.AddRange(r);
            for (int i = 1; i < trozos.Length - 1; i++)
            {
                Jugador j = new Jugador(trozos[i], roles[i - 1]);
                jugadors.Add(j);
            }
            miJugador = new Jugador(userName, trozos[trozos.Length - 1]);    
            
            this.server = server;
        }

        //Recibimos un nuevo movimiento (el codigo nos indica el tipo de movimiento)
        public void NuevoMovimiento(string mensaje) //"idPartida*resDado*nombreTirador"
        {
            string[] trozos = mensaje.Split('*');
            if (miTurno == false)
            { 
                dadolbl.Text = trozos[2] + " ha sacado un " + trozos[1];
                dado.Image = Image.FromFile("dado" + trozos[1] + ".png");
            }
        }
        // Recibimos que alguien tiene una nueva casilla "idPartida*nombreJugador*rolJugador*nuevaCasilla"
        public void setCasillaJugador(string mensaje)
        {
            string[] trozos = mensaje.Split('*');
            int j = 0;
            bool encontrado = false;
            while ((j < jugadors.Count) && (encontrado == false))
            {
                if (jugadors[j].GetRol() == trozos[2])
                    jugadors[j].SetCasilla(Convert.ToInt32(trozos[3]));
            }
            moverCasillaJugadores();

        }
        // Anuncia a todos los jugadores del ganador "idPartida*nombreGanador"
        public void Ganador(string mensaje)
        {
            string[] trozos = mensaje.Split('*');
            if (trozos[1] == miJugador.GetNombre())
                MessageBox.Show("Felicidades! Has ganado la partida");
            else
                MessageBox.Show("Lo siento... " + trozos[1] + " ha ganado la partida");

        }
        // INEFICIENTEEE --------------------------------------------------------------------
        public void moverCasillaJugadores()
        {
            foreach(Jugador j in this.jugadors)
            {
                int idc = j.GetCasilla();
                int x = this.casillas.DameCasilla(idc).GetX();
                int y = this.casillas.DameCasilla(idc).GetY();
                Bitmap bitmap = MakeNewImage(j);
                piezas[j.GetRolNum()].Location = new Point(Convert.ToInt32(x - hostBox.Size.Width / 2), Convert.ToInt32(y - hostBox.Size.Height / 2));
            }
        }
        //Actualizar turno: una vez se ha respondido, se notifica el resultado
        public void ActualizarTurno(string mensaje) //"idPartida*nombreJugador*resultado(0,1,2)*(siguienteTurno*quesito)"
        {
            // 0 -> mal contestada pero se actualiza turno
            // 1 -> bien contestada pero sin quesito
            // 2 -> bien contestada y con quesito
            string[] trozos = mensaje.Split('*');
            if (trozos[2]=="0")
                MessageBox.Show(trozos[1] + " ha contestado mal");
            else if(trozos[2] == "1")
                MessageBox.Show(trozos[1] + " ha contestado bien, sigue tirando");
            else
            {
                MessageBox.Show(trozos[1] + " ha ganado un quesito!"); ////////////////////////////////////////////
            }

            if (((trozos[2] == "0")|| (trozos[2] == "2")) && (trozos[3] == miJugador.GetRol()))
            {
                miTurno = true;
                dadoClick = true;
                MessageBox.Show("ES TU TURNO");
            }
            else
            {
                miTurno = false;
                dadoClick = false;
                MessageBox.Show("Es el turno de "+trozos[1]);
            }
            string siguienteTurno = trozos[2];
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
            if ((miTurno == true) && (dadoClick == true))
            {
                Random dice = new Random();
                int num = dice.Next(1, 6);
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
                int m = 0;
                double xm = ubicaciones[m].Size.Width / 2;
                double ym = ubicaciones[m].Size.Height;
                while (m<movimientos.Count)
                {
                    texto = texto + movimientos[m] + ",";
                    Casilla c = casillas.DameCasilla(movimientos[m]); 
                    ubicaciones[m].Location = new Point(Convert.ToInt32(c.GetX() - xm), Convert.ToInt32(c.GetY() - ym));
                    ubicaciones[m].Visible = true;
                    m++;
                }
                texto.Remove(texto.Length - 1);
                movimientosLbl.Text = "Posibles movimientos: " + texto;

                dadoClick = false;  // No puedes clickar en el dado
                tableroClick = true; // Pero ya puedes clickar en el tablero
                dadolbl.Text = "Avanzo " + num.ToString() + " casillas";

                //Construimos el mensaje para enviar el resultado del dado
                string resDado = "8/" + partida + "/" + num + "/" + miJugador.GetRol() + "/" + miJugador.GetNombre();
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
        }
        // Extraer la pieza del jugador
        private Bitmap MakeNewImage(Jugador jug)        
        {
            List<Bitmap> listBit = new List<Bitmap>();
            listBit.Add(jug.GetEmboltorioBitmap());

            int p = 0;
            while(p < jug.GetQuesitos().Length)
            {
                if (jug.GetQuesitos()[p] == 1)
                    listBit.Add(this.quesitos[p]);
                p++;
            }
            // Con la lista hecha se trata de sobreponer Bitmaps y sacar uno con todas las imágenes
            // Servirá para hacer las fichas: introduciendo el recipiente: JugRojo/JugVerde... + quesitos: quesitoVerde/quesitoAmarillo...
            int i = 1;
            while (i < listBit.Count)
            {
                Graphics g = Graphics.FromImage(listBit[0]);
                g.DrawImage(listBit[i], new Point(0, 0));
                i++;
            }
            return listBit[0];
        }
        private void tableroBox_Click(object sender, EventArgs e)
        {
            // Pasos:
            // 1. Comprobar si es tu turno
            // 2. Mirar en qué casilla has picado -> idcasilla
            // 3. Comprobar si idcasilla está en posibles movimientos
            // 4. If yes: Ubicar tu pieza en idcasilla.coordenadas;

            if ((miTurno == true) && (tableroClick == true)) //1
            {
                MessageBox.Show("Click tablero");
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
                }
                else
                    idcasilla = -1;

                bool encontrado = false; //3
                int n = 0;
                while ((n<this.casillas.DameCasilla(miCasilla).GetMovimientos().Count) && (encontrado==false))
                {
                    if (this.casillas.DameCasilla(miCasilla).GetMovimientos()[n] == idcasilla)
                        encontrado = true;
                    else
                        n++;
                }
                if (encontrado == true)
                {
                    this.tableroClick = false;
                    this.miCasilla = idcasilla;
                    int x = this.casillas.DameCasilla(miCasilla).GetX();
                    int y = this.casillas.DameCasilla(miCasilla).GetY();
                    foreach (PictureBox u in ubicaciones) // Desaparecen las ubicaciones
                        u.Visible = false;
                    Bitmap bitmap = MakeNewImage(miJugador);
                    piezas[miJugador.GetRolNum()].Location = new Point(Convert.ToInt32(x - hostBox.Size.Width / 2), Convert.ToInt32(y - hostBox.Size.Height / 2));

                    //Enviamos el movimiento al servidor
                    string mensaje = "10/" + partida + "/" + this.miCasilla + "/" + miJugador.GetRol();
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }              
            
            }

        }

    }
}
