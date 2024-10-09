using backEnd.DataAccess;
using backEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Logica
{
    public class LogCodigoBarras
    {
        public ResEscanearCodigo escanear(CodigoBarras producto)
        {
            ResEscanearCodigo res = new ResEscanearCodigo();

            try
            {
                if (producto == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El producto es nulo");
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
                else if (string.IsNullOrEmpty(producto.Marca))
                {
                    res.exito = false;
                    res.mensaje.Add("Marca vacía");
                }
                else if (string.IsNullOrEmpty(producto.Categoria))
                {
                    res.exito = false;
                    res.mensaje.Add("Categoría vacía");
                }
                else if (string.IsNullOrEmpty(producto.Informacion_Nutricional))
                {
                    res.exito = false;
                    res.mensaje.Add("Información nutricional vacía");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";
                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Escanear_Codigo(
                        producto.Codigo_Barras,
                        producto.Nombre,
                        producto.Categoria,
                        producto.Marca,
                        producto.Informacion_Nutricional,
                        ref exito,
                        ref mensaje
                    );

                    // Evaluar el resultado del SP
                    if (exito == true)
                    {
                        res.exito = true;
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add(mensaje);  // Aquí se agrega el mensaje devuelto por el SP
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message);  // En caso de que ocurra un error en la lógica
            }

            return res;
        }
    }
}
