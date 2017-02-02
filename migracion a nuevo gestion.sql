
BEGIN /** COMIENZO MIGRACION DE LICENCIAS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT Licencias ON

INSERT INTO
	Licencias 
	(
		 Id,
		 Serial,
		 NumeroDeLlave,
		 CreatedDate,
		 UpdatedDate
	)

SELECT
	ID,
	Serial,
	NumeroDeLlave,
	GETDATE(),
	GETDATE()
	FROM
	Gestion.dbo.Licencias

SET IDENTITY_INSERT Licencias OFF

END

BEGIN /** COMIENZO MIGRACION DE PAISES **/

USE Gestion_Nuevo

SET IDENTITY_INSERT Paises ON

INSERT INTO
	Paises 
	(
		 Id,
		 Codigo,
		 Descripcion,
		 CreatedDate,
		 UpdatedDate
	)

SELECT
	ID,
	Codigo,
	Descripcion,
	GETDATE(),
	GETDATE()
	FROM
	Gestion.dbo.Paises

SET IDENTITY_INSERT Paises OFF

END

BEGIN /** COMIENZO MIGRACION DE ESTADOS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT Estados ON

INSERT INTO
	Estados 
	(
		 Id,
		 Numero,
		 Descripcion,
		 CreatedDate,
		 UpdatedDate
	)

SELECT
	ID,
	Numero,
	Descripcion,
	GETDATE(),
	GETDATE()
	FROM
	Gestion.dbo.Estados

SET IDENTITY_INSERT Estados OFF

END

BEGIN /** COMIENZO MIGRACION DE PROVINCIAS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT Provincias ON

INSERT INTO
	Provincias 
	(
		 Id,
		 PaisId,
		 Codigo,
		 Descripcion,
		 CreatedDate,
		 UpdatedDate
	)

SELECT
	ID,
	PaisID,
	Codigo,
	Descripcion,
	GETDATE(),
	GETDATE()
	FROM
	Gestion.dbo.Provincias

SET IDENTITY_INSERT Provincias OFF

END

BEGIN /** COMIENZO MIGRACION DE LOCALIDADES **/

USE Gestion_Nuevo

SET IDENTITY_INSERT Localidades ON

INSERT INTO
	Localidades 
	(
		 Id,
		 ProvinciaId,
		 Codigo,
		 Descripcion,
		 CreatedDate,
		 UpdatedDate
	)

SELECT
	ID,
	ProvinciaID,
	Codigo,
	Descripcion,
	GETDATE(),
	GETDATE()
	FROM
	Gestion.dbo.Localidades

SET IDENTITY_INSERT Localidades OFF

END

BEGIN /** COMIENZO MIGRACION DE MEDIOS DE DIFUSION **/

USE Gestion_Nuevo

SET IDENTITY_INSERT MediosDifusion ON

INSERT INTO
	MediosDifusion 
	(
		 Id,
		 MapaVisible,
		 Descripcion,
		 CreatedDate,
		 UpdatedDate
	)

SELECT
	ID,
	MapaVisible,
	Descripcion,
	GETDATE(),
	GETDATE()
	FROM
	Gestion.dbo.MediosDifusion

SET IDENTITY_INSERT MediosDifusion OFF

END

BEGIN /** COMIENZO MIGRACION DE TERMINALES **/

USE Gestion_Nuevo

SET IDENTITY_INSERT TipoTerminales ON

INSERT INTO
	TipoTerminales 
	(
		 Id,
		 Descripcion,
		 CreatedDate,
		 UpdatedDate
	)

SELECT
	ID,
	Descripcion,
	GETDATE(),
	GETDATE()
	FROM
	Gestion.dbo.TipoTerminales

SET IDENTITY_INSERT TipoTerminales OFF

END

