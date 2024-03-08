namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RaporAnaCekme")]
    public partial class RaporAnaCekme
    {
        public int Id { get; set; }

        public int RaporId { get; set; }

        public int OlcuNoktaId { get; set; }

        [StringLength(250)]
        public string OlcuNoktaAd { get; set; }

        public double? AnaCekmeCekmeTolerans { get; set; }

        public virtual OlcuNoktalari OlcuNoktalari { get; set; }

        public virtual RaporDetay RaporDetay { get; set; }
    }
}
