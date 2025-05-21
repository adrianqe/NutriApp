using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace NutriApp.DTO
{
    public class JwtHelper
    {
        public static JwtPayload DecodeToken(string jwtToken)
        {
            // Separar las partes del token
            var parts = jwtToken.Split('.');
            if (parts.Length < 2)
            {
                throw new ArgumentException("El token JWT no tiene un formato válido.");
            }

            // Obtener el payload (segunda parte del token)
            string payloadBase64 = parts[1];

            // Corregir el formato Base64 URL
            payloadBase64 = payloadBase64.Replace('-', '+').Replace('_', '/');
            switch (payloadBase64.Length % 4)
            {
                case 2: payloadBase64 += "=="; break;
                case 3: payloadBase64 += "="; break;
            }

            // Decodificar Base64 a JSON
            string jsonPayload = Encoding.UTF8.GetString(Convert.FromBase64String(payloadBase64));

            // Deserializar JSON a JwtPayload
            return JsonSerializer.Deserialize<JwtPayload>(jsonPayload);
        }
    }
}
