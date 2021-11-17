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

	//Iniciamos la lista de conectados
ListaConectados listaC;

	//Para poder notificar a todos los clientes, necesitamos que el vector de sockets sea una variable global
int i;
int sockets[100];


//Funciones del servidor

//LogIn --> Codigo 1
int LogIn(char nombre[25], char contrasenya[20]){
	//Retorna: 0--> Todo OK ; 1 --> Usuario no existe ; 2 --> Contrasenya no coincide ; -1 --> Error de consulta
	
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
			printf("%s\n",row[0]);
			if (strcmp(contrasenya,row[0])==0)
				return 0;
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
		printf("Lista de conectados con nuevos datos\n");
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
		printf("%d\n",codigo);
		
		
		//Codigo 0 --> Desconexión
		if (codigo == 0){
			//Mensaje en buff: 0/
			//Return en buff2: -
			terminar = 1;
			
			pthread_mutex_lock(&mutex);
			RetirarDeListaConectados(nombre);
			NotificarNuevaListaConectados();
			
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
	serv_adr.sin_port = htons(50051);
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
	
	conn = mysql_real_connect(conn,"shiva2.upc.es","root","mysql","T1_BBDD",0,NULL,0);
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

