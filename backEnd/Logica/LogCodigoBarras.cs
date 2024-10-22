using backEnd.DataAccess;
using backEnd.Entidades;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace backEnd.Logica
{
    public class LogCodigoBarras
    {
        private static readonly HttpClient client = new HttpClient(); // Se crea un cliente HTTP para hacer las solicitudes a OpenFoodFacts

        public async Task<ResEscanearCodigo> escanear(ReqEscanearCodigo req)
        {
            ResEscanearCodigo res = new ResEscanearCodigo();
            CodigoBarras producto = null;

            producto = await ObtenerProductoDeOpenFoodFacts(req.Codigo_Barras);

            try
            {
                if (producto == null)
                {
                    res.exito = false;
                    res.mensaje.Add("Request nulo");
                }
                else if (string.IsNullOrEmpty(producto.Codigo_Barras))
                {
                    res.exito = false;
                    res.mensaje.Add("Código de barras vacío");
                }
                else if (string.IsNullOrEmpty(producto.Nombre))
                {
                    res.exito = false;
                    res.mensaje.Add("Nombre vacío");
                }
                else
                {
                    // Se insertan los datos obtenidos en la base de datos y se obtienen los resultados
                    bool? exito = false;
                    string mensaje = "";
                    ConectionDataContext miLinq = new ConectionDataContext();

                    // Ejecutar el procedimiento almacenado y capturar los resultados
                    var productoEscaneado = miLinq.SP_Escanear_Codigo(
                        producto.Codigo_Barras,
                        producto.Nombre,
                        producto.Categoria,
                        producto.Marca,
                        producto.Informacion_Nutricional,
                        producto.nutri_score,
                        ref exito,
                        ref mensaje
                    ).ToList(); // No uses ToString() aquí

                    // Validar si el producto fue insertado o ya existía
                    if (exito == true && productoEscaneado.Any())
                    {
                        res.exito = true;

                        // Mapear el resultado a la respuesta
                        foreach (SP_Escanear_CodigoResult unProductoEscaneado in productoEscaneado)
                        {
                            res.codigoBarras.Add(factoriaCodigoBarras(unProductoEscaneado));
                        }
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add(mensaje); // Mensaje devuelto por el SP
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message);  // Capturar cualquier excepción en la lógica
            }

            return res;
        }

        // Método para mapear el resultado del SP a la entidad CodigoBarras
        private CodigoBarras factoriaCodigoBarras(SP_Escanear_CodigoResult productoLinq)
        {
            CodigoBarras productoFabricado = new CodigoBarras();
            productoFabricado.Codigo_Barras = productoLinq.Codigo_Barras;
            productoFabricado.Nombre = productoLinq.Nombre;
            productoFabricado.Categoria = productoLinq.Categoria;
            productoFabricado.Marca = productoLinq.Marca;
            productoFabricado.Informacion_Nutricional = productoLinq.Informacion_Nutricional;
            productoFabricado.nutri_score = productoLinq.Nutri_Score;
            return productoFabricado;
        }


        // Este método se encarga de hacer la solicitud a OpenFoodFacts y obtener los datos del producto
        public async Task<CodigoBarras> ObtenerProductoDeOpenFoodFacts(string codigoBarras)
        {
            try
            {
                string url = $"https://world.openfoodfacts.org/api/v0/product/{codigoBarras}.json";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var productData = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                if (productData != null && productData.status == 1)
                {
                    CodigoBarras producto = new CodigoBarras
                    {
                        Codigo_Barras = codigoBarras,
                        Nombre = productData.product.product_name != null ? productData.product.product_name.ToString() : "",
                        Categoria = productData.product.categories != null ? productData.product.categories.ToString() : "",
                        Marca = productData.product.brands != null ? productData.product.brands.ToString() : "",
                        Informacion_Nutricional = productData.product.nutriments != null ? JsonConvert.SerializeObject(productData.product.nutriments) : "",
                        nutri_score = CalcularCalificacionNutricional(JsonConvert.SerializeObject(productData.product.nutriments))
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

        private int CalcularCalificacionNutricional(string informacionNutricionalJson)
        {
            dynamic infoNutricional = JsonConvert.DeserializeObject(informacionNutricionalJson);

            int puntaje = 0;
            int factoresConsiderados = 0;

            // Evaluar las calorías: 1 = Bueno, 5 = Malo
            if (infoNutricional["energy-kcal_value"] != null)
            {
                double calorias = infoNutricional["energy-kcal_value"];
                if (calorias < 100) puntaje += 1; // Bajo en calorías
                else if (calorias < 200) puntaje += 2;
                else if (calorias < 300) puntaje += 3;
                else if (calorias < 400) puntaje += 4;
                else puntaje += 5; // Alto en calorías
                factoresConsiderados++;
            }

            // Evaluar la grasa saturada
            if (infoNutricional["saturated-fat_value"] != null)
            {
                double grasaSaturada = infoNutricional["saturated-fat_value"];
                if (grasaSaturada < 1) puntaje += 1; // Bajo en grasa saturada
                else if (grasaSaturada < 5) puntaje += 2;
                else if (grasaSaturada < 10) puntaje += 3;
                else if (grasaSaturada < 15) puntaje += 4;
                else puntaje += 5; // Alto en grasa saturada
                factoresConsiderados++;
            }

            // Evaluar los azúcares
            if (infoNutricional["sugars_value"] != null)
            {
                double azucares = infoNutricional["sugars_value"];
                if (azucares < 5) puntaje += 1; // Bajo en azúcar
                else if (azucares < 10) puntaje += 2;
                else if (azucares < 20) puntaje += 3;
                else if (azucares < 30) puntaje += 4;
                else puntaje += 5; // Alto en azúcar
                factoresConsiderados++;
            }

            // Evaluar la sal
            if (infoNutricional["salt_value"] != null)
            {
                double sal = infoNutricional["salt_value"];
                if (sal < 0.3) puntaje += 1; // Bajo en sal
                else if (sal < 0.7) puntaje += 2;
                else if (sal < 1) puntaje += 3;
                else if (sal < 1.5) puntaje += 4;
                else puntaje += 5; // Alto en sal
                factoresConsiderados++;
            }

            // Evaluar las proteínas
            if (infoNutricional["proteins_value"] != null)
            {
                double proteinas = infoNutricional["proteins_value"];
                if (proteinas > 10) puntaje += 1; // Alto en proteínas
                else puntaje += 3; // Bajo en proteínas
                factoresConsiderados++;
            }

            // Evaluar la fibra
            if (infoNutricional["fiber_value"] != null)
            {
                double fibra = infoNutricional["fiber_value"];
                if (fibra > 5) puntaje += 1; // Alto en fibra
                else puntaje += 3; // Bajo en fibra
                factoresConsiderados++;
            }

            // Calcular el promedio de la puntuación
            if (factoresConsiderados > 0)
            {
                double promedio = (double)puntaje / factoresConsiderados;

                // Asegurar que el puntaje esté entre 1 y 5
                return Math.Max(1, Math.Min(5, (int)Math.Ceiling(promedio)));
            }

            // Si no se evaluaron factores, devolver un puntaje neutro de 3
            return 3;
        }

    }
}
