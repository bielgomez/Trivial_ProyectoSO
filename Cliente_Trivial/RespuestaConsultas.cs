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
    public partial class RespuestaConsultas : Form
    {
        public RespuestaConsultas()
        {
            InitializeComponent();
        }
        // Escribe la pregunta
        public void SetQuestion (string text)
        {
            askLbl.Text = text;
        }

        //Muestra informacion
        public void SetDataGrid (int code, string mensaje)
        {
            string[] jugs = mensaje.Split('/');
            if (code == 3) // Significa que se muestran jugadores, únicamente nombres
            {
                if (mensaje == "0")
                {
                    infoLabel.Text = "No has jugado con nadie... Todavía";
                    infoGridView.Visible = false;
                }
                else
                {
                    infoGridView.Visible = true;
                    infoGridView.ColumnCount = 1;
                    infoGridView.RowCount = jugs.Length;
                    infoGridView.ColumnHeadersVisible = true;
                    infoGridView.Columns[0].HeaderText = "Jugadores";
                    infoGridView.RowHeadersVisible = false;
                    infoGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    infoGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    int totalRowHeight = infoGridView.ColumnHeadersHeight;
                    infoGridView.RowsDefaultCellStyle.BackColor = Color.White;
                    for (int q = 0; q < jugs.Length; q++)
                    {
                        infoGridView.Rows[q].Cells[0].Value = jugs[q];
                        totalRowHeight += infoGridView.Rows[q].Height;
                    }
                    infoGridView.Height = totalRowHeight;
                    infoGridView.Height = infoGridView.Height + 5;
                }
            }
            else if (code == 4) // Significa que se visualizan partidas
            {
                if (mensaje == "0")
                {
                    infoLabel.Text = "No hay ninguna coincidencia... Todavía! Sigue jugando";
                    infoGridView.Visible = false;
                }
                else
                {
                    infoGridView.Visible = true;
                    infoGridView.ColumnCount = 1;
                    infoGridView.RowCount = jugs.Length;
                    infoGridView.ColumnHeadersVisible = true;
                    infoGridView.Columns[0].HeaderText = "Jugador";
                    infoGridView.Columns[1].HeaderText = "Número de partida";
                    infoGridView.Columns[2].HeaderText = "Ganador";
                    infoGridView.RowHeadersVisible = false;
                    infoGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    infoGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    int totalRowHeight = infoGridView.ColumnHeadersHeight;
                    infoGridView.RowsDefaultCellStyle.BackColor = Color.White;
                    for (int q = 0; q < jugs.Length; q++)
                    {
                        infoGridView.Rows[q].Cells[0].Value = "HHEEEEEEEEEEEEEEEEEEEEEEEEEEEy";//errataHechaAPropositoparaverComoLLeganLos datos;
                        totalRowHeight += infoGridView.Rows[q].Height;
                    }
                    infoGridView.Height = totalRowHeight;
                    infoGridView.Height = infoGridView.Height + 5;
                }
            }
            else
            {
                if (mensaje == "0")
                {
                    infoLabel.Text = "No hay ninguna coincidencia... Todavía! Sigue jugando";
                    infoGridView.Visible = false;
                }
                else
                {
                    infoGridView.Visible = true;
                    infoGridView.ColumnCount = 1;
                    infoGridView.RowCount = jugs.Length;
                    infoGridView.ColumnHeadersVisible = true;
                    infoGridView.Columns[0].HeaderText = "Jugador";
                    infoGridView.Columns[1].HeaderText = "Número de partida";
                    infoGridView.Columns[2].HeaderText = "Ganador";
                    infoGridView.RowHeadersVisible = false;
                    infoGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    infoGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    int totalRowHeight = infoGridView.ColumnHeadersHeight;
                    infoGridView.RowsDefaultCellStyle.BackColor = Color.White;
                    for (int q = 0; q < jugs.Length; q++)
                    {
                        infoGridView.Rows[q].Cells[0].Value = "HHEEEEEEEEEEEEEEEEEEEEEEEEEEEy";//errataHechaAPropositoparaverComoLLeganLos datos;
                        totalRowHeight += infoGridView.Rows[q].Height;
                    }
                    infoGridView.Height = totalRowHeight;
                    infoGridView.Height = infoGridView.Height + 5;
                }



            }
                
        }
        private void RespuestaConsultas_Load(object sender, EventArgs e)
        {

        }
    }
}
