/****** Object:  Procedure [dbo].[SP_Registrar_Nuevo_Usuario]    Committed by VersionSQL https://www.versionsql.com ******/

--		Procedimiento para registrar un nuevo usuario
CREATE PROCEDURE [SP_Registrar_Nuevo_Usuario]
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
        IF NOT EXISTS (SELECT 1 FROM TB_Usuarios WHERE Email = @Email)
        BEGIN
            INSERT INTO TB_Usuarios (Nombre, Email, Password_Hash)
            VALUES (@Nombre, @Email, @Password_Hash);

            SET @Exito = 1;
            SET @Mensaje = 'Usuario registrado exitosamente.';
        END
        ELSE
        BEGIN
            SET @Exito = 0;
            SET @Mensaje = 'El email ya esta registrado.';
        END
    END TRY
    BEGIN CATCH
        DECLARE @ProcedureName NVARCHAR(255) = 'SP_Registrar_Nuevo_Usuario';
        INSERT INTO TB_Errores (SEVERIDAD, STORE_PROCEDURE, NUMERO, DESCRIPCION, LINEA)
        VALUES (ERROR_SEVERITY(), @ProcedureName, ERROR_NUMBER(), ERROR_MESSAGE(), ERROR_LINE());

        SET @Exito = 0;
        SET @Mensaje = 'Error al registrar el usuario.';
    END CATCH
END;
