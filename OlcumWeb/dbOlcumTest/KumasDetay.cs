namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KumasDetay")]
    public partial class KumasDetay
    {
        public int id { get; set; }

        public int orderID { get; set; }

        [StringLength(250)]
        public string kumasCins { get; set; }

        [StringLength(250)]
        public string kumasLokasyonu { get; set; }

        [StringLength(250)]
        public string kumasMetre { get; set; }
    }
}
