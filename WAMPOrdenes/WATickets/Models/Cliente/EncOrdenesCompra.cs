 
namespace WATickets.Models.Cliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EncOrdenesCompra")]
    public partial class EncOrdenesCompra
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string Moneda { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime DocDate { get; set; }
        public string Series { get; set; }
        public string Estado { get; set; }
        public string Comentarios { get; set; }
    }
}