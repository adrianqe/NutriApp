using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
    public class ReqInsertarUsuario
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
