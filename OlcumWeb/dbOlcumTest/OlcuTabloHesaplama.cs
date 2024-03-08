namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OlcuTabloHesaplama")]
    public partial class OlcuTabloHesaplama
    {
        public int id { get; set; }

        public int olcuTabloID { get; set; }

        public int olcuNoktaID { get; set; }

        public double? tolerans { get; set; }

        public double? cekme { get; set; }

        public double? oran { get; set; }
    }
}
