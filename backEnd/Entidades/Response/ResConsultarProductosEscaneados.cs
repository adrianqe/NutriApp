using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
    public class ResConsultarProductosEscaneados : ResBase
    {
        public List<Producto> ProductosEscaneados = new List<Producto>();
    }
}
