namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HazirTabloAra")]
    public partial class HazirTabloAra
    {
        public int id { get; set; }

        public int hazirtabloID { get; set; }

        public int olcuNoktaID { get; set; }

        public int bedenID { get; set; }

        public double deger { get; set; }

        public int? satirIndexi { get; set; }

        public virtual Bedenler Bedenler { get; set; }

        public virtual HazirTabloDetay HazirTabloDetay { get; set; }

        public virtual OlcuNoktalari OlcuNoktalari { get; set; }
    }
}
