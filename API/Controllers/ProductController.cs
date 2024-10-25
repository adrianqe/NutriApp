using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using backEnd.Entidades;
using backEnd.Logica;

namespace API.Controllers
{
    public class ProductController : ApiController
    {

        //Post api/values
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/productos/actualizarProducto")]
        public ResActualizarProducto Post(ReqActualizarProducto req)
        {
            return new LogProducto().actualizar(req);
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("api/productos/eliminarProducto")]
        public ResEliminarProducto eliminarProducto(ReqEliminarProducto req)
        {
            return new LogProducto().eliminar(req);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/productos/buscarProducto")]
        public Task<ResBuscarProducto> buscarProducto(ReqBuscarProducto req)
        {
            var resultado = new LogProducto().buscar(req); // Usamos await
            return resultado;
        }
    }
}