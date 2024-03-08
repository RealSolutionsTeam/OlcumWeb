namespace OlcumWeb.Models.YalinUretim
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderTakipKimlikBilgiler")]
    public partial class OrderTakipKimlikBilgiler
    {
        public int ID { get; set; }

        public int? OrderID { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        [StringLength(200)]
        public string ModelAd { get; set; }

        [StringLength(50)]
        public string ArtikelNo { get; set; }

        public DateTime? GuncelTerminTarih { get; set; }

        public DateTime? IstenenTerminTarih { get; set; }

        public int? GerceklesenKesimAdet { get; set; }

        public int? SiparisAdet { get; set; }

        public int? UrunMudurID { get; set; }

        [StringLength(50)]
        public string UrunMudurAd { get; set; }

        public byte[] SentezFoto { get; set; }

        public int? HizliReceteID { get; set; }

        public int? DetayliReceteID { get; set; }

        public int? KirmiziCizgiVarMi { get; set; }

        public int? HataKirilimID { get; set; }

        public int? KaliteHataKirilimID { get; set; }

        public int? UctanUcaVarMi { get; set; }

        public int? StokAdetVarMi { get; set; }

        [StringLength(50)]
        public string PastalToken { get; set; }

        public int? PastalID { get; set; }

        public int? GenelPlanlamaID { get; set; }

        public int? OrderTahsisVarMi { get; set; }

        public int? OlcumVarMi { get; set; }

        public int? OnMaliyetID { get; set; }

        public double? KumasCikisMetre { get; set; }

        public int? KumasCikisTopSayisi { get; set; }

        public DateTime? SonGuncellemeTarih { get; set; }

        public int? Boslar { get; set; }

        public int? Toplam { get; set; }
    }
}
