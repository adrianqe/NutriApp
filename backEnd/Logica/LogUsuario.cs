﻿using backEnd.DataAccess;
using backEnd.Entidades;
using backEnd.Entidades.Response;
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
                //Validar que el request no sea nulo
                if (req == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El request no puede ser nulo");
                }
                else if (String.IsNullOrEmpty(req.usuario.Nombre))
                {
                    res.exito = false;
                    res.mensaje.Add("El nombre no puede ser nulo o vacio");
                }
                else if (String.IsNullOrEmpty(req.usuario.Email))
                {
                    res.exito = false;
                    res.mensaje.Add("El email no puede ser nulo o vacio");
                }
                else if (String.IsNullOrEmpty(req.usuario.Password))
                {
                    res.exito = false;
                    res.mensaje.Add("El password no puede ser nulo o vacio");
                }
                else
                {
                    // Variables de salida del SP
                    bool? exito = false;
                    string mensaje = "";

                    // Crear el contexto de conexión y llamar al SP
                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Registrar_Nuevo_Usuario(
                        req.usuario.Nombre,
                        req.usuario.Email,
                        req.usuario.Password,  // Esto debería ser el hash ya procesado
                        ref exito,  // Aquí se pasa por referencia
                        ref mensaje // Mensaje de salida del SP
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
                else if (req.usuario == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El usuario no puede ser nulo");
                }
                else if (req.usuario.Usuario_ID == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID de usuario no puede ser nulo");
                }
                else if (string.IsNullOrEmpty(req.usuario.Nombre))
                {
                    res.exito = false;
                    res.mensaje.Add("El nombre no puede ser nulo o vacio");
                }
                else if (string.IsNullOrEmpty(req.usuario.Email))
                {
                    res.exito = false;
                    res.Equals("El email no puede ser nulo o vacio");
                }
                else if (string.IsNullOrEmpty(req.usuario.Password))
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
                        req.usuario.Usuario_ID,
                        req.usuario.Nombre,
                        req.usuario.Email,
                        req.usuario.Password,
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
                else if (req.usuario == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El usuario no puede ser nulo");
                }
                else if (req.usuario.Usuario_ID == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID de usuario no puede ser nulo");
                }
                else if (req.usuario.Usuario_ID <= 0) // Validar que el ID sea mayor a cero
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
                        req.usuario.Usuario_ID,
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
