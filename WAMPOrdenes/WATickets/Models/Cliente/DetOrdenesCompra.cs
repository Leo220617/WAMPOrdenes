 
 

namespace WATickets.Models.Cliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetOrdenesCompra")]
    public partial class DetOrdenesCompra
    {
        [Key, Column(Order = 0)]
        public int DocEntry { get; set; }
        [Key, Column(Order = 1)]
        public int LineNum { get; set; }
        public string CodPro { get; set; }
        public string NombreProducto { get; set; }
        public decimal Cantidad { get; set; }
        public string Almacen { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string Status { get; set; }

    }
}