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
        public ResInsertarUsuario insertar(ReqInsertarUsuario req)
        {
            ResInsertarUsuario res = new ResInsertarUsuario();

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

                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Registrar_Nuevo_Usuario(
                        req.Nombre,
                        req.Email,
                        hashedPassword,  // Usar la contraseña encriptada
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
                res.mensaje.Add(ex.Message);
            }

            return res;
        }

        public ResIniciarSesionUsuario iniciarSesion(ReqIniciarSesionUsuario req)
        {
            ResIniciarSesionUsuario res = new ResIniciarSesionUsuario();

            try
            {
                if (string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password))
                {
                    res.exito = false;
                    res.mensaje.Add("El correo electrónico y la contraseña son requeridos.");
                    return res;
                }

                bool? exito = false;
                string hashedPasswordFromDB = null; // Variable para recibir el hash almacenado

                ConectionDataContext miLinq = new ConectionDataContext();
                miLinq.SP_Iniciar_Sesion(req.Email, ref hashedPasswordFromDB, ref exito);

                if (exito == true && !string.IsNullOrEmpty(hashedPasswordFromDB))
                {
                    // Verificar la contraseña ingresada contra el hash de la base de datos
                    if (PasswordHelper.VerificarContraseña(req.Password, hashedPasswordFromDB))
                    {
                        // La contraseña es correcta
                        res.exito = true;
                        res.mensaje.Add("Inicio de sesión exitoso.");
                        // Aquí puedes generar el token JWT si corresponde
                        // res.Token = GenerarToken(res.Usuario); // Implementa la lógica para generar el token
                    }
                    else
                    {
                        res.exito = false;
                        res.mensaje.Add("Password incorrectas.");
                    }
                }
                else
                {
                    res.exito = false;
                    res.mensaje.Add("Email incorrectas.");
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add($"Error: {ex.Message}");
                res.mensaje.Add($"StackTrace: {ex.StackTrace}");
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
                    // Variables de salida del SP
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
