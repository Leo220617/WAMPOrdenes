 
namespace WATickets.Models.Cliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdenesLineas")]
    public partial class OrdenesLineas
    {
        public int id { get; set; }
        public int Orden { get; set; }
        public int OrdenFabricacion { get; set; }
        public int Linea { get; set; }
        public string Articulo { get; set; }
        public string ItemName { get; set; }
        public decimal Requerido { get; set; }
    }
}