using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NutriApp.DTO
{
    public class ResRegistrar : ResBase
    {
        [JsonProperty("exito")]
        public bool Exito { get; set; }

        [JsonProperty("mensaje")]
        public List<string> Mensaje { get; set; } = new List<string>();
        [JsonProperty("Token")]
        public string Token { get; set; } = "";
    }
}
