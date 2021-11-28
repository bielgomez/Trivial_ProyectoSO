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
        
        public Tablero()
        {
            InitializeComponent();
            Bitmap tablero = new Bitmap(Application.StartupPath + @"\tablero.png");
            this.BackgroundImage = tablero;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            PictureBox dado = new PictureBox();

        }

        private void Tablero_Load(object sender, EventArgs e)
        {
            dado.Image = Image.FromFile("dado1.png");
            username_lbl.Text = "Username: " + userName;
            partida_lbl.Text = "Partida: " + partida;
            playersGridView.ColumnCount = 3;
            playersGridView.RowCount = this.jugadores.Count;
            playersGridView.ColumnHeadersVisible = true;
            playersGridView.Columns[0].HeaderText = "Jugador";
            playersGridView.Columns[1].HeaderText = "Turno";
            playersGridView.Columns[2].HeaderText = "Puntos";
            playersGridView.RowHeadersVisible = false;
            playersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            playersGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            playersGridView.SelectAll();

            playersGridView.RowsDefaultCellStyle.BackColor = Color.White;
            for (int i = 0; i < this.jugadores.Count; i++)
            {
                playersGridView.Rows[i].Cells[0].Value = jugadores[i];
                playersGridView.Rows[i].Cells[1].Value = "NO";
                playersGridView.Rows[i].Cells[2].Value = "0";
                
            }
            playersGridView.Rows[0].Cells[1].Value = "SI";
            playersGridView.Rows[0].DefaultCellStyle.BackColor = Color.Blue;

            playersGridView.Show();

            //Establecemos el turno inicial
            if (rol == "host")
                miTurno = true;
            else
                miTurno = false;
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
                playersGridView.Rows[i].Cells[1].Value = "NO";

            playersGridView.RowsDefaultCellStyle.BackColor = Color.White;

            switch (siguienteTurno)
            {
                case "host":
                    playersGridView.Rows[0].Cells[1].Value = "SI";
                    playersGridView.Rows[0].DefaultCellStyle.BackColor = Color.Blue;
                    break;
                case "jug2":
                    playersGridView.Rows[1].Cells[1].Value = "SI";
                    playersGridView.Rows[1].DefaultCellStyle.BackColor = Color.Blue;
                    break;
                case "jug3":
                    playersGridView.Rows[2].Cells[1].Value = "SI";
                    playersGridView.Rows[2].DefaultCellStyle.BackColor = Color.Blue;
                    break;
                case "jug4":
                    playersGridView.Rows[3].Cells[1].Value = "SI";
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

                dadolbl.Text = "Avanza " + num.ToString() + " casillas.";

                //Construimos el mensaje para enviar el resultado del dado
                string resDado = "8/" + partida + "/" + num + "/" + rol + "/" + userName;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(resDado);
                server.Send(msg);
            }
            else
                MessageBox.Show("No es tu turno");
            
        }

    
    }
}
