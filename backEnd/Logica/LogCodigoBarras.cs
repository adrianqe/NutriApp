using backEnd.DataAccess;
using backEnd.Entidades;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace backEnd.Logica
{
    public class LogCodigoBarras
    {
        private static readonly HttpClient client = new HttpClient(); // Se crea un cliente HTTP para hacer las solicitudes a OpenFoodFacts
        private NutriScore NutriScore = new NutriScore();

        public async Task<ResEscanearCodigo> escanear(ReqEscanearCodigo req) // Se crea  un método asíncrono para escanear el código de barras
        {
            ResEscanearCodigo res = new ResEscanearCodigo();
            CodigoBarras producto = null;

            producto = await ObtenerProductoDeOpenFoodFacts(req.Codigo_Barras); // Se obtiene el producto de la API OpenFoodFacts

            try
            {
                if (producto == null)
                {
                    res.exito = false;
                    res.mensaje.Add("Producto no encontrado");
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
                        producto.Ingredientes,
                        ref exito,
                        ref mensaje
                    ).ToList();

                    // Validar si el producto fue insertado o ya existía
                    if (exito == true && productoEscaneado.Any())
                    {
                        res.exito = true;

                        // Registrar en el historial el producto escaneado
                        int productoID = productoEscaneado.First().Producto_ID;
                        bool? exitoHistorial = false;
                        string mensajeHistorial = "";
                        miLinq.SP_HistorialUsuario(req.UsuarioID, productoID, ref exitoHistorial, ref mensajeHistorial);

                        // Agregar el mensaje del historial a la respuesta si falló
                        if (exitoHistorial == false)
                        {
                            res.exito = false;
                            res.mensaje.Add(mensajeHistorial);
                        }

                        // Mapear y detectar alérgenos
                        miLinq.SP_HistorialUsuario(1, productoID, ref exitoHistorial, ref mensajeHistorial); // El 1 es el ID del usuario, este valor debe ser dinámico

                        // Mapear el resultado a la respuesta
                        foreach (SP_Escanear_CodigoResult unProductoEscaneado in productoEscaneado)
                        {
                            var codigoBarras = factoriaCodigoBarras(unProductoEscaneado);

                            // Detectar alérgenos para este producto
                            codigoBarras.alergenos = DetectarAlergias(req.UsuarioID, unProductoEscaneado.Ingredientes);

                            // Añadir el producto con alérgenos detectados a la respuesta
                            res.codigoBarras.Add(codigoBarras);
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
            productoFabricado.Ingredientes = productoLinq.Ingredientes;
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
                        nutri_score = NutriScore.CalcularCalificacion(JsonConvert.SerializeObject(productData.product.nutriments)),
                        Ingredientes = productData.product.ingredients_text != null ? productData.product.ingredients_text.ToString() : ""
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


        public List<string> DetectarAlergias(int userID, string ingredientes)
        {
            var alergiasDetectadas = new List<string>();

            if (string.IsNullOrEmpty(ingredientes))
                return alergiasDetectadas; // Si no hay ingredientes, devuelve una lista vacía

            // Convertir los ingredientes a minúsculas para garantizar insensibilidad a mayúsculas
            ingredientes = ingredientes.ToLowerInvariant();

            using (var context = new ConectionDataContext())
            {
                // Obtener las alergias del usuario
                var alergiasUsuario = context.SP_Consultar_Usuario_Alergias(userID);

                foreach (var alergia in alergiasUsuario)
                {
                    // Dividir las palabras clave por comas
                    var palabrasClave = alergia.Palabras_Clave.Split(',');

                    foreach (var palabraClave in palabrasClave)
                    {
                        // Limpiar espacios en blanco y convertir a minúsculas
                        var palabra = palabraClave.Trim().ToLowerInvariant();

                        // Verificar si la palabra clave está en los ingredientes
                        if (!string.IsNullOrEmpty(palabra) && ingredientes.Contains(palabra))
                        {
                            alergiasDetectadas.Add(alergia.Nombre); // Agregar el nombre de la alergia detectada
                            break; // Detener la búsqueda para esta alergia si se encuentra una palabra clave
                        }
                    }
                }
            }

            return alergiasDetectadas;
        }
    }
}
