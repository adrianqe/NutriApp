/****** Object:  Procedure [dbo].[SP_Buscar_Producto_Por_Nombre]    Committed by VersionSQL https://www.versionsql.com ******/

-- Procedimiento para buscar producto por nombre
CREATE PROCEDURE [SP_Buscar_Producto_Por_Nombre]
    @Nombre NVARCHAR(100),
    @Exito BIT OUTPUT,
    @Mensaje NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Exito = 0;
    SET @Mensaje = '';

    BEGIN TRY
        -- Buscar el producto por nombre
        SELECT Producto_ID, Nombre, Categoria, Marca, Informacion_Nutricional 
        FROM TB_Productos 
        WHERE Nombre LIKE '%' + @Nombre + '%';

        SET @Exito = 1;
        SET @Mensaje = 'Búsqueda realizada exitosamente.';
    END TRY
    BEGIN CATCH
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA)
        VALUES (ERROR_SEVERITY(), 'SP_Buscar_Producto_Por_Nombre', ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());

        SET @Exito = 0;
        SET @Mensaje = 'Error al buscar el producto.';
    END CATCH
END;
