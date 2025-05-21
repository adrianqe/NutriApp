using Newtonsoft.Json;

namespace NutriApp.DTO
{
    public class Producto
    {
        [JsonProperty("Codigo_Barras")]
        public string Codigo_Barras { get; set; }

        [JsonProperty("Nombre")]
        public string Nombre { get; set; }

        [JsonProperty("Categoria")]
        public string Categoria { get; set; }

        [JsonProperty("Marca")]
        public string Marca { get; set; }

        [JsonProperty("Informacion_Nutricional")]
        public string InformacionNutricional { get; set; }

        [JsonProperty("nutri_score")]
        public int NutriScore { get; set; }

        [JsonProperty("Ingredientes")]
        public string Ingredientes { get; set; }

        [JsonProperty("alergenos")]
        public List<string> Alergenos { get; set; }
        [JsonProperty("Fecha_Escaneo")]
        public DateTime FechaEscaneo { get; set; }
        public ImageSource ImagenProducto { get; set; }
        public string Imagen { get; set; }
        public int productoID { get; set; }
    }
}
