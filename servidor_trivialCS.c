#include <stdio.h>
#include <string.h>
#include <stdlib.h> //Necesario para atof
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>
#include <ctype.h>
#include <mysql.h>
#include <pthread.h>

//Variables globales
	//Para la BBDD
MYSQL *conn;

//Esctructura necesaria para acceso autoexcluyente
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

	//Lista de conectados (formada por usuarios)
typedef struct{
	char nombre[25];
	int *socket;
}Usuario;

typedef struct{
	Usuario conectados[50];
	int num;
}ListaConectados;

typedef struct{
	int estado;
	int numInvitados;
	char host[25];
	char jug2[25];
	char jug3[25];
	char jug4[25];
}Partidas;


//Iniciamos la lista de conectados
ListaConectados listaC;
int len_tablaP=100;
Partidas tablaP[100];

//Para poder notificar a todos los clientes, necesitamos que el vector de sockets sea una variable global
int i;
int sockets[100];

//Funciones del servidor

//LogIn --> Codigo 1
int LogIn(char nombre[25], char contrasenya[20]){
	//Retorna: 0--> Todo OK ; 1 --> Usuario no existe ; 2 --> Contrasenya no coincide ; -1 --> Error de consulta ; 3 --> Usuario ya conectado
	
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	int err;
	
	char consulta[80];
	strcpy(consulta,"SELECT contrasenya FROM jugadores WHERE nombre='");
	strcat(consulta,nombre);
	strcat(consulta,"'");
	
	
	err=mysql_query(conn,consulta);
	if (err != 0){
		printf("Error al consultar la BBDD %u %s",mysql_errno(conn),mysql_error(conn));
		return -1;
	}
	else{
		//Recibimos el resultado de la consulta
		resultado = mysql_store_result(conn);
		row = mysql_fetch_row(resultado);
		if (row == NULL)
			return 1;
		else
		{
			printf("Contrasenya: %s\n",row[0]);
			if (strcmp(contrasenya,row[0])==0){
				int i = 0;
				int encontrado = 0;
				while (i<listaC.num && encontrado == 0){
					if (strcmp(listaC.conectados[i].nombre,nombre)==0)
						encontrado = 1;
					else
						i=i+1;
				}
				if (encontrado==1)
					return 3;
				else
					return 0;
			}
			else
				return 2;
			
		}
		
	}
}

//Registro --> Codigo 2
int Registro(char nombre[25], char contrasenya[20], char mail[100]){
	//Retorna 0--> Todo OK ; 1 --> Usuario ya existente ; -1 --> Error de consulta
	
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	int err;
	
	//Busca si ya hay un usuario con ese nombre
	char consulta[80];
	strcpy(consulta, "SELECT nombre FROM jugadores WHERE nombre='");
	strcat(consulta, nombre);
	strcat(consulta, "'");
	
	err = mysql_query(conn, consulta);
	if (err!=0)
		return -1;
	
	else {
		resultado = mysql_store_result(conn);
		row =  mysql_fetch_row(resultado);
		
		//Se registra si no hay nadie con el mismo nombre
		if (row==NULL) {
			
			err=mysql_query(conn, "SELECT * FROM jugadores WHERE id=(SELECT max(id) FROM jugadores);");
			if (err!=0){
				return -1;
			}
			resultado = mysql_store_result(conn);
			row =  mysql_fetch_row(resultado);
			printf("%s\n",row[0]);
			int id = atoi(row[0])+1;
			sprintf(consulta,"INSERT INTO jugadores VALUES ('%d','%s','%s','%s');", id, nombre, contrasenya, mail);
			printf("%s\n",consulta);
			err= mysql_query(conn, consulta);
			if (err!=0)
				return -1;						
			else
				return 0; 
		}
		else
			return 1;  
		
	}
}

