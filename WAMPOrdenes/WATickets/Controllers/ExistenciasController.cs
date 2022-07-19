﻿using System;
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

    public class ExistenciasController : ApiController
    {
        ModelCliente db = new ModelCliente();
        G G = new G();


        public async Task<HttpResponseMessage> Get([FromUri] Filtros filtro)
        {
            try
            {

                Parametros parametros = db.Parametros.FirstOrDefault();
                var conexion = G.DevuelveCadena(db);

                var SQL = parametros.SQLExistencias;
                if (filtro.Codigo1 > 0)
                {
                    SQL = SQL.Replace("@reemplazo", filtro.Codigo1.ToString());
                }
                SqlConnection Cn = new SqlConnection(conexion);
                SqlCommand Cmd = new SqlCommand(SQL, Cn);
                SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                DataSet Ds = new DataSet();
                Cn.Open();
                Da.Fill(Ds, "Existencias");


                Cn.Close();


                return Request.CreateResponse(HttpStatusCode.OK, Ds);

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