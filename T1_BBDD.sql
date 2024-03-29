DROP DATABASE IF EXISTS T1_BBDD;
CREATE DATABASE T1_BBDD;
USE T1_BBDD;

CREATE TABLE jugadores (
	id INT not NULL, 
	PRIMARY KEY(id),
	nombre VARCHAR(25),
	contrasenya VARCHAR(20),
	mail VARCHAR(100)
)ENGINE = InnoDB;

CREATE TABLE partidas (
	id INT not NULL,
	PRIMARY KEY(id),
	fecha VARCHAR(10),
	ganador VARCHAR(25) DEFAULT '-',
	duracion INT not NULL
)ENGINE = InnoDB;

CREATE TABLE registro (
	idJ INT,
	FOREIGN KEY(idJ) REFERENCES jugadores(id) ON DELETE CASCADE,
	idP INT,
	FOREIGN KEY(idP) REFERENCES partidas(id) ON DELETE CASCADE,
	puntos INT not NULL
)ENGINE = InnoDB;

INSERT INTO jugadores VALUES (1, 'Miguel', 'mimara','miguel.valero@upc.edu');
INSERT INTO jugadores VALUES (2, 'Laia', 'telecos','laia.costa.cid@estudiantat.upc.edu');
INSERT INTO jugadores VALUES (3, 'Maria', 'telematica','maria.ubiergo@estudiantat.upc.edu');
INSERT INTO jugadores VALUES (4, 'Biel', 'aeros','biel.gomez@estudiantat.upc.edu');

INSERT INTO partidas VALUES (1, '28/09/2021', 'Miguel',900);
INSERT INTO partidas VALUES (2, '28/09/2021', 'Laia', 450);
INSERT INTO partidas VALUES (3, '29/09/2021', 'Biel',500);
INSERT INTO partidas VALUES (4, '29/09/2021', 'Biel',350);
INSERT INTO partidas VALUES (5, '29/09/2021', 'Maria',300);

INSERT INTO registro VALUES (1, 1, 10);
INSERT INTO registro VALUES (2, 1, 8);
INSERT INTO registro VALUES (2, 2, 7);
INSERT INTO registro VALUES (3, 2, 5);
INSERT INTO registro VALUES (4, 3, 11);
INSERT INTO registro VALUES (2, 3, 9);
INSERT INTO registro VALUES (4, 4, 8);
INSERT INTO registro VALUES (1, 4, 7);
INSERT INTO registro VALUES (3, 5, 6);
INSERT INTO registro VALUES (2, 5, 5);
