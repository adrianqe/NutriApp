/****** Object:  Table [dbo].[TB_Usuarios]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE TABLE [dbo].[TB_Usuarios](
	[Usuario_ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Password_Hash] [nvarchar](255) NOT NULL,
	[Fecha_Registro] [datetime2](7) NOT NULL,
	[cod_verificacion] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Usuario_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[TB_Usuarios] ADD  DEFAULT (sysutcdatetime()) FOR [Fecha_Registro]
