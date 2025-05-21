/****** Object:  Procedure [dbo].[SP_ObtenerHistorialEscaneos]    Committed by VersionSQL https://www.versionsql.com ******/

CREATE PROCEDURE [SP_ObtenerHistorialEscaneos]
    @Usuario_ID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Seleccionar productos escaneados por el usuario
        SELECT 
            P.Producto_ID,
            P.Codigo_Barras,
            P.Nombre,
            P.Categoria,
            P.Marca,
            P.Informacion_Nutricional,
            P.nutri_score,
            P.Ingredientes,
            H.Fecha_Escaneo
        FROM 
            TB_Historial H
        INNER JOIN 
            TB_Productos P ON H.Producto_ID = P.Producto_ID
        WHERE 
            H.Usuario_ID = @Usuario_ID
        ORDER BY 
            H.Fecha_Escaneo DESC; -- Ordenar por fecha de escaneo (más reciente primero)

    END TRY
    BEGIN CATCH
        -- Capturar detalles del error
        DECLARE @ProcedureName NVARCHAR(255) = 'SP_ObtenerHistorialEscaneos';
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA, FECHA_HORA)
        VALUES (ERROR_SEVERITY(), @ProcedureName, ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE(), SYSUTCDATETIME());
        
        -- Manejar el error con un mensaje general
        RAISERROR('Error al obtener el historial de escaneos del usuario.', 16, 1);
    END CATCH
END;
