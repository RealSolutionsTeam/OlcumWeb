namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bedenler")]
    public partial class Bedenler
    {
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string beden { get; set; }

        public int? bedenSistemi { get; set; }
    }
}
