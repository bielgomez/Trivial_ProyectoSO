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
        List<string> invitados;
        List<string> convidatsLlista;
        public Tablero()
        {
            InitializeComponent();
        }

        private void Tablero_Load(object sender, EventArgs e)
        {
            playersGridView.ColumnCount = 2;
            playersGridView.RowCount = this.convidatsLlista.Count;
            playersGridView.ColumnHeadersVisible = true;
            playersGridView.Columns[0].HeaderText = "Jugador";
            playersGridView.Columns[1].HeaderText = "Turno";
            playersGridView.RowHeadersVisible = false;
            playersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            playersGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            playersGridView.SelectAll();

            //for(int i = 0; i<this.invitados.Count; i++)
            for (int i = 0; i < this.convidatsLlista.Count; i++)
            {
                //playersGridView.Rows[i].Cells[0].Value = invitados[i];
                playersGridView.Rows[i].Cells[0].Value = convidatsLlista[i];
                playersGridView.Rows[i].Cells[1].Value = "0";
            }
            playersGridView.Rows[0].Cells[1].Value = "1";
            playersGridView.Show();

        }

        //public void SetPartida(int id_partida, List<string> invitados) {
        public void SetPartida(int id_partida, string convidats)
        {
            this.partida = id_partida;
            //this.invitados = new List<string>();
            //for(int i = 0; i<invitados.Count; i++)
            //this.invitados.Add(invitados[i]);
            this.convidatsLlista = new List<string>();
            string[] trozos = convidats.Split('*');
            for(int i = 0; i<trozos.Length; i++)
                this.convidatsLlista.Add(trozos[i]);
        }
    }
}
