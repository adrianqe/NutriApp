using backEnd.DataAccess;
using backEnd.Entidades;
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

                    
                    ConectionDataContext miLinq = new ConectionDataContext();
                    miLinq.SP_Registrar_Nuevo_Usuario(
                        req.Nombre,
                        req.Email,
                        req.Password,  // Esto debería ser el hash ya procesado
                        ref exito,  // Aquí se pasa por referencia
                        ref mensaje // Mensaje de salida del SP
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
        public ResObtenerUsuarios obtener()
        {
            ResObtenerUsuarios res = new ResObtenerUsuarios();
            res.usuarios = new List<Usuario>(); 
            res.mensaje = new List<string>();  

            try
            {
               
                ConectionDataContext miLinq = new ConectionDataContext();

               
                List<SP_Obtener_UsuariosResult> usuariosBD = miLinq.SP_Obtener_Usuarios().ToList();

              
                foreach (var usuarioBD in usuariosBD)
                {
                    Usuario usuario = new Usuario
                    {
                        Usuario_ID = usuarioBD.Usuario_ID,
                        Nombre = usuarioBD.Nombre,
                        Email = usuarioBD.Email,
                        FechaCreacion = usuarioBD.FechaCreacion
                    };
                    res.usuarios.Add(usuario);
                }

                res.exito = true;  
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message); 
            }

            return res;
        }
        private Usuario factoriaUsuario(SP_Obtener_UsuariosResult usuarioBD)
        {
            
            Usuario usuario = new Usuario
            {
                Usuario_ID = usuarioBD.Usuario_ID,
                Nombre = usuarioBD.Nombre,
                Email = usuarioBD.Email,
                FechaCreacion = usuarioBD.FechaCreacion 
            };

            return usuario;
        }

    }
}

        public ResConsultarUsuario consultar(ReqConsultarUsuario req)
        {
            ResConsultarUsuario res = new ResConsultarUsuario();

            try
            {
                if (req.UsuarioID == null)
                {
                    res.exito = false;
                    res.mensaje.Add("El ID de usuario no puede ser nulo");
                }
                else
                {
                    bool? exito = false;
                    string mensaje = "";

                    ConectionDataContext miLinq = new ConectionDataContext();

                    List<SP_Consultar_UsuarioResult> resultado = miLinq.SP_Consultar_Usuario(req.UsuarioID, ref exito, ref mensaje).ToList();

                    foreach (SP_Consultar_UsuarioResult usuarioDB in resultado)
                    {
                        res.Usuarios.Add(this.factoriaUsuario(usuarioDB));
                    }
                    res.exito = true;
                }
            }
            catch (Exception ex)
            {
                res.exito = false;
                res.mensaje.Add(ex.Message);
            }
            return res;
        }

        private Usuario factoriaUsuario(SP_Consultar_UsuarioResult usuarioBD) // Método para convertir el resultado del SP en un objeto Usuario
        {
            Usuario usuario = new Usuario();
            usuario.Usuario_ID = usuarioBD.Usuario_ID;
            usuario.Nombre = usuarioBD.Nombre;
            usuario.Email = usuarioBD.Email;
            usuario.FechaRegistro = usuarioBD.Fecha_Registro;
            return usuario;
        }
    }
}
