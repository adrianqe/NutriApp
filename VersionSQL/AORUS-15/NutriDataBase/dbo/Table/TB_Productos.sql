/****** Object:  Table [dbo].[TB_Productos]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE TABLE [dbo].[TB_Productos](
	[Producto_ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Codigo_Barras] [nvarchar](50) NOT NULL,
	[Categoria] [nvarchar](100) NULL,
	[Marca] [nvarchar](50) NULL,
	[Informacion_Nutricional] [nvarchar](max) NULL,
	[Nutri_Score] [int] NULL,
	[Fecha_Registro] [datetime2](7) NOT NULL,
	[Ingredientes] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Producto_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo_Barras] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[TB_Productos] ADD  DEFAULT (sysutcdatetime()) FOR [Fecha_Registro]
ALTER TABLE [dbo].[TB_Productos]  WITH CHECK ADD CHECK  (([Nutri_Score]>=(1) AND [Nutri_Score]<=(5)))
