/****** Object:  Table [dbo].[TB_Usuario_Alergias]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE TABLE [dbo].[TB_Usuario_Alergias](
	[Usuario_ID] [int] NOT NULL,
	[Id_Alergia] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Usuario_ID] ASC,
	[Id_Alergia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[TB_Usuario_Alergias]  WITH CHECK ADD FOREIGN KEY([Id_Alergia])
REFERENCES [dbo].[cat_Alergias] ([Id_Alergia])
ALTER TABLE [dbo].[TB_Usuario_Alergias]  WITH CHECK ADD FOREIGN KEY([Usuario_ID])
REFERENCES [dbo].[TB_Usuarios] ([Usuario_ID])
ALTER TABLE [dbo].[TB_Usuario_Alergias]  WITH CHECK ADD  CONSTRAINT [FK_TB_Usuario_Alergias_Usuario] FOREIGN KEY([Usuario_ID])
REFERENCES [dbo].[TB_Usuarios] ([Usuario_ID])
ON DELETE CASCADE
ALTER TABLE [dbo].[TB_Usuario_Alergias] CHECK CONSTRAINT [FK_TB_Usuario_Alergias_Usuario]
