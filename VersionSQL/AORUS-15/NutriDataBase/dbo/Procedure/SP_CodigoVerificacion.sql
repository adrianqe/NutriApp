/****** Object:  Procedure [dbo].[SP_CodigoVerificacion]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE PROCEDURE [SP_CodigoVerificacion]
    @Email NVARCHAR(255),
    @cod_verificacion INT,
    @Exito BIT OUTPUT,
    @Mensaje NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Exito = 0;  -- Inicializa el estado de éxito
    SET @Mensaje = '';  -- Inicializa el mensaje

    BEGIN TRY
        -- Verificar si el código y el correo coinciden
        IF EXISTS (
            SELECT 1 
            FROM TB_Usuarios 
            WHERE Email = @Email 
              AND  cod_verificacion =  @cod_verificacion
        )
        BEGIN
            SET @Exito = 1;  -- Código válido
            SET @Mensaje = 'Código de verificación válido.';
        END
        ELSE
        BEGIN
            SET @Exito = 0;  -- Código inválido
            SET @Mensaje = 'Código de verificación inválido.';
        END
    END TRY
    BEGIN CATCH
        -- Manejo de errores
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA)
        VALUES (ERROR_SEVERITY(), 'sp_CodigoVerificacion', ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());

        SET @Exito = 0;  -- Marca como error
        SET @Mensaje = 'Error al verificar el código de verificación.';
    END CATCH
END;
