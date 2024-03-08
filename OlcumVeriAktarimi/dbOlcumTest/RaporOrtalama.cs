namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RaporOrtalama")]
    public partial class RaporOrtalama
    {
        public int id { get; set; }

        public int raporID { get; set; }

        public int olcuNoktaID { get; set; }

        [Required]
        [StringLength(250)]
        public string olcuNoktaAd { get; set; }

        public double? ortUygulananCekmeTolerans { get; set; }

        public double? ortGerceklesenCekmeTolerans { get; set; }

        public double? ortOrtalamaDeger { get; set; }

        public double? kararVerilenCekmeTolerans { get; set; }

        public string aciklama { get; set; }

        public virtual OlcuNoktalari OlcuNoktalari { get; set; }

        public virtual RaporDetay RaporDetay { get; set; }
    }
}
