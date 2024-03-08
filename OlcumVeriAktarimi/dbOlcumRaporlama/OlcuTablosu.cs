namespace OlcumVeriAktarimi.dbOlcumRaporlama
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OlcuTablosu")]
    public partial class OlcuTablosu
    {
        [Key]
        public int ottabloID { get; set; }

        [Required]
        [StringLength(200)]
        public string orderNo { get; set; }

        [StringLength(500)]
        public string orderArtikel { get; set; }

        [StringLength(25)]
        public string giysiTuru { get; set; }

        [StringLength(50)]
        public string modelistAd { get; set; }

        [StringLength(15)]
        public string enBoyCekme { get; set; }

        [Column(TypeName = "date")]
        public DateTime? tarih { get; set; }

        [StringLength(7)]
        public string anaBeden { get; set; }

        public int? ottabloTuru { get; set; }

        [StringLength(15)]
        public string olcuTuru { get; set; }

        public int? karsitNo { get; set; }

        public string aciklama { get; set; }

        public int? olcuTuruNo { get; set; }

        public int? satirSayisi { get; set; }
    }
}