BEGIN /** COMIENZO MIGRACION DE PRODUCTOS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT Productos ON

INSERT INTO
	Productos 
	(
		 Id,
		 Numero,
		 Descripcion,
		 CreatedDate,
		 UpdatedDate
	)

SELECT
	ID,
	Numero,
	Descripcion,
	GETDATE(),
	GETDATE()
	FROM
	Gestion.dbo.Productos

SET IDENTITY_INSERT Productos OFF

END

BEGIN /** COMIENZO MIGRACION DE PRODUCTOSMODULOS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT ProductosModulos ON

INSERT INTO
	ProductosModulos 
	(
		 [Id],
		 [ProductoId],
		 [Descripcion],
		 [Codigo],
		 [CreatedDate],
		 [UpdatedDate]
	)

SELECT
		 [Id],
		 [ProductoId],
		 [Descripcion],
		 [Codigo],
		GETDATE(),
		GETDATE()
	FROM
	Gestion.dbo.ProductosModulos

SET IDENTITY_INSERT ProductosModulos OFF

END

BEGIN /** COMIENZO MIGRACION DE REVENDEDORES **/

USE Gestion_Nuevo

SET IDENTITY_INSERT Revendedores ON

INSERT INTO
	Revendedores 
	(
		 Id,
		 Nombre,
		 Comision,
		 BajoContrato,
		 CreatedDate,
		 UpdatedDate
	)

SELECT
	ID,
	Nombre,
	Comision,
	BajoContrato,
	GETDATE(),
	GETDATE()
	FROM
	Gestion.dbo.Revendedores

SET IDENTITY_INSERT Revendedores OFF

END

BEGIN /** COMIENZO MIGRACION DE SITIOS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT Sitios ON

INSERT INTO
	Sitios 
	(
		 Id,
		 Descripcion,
		 Url,
		 CreatedDate,
		 UpdatedDate
	)

SELECT
	ID,
	Descripcion,
	Url,
	GETDATE(),
	GETDATE()
	FROM
	Gestion.dbo.Sitios

SET IDENTITY_INSERT Sitios OFF

END

BEGIN /** COMIENZO MIGRACION DE USERPROFILES **/

USE Gestion_Nuevo

SET IDENTITY_INSERT UserProfile ON

INSERT INTO
	UserProfile 
	(
		 Id,
		 UserName,
		 Email,
		 Nombre,
		 Apellido
	)

SELECT
	UserId,
	UserName,
	Email,
	Nombre,
	Apellido
	FROM
	Gestion.dbo.UserProfile

SET IDENTITY_INSERT UserProfile OFF

END

BEGIN /** COMIENZO MIGRACION DE MEMBERSHIP **/

USE Gestion_Nuevo

INSERT INTO
	webpages_Membership 
           ([UserId]
           ,[CreateDate]
           ,[ConfirmationToken]
           ,[IsConfirmed]
           ,[LastPasswordFailureDate]
           ,[PasswordFailuresSinceLastSuccess]
           ,[Password]
           ,[PasswordChangedDate]
           ,[PasswordSalt]
           ,[PasswordVerificationToken]
           ,[PasswordVerificationTokenExpirationDate])

SELECT
			[UserId]
           ,[CreateDate]
           ,[ConfirmationToken]
           ,[IsConfirmed]
           ,[LastPasswordFailureDate]
           ,[PasswordFailuresSinceLastSuccess]
           ,[Password]
           ,[PasswordChangedDate]
           ,[PasswordSalt]
           ,[PasswordVerificationToken]
           ,[PasswordVerificationTokenExpirationDate]
	FROM
	Gestion.dbo.webpages_Membership

END

BEGIN /** COMIENZO MIGRACION DE ROLES **/

USE Gestion_Nuevo

SET IDENTITY_INSERT webpages_Roles ON

INSERT INTO
	webpages_Roles 
	(
		RoleId,
		RoleName
	)

SELECT
		RoleId,
		RoleName
	FROM
	Gestion.dbo.webpages_Roles

SET IDENTITY_INSERT webpages_Roles OFF

END

BEGIN /** COMIENZO MIGRACION DE USERS IN ROLES **/

USE Gestion_Nuevo

INSERT INTO
	webpages_UsersInRoles 
	(
		RoleId,
		UserId
	)

SELECT
		RoleId,
		UserId
	FROM
	Gestion.dbo.webpages_UsersInRoles

END

BEGIN /** COMIENZO MIGRACION DE LOGS DEL SISTEMA **/

USE Gestion_Nuevo

SET IDENTITY_INSERT LogsRegistrosSistema ON

INSERT INTO
	LogsRegistrosSistema 
			(
			[Id]
			,[DescripcionAccion]
           ,[ObservacionesAccion]
           ,[UserProfileId]
           ,[Fecha]
		   ,[CreatedDate]
		   ,[UpdatedDate])
		   

