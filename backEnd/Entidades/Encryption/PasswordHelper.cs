using System;
using System.Security.Cryptography;
using System.Text;

public class PasswordHelper
{
    public static string HashContraseña(string contraseña)
    {
        // Generar un salt aleatorio
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        // Derivar el hash de la contraseña
        var pbkdf2 = new Rfc2898DeriveBytes(contraseña, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(20); // Generar el hash de 20 bytes

        // Combinar el salt y el hash
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        // Convertir a Base64 para almacenarlo
        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerificarContraseña(string contraseña, string hashedPassword)
    {
        // Convertir el hash almacenado de Base64 a bytes
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);

        // Recuperar el salt
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        // Derivar el hash de la contraseña ingresada
        var pbkdf2 = new Rfc2898DeriveBytes(contraseña, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(20);

        // Comparar el hash generado con el almacenado
        for (int i = 0; i < 20; i++)
        {
            if (hashBytes[i + 16] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}