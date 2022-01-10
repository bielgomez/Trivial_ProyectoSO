using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Trivial
{
    class ListaPreguntas
    {
        //Atributos
        Pregunta[] lista;
        int num;
        
        //Constructor
        public ListaPreguntas(string fichero)
        {
            //Rellenamos la lista con los datos del fichero
            this.num = 0;
            int numPreguntas = File.ReadLines(fichero).Count();
            lista = new Pregunta[numPreguntas];
            StreamReader r = new StreamReader(fichero);
            string linea = r.ReadLine();
            while ((linea != null) && (linea != ""))
            {
                string[] trozos = linea.Split('/');
                Pregunta nuevaPregunta = new Pregunta(trozos);
                lista[num] = nuevaPregunta;
                num++;
                linea = r.ReadLine();
            }
            r.Close();
        }

        //Métodos
        public Pregunta DamePregunta(int posicion)
        {
            return lista[posicion];
        }

        public int DameLongitud()
        {
            return lista.Length;
        }
    }
}
