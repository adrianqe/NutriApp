using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriApp.DTO
{
    public class ResBuscarProducto
    {
        public List<Producto> ProductosEncontrados { get; set; } = new List<Producto>();
        public bool Exito { get; set; }
        public List<string> Mensaje { get; set; } = new List<string>();
    }
}
