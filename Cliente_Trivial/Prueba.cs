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
    public partial class Prueba : Form
    {

        //Debe retornar 0 si ha respondido bien 
        //Debe retornar 1 si ha respondido mal o no ha respondido
        string categoria;
        ListaPreguntas geografia = new ListaPreguntas(@".\\geografia.txt");
        ListaPreguntas historia = new ListaPreguntas(@".\\historia.txt");
        ListaPreguntas ciencia = new ListaPreguntas(@".\\ciencia.txt");
        ListaPreguntas deportes = new ListaPreguntas(@".\\deportes.txt");
        ListaPreguntas entretenimiento = new ListaPreguntas(@".\\cultura.txt");
        ListaPreguntas tecnologia = new ListaPreguntas(@".\\tecnologia.txt");
        Pregunta pregunta;
        Random number = new Random();
        string enunciado;
        string[] opciones;
        int correcta;
        int acierto=1;
        int segundos;
        

        public Prueba()
        {
            InitializeComponent();
            // El fondo del Form es la imagen del tablero
            Bitmap fondo = new Bitmap(Application.StartupPath + @"\fondonegro.png");
            this.BackgroundImage = (Image)fondo;
            this.BackgroundImageLayout = ImageLayout.Center;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        //Función para pasarle la categoría al formulario
        public void SetCategory(string categoria)
        {
            this.categoria = categoria;
        }
        //Función para enviar si el cliente ha acertado
        public int GetAcierto()
        {
            return this.acierto;
        }

        private void Prueba_Load(object sender, EventArgs e)
        {
            //Seleccionamos una pregunta random de la categoría correspondiente
            if (categoria=="Ciencia")
            {
                int num = number.Next(1, ciencia.DameLongitud());
                pregunta = ciencia.DamePregunta(num);
                cat_label.Text = "CIENCIA";
            }
            else if (categoria == "Historia")
            {
                int num = number.Next(1, historia.DameLongitud());
                pregunta = historia.DamePregunta(num);
                cat_label.Text = "HISTORIA";
            }
            else if (categoria == "Geografía")
            {
                int num = number.Next(1, geografia.DameLongitud());
                pregunta = geografia.DamePregunta(num);
                cat_label.Text = "GEOGRAFÍA";
            }
            else if (categoria == "Deportes")
            {
                int num = number.Next(1, deportes.DameLongitud());
                pregunta = deportes.DamePregunta(num);
                cat_label.Text = "DEPORTES";
            }
           else if (categoria == "Entretenimiento")
            {
                int num = number.Next(1, entretenimiento.DameLongitud());
                pregunta = entretenimiento.DamePregunta(num);
                cat_label.Text = "ENTRETENIMIENTO";
            }
            if (categoria == "Tecnología")
            {
                int num = number.Next(1, tecnologia.DameLongitud());
                pregunta = tecnologia.DamePregunta(num);
                cat_label.Text = "TECNOLOGÍA";
            }

            //Nos quedamos con las variables de la pregunta
            enunciado = pregunta.GetEnunciado();
            opciones=pregunta.GetOpciones();
            correcta = pregunta.GetCorrecta();

            //Escribimos enunciado y opciones en el formulario.
            pregunta_label.Text = enunciado;
            opcion0.Text = opciones[0];
            opcion1.Text = opciones[1];
            opcion2.Text = opciones[2];
            opcion3.Text = opciones[3];

            //Iniciamos el temporizador y lo mostramos
            timer1.Start();
            segundos = 20;
            timer_label.Text=segundos. ToString();
        }

        private void Enviar_Click(object sender, EventArgs e)
        {
            if (opcion0.Checked)
            {  
                if(correcta==0)
                {
                    acierto = 0;
                    this.Close();
                }

            }
            else if (opcion1.Checked)
            {
                if (correcta == 1)
                {
                    acierto=0;
                    this.Close();
                }     
            }
            else if (opcion2.Checked)
            {
                if(correcta == 2)
                {
                    acierto = 0;
                    this.Close();
                }
            }
            else if (opcion3.Checked)
            {
                if(correcta==3)
                {
                    acierto = 0;
                    this.Close();
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            segundos = segundos - 1;
            if(segundos>0)
            {
                timer_label.Text = segundos.ToString();              
            }
            else
            {
                timer1.Stop();
                this.Close();
            }
        }
    }
}
