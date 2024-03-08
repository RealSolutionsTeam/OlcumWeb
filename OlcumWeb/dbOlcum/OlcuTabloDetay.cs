namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OlcuTabloDetay")]
    public partial class OlcuTabloDetay
    {
        public int id { get; set; }

        public int orderID { get; set; }

        public int giysiTuruID { get; set; }

        public int olcuTuruID { get; set; }

        public int anaBedenID { get; set; }

        [StringLength(20)]
        public string enBoyCekme { get; set; }

        public int? tabloTuru { get; set; }

        public string aciklama { get; set; }

        public DateTime? tarih { get; set; }

        public int kullaniciID { get; set; }

        public bool? aktarimMi { get; set; }

        public int? oldOlcuTabloId { get; set; }

        public bool? hataOldu { get; set; }

        public virtual GiysiTurleri GiysiTurleri { get; set; }

        public virtual OlcuTurleri OlcuTurleri { get; set; }

        public virtual Order Order { get; set; }

        public virtual PersonelTablo PersonelTablo { get; set; }
    }
}
