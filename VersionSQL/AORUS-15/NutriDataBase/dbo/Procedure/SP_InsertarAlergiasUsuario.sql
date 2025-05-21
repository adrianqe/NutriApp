/****** Object:  Procedure [dbo].[SP_InsertarAlergiasUsuario]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE PROCEDURE [SP_InsertarAlergiasUsuario]
    @Usuario_ID INT,
    @Alergias NVARCHAR(MAX) -- Formato: '1,2,3'
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Eliminar alergias previas para evitar duplicados
        DELETE FROM TB_Usuario_Alergias WHERE Usuario_ID = @Usuario_ID;

        -- Insertar nuevas alergias
        DECLARE @xml XML = CAST('<root><id>' + REPLACE(@Alergias, ',', '</id><id>') + '</id></root>' AS XML);

        INSERT INTO TB_Usuario_Alergias (Usuario_ID, Id_Alergia)
        SELECT @Usuario_ID, T.value('.', 'INT') 
        FROM @xml.nodes('/root/id') AS X(T);

        RETURN 1; -- Éxito
    END TRY
    BEGIN CATCH
        RETURN 0; -- Error
    END CATCH
END;
