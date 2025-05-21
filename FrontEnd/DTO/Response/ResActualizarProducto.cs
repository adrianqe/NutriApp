using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriApp.DTO
{
    public class ResActualizarProducto
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public Producto ProductoActualizado { get; set; }
    }
}
