using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriApp.DTO
{
    public class ReqEscanearProducto
    {
        [JsonProperty("Codigo_Barras")]
        public string? Codigo_Barras { get; set; }

        [JsonProperty("UsuarioID")]
        public int UsuarioID { get; set; }
    }
}
