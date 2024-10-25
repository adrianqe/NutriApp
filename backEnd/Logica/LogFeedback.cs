using backEnd.DataAccess;
using backEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Logica
{
    public class LogFeedback
    {
        public ResInsertarFedback insertar(ReqInsertarFeedback req)
        {
            ResInsertarFedback res = new ResInsertarFedback();
            try
            {
                //Validar que el feedback no sea nulo
                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El feedback no puede ser nulo");
                }
                else if (req.feedback.UsuarioID == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El usuarioID no puede ser nulo o vacio");
                }
                else if (req.feedback.Calificacion == null || req.feedback.Calificacion < 1 || req.feedback.Calificacion > 5)
                {// Las calificaciones van de 1 a 5
                    res.exito = false;
                    res.mensaje.Add("La calificacion no puede ser nula o no estar dentro del rango de 1 a 5");
                }
                else if (string.IsNullOrEmpty(req.feedback.Comentario))
                {
                    res.exito = false;
                    res.mensaje.Add("El comentario no puede ser nulo o vacio");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";

                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Registrar_Feedback(
                        req.feedback.UsuarioID,
                        req.feedback.ProductoID,
                        req.feedback.Calificacion,
                        req.feedback.Comentario,
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

        public ResEliminarfeedback eliminar(ReqEliminarFeedback req)
        {
            ResEliminarfeedback res = new ResEliminarfeedback();

            try
            {
                //Validar que el feedback no sea nulo
                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El feedback no puede ser nulo");
                }
                else if (req.feedback.FeedbackID == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID de feedback no puede ser nulo");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";

                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Eliminar_Feedback(
                        req.feedback.FeedbackID,
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
