using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
    public class ReqConsultarFeedback
    {
        public int? UsuarioID { get; set; }
        public int? ProductoID { get; set; }
    }
}
