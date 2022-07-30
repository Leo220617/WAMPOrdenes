using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WATickets.Models;
using WATickets.Models.Cliente;

namespace WATickets.Controllers
{
    [Authorize]

    public class GeneradosController: ApiController
    {
        ModelCliente db = new ModelCliente();
        G G = new G();
        public async Task<HttpResponseMessage> Get([FromUri] Filtros filtro)
        {
            try
            {
                var time = new DateTime();
                if (filtro.FechaFinal != time)
                {
                    filtro.FechaFinal = filtro.FechaFinal.AddDays(1);
                }
                var BT = db.Generados.Where(a => (filtro.FechaInicial == time ? true : a.Fecha >= filtro.FechaInicial) && (filtro.FechaFinal == time ? true : a.Fecha <= filtro.FechaFinal)).ToList();



                return Request.CreateResponse(HttpStatusCode.OK, BT);

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}