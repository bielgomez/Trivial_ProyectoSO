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
        public ListaCasillas()
        {
            this.lista = new List<Casilla>();
            this.num = 0;

            //Crear las casillas que existen

            //Caselles exteriors (cercle)
            int i = 0;
            while (i < 42)
            {
                for (int dau = 1; dau <= 6; dau++)
                {
                    Casilla nuevaCasilla = new Casilla(i, dau);
                    this.lista.Add(nuevaCasilla);
                    num++;
                }
                i++;
            }

            //Caselles interiors
            i = 100;
            while (i < 155)
            {
                if ((((i % 10) % 5) == 0) && ((i % 10) != 0))
                    i = i + 5;
                for (int dau = 1; dau <= 6; dau++)
                {
                    Casilla nuevaCasilla = new Casilla(i, dau);
                    this.lista.Add(nuevaCasilla);
                    num++;
                }
                i++;
            }
            //Casella central
            for (int dau = 1; dau <= 6; dau++)
            {
                Casilla nuevaCasilla = new Casilla(1000, dau);
                this.lista.Add(nuevaCasilla);
                num++;
            }
        }

        //Métodos
        public void CalcularMovimientos()
        {
            foreach (Casilla casilla in this.lista)
            {
                casilla.CalculaPosiblesMovimientos();
            }
        }

        public List<int> DameMovimientosPosibles(int idCasilla, int dado)
        {
            //Retorna null si no puede retornar lo que le piden
            int i = 0;
            bool encontrado = false;
            while((i<lista.Count) && (encontrado == false))
            {
                if ((lista[i].GetId()==idCasilla) && (lista[i].GetDado() == dado))
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
                return lista[i].GetMovimientos();
            }
            else
            {
                return null;
            }
        }
    }
}
