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
            {
                ConectadosGridView.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                ConectadosGridView.Rows[i].Cells[0].Value = conectados[i];
            }
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
                //MessageBox.Show(Convert.ToString(tableros[i].DameIdPartida()));
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
            bool adios = false;
            while (adios == false)
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
                                nameUserTxt.Text = "Username: " + userName;
                            }
                            //Errores 
                            else if (mensaje == "1")
                            {
                                MessageBox.Show("Este usuario no existe");
                                adios = true;
                            }
                            else if (mensaje == "2")
                            {
                                MessageBox.Show("Contraseña incorrecta");
                                adios = true;
                            }
                            else if (mensaje == "3")
                            {
                                MessageBox.Show("Este usuario ya esta conectado");
                                adios = true;
                            }
                            else
                            {
                                MessageBox.Show("Error de consulta. Pruebe otra vez.");
                                adios = true;
                            }
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
                            adios = true;
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
                                MessageBox.Show("Invitaciones enviadas con exito\n excepto las de: "+show+"\nInténtalo de nuevo");
                            }
                            break;

                        case 8: //Notificación de invitacion a una partida
                            // "nombreHost*idPartida"
                            string[] split = mensaje.Split('*');
                            invitacion = new Invitacion();
                            invitacion.SetHost(split[0]);
                            invitacion.ShowDialog();
                            string respuesta = "7/" + invitacion.GetRespuesta() + "/" + split[1]+"\0";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(respuesta);
                            server.Send(msg);                       
                            
                            break;

                        case 9: //Notificación de inicio de partida "idPartida*listaJugadores(*)*rolTuJugador"
                            //Iniciamos un thread para esta partida
                            ThreadStart ts = delegate { NuevaPartida(mensaje); };
                            Thread T = new Thread(ts);
                            T.Start();
                            break;

                        case 10://Notificación de fin de partida "idPartida*nombreFinalizador"
                            //Enviar esta notificacion al Tablero correspondiente
                            int numTablero = DamePosicionLista(tableros, Convert.ToInt32(mensaje.Split('*')[0]));
                            if(numTablero>=0)
                                tableros[numTablero].Close();
                            MessageBox.Show(mensaje.Split('*')[1] + " ha finalizado \nla partida " + mensaje.Split('*')[0]);
                            break;

                        case 11: //Notificación del resultado del dado de un jugador "idPartida*resDado*nombreTirador"
                            //Enviar al tablero correspondiente 
                            int idPartida = Convert.ToInt32(mensaje.Split('*')[0]);
                            numTablero = DamePosicionLista(tableros, idPartida);
                            tableros[numTablero].NuevoMovimiento(mensaje);
                            break;
                        case 12: //Notificación del movimiento de otro jugador "idPartida*nombreJugador*rolJugador*nuevaCasilla"
                            //Enviar al tablero correspondiente 
                            idPartida = Convert.ToInt32(mensaje.Split('*')[0]);
                            numTablero = DamePosicionLista(tableros, idPartida);
                            tableros[numTablero].setCasillaJugador(mensaje);
                            break;

                        case 13: //Notificacion del resultado de un jugador "idPartida*nombreJugador*resultado(0,1,2)*(siguienteTurno*quesito)"
                                 // 0 -> mal contestada pero se actualiza turno
                                 // 1 -> bien contestada pero sin quesito
                                 // 2 -> bien contestada y con quesito 
                            idPartida = Convert.ToInt32(mensaje.Split('*')[0]);
                            numTablero = DamePosicionLista(tableros, idPartida);
                            tableros[numTablero].ActualizarResultadoPregunta(mensaje);
                            break;

                        case 14: //Notificación que alguien ha conseguido los 6 quesitos 
                            //Enviar al tablero correspondiente "idPartida*nombreGanador"
                            idPartida = Convert.ToInt32(mensaje.Split('*')[0]);
                            numTablero = DamePosicionLista(tableros, idPartida);
                            tableros[numTablero].Ganador(mensaje);
                            break;

                        case 15: //Notificacion de mensaje en el chat
                            //Enviar al tablero correspondiente "idPartida*nombre*mensaje"
                            idPartida = Convert.ToInt32(mensaje.Split('*')[0]);
                            numTablero = DamePosicionLista(tableros, idPartida);
                            tableros[numTablero].NuevoMensajeChat(mensaje);
                            break;
                        case 16: // Notifica que la partida no se inicia porque x lo ha rechazado "idPartida*x"
                            idPartida = Convert.ToInt32(mensaje.Split('*')[0]);
                            MessageBox.Show(mensaje.Split('*')[1] + " ha rechazado la partida "+ mensaje.Split('*')[0]+"\n No se iniciará");
                            break;
                    }
                }
                catch (SocketException)
                {
                    MessageBox.Show("Server desconectado");
                }
            }
            //Mensaje de desconexion
            byte[] msg0 = System.Text.Encoding.ASCII.GetBytes("0/");
            server.Send(msg0);

            //Desconexión del servidor
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        //Funcion para ejecutar un nuevo thread con el formulario de una partida
        private void NuevaPartida(string mensaje)
        {
            //Creamos el nuevo formulario tablero para la nueva partida
            Tablero tablero = new Tablero();
            tablero.SetPartida(mensaje,this.server,this.userName);
            tableros.Add(tablero);
            tablero.ShowDialog();
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
            conexion.Visible = false;
            invitadosGridView.Visible = false;
            label6.Visible = false;

            //Fondo
            candadoBox.Image = Image.FromFile(".\\candadoCerrado.jpg");
            candadoBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        //Botón de conexión/desconexión.
        private void conexion_Click(object sender, EventArgs e)
        {
            //En funcion del estado del sistema (desconectado/conectado), el boton permite conectarse/desconectarse respectivamente
            //Caso Conectado --> Queremos desconectarnos
            if (c==1)
            {
                try
                {
                    //Cerramos todos los tableros que haya abiertos
                    for (int i = 0; i < tableros.Count; i++)
                        tableros[i].Close();
                    tableros.Clear();

                    //Mensaje de desconexion
                    string mensaje = "0/";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Detenemos el thread
                    atender.Abort();

                    //Desconexión del servidor
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    conexion.Visible=false;
                    c = 0;

                    //Cambios de color de fondos
                    this.BackColor = Color.DarkSlateGray;

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
                //Se conecta al servidor solamente entrar

                IPAddress direc = IPAddress.Parse("147.83.117.22");
                IPEndPoint ipep = new IPEndPoint(direc, 50051);

                //@IP_Shiva1: 147.83.117.22
                //@IP_LocalHost: 192.168.56.102
                //#Port_Shiva1: 50051.2.3
                //#Port_localhost: 9080

                //Creamos el socket 
                this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
               
                server.Connect(ipep);
                
                conexion.Text = "Desconectar";
                conexion.Visible = true;
                c = 1;

                //Ponemos en marcha el thread que atenderá los mensajes de los clientes
                ThreadStart ts = delegate { AtenderServidor(); };
                atender = new Thread(ts);
                atender.Start();
                
                userName = NameBox.Text;
                //Construimos el mensaje y lo enviamos (Codigo 1/ --> LogIn)
                string mensaje = "1/" + NameBox.Text + "/" + PasswordBox.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

            }
            catch (SocketException)
            {
                MessageBox.Show("No he podido conectar con el servidor");
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
                //Creamos el socket i nos conectamos
                IPAddress direc = IPAddress.Parse("147.83.117.22");
                IPEndPoint ipep = new IPEndPoint(direc, 50051);
                this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Connect(ipep);

                //@IP_Shiva1: 147.83.117.22
                //@IP_LocalHost: 192.168.56.102
                //#Port_Shiva1: 50051.2.3
                //#Port_localhost: 9080

                //Abrimos el thread
                ThreadStart ts = delegate { AtenderServidor(); };
                atender = new Thread(ts);
                atender.Start();

                //Construimos el mensaje y lo enviamos (Codigo 2/ --> Registrarse)
                if ((userBox.Text != "0") && (password2Box.Text != "0"))
                {
                    string mensaje = "2/" + userBox.Text + "/" + password2Box.Text + "/" + mailBox.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                    MessageBox.Show("Ningún campo puede ser 0");              

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
            if (invitadosGridView.Visible == false)
            {
                invitados = new List<string>();
                invitadosGridView.Visible = true;
                invitarButton.Visible = true;
                label6.Visible = true;
            }

            string invitado = ConectadosGridView.CurrentCell.Value.ToString();

            //Comprovamos que no somos nosotros mismos
            if (invitado == userName)
                MessageBox.Show("No te puedes autoinvitar");
            else
            {
                if (invitados.Count <= 3)
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
                    if (encontrado == false)
                    {
                        invitados.Add(invitado);
                        CrearInvitadosGridView(invitados);
                    }
                }
                else
                    MessageBox.Show("El numero maximo de invitados es 3");

            }
                
            ConectadosGridView.SelectAll();
        }


        private void CrearInvitadosGridView(List<string> invitados)
        {
            invitadosGridView.ColumnCount = 1;
            invitadosGridView.RowCount = invitados.Count;
            invitadosGridView.ColumnHeadersVisible = false;
            invitadosGridView.RowHeadersVisible = false;
            invitadosGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            invitadosGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            invitadosGridView.SelectAll();

            for (int i = 0; i < invitados.Count; i++)
                invitadosGridView.Rows[i].Cells[0].Value = invitados[i];
                
        }
        private void invitarButton_Click(object sender, EventArgs e)
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
            label6.Visible = false;
            invitadosGridView.Visible = false;
            invitarButton.Visible = false;
        }

        private void invitadosGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string eliminado = invitadosGridView.CurrentCell.Value.ToString();
            invitados.Remove(eliminado);
            if (invitados.Count == 0)
            {
                invitadosGridView.Visible = false;
                invitarButton.Visible = false;
            }
            else
                CrearInvitadosGridView(invitados);

        }
    }
}
