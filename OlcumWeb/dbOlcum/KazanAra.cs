namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KazanAra")]
    public partial class KazanAra
    {
        public int id { get; set; }

        public int kazanDetayID { get; set; }

        public int olcuNoktaID { get; set; }

        public int bedenID { get; set; }

        [StringLength(50)]
        public string enBoyCekme { get; set; }

        public double? deger { get; set; }

        public int? pantNo { get; set; }

        public int? bpantNo { get; set; }

        public virtual Bedenler Bedenler { get; set; }

        public virtual KazanDetay KazanDetay { get; set; }

        public virtual OlcuNoktalari OlcuNoktalari { get; set; }
    }
}
