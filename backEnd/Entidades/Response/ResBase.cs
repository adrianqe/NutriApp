using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
    public class ResBase
    {
        public bool exito { get; set; }
        public List<string> mensaje { get; set; } = new List<string>();
    }
}