SELECT
			[Id]
			,[DescripcionAccion]
           ,[ObservacionesAccion]
           ,[UserProfileID]
           ,[Fecha]
		   ,GETDATE()
		   ,GETDATE()
	FROM
	Gestion.dbo.LogsRegistrosSistema

SET IDENTITY_INSERT LogsRegistrosSistema OFF

END

BEGIN /** COMIENZO MIGRACION DE CLIENTES **/

USE Gestion_Nuevo

SET IDENTITY_INSERT Clientes ON

INSERT INTO
	Clientes 
           ([Id]
		   ,[RazonSocial]
           ,[Calle]
           ,[Altura]
           ,[Piso]
           ,[Departamento]
           ,[Domicilio]
           ,[SitioWeb]
           ,[Referencia]
           ,[Latitud]
           ,[Longitud]
           ,[FechaIngreso]
           ,[LocalidadId]
           ,[RevendedorId]
           ,[CuentaCorrienteId]
           ,[MedioDifusionId]
           ,[CreatedDate]
           ,[UpdatedDate])

SELECT
			[Id]
           ,[RazonSocial]
           ,[Calle]
           ,[Altura]
           ,[Piso]
           ,[Departamento]
           ,[Domicilio]
           ,[SitioWeb]
           ,[Referencia]
           ,[Latitud]
           ,[Longitud]
           ,[FechaIngreso]
           ,[LocalidadId]
           ,[RevendedorId]
           ,[CuentaCorrienteId]
           ,[MedioDifusionId]
           ,GETDATE()
           ,GETDATE()
	FROM
	Gestion.dbo.Clientes

SET IDENTITY_INSERT Clientes OFF

END

BEGIN /** COMIENZO MIGRACION DE CLIENTESCONTACTOS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT ClientesContactos ON

INSERT INTO
ClientesContactos
           ([Id]
		   ,[Email]
           ,[flgPrincipal]
           ,[Nombre]
           ,[Telefono]
           ,[ClienteID]
           ,[Otros]
           ,[esInstitucional]
		   ,[CreatedDate]
		   ,[UpdatedDate])

SELECT
			[Id]
		   ,[Email]
           ,[flgPrincipal]
           ,[Nombre]
           ,[Telefono]
           ,[ClienteID]
           ,[Otros]
           ,[esInstitucional]
			,GETDATE()
			,GETDATE()
	FROM
	Gestion.dbo.ClientesContactos

SET IDENTITY_INSERT ClientesContactos OFF

END

BEGIN /** COMIENZO MIGRACION DE CLIENTESGESTIONES **/

USE Gestion_Nuevo

SET IDENTITY_INSERT ClientesGestiones ON

INSERT INTO
	ClientesGestiones
           ([Id]
		   ,[EstadoId]
           ,[ClienteId]
           ,[Observaciones]
           ,[PdfGestion]
           ,[Fecha]
           ,[FechaRecontacto]
           ,[CreatedDate]
           ,[UpdatedDate])

SELECT
			[Id]
		   ,[EstadoId]
           ,[ClienteId]
           ,[Observaciones]
           ,[PdfGestion]
           ,[Fecha]
           ,[FechaRecontacto]
           ,GETDATE()
           ,GETDATE()
	FROM
	Gestion.dbo.ClientesGestiones

SET IDENTITY_INSERT ClientesGestiones OFF

END

