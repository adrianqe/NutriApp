using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
    public class ResBuscarProducto : ResBase
    {
        public List<CodigoBarras> ProductosEncontrados { get; set; } = new List<CodigoBarras>();
    }
}
