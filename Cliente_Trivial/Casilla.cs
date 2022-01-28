using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivial
{
    public class Casilla
    {
		//Atributos
		int id;
        List<int>[] movimientos; //
        string categoria; //Geografia,Historia,...,Tira otra vez
        string color;
		int x; // Para ubicar pictureBox no acepta double
		int y;
        //int xubi;             this.xubi = Convert.ToInt32(x + ubiBox.Size.Width / 2);
        //int yubi;             this.yubi = Convert.ToInt32(y + ubiBox.Size.Height);

        //Constructor
        public Casilla(int id, int xorigen, int yorigen)
        {
            this.id = id;
            this.movimientos = new List<int>[6];

            // Determinar categoria
            if (this.id == 1000)
            {
                this.categoria = "Central";
                this.color = "Central";
            }
            else
            {
                bool encontrado = false;
                List<int> amarillas = new List<int>();
                int[] camarillas = new int[] { 1, 10, 21, 32, 41, 100, 124, 133, 142, 151 };
                amarillas.AddRange(camarillas);
                foreach (int i in amarillas)
                {
                    if (i == this.id)
                    {
                        encontrado = true;
                        this.categoria = "Historia";
                        this.color = "Amarillo";
                    }
                }
                if (encontrado == false)
                {
                    List<int> azules = new List<int>();
                    int[] cazules = new int[] { 0, 11, 20, 22, 31, 103, 112, 121, 130, 154 };
                    azules.AddRange(cazules);
                    foreach (int i in azules)
                    {
                        if (i == this.id)
                        {
                            encontrado = true;
                            this.categoria = "Geografía";
                            this.color = "Azul";
                        }
                    }
                    if (encontrado == false)
                    {
                        List<int> rojas = new List<int>();
                        int[] crojo = new int[] { 3, 14, 25, 36, 34, 114, 123, 132, 141, 150 };
                        rojas.AddRange(crojo);
                        foreach (int i in rojas)
                        {
                            if (i == this.id)
                            {
                                encontrado = true;
                                this.categoria = "Tecnología";
                                this.color = "Rojo";
                            }
                        }
                        if (encontrado == false)
                        {
                            List<int> lilas = new List<int>();
                            int[] clila = new int[] { 4, 13, 15, 24, 35, 102, 111, 120, 144, 153 };
                            lilas.AddRange(clila);
                            foreach (int i in lilas)
                            {
                                if (i == this.id)
                                {
                                    encontrado = true;
                                    this.categoria = "Entretenimiento";
                                    this.color = "Lila";
                                }
                            }
                            if (encontrado == false)
                            {
                                List<int> naranjas = new List<int>();
                                int[] cnara = new int[] { 6, 8, 17, 28, 39, 101, 110, 134, 143, 152 };
                                naranjas.AddRange(cnara);
                                foreach (int i in naranjas)
                                {
                                    if (i == this.id)
                                    {
                                        encontrado = true;
                                        this.categoria = "Deportes";
                                        this.color = "Naranja";
                                    }
                                }
                                if (encontrado == false)
                                {
                                    List<int> verdes = new List<int>();
                                    int[] cverde = new int[] { 7, 18, 20, 22, 38, 104, 113, 122, 131, 140 };
                                    verdes.AddRange(cverde);
                                    foreach (int i in verdes)
                                    {
                                        if (i == this.id)
                                        {
                                            encontrado = true;
                                            this.categoria = "Ciencia";
                                            this.color = "Verde";
                                        }
                                    }
                                    if (encontrado == false) // { 2, 5, 9, 12, 16, 19, 23, 26, 30, 33, 37, 40 };
                                    {
                                        this.categoria = "Tira otra vez";
                                        this.color = "Gris";
                                    }
                                }
                            }
                        }
                    }
                }
            }         
            
            // Calculo coordenadas
            double x, y;
            double espesor = 45; // Diferencia entre la 00 y la 01
            double radi = 675 / 2; // Cuestiones esteticas
            double alpha = 8.57; // El incremento de angulo que debería haber entre casillas perimetro       
            double r04 = 2 * espesor;
            double r03 = 3 * espesor;
            double r02 = 4 * espesor;
            double r01 = 5 * espesor;
            double r00 = 6 * espesor;
            double rtotal = radi; //Radio del tablero
            if (id == 1000)
            {
                x = xorigen;
                y = yorigen;
            }
            else
            {
                double beta;
                double dist_origen;
                if ((id < 42) && (id >= 0)) // Significa que estas en el perimetro
                {
                    dist_origen = rtotal - (rtotal - r00) / 2;
                    beta = id * alpha;
                }
                else
                {
                    int ppiso = id % 10;
                    int rriera = (id - ppiso) % 100 / 10;
                    beta = rriera * 7 * alpha;
                    if (ppiso == 0)
                        dist_origen = r00 - espesor / 2;
                    else if (ppiso == 1)
                        dist_origen = r01 - espesor / 2;
                    else if (ppiso == 2)
                        dist_origen = r02 - espesor / 2;
                    else if (ppiso == 3)
                        dist_origen = r03 - espesor / 2;
                    else
                        dist_origen = r04 - espesor / 2;
                }
                x = xorigen + dist_origen * Math.Sin(beta * Math.PI / 180);
                y = yorigen - dist_origen * Math.Cos(beta * Math.PI / 180);

            }
            this.x = Convert.ToInt32(x);
            this.y = Convert.ToInt32(y);
        }

		//Métodos
		public int GetId()
        {
			return this.id;
        }
        public string GetCategoria()
        {
            return this.categoria;
        }
        public int GetX()
        {
            return this.x;
        }
        public int GetY()
        {
            return this.y;
        }
        public List<int> GetMovimientos(int dado)
        {
			return this.movimientos[dado-1];
        }
		public void CalculaPosiblesMovimientos(int dado)
        {
			this.movimientos[dado-1] = new List<int>();
            int id = this.id;
			//Casilla forma parte del circulo exterior (ids de 0 a 41)
			if ((id>=0) && (id < 42))
            {
				//Movimientos dentro del circulo
				int pos1 = id + dado;
				int pos2 = id - dado;

				//Correcciones
				if (pos1 >= 42)
					pos1 = pos1 - 42;
				if (pos2 < 0)
					pos2 = 42 + pos2;

				//Lo metemos en el vector
				this.movimientos[dado - 1].Add(pos1);
				this.movimientos[dado - 1].Add(pos2);

				//Movimientos hacia el interior del tablero
				//Desde casilla del "quesito"
				if ((id%7) == 0)
                {
					if (dado == 6)
						this.movimientos[dado - 1].Add(1000);
					else
						this.movimientos[dado - 1].Add(100 + (id/7)*10 + (dado-1));
                }
				//Desde casillas mas cercanas al "quesito inferior"
				else if ((id%7)==1 || (id%7)==2 || (id % 7) == 3)
                {
					if (dado > (id % 7))
					{
						int n = dado - (id % 7); //n es el que et sobra per pujar
						int m = (id - (id % 7)) / 7; //casella del "quesito"

						this.movimientos[dado - 1].Add(100 + m * 10 + (n-1));

						if (((id%7)==2 || (id % 7) == 3) && (dado>(7-(id%7))))
                        {
							n = dado - (7 - (id % 7));
							m = (id + (7 - (id % 7))) / 7;
                            this.movimientos[dado - 1].Add(100 + m * 10 + (n-1));
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
                        if (m >= 6)
                            m = 0;

						this.movimientos[dado - 1].Add(100 + m * 10 + (n-1));

						if (((id%7)==4 || (id%7)==5) && (dado >((id % 7))))
                        {
							n = dado - (id % 7);
							m = (id - (7 - id % 7)) / 7;

							this.movimientos[dado - 1].Add(100 + m * 10 + (n-1));
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
						this.movimientos[dado - 1].Add(rama * 7);
                    }
                }
                else
                {
                    for (rama = 0; rama < 6; rama++)
                    {
						this.movimientos[dado - 1].Add(100 + rama * 10 + (5 - dado));
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
					if (pos1 >= 42)
						pos1 = pos1 - 42;
					if (pos2 < 0)
						pos2 = 42 + pos2;

					this.movimientos[dado - 1].Add(pos1);
					this.movimientos[dado - 1].Add(pos2);
				}
				else if (dado == (posRama + 1))
                {
					this.movimientos[dado - 1].Add(7 * rama);
                }
                else
                {
					this.movimientos[dado - 1].Add(100 + rama * 10 + (posRama - dado));
                }

				//Cap a l'interior del taulell
				if (dado < 5-posRama)
                {
					this.movimientos[dado - 1].Add(100 + 10 * rama + (dado + posRama));
                }
				else if (dado == 5 - posRama)
                {
					this.movimientos[dado - 1].Add(1000);
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
						this.movimientos[dado - 1].Add(a);
						m++;
						if (m > 5)
							m = 0;
                    }
                }
            }
        }
    }
}
