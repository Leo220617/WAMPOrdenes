CREATE TABLE [dbo].[Agrupados](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[DocEntry] [int] NOT NULL,
	[Articulo] [varchar](50) NOT NULL,
	[SustPor] [varchar](50) NULL,
	[ItemName] [varchar](500) NULL,
	[Requerido] [money] NULL,
	[CantidadReal] [money] NULL,
 CONSTRAINT [PK_Agrupados_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[BitacoraErrores](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](max) NULL,
	[Fecha] [datetime] NULL,
	[StackTrace] [varchar](max) NULL,
 CONSTRAINT [PK_BitacoraErrores] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[ConexionSAP](
	[id] [varchar](3) NULL,
	[SAPUser] [varchar](50) NULL,
	[SAPPass] [varchar](100) NULL,
	[SQLUser] [varchar](50) NULL,
	[ServerSQL] [varchar](100) NULL,
	[ServerLicense] [varchar](50) NULL,
	[SQLPass] [varchar](100) NULL,
	[SQLType] [varchar](50) NULL,
	[SQLBD] [varchar](50) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[DetOrdenesCompra](
	[DocEntry] [int] NOT NULL,
	[LineNum] [int] NOT NULL,
	[CodPro] [varchar](500) NULL,
	[NombreProducto] [varchar](5000) NULL,
	[Cantidad] [money] NULL,
	[Almacen] [varchar](5) NULL,
	[PrecioUnitario] [money] NULL,
	[Status] [varchar](50) NULL,
 CONSTRAINT [PK_DetOrdenesCompra] PRIMARY KEY CLUSTERED 
(
	[DocEntry] ASC,
	[LineNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[EncOrdenesCompra](
	[DocEntry] [int] NOT NULL,
	[DocNum] [int] NULL,
	[Moneda] [varchar](5) NULL,
	[CardCode] [varchar](500) NULL,
	[CardName] [varchar](500) NULL,
	[DocDate] [datetime] NULL,
	[Series] [varchar](5) NULL,
	[Estado] [varchar](50) NULL,
	[Comentarios] [varchar](5000) NULL,
 CONSTRAINT [PK_EncOrdenesCompra] PRIMARY KEY CLUSTERED 
(
	[DocEntry] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[Generados](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[BaseEntry] [int] NULL,
	[DocEntry] [int] NULL,
	[Message] [varchar](50) NULL,
	[Tipo] [varchar](50) NULL,
	[Fecha] [datetime] NULL,
	[OrdenCompra] [int] NULL,
 CONSTRAINT [PK_Generados] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Login](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idRol] [int] NULL,
	[Email] [varchar](200) NULL,
	[Nombre] [varchar](100) NULL,
	[Activo] [bit] NULL,
	[Clave] [varchar](500) NULL,
	[CambiarClave] [bit] NULL,
 CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OrdenesLineas](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Orden] [int] NULL,
	[OrdenFabricacion] [int] NULL,
	[Linea] [int] NULL,
	[Articulo] [varchar](50) NULL,
	[ItemName] [varchar](500) NULL,
	[Requerido] [money] NULL,
 CONSTRAINT [PK_OrdenesLineas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Parametros](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[SQLEncOrdenesCompra] [varchar](max) NULL,
	[SQLDetOrdenesCompra] [varchar](max) NULL,
	[SQLOrdenesProduccion] [varchar](max) NULL,
	[SQLExistencias] [varchar](max) NULL,
	[SQLProductoxOrdenes] [varchar](max) NULL,
	[SQLAgrupado] [varchar](max) NULL,
	[SQLLineaxOrden] [varchar](max) NULL,
	[SQLOrdenesFabricacionAsociadas] [varchar](max) NULL,
	[SQLPreguntaExisten] [varchar](max) NULL,
	[SQLRecibo] [varchar](max) NULL,
 CONSTRAINT [PK_Parametros] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Roles](
	[idRol] [int] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](50) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[idRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SeguridadModulos](
	[CodModulo] [int] NOT NULL,
	[Descripcion] [varchar](150) NOT NULL,
 CONSTRAINT [PK_SeguridadModulos_1] PRIMARY KEY CLUSTERED 
(
	[CodModulo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SeguridadRolesModulos](
	[CodRol] [int] NOT NULL,
	[CodModulo] [int] NOT NULL,
 CONSTRAINT [PK_SeguridadRolesModulos_1] PRIMARY KEY CLUSTERED 
(
	[CodRol] ASC,
	[CodModulo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

