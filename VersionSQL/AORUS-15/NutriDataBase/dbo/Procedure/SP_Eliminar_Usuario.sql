/****** Object:  Procedure [dbo].[SP_Eliminar_Usuario]    Committed by VersionSQL https://www.versionsql.com ******/

--		Eliminar Usuarios
CREATE PROCEDURE [SP_Eliminar_Usuario]
    @Usuario_ID INT,
    @Exito BIT OUTPUT,
    @Mensaje NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Exito = 0;
    SET @Mensaje = '';

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM TB_Usuarios WHERE Usuario_ID = @Usuario_ID)
        BEGIN
            DELETE FROM TB_Usuarios
            WHERE Usuario_ID = @Usuario_ID;

            SET @Exito = 1;
            SET @Mensaje = 'Usuario eliminado exitosamente.';
        END
        ELSE
        BEGIN
            SET @Exito = 0;
            SET @Mensaje = 'Usuario no encontrado.';
        END
    END TRY
    BEGIN CATCH
        DECLARE @ProcedureName NVARCHAR(255) = 'SP_Eliminar_Usuario';
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA)
        VALUES (ERROR_SEVERITY(), @ProcedureName, ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());

        SET @Exito = 0;
        SET @Mensaje = 'Error al eliminar el usuario.';
    END CATCH
END;
