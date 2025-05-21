using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace backEnd.Entidades
{
    public class ImageService
    {
        private const string ApiKey = "----"; // Cambiar a la clave de API generada por Google
        private const string SearchEngineId = "---"; // Cambiar al ID del motor de búsqueda personalizado
        private const string BaseUrl = "https://www.googleapis.com/customsearch/v1";

        public async Task<string> BuscarImagenAsync(string nombre, string marca)
        {
            try
            {
                string query = $"{nombre} {marca}";
                string url = $"{BaseUrl}?q={Uri.EscapeDataString(query)}&cx={SearchEngineId}&searchType=image&key={ApiKey}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(jsonResponse);

                    // Obtén el primer resultado de la búsqueda
                    var imageUrl = data["items"]?[0]?["link"]?.ToString();
                    return imageUrl ?? "No se encontraron imágenes";
                }
            }
            catch (Exception ex)
            {
                return $"Error al buscar la imagen: {ex.Message}";
            }
        }
    }
}
