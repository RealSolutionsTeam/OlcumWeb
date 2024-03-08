namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Modeller")]
    public partial class Modeller
    {
        public int id { get; set; }

        [Required]
        [StringLength(400)]
        public string modelAd { get; set; }

        public int musteriID { get; set; }
    }
}
