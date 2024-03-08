namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Resims
    {
        public int Id { get; set; }

        public byte[] ResimUrl { get; set; }

        public DateTime? Tarih { get; set; }

        public int? FinalKontrol2Id { get; set; }

        public int? KullaniciId { get; set; }

        public int? InlineKontrol2Id { get; set; }
    }
}
