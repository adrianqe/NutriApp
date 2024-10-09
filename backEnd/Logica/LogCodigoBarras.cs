using backEnd.DataAccess;
using backEnd.Entidades;
using backEnd.Entidades.;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Logica
{
    public class LogCodigoBarras
    {
        public ResEscanearCodigo escanear(ReqEscanearCodigo req)
        {
            ResEscanearCodigo res = new ResEscanearCodigo();

            try
            {
                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("Peticion nula");
                }
                else if (string.IsNullOrEmpty(req.codigoBarras.Codigo_Barras))
                {
                    res.exito = false;
                    res.mensaje.Add("Codigo de barras vacío");
                }
                else if (string.IsNullOrEmpty(req.codigoBarras.Nombre))
                {
                    res.exito = false;
                    res.mensaje.Add("Nombre vacío");
                }
                else if (string.IsNullOrEmpty(req.codigoBarras.Marca))
                {
                    res.exito = false;
                    res.mensaje.Add("Marca vacía");
                }
                else if (string.IsNullOrEmpty(req.codigoBarras.Categoria))
                {
                    res.exito = false;
                    res.mensaje.Add("Categoria vacía");
                }
                else if (string.IsNullOrEmpty(req.codigoBarras.Informacion_Nutricional))
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
                        req.codigoBarras.Codigo_Barras,
                        req.codigoBarras.Nombre,
                        req.codigoBarras.Categoria,
                        req.codigoBarras.Marca,
                        req.codigoBarras.Informacion_Nutricional,
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
