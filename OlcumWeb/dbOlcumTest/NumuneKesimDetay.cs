namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NumuneKesimDetay")]
    public partial class NumuneKesimDetay
    {
        public int id { get; set; }

        public int orderID { get; set; }

        public int giysiTuruID { get; set; }

        public int olcuTuruID { get; set; }

        public int kullaniciID { get; set; }

        [StringLength(450)]
        public string garni { get; set; }

        [StringLength(450)]
        public string garni2 { get; set; }

        [StringLength(450)]
        public string garni3 { get; set; }

        [StringLength(450)]
        public string tela { get; set; }

        public int? adet { get; set; }

        [StringLength(400)]
        public string olcuNoktasi1 { get; set; }

        [StringLength(400)]
        public string olcu1 { get; set; }

        [StringLength(400)]
        public string olcuNoktasi2 { get; set; }

        [StringLength(400)]
        public string olcu2 { get; set; }

        [StringLength(400)]
        public string olcuNoktasi3 { get; set; }

        [StringLength(400)]
        public string olcu3 { get; set; }

        [StringLength(400)]
        public string olcuNoktasi4 { get; set; }

        [StringLength(400)]
        public string olcu4 { get; set; }

        [StringLength(400)]
        public string olcuNoktasi5 { get; set; }

        [StringLength(400)]
        public string olcu5 { get; set; }

        [StringLength(400)]
        public string olcuNoktasi6 { get; set; }

        [StringLength(400)]
        public string olcu6 { get; set; }

        [StringLength(425)]
        public string cekme { get; set; }

        [StringLength(425)]
        public string kumasEni { get; set; }

        [StringLength(400)]
        public string topNo { get; set; }

        public int? kumasID { get; set; }

        [StringLength(400)]
        public string baski { get; set; }

        [StringLength(400)]
        public string nakis { get; set; }

        public string aciklama { get; set; }

        [Column(TypeName = "date")]
        public DateTime? tarih { get; set; }
    }
}
