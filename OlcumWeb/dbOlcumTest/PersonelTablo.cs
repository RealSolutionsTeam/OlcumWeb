namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PersonelTablo")]
    public partial class PersonelTablo
    {
        public int id { get; set; }

        [StringLength(50)]
        public string personelAd { get; set; }

        [StringLength(50)]
        public string personelSoyad { get; set; }

        [StringLength(75)]
        public string personelDepartman { get; set; }

        [StringLength(75)]
        public string personelGorev { get; set; }

        [StringLength(25)]
        public string personelkAdi { get; set; }

        [StringLength(25)]
        public string personelSifre { get; set; }

        public bool? personelAdminmi { get; set; }

        public int rolId { get; set; }
    }
}
