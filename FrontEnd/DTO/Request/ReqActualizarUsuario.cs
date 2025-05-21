using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriApp.DTO
{
    public class ReqActualizarUsuario
    {
        public int? Usuario_ID { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
