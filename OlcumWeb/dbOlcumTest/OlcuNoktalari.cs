namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OlcuNoktalari")]
    public partial class OlcuNoktalari
    {
        public int id { get; set; }

        public int? giysiTuruID { get; set; }

        [Required]
        [StringLength(150)]
        public string olcuNoktasi { get; set; }

        public int tabanID { get; set; }

        public bool? anaNoktami { get; set; }

        public bool? enBoy { get; set; }

        public double? siraNo { get; set; }
    }
}
