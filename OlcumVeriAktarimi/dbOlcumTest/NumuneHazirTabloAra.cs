namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NumuneHazirTabloAra")]
    public partial class NumuneHazirTabloAra
    {
        public int Id { get; set; }

        public int HazirTabloId { get; set; }

        public int olcuNoktaId { get; set; }

        [StringLength(150)]
        public string olcuNoktasi { get; set; }

        public virtual NumuneHazirTabloDetay NumuneHazirTabloDetay { get; set; }

        public virtual OlcuNoktalari OlcuNoktalari { get; set; }
    }
}