//Recuperar contrasenya --> Codigo 3
int RecuperarContrasenya(char nombre[25], char contrasenya[20]){
	//Retorna 0 --> Todo OK (+ la contrasenya en la variable contrasenya); 1--> No hay usuario (+ '\0' en la variable contrasenya); -1 --> Error de consulta (+ '\0' en la variable contrasenya)
	
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	int err;
	
	char consulta[80];
	strcpy(consulta,"SELECT contrasenya FROM jugadores WHERE nombre='");
	strcat(consulta,nombre);
	strcat(consulta,"'");
	
	err=mysql_query(conn,consulta);
	if (err!=0)
		return -1;
	else{
		resultado = mysql_store_result(conn);
		row = mysql_fetch_row(resultado);
		
		if (row == NULL){
			strcpy(contrasenya,"\0");
			return 1;
		}
		else{
			strcpy(contrasenya,row[0]);
			return 0;
		}
	}
}

//Partida mas larga --> Codigo 4
int DamePartidaLarga(){
	//Retorna idP partida mas larga --> Todo OK ; -1 --> Error de consulta ; -2 --> No hay partidas en BBDD
	
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	int err;
	
	err=mysql_query (conn, "SELECT partidas.id FROM partidas WHERE partidas.duracion = (SELECT MAX(partidas.duracion) FROM partidas)");
	if (err!=0)
		return -1;
	else{
		resultado = mysql_store_result(conn);
		row = mysql_fetch_row(resultado);
		
		if (row == NULL)
			return -2;
		else{
			int idP = atoi(row[0]);
			return idP; 
		}
	}
}

