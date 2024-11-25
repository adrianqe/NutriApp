/****** Object:  Table [dbo].[cat_Alergias]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE TABLE [dbo].[cat_Alergias](
	[Id_Alergia] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Palabras_Clave] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Alergia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
