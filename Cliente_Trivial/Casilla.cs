using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivial
{
    class Casilla
    {
		//Atributos
		int id;
		int dado;
		List<int> movimientos;
		string categoria; //Geografia,Historia,...,Tira otra vez
		double x;
		double y;

		//Constructor
		public Casilla(int id, int dado)
		{
			this.id = id;
			this.dado = dado;
		}

		//Métodos
		public int GetId()
        {
			return this.id;
        }
		public int GetDado()
        {
			return this.dado;
        }
		public List<int> GetMovimientos()
        {
			return this.movimientos;
        }

		public void CalculaPosiblesMovimientos()
        {
			movimientos = new List<int>();

			//Casilla forma parte del circulo exterior (ids de 0 a 41)
			if (id>=0 && id < 42)
            {
				//Movimientos dentro del circulo
				int pos1 = id + dado;
				int pos2 = id - dado;

				//Correcciones
				if (pos1 > 42)
					pos1 = pos1 - 42;
				if (pos2 < 0)
					pos2 = 42 + pos2;

				//Lo metemos en el vector
				this.movimientos.Add(pos1);
				this.movimientos.Add(pos2);

				//Movimientos hacia el interior del tablero
				//Desde casilla del "quesito"
				if ((id%7) == 0)
                {
					if (dado == 6)
						this.movimientos.Add(1000);
					else
						this.movimientos.Add(100 + (id / 7) + (dado-1));
                }
				//Desde casillas mas cercanas al "quesito inferior"
				else if ((id%7)==1 || (id%7)==2 || (id % 7) == 3)
                {
					if (dado > (id % 7))
					{
						int n = dado - (id % 7); //n es el que et sobra per pujar
						int m = (id - (id % 7)) / 7; //casella del "quesito"

						this.movimientos.Add(100 + m * 10 + (n-1));

						if (((id%7)==2 || (id % 7) == 3) && (dado>(7-(id%7))))
                        {
							n = dado - (7 - (id % 7));
							m = (id + (7 - (id % 7)) / 7);
                            this.movimientos.Add(100 + m * 10 + (n-1));
                        }
					}

                }
				//Desde las casillas mas cercanas al "quesito" superior
                else
                {
					if (dado > (7 - id % 7))
                    {
						int n = dado - (7-(id % 7));
						int m = (id + (7 - id % 7)) / 7;

						this.movimientos.Add(100 + m * 10 + (n-1));

						if (((id%7)==4 || (id%7)==5) && (dado > (id % 7)))
                        {
							n = dado - (id % 7);
							m = (id - (7 - id % 7)) / 7;

							this.movimientos.Add(100 + m * 10 + (n-1));
                        }

                    }
                }
            }

			//Casella central
			else if (id==1000)
            {
				int rama;
				if (dado == 6)
                {
					for(rama = 0; rama < 6; rama++)
                    {
						this.movimientos.Add(rama * 7);
                    }
                }
                else
                {
                    for (rama = 0; rama < 6; rama++)
                    {
						this.movimientos.Add(100 + rama * 10 + (5 - dado));
                    }
                }
            }

            //Caselles interiors (100-104,110-114,120-124,130-134,140-144,150-154)
            else
            {
				int posRama = id % 10; //(0-1-2-3-4)
				int rama = ((id - posRama) % 100)/10; //(0-1-2-3-4-5)

				//Moviments cap al cercle exterior
				if (dado > (posRama + 1))
                {
					int pos1 = (7 * rama) + (dado - (posRama+1));
					int pos2= (7 * rama) - (dado - (posRama+1));

					//Correcciones
					if (pos1 > 42)
						pos1 = pos1 - 42;
					if (pos2 < 0)
						pos2 = 42 + pos2;

					this.movimientos.Add(pos1);
					this.movimientos.Add(pos2);
				}
				else if (dado == (posRama + 1))
                {
					this.movimientos.Add(7 * rama);
                }
                else
                {
					this.movimientos.Add(100 + rama * 10 + (posRama - dado));
                }

				//Cap a l'interior del taulell
				if (dado < 5-posRama)
                {
					this.movimientos.Add(100 + 10 * rama + (dado + posRama));
                }
				else if (dado == 5 - posRama)
                {
					this.movimientos.Add(1000);
                }
                else
                {
					int n = dado - (5 - posRama); //moviments que em queden
					int m = rama + 1;
					if (m > 5)
						m = 0;
					while (m != rama)
                    {
						int a = 100 + m * 10 + (5 - n);
						this.movimientos.Add(a);
						m++;
						if (m > 5)
							m = 0;
                    }
                }
            }
        }

		//
	}
}
