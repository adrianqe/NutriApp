using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
    public class ResEscanearCodigo : ResBase
    {
        public List<CodigoBarras> codigoBarras { get; set; } = new List<CodigoBarras>();
    }
}
