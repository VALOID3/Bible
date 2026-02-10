use DB_Pray;
go

create procedure spGestionUsuarios(
	@Opc		CHAR(1),
	@email		VARCHAR(40),
	@nombre		VARCHAR(20) = NULL,
	@apellido1	VARCHAR(20) = NULL,
	@apellido2	VARCHAR(20) = NULL,
	@fechaNac	DATE = NULL,
	@idGenero	SMALLINT = NULL,
	@contrasena VARCHAR(40) = NULL,
	@estatusNuevo	VARCHAR(15) = NULL
)
as
begin
	declare @hoy DATETIME;
	set		@hoy = GETDATE();

	--INSERTAR USUARIO
	IF @Opc = 'I'
	begin
		--el estatus con id 1 significa "Activo"
		insert into Usuarios(email, nombre, apellido1, apellido2, fechaNac, idGenero, IdEstatus)
					values(@email, @nombre,@apellido1, @apellido2, @fechaNac, @idGenero, 1);
	
		insert into FechasGestionUsuarios(email, tipo, fecha)
					values(@email, 'Registro',@hoy)

		insert into FechasGestionUsuarios(email, tipo, fecha)
					values(@email, 'Baja', null)

		insert into ContrasenasdeUsuarios(email, tipo, contrasena)
					values(@email, 'Actual', @contrasena);

		insert into ContrasenasdeUsuarios(email, tipo, contrasena)
					values(@email, 'Anterior', null);
	end;

	--ACTUALIZAR USUARIO
	IF @Opc = 'U'
	begin

		--Este if es para revisar que el usuario este 'Activo'
		IF (select idEstatus from Usuarios where email = @email) = 1
		begin
			update Usuarios set nombre=@nombre, apellido1=@apellido1, apellido2=@apellido2, fechaNac=@fechaNac
						  where email = @email;
		end;
	end;

	--BAJA LÓGICA DE USUARIO
	IF @Opc = 'B'
	begin

		--Este if es para revisar que el usuario este 'Activo'
		IF (select idEstatus from Usuarios where email = @email) = 1
		begin
			--Ya que es una baja lógica, sólo se coloca el estatus con id 3 que significa 'Eliminado'
			update Usuarios set IdEstatus = 3 where email = @email;

			--Y se guarda la fecha en que se dio de baja
			update FechasGestionUsuarios set fecha=@hoy where email=@email and tipo='Baja';
		end;
	end;

	--ACTUALIZAR ESTATUS DE USUARIO
	IF @Opc = 'E'
	begin
		--Este if revisa que el estatus actual del usuario no sea 'Eliminado', pues no debería poder activarse alguien que se dio de baja
		if (select idEstatus from Usuarios where email = @email) = 1 or (select idEstatus from Usuarios where email = @email) = 2
		begin
			if @estatusNuevo = 'Activo'
			begin
				update Usuarios set IdEstatus = 1 where email = @email;
			end;

			if @estatusNuevo = 'Inhabilitado'
			begin
				update Usuarios set IdEstatus = 2 where email = @email;
			end;
		end;
	end;

	--ENCONTRAR USUARIO PARA LOGIN
	IF @Opc = 'L'
	begin
		--Revisamos que los datos coincidan con los de un usuario activo y su contraseña actual
		   select U.email, C.contrasena from Usuarios U
				  inner join ContrasenasdeUsuarios C
				  on U.email = C.email
				  where U.email = @email and C.contrasena = @contrasena and C.tipo = 'Actual'
				  and U.IdEstatus = 1;
	end;

	--OBTENER INFORMACIÓN DE USUARIO
	IF @Opc = 'O'
	begin
		--Aqui obtenemos todos los datos del usuario que se busca
		select U.email, C.contrasena, U.nombre, U.apellido1, U.apellido2, U.fechaNac, U.idGenero, U.IdEstatus from Usuarios U
	          inner join ContrasenasdeUsuarios C
			  on U.email = C.email
			  where U.email = @email and C.tipo = 'Actual';
	end;

	--CAMBIO DE CONTRASEÑA DE USUARIO
	IF @Opc = 'C'
	begin
		--Antes de actualizar la contraseña, se coloca la 'Actual' como 'Anterior'
		update ContrasenasdeUsuarios set contrasena = (select contrasena from ContrasenasdeUsuarios
					  where email = @email and tipo='Actual') where tipo='Anterior' and email=@email;
	
		--Y ahora sí­ se guarda la nueva contraseña
		update ContrasenasdeUsuarios set contrasena= @contrasena
					  where email = @email and tipo='Actual';
	end;

	--OBTENER CONTRASEÑAS DE USUARIO
	IF @Opc = 'P'
	begin
		select contrasena, tipo from ContrasenasdeUsuarios
	     where email = @email
	end;

	--GENERAR CONTRASEÑA TEMPORAL AL USUARIO
	IF @Opc = 'T'
	begin
		declare @contraNueva VARCHAR(40);
		set @contraNueva = (select dbo.Contra_al_Azar(8));
		--Antes de actualizar la contraseña, se coloca la 'Actual' como 'Anterior'
		update ContrasenasdeUsuarios set contrasena = (select contrasena from ContrasenasdeUsuarios
					  where email = @email and tipo='Actual') where tipo='Anterior' and email=@email;
	
		--Y ahora sí­ se guarda la nueva contraseña
		update ContrasenasdeUsuarios set contrasena= @contraNueva
					  where email = @email and tipo='Actual';
	end;

