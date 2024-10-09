using backEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using backEnd.Logica;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class escanerController : ApiController
    {
        private static readonly HttpClient client = new HttpClient();

        // POST api/escanear/producto
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/escanear/producto")]
        public async Task<IHttpActionResult> Post(ReqEscanearCodigo req)
        {
            // Realizar la solicitud a OpenFoodFacts
            var product = await ObtenerProductoDeOpenFoodFacts(req.codigoBarras.Codigo_Barras);

            if (product == null)
            {
                return NotFound(); // Producto no encontrado en OpenFoodFacts
            }

            // Lógica adicional para procesar el producto (si es necesario)
            return Ok(product);
        }

        // Método que consulta OpenFoodFacts
        private async Task<dynamic> ObtenerProductoDeOpenFoodFacts(string codigoBarras)
        {
            try
            {
                // URL de la API de OpenFoodFacts
                string url = $"https://world.openfoodfacts.org/api/v0/product/{codigoBarras}.json";

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Leer el contenido de la respuesta
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Parsear el JSON a un objeto dinámico o a un tipo de datos que definas
                var productData = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                // Retornar el producto
                if (productData != null && productData.status == 1)
                {
                    return productData.product;
                }
                return null;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (puedes agregar más lógica aquí para registrar errores)
                Console.WriteLine(ex.Message);
                return null;
            }






            //Este deberia funcionar en caso de que el otro se caiga
            //Post api/values
            [System.Web.Http.HttpPost]
            [System.Web.Http.Route("api/escanear/producto")]
            public ResEscanearCodigo Post(ReqEscanearCodigo req)
            {
                return new LogCodigoBarras().escanear(req);
            }
            
        }
    }
}