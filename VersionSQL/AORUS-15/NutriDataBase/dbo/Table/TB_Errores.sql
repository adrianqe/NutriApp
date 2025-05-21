/****** Object:  Table [dbo].[TB_Errores]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE TABLE [dbo].[TB_Errores](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[SEVERIDAD] [int] NOT NULL,
	[STORE_PROCEDURE] [nvarchar](255) NOT NULL,
	[NUMERO] [int] NOT NULL,
	[DESCRIPCION] [nvarchar](255) NOT NULL,
	[LINEA] [int] NOT NULL,
	[FECHA_HORA] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[TB_Errores] ADD  DEFAULT (sysutcdatetime()) FOR [FECHA_HORA]
