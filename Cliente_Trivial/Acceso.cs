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

        RespuestaConsultas formConsultas;

        public Acceso()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Bitmap portada = new Bitmap(Application.StartupPath + @"\fondo1.png");
            this.BackgroundImage = portada;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            Bitmap host = new Bitmap(Application.StartupPath + @"\JugAzul.png");
            Bitmap jug1 = new Bitmap(Application.StartupPath + @"\JugLila.png");
            Bitmap jug2 = new Bitmap(Application.StartupPath + @"\JugVerde.png");
            Bitmap jug3 = new Bitmap(Application.StartupPath + @"\JugRojo.png");

            List<Bitmap> piezasbit = new List<Bitmap>();
            Bitmap[] bitlist = new Bitmap[] { host, jug1, jug2, jug3 };
            piezasbit.AddRange(bitlist);

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
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
            ConectadosGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ConectadosGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ConectadosGridView.BackgroundColor = Color.White;
            
            ConectadosGridView.SelectAll();

            int totalRowHeight = ConectadosGridView.ColumnHeadersHeight;
            //Introduccion de los datos
            for (int i = 0; i < conectados.Length; i++)
            {
                ConectadosGridView.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                ConectadosGridView.Rows[i].Cells[0].Value = conectados[i];
                totalRowHeight += ConectadosGridView.Rows[i].Height;
            }
            ConectadosGridView.Height = totalRowHeight;
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
            while (adios==false)
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
                                Bitmap portada = new Bitmap(Application.StartupPath + @"\fondo2.jpg");
                                this.BackgroundImage = portada;
                                this.BackgroundImageLayout = ImageLayout.Stretch;

                                inv_lbl.Visible = true;
                                inv_lbl.Text = userName + ", pulsa sobre quién quieras invitar";
                                consultasButton.Visible = true;
                                accederBox.Visible = false;
                                registroBox.Visible = false;
                                ConectadosGridView.Visible = true;
                                labelConectados.Visible = true;
                                

                                conexion.Text = "Desconectar";
                                conexion.Visible = true;
                                c = 1;

                                regLabel.Visible = false;
                                regVisible.Visible = false;
                                inicio.Visible = false;
                                eliminarLbl.Visible = false;
                                eliminarCuenta.Visible = false;
                            }
                            //Errores 
                            else if (mensaje == "1")
                            {
                                MessageBox.Show("Este usuario no existe");
                                NameBox.Clear();
                                PasswordBox.Clear();
                                adios = true;
                            }
                            else if (mensaje == "2")
                            {
                                MessageBox.Show("Contraseña incorrecta");
                                PasswordBox.Clear();
                                adios = true;
                            }
                            else if (mensaje == "3")
                            {
                                MessageBox.Show("Este usuario ya esta conectado");
                                NameBox.Clear();
                                PasswordBox.Clear();
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
                            {
                                MessageBox.Show("Se ha registrado correctamente.");
                                registroBox.Visible = false;
                                inicio.Visible = false;
                                regVisible.Visible = true;
                                accederBox.Visible = true;
                                regLabel.Visible = true;
                                eliminarLbl.Visible = true;
                                eliminarCuenta.Visible = true;
                            }
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

                        case 3: //"3/0 o jugador1*jugador2*..." (0-No ha jugado con nadie, lista contrincantes)
                            if (mensaje == "-1")
                                MessageBox.Show("Error de consulta. Prueba otra vez");
                            else
                            {
                                this.formConsultas = new RespuestaConsultas();
                                this.formConsultas.SetQuestion(ConQuienLbl.Text);
                                this.formConsultas.SetDataGrid(codigo, mensaje);
                                this.formConsultas.ShowDialog();
                            }
                                break;

                        case 4: //"4/0 o nombre1,idPartida,ganadorPartida*nombre2,idPartida,ganadorParida*..." (Resultados Partidas con jugadores)
                            if (mensaje == "-1")
                                MessageBox.Show("Error de consulta. Prueba otra vez");
                            else
                            {
                                this.formConsultas = new RespuestaConsultas();
                                this.formConsultas.SetQuestion(companyia.Text);
                                this.formConsultas.SetDataGrid(codigo, mensaje);
                                this.formConsultas.ShowDialog();
                            }
                            break;

                        case 5: //"5/nombreJugador/puntos o -1 o -2" (nombre del jugador con más puntos o Error de consulta o No hay jugadores en la BBDD)
                            if (mensaje == "-1")
                                MessageBox.Show("Error de consulta. Prueba otra vez");
                            else {
                                this.formConsultas = new RespuestaConsultas();
                                this.formConsultas.SetQuestion(jugMaxBtn.Text);
                                this.formConsultas.SetDataGrid(codigo, mensaje);
                                this.formConsultas.ShowDialog();
                            }
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
                            else//////
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
                            if (numTablero >= 0)
                            {
                                tableros[numTablero].FinalizarPartida();
                                tableros[numTablero].Close();
                            }
                            if (mensaje.Split('*')[1] != userName)
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

                        case 17: //Respuesta a borrar un jugador de la BBDD
                            if (mensaje == "0")
                            {
                                MessageBox.Show("Usuario eliminado con éxito");
                                eliminarBox.Visible = false;
                                accederBox.Visible = true;
                                regVisible.Visible = true;
                                regLabel.Visible = true;
                                volverLbl.Visible = false;
                                eliminarLbl.Visible = true;
                                eliminarCuenta.Visible = true;
                                usuarioEliminado.Clear();
                                contrasenyaEliminado.Clear();
                            }
                            else if (mensaje == "-1")
                                MessageBox.Show("Error al eliminar el usuario");
                            else if (mensaje == "1")
                            {
                                MessageBox.Show("El usuario que quiere eliminar no existe");
                                usuarioEliminado.Clear();
                                contrasenyaEliminado.Clear();
                            }
                            else if (mensaje == "2")
                            {
                                MessageBox.Show("Contraseña incorrecta");
                                contrasenyaEliminado.Clear();
                            }
                            else
                            {
                                MessageBox.Show("El usuario esta conectado.\n Para eliminar un usuario, éste debe estar desconectado");
                                usuarioEliminado.Clear();
                                contrasenyaEliminado.Clear();
                            } 
                            adios = true;
                            break;
                        case 18: //18/idPartida1,duraion1*idPartida2,duracion2*..." (retorna las partidas jugadas en la fecha y su duración)
                            if (mensaje == "-1")
                                MessageBox.Show("Error de consulta. Prueba otra vez");
                            else
                            {
                                this.formConsultas = new RespuestaConsultas();
                                this.formConsultas.SetQuestion(fechaBtn.Text);
                                this.formConsultas.SetDataGrid(codigo, mensaje);
                                this.formConsultas.ShowDialog();
                            }
                            
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
            registroBox.Visible = false;
            consultaBox.Visible = false;
            consultasButton.Visible = false;
            ConectadosGridView.Visible = false;
            labelConectados.Visible = false;
            invitarButton.Visible = false;
            conexion.Visible = false;
            invitadosGridView.Visible = false;
            label6.Visible = false;
            inicio.Visible = false;
            eliminarBox.Visible = false;
            inv_lbl.Visible = false;

            //Fondo
            candadoBox.Image = Image.FromFile(".\\candadoCerrado.jpg");
            candadoBox.SizeMode = PictureBoxSizeMode.StretchImage;

            //Fondo
            candadoEliminado.Image = Image.FromFile(".\\candadoCerrado.jpg");
            candadoEliminado.SizeMode = PictureBoxSizeMode.StretchImage;
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
                    Bitmap portada = new Bitmap(Application.StartupPath + @"\fondo1.png");
                    this.BackgroundImage = portada;
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                    consultaBox.Visible = false;
                    consultasButton.Visible = false;
                    accederBox.Visible = true;
                    registroBox.Visible = false;
                    ConectadosGridView.Visible = false;
                    labelConectados.Visible = false;
                    invitarButton.Text = "Invitar";
                    invitarButton.Visible = false;
                    regLabel.Visible = true;
                    regVisible.Visible = true;
                    eliminarLbl.Visible = true;
                    eliminarCuenta.Visible = true;
                    invitadosGridView.Visible = false;
                    label6.Visible = false;
                    inv_lbl.Visible = false;    


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
                if (ConQuienLbl.Checked) 
                {
                    //Construimos el mensaje y lo enviamos ( "3/miNombre" ) para saber con quien he jugado
                    string mensaje = "3/" + NameBox.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                }
                else if (companyia.Checked)
                {
                    //Construimos el mensaje y lo enviamos ( "4/x/y/z..." ) para saber qué partidas he jugado con ellos
                    string mensaje = "4/"+nombresBox.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else if (jugMaxBtn.Checked)
                {
                    //Construimos el mensaje y lo enviamos ( "5/" ) Queremos saber el jugador con mas puntos
                    string mensaje = "5/";
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else //Construimos el mensaje y lo enviamos ( "14/fecha dia/mes/año == 28/01/22" ) Partidas durante un día en concreto
                {
                    string fecha = dateTimePicker.Value.ToString().Split(' ')[0];
                    string[] fechas = fecha.Split('/');
                    int año = Convert.ToInt32(fechas[2]) - 2000;
                    string mensaje = "14/" + fechas[0] + "/" + fechas[1] + "/" + año.ToString();
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
            try
            {
                string invitado = ConectadosGridView.CurrentCell.Value.ToString();

                //Comprovamos que no somos nosotros mismos
                if (invitado == userName)
                {
                    MessageBox.Show("No te puedes autoinvitar");
                }
                else
                {
                    if (invitadosGridView.Visible == false)
                    {
                        invitados = new List<string>();
                        invitadosGridView.Visible = true;
                        invitarButton.Visible = true;
                        label6.Visible = true;
                    }

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
                        MessageBox.Show("El numero máximo de invitados es 3");
                }
                ConectadosGridView.SelectAll();


            }
            catch (NullReferenceException)
            {

            }   
            
        }

        private void CrearInvitadosGridView(List<string> invitados)
        {
            invitadosGridView.ColumnCount = 1;
            invitadosGridView.RowCount = invitados.Count;
            invitadosGridView.ColumnHeadersVisible = false;
            invitadosGridView.RowHeadersVisible = false;
            invitadosGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            invitadosGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            invitadosGridView.SelectAll();
            invitadosGridView.BackgroundColor = Color.White;

            int totalRowHeight = invitadosGridView.ColumnHeadersHeight;
            for (int i = 0; i < invitados.Count; i++)
            {
                invitadosGridView.Rows[i].Cells[0].Value = invitados[i];
                totalRowHeight += invitadosGridView.Rows[i].Height;
            }
            invitadosGridView.Height = totalRowHeight;
            invitadosGridView.Height = invitadosGridView.Height + 5;

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
            try
            {
                string eliminado = invitadosGridView.CurrentCell.Value.ToString();
                invitados.Remove(eliminado);
                if (invitados.Count == 0)
                {
                    invitadosGridView.Visible = false;
                    invitarButton.Visible = false;
                    label6.Visible = false;
                }
                else
                    CrearInvitadosGridView(invitados);
            }
            catch (NullReferenceException)
            {

            }
        }

        private void regVisible_Click(object sender, EventArgs e)
        {
            registroBox.Visible = true;
            regVisible.Visible = false;
            accederBox.Visible = false; 
            regLabel.Visible = false;
            inicio.Visible=true;
            eliminarLbl.Visible = false;
            eliminarCuenta.Visible = false;
        }

        private void inicio_Click(object sender, EventArgs e)
        {
            registroBox.Visible = false;
            inicio.Visible = false;
            regVisible.Visible = true;
            accederBox.Visible = true;
            regLabel.Visible = true;
            eliminarLbl.Visible = true;
            eliminarCuenta.Visible = true;
        }

        private void eliminarCuenta_Click(object sender, EventArgs e)
        {
            eliminarBox.Visible = true;
            eliminarLbl.Visible = false;
            eliminarCuenta.Visible = false;
            regVisible.Visible = false;
            accederBox.Visible = false;
            regLabel.Visible = false;
            volverLbl.Visible = true;
        }

        private void volverLbl_Click(object sender, EventArgs e)
        {
            eliminarBox.Visible = false;
            accederBox.Visible = true;
            regVisible.Visible = true;
            regLabel.Visible = true;
            volverLbl.Visible = false;
            eliminarLbl.Visible = true;
            eliminarCuenta.Visible = true;
        }

        private void eliminarBtn_Click(object sender, EventArgs e)
        {
            if (usuarioEliminado.Text == "0")
            {
                MessageBox.Show("Nombre de usuario invalido");
            }
            else
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

                    //Enviamos el mensaje de eliminar jugador de BBDD
                    string mensaje = "13/"+usuarioEliminado.Text+"/"+contrasenyaEliminado.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                catch (SocketException)
                {
                    MessageBox.Show("Servidor no disponible");
                }
            }
            
        }

        private void candadoEliminado_Click(object sender, EventArgs e)
        {
            //Desea poder ver la contraseña
            if (contrasenyaEliminado.UseSystemPasswordChar == true)
            {
                contrasenyaEliminado.UseSystemPasswordChar = false;
                candadoEliminado.Image = Image.FromFile(".\\candadoAbierto.jpg");
            }

            //Desea ocultar la contraseña
            else
            {
                contrasenyaEliminado.UseSystemPasswordChar = true;
                candadoEliminado.Image = Image.FromFile(".\\candadoCerrado.jpg");
            }
        }
               
    }
    
}
