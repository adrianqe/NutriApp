using backEnd.Entidades;
using backEnd.Logica;
using backEnd.Request;
using backEnd.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    public class UsuarioController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/usuario/registrar")]
        public async Task<ResToken> RegistrarUsuario(ReqInsertarUsuario req)
        {
            var res = new ResInsertarUsuario(); // Este objeto no se devuelve
            var resToken = new ResToken(); // Este es el objeto que se devuelve

            // Llamada al servicio lógico asincrónico para registrar el usuario
            var usuarioRes = await new LogUsuario().registrar(req);

            if (!usuarioRes.exito)
            {
                resToken.exito = false;
                resToken.mensaje = usuarioRes.mensaje;
                return resToken; // Devolver respuesta de error
            }

            // Si el registro es exitoso, generar el token JWT
            var token = tokenRegistrar(req.Nombre, req.Email, usuarioRes.Usuario_ID.Value, usuarioRes.codigoVerificacion);

            // Asignar los valores en el objeto de respuesta
            resToken.exito = true;
            resToken.Token = token; // Aquí asignamos correctamente el token al objeto que se devolverá
            resToken.mensaje = usuarioRes.mensaje;

            Console.WriteLine($"Usuario registrado: {req.Nombre}");
            System.Diagnostics.Debug.WriteLine($"Registro Exitoso: Token - {token}");

            return resToken; // Devolver el objeto correcto
        }


        // POST: api/usuario/verificar
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/usuario/verificar")]
        public ResVerificarUsuario VerificarUsuario(ReqVerificarUsuario req)
        {
            return new LogUsuario().verificar(req);
        }

        // POST: api/usuario/reenvarCodigo
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/usuario/reenvarCodigo")]
        public async Task<ResReenviarCodigo> reenviarCodigo(ReqReenviarCodigo req)
        {
            return await new LogUsuario().reenviar(req);
        }

        // PUT: api/usuario/actualizar
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/usuario/actualizar")]
        public ResActualizarUsuario ActualizarUsuario(ReqActualizarUsuario req)
        {
            return new LogUsuario().actualizar(req);
        }

        // DELETE: api/usuario/eliminar
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("api/usuario/eliminar")]
        public ResEliminarUsuario EliminarUsuario(ReqEliminarUsuario req)
        {
            return new LogUsuario().eliminar(req);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/usuario/iniciarSesion")]
        public ResToken IniciarSesion(ReqIniciarSesionUsuario req)
        {

            var usuario = new LogUsuario().iniciarSesion(req);
            var resToken = new ResToken();
            var res = new ResIniciarSesionUsuario();


            if (usuario == null)
            {
                resToken.exito = false;
                resToken.mensaje = new List<string> { "Usuario o contraseña incorrectos." };
                return resToken;
            }

            if (!usuario.IDUsuario.HasValue)
            {
                resToken.exito = false;
                resToken.mensaje = new List<string> { "Usuario no encontrado." };
                return resToken;
            }

            // aqui se gemera el token JWT
            var token = tokenLogIn(usuario.IDUsuario.Value, usuario.nombre, usuario.email);

            resToken.exito = true;
            resToken.Token = token;

            Usuario usuarioModelo = new Usuario(res);

            Console.WriteLine(usuarioModelo.Nombre);

            System.Diagnostics.Debug.WriteLine($"Inicio de secion Exitoso: Token - {token}");

            return resToken;
        }

        // POST: api/usuario/registrarAlergias
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/usuario/registrarAlergias")]
        public ResInsertarAlergias InsertarAlergias(ReqInsertarAlergias req)
        {
            return new Alergias().InsertarAlergias(req);
        }

        // GET: api/historial/obtener
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/usuario/historialEscaneos")]
        public Task<ResEscanearCodigo> ObtenerHistorialEscaneos(int usuarioID)
        {
            return new LogUsuario().ObtenerHistorialEscaneos(usuarioID);
        }

        // Método para generar el Token JWT
        private string tokenRegistrar(string nombre, string Email, int Usuario_ID, int CodigoVerificacion)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("YourNewSecureKeyThatIsAtLeast32BytesLongAndSecure1234");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, nombre),
            new Claim(ClaimTypes.Email, Email),
            new Claim(ClaimTypes.NameIdentifier, Usuario_ID.ToString()),
            new Claim("CodigoVerificacion", CodigoVerificacion.ToString())
        }),
                //Expires = DateTime.UtcNow.AddHours(2), --> Tiempo de expiración del token descomentar si se desea
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "https://localhost:44386",
                Audience = "https://localhost:44386"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        private string tokenLogIn(int idUsuario, string nombre, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("YourNewSecureKeyThatIsAtLeast32BytesLongAndSecure1234");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
            new Claim(ClaimTypes.Name, nombre),
            new Claim(ClaimTypes.Email, email)
                }),
                //Expires = DateTime.UtcNow.AddHours(2), --> Tiempo de expiración del token descomentar si se desea
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "https://localhost:44386",
                Audience = "https://localhost:44386"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); // En esta linea
            return tokenHandler.WriteToken(token);
        }

    }
}