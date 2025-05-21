using backEnd.DataAccess;
using backEnd.Entidades;
using backEnd.Request;
using backEnd.Response;
using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Logica
{
    public class LogUsuario
    {
        public async Task<ResInsertarUsuario> registrar(ReqInsertarUsuario req)
        {
            ResInsertarUsuario res = new ResInsertarUsuario();
            EmailService emailService = new EmailService();

            try
            {
                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El request no puede ser nulo");
                }
                else if (String.IsNullOrEmpty(req.Nombre))
                {
                    res.exito = false;
                    res.mensaje.Add("El nombre no puede ser nulo o vacio");
                }
                else if (String.IsNullOrEmpty(req.Email))
                {
                    res.exito = false;
                    res.mensaje.Add("El email no puede ser nulo o vacio");
                }
                else if (String.IsNullOrEmpty(req.Password))
                {
                    res.exito = false;
                    res.mensaje.Add("El password no puede ser nulo o vacio");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";
                    int? usuario_ID = null;

                    // Encriptar la contraseña antes de almacenarla
                    string hashedPassword = PasswordHelper.HashContraseña(req.Password);

                    // Enviar el correo y esperar el código de verificación
                    int? codigoVerificacion = await emailService.EnviarEmailAsync(req.Email);

                    // Aquí podrías guardar el código de verificación temporalmente en la base de datos
                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Registrar_Nuevo_Usuario(
                        req.Nombre,
                        req.Email,
                        hashedPassword,  // Guarda contraseña encriptada
                        codigoVerificacion,  // Guarda el código de verificación
                        ref exito,
                        ref mensaje,
                        ref usuario_ID  // Parámetro de salida para obtener el Usuario_ID

                    );

                    // Evaluar el resultado del SP
                    if (exito == true)
                    {
                        res.exito = true;
                        res.mensaje.Add("Usuario registrado exitosamente. Código de verificación enviado." + codigoVerificacion);
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

        public ResVerificarUsuario verificar(ReqVerificarUsuario req)
        {
            ResVerificarUsuario res = new ResVerificarUsuario();
            try
            {
                if (string.IsNullOrEmpty(req.Email))
                {
                    res.exito = false;
                    res.mensaje.Add("El correo electrónico es requerido.");
                }
                else if (req.CodigoVerificacion == null || req.CodigoVerificacion > 9999 || req.CodigoVerificacion < 1000)
                {
                    res.exito = false;
                    res.mensaje.Add("El código de verificación es requerido y debe ser de 4 dígitos.");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";

                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_CodigoVerificacion(req.Email, req.CodigoVerificacion, ref exito, ref mensaje);

                    if (exito == true)
                    {
                        res.exito = true;
                        res.mensaje.Add("Usuario registrado exitosamente. Código de verificación enviado al email.");
                        int codigoVerificacion = await emailService.EnviarEmailAsync(req.Email);
                        res.codigoVerificacion = codigoVerificacion;
                        res.Usuario_ID = usuario_ID; // Asignar el ID del usuario en la respuesta
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
                res.mensaje.Add(ex.Message);  // En caso de que ocurra un error en la lógica
            }

            return res;
        }

        public ResVerificarUsuario verificar(ReqVerificarUsuario req)
        {
            ResVerificarUsuario res = new ResVerificarUsuario();
            try
            {
                bool? exito = false;
                string mensaje = "";

                ConectionDataContext miLinq = new ConectionDataContext();
                miLinq.SP_Activar_Usuario(req.Email, ref exito, ref mensaje);

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
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message);  // En caso de que ocurra un error en la lógica
            }

            return res;
        }

        public async Task<ResReenviarCodigo> reenviar(ReqReenviarCodigo req)
        {
            ResReenviarCodigo res = new ResReenviarCodigo();
            EmailService emailService = new EmailService();

            try
            {
                bool? exito = false;
                bool? estado = false;
                string mensaje = "";

                ConectionDataContext miLinq = new ConectionDataContext();
                miLinq.SP_Validar_Usuario_Inactivo(req.Email, ref estado, ref exito, ref mensaje);

                if (exito == true)
                {
                    res.exito = true;
                    res.mensaje.Add("El código de verificación fue reenviado exitosamente.");
                    int codigoVerificacion = await emailService.EnviarEmailAsync(req.Email);
                    res.codigoVerificacion = codigoVerificacion;
                }
                else
                {
                    res.exito = false;
                    res.mensaje.Add(mensaje);  // Aquí se agrega el mensaje devuelto por el SP
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                res.exito = false;
                res.mensaje.Add($"Error al reenviar el código de verificación: {ex.Message}");
            }

            return res;
        }

        public ResIniciarSesionUsuario iniciarSesion(ReqIniciarSesionUsuario req)
        {
            ResIniciarSesionUsuario res = new ResIniciarSesionUsuario { mensaje = new List<string>() };

            try
            {
                // Validación de entrada
                if (string.IsNullOrEmpty(req.Email))
                {
                    res.exito = false;
                    res.mensaje.Add("El correo electrónico es requerido.");
                    return res;
                }
                if (string.IsNullOrEmpty(req.Password))
                {
                    res.exito = false;
                    res.mensaje.Add("La contraseña es requerida.");
                    return res;
                }

                bool? exito = false;
                string hashedPasswordFromDB = null;
                string nombre = null;
                int? usuarioId = null;

                using (ConectionDataContext miLinq = new ConectionDataContext())
                {
                    miLinq.SP_Iniciar_Sesion(req.Email, ref hashedPasswordFromDB, ref exito, ref usuarioId, ref nombre);
                }

                if (exito == true && !string.IsNullOrEmpty(hashedPasswordFromDB))
                {

                    if (PasswordHelper.VerificarContraseña(req.Password, hashedPasswordFromDB))
                    {
                        res.exito = true;
                        res.mensaje.Add("Inicio de sesión exitoso.");
                        res.IDUsuario = usuarioId;
                        res.nombre = nombre;
                        res.email = req.Email;
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add("Contraseña incorrecta.");
                    }
                }
                else
                {
                    res.exito = false;
                    res.mensaje.Add("Correo electrónico incorrecto.");
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add("Ocurrió un error en el inicio de sesión.");
                res.mensaje.Add($"Detalle del error: {ex.Message}");
            }

            return res;

        }

        public async Task<ResEscanearCodigo> ObtenerHistorialEscaneos(int usuarioID)
        {
            ResEscanearCodigo res = new ResEscanearCodigo();

            try
            {
                using (ConectionDataContext miLinq = new ConectionDataContext())
                {
                    var historialEscaneos = miLinq.SP_ObtenerHistorialEscaneos(usuarioID).ToList();

                    if (historialEscaneos.Any())
                    {
                        res.exito = true;

                        foreach (var item in historialEscaneos)
                        {
                            var historial = new CodigoBarras
                            {
                                Codigo_Barras = item.Codigo_Barras,
                                Nombre = item.Nombre,
                                Categoria = item.Categoria,
                                Marca = item.Marca,
                                Informacion_Nutricional = item.Informacion_Nutricional,
                                nutri_score = item.nutri_score,
                                Ingredientes = item.Ingredientes,
                                Fecha_Escaneo = item.Fecha_Escaneo,
                                Imagen = item.Imagen,
                                alergenos = DetectarAlergias(usuarioID, item.Ingredientes),
                                productoID = item.Producto_ID

                            };
                            res.codigoBarras.Add(historial);
                        }
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add("No se encontro historial de escaneos para este usuario.");
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;

                res.mensaje.Add($"Error al obtener el historial de escaneos: {ex.Message}");

                Console.WriteLine(ex.ToString());

            }

            return res;
        }

        public ResActualizarUsuario actualizar(ReqActualizarUsuario req)
        {
            ResActualizarUsuario res = new ResActualizarUsuario();

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
                    res.mensaje.Add("El usuario no puede ser nulo");
                }
                else if (req.Usuario_ID == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID de usuario no puede ser nulo");
                }
                else if (string.IsNullOrEmpty(req.Nombre))
                {
                    res.exito = false;
                    res.mensaje.Add("El nombre no puede ser nulo o vacio");
                }
                else if (string.IsNullOrEmpty(req.Email))
                {
                    res.exito = false;
                    res.Equals("El email no puede ser nulo o vacio");
                }
                else if (string.IsNullOrEmpty(req.Password))
                {
                    res.exito = false;
                    res.mensaje.Add("El password no puede ser nulo o vacio");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";

                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Actualizar_Usuario(
                        req.Usuario_ID,
                        req.Nombre,
                        req.Email,
                        req.Password,
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

        public ResEliminarUsuario eliminar(ReqEliminarUsuario req)
        {
            ResEliminarUsuario res = new ResEliminarUsuario();

            try
            {
                // Validar que el request no sea nulo
                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El request no puede ser nulo");
                }
                else if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El usuario no puede ser nulo");
                }
                else if (req.Usuario_ID == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID de usuario no puede ser nulo");
                }
                else if (req.Usuario_ID <= 0) // Validar que el ID sea mayor a cero
                {
                    res.exito = false;
                    res.mensaje.Add("El ID de usuario no puede ser menor o igual a cero");
                }
                else
                {
                    // Variables de salida del procedimientoo (sp)
                    bool? exito = false;
                    string mensaje = "";

                    // Crear el contexto de conexión y llamar al SP para eliminar
                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Eliminar_Usuario(
                        req.Usuario_ID,
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
