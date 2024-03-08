namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RaporBagliOrder")]
    public partial class RaporBagliOrder
    {
        public int id { get; set; }

        public int raporId { get; set; }

        public int orderId { get; set; }

        [Required]
        [StringLength(50)]
        public string orderNo { get; set; }

        public virtual Order Order { get; set; }

        public virtual RaporDetay RaporDetay { get; set; }
    }
}
