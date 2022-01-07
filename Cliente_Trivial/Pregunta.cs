using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivial
{
    class Pregunta
    {
        //Atributos
        string pregunta;
        string[] opciones;
        int correcta; //Posicion de la respuesta correcta en el vector opciones

        //Constructor
        public Pregunta(string[] datos)
        {
            this.pregunta = datos[0];
            this.opciones = new string[4];
            int i = 0;
            while(i<4)
            {
                this.opciones[i] = datos[i + 1];
                i++;
            }
            this.correcta = Convert.ToInt32(datos[5]);
        }

        //Métodos
        public string GetEnunciado()
        {
            return this.pregunta;
        }
        public string[] GetOpciones()
        {
            return this.opciones;
        }
        public int GetCorrecta()
        {
            return this.correcta;
        }
        
    }
}
