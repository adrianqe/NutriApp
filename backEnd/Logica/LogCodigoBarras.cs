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
                else
                {
                    bool? exito = false;
                    string mensaje = "";
                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Escanear_Codigo(
                        req.codigoBarras.Codigo_Barras,
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
