using backEnd.DataAccess;
using backEnd.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Logica
{
    public class LogProducto
    {
        private static readonly HttpClient client = new HttpClient(); // Se crea un cliente HTTP para hacer las solicitudes a OpenFoodFacts

        private NutriScore NutriScore = new NutriScore();
        public ResActualizarProducto actualizar(ReqActualizarProducto req)
        {
            ResActualizarProducto res = new ResActualizarProducto();

            try
            {

                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El request no puede ser nulo");
                }
                else if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El producto no puede ser nulo");
                }
                else if (req.Producto_ID <= 0)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID del producto no puede ser menor o igual a cero");
                }
                else if (string.IsNullOrEmpty(req.Nombre))
                {
                    res.exito = false;
                    res.mensaje.Add("El nombre del producto no puede ser nulo o vacío");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";

                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Actualizar_Producto(
                        req.Producto_ID,
                        req.Nombre,
                        req.Categoria,
                        req.Marca,
                        req.Informacion_Nutricional,
                        req.Ingredientes,
                        ref exito,
                        ref mensaje
                    );

                    if (exito == true)
                    {
                        res.exito = true;
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add(mensaje);
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message);
            }

            return res;
        }

        public ResEliminarProducto eliminar(ReqEliminarProducto req)
        {
            ResEliminarProducto res = new ResEliminarProducto();

            try
            {
                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El request no puede ser nulo");
                }
                else if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El request no puede ser nulo");
                }
                else if (req.Producto_ID <= 0)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID del producto no puede ser menor o igual a cero");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";


                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Eliminar_Producto(
                        req.Producto_ID,
                        ref exito,
                        ref mensaje
                    );

                    if (exito == true)
                    {
                        res.exito = true;
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add(mensaje);
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message);
            }

            return res;
        }

        public async Task<ResBuscarProducto> buscar(ReqBuscarProducto req)
        {
            ResBuscarProducto res = new ResBuscarProducto();

            // Buscar productos por nombre (devuelve lista)
            List<CodigoBarras> productos = await BuscarProductoPorNombre(req.NombreProducto);

            try
            {
                if (productos == null || !productos.Any())
                {
                    res.exito = false;
                    res.mensaje.Add("No se encontraron productos con ese nombre.");
                }
                else
                {
                    ConectionDataContext miLinq = new ConectionDataContext();
                    bool? exito = false;
                    string mensaje = "";

                    foreach (var producto in productos)
                    {
                        // Enviar cada producto al SP para almacenarlo o verificar si ya existe
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

                        if (exito == true && productoEscaneado.Any())
                        {
                            // Mapear y agregar cada producto encontrado o insertado al resultado
                            foreach (SP_Escanear_CodigoResult unProductoBuscado in productoEscaneado)
                            {
                                res.ProductosEncontrados.Add(factoriaCodigoBarras(unProductoBuscado));
                            }
                        }
                        else
                        {
                            res.mensaje.Add(mensaje);
                        }
                    }

                    // Si al menos un producto fue procesado correctamente
                    if (res.ProductosEncontrados.Any())
                    {
                        res.exito = true;
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add("No se pudo procesar ningún producto.");
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message);
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
        public async Task<List<CodigoBarras>> BuscarProductoPorNombre(string nombreProducto)
        {
            try
            {
                // Reemplaza los espacios en blanco por %20 para hacer la URL correcta
                string nombreProductoEncoded = Uri.EscapeDataString(nombreProducto);
                string url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={nombreProductoEncoded}&search_simple=1&json=1";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var productData = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                List<CodigoBarras> productosEncontrados = new List<CodigoBarras>();

                if (productData != null && productData.count > 0)
                {
                    foreach (var product in productData.products)
                    {
                        CodigoBarras producto = new CodigoBarras
                        {
                            Codigo_Barras = product.code != null ? product.code.ToString() : "",
                            Nombre = product.product_name != null ? product.product_name.ToString() : "",
                            Categoria = product.categories != null ? product.categories.ToString() : "",
                            Marca = product.brands != null ? product.brands.ToString() : "",
                            Informacion_Nutricional = product.nutriments != null ? JsonConvert.SerializeObject(product.nutriments) : "",
                            nutri_score = NutriScore.CalcularCalificacion(JsonConvert.SerializeObject(product.nutriments)),
                            Ingredientes = product["ingredients_text"] != null ? product["ingredients_text"].ToString() : "Información no disponible"
                        };

                        productosEncontrados.Add(producto);
                    }
                }

                return productosEncontrados;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
