namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InlineKontrol2s
    {
        public int Id { get; set; }

        public string OperasyonAd { get; set; }

        public int? KontrolAdet { get; set; }

        public int? HataAdet { get; set; }

        public int? KullaniciId { get; set; }

        public string HataAyrinti { get; set; }

        public DateTime? Tarih { get; set; }

        public string Aciklama { get; set; }

        public int? KontrolId { get; set; }

        public int? TamirYuzdesi { get; set; }

        [StringLength(50)]
        public string Olcum { get; set; }

        [StringLength(500)]
        public string Karar { get; set; }
    }
}
