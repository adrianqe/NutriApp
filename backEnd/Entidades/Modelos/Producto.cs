using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
    public class Producto
    {
        public int? Producto_ID { get; set; } 
        public string Nombre { get; set; } 
        public string Categoria { get; set; } 
        public string Marca { get; set; } 
        public string Informacion_Nutricional { get; set; }
    }
}
