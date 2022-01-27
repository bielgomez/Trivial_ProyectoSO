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
    public partial class Invitacion : Form
    {
        string respuesta;
        string host;
        public Invitacion()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;


        }
        public void SetHost(string host)
        {
            this.host = host;
        }

        public string GetRespuesta()
        {
            return this.respuesta;
        }
        private void Invitacion_Load(object sender, EventArgs e)
        {
            invitationLabel.Text = host + " te ha invitado a una partida.\n ¿Deseas unirte?";
        }

        private void siButton_Click(object sender, EventArgs e)
        {
            this.respuesta = "SI";
            this.Close();
        }

        private void noButton_Click(object sender, EventArgs e)
        {
            this.respuesta="NO";
            this.Close();
        }

    }
}
