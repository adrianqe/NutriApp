using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriApp.DTO
{
    public class JwtPayload
    {
        public string unique_name { get; set; } // "nombre de usuario"
        public string email { get; set; } // "email"
        public string nameid { get; set; } // "UsuarioID" (cambiado de nameId a nameid)
        public string CodigoVerificacion { get; set; } // "codigo de verificacion"
        public long nbf { get; set; } // "nbf" (not before)
        public long exp { get; set; } // "exp" (expiration)
        public long iat { get; set; } // "iat" (issued at)
        public string iss { get; set; } // "iss" (issuer)
        public string aud { get; set; } // "aud" (audience)
    }
}
