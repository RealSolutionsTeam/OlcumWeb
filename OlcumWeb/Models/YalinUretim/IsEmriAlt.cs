namespace OlcumWeb.Models.YalinUretim
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IsEmriAlt")]
    public partial class IsEmriAlt
    {
        public int IsEmriAltID { get; set; }

        public int? BolumIsTurID { get; set; }

        [StringLength(50)]
        public string IsEmriAltNo { get; set; }

        [StringLength(50)]
        public string ModelKod { get; set; }

        [StringLength(200)]
        public string MusteriAd { get; set; }

        [StringLength(200)]
        public string IsEmriAdi { get; set; }

        public int? SipAdet { get; set; }

        public int? KesimAdet { get; set; }

        public bool? Durum { get; set; }

        public int? PlanlananKesimAdet { get; set; }

        public DateTime? SonGuncellemeTarihi { get; set; }

        public int? AtolyeID { get; set; }

        public int? UrunMudurID { get; set; }

        public bool? ReceteVarMi { get; set; }

        public DateTime? TerminTarih { get; set; }

        [StringLength(50)]
        public string PosetRengi { get; set; }

        public int? SentezYuklendiMi { get; set; }

        public DateTime? KazanGelisTarih { get; set; }

        [StringLength(50)]
        public string YikamaYeri { get; set; }

        public DateTime? DikimGelisTarih { get; set; }
    }
}
