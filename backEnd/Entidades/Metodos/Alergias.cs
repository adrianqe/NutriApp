using backEnd.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
    public class Alergias
    {
        // Método para insertar alergias de un usuario
        public ResInsertarAlergias InsertarAlergias(ReqInsertarAlergias req)
        {
            var res = new ResInsertarAlergias
            {
                exito = false,
                mensaje = new List<string>()
            };

            try
            {
                // Convertir la lista de alergias a un formato delimitado por comas
                string alergias = string.Join(",", req.alergias);

                using (var context = new ConectionDataContext())
                {
                    // Llamar al procedimiento almacenado con la lista de alergias
                    int resultado = context.SP_InsertarAlergiasUsuario(req.userID, alergias);

                    if (resultado == 1) // Verificar si el SP fue exitoso
                    {
                        res.exito = true;
                        res.mensaje.Add("Alergias insertadas correctamente.");
                    }
                    else
                    {
                        res.mensaje.Add("Ocurrió un error al insertar las alergias.");
                    }
                }
            }
            catch (Exception ex)
            {
                res.mensaje.Add($"Error al insertar alergias: {ex.Message}");
            }

            return res;
        }


        // Método para obtener las alergias de un usuario
        public List<string> ObtenerAlergias(int userID)
        {
            using (var context = new ConectionDataContext())
            {
                var resultado = new List<string>();
                var alergiasUsuario = context.SP_Consultar_Usuario_Alergias(userID);

                foreach (var alergia in alergiasUsuario)
                {
                    resultado.Add(alergia.Nombre);
                }

                return resultado;
            }
        }

        public List<string> DetectarAlergias(int userID, string ingredientes)
        {
            var alergiasDetectadas = new List<string>();

            using (var context = new ConectionDataContext())
            {
                // Obtener las alergias del usuario
                var alergiasUsuario = context.SP_Consultar_Usuario_Alergias(userID);

                foreach (var alergia in alergiasUsuario)
                {
                    // Comparar cada palabra clave de la alergia con los ingredientes (case-insensitive)
                    if (ingredientes.IndexOf(alergia.Palabras_Clave, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        alergiasDetectadas.Add(alergia.Palabras_Clave);
                    }
                }
            }

            return alergiasDetectadas;
        }
    }
}
