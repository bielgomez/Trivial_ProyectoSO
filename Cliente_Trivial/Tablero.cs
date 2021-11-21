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
        }

        private void Tablero_Load(object sender, EventArgs e)
        {
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

        //public void SetPartida(int id_partida, List<string> invitados) {
        public void SetPartida(string mensaje)
        {
            string[] trozos = mensaje.Split('*');
            this.partida = Convert.ToInt32(trozos[0]);

            jugadores = new List<string>();
            for (int i = 1; i < trozos.Length; i++)
                jugadores.Add(trozos[i]);

        }
    }
}
