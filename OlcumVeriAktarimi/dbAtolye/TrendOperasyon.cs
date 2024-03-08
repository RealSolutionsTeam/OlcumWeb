namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TrendOperasyon")]
    public partial class TrendOperasyon
    {
        public int Id { get; set; }

        public int? AtolyeId { get; set; }

        [StringLength(100)]
        public string OperasyonAd { get; set; }

        public int? ToplamKontrolAdet { get; set; }

        public int? ToplamHataAdet { get; set; }

        public int? Sonuc { get; set; }
    }
}
