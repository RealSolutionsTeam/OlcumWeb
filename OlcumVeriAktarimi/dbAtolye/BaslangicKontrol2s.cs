namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BaslangicKontrol2s
    {
        public int Id { get; set; }

        public int? KontrolId { get; set; }

        public string Baslik { get; set; }

        public DateTime? SaglamTarih { get; set; }

        public DateTime? HataTarih { get; set; }

        public int? KullaniciId { get; set; }
    }
}
