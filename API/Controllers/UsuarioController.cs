using backEnd.Entidades;
using backEnd.Logica;
using backEnd.Request;
using backEnd.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace API.Controllers
{
    public class UsuarioController : ApiController
    {
        // POST: api/usuario/registrar
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/usuario/registrar")]
        public async Task<ResInsertarUsuario> RegistrarUsuario(ReqInsertarUsuario req)
        {
            return await new LogUsuario().registrar(req);
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

        // POST: api/usuario/iniciarSesion
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/usuario/iniciarSesion")]
        public ResIniciarSesionUsuario IniciarSesion(ReqIniciarSesionUsuario req)
        {
            return new LogUsuario().iniciarSesion(req);
        }

        // POST: api/usuario/registrarAlergias
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/usuario/registrarAlergias")]
        public ResInsertarAlergias InsertarAlergias(ReqInsertarAlergias req)
        {
            return new Alergias().InsertarAlergias(req);
        }
    }
}