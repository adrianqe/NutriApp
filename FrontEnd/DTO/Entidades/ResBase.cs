using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriApp.DTO
{
    public class ResBase
    {
        public bool exito { get; set; } // Indica si la operación fue exitosa.
        public List<string> mensaje { get; set; } // Mensaje descriptivo del resultado.
    }
}
