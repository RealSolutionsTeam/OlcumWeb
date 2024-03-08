namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Hatas
    {
        public int Id { get; set; }

        public string HataAd { get; set; }

        public int? RootId { get; set; }

        public string Url { get; set; }

        public int? Puan { get; set; }

        public bool? Durum { get; set; }

        public int? DepartmanId { get; set; }

        public int? SiraNo { get; set; }
    }
}
