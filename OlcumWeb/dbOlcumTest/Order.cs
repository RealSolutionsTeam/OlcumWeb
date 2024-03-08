namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string orderNo { get; set; }

        [StringLength(100)]
        public string musteri { get; set; }

        [StringLength(150)]
        public string modelAd { get; set; }

        [StringLength(100)]
        public string modelNo { get; set; }

        [StringLength(100)]
        public string realModelNo { get; set; }

        [StringLength(100)]
        public string kumas { get; set; }

        [StringLength(250)]
        public string fit { get; set; }

        [StringLength(250)]
        public string giysiTuru { get; set; }
    }
}
