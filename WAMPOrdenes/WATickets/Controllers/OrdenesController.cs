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

    public class OrdenesController: ApiController
    {
        ModelCliente db = new ModelCliente();
        G G = new G();
        [Route("api/Ordenes/InsertarOrdenes")]
        public async Task<HttpResponseMessage> GetInsertarOrden()
        {
            try
            {
            
                var Parametros = db.Parametros.FirstOrDefault();
                
                    
                db.Database.ExecuteSqlCommand("delete from DetOrdenesCompra");
                db.Database.ExecuteSqlCommand("delete from EncOrdenesCompra");

                var conexion = G.DevuelveCadena(db);
                    var SQL = Parametros.SQLEncOrdenesCompra;
                    SqlConnection Cn = new SqlConnection(conexion);
                    SqlCommand Cmd = new SqlCommand(SQL, Cn);
                    SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                    DataSet Ds = new DataSet();
                    Cn.Open();
                    Da.Fill(Ds, "Encabezado");

                
                    foreach (DataRow item in Ds.Tables["Encabezado"].Rows)
                    {

                        var enc = new EncOrdenesCompra();
                   
                        enc.DocEntry = Convert.ToInt32(item["DocEntry"].ToString());
                        enc.DocNum = Convert.ToInt32(item["DocNum"].ToString());
                        enc.Moneda = item["Moneda"].ToString();
                        enc.CardCode = item["Proveedor"].ToString();
                        enc.CardName = item["NombreProveedor"].ToString();
                        enc.DocDate = Convert.ToDateTime(item["Fecha"].ToString());
                        enc.Series = item["Series"].ToString();
                        enc.Estado = item["Status"].ToString();
                        enc.Comentarios = item["Comentario"].ToString();
                        db.EncOrdenesCompra.Add(enc);
                        db.SaveChanges();
                 

                    }
                    Cn.Close();
                    Cn.Dispose();

                SQL = Parametros.SQLDetOrdenesCompra;
                Cn = new SqlConnection(conexion);
                Cmd = new SqlCommand(SQL, Cn);
                Da = new SqlDataAdapter(Cmd);
                Ds = new DataSet();
                Cn.Open();
                Da.Fill(Ds, "Detalle");

                 
                foreach (DataRow item in Ds.Tables["Detalle"].Rows)
                {
                    
                    DetOrdenesCompra det = new DetOrdenesCompra();
                    det.DocEntry = Convert.ToInt32(item["DocEntry"].ToString());
                    det.LineNum = Convert.ToInt32(item["LineNum"].ToString());
                    det.CodPro = item["CodPro"].ToString();
                    det.NombreProducto = item["NombreProducto"].ToString();
                    det.Cantidad = Convert.ToInt32(item["Cantidad"]);
                    det.Almacen = item["Almacen"].ToString();
                    det.PrecioUnitario = Convert.ToDecimal(item["PrecioUnitario"]);
                    det.Status = item["Status"].ToString();
                    db.DetOrdenesCompra.Add(det);
                    db.SaveChanges();
                }


                Cn.Close();
                Cn.Dispose();

                return Request.CreateResponse(HttpStatusCode.OK);
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

        public async Task<HttpResponseMessage> Get([FromUri] Filtros filtro)
        {
            try
            {

                var Ordenes = db.EncOrdenesCompra.Select(a => new {
                    a.DocEntry,
                    a.DocNum,
                    a.Moneda,
                    a.CardCode,
                    a.CardName,
                    a.DocDate,
                    a.Series,
                    a.Estado,
                    a.Comentarios,
                    Detalle = db.DetOrdenesCompra.Where(b => b.DocEntry == a.DocEntry).ToList()
                }).ToList();




                return Request.CreateResponse(HttpStatusCode.OK, Ordenes);

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


        [Route("api/Ordenes/Consultar")]
        public HttpResponseMessage GetOne([FromUri]int id)
        {
            try
            {
                

                var Ordenes = db.EncOrdenesCompra.Select(a => new
                {
                    a.DocEntry,
                    a.DocNum,
                    a.Moneda,
                    a.CardCode,
                    a.CardName,
                    a.DocDate,
                    a.Series,
                    a.Estado,
                    a.Comentarios,
                    Detalle = db.DetOrdenesCompra.Where(b => b.DocEntry == a.DocEntry).ToList()
                }).Where(a => a.DocEntry == id).FirstOrDefault();

                 


                if (Ordenes == null)
                {
                    throw new Exception("Esta Orden no se encuentra registrado");
                }

                return Request.CreateResponse(HttpStatusCode.OK, Ordenes);
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