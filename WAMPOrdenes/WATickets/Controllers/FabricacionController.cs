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
    public class FabricacionController: ApiController
    {
        ModelCliente db = new ModelCliente();
        G G = new G();

        public async Task<HttpResponseMessage> Get([FromUri] Filtros filtro)
        {
            try
            {

                Parametros parametros = db.Parametros.FirstOrDefault();
                var client = (ProductionOrders)Conexion.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                if (client.GetByKey(filtro.Codigo1))
                {
                    client.ProductionOrderStatus = BoProductionOrderStatusEnum.boposReleased;
                    var respuesta = client.Update();
                    if (respuesta == 0)
                    {



                        Conexion.Desconectar();
                        return Request.CreateResponse(HttpStatusCode.OK);


                    }
                    else
                    {
                        BitacoraErrores be = new BitacoraErrores();

                        be.Descripcion = Conexion.Company.GetLastErrorDescription();
                        be.StackTrace = "Error";
                        be.Fecha = DateTime.Now;

                        db.BitacoraErrores.Add(be);
                        db.SaveChanges();
                        Conexion.Desconectar();
                        return Request.CreateResponse(HttpStatusCode.InternalServerError);

                    }
                }
                else
                {
                    Conexion.Desconectar();

                    return Request.CreateResponse(HttpStatusCode.InternalServerError);

                }



            }
            catch (Exception ex)
            {
                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StackTrace = ex.StackTrace.ToString();
                be.Fecha = DateTime.Now;
                db.BitacoraErrores.Add(be);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}