end;
go


--Procedures con DB_Bible
create procedure spConsultaIdiomas
as
begin
		select Id_Idioma, Nombre from DB_Bible.dbo.Idiomas;
end;
go

create procedure spConsultaVersiones(
		@Idioma	SMALLINT
)
as
begin
	--Obtenemos las versiones de la biblia del idioma elegido
	select Id_Version, NombreVersion from DB_Bible.dbo.Versiones where Id_Idioma = @Idioma;
end;
go

create procedure spConsultaTestamentos(
		@Idioma	SMALLINT
)
as
begin
	--Obtenemos los testamentos del idioma elegido
	select Id_Testamento, Nombre from DB_Bible.dbo.Testamentos where Id_Idioma = @Idioma;
end;
go

create procedure spConsultaLibros(
		@Idioma	SMALLINT,
		@Testamento	SMALLINT
)
as
begin
	--Obtenemos los libros de la biblia con las preferencias elegidas
	select Id_Libro, Nombre from DB_Bible.dbo.Libros where Id_Idioma = @Idioma and Id_Testamento = @Testamento;
end;
go

create procedure spConsultaCapitulos(
		@Libro	SMALLINT
)
as
begin
	select CapitulosTot from DB_Bible.dbo.Libros where Id_Libro = @Libro;
end;
go

create procedure spNumerodeVersiculos(
	@Libro	SMALLINT,
	@NumCap	TINYINT
)
as
begin
	SELECT COUNT(Id_Vers) AS TotalVers FROM DB_Bible.dbo.Versiculos where Id_Libro = @Libro and NumeroCap = @NumCap;
end;
go

-- PROCEDURE CONSULTAR LA BIBLIA 
create procedure spConsultaBiblia(
		@Version	SMALLINT,
		@Libro		SMALLINT,
		@NumeroCapitulo	TINYINT,
		@NumeroVersiculo TINYINT = NULL
)
as
begin

	IF	@NumeroVersiculo is null
	begin
		--Consultar todos los versiculos del capitulo elegido
		select V.Id_Version, V.Id_Libro, L.Nombre, V.NumeroCap, V.NumeroVers, V.Texto from DB_Bible.dbo.Versiculos V join DB_Bible.dbo.Libros L on V.Id_Libro = L.Id_Libro  
		          where V.Id_Version = @Version and V.Id_Libro = @Libro and V.NumeroCap = @NumeroCapitulo
		      order by L.OrdenLibro, V.NumeroCap, V.NumeroVers;
	end
	ELSE
	begin
		--Consultar el versiculo en particular elegido
		select V.Id_Version, V.Id_Libro, L.Nombre, V.NumeroCap, V.NumeroVers, V.Texto from DB_Bible.dbo.Versiculos V join DB_Bible.dbo.Libros L on V.Id_Libro = L.Id_Libro 
		          where V.Id_Version = @Version and V.Id_Libro = @Libro and V.NumeroCap = @NumeroCapitulo and V.NumeroVers = @NumeroVersiculo
				  order by L.OrdenLibro, V.NumeroCap, V.NumeroVers;
	end;

end;
go


--Procedures Favoritos

