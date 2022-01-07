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
        int miCasilla;

        ListaPreguntas geografia;
        ListaPreguntas historia;
        ListaPreguntas ciencia;
        ListaPreguntas deportes;
        ListaPreguntas literatura;
        ListaPreguntas cultura;

        Queue<string> chat;
        
        public Tablero()
        {
            InitializeComponent();
            Bitmap tablero = new Bitmap(Application.StartupPath + @"\tablero.png");
            this.BackgroundImage = tablero;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            PictureBox dado = new PictureBox();

            casillas = new ListaCasillas();
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
                else if (num == 6)
                    dado.Image = Image.FromFile("dado6.png");

                List<int> movimientos = casillas.DameMovimientosPosibles(miCasilla,num);
                string texto = "";
                foreach (int posicion in movimientos)
                    texto = texto + posicion + ",";
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

        
    }
}
