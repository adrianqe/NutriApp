/****** Object:  Procedure [dbo].[SP_Activar_Usuario]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE PROCEDURE [SP_Activar_Usuario]
    @Email NVARCHAR(150),
    @Exito BIT OUTPUT,
    @Mensaje NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Exito = 0;  -- Inicializa el estado de éxito
    SET @Mensaje = '';  -- Inicializa el mensaje

    BEGIN TRY
        -- Verificar si el usuario con el email existe
        IF EXISTS (
            SELECT 1
            FROM TB_Usuarios
            WHERE Email = @Email
        )
        BEGIN
            -- Actualizar el estado del usuario a activo
            UPDATE TB_Usuarios
            SET EsActivo = 1
            WHERE Email = @Email;

            SET @Exito = 1;  -- Operación exitosa
            SET @Mensaje = 'Usuario activado correctamente.';
        END
        ELSE
        BEGIN
            SET @Exito = 0;  -- Usuario no encontrado
            SET @Mensaje = 'Usuario no encontrado.';
        END
    END TRY
    BEGIN CATCH
        -- Manejo de errores
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA)
        VALUES (ERROR_SEVERITY(), 'SP_Activar_Usuario', ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());

        SET @Exito = 0;  -- Marca como error
        SET @Mensaje = 'Error al intentar activar el usuario.';
    END CATCH
END;
