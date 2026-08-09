USE [master]
GO

CREATE DATABASE [SM_DB]
GO

USE [SM_DB]
GO 

CREATE TABLE [dbo].[tbError](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Mensaje] [varchar](max) NOT NULL,
	[Lugar] [varchar](50) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
	[ConsecutivoUsuario] [int] NOT NULL,
 CONSTRAINT [PK_tbError] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tbEstado](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbEstado] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tbMensaje](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Mensaje] [varchar](max) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
	[ConsecutivoUsuario] [int] NOT NULL,
	[ConsecutivoSolicitud] [int] NOT NULL,
 CONSTRAINT [PK_tbMensaje] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tbRol](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbRol] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tbSolicitud](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](150) NOT NULL,
	[Descripcion] [varchar](max) NOT NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[FechaFinalizacion] [datetime] NULL,
	[ConsecutivoUsuario] [int] NOT NULL,
	[ConsecutivoAdmin] [int] NOT NULL,
	[ConsecutivoEstado] [int] NOT NULL,
	[Solucion] [varchar](max) NULL,
 CONSTRAINT [PK_tbSolicitud] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tbUsuario](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Identificacion] [varchar](15) NOT NULL,
	[Nombre] [varchar](250) NOT NULL,
	[CorreoElectronico] [varchar](100) NOT NULL,
	[Contrasenna] [varchar](100) NOT NULL,
	[Estado] [bit] NOT NULL,
	[IndicadorTemp] [bit] NOT NULL,
	[ConsecutivoRol] [int] NOT NULL,
 CONSTRAINT [PK_tbUsuario] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tbEstado] ON 
