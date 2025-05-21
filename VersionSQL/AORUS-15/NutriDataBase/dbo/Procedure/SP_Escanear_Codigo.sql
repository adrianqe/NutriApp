/****** Object:  Procedure [dbo].[SP_Escanear_Codigo]    Committed by VersionSQL https://www.versionsql.com ******/

/*			STORE PROCEDURES		*/

-- Procedimiento para escanear un codigo de barras y devolver el producto
CREATE PROCEDURE [SP_Escanear_Codigo]
    @Codigo_Barras NVARCHAR(50),
    @Nombre NVARCHAR(100) = NULL, 
    @Categoria NVARCHAR(50) = NULL, 
    @Marca NVARCHAR(50) = NULL, 
    @Informacion_Nutricional NVARCHAR(MAX) = NULL,
    @Nutri_Score INT = NULL,
    @Ingredientes NVARCHAR(MAX) = NULL,  -- Nuevo parámetro para ingredientes
    @Exito BIT OUTPUT,          
    @Mensaje NVARCHAR(255) OUTPUT 
AS
BEGIN
    SET NOCOUNT ON;
    SET @Exito = 0;
    SET @Mensaje = '';

    BEGIN TRY
        DECLARE @Producto_ID INT;

        IF NOT EXISTS (SELECT 1 FROM TB_Productos WHERE Codigo_Barras = @Codigo_Barras)
        BEGIN
            -- Insertar nuevo producto
            INSERT INTO TB_Productos (Nombre, Codigo_Barras, Categoria, Marca, Informacion_Nutricional, Nutri_Score, Ingredientes)
            VALUES (@Nombre, @Codigo_Barras, @Categoria, @Marca, @Informacion_Nutricional, @Nutri_Score, @Ingredientes);

            -- Obtener el ID del producto recién insertado
            SET @Producto_ID = SCOPE_IDENTITY();
            SET @Exito = 1;
            SET @Mensaje = 'Producto insertado exitosamente.';
        END
        ELSE
        BEGIN
            -- Si el producto ya existe, obtener su ID
            SELECT @Producto_ID = Producto_ID
            FROM TB_Productos
            WHERE Codigo_Barras = @Codigo_Barras;

            SET @Exito = 1;
            SET @Mensaje = 'El producto ya existe.';
        END

        -- Retornar el producto insertado o existente, incluyendo el ID
        SELECT Producto_ID, Nombre, Codigo_Barras, Categoria, Marca, Informacion_Nutricional, Nutri_Score, Ingredientes
        FROM TB_Productos 
        WHERE Producto_ID = @Producto_ID;
        
    END TRY
    BEGIN CATCH
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA)
        VALUES (ERROR_SEVERITY(), 'SP_Escanear_Codigo', ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());

        SET @Exito = 0;
        SET @Mensaje = 'Error al escanear el código de barras.';
    END CATCH
END;
