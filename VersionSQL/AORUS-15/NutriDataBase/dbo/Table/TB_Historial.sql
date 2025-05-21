/****** Object:  Table [dbo].[TB_Historial]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE TABLE [dbo].[TB_Historial](
	[Historial_ID] [int] IDENTITY(1,1) NOT NULL,
	[Usuario_ID] [int] NOT NULL,
	[Producto_ID] [int] NOT NULL,
	[Fecha_Escaneo] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Historial_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[TB_Historial] ADD  DEFAULT (sysutcdatetime()) FOR [Fecha_Escaneo]
ALTER TABLE [dbo].[TB_Historial]  WITH CHECK ADD  CONSTRAINT [FK_Historial_Producto] FOREIGN KEY([Producto_ID])
REFERENCES [dbo].[TB_Productos] ([Producto_ID])
ON DELETE CASCADE
ALTER TABLE [dbo].[TB_Historial] CHECK CONSTRAINT [FK_Historial_Producto]
ALTER TABLE [dbo].[TB_Historial]  WITH CHECK ADD  CONSTRAINT [FK_Historial_Usuario] FOREIGN KEY([Usuario_ID])
REFERENCES [dbo].[TB_Usuarios] ([Usuario_ID])
ON DELETE CASCADE
ALTER TABLE [dbo].[TB_Historial] CHECK CONSTRAINT [FK_Historial_Usuario]