GO
INSERT [dbo].[tbEstado] ([Consecutivo], [Nombre]) VALUES (1, N'Abierto')
GO
INSERT [dbo].[tbEstado] ([Consecutivo], [Nombre]) VALUES (2, N'Cancelado')
GO
INSERT [dbo].[tbEstado] ([Consecutivo], [Nombre]) VALUES (3, N'Atendido')
GO
SET IDENTITY_INSERT [dbo].[tbEstado] OFF
GO
SET IDENTITY_INSERT [dbo].[tbMensaje] ON 
GO
INSERT [dbo].[tbMensaje] ([Consecutivo], [Mensaje], [FechaHora], [ConsecutivoUsuario], [ConsecutivoSolicitud]) VALUES (1, N'Hola', CAST(N'2026-08-08T20:21:08.577' AS DateTime), 4, 10)
GO
INSERT [dbo].[tbMensaje] ([Consecutivo], [Mensaje], [FechaHora], [ConsecutivoUsuario], [ConsecutivoSolicitud]) VALUES (2, N'Hola en que te puedo ayudar?', CAST(N'2026-08-08T20:21:32.160' AS DateTime), 3, 10)
GO
INSERT [dbo].[tbMensaje] ([Consecutivo], [Mensaje], [FechaHora], [ConsecutivoUsuario], [ConsecutivoSolicitud]) VALUES (3, N'Tengo un problema con el SQL', CAST(N'2026-08-08T20:21:58.470' AS DateTime), 4, 10)
GO
INSERT [dbo].[tbMensaje] ([Consecutivo], [Mensaje], [FechaHora], [ConsecutivoUsuario], [ConsecutivoSolicitud]) VALUES (4, N'Claro, que seria especificamente?', CAST(N'2026-08-08T20:22:18.007' AS DateTime), 3, 10)
GO
SET IDENTITY_INSERT [dbo].[tbMensaje] OFF
GO
SET IDENTITY_INSERT [dbo].[tbRol] ON 
GO
INSERT [dbo].[tbRol] ([Consecutivo], [Nombre]) VALUES (1, N'Usuario')
GO
INSERT [dbo].[tbRol] ([Consecutivo], [Nombre]) VALUES (2, N'Soporte')
GO
SET IDENTITY_INSERT [dbo].[tbRol] OFF
GO
SET IDENTITY_INSERT [dbo].[tbSolicitud] ON 
GO
INSERT [dbo].[tbSolicitud] ([Consecutivo], [Titulo], [Descripcion], [FechaRegistro], [FechaFinalizacion], [ConsecutivoUsuario], [ConsecutivoAdmin], [ConsecutivoEstado], [Solucion]) VALUES (8, N'Error SQL por bloqueo en actualización de registros', N'Se produjo un error de SQL durante la actualización de registros debido a un bloqueo concurrente en la base de datos.', CAST(N'2026-08-06T14:23:47.030' AS DateTime), CAST(N'2026-08-06T14:28:39.037' AS DateTime), 4, 6, 2, N'Solicitud cancelada por el usuario')
GO
INSERT [dbo].[tbSolicitud] ([Consecutivo], [Titulo], [Descripcion], [FechaRegistro], [FechaFinalizacion], [ConsecutivoUsuario], [ConsecutivoAdmin], [ConsecutivoEstado], [Solucion]) VALUES (9, N'Error identificado por timeout SQL Server', N'Error identificado por timeout SQL ServerError identificado por timeout SQL ServerError identificado por timeout SQL Server', CAST(N'2026-08-06T14:51:46.640' AS DateTime), NULL, 4, 6, 1, NULL)
GO
INSERT [dbo].[tbSolicitud] ([Consecutivo], [Titulo], [Descripcion], [FechaRegistro], [FechaFinalizacion], [ConsecutivoUsuario], [ConsecutivoAdmin], [ConsecutivoEstado], [Solucion]) VALUES (10, N'Error SQL por bloqueo en actualización de registros', N'Error SQL por bloqueo en actualización de registrosError SQL por bloqueo en actualización de registrosError SQL por bloqueo en actualización de registros', CAST(N'2026-08-06T14:52:14.270' AS DateTime), NULL, 4, 3, 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[tbSolicitud] OFF
GO
SET IDENTITY_INSERT [dbo].[tbUsuario] ON 
GO
INSERT [dbo].[tbUsuario] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Estado], [IndicadorTemp], [ConsecutivoRol]) VALUES (3, N'305400751', N'Ana Hernández', N'ahernandez00751@ufide.ac.cr', N'$2a$11$05HEvxzSczj6PPPPvlIweO7J.KUhPXJloveOlbuyysqhYOKGN0jIK', 1, 0, 2)
GO
INSERT [dbo].[tbUsuario] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Estado], [IndicadorTemp], [ConsecutivoRol]) VALUES (4, N'3027080256', N'Claudio Hernández', N'chernandez80256@ufide.ac.cr', N'$2a$11$ecXprHZ2Xzw7ysjZHHJOc.xEVcQm/EW.6Z6MB7E9988VsPVe6j1GW', 1, 0, 1)
GO
INSERT [dbo].[tbUsuario] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Estado], [IndicadorTemp], [ConsecutivoRol]) VALUES (6, N'12345678', N'Teresita Hernández', N'thernandez123456@ufide.ac.cr', N'$2a$11$IhXgKlUc.QVZsmQ8SMyckuzvAyrqo8ALvC4ZluiUpm.GfjEYP/g9K', 1, 0, 2)
GO
SET IDENTITY_INSERT [dbo].[tbUsuario] OFF
GO