//Jugador con mas puntos --> Codigo 5
int DameJugadorMasPuntos(char nombre[25]){
	//Retorna 0 --> Todo OK (+ nombre del jugador con mas puntos en variable nombre) ; -1 --> Error de consulta (+ '\0' en la variable nombre); -2 --> No hay jugadores en BBDD (+ '\0' en la variable nombre)
	
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	int err;
	
	err=mysql_query (conn, "SELECT jugadores.nombre FROM (jugadores, registro) WHERE registro.puntos=(SELECT MAX(registro.puntos) FROM registro) AND registro.idJ=jugadores.id");
	if (err!=0){
		strcpy(nombre,"\0");
		return -1;
	}
	else{
		resultado = mysql_store_result(conn);
		row = mysql_fetch_row(resultado);
		
		if (row == NULL){
			strcpy(nombre,"\0");
			return -2;
		}
		else{
			strcpy(nombre,row[0]); 
			return 0;
		}
	}
	
}
//Dame lista conectados --> Codigo 6
int DameListaConectados(char lista[512]){
	//Retorna 0 --> Todo OK(+ char con la lista de conectados : user1/user2/user3/ etc) ; -1 --> Lista vacia (+ lista = '\0')
	
	strcpy(lista,"\0");
	if (listaC.num != 0){
		int i;
		for (i=0;i<listaC.num;i++)
			sprintf(lista,"%s%s*",lista,listaC.conectados[i].nombre);
		lista[strlen(lista)-1]='\0';
		
		return 0;
	}
	else
		return -1;
	
}
//Añadir a la lista de conectados
void AnadirAListaConectados (char nombre[25],int *socket){
	
	if (nombre != NULL && socket != NULL){
		//Creamos un nuevo usuario que añadir a la lista
		Usuario nuevoUsuario;
		strcpy(nuevoUsuario.nombre,nombre);
		nuevoUsuario.socket = socket;
		
		//Lo añadimos
		listaC.conectados[listaC.num]=nuevoUsuario;
		listaC.num = listaC.num+1;
		printf("Usuario %s añadido", nombre);
	}	
}
//Retirar de la lista de conectados
void RetirarDeListaConectados (char nombre[25]) {
	
	if (nombre != NULL){
		int n = 0;
		int encontrado = 0;
		
		while(n<listaC.num && encontrado==0){
			if (strcmp(listaC.conectados[n].nombre,nombre)==0){
				encontrado = 1;
			}
			else
				n++;
		}
		if (encontrado==1){
			while(n<listaC.num){
				listaC.conectados[n]=listaC.conectados[n+1];
				n++;
			}
			listaC.num = listaC.num-1;
		}
	}
}
//Notificar actualizacion de la lista de conectados
void NotificarNuevaListaConectados(){
	
	char lista[512];
	char notificacion[512];
	
	//pthread_mutex_lock(&mutex);
	int res = DameListaConectados(lista);
	//pthread_mutex_unlock(&mutex);
	
	printf("Notificacion de actualizacion de ListaConectados\n");
	if (res == 0){
		printf("Lista de conectados con nuevos datos: %s\n",lista);
		sprintf(notificacion,"6/%s",lista);
	}
	else{
		printf("Lista de conectados vacia\n");
		sprintf(notificacion,"6/%s",lista);
	}
	
	//Enviamos la actualizacion generada a todos los socket
	int j;
	for (j=0;j<listaC.num;j++){
		write(listaC.conectados[j].socket,notificacion,strlen(notificacion));
	}
	
}
//Retorna el socket del nombre buscado en la lista de conectados.
int DameSocketConectado(char nombre[25]){
	//Retorna -1 --> El usuario buscado no se ha encontrado conectado.
	//Retorna número socket--> Se ha encontrado el resultado conectado.
	int i=0;
	int encontrado=0;
	while (i<listaC.num && !encontrado){
		if (strcmp(listaC.conectados[i].nombre,nombre)==0)
			encontrado=1;
		else
			i++;
	}
	if (encontrado==1)
	{
		return listaC.conectados[i].socket;
	}
	else 
		return -1;
}
//Invitar a los conectados seleccionados a una partida
int Invitar(char invitados[500], char nombre[25], char noDisponibles[500],int partida) {
	//invitados; nombre: quien invita
	//Retorna numero de invitados --> Todo OK (+ notifica a los invitados (8/persona_que_le_ha_invitado*id_partida))
	//       -1 --> Alguno de los usuarios invitados se ha desconectado (+nombres de los desconectados en noDisponibles)
	
	strcpy(noDisponibles,"\0");
	int error = 0;
	char *p = strtok(invitados,"*");
	int numInvitados = 0;
	
	while (p != NULL) {
		int encontrado = 0;
		int i = 0;
		while ((i<listaC.num)&&(encontrado == 0)) {
			if (strcmp(listaC.conectados[i].nombre,p) == 0) {
				char invitacion[512];
				sprintf(invitacion, "8/%s*%d*", nombre,partida);
				//Invitacion: 8/quien invita*id_partida
				printf("Invitacion: %s\n",invitacion);
				write(listaC.conectados[i].socket, invitacion, strlen(invitacion));
				encontrado = 1;
				numInvitados = numInvitados +1;
			}
			else
				i = i + 1;
		}
		if (encontrado == 0){
			error = -1;
			sprintf(noDisponibles,"%s%s*",noDisponibles,p);
			noDisponibles[strlen(noDisponibles)-1] = '\0';
		}
		p = strtok(NULL, "*");		
	}
	if(error == 0){
		error = numInvitados;
		pthread_mutex_lock(&mutex);
		tablaP[partida].numInvitados = numInvitados;
		pthread_mutex_unlock(&mutex);
	}
	
	return error;
}
//Busca una partida disponible en la tabla de partidas y le asocia el  host
int CrearPartida(char nombre[25]){
//int CrearPartida(char invitados[500], char nombre[25]){
	//Retorna -1 --> si no hay ningún espacio libre en la tabla de partidas.
	//Retorna id de la partida-->  si se ha asignado.
	int i=0;
	int encontrado=0;
	while((i<len_tablaP)&&!encontrado)
	{	
		pthread_mutex_lock(&mutex);
		if(tablaP[i].estado==0)
		{
			strcpy(tablaP[i].host,nombre);
			tablaP[i].estado=1;
			printf("Estado partida %d: %d\n",i, tablaP[i].estado);
			strcpy(tablaP[i].jug2,"0");
			strcpy(tablaP[i].jug3,"0");
			strcpy(tablaP[i].jug4,"0");
			encontrado=1;
		}
		else 
		   i++;
		pthread_mutex_unlock(&mutex);
		
	}
	if(encontrado==0)
		return -1;
	else{
		return i;
	}
}
//Añade un jugador a la partida indicada
int AnadirJugador(char nombre[25],int partida){
	//Retorna numero de jugadores que faltan para aceptar
	//Cliente ya no permite que se inviten a mas de 3 personas
	printf("Añadir jugador: %s\n",nombre);
	pthread_mutex_lock(&mutex);
	if(strcmp(tablaP[partida].jug2,"0")==0){
		strcpy(tablaP[partida].jug2,nombre);
	}
	else if (strcmp(tablaP[partida].jug3,"0")==0)
		strcpy(tablaP[partida].jug3,nombre);
	else if(strcmp(tablaP[partida].jug4,"0")==0)
		strcpy(tablaP[partida].jug4,nombre);

	tablaP[partida].numInvitados = tablaP[partida].numInvitados-1;
	int res = tablaP[partida].numInvitados;
	pthread_mutex_unlock(&mutex);
	return res;	
}

