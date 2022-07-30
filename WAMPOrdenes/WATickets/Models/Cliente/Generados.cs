 

namespace WATickets.Models.Cliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Generados")]
    public partial class Generados
    {
        public int id { get; set; }
        public int BaseEntry { get; set; }
        public int DocEntry { get; set; }
        public string Message { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }
        public int OrdenCompra { get; set; }
    }
}