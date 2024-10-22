using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
    public class CodigoBarras
    {
        public string Codigo_Barras { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Marca { get; set; }
        public string Informacion_Nutricional { get; set; }
        public int? nutri_score { get; set; }
    }
}
