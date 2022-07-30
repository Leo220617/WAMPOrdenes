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

    public class AgrupadoProcesarController: ApiController
    {
        ModelCliente db = new ModelCliente();
        G G = new G();

        [HttpPost]
        public HttpResponseMessage Post([FromBody] List<Agrupados> agrup)
        {
            try
            {
                var Agrupado1 = db.Agrupados.ToList();
                foreach(var item in Agrupado1)
                {
                    db.Agrupados.Remove(item);
                    db.SaveChanges();
                }
                foreach (var item in agrup)
                {
                    //var Agrupado = db.Agrupados.Where(a => a.DocEntry == item.DocEntry && a.Articulo == item.Articulo).FirstOrDefault();
                    //if (Agrupado != null)
                    //{
                    //    db.Agrupados.Remove(Agrupado);
                    //    db.SaveChanges();
                    //}

                    var Agrupado = new Agrupados();
                    Agrupado.DocEntry = item.DocEntry;
                    Agrupado.Articulo = item.Articulo;
                    Agrupado.SustPor = item.SustPor;
                    Agrupado.ItemName = item.ItemName;
                    Agrupado.Requerido = item.Requerido;
                    Agrupado.CantidadReal = item.CantidadReal;
                    db.Agrupados.Add(Agrupado);
                    db.SaveChanges();



                }

                var Codigo1 = agrup.FirstOrDefault().DocEntry;
                // var PLineas = db.OrdenesLineas.Where(a => a.Orden == Codigo1).ToList();
                var PLineas = db.OrdenesLineas.ToList();

                foreach (var item in PLineas)
                {
                    db.OrdenesLineas.Remove(item);
                    db.SaveChanges();
                }

                Parametros parametros = db.Parametros.FirstOrDefault();
                var conexion = G.DevuelveCadena(db);

                var SQL = parametros.SQLLineaxOrden;
                if (Codigo1 > 0)
                {
                    SQL = SQL.Replace("@reemplazo", Codigo1.ToString());
                }
                SqlConnection Cn = new SqlConnection(conexion);
                SqlCommand Cmd = new SqlCommand(SQL, Cn);
                SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                DataSet Ds = new DataSet();
                Cn.Open();
                Da.Fill(Ds, "PLineas");
                foreach (DataRow item in Ds.Tables["PLineas"].Rows)
                {

                    var enc = new OrdenesLineas();

                    enc.Orden = Convert.ToInt32(item["Orden"].ToString());
                    enc.OrdenFabricacion = Convert.ToInt32(item["OrdenFabricacion"].ToString());
                    enc.Articulo = item["Articulo"].ToString();
                    enc.ItemName = item["ItemName"].ToString();
                    enc.Linea = Convert.ToInt32(item["Linea"].ToString());
                    enc.Requerido = Convert.ToDecimal(item["Requerido"].ToString());
                    db.OrdenesLineas.Add(enc);
                    db.SaveChanges();


                }

                Cn.Close();
                Cn.Dispose();

                //var AgrupadosConDiferencias = db.Agrupados.Where(a => a.DocEntry == Codigo1 && (a.CantidadReal - a.Requerido) > 0).ToList();

                var AgrupadosConDiferencias = db.Agrupados.Where(a => a.DocEntry == Codigo1 && (a.CantidadReal != a.Requerido)).ToList();

                var AgrupadosConSustitutos = db.Agrupados.Where(a => a.DocEntry == Codigo1 && !string.IsNullOrEmpty(a.SustPor)).ToList();

                foreach(var item in AgrupadosConDiferencias)
                {

                    var itemLinea = db.OrdenesLineas.Where(a => a.Orden == Codigo1 && a.Articulo == item.Articulo).FirstOrDefault();
                    var client = (ProductionOrders)Conexion.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                    if (client.GetByKey(itemLinea.OrdenFabricacion))
                    {
                        client.Lines.SetCurrentLine(itemLinea.Linea);
                        client.Lines.PlannedQuantity = client.Lines.PlannedQuantity + Convert.ToDouble(item.CantidadReal - item.Requerido);// Convert.ToDouble( item.Requerido + ( item.CantidadReal - item.Requerido));
                        client.Lines.BaseQuantity = client.Lines.BaseQuantity + Convert.ToDouble(item.CantidadReal - item.Requerido); //Convert.ToDouble(item.Requerido + (item.CantidadReal - item.Requerido));

                        var respuesta = client.Update();
                        if (respuesta == 0)
                        {



                            Conexion.Desconectar();
                           


                        }
                        else
                        {
                            BitacoraErrores be = new BitacoraErrores();

                            be.Descripcion = Conexion.Company.GetLastErrorDescription();
                            be.StackTrace = "Error procensando diferencias";
                            be.Fecha = DateTime.Now;

                            db.BitacoraErrores.Add(be);
                            db.SaveChanges();
                            Conexion.Desconectar();
                            

                        }
                    }
                    else
                    {
                        Conexion.Desconectar();

                         

                    }
                }


                foreach(var item in AgrupadosConSustitutos)
                {
                    var itemLinea = db.OrdenesLineas.Where(a => a.Orden == Codigo1 && a.Articulo == item.Articulo).ToList();

                    foreach(var item2 in itemLinea)
                    {
                        var client = (ProductionOrders)Conexion.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                        if (client.GetByKey(item2.OrdenFabricacion))
                        {
                            client.Lines.SetCurrentLine(item2.Linea);
                            client.Lines.ItemNo = item.SustPor;
                             

                            var respuesta = client.Update();
                            if (respuesta == 0)
                            {



                                Conexion.Desconectar();



                            }
                            else
                            {
                                BitacoraErrores be = new BitacoraErrores();

                                be.Descripcion = Conexion.Company.GetLastErrorDescription();
                                be.StackTrace = "Error procensando diferencias";
                                be.Fecha = DateTime.Now;

                                db.BitacoraErrores.Add(be);
                                db.SaveChanges();
                                Conexion.Desconectar();


                            }
                        }
                        else
                        {
                            Conexion.Desconectar();



                        }
                    }

                }

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
    }
}