create procedure spGestionFavoritos(
		@Opc		CHAR(1),
		@idioma		SMALLINT = NULL,
		@testamento	SMALLINT = NULL,
		@version	SMALLINT = NULL,
		@libro		SMALLINT = NULL,
		@numeroCap	TINYINT = NULL,
		@numeroVers	TINYINT = NULL,
		@id			SMALLINT = NULL,
		@email	VARCHAR(40)
)
as		    
begin

	--Insertar Favorito
	IF @Opc = 'I'
	begin
		insert into Favoritos(Id_Idioma, Id_Testamento, Id_Version, Id_Libro, NumeroCap, NumeroVers, emailUsuario)
		VALUES(@idioma, @testamento, @version, @libro, @numeroCap, @numeroVers, @email);
	end;

	--Consultar favoritos
	IF @Opc = 'C'
	begin
		select F.idFav, V.Id_Version, V.Id_Libro, L.Nombre, V.NumeroCap, V.NumeroVers, V.Texto from DB_Bible.dbo.Versiculos V 
		join DB_Bible.dbo.Libros L on V.Id_Libro = L.Id_Libro 
		join Favoritos F on (V.Id_Version = F.Id_Version and V.Id_Libro = F.Id_Libro and
								V.NumeroCap = F.NumeroCap and V.NumeroVers = F.NumeroVers)
		where F.emailUsuario = @email and F.Id_Idioma = @idioma and F.Id_Version = @version
		order by L.OrdenLibro, V.NumeroCap, V.NumeroVers;
	end;

	--Borrar todos los favoritos
	IF @Opc = 'B'
	begin
		delete from Favoritos where emailUsuario = @email;
	end;

	--Borrar un favorito
	IF @Opc = 'U'
	begin
		delete from Favoritos where emailUsuario = @email and idFav = @id; 
	end;

end;
go

create procedure spGestionBusquedas(
	@Opc			CHAR(1),
	@palabra		VARCHAR(30) = NULL,
	@idioma			SMALLINT = NULL,
	@version		SMALLINT = NULL,
	@testamento		SMALLINT = NULL,
	@libro			SMALLINT = NULL,
	@huboresultados	BIT = NULL,
	@id_busqueda	SMALLINT = NULL,
	@email			VARCHAR(40) = NULL
)
as
begin
	declare @hoy DATETIME;
	set		@hoy = GETDATE();

	--Realizar busqueda
	IF @Opc = 'B'
	begin
		if @libro is null
	   begin
			select L.Nombre, V.NumeroCap, V.NumeroVers, V.Texto from DB_Bible.dbo.Versiculos V 
			join DB_Bible.dbo.Libros L on V.Id_Libro = L.Id_Libro  
		    where V.Id_Version = @version and V.Texto LIKE CONCAT('%', @palabra, '%')
		    order by L.OrdenLibro, V.NumeroCap, V.NumeroVers;
	   end

	   else
	   begin
			select L.Nombre, V.NumeroCap, V.NumeroVers, V.Texto from DB_Bible.dbo.Versiculos V 
			join DB_Bible.dbo.Libros L on V.Id_Libro = L.Id_Libro  
		    where V.Id_Version = @version and V.Id_Libro = @libro and V.Texto LIKE CONCAT('%', @palabra, '%')
		    order by L.OrdenLibro, V.NumeroCap, V.NumeroVers; 
	   end;
	end;

	--Guardar busqueda
	IF @Opc = 'G'
	begin
		IF @libro is null or @testamento is null
		begin
			insert into Busquedas(palabras, idioma, _version, fechaBusqueda, HuboResultados, emailUsuario)
			 values(@palabra, @idioma, @version, @hoy, @huboresultados, @email);
		end

		else
		begin
			insert into Busquedas(palabras, idioma, _version, testamento, libro, fechaBusqueda, HuboResultados, emailUsuario)
			 values(@palabra, @idioma, @version, @testamento, @libro, @hoy, @huboresultados, @email);
		 end;
	end;

	--Consultar historial de busquedas
	IF @Opc = 'H'
	begin
		select B.idBusqueda, I.Nombre, V.NombreVersion, T.Nombre, L.Nombre, B.palabras, B.fechaBusqueda, B.HuboResultados from Busquedas B
		join DB_Bible.dbo.Idiomas I on B.idioma = I.Id_Idioma
		join DB_Bible.dbo.Versiones V on B._version = V.Id_Version
		left join DB_Bible.dbo.Testamentos T on B.testamento = T.Id_Testamento
		left join DB_Bible.dbo.Libros L on B.libro = L.Id_Libro
		where B.emailUsuario = @email;
	end;

	--Borrar todo el historial de busquedas
	IF @Opc = 'D'
	begin
		delete from Busquedas where emailUsuario = @email;
	end;

	--Borrar una busqueda
	IF @Opc = 'U'
	begin
		delete from Busquedas where emailUsuario = @email and idBusqueda = @id_busqueda;
	end;
end;
go

--Vista para poder usar la función Rand()
CREATE VIEW RandView
AS
SELECT RAND() AS Rnd
go

