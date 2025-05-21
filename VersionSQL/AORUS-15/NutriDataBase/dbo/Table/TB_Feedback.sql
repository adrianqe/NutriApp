/****** Object:  Table [dbo].[TB_Feedback]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE TABLE [dbo].[TB_Feedback](
	[Feedback_ID] [int] IDENTITY(1,1) NOT NULL,
	[Usuario_ID] [int] NOT NULL,
	[Producto_ID] [int] NOT NULL,
	[Calificacion] [int] NOT NULL,
	[Comentario] [nvarchar](500) NULL,
	[Fecha] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Feedback_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[TB_Feedback] ADD  DEFAULT (sysutcdatetime()) FOR [Fecha]
ALTER TABLE [dbo].[TB_Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_Producto] FOREIGN KEY([Producto_ID])
REFERENCES [dbo].[TB_Productos] ([Producto_ID])
ON DELETE CASCADE
ALTER TABLE [dbo].[TB_Feedback] CHECK CONSTRAINT [FK_Feedback_Producto]
ALTER TABLE [dbo].[TB_Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_Usuario] FOREIGN KEY([Usuario_ID])
REFERENCES [dbo].[TB_Usuarios] ([Usuario_ID])
ON DELETE CASCADE
ALTER TABLE [dbo].[TB_Feedback] CHECK CONSTRAINT [FK_Feedback_Usuario]
ALTER TABLE [dbo].[TB_Feedback]  WITH CHECK ADD CHECK  (([Calificacion]>=(1) AND [Calificacion]<=(5)))
