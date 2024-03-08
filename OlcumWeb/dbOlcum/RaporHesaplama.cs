namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RaporHesaplama")]
    public partial class RaporHesaplama
    {
        public int id { get; set; }

        public int raporID { get; set; }

        public int raporAraID { get; set; }

        public int olcuNoktaID { get; set; }

        [Required]
        [StringLength(250)]
        public string olcuNoktaAd { get; set; }

        public double? uygulananCekmeTolerans { get; set; }

        public double? gerceklesenCekmeTolerans { get; set; }

        public double? ortalamaDeger { get; set; }

        public virtual OlcuNoktalari OlcuNoktalari { get; set; }

        public virtual RaporAra RaporAra { get; set; }

        public virtual RaporDetay RaporDetay { get; set; }
    }
}