BEGIN /** COMIENZO MIGRACION DE CLIENTESLICENCIAS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT ClientesLicencias ON

INSERT INTO
	ClientesLicencias
           ([Id]
		   ,[LicenciaId]
           ,[ClienteId]
           ,[CnnDataSource]
           ,[CnnCatalog]
           ,[CnnUser]
           ,[CnnPassword]
           ,[FechaDeVencimiento]
           ,[ConexionServidor]
           ,[SitioId]
           ,[SitioPuerto]
           ,[Alias]
           ,[AndroidPassword]
           ,[AndroidUrl]
           ,[FtpAndroidDir]
           ,[FtpAndroidUser]
           ,[FtpAndroidPassword]
           ,[SitioSubDominio]
		   ,[CreatedDate]
		   ,[UpdatedDate])

SELECT
			[Id]
		   ,[LicenciaId]
           ,[ClienteId]
           ,[CnnDataSource]
           ,[CnnCatalog]
           ,[CnnUser]
           ,[CnnPassword]
           ,[FechaDeVencimiento]
           ,[ConexionServidor]
           ,[SitioId]
           ,[SitioPuerto]
           ,[Alias]
           ,[AndroidPassword]
           ,[AndroidUrl]
           ,[FtpAndroidDir]
           ,[FtpAndroidUser]
           ,[FtpAndroidPassword]
           ,[SitioSubDominio]
		   ,GETDATE()
		   ,GETDATE()
	FROM
	Gestion.dbo.ClientesLicencias

SET IDENTITY_INSERT ClientesLicencias OFF

END

BEGIN /** COMIENZO MIGRACION DE CLIENTESTERMINALES **/

USE Gestion_Nuevo

SET IDENTITY_INSERT ClientesTerminales ON

INSERT INTO
	ClientesTerminales
           ([Id]
		   ,[TipoTerminalId]
           ,[ClienteId]
           ,[Valor1]
           ,[Valor2]
           ,[Valor3]
           ,[Valor4]
           ,[Referencia]
           ,[CreatedDate]
           ,[UpdatedDate])

SELECT
			[Id]
		   ,[TipoTerminalId]
           ,[ClienteId]
           ,[Valor1]
           ,[Valor2]
           ,[Valor3]
           ,[Valor4]
		   ,[Referencia]
           ,GETDATE()
		   ,GETDATE()
	FROM
	Gestion.dbo.ClientesTerminales

SET IDENTITY_INSERT ClientesTerminales OFF

END

BEGIN /** COMIENZO MIGRACION DE CLIENTESUSUARIOS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT ClientesUsuarios ON

INSERT INTO
	ClientesUsuarios
           ([Id]
           ,[UsuarioId]
           ,[ClienteId]
           ,[ShamanFullId]
           ,[ShamanExpressId]
           ,[CreatedDate]
           ,[UpdatedDate])

SELECT
			[Id]
           ,[UsuarioId]
           ,[ClienteId]
           ,[ShamanFullId]
           ,[ShamanExpressId]
           ,GETDATE()
		   ,GETDATE()
	FROM
	Gestion.dbo.ClientesUsuarios

SET IDENTITY_INSERT ClientesUsuarios OFF

END

BEGIN /** COMIENZO MIGRACION DE CLIENTESLICENCIASPRODUCTOS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT ClientesLicenciasProductos ON

INSERT INTO
	ClientesLicenciasProductos
           ([Id]
		   ,[ClientesLicenciaId]
		   ,[ProductoId]
		   ,[CreatedDate]
		   ,[UpdatedDate])

SELECT
			[Id]
			,[ClientesLicenciaId]
			,[ProductoId]
			,GETDATE()
			,GETDATE()
	FROM
	Gestion.dbo.ClientesLicenciasProductos

SET IDENTITY_INSERT ClientesLicenciasProductos OFF

END

BEGIN /** COMIENZO MIGRACION DE CLIENTESLICENCIASPRODUCTOSMODULOS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT ClientesLicenciasProductosModulos ON

INSERT INTO
	ClientesLicenciasProductosModulos
           ([Id]
		   ,[ClientesLicenciasProductoId]
		   ,[ProductosModuloId]
		   ,[CreatedDate]
		   ,[UpdatedDate])

SELECT
			[Id]
			,[ClientesLicenciasProductoID]
			,[ProductosModuloID]
			,GETDATE()
			,GETDATE()
	FROM
	Gestion.dbo.ClientesLicenciasProductosModulos

SET IDENTITY_INSERT ClientesLicenciasProductosModulos OFF

END

BEGIN /** COMIENZO MIGRACION DE LICENCIASPRODUCTOS **/

USE Gestion_Nuevo

INSERT INTO
	Licencias_Productos
           ([LicenciaId]
           ,[ProductoId])

SELECT
			[LicenciaId]
           ,[ProductoId]
	FROM
	Gestion.dbo.Licencias_Productos

END

