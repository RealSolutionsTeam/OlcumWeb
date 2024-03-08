namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CogaltmaDetay")]
    public partial class CogaltmaDetay
    {
        public int id { get; set; }

        public int orderID { get; set; }

        public int atolyeID { get; set; }

        public int giysiTuruID { get; set; }

        public int olcuTuruID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? kesimeGidis { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dikimeGidis { get; set; }

        [Column(TypeName = "date")]
        public DateTime? yikamayaGidis { get; set; }

        [Column(TypeName = "date")]
        public DateTime? yikamadanGidis { get; set; }

        [StringLength(100)]
        public string kalipAdi { get; set; }

        [StringLength(100)]
        public string topNo { get; set; }

        public int? olcumNo { get; set; }

        public string aciklama { get; set; }

        public DateTime tarih { get; set; }

        public int kullaniciID { get; set; }

        [StringLength(50)]
        public string receteKod { get; set; }

        public bool? aktarimMi { get; set; }
    }
}
