using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trivial
{
    public class Jugador
    {
        string nombre;
        string rol;
        int rolnum;
        int casilla; 
        Bitmap bitmap;  // Imagen de emboltorio
        int[] quesitos; // ORDEN: Verde, Azul, Amarillo, Lila, Naranja, Rojo

        public Jugador(string nombre, string rol)
        {
            this.nombre = nombre;
            this.rol = rol;
            this.casilla = 1000; //La casilla inicial es la central
            switch (rol)
            {
                case "host":
                    this.bitmap = new Bitmap(Application.StartupPath + @"\JugAzul.png");
                    this.rolnum = 0;
                    break;
                case "jug2":
                    this.bitmap =  new Bitmap(Application.StartupPath + @"\JugLila.png");
                    this.rolnum = 1;
                    break;
                case "jug3":
                    this.bitmap = new Bitmap(Application.StartupPath + @"\JugRojo.png");
                    this.rolnum = 2;
                    break;
                case "jug4":
                    this.bitmap = new Bitmap(Application.StartupPath + @"\JugVerde.png");
                    this.rolnum = 3;
                    break;
            }
            this.quesitos = new int[6];
            foreach (int quesito in this.quesitos)
                quesito.Equals(0);
        }
        public Bitmap GetEmboltorioBitmap()
        {
            return this.bitmap;
        }
        public int[] GetQuesitos()
        {
            return this.quesitos;
        }
        public int GetRolNum()
        {
            return this.rolnum;
        }
        public int GetCasilla()
        {
            return this.casilla;
        }
        public string GetNombre()
        {
            return this.nombre;
        }
        public string GetRol()
        {
            return this.rol;
        }
        public void SetCasilla(int casilla)
        {
            this.casilla=casilla;
        }
        //Añadir un quesito al jugador
        public void SetQuesito(string color)
        {
            switch (color)
            {
                case "Verde":
                    this.quesitos[0] = 1;
                    break;
                case "Azul":
                    this.quesitos[1] = 1;
                    break;
                case "Amarillo":
                    this.quesitos[2] = 1;
                    break;
                case "Lila":
                    this.quesitos[3] = 1;
                    break;
                case "Naranja":
                    this.quesitos[4] = 1;
                    break;
                case "Rojo":
                    this.quesitos[5] = 1;
                    break;
            }
        }

    }

}