ALTER TABLE [dbo].[tbUsuario] ADD  CONSTRAINT [UK_CorreoElectronico] UNIQUE NONCLUSTERED 
(
	[CorreoElectronico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO


ALTER TABLE [dbo].[tbUsuario] ADD  CONSTRAINT [UK_Identificacion] UNIQUE NONCLUSTERED 
(
	[Identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbMensaje]  WITH CHECK ADD  CONSTRAINT [FK_tbMensaje_tbSolicitud] FOREIGN KEY([ConsecutivoSolicitud])
REFERENCES [dbo].[tbSolicitud] ([Consecutivo])
GO
ALTER TABLE [dbo].[tbMensaje] CHECK CONSTRAINT [FK_tbMensaje_tbSolicitud]
GO
ALTER TABLE [dbo].[tbMensaje]  WITH CHECK ADD  CONSTRAINT [FK_tbMensaje_tbUsuario] FOREIGN KEY([ConsecutivoUsuario])
REFERENCES [dbo].[tbUsuario] ([Consecutivo])
GO
ALTER TABLE [dbo].[tbMensaje] CHECK CONSTRAINT [FK_tbMensaje_tbUsuario]
GO
ALTER TABLE [dbo].[tbSolicitud]  WITH CHECK ADD  CONSTRAINT [FK_tbSolicitud_Admin] FOREIGN KEY([ConsecutivoAdmin])
REFERENCES [dbo].[tbUsuario] ([Consecutivo])
GO
ALTER TABLE [dbo].[tbSolicitud] CHECK CONSTRAINT [FK_tbSolicitud_Admin]
GO
ALTER TABLE [dbo].[tbSolicitud]  WITH CHECK ADD  CONSTRAINT [FK_tbSolicitud_Estado] FOREIGN KEY([ConsecutivoEstado])
REFERENCES [dbo].[tbEstado] ([Consecutivo])
GO
ALTER TABLE [dbo].[tbSolicitud] CHECK CONSTRAINT [FK_tbSolicitud_Estado]
GO
ALTER TABLE [dbo].[tbSolicitud]  WITH CHECK ADD  CONSTRAINT [FK_tbSolicitud_Usuario] FOREIGN KEY([ConsecutivoUsuario])
REFERENCES [dbo].[tbUsuario] ([Consecutivo])
GO
ALTER TABLE [dbo].[tbSolicitud] CHECK CONSTRAINT [FK_tbSolicitud_Usuario]
GO

CREATE PROCEDURE [dbo].[spActualizarContrasenna]
           @Consecutivo int,
           @Contrasenna varchar(100),
		   @IndicadorTemp bit
AS
BEGIN

 UPDATE tbUsuario
 SET Contrasenna = @Contrasenna,
 IndicadorTemp = @IndicadorTemp
 WHERE Consecutivo = @Consecutivo

END
GO

CREATE PROCEDURE [dbo].[spActualizarPerfil]
    @Consecutivo        int,
    @Identificacion     varchar(15),
    @Nombre             varchar(250),
    @CorreoElectronico  varchar(100)
AS
BEGIN

    UPDATE  tbUsuario
    SET     Identificacion      = @Identificacion,
            Nombre              = @Nombre,
            CorreoElectronico   = @CorreoElectronico
    WHERE   Consecutivo = @Consecutivo

END
GO


CREATE PROCEDURE [dbo].[spCancelarSolicitudUsuario]
    @ConsecutivoSolicitud   int,
    @ConsecutivoUsuario     int
AS
BEGIN

    UPDATE  tbSolicitud
    SET     ConsecutivoEstado = 2,
            FechaFinalizacion = GETDATE(),
			Solucion = 'Solicitud cancelada por el usuario'
    WHERE   Consecutivo        = @ConsecutivoSolicitud
        AND ConsecutivoUsuario = @ConsecutivoUsuario
        AND ConsecutivoEstado  = 1

END
GO

CREATE PROCEDURE [dbo].[spConsultarMensajes]
    @ConsecutivoSolicitud   int
AS
BEGIN
	

    SELECT  M.Consecutivo,
            M.Mensaje,
            M.FechaHora,
            M.ConsecutivoUsuario,
            U.Nombre 'NombreUsuario'
      FROM  dbo.tbMensaje M
      INNER JOIN dbo.tbUsuario U ON U.Consecutivo = M.ConsecutivoUsuario
      WHERE M.ConsecutivoSolicitud = @ConsecutivoSolicitud
      ORDER BY M.FechaHora

END
GO

CREATE PROCEDURE [dbo].[spConsultarSolicitud]
    @ConsecutivoSolicitud  int
AS
BEGIN
	
    SELECT S.Consecutivo,
           S.Titulo,
           S.Descripcion,
           S.FechaRegistro,
           S.FechaFinalizacion,
           U.Nombre 'NombreAdmin',
           E.Nombre 'NombreEstado'
      FROM dbo.tbSolicitud S
      INNER JOIN dbo.tbUsuario U ON S.ConsecutivoAdmin = U.Consecutivo
      INNER JOIN dbo.tbEstado E ON S.ConsecutivoEstado = E.Consecutivo
      WHERE S.Consecutivo = @ConsecutivoSolicitud

END
GO

CREATE PROCEDURE [dbo].[spConsultarSolicitudesAbiertas]
    @ConsecutivoUsuario  int,
    @ConsecutivoRol      int
AS
BEGIN
	
    IF(@ConsecutivoRol = 1)
    BEGIN

        SELECT  S.Consecutivo,
                S.Titulo,
                U.Nombre 'NombreInterlocutor'
        FROM    dbo.tbSolicitud S
        INNER   JOIN dbo.tbUsuario U ON S.ConsecutivoAdmin = U.Consecutivo
        WHERE   S.ConsecutivoUsuario = @ConsecutivoUsuario
            AND S.ConsecutivoEstado = 1

    END
    ELSE
    BEGIN

        SELECT  S.Consecutivo,
                S.Titulo,
                U.Nombre 'NombreInterlocutor'
        FROM    dbo.tbSolicitud S
        INNER   JOIN dbo.tbUsuario U ON S.ConsecutivoUsuario = U.Consecutivo
        WHERE   S.ConsecutivoAdmin = @ConsecutivoUsuario
            AND S.ConsecutivoEstado = 1
    END
END
GO

CREATE PROCEDURE [dbo].[spConsultarSolicitudesAdmin]
 @ConsecutivoAdmin int
AS
BEGIN
	
    SELECT S.Consecutivo,
           S.Titulo,
           S.Descripcion,
           S.FechaRegistro,
           S.FechaFinalizacion,
           U.Nombre 'NombreUsuario',
           E.Nombre 'NombreEstado'
      FROM dbo.tbSolicitud S
      INNER JOIN dbo.tbUsuario U ON S.ConsecutivoUsuario = U.Consecutivo
      INNER JOIN dbo.tbEstado E ON S.ConsecutivoEstado = E.Consecutivo
      WHERE S.ConsecutivoAdmin = @ConsecutivoAdmin

END
GO

CREATE PROCEDURE [dbo].[spConsultarSolicitudesUsuario]
 @ConsecutivoUsuario  int
AS
BEGIN
	
    SELECT S.Consecutivo,
           S.Titulo,
           S.Descripcion,
           S.FechaRegistro,
           S.FechaFinalizacion,
           U.Nombre 'NombreAdmin',
           E.Nombre 'NombreEstado'
      FROM dbo.tbSolicitud S
      INNER JOIN dbo.tbUsuario U ON S.ConsecutivoAdmin = U.Consecutivo
      INNER JOIN dbo.tbEstado E ON S.ConsecutivoEstado = E.Consecutivo
      WHERE S.ConsecutivoUsuario = @ConsecutivoUsuario

END
GO

CREATE PROCEDURE [dbo].[spConsultarUsuario]
    @Consecutivo  int
AS
BEGIN
	
    SELECT  Consecutivo,
            Identificacion,
            Nombre,
            CorreoElectronico,
            Estado,
            IndicadorTemp
    FROM    dbo.tbUsuario
    WHERE   Consecutivo = @Consecutivo

END
GO

CREATE PROCEDURE [dbo].[spIniciarSesionUsuario]
           @CorreoElectronico varchar(100),
           @Contrasenna varchar(100)
AS
BEGIN

SELECT U.Consecutivo, Identificacion,U.Nombre,CorreoElectronico,Estado,IndicadorTemp, Contrasenna, ConsecutivoRol, R.Nombre 'NombreRol'
  FROM dbo.tbUsuario U
  INNER JOIN dbo.tbRol R ON U.ConsecutivoRol = R.Consecutivo
  WHERE CorreoElectronico = @CorreoElectronico
      --AND Contrasenna = @Contrasenna
	  AND Estado = 1
END
GO

CREATE PROCEDURE [dbo].[spRegistrarError]
           @Mensaje varchar(max),
           @Lugar varchar(250),
           @FechaHora datetime,
           @ConsecutivoUsuario int
AS
BEGIN

INSERT INTO dbo.tbError(Mensaje,Lugar, FechaHora,ConsecutivoUsuario)
     VALUES (@Mensaje, @Lugar, @FechaHora, @ConsecutivoUsuario)

END
GO

CREATE PROCEDURE [dbo].[spRegistrarMensaje]
    @ConsecutivoUsuario     int,
    @ConsecutivoSolicitud   int,
    @Mensaje                varchar(max)
AS
BEGIN
	
    INSERT INTO dbo.tbMensaje (Mensaje,FechaHora,ConsecutivoUsuario,ConsecutivoSolicitud)
    VALUES (@Mensaje,GETDATE(),@ConsecutivoUsuario,@ConsecutivoSolicitud)

    SELECT SCOPE_IDENTITY() AS 'Consecutivo'

END
GO

CREATE PROCEDURE [dbo].[spRegistrarSolicitud]
    @Titulo             varchar(150),
    @Descripcion        varchar(max),
    @ConsecutivoUsuario int
AS
BEGIN

    --ADMIN CON MENOS SOLICITUDES EN COLA
    DECLARE @ConsecutivoAdmin INT

    SELECT  TOP 1 @ConsecutivoAdmin = U.Consecutivo
    FROM    dbo.tbUsuario U
    LEFT JOIN dbo.tbSolicitud S ON  S.ConsecutivoAdmin = U.Consecutivo
                                AND S.ConsecutivoEstado = 1
    WHERE   U.ConsecutivoRol = 2
    GROUP BY U.Consecutivo
    ORDER BY COUNT(S.Consecutivo)


    INSERT INTO dbo.tbSolicitud(Titulo,Descripcion,FechaRegistro,FechaFinalizacion,
                                ConsecutivoUsuario,ConsecutivoAdmin,ConsecutivoEstado, Solucion)
     VALUES(@Titulo,@Descripcion,GETDATE(),NULL,@ConsecutivoUsuario,@ConsecutivoAdmin,1,NULL)

	 SELECT SCOPE_IDENTITY() 'Consecutivo'
END
GO

CREATE PROCEDURE [dbo].[spRegistrarUsuario]
           @Identificacion varchar(15),
           @Nombre varchar(250),
           @CorreoElectronico varchar(100),
           @Contrasenna varchar(100)
AS
BEGIN

IF NOT EXISTS ( SELECT 1 FROM tbUsuario
WHERE Identificacion = @Identificacion
OR CorreoElectronico = @CorreoElectronico)

   BEGIN

DECLARE @Estado BIT = 1
DECLARE @ClaveTemp BIT = 0
DECLARE @Rol INT = 1

INSERT INTO dbo.tbUsuario (Identificacion,Nombre,CorreoElectronico,Contrasenna,Estado,IndicadorTemp,ConsecutivoRol)
     VALUES (@Identificacion, @Nombre, @CorreoElectronico, @Contrasenna, @Estado, @ClaveTemp,@Rol)

   END
END
GO

CREATE PROCEDURE [dbo].[spValidarAccesoSolicitud]
    @ConsecutivoSolicitud  int,
    @ConsecutivoUsuario    int
AS
BEGIN
	
    SELECT  COUNT(1)
    FROM    dbo.tbSolicitud
    WHERE   Consecutivo = @ConsecutivoSolicitud
        AND (ConsecutivoUsuario = @ConsecutivoUsuario OR ConsecutivoAdmin = @ConsecutivoUsuario)

END
GO

CREATE PROCEDURE [dbo].[spValidarCorreo]
           @CorreoElectronico varchar(100)
AS
BEGIN

SELECT Consecutivo, 
      Identificacion, 
	  Nombre,
	  CorreoElectronico,
	  Estado,
	  IndicadorTemp
  FROM dbo.tbUsuario
  WHERE CorreoElectronico = @CorreoElectronico
	  AND Estado = 1
END
GO
USE [master]
GO
ALTER DATABASE [SM_DB] SET  READ_WRITE 
GO
