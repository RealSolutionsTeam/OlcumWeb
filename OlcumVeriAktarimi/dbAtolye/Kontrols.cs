namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Kontrols
    {
        public int Id { get; set; }

        public int? KullaniciId { get; set; }

        public int? AtolyeId { get; set; }

        public string OrderNumarasi { get; set; }

        public int? TypeId { get; set; }

        public DateTime? Tarih { get; set; }

        public TimeSpan? Sure { get; set; }

        [Column("Status ")]
        public bool? Status_ { get; set; }

        public string RevizyonNo { get; set; }

        public int? DepartmanId { get; set; }

        public string Bolum { get; set; }

        [StringLength(250)]
        public string Bilgi { get; set; }

        public int? UtuPaketId { get; set; }

        public int? BirimId { get; set; }

        public int? EkipId { get; set; }

        public int? LiderId { get; set; }

        [StringLength(200)]
        public string Bolge { get; set; }

        public int? YMKNoId { get; set; }

        public int? KMKNoId { get; set; }

        public int? MSarj { get; set; }

        [StringLength(50)]
        public string UrunMudur { get; set; }

        public int? ToplamUrunAdet { get; set; }

        public int? MakinaciAdId { get; set; }

        [StringLength(50)]
        public string SevkBilgi { get; set; }

        [StringLength(50)]
        public string Vardiya { get; set; }

        [StringLength(50)]
        public string BrandaNo { get; set; }

        [StringLength(50)]
        public string GelenBolum { get; set; }

        public string YuzdeYuzMailKritik { get; set; }

        [StringLength(50)]
        public string YuzdeYuzMailOnay { get; set; }

        [StringLength(700)]
        public string Karar { get; set; }

        public int? OnKontrolId { get; set; }

        public int? KontrolTur { get; set; }

        public int? SpreyciId { get; set; }

        public bool? TamirMi { get; set; }
    }
}