void IniciarPartida (int partida, char jugadores_partida[500]){
	char ini[20];
	sprintf(ini, "9/%d*%s", partida, jugadores_partida);
	printf("Iniciar partida: %s\n",ini);
	int socket1 = DameSocketConectado(tablaP[partida].host);
	if(socket1!=-1)
		write(socket1,ini,strlen(ini));
	int socket2 = DameSocketConectado(tablaP[partida].jug2);
	if(socket2!=-1)
		write(socket2,ini,strlen(ini));
	
	if (strcmp(tablaP[partida].jug3,"0")!=0){
		int socket3=DameSocketConectado(tablaP[partida].jug3);
		if(socket3!=-1)
			write(socket3,ini,strlen(ini));
	}
	if (strcmp(tablaP[partida].jug4,"0")!=0){
		int socket4=DameSocketConectado(tablaP[partida].jug4);
		if(socket4!=-1)
			write(socket4,ini,strlen(ini));
	}	
}
//Notifica a los jugadores de la tabla que ha acabado la partida.
void FinPartida(int partida){
	char fin[20];
	sprintf(fin,"10/%d", partida);
	int socket1=DameSocketConectado(tablaP[partida].host);
	if(socket1!=-1){
		write(socket1,fin,strlen(fin));
		printf("notificación: %s enviada a socket:%d \n",fin,socket1);
	}
	int socket2=DameSocketConectado(tablaP[partida].jug2);
	if (socket2!=-1){
		write(socket2,fin,strlen(fin));
		printf("notificación: %s enviada a socket:%d \n",fin,socket2);
	}
	
	if (strcmp(tablaP[partida].jug3,"0")!=0){
		int socket3=DameSocketConectado(tablaP[partida].jug3);
		if(socket3!=-1)
		{
			write(socket3,fin,strlen(fin));
			printf("notificación: %s enviada a socket:%d \n",fin,socket3);
		}	
	}
	if (strcmp(tablaP[partida].jug4,"0")!=0){
		int socket4=DameSocketConectado(tablaP[partida].jug4);
		if(socket4!=-1){
			write(socket4,fin,strlen(fin));
			printf("notificación: %s enviada a socket:%d \n",fin,socket4);
			
		}
	}	
}
//Eliminia una partida de la tabla de partidas.
void EliminarPartida(int partida){
	tablaP[partida].estado=0;
	tablaP[partida].numInvitados=-1;
	strcpy(tablaP[partida].host,"0");
	strcpy(tablaP[partida].jug2,"0");
	strcpy(tablaP[partida].jug3,"0");
	strcpy(tablaP[partida].jug4,"0");
}
//Retorna los jugadores de una partida recibida como parametro
void DameJugadoresPartida(int partida, char jugadores[500]){
	sprintf(jugadores,"%s*%s",tablaP[partida].host,tablaP[partida].jug2);
	
	if (strcmp(tablaP[partida].jug3,"0")!=0){
		sprintf(jugadores,"%s*%s",jugadores,tablaP[partida].jug3);
		if(strcmp(tablaP[partida].jug4,"0")!=0)
			sprintf(jugadores,"%s*%s",jugadores,tablaP[partida].jug4);
	}
}
//Retorna los id de las partidas en las que participa el jugador recibido como parámetro
void DamePartidasJugador(char nombre[25], char partidas[100]){
	int i=0;
	strcpy(partidas,"\0");
	while(i<len_tablaP)
	{
		if((strcmp(nombre,tablaP[i].host)==0)||(strcmp(nombre,tablaP[i].jug2)==0)||(strcmp(nombre,tablaP[i].jug3)==0)||(strcmp(nombre,tablaP[i].jug4)==0))
			sprintf(partidas,"%s%d/",partidas,i);
		i=i+1;
	}
}

