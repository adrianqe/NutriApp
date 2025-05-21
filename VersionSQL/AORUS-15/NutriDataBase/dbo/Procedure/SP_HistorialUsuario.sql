/****** Object:  Procedure [dbo].[SP_HistorialUsuario]    Committed by VersionSQL https://www.versionsql.com ******/

--Historial de usuario--

CREATE PROCEDURE [SP_HistorialUsuario]
    @Usuario_ID INT,
    @Producto_ID INT,
    @Exito BIT OUTPUT,
    @Mensaje NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Exito = 0; -- Inicializar como fallo
    SET @Mensaje = ''; -- Inicializar como vacío

    BEGIN TRY
        -- Insertar el registro en el historial de búsqueda
        INSERT INTO TB_Historial (Usuario_ID, Producto_ID, Fecha_Escaneo)
        VALUES (@Usuario_ID, @Producto_ID, SYSUTCDATETIME());

        -- Indicar éxito y mensaje
        SET @Exito = 1;
        SET @Mensaje = 'Historial de búsqueda registrado exitosamente.';

    END TRY
    BEGIN CATCH
        -- Capturar detalles del error
        DECLARE @ProcedureName NVARCHAR(255) = 'SP_RegistrarHistorialUsuario';
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA, FECHA_HORA)
        VALUES (ERROR_SEVERITY(), @ProcedureName, ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE(), SYSUTCDATETIME());

        -- Indicar error en las variables de salida
        SET @Exito = 0;
        SET @Mensaje = 'Error al registrar el historial de búsqueda del usuario.';
    END CATCH
END;
