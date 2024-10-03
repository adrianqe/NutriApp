using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades.Response
{
    public class ResConsultarProductosEscaneados : ResBase
    {
        public List<Producto> ProductosEscaneados { get; set; } 
    }
}
