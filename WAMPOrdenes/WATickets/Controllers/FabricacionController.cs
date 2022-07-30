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
                    if(filtro.Categoria == "1")
                    {
                        client.ProductionOrderStatus = BoProductionOrderStatusEnum.boposReleased;

                    }
                    else
                    {
                        client.ProductionOrderStatus = BoProductionOrderStatusEnum.boposClosed;
                    }
                    var respuesta = client.Update();
                    if (respuesta == 0)
                    {



                       // Conexion.Desconectar();
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
                Conexion.Desconectar();

                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StackTrace = ex.StackTrace.ToString();
                be.Fecha = DateTime.Now;
                db.BitacoraErrores.Add(be);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        [Route("api/Fabricacion/Emision")]
        public async Task<HttpResponseMessage> GETEMISION([FromUri] Filtros filtro)
        {
            try
            {
                List<int> listaOrdenes = new List<int>();
                Parametros parametros = db.Parametros.FirstOrDefault();
                var conexion = G.DevuelveCadena(db);
                var SQL = parametros.SQLOrdenesFabricacionAsociadas;
                if (filtro.Codigo1 > 0)
                {
                    SQL = SQL.Replace("@reemplazo", filtro.Codigo1.ToString() );
                }

                SqlConnection Cn = new SqlConnection(conexion);
                SqlCommand Cmd = new SqlCommand(SQL, Cn);
                SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                DataSet Ds = new DataSet();
                Cn.Open();
                Da.Fill(Ds, "Ordenes");
                foreach (DataRow item in Ds.Tables["Ordenes"].Rows)
                {

                    listaOrdenes.Add(Convert.ToInt32(item["OrdenFabricacion"].ToString()));


                }

                Cn.Close();
                Cn.Dispose();


                foreach (var item in listaOrdenes)
                {
                    bool bandera = false;

                    SQL = parametros.SQLPreguntaExisten;

                    if(filtro.Codigo1 > 0)
                    {
                        SQL += " " + item;
                    }

                    Cn = new SqlConnection(conexion);
                    Cmd = new SqlCommand(SQL, Cn);
                    Da = new SqlDataAdapter(Cmd);
                    Ds = new DataSet();
                    Cn.Open();
                    Da.Fill(Ds, "Lineas");
                        if(Ds.Tables["Lineas"].Rows.Count > 0)
                    {
                        bandera = true;
                    }
                    Cn.Close();

                    if(!bandera)
                    {
                        int Orden = 0;
                        SQL = parametros.SQLLineaxOrden;
                        if (filtro.Codigo1 > 0)
                        {
                            SQL = SQL.Replace("@reemplazo", filtro.Codigo1.ToString() + " and t0.Orde_Fabración = " + item);
                        }
                        Cn = new SqlConnection(conexion);
                        Cmd = new SqlCommand(SQL, Cn);
                        Da = new SqlDataAdapter(Cmd);
                        Ds = new DataSet();
                        Cn.Open();
                        Da.Fill(Ds, "Agrupado");


                        var client2 = (Documents)Conexion.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
                        client2.Reference2 = item.ToString();
                        client2.Series = 19;



                        foreach (DataRow item2 in Ds.Tables["Agrupado"].Rows)
                        {
                             
                            client2.Lines.Quantity = Convert.ToDouble(item2["Requerido"].ToString());
                            client2.Lines.BaseEntry = item;
                            client2.Lines.WarehouseCode = "12";
                            client2.Lines.BaseLine = Convert.ToInt32(item2["Linea"].ToString()); 
                            client2.Lines.Add();

                            Orden = Convert.ToInt32(item2["Orden"].ToString());
                        }
                        try
                        {
                            var respuesta = client2.Add();
                            if (respuesta == 0)
                            {
                                Generados be = new Generados();
                                be.BaseEntry = item;
                                be.DocEntry = Convert.ToInt32(Conexion.Company.GetNewObjectKey());
                                be.Message = "GENERADO";
                                be.Tipo = "EMISION";
                                be.Fecha = DateTime.Now;
                                be.OrdenCompra = Orden;
                                db.Generados.Add(be);
                                db.SaveChanges();
                                //Conexion.Desconectar();
                            }
                            else
                            {
                                BitacoraErrores be = new BitacoraErrores();

                                be.Descripcion = Conexion.Company.GetLastErrorDescription();
                                be.StackTrace = "NO GENERADO";
                                be.Fecha = DateTime.Now;

                                db.BitacoraErrores.Add(be);
                                db.SaveChanges();
                                Conexion.Desconectar();


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
                        }

                        Cn.Close();
                        Cn.Dispose();
                    }
                   
                    
                }





                return Request.CreateResponse(HttpStatusCode.OK);



            }
            catch (Exception ex)
            {
                Conexion.Desconectar();

                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StackTrace = ex.StackTrace.ToString();
                be.Fecha = DateTime.Now;
                db.BitacoraErrores.Add(be);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("api/Fabricacion/Recibo")]
        public async Task<HttpResponseMessage> GETRECIBO([FromUri] Filtros filtro)
        {
            try
            {
                List<int> listaOrdenes = new List<int>();
                Parametros parametros = db.Parametros.FirstOrDefault();
                var conexion = G.DevuelveCadena(db);
                var SQL = parametros.SQLOrdenesFabricacionAsociadas;
                if (filtro.Codigo1 > 0)
                {
                    SQL = SQL.Replace("@reemplazo", filtro.Codigo1.ToString());
                }

                SqlConnection Cn = new SqlConnection(conexion);
                SqlCommand Cmd = new SqlCommand(SQL, Cn);
                SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                DataSet Ds = new DataSet();
                Cn.Open();
                Da.Fill(Ds, "Ordenes");
                foreach (DataRow item in Ds.Tables["Ordenes"].Rows)
                {

                    listaOrdenes.Add(Convert.ToInt32(item["OrdenFabricacion"].ToString()));


                }

                Cn.Close();
                Cn.Dispose();


                foreach (var item in listaOrdenes)
                {
                    bool bandera = false;

                    SQL = parametros.SQLPreguntaExisten;
                    SQL = SQL.Replace("IGE1", "IGN1");
                    if (filtro.Codigo1 > 0)
                    {
                        SQL += " " + item;
                    }

                    Cn = new SqlConnection(conexion);
                    Cmd = new SqlCommand(SQL, Cn);
                    Da = new SqlDataAdapter(Cmd);
                    Ds = new DataSet();
                    Cn.Open();
                    Da.Fill(Ds, "Lineas");
                    if (Ds.Tables["Lineas"].Rows.Count > 0)
                    {
                        bandera = true;
                    }
                    Cn.Close();

                    if (!bandera)
                    {
                        int Orden = 0;

                        SQL = parametros.SQLRecibo;
                        if (filtro.Codigo1 > 0)
                        {
                            SQL = SQL + " " + item ;
                        }
                        Cn = new SqlConnection(conexion);
                        Cmd = new SqlCommand(SQL, Cn);
                        Da = new SqlDataAdapter(Cmd);
                        Ds = new DataSet();
                        Cn.Open();
                        Da.Fill(Ds, "Agrupado");


                        var client2 = (Documents)Conexion.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);
                        client2.Reference2 = item.ToString();
                        client2.Series = 18;



                        foreach (DataRow item2 in Ds.Tables["Agrupado"].Rows)
                        {

                            client2.Lines.Quantity = Convert.ToDouble(item2["Cantidad"].ToString());
                            client2.Lines.BaseEntry = item;
                            client2.Lines.WarehouseCode = "01";
                            //client2.Lines.BaseLine = Convert.ToInt32(item2["Linea"].ToString());
                            client2.Lines.Add();

                            Orden = Convert.ToInt32(item2["Orden"].ToString());

                        }
                        try
                        {
                            var respuesta = client2.Add();
                            if (respuesta == 0)
                            {
                                Generados be = new Generados();
                                be.BaseEntry = item;
                                be.DocEntry = Convert.ToInt32(Conexion.Company.GetNewObjectKey());
                                be.Message = "GENERADO";
                                be.Tipo = "RECIBO";
                                be.Fecha = DateTime.Now;
                                be.OrdenCompra = Orden;
                                db.Generados.Add(be);
                                db.SaveChanges();
                                //Conexion.Desconectar();
                            }
                            else
                            {
                                BitacoraErrores be = new BitacoraErrores();

                                be.Descripcion = Conexion.Company.GetLastErrorDescription();
                                be.StackTrace = "NO GENERADO";
                                be.Fecha = DateTime.Now;

                                db.BitacoraErrores.Add(be);
                                db.SaveChanges();
                                Conexion.Desconectar();


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
                        }

                        Cn.Close();
                        Cn.Dispose();
                    }


                }





                return Request.CreateResponse(HttpStatusCode.OK);



            }
            catch (Exception ex)
            {
                Conexion.Desconectar();

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