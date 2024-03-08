namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HazirTabloDetay")]
    public partial class HazirTabloDetay
    {
        public int id { get; set; }

        public int modelID { get; set; }

        public int kullanıcıID { get; set; }

        public DateTime? tarih { get; set; }

        [StringLength(250)]
        public string tabloAd { get; set; }
    }
}
