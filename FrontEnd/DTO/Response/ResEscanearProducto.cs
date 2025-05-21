using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriApp.DTO
{
    public class ResEscanearProducto
    {
        public List<Producto> CodigoBarras { get; set; } = new List<Producto>();
        public bool exito { get; set; }
        public List<string> mensaje { get; set; } = new List<string>();
    }

}
