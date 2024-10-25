using backEnd.Entidades;
using backEnd.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace API.Controllers
{
    public class UsuarioController : ApiController
    {
        // POST: api/usuario/insertar
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/usuario/insertar")]
        public ResInsertarUsuario InsertarUsuario(ReqInsertarUsuario req)
        {
            return new LogUsuario().insertar(req);
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
    }
}