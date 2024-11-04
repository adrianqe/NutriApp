/****** Object:  Procedure [dbo].[SP_Actualizar_Usuario]    Committed by VersionSQL https://www.versionsql.com ******/

--		Actualizar informacion del Usuario
CREATE PROCEDURE [SP_Actualizar_Usuario]
    @Usuario_ID INT,
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(150),
    @Password_Hash NVARCHAR(255),
    @Exito BIT OUTPUT,
    @Mensaje NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Exito = 0;
    SET @Mensaje = '';

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM TB_Usuarios WHERE Usuario_ID = @Usuario_ID)
        BEGIN
            UPDATE TB_Usuarios
            SET Nombre = @Nombre,
                Email = @Email,
                Password_Hash = @Password_Hash
            WHERE Usuario_ID = @Usuario_ID;

            SET @Exito = 1;
            SET @Mensaje = 'Usuario actualizado exitosamente.';
        END
        ELSE
        BEGIN
            SET @Exito = 0;
            SET @Mensaje = 'Usuario no encontrado.';
        END
    END TRY
    BEGIN CATCH
        DECLARE @ProcedureName NVARCHAR(255) = 'SP_Actualizar_Usuario';
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA)
        VALUES (ERROR_SEVERITY(), @ProcedureName, ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());

        SET @Exito = 0;
        SET @Mensaje = 'Error al actualizar el usuario.';
    END CATCH
END;
