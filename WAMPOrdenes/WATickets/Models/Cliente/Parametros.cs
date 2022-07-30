 

namespace WATickets.Models.Cliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Parametros")]
    public partial class Parametros
    {
        public int id { get; set; }
        public string SQLEncOrdenesCompra { get; set; }
        public string SQLDetOrdenesCompra { get; set; }
        public string SQLOrdenesProduccion { get; set; }
        public string SQLExistencias { get; set; }
        public string SQLProductoxOrdenes { get; set; }
        public string SQLAgrupado { get; set; }
        public string SQLLineaxOrden { get; set; }
        public string SQLOrdenesFabricacionAsociadas { get; set; }
        public string SQLPreguntaExisten { get; set; }
        public string SQLRecibo { get; set; }
    }
}