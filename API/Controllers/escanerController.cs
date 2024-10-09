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
        //API De OpenFoodFacts para la consulta de productos mediante el código de barras
        private static readonly HttpClient client = new HttpClient();

        // POST api/escanear/producto
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/escanear/producto")]
        public async Task<IHttpActionResult> Post(ReqEscanearCodigo req)
        {
            if (string.IsNullOrEmpty(req.Codigo_Barras))
            {
                return BadRequest("El código de barras no puede estar vacío.");
            }

            // Realizar la solicitud a OpenFoodFacts con el código de barras
            var product = await ObtenerProductoDeOpenFoodFacts(req.Codigo_Barras);

            if (product == null)
            {
                return NotFound(); // Producto no encontrado en OpenFoodFacts
            }

            // Aquí puedes pasar los datos obtenidos a la lógica para guardarlos en la base de datos
            var res = new LogCodigoBarras().escanear(product);

            return Ok(res); // Retornar el resultado del proceso
        }


        // Método que consulta OpenFoodFacts
        private async Task<CodigoBarras> ObtenerProductoDeOpenFoodFacts(string codigoBarras)
        {
            try
            {
                // URL de la API de OpenFoodFacts
                string url = $"https://world.openfoodfacts.org/api/v0/product/{codigoBarras}.json";

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Leer el contenido de la respuesta
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Parsear el JSON y mapearlo a la clase CodigoBarras
                var productData = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                if (productData != null && productData.status == 1)
                {
                    // Crear una instancia de CodigoBarras y mapear los valores
                    CodigoBarras producto = new CodigoBarras
                    {
                        Codigo_Barras = codigoBarras,
                        Nombre = productData.product.product_name != null ? productData.product.product_name.ToString() : "",
                        Categoria = productData.product.categories != null ? productData.product.categories.ToString() : "",
                        Marca = productData.product.brands != null ? productData.product.brands.ToString() : "",
                        Informacion_Nutricional = productData.product.nutriments != null ? JsonConvert.SerializeObject(productData.product.nutriments) : ""
                    };

                    return producto;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
