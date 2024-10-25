using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backEnd.Entidades;

namespace backEnd.Request
{
    public class ReqIniciarSesionUsuario
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
