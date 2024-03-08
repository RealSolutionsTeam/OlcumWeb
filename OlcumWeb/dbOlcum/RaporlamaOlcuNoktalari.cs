namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RaporlamaOlcuNoktalari")]
    public partial class RaporlamaOlcuNoktalari
    {
        public int Id { get; set; }

        public int GiysiTuruId { get; set; }

        public int OlcuNoktaTabanId { get; set; }

        [Required]
        [StringLength(150)]
        public string AnaOlcuNoktasiAd { get; set; }
    }
}
