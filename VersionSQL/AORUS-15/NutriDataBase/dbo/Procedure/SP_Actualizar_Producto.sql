/****** Object:  Procedure [dbo].[SP_Actualizar_Producto]    Committed by VersionSQL https://www.versionsql.com ******/

--		Actualizar informacion del producto
CREATE PROCEDURE [SP_Actualizar_Producto]
    @Producto_ID INT,
    @Nombre NVARCHAR(100),
    @Categoria NVARCHAR(50),
    @Marca NVARCHAR(50),
    @Informacion_Nutricional NVARCHAR(MAX),
	@Ingredientes NVARCHAR(MAX) = NULL,
    @Exito BIT OUTPUT,
    @Mensaje NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Exito = 0;
    SET @Mensaje = '';

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM TB_Productos WHERE Producto_ID = @Producto_ID)
        BEGIN
            UPDATE TB_Productos
            SET Nombre = @Nombre,
                Categoria = @Categoria,
                Marca = @Marca,
                Informacion_Nutricional = @Informacion_Nutricional,
				Ingredientes = @Ingredientes
            WHERE Producto_ID = @Producto_ID;

            SET @Exito = 1;
            SET @Mensaje = 'Producto actualizado exitosamente.';
        END
        ELSE
        BEGIN
            SET @Exito = 0;
            SET @Mensaje = 'Producto no encontrado.';
        END
    END TRY
    BEGIN CATCH
        DECLARE @ProcedureName NVARCHAR(255) = 'SP_Actualizar_Producto';
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA)
        VALUES (ERROR_SEVERITY(), @ProcedureName, ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());

        SET @Exito = 0;
        SET @Mensaje = 'Error al actualizar el producto.';
    END CATCH
END;
