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
		int[] movimientos;

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
		public int[] GetMovimientos()
        {
			return this.movimientos;
        }

		public void NuevaCasilla(int id, int dado)
        {
			this.id = id;
			this.dado = dado;
        }

		public void CalculaPosiblesMovimientos()
        {
			movimientos = new int[10];
			//Casilla forma parte del circulo exterior (ids de 1 a 41)
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
				this.movimientos[0] = pos1;
				this.movimientos[1] = pos2;

				//Movimientos hacia el interior del tablero
				//Desde casilla del "quesito"
				if ((id%7) == 0)
                {
					if (dado == 6)
						this.movimientos[2] = 1000;
					else
						this.movimientos[2] = 100 + (id / 7) + dado;
                }
				//Desde casillas mas cercanas al "quesito inferior"
				else if ((id%7)==1 || (id%7)==2 || (id % 7) == 3)
                {
					if (dado > (id % 7))
					{
						int n = dado - (id % 7);
						int m = (id - (id % 7)) % 7;

						this.movimientos[2] = 100 + m * 10 + n;

						if (((id%7)==2 || (id % 7) == 3) && (dado>(7-(id%7))))
                        {
							n = dado - (7 - (id % 7));
							m = (id + (7 - (id % 7)) % 7);
							this.movimientos[3] = 100 + m * 10 + n;
                        }
					}

                }
				//Desde las casillas mas cercanas al "quesito" superior
                else
                {
					if (dado > (7 - id % 7))
                    {
						int n = dado - (7-(id % 7));
						int m = (id + (7 - id % 7)) % 7;

						this.movimientos[2] = 100 + m * 10 + n;

						if (((id%7)==4 || (id%7)==5) && (dado > (id % 7)))
                        {
							n = dado - (id % 7);
							m = (id - (7 - id % 7)) % 7;

							this.movimientos[3] = 100 + m * 10 + n;
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
					int i = 0;
					for(rama = 0; rama < 6; rama++)
                    {
						this.movimientos[i] = rama * 7;
						i++;
                    }
                }
                else
                {
					int i = 0;
                    for (rama = 0; rama < 6; rama++)
                    {
						this.movimientos[i] = 100 + rama * 10 + (5 - dado);
						i++;
                    }
                }
            }
            //Caselles interiors (100-104,110-114,120-124,130-134,140-144,150-154)
            else
            {
				int posRama = id % 10;
				int rama = (id - posRama) % 10;
				int i = 0;
				//Moviments cap al cercle exterior
				if (dado > (posRama + 1))
                {
					int pos1 = (7 * rama) + (dado + posRama);
					int pos2= (7 * rama) - (dado - posRama);

					//Correcciones
					if (pos1 > 42)
						pos1 = pos1 - 42;
					if (pos2 < 0)
						pos2 = 42 + pos2;

					this.movimientos[i] = pos1;
					i++;
					this.movimientos[i] = pos2;
					i++;
				}
				else if (dado == (posRama + 1))
                {
					this.movimientos[i] = (7 * rama);
					i++;
                }
                else
                {
					this.movimientos[i] = 100 + rama * 10 + (posRama - dado);
					i++;
                }

				//Cap a l'interior del taulell
				if (dado < 5-posRama)
                {
					this.movimientos[i] = 100 + 10 * rama + (dado+posRama);
					i++;
                }
				else if (dado == 5 - posRama)
                {
					this.movimientos[i] = 1000;
					i++;
                }
                else
                {
					int n = dado - (5 - posRama);
					int m = rama + 1;
					if (m > 5)
						m = 0;
					while (m != rama)
                    {
						this.movimientos[i] = 100 + m * 10 + (5 - n);
						m++;
						if (m > 5)
							m = 0;
                    }
                }
            }
        }
	}
}
