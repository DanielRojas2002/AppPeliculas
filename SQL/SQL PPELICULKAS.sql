CREATE DATABASE [DBPELICULAS]
GO

USE [DBPELICULAS]
GO



IF OBJECT_ID('dbo.CATEGORIA', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[CATEGORIA]
END
GO
CREATE TABLE [dbo].[CATEGORIA]
(
	IdCategoria			INT IDENTITY(1,1) NOT NULL,
	Descripcion			NVARCHAR(80)	  NOT NULL,
	CONSTRAINT PK_IdCategoria				  PRIMARY KEY (IdCategoria)
);
GO

IF OBJECT_ID('dbo.TIPOENTRADA', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[TIPOENTRADA]
END
GO
CREATE TABLE [dbo].[TIPOENTRADA]
(
	IdTipoEntrada		INT IDENTITY(1,1) NOT NULL,
	Descripcion			NVARCHAR(20)	  NOT NULL,
	CONSTRAINT PK_IdTipoEntrada				  PRIMARY KEY (IdTipoEntrada)
);
GO


IF OBJECT_ID('dbo.ESTATUSCARRITO', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[ESTATUSCARRITO]
END
GO
CREATE TABLE [dbo].[ESTATUSCARRITO]
(
	IdEstatus	INT IDENTITY(1,1) NOT NULL,
	Descripcion			NVARCHAR(20)	  NOT NULL,
	CONSTRAINT PK_IdEstatus				  PRIMARY KEY (IdEstatus)
);
GO

IF OBJECT_ID('dbo.AUTOR', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[AUTOR]
END
GO
CREATE TABLE [dbo].[AUTOR]
(
	IdAutor			INT IDENTITY(1,1) NOT NULL,
	Nombre			NVARCHAR(80)	  NOT NULL,
	A_Paterno			NVARCHAR(80)	  NOT NULL,
	A_Materno			NVARCHAR(80)	  NOT NULL,
	CONSTRAINT PK_Autor				  PRIMARY KEY (IdAutor)
);
GO


IF OBJECT_ID('dbo.CATEGORIA_AUTOR', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[CATEGORIA_AUTOR]
END
GO
CREATE TABLE [dbo].[CATEGORIA_AUTOR]
(
	IdCategoriaAutor			INT IDENTITY(1,1) NOT NULL,
	IdCategoria			INT	  NOT NULL,
	IdAutor			INT	  NOT NULL,
	
	CONSTRAINT PK_CategoriaAutor				  PRIMARY KEY (IdCategoriaAutor),
	CONSTRAINT FK_CategoriaAutor_Categoria			  FOREIGN KEY (IdCategoria)			REFERENCES CATEGORIA(IdCategoria),
	CONSTRAINT FK_CategoriaAutor_Autor				  FOREIGN KEY (IdAutor)			REFERENCES AUTOR(IdAutor)
);
GO

ALTER TABLE [CATEGORIA_AUTOR]
ADD CONSTRAINT UC_IdCategoria_IdAutor UNIQUE (IdCategoria, IdAutor);


IF OBJECT_ID('dbo.ESTATUS_PELICULA', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[ESTATUS_PELICULA]
END
GO
CREATE TABLE [dbo].[ESTATUS_PELICULA]
(
	IdEstatusPelicula		INT IDENTITY(1,1) NOT NULL,
	Descripcion			NVARCHAR(20)	  NOT NULL,
	CONSTRAINT PK_IdEstatusPelicula				  PRIMARY KEY (IdEstatusPelicula	)
);
GO



IF OBJECT_ID('dbo.PELICULA', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[PELICULA]
END
GO
CREATE TABLE [dbo].[PELICULA]
(
	IdPelicula			INT IDENTITY(1,1) NOT NULL,
	IdCategoria				INT				  NOT NULL,
	IdEstatusPelicula			INT				  NOT NULL,
	Titulo				NVARCHAR(80)	  NOT NULL,
	Imagen				NVARCHAR(280)   NULL,
	Descripcion			NVARCHAR(280)	  NOT NULL,
	FechaRegistro  		DATETIME		  NULL,
	Duracion			TiME			  NOT NULL,
	Stock				INT			  NULL	
	CONSTRAINT PK_IdUsuario				  PRIMARY KEY (IdPelicula),
	CONSTRAINT FK_IdCategoria_Pelicula			  FOREIGN KEY (IdCategoria)			REFERENCES CATEGORIA(IdCategoria),
	CONSTRAINT FK_IdEstatusPelicula_Pelicula		  FOREIGN KEY (IdEstatusPelicula)		REFERENCES ESTATUS_PELICULA(IdEstatusPelicula),
);
GO

IF OBJECT_ID('dbo.ALMACEN', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[ALMACEN]
END
GO
CREATE TABLE [dbo].[ALMACEN]
(
	IdAlmacen			INT IDENTITY(1,1) NOT NULL,
	IdPelicula				INT				  NOT NULL,
	IdTipoEntrada		INT				  NOT NULL,
	FechaRegistro  		DATETIME		  NOT NULL,
	Cantidad	INT  NOT NULL
	CONSTRAINT PK_Almacen				  PRIMARY KEY (IdAlmacen),
	CONSTRAINT PK_Almacen_Pelicula			  FOREIGN KEY (IdPelicula)			REFERENCES PELICULA(IdPelicula),
	CONSTRAINT PK_Almacen_TipoEntrada		  FOREIGN KEY (IdTipoEntrada)			REFERENCES TipoEntrada(IdTipoEntrada)
);
GO



IF OBJECT_ID('dbo.PELICULA_AUTOR', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[PELICULA_AUTOR]
END
GO
CREATE TABLE [dbo].[PELICULA_AUTOR]
(
	IdPeliculaAutor			INT IDENTITY(1,1) NOT NULL,
	IdPelicula				INT				  NOT NULL,
	IdCategoriaAutor			INT				  NOT NULL
	CONSTRAINT PK_PeliculaAutor				  PRIMARY KEY (IdPeliculaAutor),
	CONSTRAINT FK_PeliculaAutor_Pelicula		  FOREIGN KEY (IdPelicula)			REFERENCES PELICULA(IdPelicula),
	CONSTRAINT FK_PeliculaAutor_CategoriaAutor		  FOREIGN KEY (IdCategoriaAutor)		REFERENCES CATEGORIA_AUTOR(IdCategoriaAutor)
);
GO

ALTER TABLE [PELICULA_AUTOR]
ADD CONSTRAINT UC_IdPelicula_IdAutor UNIQUE (IdPelicula, IdCategoriaAutor);

IF OBJECT_ID('dbo.TIPOUSUARIO', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[TIPOUSUARIO]
END
GO
CREATE TABLE [dbo].[TIPOUSUARIO]
(
	IdTipoUsuario		INT IDENTITY(1,1) NOT NULL,
	Descripcion			NVARCHAR(20)	  NOT NULL,
	CONSTRAINT PK_TipoUsuario			  PRIMARY KEY (IdTipoUsuario)
);
GO

IF OBJECT_ID('dbo.USUARIO', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[USUARIO]
END
GO
CREATE TABLE [dbo].[USUARIO]
(
	IdUsuario			INT IDENTITY(1,1) NOT NULL,
	IdTipoUsuario			INT				  NOT NULL,
	Nombre				NVARCHAR(80)	  NOT NULL,
	A_Paterno				NVARCHAR(80)	   NULL,
	A_Materno				NVARCHAR(80)	   NULL,
	Correo				NVARCHAR(150)	  NOT NULL,
	Contrasena			NVARCHAR(80)	  NOT NULL,
	FechaRegistro  		DATETIME		  NOT NULL,	
	CONSTRAINT PK_Usuario				  PRIMARY KEY (IdUsuario),
	CONSTRAINT FK_Usuario_TipoUsuario		  FOREIGN KEY (IdTipoUsuario)			REFERENCES TIPOUSUARIO(IdTipoUsuario)

	
);
GO


IF OBJECT_ID('dbo.CARRITO', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[CARRITO]
END
GO
CREATE TABLE [dbo].[CARRITO]
(
	IdCarrito			INT IDENTITY(1,1) NOT NULL,
	IdUsuario			INT				  NOT NULL,
	FechaRegistro  		DATETIME		  NOT NULL,	
	FechaPedido  		DATETIME		  NULL,	
	IdEstatus			INT				  NOT NULL,
	CONSTRAINT PK_Carrito				  PRIMARY KEY (IdCarrito),
	CONSTRAINT FK_Carrito_Usuario		  FOREIGN KEY (IdUsuario)			REFERENCES USUARIO(IdUsuario),
	CONSTRAINT FK_Carrito_Estatus		  FOREIGN KEY (IdEstatus)			REFERENCES ESTATUSCARRITO(IdEstatus)

	
);
GO




IF OBJECT_ID('dbo.CARRITO_DETALLE', 'U') IS NOT NULL
BEGIN
	DROP TABLE [dbo].[CARRITO_DETALLE]
END
GO
CREATE TABLE [dbo].[CARRITO_DETALLE]
(
	IdCarritoDetalle			INT IDENTITY(1,1) NOT NULL,
	IdCarrito			INT				  NOT NULL,
	IdPelicula  		INT				  NOT NULL,	
	Stock				INT				  NOT NULL,	
	CONSTRAINT PK_CarritoDetalle				  PRIMARY KEY (IdCarritoDetalle),
	CONSTRAINT FK_CarritoDetalle_Carrito		  FOREIGN KEY (IdCarrito)			REFERENCES CARRITO(IdCarrito),
	CONSTRAINT FK_CarritoDetalle_Pelicula		  FOREIGN KEY (IdPelicula)			REFERENCES PELICULA(IdPelicula)

	
);
GO