--Funcion para crear contraseña temporal
CREATE FUNCTION Contra_al_Azar(@longitud_pass as INT = 8) 
RETURNS varchar(40)
as
begin
	declare @char_allow AS VARCHAR(83) = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890¡#$%&/=’?¡¿:;,.-_+*{}';
	declare @Random int;
	declare @Rnd float;
	declare @Upper int;
	declare @Lower int;
	declare @contrasenia as varchar(40);
	declare @count int;
 
	set @Lower=1;
	set @Upper=LEN(@char_allow);
 
	set @contrasenia='';
	set @count=0;
	--RAND devuelve un numero decimal de 0 y 1 ambos excluidos
	while (@count<@longitud_pass)
	begin
		select @Rnd=Rnd from RandView;
		set @Random = ROUND(((@Upper - @Lower) * @Rnd+ @Lower), 0);
		set @contrasenia=@contrasenia + substring(@char_allow, @Random, 1) + '';
 
		set @count=@count + 1;
 
	end
 
	return @contrasenia
end;
go

----------PRUEBAS----------

SELECT dbo.Contra_al_Azar(8);

EXEC spGestionUsuarios @Opc = 'I', @email ='Eduardo@hotmail.com', @nombre='armando', @apellido1='santander', @apellido2='hernandez', @fechaNac='2001-10-17', @idGenero=1, @contrasena='jose17_21';

EXEC spGestionUsuarios @Opc = 'U', @email ='Eduardo@hotmail.com', @nombre='ricardo', @apellido1='gonzallo', @apellido2='cavazos', @fechaNac='2001-03-07';

EXEC spGestionUsuarios @Opc = 'E', @email = 'Eduardo@hotmail.com', @estatusNuevo = 'Activo';

EXEC spGestionUsuarios @Opc = 'L', @email ='Eduardo@hotmail.com', @contrasena='jose17_21';

EXEC spGestionUsuarios @Opc = 'O', @email ='andreaR@correo.com';

EXEC spGestionUsuarios @Opc = 'P', @email ='andreaR@correo.com';

EXEC spGestionUsuarios @Opc = 'C', @email ='Eduardo@hotmail.com', @contrasena='lapiz_7';

EXEC spGestionUsuarios @Opc = 'B', @email ='Eduardo@hotmail.com';

EXEC spGestionUsuarios @Opc = 'T', @email = 'andreaR@correo.com';

EXEC spConsultaIdiomas;

EXEC spConsultaVersiones @Idioma = 1;

EXEC spConsultaTestamentos @Idioma = 2;

EXEC spConsultaLibros @Idioma = 1, @Testamento = 1;

EXEC spConsultaCapitulos @Libro = 8;

EXEC spNumerodeVersiculos @Libro = 3, @NumCap = 9;

EXEC spConsultaBiblia @Version = 1, @Libro = 3, @NumeroCapitulo = 9;

EXEC spGestionFavoritos @Opc = 'I', @idioma = 1, @testamento = 1, @version = 1, @libro = 3, @numeroCap = 9, @numeroVers = 1, @email = 'andreaR@correo.com';

EXEC spGestionFavoritos @Opc = 'I', @idioma = 1, @testamento = 1, @version = 1, @libro = 3, @numeroCap = 9, @numeroVers = 2, @email = 'andreaR@correo.com';

EXEC spGestionFavoritos @Opc = 'I', @idioma = 1, @testamento = 1, @version = 1, @libro = 3, @numeroCap = 9, @numeroVers = 3, @email = 'andreaR@correo.com';

EXEC spGestionFavoritos @Opc = 'C', @idioma = 1, @version = 1, @email = 'andreaR@correo.com';

EXEC spGestionFavoritos @Opc = 'U', @id = 2, @email = 'andreaR@correo.com';

EXEC spGestionFavoritos @Opc = 'B', @email = 'andreaR@correo.com';

EXEC spGestionBusquedas @Opc = 'B', @version = 1, @libro = 3, @palabra = 'buey';

EXEC spGestionBusquedas @Opc = 'B', @version = 1, @libro = 3, @palabra = 'Jehová';

EXEC spGestionBusquedas @Opc = 'G', @palabra = 'buey', @idioma = 1, @version = 1, @testamento = 1, @libro = 3, @huboresultados = 1, @email = 'andreaR@correo.com';

EXEC spGestionBusquedas @Opc = 'G', @palabra = 'buey', @idioma = 1, @version = 1, @huboresultados = 1, @email = 'andreaR@correo.com';

EXEC spGestionBusquedas @Opc = 'G', @palabra = 'Jehová', @idioma = 1, @version = 1, @testamento = 1, @libro = 3, @huboresultados = 1, @email = 'andreaR@correo.com';

EXEC spGestionBusquedas @Opc = 'H', @email = 'andreaR@correo.com';

EXEC spGestionBusquedas @Opc = 'U', @email = 'andreaR@correo.com', @id_busqueda = 2;

EXEC spGestionBusquedas @Opc = 'D', @email = 'andreaR@correo.com';

