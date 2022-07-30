 

namespace WATickets.Models.Cliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Agrupados")]
    public partial class Agrupados
    {
        public int id { get; set; }
        public int DocEntry { get; set; }
        public string Articulo { get; set; }
        public string SustPor { get; set; }
        public string ItemName { get; set; }
        public decimal Requerido { get; set; }
        public decimal CantidadReal { get; set; }
    }
}