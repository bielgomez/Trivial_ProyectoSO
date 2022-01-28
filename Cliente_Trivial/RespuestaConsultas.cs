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
        int totalRowHeight;
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
            if (code == 3) //"3/0 o jugador1*jugador2*..." (0-No ha jugado con nadie, lista contrincantes)
            {
                if (mensaje == "0")
                {
                    infoLabel.Visible = true;
                    infoLabel.Text = "No has jugado con nadie... Todavía";
                    infoGridView.Visible = false;
                }
                else
                {
                    infoLabel.Visible = false;
                    infoGridView.Visible = true;
                    infoGridView.ColumnCount = 1;
                    infoGridView.RowCount = jugs.Length;
                    infoGridView.ColumnHeadersVisible = true;
                    infoGridView.Columns[0].HeaderText = "Jugadores";
                    infoGridView.RowHeadersVisible = false;
                    infoGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    infoGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    infoGridView.SelectAll();
                    infoGridView.BackgroundColor = Color.White;

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
            else if (code == 4) //"4/0 o nombre1,idPartida,ganadorPartida*nombre2,idPartida,ganadorParida*..." (Resultados Partidas con jugadores)
            {
                if (mensaje == "0")
                {
                    infoLabel.Visible = true;
                    infoLabel.Text = "No hay ninguna coincidencia... Todavía! Sigue jugando";
                    infoGridView.Visible = false;
                }
                else
                {
                    infoLabel.Visible = false;
                    infoGridView.Visible = true;
                    infoGridView.ColumnCount = 3;                    
                    infoGridView.ColumnHeadersVisible = true;
                    infoGridView.Columns[0].HeaderText = "Contrincante";
                    infoGridView.Columns[1].HeaderText = "Número de partida";
                    infoGridView.Columns[2].HeaderText = "Ganador";
                    infoGridView.RowHeadersVisible = false;
                    infoGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    infoGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    infoGridView.SelectAll();
                    infoGridView.BackgroundColor = Color.White;

                    int totalRowHeight = infoGridView.ColumnHeadersHeight;
                    infoGridView.RowsDefaultCellStyle.BackColor = Color.White;
                    string[] trossos = mensaje.Split('*');
                    infoGridView.RowCount = trossos.Length;
                    int q = 0;
                    while(q<trossos.Length)
                    {
                        string[] info = trossos[q].Split(',');
                        infoGridView.Rows[q].Cells[0].Value = info[0];
                        infoGridView.Rows[q].Cells[1].Value = info[1];
                        infoGridView.Rows[q].Cells[2].Value = info[2];
                        totalRowHeight += infoGridView.Rows[q].Height;
                        q++;
                    }
                    infoGridView.Height = totalRowHeight;
                    infoGridView.Height = infoGridView.Height + 5;
                }
            }
            else if (code == 5)
            {
                infoGridView.Visible = false;
                infoLabel.Visible = true;
                if (mensaje == "-2")
                {
                    infoLabel.Text = "No se ha encontrado ningún jugador en la base de datos.";
                }
                else
                {
                    MessageBox.Show(mensaje);
                    infoLabel.Text = "El jugador con más puntos es: " + mensaje.Split('*')[0] + "\nTiene la friolera cantidad de "+ mensaje.Split('*')[1] + " puntos! \n¿A qué estás esperando para superarlo?";
                }
            }
            else // Code ==18
            {
                if (mensaje == "0")
                {
                    infoLabel.Text = "No hay ninguna coincidencia... Todavía! Sigue jugando";
                    infoGridView.Visible = false;
                }
                else
                {
                    infoGridView.Visible = true;
                    infoGridView.ColumnCount = 2;

                    infoGridView.ColumnHeadersVisible = true;
                    infoGridView.Columns[0].HeaderText = "Número de partida";
                    infoGridView.Columns[1].HeaderText = "Duración";
                    infoGridView.RowHeadersVisible = false;
                    infoGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    infoGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    infoGridView.SelectAll();
                    infoGridView.BackgroundColor = Color.White;

                    int totalRowHeight = infoGridView.ColumnHeadersHeight;
                    infoGridView.RowsDefaultCellStyle.BackColor = Color.White;

                    totalRowHeight = infoGridView.ColumnHeadersHeight;
                    infoGridView.RowsDefaultCellStyle.BackColor = Color.White;
                    string[] trossos = mensaje.Split('*');
                    infoGridView.RowCount = trossos.Length;
                    int q = 0;
                    while (q < trossos.Length)
                    {
                        string[] info = trossos[q].Split(',');
                        infoGridView.Rows[q].Cells[0].Value = info[0];
                        infoGridView.Rows[q].Cells[1].Value = info[1] + " s";
                        totalRowHeight += infoGridView.Rows[q].Height;
                        q++;
                    }
                    infoGridView.Height = totalRowHeight;
                    infoGridView.Height = infoGridView.Height + 5;

                }



            }
                
        }
        
    }
}
