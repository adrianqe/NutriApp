using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using backEnd.Entidades;
using backEnd.Logica;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class FeedbackController : ApiController
    {
        // POST: api/feedback/insertar
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/feedback/insertar")]
        public ResInsertarFedback InsertarFeedback(ReqInsertarFeedback req)
        {
            return new LogFeedback().insertar(req);
        }

        // DELETE: api/feedback/eliminar
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("api/feedback/eliminar")]
        public ResEliminarfeedback EliminarFeedback(ReqEliminarFeedback req)
        {
            return new LogFeedback().eliminar(req);
        }

        // GET: api/feedback/obtener
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/feedback/obtener")]
        public Task<ResObtenerFeedback> ObtenerFeedback(int pruductoID)
        {
            return new LogFeedback().ObtenerFeedback(pruductoID);
        }
    }
}