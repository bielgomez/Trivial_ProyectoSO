#include <stdio.h>

typedef struct{
	int id;
	int dado;
	char posibles[100];
}Casilla;

typedef struct{
	Casilla casillas[500];
	int num;
}ListaCasillas;


void AnadirAListaCasillas(ListaCasillas *lista, int idCasilla, int dado, char movimientos[200]){
	Casilla nuevaCasilla;
	nuevaCasilla.id = idCasilla;
	nuevaCasilla.dado = dado;
	sprintf(nuevaCasilla.posibles,"%s",movimientos);
	lista->casillas[lista->num] = nuevaCasilla;
	lista->num = (lista->num)+1;
	printf("He añadido la casilla %d tirada %d\n",idCasilla,dado);
	printf("Lista num: %d\n", lista->num);
}

int main(int argc, char *argv[]) {
	//Programa para calcular los possibles movimientos
	//Caselles de la rodona -> 0-41 (cada tres 1 te possibilitat de pujar)
	//Caselles que pugen -> 100-104,110-114,120-124,130-134,140-144,150-154 (grups de 5)
	//Casella central --> 1000

	ListaCasillas miLista;
	miLista.num = 0;
	
	printf("Comenzamos\n");
	int casilla = 0;
		
	//Calculamos el circulo (casella de 0 a 41)
	while(casilla<42){
		//Dau		
		int dado = 1;
		while (dado <= 6){
			//pos1 i pos2 son els moviments sense sortir del cercle
			int pos1 = casilla+dado;
			int pos2 = casilla-dado;
			int pos3;
			int pos4;
			
			char movimientos[200];
			
			//Correccions
			if (pos1>41)
				pos1 = pos1-42;
			if (pos2<0)
				pos2=42+pos2;
			
			//pos3 i pos4 son els moviments que inclouen la branca central
			//Caselles tipus 0
			if((casilla%7)==0){//Caselles on hi ha pujada
				if (dado==6)
					pos3 = 1000; 
				else
					pos3 = 100+(casilla/7)*10+dado;
				
				//Ho posem a la llista
				sprintf(movimientos,"%d/%d/%d",pos1,pos2,pos3);
				AnadirAListaCasillas(&miLista,casilla,dado,movimientos);
			}
			//Caselles tipus 1,2,3
			else if (((casilla%7)==1) || ((casilla%7)==2) || ((casilla%7)==3)){
				if (dado>(casilla%7)){
					int n = dado-(casilla%7);
					int m = (casilla-(casilla%7))%7;
					pos3 = 100+m*10+n;
					
					if ((((casilla%7)==2) || ((casilla%7)==3)) && (dado>(7-(casilla%7)))){
						int n = dado-(7-(casilla%7));
						int m = (casilla+(7-(casilla%7)))%7;
						pos4 = 100+m*10+n;
						sprintf(movimientos,"%d/%d/%d/%d",pos1,pos2,pos3,pos4);
						
					}
					else
						sprintf(movimientos,"%d/%d/%d",pos1,pos2,pos3);
					
					AnadirAListaCasillas(&miLista,casilla,dado,movimientos);
				}
				else{
					sprintf(movimientos,"%d/%d",pos1,pos2);
					AnadirAListaCasillas(&miLista,casilla,dado,movimientos);
				}
	
			}
			//Caselles tipus 4,5,6
			else{
				if (dado>(7-(casilla%7))){
					int n = dado-(casilla%7);
					int m = (casilla+(casilla%7))%7;
					pos3 = 100+m*10+n;
					
					if ((((casilla%7)==4) || ((casilla%7)==5)) && (dado>(7-(casilla%7)))){
						int n = dado-(7-(casilla%7));
						int m = (casilla-(7-(casilla%7)))%7;
						pos4 = 100+m*10+n;
						sprintf(movimientos,"%d/%d/%d/%d",pos1,pos2,pos3,pos4);
					}
					else{
						sprintf(movimientos,"%d/%d/%d",pos1,pos2,pos3);
					}
					
					AnadirAListaCasillas(&miLista,casilla,dado,movimientos);
				}
				else{
					sprintf(movimientos,"%d/%d",pos1,pos2);
					AnadirAListaCasillas(&miLista,casilla,dado,movimientos);
				}
			}
			
			dado=dado+1;
		}
		casilla=casilla+1;
	}
	
	//Calculem les branques cap a dins
	casilla = 100;
	while (casilla<155){
		int dado = 1;
		while(dado<=6){
			int pos1;
			int pos2;
			int pos3;
			int pos4;
			int pos5;
			int pos6;
			int pos7;
			
			
			dado=dao+1;
		}
		casilla=casilla+1;
	}
	
	int i;
	printf("num: %d\n",miLista.num);
	for(i=0;i<miLista.num;i++){
		printf("-Nueva Combinacion-----------------\n");
		printf("Id: %d\n",miLista.casillas[i].id);
		printf("Tirada: %d\n",miLista.casillas[i].dado);
		printf("Possibles movimientos: %s\n",miLista.casillas[i].posibles);
		printf("-----------------------------------\n");
	}
	printf("Acabamos\n");
	
	return 0;
}

