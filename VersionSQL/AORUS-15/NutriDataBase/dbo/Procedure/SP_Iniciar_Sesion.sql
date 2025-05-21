/****** Object:  Procedure [dbo].[SP_Iniciar_Sesion]    Committed by VersionSQL https://www.versionsql.com ******/

-- Procedimiento para el inicio de sesión

CREATE PROCEDURE [SP_Iniciar_Sesion]
    @Email NVARCHAR(150),
    @Password_Hash NVARCHAR(255) OUTPUT,
    @Exito BIT OUTPUT,
    @Usuario_ID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Exito = 0;
 
    BEGIN TRY
        -- Verificar si el usuario existe y está activo
        IF EXISTS (
            SELECT 1
            FROM TB_Usuarios
            WHERE Email = @Email
              AND EsActivo = 1 -- Validar que el usuario esté activo
        )
        BEGIN
            SELECT 
                @Password_Hash = Password_Hash,
                @Usuario_ID = Usuario_ID -- Asignar Usuario_ID
            FROM TB_Usuarios
            WHERE Email = @Email
              AND EsActivo = 1; -- Validar que el usuario esté activo
 
            SET @Exito = 1; -- Usuario encontrado y activo
        END
        ELSE
        BEGIN
            SET @Exito = 0; -- Usuario no encontrado o inactivo
        END
    END TRY
    BEGIN CATCH
        -- Manejo de errores
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA)
        VALUES (ERROR_SEVERITY(), 'SP_Iniciar_Sesion', ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());
    END CATCH
END;