//Atencion a los diferentes clientes (threads)
int *AtenderCliente(void *socket){
	
	char buff[512];
	char buff2[512];
	
	int ret;
	int sock_conn;
	int *s;
	s = (int *) socket;
	sock_conn = *s;
	
	//Variable para saber si se tiene que desconectar
	int terminar = 0;
	while (terminar==0)
	{
		// Informacion recibida almacenada en buff
		ret=read(sock_conn,buff, sizeof(buff));
		printf ("Recibido\n");
		
		printf("%s\n",buff);
		
		// Tenemos que añadirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		buff[ret]='\0';
		
		//Escribimos en consola lo que nos ha llegado (buff)
		printf("Se ha conectado: %s\n",buff);
		
		//Datos de este cliente
		char nombre[25];
		char contrasenya[20];
		char mail[100];
		
		//Obtenemos el codigo que nos indica el tipo de petición.
		char *p = strtok(buff,"/");
		int codigo = atoi(p);
		printf("Codigo: %d\n",codigo);
		
		
		//Codigo 0 --> Desconexión
		if (codigo == 0){
			//Mensaje en buff: 0/
			//Return en buff2: -
			terminar = 1;
			
			pthread_mutex_lock(&mutex);
			RetirarDeListaConectados(nombre);
			NotificarNuevaListaConectados();
			char partidas[100];
			DamePartidasJugador(nombre,partidas);
			char *p=strtok(partidas,"/");
			while(p!=NULL)
			{	
				int partida=atoi(p);
				FinPartida(partida);
				printf("Notificación de fin de partida %d\n",partida);
				EliminarPartida(partida);
				p=strtok(NULL,"/");
			}
						
			pthread_mutex_unlock(&mutex);
			
			
		}
		else {
			//Codigo 1 --> Comprovación para el Login
			if (codigo == 1){
				//Mesnaje en buff: 1/username/contrasenya
				//Return en buff2: 0--> Todo OK ; 1 --> Usuario no existe ; 2 --> Contrasenya no coincide ; -1 --> Error de consulta
				
				p = strtok(NULL,"/");
				strcpy(nombre,p);
				p = strtok(NULL,"/");
				strcpy(contrasenya,p);
				
				int res = LogIn(nombre,contrasenya);
				
				sprintf(buff2,"1/%d", res);
				
				//Añadimos a la lista de conectados si la comprovacion ha sido correcta
				if (res == 0){
					pthread_mutex_lock(&mutex);  //Autoexclusion
					AnadirAListaConectados(nombre,sock_conn);
					NotificarNuevaListaConectados();
					pthread_mutex_unlock(&mutex);
					
				}
				
			}
			
			//Codigo 2 --> Insert de nuevos jugadores
			else if (codigo==2){ 
				//Mensaje en buff: 2/username/contrasenya/mail
				//Return en buff2: 0--> Todo OK ; 1 --> Usuario ya existente ; -1 --> Error de consulta
				
				p = strtok(NULL, "/");
				strcpy(nombre, p);
				p = strtok (NULL, "/");
				strcpy (contrasenya, p);
				p = strtok(NULL,"/");
				strcpy(mail, p);
				
				int res = Registro(nombre,contrasenya,mail);
				sprintf(buff2,"2/%d",res);
				
			}
			
			
			//Codigo 3 --> Recuperar contraseña 
			else if (codigo == 3){
				//Mensaje en buff: 3/usuario
				//Return en buff2: contrasenya --> Todo OK ; 1--> No hay usuario ; -1 --> Error de consulta
				
				p = strtok(NULL,"/");
				strcpy(nombre,p);
				
				int res = RecuperarContrasenya(nombre,contrasenya);
				if (res ==0)
					sprintf(buff2,"3/%s",contrasenya);
				else
					sprintf(buff2,"3/%d",res);
				
			}
			
			//Codigo 4 --> Obtener la partida mas larga 
			else if (codigo == 4){
				//Mensaje en buff: 4/
				//Return en buff2: idP partida mas larga --> Todo OK ; -1 --> Error de consulta ; -2 --> No hay partidas en BBDD
				
				int res = DamePartidaLarga();
				sprintf(buff2,"4/%d",res);
				
			}
			
			//Codigo 5 --> Obtener jugador con más puntos
			else if (codigo==5){
				//Mensaje en buff: 5/
				//Return en buff 2: nombre jugador con mas puntos --> Todo OK ; -1 --> Error de consulta ; -2 --> No hay jugadores en BBDD
				
				int res = DameJugadorMasPuntos(nombre);
				if (res == 0)
					sprintf(buff2,"5/%s",nombre);
				else
					sprintf(buff2,"5/%d",res);
				
			}
			
			//Codigo 6 --> Invitar a otros usuarios conectados a una partida
			else if (codigo ==  6) {
				//Mensaje en buff: 6/invitado1*invitado2*...
				//Return en buff2: 7/0 (Todo OK) ; 7/invitado_no_disponible1/... (si hay invitados que se han desconectado)
				
				p = strtok(NULL, "/");
				char invitados[500];
				printf("Invitados: %s\n", invitados);
				char noDisponibles[500];
				strcpy(invitados, p);
								
				int partida=CrearPartida(nombre);
				if (partida==-1)
					sprintf(buff2,"7/-1");
				else
					{
					int res = Invitar(invitados, nombre, noDisponibles,partida);
					printf("Resultado de invitar: %d\n",res);
					
					if (res == -1){
						sprintf(buff2,"7/%s",noDisponibles);
					}
					else{
						strcpy(buff2,"7/0");						 
					}
				}
				
			}
			//Codigo 7 --> Respuesta a una invitacion de partida
			else if (codigo ==  7) {
				//Mensaje en buff: 7/respuesta(SI/NO)/id_partida
				//Mensaje en buff2: -
				
				p = strtok(NULL,"/");
				char respuesta[3];
				strcpy(respuesta,p);
				printf("%s\n",respuesta);
				p = strtok(NULL,"/");
				printf("Id partida: %s\n",p);
				int id_partida;
				id_partida = atoi(p);
				if (strcmp(respuesta,"NO")==0){
					FinPartida(id_partida);
					EliminarPartida(id_partida);
				}
				else{
					int res = AnadirJugador(nombre,id_partida);
					printf("Añadido a la partida %s\n",nombre);
					printf("%d\n",res);
					if (res == 0){
						printf("Iniciar partida: \n");
						char jugadores_partida[500];
						DameJugadoresPartida(id_partida,jugadores_partida);
						IniciarPartida(id_partida,jugadores_partida);
					}
				}
				
			}
			
			// Y lo enviamos
			if (codigo!=0){
				write (sock_conn,buff2, strlen(buff2));
				printf("Codigo: %d , Resultado: %s\n",codigo,buff2);//Vemos el resultado de la accion.
			}
		}
		
	}
	
	// Se acabo el servicio para este cliente
	close(sock_conn);
	pthread_exit(0);
}


