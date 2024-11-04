/****** Object:  Procedure [dbo].[SP_Iniciar_Sesion]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE PROCEDURE [SP_Iniciar_Sesion]
    @Email NVARCHAR(150),
    @Password_Hash NVARCHAR(255) OUTPUT,
    @Exito BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Exito = 0;

    BEGIN TRY
        -- Verificar si el usuario existe
        IF EXISTS (
            SELECT 1
            FROM TB_Usuarios 
            WHERE Email = @Email
        )
        BEGIN
            -- Devolver el hash almacenado
            SELECT @Password_Hash = Password_Hash
            FROM TB_Usuarios
            WHERE Email = @Email;
            
            SET @Exito = 1; -- Usuario encontrado
        END
    END TRY
    BEGIN CATCH
        -- Capturar cualquier error
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA)
        VALUES (ERROR_SEVERITY(), 'SP_Iniciar_Sesion', ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());
    END CATCH
END;
