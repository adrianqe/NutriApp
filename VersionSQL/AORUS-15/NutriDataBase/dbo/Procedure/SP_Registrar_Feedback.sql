/****** Object:  Procedure [dbo].[SP_Registrar_Feedback]    Committed by VersionSQL https://www.versionsql.com ******/

--		Procedimiento para registrar una resenia de usuario (Feedback)
CREATE PROCEDURE [SP_Registrar_Feedback]
    @Usuario_ID INT,
    @Producto_ID INT,
    @Calificacion INT,
    @Comentario NVARCHAR(500),
    @Exito BIT OUTPUT,
    @Mensaje NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Exito = 0;
    SET @Mensaje = '';

    BEGIN TRY
        -- Verificar que la calificaci n est  en el rango permitido
        IF @Calificacion >= 1 AND @Calificacion <= 5
        BEGIN
            -- Verificar si el usuario y el producto existen
            IF EXISTS (SELECT 1 FROM TB_Usuarios WHERE Usuario_ID = @Usuario_ID) AND
               EXISTS (SELECT 1 FROM TB_Productos WHERE Producto_ID = @Producto_ID)
            BEGIN
                -- Registrar la rese a
                INSERT INTO TB_Feedback (Usuario_ID, Producto_ID, Calificacion, Comentario)
                VALUES (@Usuario_ID, @Producto_ID, @Calificacion, @Comentario);

                SET @Exito = 1;
                SET @Mensaje = 'Resenia registrada exitosamente.';
            END
            ELSE
            BEGIN
                SET @Exito = 0;
                SET @Mensaje = 'Usuario o Producto no existen.';
            END
        END
        ELSE
        BEGIN
            SET @Exito = 0;
            SET @Mensaje = 'La calificacion debe estar entre 1 y 5.';
        END
    END TRY
    BEGIN CATCH
        -- Manejo de errores
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA)
        VALUES (ERROR_SEVERITY(), 'SP_Registrar_Feedback', ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());

        SET @Exito = 0;
        SET @Mensaje = 'Error al registrar el Feedback.';
    END CATCH
END;
