CREATE DATABASE DB_Pray;

use DB_Pray

if OBJECT_ID('Generos') is not null
      drop table DB_Pray.dbo.Generos;

CREATE TABLE DB_Pray.dbo.Generos(
	idGenero	SMALLINT NOT NULL IDENTITY(1, 1),
	genero		VARCHAR(15) NOT NULL,
	CONSTRAINT PK_Gen
		PRIMARY KEY(idGenero)
);

--insert generos 
insert into DB_Pray.dbo.Generos(genero)
     values('Masculino'),
	       ('Femenino');

		   select *from Generos;
		   ------------fin

if OBJECT_ID('Estatus') is not null
      drop table Estatus;

CREATE TABLE DB_Pray.dbo.Estatus(
	idEstatus	SMALLINT NOT NULL IDENTITY(1, 1),
	estatus		VARCHAR(15) NOT NULL,
	CONSTRAINT PK_Est
		PRIMARY KEY(idEstatus)
);

insert into DB_Pray.dbo.Estatus(estatus)
          values('Activo'),
		         ('Inhabilitado'),
				 ('Eliminado')

				 select *from Estatus;

if OBJECT_ID('Usuarios') is not null
      drop table Usuarios;

CREATE TABLE DB_Pray.dbo.Usuarios(
	email		VARCHAR(40) NOT NULL,
	nombre		VARCHAR(20),
	apellido1	VARCHAR(20),
	apellido2	VARCHAR(20),
	fechaNac	DATE,
	idGenero	SMALLINT,
	IdEstatus	SMALLINT,
	CONSTRAINT PK_User
		PRIMARY KEY(email),
	CONSTRAINT FK_User_Gen
		FOREIGN KEY(idGenero)
			REFERENCES DB_Pray.dbo.Generos (idGenero),
	CONSTRAINT FK_User_Est
		FOREIGN KEY(IdEstatus)
			REFERENCES DB_Pray.dbo.Estatus(idEstatus)
);


if OBJECT_ID('ContrasenasdeUsuarios') is not null
      drop table ContrasenasdeUsuarios;

CREATE TABLE DB_Pray.dbo.ContrasenasdeUsuarios(
	idContra	SMALLINT NOT NULL IDENTITY(1, 1),
	email		VARCHAR(40),
	tipo		VARCHAR(15),
	contrasena	VARCHAR(40),
	CONSTRAINT PK_Contra
		PRIMARY KEY(idContra),
	CONSTRAINT FK_Contra_User
		FOREIGN KEY(email)
			REFERENCES DB_Pray.dbo.Usuarios(email)
);

if OBJECT_ID('FechasGestionUsuarios') is not null
      drop table FechasGestionUsuarios;

CREATE TABLE DB_Pray.dbo.FechasGestionUsuarios(
	idFecha		SMALLINT NOT NULL IDENTITY(1, 1),
	email		VARCHAR(40),
	tipo		VARCHAR(15),
	fecha		DATETIME,
	CONSTRAINT PK_FechasGes
		PRIMARY KEY(idFecha),
	CONSTRAINT FK_FechasGes_User
		FOREIGN KEY(email)
			REFERENCES DB_Pray.dbo.Usuarios(email)
);

if OBJECT_ID('Favoritos') is not null
      drop table Favoritos;

CREATE TABLE DB_Pray.dbo.Favoritos(
	idFav           SMALLINT NOT NULL IDENTITY(1, 1),
	Id_Idioma		SMALLINT NOT NULL,
	Id_Testamento	SMALLINT NOT NULL,
    Id_Version		SMALLINT NOT NULL,
	Id_Libro		SMALLINT NOT NULL,
	NumeroCap		TINYINT NOT NULL,
	NumeroVers		TINYINT NOT NULL,
	emailUsuario    VARCHAR(40),
    CONSTRAINT PK_Favoritos
        PRIMARY KEY(idFav),
    CONSTRAINT FK_Favoritos_User
        FOREIGN KEY(emailUsuario)
            REFERENCES DB_Pray.dbo.Usuarios(email)
);


if OBJECT_ID('Busquedas') is not null
      drop table Busquedas;

CREATE TABLE DB_Pray.dbo.Busquedas(
	idBusqueda		SMALLINT NOT NULL IDENTITY(1, 1),
	palabras		VARCHAR(30) NOT NULL,
	idioma			SMALLINT NOT NULL,
	_version		SMALLINT NOT NULL,
	testamento		SMALLINT,
	libro			SMALLINT,
	fechaBusqueda	DATETIME NOT NULL,
	HuboResultados	BIT NOT NULL,
	emailUsuario	VARCHAR(40),
	CONSTRAINT PK_Busquedas
		PRIMARY KEY(idBusqueda),
	CONSTRAINT FK_Busquedas_User
		FOREIGN KEY(emailUsuario)
			REFERENCES DB_Pray.dbo.Usuarios(email)
);


