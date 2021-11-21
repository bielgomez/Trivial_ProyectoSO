using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trivial
{
    public partial class Tablero : Form
    {
        int partida;
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

            //for(int i = 0; i<this.invitados.Count; i++)
            for (int i = 0; i < this.jugadores.Count; i++)
            {
                //playersGridView.Rows[i].Cells[0].Value = invitados[i];
                playersGridView.Rows[i].Cells[0].Value = jugadores[i];
                playersGridView.Rows[i].Cells[1].Value = "NO";
                playersGridView.Rows[i].Cells[2].Value = "0";
            }
            playersGridView.Rows[0].Cells[1].Value = "SI";
            playersGridView.Show();

        }

        
        public void SetPartida(string mensaje)
        {
            string[] trozos = mensaje.Split('*');
            this.partida = Convert.ToInt32(trozos[0]);

            jugadores = new List<string>();
            for (int i = 1; i < trozos.Length; i++)
                jugadores.Add(trozos[i]);

        }


        //Tirar el dado y mostrar el resultado
        private void dado_Click_1(object sender, EventArgs e)
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
        }
    }
}
