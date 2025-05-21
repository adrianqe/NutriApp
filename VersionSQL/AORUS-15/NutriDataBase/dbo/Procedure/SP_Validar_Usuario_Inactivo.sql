/****** Object:  Procedure [dbo].[SP_Validar_Usuario_Inactivo]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE PROCEDURE [SP_Validar_Usuario_Inactivo]
    @Email NVARCHAR(150),
    @EsInactivo BIT OUTPUT,
    @Exito BIT OUTPUT,
    @Mensaje NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @EsInactivo = 0;  -- Inicializa como no inactivo
    SET @Exito = 0;  -- Inicializa el estado de éxito
    SET @Mensaje = '';  -- Inicializa el mensaje

    BEGIN TRY
        -- Verificar si el usuario existe
        IF EXISTS (
            SELECT 1 
            FROM TB_Usuarios
            WHERE Email = @Email
        )
        BEGIN
            -- Obtener el estado de EsActivo
            DECLARE @EsActivo BIT;
            SELECT @EsActivo = EsActivo
            FROM TB_Usuarios
            WHERE Email = @Email;

            -- Si está activo, EsInactivo será 0, si no, será 1
            IF @EsActivo = 0
            BEGIN
                SET @EsInactivo = 1;  -- Usuario inactivo
                SET @Mensaje = 'El usuario está inactivo.';
				SET @Exito = 1;
            END
            ELSE
            BEGIN
                SET @EsInactivo = 0;  -- Usuario activo
                SET @Mensaje = 'El usuario está activo.';
				SET @Exito = 0;
            END
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
        VALUES (ERROR_SEVERITY(), 'SP_Validar_Usuario_Inactivo', ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());

        SET @Exito = 0;  -- Marca como error
        SET @Mensaje = 'Error al intentar validar el estado del usuario.';
    END CATCH
END;
