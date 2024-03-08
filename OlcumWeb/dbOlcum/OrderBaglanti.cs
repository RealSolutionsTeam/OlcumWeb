namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderBaglanti")]
    public partial class OrderBaglanti
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(50)]
        public string tasarimOrder { get; set; }

        [StringLength(50)]
        public string bagliOrder { get; set; }
    }
}
