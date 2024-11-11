using backEnd.DataAccess;
using backEnd.Entidades;
using backEnd.Request;
using backEnd.Response;
using System;
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
                        ref mensaje
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

        public ResIniciarSesionUsuario iniciarSesion(ReqIniciarSesionUsuario req)
        {
            ResIniciarSesionUsuario res = new ResIniciarSesionUsuario();

            try
            {
                // Validación de entrada
                if (string.IsNullOrEmpty(req.Email))
                {
                    res.exito = false;
                    res.mensaje.Add("El correo electrónico es requerido.");
                    return res;
                }
                else if (string.IsNullOrEmpty(req.Password))
                {
                    res.exito = false;
                    res.mensaje.Add("La contraseña es requerida.");
                    return res;
                }
                else
                {
                    bool? exito = false;
                    string hashedPasswordFromDB = null; // Variable para recibir el hash almacenado

                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Iniciar_Sesion(req.Email, ref hashedPasswordFromDB, ref exito);

                    if (exito == true && !string.IsNullOrEmpty(hashedPasswordFromDB)) // Verificar que el usuario exista y tenga contraseña
                    {
                        // Verificar la contraseña ingresada contra el hash de la base de datos
                        if (PasswordHelper.VerificarContraseña(req.Password, hashedPasswordFromDB))
                        {
                            // La contraseña es correcta
                            res.exito = true;
                            res.mensaje.Add("Inicio de sesión exitoso.");
                        }
                        else
                        {
                            res.exito = false;
                            res.mensaje.Add("Password incorrecto.");
                        }
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add("Email incorrecto.");
                    }
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add("Ocurrió un error en el inicio de sesión.");
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
    }
}
