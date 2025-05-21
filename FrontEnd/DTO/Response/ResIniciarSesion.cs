using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriApp.DTO
{
    public class ResIniciarSesion : ResBase
    {
        //[JsonProperty("Token")]
        public string token { get; set; }

        //[JsonProperty("exito")]
        public bool Exito { get; set; }

        //[JsonProperty("mensaje")]
        public List<string> Mensaje { get; set; }
    }
}