BEGIN /** COMIENZO MIGRACION DE LICENCIASLOGS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT LicenciasLogs ON

INSERT INTO
	LicenciasLogs
           ([Id]
			,[LicenciaId]
			,[Type]
			,[IP]
			,[Referencias]
			,[CreatedDate]
			,[UpdatedDate]
			,[GenericDescription])

SELECT
			[Id]
           ,[LicenciaId]
		   ,[SolicitudID]
		   ,[IP]
		   ,[Referencias]
		   ,[CreatedAt]
		   ,[CreatedAt]
		   ,[GenericDescription]
			
	FROM
	Gestion.dbo.LicenciasLogs
	 WHERE LicenciaID NOT IN (0,2,24)


SET IDENTITY_INSERT LicenciasLogs OFF

END

BEGIN /** COMIENZO MIGRACION DE TICKETS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT Tickets ON

INSERT INTO
	Tickets 
	(
			[Id]
		   ,[Asunto]
           ,[UserProfileId]
           ,[TicketEstadoType]
           ,[FuturaMejora]
           ,[CreatedDate]
           ,[UpdatedDate]
	)

SELECT
		[Id]
		,[Asunto]
		,[UsuarioID]
		,[TicketEstadoID]
		,[FuturaMejora]
		,[FechaCreacion]
		,[FechaCreacion]
	FROM
	Gestion.dbo.Tickets

SET IDENTITY_INSERT Tickets OFF

END

BEGIN /** COMIENZO MIGRACION DE TICKETEVENTOS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT TicketEventos ON

INSERT INTO
	TicketEventos
           ([Id]
           ,[Descripcion]
           ,[TicketId]
           ,[UserProfileId]
           ,[ImageData]
           ,[ImageMimeType]
           ,[TicketTipoEventoType]
           ,[CreatedDate]
           ,[UpdatedDate])

SELECT
			[Id]
			,[Descripcion]
			,[TicketID]
			,[UserID]
			,[ImageData]
			,[ImageMimeType]
			,[TicketTipoEventoID]
			,[FechaCreacion]
			,[FechaCreacion]
	FROM
	Gestion.dbo.TicketEventos

SET IDENTITY_INSERT TicketEventos OFF

END

BEGIN /** COMIENZO MIGRACION DE VIDEOS **/

USE Gestion_Nuevo

SET IDENTITY_INSERT Videos ON

INSERT INTO
	Videos 
	(
			[Id]
		   ,[Descripcion]
           ,[Alias]
           ,[EsPublico]
           ,[CreatedDate]
           ,[UpdatedDate]
	)

SELECT
		[Id]
		,[Descripcion]
		,[Alias]
		,[esPublico]
		,[Fecha]
		,[Fecha]
	FROM
	Gestion.dbo.Videos

SET IDENTITY_INSERT Videos OFF

END

BEGIN /** COMIENZO MIGRACION DE VIDEOSCLIENTES **/

USE Gestion_Nuevo

SET IDENTITY_INSERT VideosClientes ON

INSERT INTO
	VideosClientes 
	(
			[Id]
		   ,[ClienteId]
           ,[VideoId]           
           ,[CreatedDate]
           ,[UpdatedDate]
	)

SELECT
		[Id]
		,[ClienteID]
		,[VideoID]
		,GETDATE()
		,GETDATE()
	FROM
	Gestion.dbo.VideosClientes

SET IDENTITY_INSERT VideosClientes OFF

END

/** UPDATES DE TABLAS VIEJAS DE TYPES, ACTUALIZO A ENUMS DE PROYECTO **/

BEGIN /** COMIENZO UPDATE SolicitudID de LicenciasLogs **/

USE Gestion_Nuevo

	/** Actualizo tipo Android **/
	UPDATE
		LicenciasLogs
	SET
		Type = 2
	WHERE
		Type = 3
	
		/** Actualizo tipo Default **/
	UPDATE
		LicenciasLogs
	SET
		Type = 1
	WHERE
		Type <> 3

END

BEGIN /** COMIENZO UPDATE de ticketestadotype **/

USE Gestion_Nuevo

	/** Actualizo resueltos **/
	UPDATE
		Tickets
	SET
		TicketEstadoType = 3
	WHERE
		ID IN (SELECT ID FROM Gestion.dbo.Tickets WHERE Resuelto = 1)
END

