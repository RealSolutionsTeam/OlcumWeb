namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KazanDetay")]
    public partial class KazanDetay
    {
        public int id { get; set; }

        public int orderID { get; set; }

        public int atolyeID { get; set; }

        public int? giysiTuruID { get; set; }

        public int olcuTuruID { get; set; }

        [StringLength(50)]
        public string yikamaYeri { get; set; }

        [StringLength(50)]
        public string yikamaSorumlu { get; set; }

        public int tabloTuru { get; set; }

        public int? olcumNo { get; set; }

        public string aciklama { get; set; }

        public DateTime tarih { get; set; }

        public int kullaniciID { get; set; }

        public bool aparatliMi { get; set; }

        public bool? aktarimMi { get; set; }

        public int utuPaketID { get; set; }

        public bool? durum { get; set; }

        public int olcuTabloOlcuTurId { get; set; }

        public int? kaliteKontrolId { get; set; }

        public int? kaliteInspectionId { get; set; }

        public int? kDerece { get; set; }

        public int? kDakika { get; set; }

        public virtual OlcuTurleri OlcuTurleri { get; set; }
    }
}