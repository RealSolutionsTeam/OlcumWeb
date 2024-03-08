namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OlcuTabloAra")]
    public partial class OlcuTabloAra
    {
        public int id { get; set; }

        public int olcuTabloID { get; set; }

        public int olcuNoktaID { get; set; }

        public int bedenID { get; set; }

        public double deger { get; set; }

        [StringLength(500)]
        public string orijinalOlcuNok { get; set; }

        public bool? hataOldu { get; set; }
    }
}