//Programa principal del servidor

int main(int argc, char *argv[]) {
	
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;
	
	// INICIALITZACIONES
	//Ponemos a 0 el numeros de conectados en la lista de conectados
	listaC.num=0;
	
	// Abrimos el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket\n");
	// Hacemos el bind al puerto
	
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicializa a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la maquina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 50051
	serv_adr.sin_port = htons(9080);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind\n");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 2) < 0)
		printf("Error en el Listen\n");
	
	//Creamos la connexión a la BBDD
	conn = mysql_init(NULL);
	if (conn == NULL){
		printf("Error al crear la connexión: %u %s\n",mysql_errno(conn),mysql_error(conn));
		exit(1);
	}
	
	conn = mysql_real_connect(conn,"localhost","root","mysql","T1_BBDD",0,NULL,0);
	if (conn==NULL){
		printf("Error al crear la connexión: %u %s\n",mysql_errno(conn),mysql_error(conn));
		exit(1);
	}
	
	
	pthread_t thread[100];
	pthread_mutex_init(&mutex,NULL);
	
	// Bucle infinito
	for(i=0;;i++){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexi?n\n");
		//sock_conn --> socket para este cliente
		sockets[i] = sock_conn;
		
		//Creamos el thread
		pthread_create (&thread[i], NULL, AtenderCliente, &sockets[i]);
	}
	pthread_mutex_destroy(&mutex);
	exit(0);
}

