using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Trivial
{
    public partial class Acceso : Form
    {
        Socket server;
        Thread atender;
        Invitacion invitacion;

        int c = 0;
        int ask = 0;

        string userName;

        delegate void DelegadoParaEscribir(string[] conectados);

        List<string> invitados;

        List<Tablero> tableros;

        public Acceso()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Bitmap portada = new Bitmap(Application.StartupPath + @"\portada.png");
            this.BackgroundImage = portada;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        //Funcion para que un thread pueda modificar objetos del formulario
        public void ListaConectadosGridView(string[] conectados)
        {
                //Queremos mostrar los datos en un Data Grid View
                //Configuracion
                labelConectados.Visible = true;
                ConectadosGridView.Visible = true;
                ConectadosGridView.ColumnCount = 1;
                ConectadosGridView.RowCount = conectados.Length;
                ConectadosGridView.ColumnHeadersVisible = false;
                ConectadosGridView.RowHeadersVisible = false;
                ConectadosGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                ConectadosGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                ConectadosGridView.SelectAll();

                //Introduccion de los datos
                for (int i = 0; i < conectados.Length; i++)
                    ConectadosGridView.Rows[i].Cells[0].Value = conectados[i];

                ConectadosGridView.Show();

        }

        //Funcion para obtener la posicion en la lista de tableros de un id de partida
        private int DamePosicionLista(List<Tablero> tableros, int idPartida)
        {
            //Retorna el tablero asignado a la partida si todo va bien y -1 si no
            bool found = false;
            int i = 0;
            while (i < tableros.Count && found == false)
            {
                MessageBox.Show(Convert.ToString(tableros[i].DameIdPartida()));
                if (tableros[i].DameIdPartida() == idPartida)
                    found = true;
                else
                    i = i + 1;
            }
            if (found == true)
                return i;
            else
                return -1;
        }

        //Funcion que ejecutará el thread de recepción de respuestas del servidor
        private void AtenderServidor()
        {
            while (true)
            {
                try
                {
                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                    int codigo = Convert.ToInt32(trozos[0]);
                    string mensaje = trozos[1].Split('\0')[0];

                    //Decidimos a quien le pasamos la información
                    switch (codigo)
                    {
                        case 1: //Respuesta de la comprovación para el LogIn
                            if (mensaje == "0") //Login correcto
                            {
                                //establecemos una lista de partidas para esta conexión
                                tableros = new List<Tablero>();

                                //Establecemos pantalla del juego
                                consultasButton.Visible = true;
                                accederBox.Visible = false;
                                registroBox.Visible = false;
                                nameUserTxt.Visible = true;
                                ConectadosGridView.Visible = true;
                                labelConectados.Visible = true;
                                invitarButton.Visible = true;
                                nameUserTxt.Text = "Estas jugando con: " + userName;
                                
                            }

                            //Errores
                            else if (mensaje == "1")
                                MessageBox.Show("Este usuario no existe");
                            else if (mensaje == "2")
                                MessageBox.Show("Contraseña incorrecta");
                            else if (mensaje == "3")
                                MessageBox.Show("Este usuario ya esta conectado");
                            else
                                MessageBox.Show("Error de consulta. Pruebe otra vez.");
                            break;

                        case 2: //Respuesta del Insert de nuevos jugadores
                            if (mensaje == "0")
                                MessageBox.Show("Se ha registrado correctamente.");

                            //Errores
                            else if (mensaje == "1")
                                MessageBox.Show("Este nombre de usuario ya existe.");
                            else
                                MessageBox.Show("Error de consulta, pruebe otra vez.");

                            userBox.Clear();
                            password2Box.Clear();
                            mailBox.Clear();
                            break;

                        case 3: //Recuperación de la contrasenya
                            if (mensaje == "-1")
                                MessageBox.Show("Error de consulta. Prueba otra vez.");
                            else
                                MessageBox.Show("Tu contraseña es: " + mensaje);
                            break;

                        case 4: //Partida más larga
                            if (mensaje == "-1")
                                MessageBox.Show("Error de consulta. Prueba otra vez.");
                            else if (mensaje == "-2")
                                MessageBox.Show("No se ha encontrado ninguna partida en la base de datos");
                            else
                                MessageBox.Show("La partida más larga ha sido la número " + mensaje + ".");
                            break;

                        case 5: //Jugador con más puntos
                            if (mensaje == "-1")
                                MessageBox.Show("Error de consulta. Prueba otra vez");
                            else if (mensaje == "-2")
                                MessageBox.Show("No se ha encontrado ningún jugador en la base de datos.");
                            else
                                MessageBox.Show("El jugador con más puntos es: " + mensaje + ".");
                            break;

                        case 6: //Notificación de actualización de la lista de conectados
                            if (mensaje == "-1")
                                MessageBox.Show("No hay usuarios conectados.");
                            else
                            {
                                //Separamos los conectados y los introducimos en un vector
                                string[] conectados = mensaje.Split('*');

                                //Aplicamos el delegado para modificar el Data Grid View
                                DelegadoParaEscribir delegado = new DelegadoParaEscribir(ListaConectadosGridView);
                                ConectadosGridView.Invoke(delegado, new object[] { conectados });
                                
                            }
                            break;

                        case 7: //Respuesta a la peticion de invitacion
                            if (mensaje == "0")
                                MessageBox.Show("Invitaciones enviadas correctamente");
                            else
                            {
                                //Recibimos las invitaciones fallidas
                                string[] noDisponibles = mensaje.Split('*');
                                string show = "";
                                for (int n = 0; n < noDisponibles.Length; n++)
                                    show = show + noDisponibles[n] + ",";
                                show = show.Remove(show.Length - 1);
                                MessageBox.Show("Invitaciones enviadas con exito\n excepto las de: "+show);
                            }
                            break;

                        case 8: //Notificación de invitacion a una partida
                            string[] split = mensaje.Split('*');
                            invitacion = new Invitacion();
                            invitacion.SetHost(split[0]);
                            invitacion.ShowDialog();
                            string respuesta = "7/" + invitacion.GetRespuesta() + "/" + split[1]+"\0";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(respuesta);
                            server.Send(msg);                       
                            
                            break;

                        case 9: //Notificación de inicio de partida
                            //Iniciamos un thread para esta partida
                            ThreadStart ts = delegate { NuevaPartida(mensaje); };
                            Thread T = new Thread(ts);
                            T.Start();
                            break;

                        case 10://Notificación de fin de partida
                            //Enviar esta notificacion al Tablero correspondiente
                            MessageBox.Show("La partida " + mensaje + " ha finalizado.");
                            int numTablero = DamePosicionLista(tableros, Convert.ToInt32(mensaje));
                            MessageBox.Show(Convert.ToString(numTablero));
                            if(numTablero>=0)
                                tableros[numTablero].Close();
                            break;

                        case 11: //Notificación del resultado del dado de un jugador y del siguiente turno
                            //Enviar al tablero correspondiente "idPartida*resDado*nombreTirador*siguienteTurno(rol)"
                            int idPartida = Convert.ToInt32(mensaje.Split('*')[0]);
                            numTablero = DamePosicionLista(tableros, Convert.ToInt32(idPartida));
                            tableros[numTablero].NuevoMovimiento(mensaje, codigo);
                            break;
                    }
                }
                catch (SocketException)
                {
                    MessageBox.Show("Server desconectado");
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al recibir los datos");
                }
            }
            
        }

        //Funcion para ejecutar un nuevo thread con el formulario de una partida
        private void NuevaPartida(string mensaje)
        {
            Tablero tablero = new Tablero();
            tablero.SetPartida(mensaje,this.server);
            tableros.Add(tablero);
            MessageBox.Show("1:" + Convert.ToString(tableros.Count));
            tablero.ShowDialog();
            MessageBox.Show("2:" + Convert.ToString(tableros.Count));
            //En un futur aqui recollirem el historial de la partida
            //En el momento que se cierra el tablero (se acaba el ShowDialog) quitamos el tablero de la lista
            tableros.Remove(tablero);

            //Acaba el thread para esta partida
        }

        //Iniciacion del Form 
        private void Acceso_Load(object sender, EventArgs e)
        {
            //Partes ocultas al inicio
            consultaBox.Visible = false;
            consultasButton.Visible = false;
            ConectadosGridView.Visible = false;
            labelConectados.Visible = false;
            nameUserTxt.Visible = false;
            invitarButton.Visible = false;

            //Fondo
            candadoBox.Image = Image.FromFile(".\\candadoCerrado.jpg");
            candadoBox.SizeMode = PictureBoxSizeMode.StretchImage;
            

            //Se conecta al servidor solamente entrar
            IPAddress direc = IPAddress.Parse("147.83.117.22");    //@IP_Shiva1: 147.83.117.22
                                                                    //@IP_LocalHost: 192.168.56.102
            IPEndPoint ipep = new IPEndPoint(direc, 50051); //#Port_Shiva1: 50051.2.3
                                                           //#Port_localhost: 9080

            try
            {
                //Creamos el socket 
                this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Connect(ipep);
                luz.BackColor = Color.Green;
                conexion.Text = "Desconectar";
                c = 1;

                //Ponemos en marcha el thread que atenderá los mensajes de los clientes
                ThreadStart ts = delegate { AtenderServidor(); };
                atender = new Thread(ts);
                atender.Start();
            }
            catch (SocketException ex)
            {
                            
            }
        }


        //Botón de conexión/desconexión.
        private void conexion_Click(object sender, EventArgs e)
        {
            //En funcion del estado del sistema (desconectado/conectado), el boton permite conectarse/desconectarse respectivamente

            //Caso Desconectado --> Queremos conectarnos
            if (c == 0)
            {
                IPAddress direc = IPAddress.Parse("147.83.117.22");
                IPEndPoint ipep = new IPEndPoint(direc, 50051);

                //Creamos el socket 
                this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);
                    luz.BackColor = Color.Green;
                    conexion.Text = "Desconectar";
                    c = 1;

                    //Ponemos en marcha el thread que atenderá los mensajes de los clientes
                    ThreadStart ts = delegate { AtenderServidor(); };
                    atender = new Thread(ts);
                    atender.Start();
                }
                catch (SocketException)
                {
                    MessageBox.Show("No he podido conectar con el servidor");
                }
                catch (Exception)
                {
                    MessageBox.Show("Se ha producido un error.");
                }
                
            }

            //Caso Conectado --> Queremos desconectarnos
            else
            {
                try
                {
                    //Mensaje de desconexion
                    string mensaje = "0/";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Detenemos el thread
                    atender.Abort();

                    //Desconexión del servidor
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    conexion.Text = "Conectar";
                    c = 0;

                    //Cambios de color de fondos
                    this.BackColor = Color.DarkSlateGray;
                    luz.BackColor = Color.DarkSlateGray;

                    //Establecemos pantalla inicial
                    consultaBox.Visible = false;
                    consultasButton.Visible = false;
                    accederBox.Visible = true;
                    registroBox.Visible = true;
                    ConectadosGridView.Visible = false;
                    labelConectados.Visible = false;
                    nameUserTxt.Visible = false;
                    invitarButton.Text = "Invitar";
                    invitarButton.Visible = false;

                    //Cerramos todos los tableros que haya abiertos
                    for (int i = 0; i < tableros.Count; i++)
                        tableros[i].Close();
                    tableros.Clear();

                    //Vaciamos las casillas por si habian quedado rellenadas
                    NameBox.Clear();
                    PasswordBox.Clear();

                }
                catch (Exception)
                {
                    MessageBox.Show("Ya estás desconectado.");
                }
            }

        }

        //Botón para acceder (LogIn)
        private void Login_Click(object sender, EventArgs e)
        {
            try
            {
                userName = NameBox.Text;
                //Construimos el mensaje y lo enviamos (Codigo 1/ --> LogIn)
                string mensaje = "1/" + NameBox.Text + "/" + PasswordBox.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

            }
            catch (Exception)
            {
                MessageBox.Show("ERROR: Compruebe que está conectado al servidor.");
            }
        }

        //Botón para registrarse 
        private void Registrarme_Click(object sender, EventArgs e)
        {
            try
            {
                //Construimos el mensaje y lo enviamos (Codigo 2/ --> Registrarse)
                string mensaje = "2/" + userBox.Text + "/" + password2Box.Text + "/" + mailBox.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

            }
            catch (Exception)
            {
                MessageBox.Show("ERROR: Compruebe que está conectado al servidor.");
            }

        }

        //Botón para hacer consultas a la BBDD a traves del servidor
        private void Preguntar_Click(object sender, EventArgs e)
        {
            try
            {
                //Queremos saber nuestra contraseña
                if (Contraseña.Checked) 
                {
                    //Construimos el mensaje y lo enviamos (Codigo 3/ --> Dame Contraseña)
                    string mensaje = "3/" + NameBox.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                }

                //Queremos saber cuanto dura la partida mas larga
                else if (duracion.Checked)
                {
                    //Construimos el mensaje y lo enviamos (Codigo 4/ --> Dame partida + larga)
                    string mensaje = "4/";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                }

                //Queremos saber el jugador con mas puntos
                else
                {
                    //Construimos el mensaje y lo enviamos (Codigo 5/ --> Dame jugador con + puntos)
                    string mensaje = "5/";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                }
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR: Compruebe que está conectado al servidor.");
            }
        }

       
        //Mostrar y ocultar las contraseñas.
        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
            PasswordBox.UseSystemPasswordChar = true;
            candadoBox.Image = Image.FromFile(".\\candadoCerrado.jpg");
        }

        //Cambio del pictureBox Candado en funcion si desea mostrar u ocultar la contraseña
        private void candadoBox_Click(object sender, EventArgs e)
        {
            //Desea poder ver la contraseña
            if (PasswordBox.UseSystemPasswordChar == true)
            {
                PasswordBox.UseSystemPasswordChar = false;
                candadoBox.Image = Image.FromFile(".\\candadoAbierto.jpg");
            }

            //Desea ocultar la contraseña
            else
            {
                PasswordBox.UseSystemPasswordChar = true;
                candadoBox.Image = Image.FromFile(".\\candadoCerrado.jpg");
            }
        }

 

        //Boton para desplegar/esconder las posibles consultas
        private void consultasButton_Click(object sender, EventArgs e)
        {
            //Si las consultas estan desplegadas --> Queremos esconderlas
            if (ask==0)
            {
                consultaBox.Visible = false;
                consultasButton.Text = "Mostrar\n consultas";
                ask = 1;
            }
            //Si las consultas estas escondidas --> Queremos desplegarlas
            else
            {
                consultaBox.Visible = true;
                consultasButton.Text = "Ocultar\n consultas";
                ask = 0;
            }

        }

        //Si cerramos el form directamente tambien tenemos que desconectar el socket y detener el thread
        private void Acceso_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (c == 1) {

                    //Mensaje de desconexion
                    string mensaje = "0/";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Detención del thread
                    atender.Abort();

                    //Desconexión del servidor
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();

                    //Cerramos todos los tableros que haya abiertos
                    for (int i = 0; i < tableros.Count; i++)
                        tableros[i].Close();
                    tableros.Clear();
                }
            }
            catch (Exception)
            {
             
            }
        }

        private void ConectadosGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Solo funciona cuando se habilita la funcion de invitar con el boton invitarButton
            if ((invitarButton.Text == "Enviar\n Invitación") && (invitados.Count <= 3))
            {
                string invitado = ConectadosGridView.CurrentCell.Value.ToString();

                //Comprovamos que no somos nosotros mismos
                if (invitado == userName)
                    MessageBox.Show("No te puedes autoinvitar");
                else
                {
                    //Comprovamos que no este ya en la lista para añadirlo
                    int i = 0;
                    bool encontrado = false;
                    while ((i < invitados.Count) && (encontrado == false))
                    {
                        if (invitado == invitados[i])
                            encontrado = true;
                        else
                            i = i + 1;
                    }
                    if (encontrado == true)
                    {
                        invitados.Remove(invitado);
                        MessageBox.Show("Has eliminado a " + invitado);
                    }
                    else
                    {
                        invitados.Add(invitado);
                        MessageBox.Show("Has añadido a " + invitado);
                    }
                }
            }
            else if ((invitarButton.Text == "Enviar\n Invitación") && (invitados.Count > 3))
                MessageBox.Show("El numero maximo de invitados es 3");
            ConectadosGridView.SelectAll();
        }

        private void invitarButton_Click(object sender, EventArgs e)
        {
            
            if (invitarButton.Text == "Invitar")
            {
                //Iniciamos la recopilacion de invitados
                MessageBox.Show("Haz click sobre los jugadores que quieras invitar");
                invitarButton.Text = "Enviar\n Invitación";
                invitados = new List<string>();
            }
            else
            {
                invitarButton.Text = "Invitar";

                //si no se clica a nadie no hace nada y vuelve al estado inicial
                if (invitados.Count != 0)
                {
                    //Construimos el mensaje
                    string mensaje = "6/";
                    for (int i = 0; i < invitados.Count; i++)
                    {
                        mensaje = mensaje + invitados[i] + "*";
                    }

                    mensaje = mensaje.Remove(mensaje.Length - 1);

                    //Lo enviamos por el socket (Codigo 6 --> Invitar a jugadores)
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                              
            }
        }
    }
}
