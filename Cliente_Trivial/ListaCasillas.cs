using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivial
{
    class ListaCasillas
    {
        //Atributos
        List<Casilla> lista;
        int num;

        //Constructor
        public ListaCasillas(int xorigen, int yorigen)
        {
            this.lista = new List<Casilla>();
            this.num = 0;

            //Crear las casillas que existen
            //Caselles exteriors (cercle)
            int i = 0;
            while (i < 42)
            {
                Casilla nuevaCasilla = new Casilla(i, xorigen, yorigen);
                this.lista.Add(nuevaCasilla);
                num++;
                
                i++;
            }

            //Caselles interiors
            i = 100;
            while (i < 155)
            {
                if ((((i % 10) % 5) == 0) && ((i % 10) != 0))
                    i = i + 5;

                Casilla nuevaCasilla = new Casilla(i, xorigen, yorigen);
                this.lista.Add(nuevaCasilla);
                num++;
                
                i++;
            }
            //Casella central
            Casilla nuevaCasillaC = new Casilla(1000,  xorigen, yorigen);
            this.lista.Add(nuevaCasillaC);
            num++;
        }

        //Métodos
        public void CalcularMovimientos()
        {
            foreach (Casilla casilla in this.lista)
            {
                for(int dado = 1; dado<7; dado++)
                    casilla.CalculaPosiblesMovimientos(dado);
            }
        }

        public List<int> DameMovimientosPosibles(int idCasilla, int dado)
        {
            //Retorna null si no puede retornar lo que le piden
            int i = 0;
            bool encontrado = false;
            while((i<lista.Count) && (encontrado == false))
            {
                if (lista[i].GetId()==idCasilla) 
                {
                    encontrado = true;
                }
                else
                {
                    i++;
                }
            }
            if (encontrado == true)
            {
                return lista[i].GetMovimientos(dado);
            }
            else
            {
                return null;
            }
        }
        public Casilla DameCasilla (int id)
        {
            int m = 0;
            bool encontrado = false;
            while((encontrado == false)&&(m<this.num))
            {
                if (lista[m].GetId() == id)
                    encontrado = true;
                else
                    m++;
            }
            return lista[m];
        }
    }
}
