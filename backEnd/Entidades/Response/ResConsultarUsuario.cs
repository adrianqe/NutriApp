using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace backEnd.Entidades
{
    public class ResConsultarUsuario : ResBase
    {
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
