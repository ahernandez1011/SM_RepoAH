USE [master]
GO

CREATE DATABASE [SM_DB]
GO

USE [SM_DB]
GO

CREATE TABLE [dbo].[tbUsuario](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Identificacion] [varchar](15) NOT NULL,
	[Nombre] [varchar](250) NOT NULL,
	[CorreoElectronico] [varchar](100) NOT NULL,
	[Contrasenna] [varchar](100) NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_tbUsuario] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tbUsuario] ON 
GO
INSERT [dbo].[tbUsuario] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Estado]) VALUES (1, N'305400751', N'Ana Hernández', N'ahernandez00751@ufide.ac.cr', N'12345678', 1)
GO
INSERT [dbo].[tbUsuario] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Estado]) VALUES (2, N'107620856', N'Claudio Hernández', N'chernandez20856@ufide.ac.cr', N'3456789', 1)
GO
SET IDENTITY_INSERT [dbo].[tbUsuario] OFF
GO

CREATE PROCEDURE [dbo].[spRegistrarUsuario]
           @Identificacion varchar(15),
           @Nombre varchar(250),
           @CorreoElectronico varchar(100),
           @Contrasenna varchar(100)
AS
BEGIN

DECLARE @Estado BIT = 1

INSERT INTO dbo.tbUsuario (Identificacion,Nombre,CorreoElectronico,Contrasenna,Estado)
     VALUES (@Identificacion, @Nombre, @CorreoElectronico, @Contrasenna, @Estado)

END
GO
USE [master]
GO
ALTER DATABASE [SM_DB] SET  READ_WRITE 
GO
