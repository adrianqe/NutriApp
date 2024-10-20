using backEnd.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using backEnd.Logica;
using Newtonsoft.Json;

public class escanerController : ApiController
{
    [System.Web.Http.HttpPost]
    [System.Web.Http.Route("api/escanear/producto")]
    public async Task<IHttpActionResult> EscanearCodigo(ReqEscanearCodigo req)
    {
        var resultado = await new LogCodigoBarras().escanear(req); // Usamos await
        return Ok(resultado);
    }
}
