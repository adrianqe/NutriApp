/****** Object:  Procedure [dbo].[SP_Consultar_Usuario_Alergias]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE PROCEDURE [SP_Consultar_Usuario_Alergias]
    @Usuario_ID INT
AS
BEGIN
    SELECT a.Nombre, a.Palabras_Clave
    FROM TB_Usuario_Alergias ua
    INNER JOIN cat_Alergias a ON ua.Id_Alergia = a.Id_Alergia
    WHERE ua.Usuario_ID = @Usuario_ID;
END;
