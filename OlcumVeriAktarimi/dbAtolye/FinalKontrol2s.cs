namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FinalKontrol2s
    {
        public int Id { get; set; }

        public int? KontrolId { get; set; }

        public DateTime? Tarih { get; set; }

        public string OperasyonAd { get; set; }

        public string HataAd { get; set; }

        public int? Adet { get; set; }

        public int? KullaniciId { get; set; }

        public string Aciklama { get; set; }
    }
}
