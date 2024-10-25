using System.Collections.Generic;
using backEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace backEnd.Response
{
    public class ResIniciarSesionUsuario : ResBase
    {
        public Usuario Usuario { get; set; }
        // public string Token { get; set; }     Cuesta Mucho, ocupo mas tiempo <3 despues lo hago

        public ResIniciarSesionUsuario()
        {
            Usuario = null;          
          //  Token = string.Empty;    Cuesta Mucho, ocupo mas tiempo <3
        }
    }
}
