namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Musteriler")]
    public partial class Musteriler
    {
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string musteriAd { get; set; }
    }
}
