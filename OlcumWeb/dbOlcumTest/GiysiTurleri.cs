namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GiysiTurleri")]
    public partial class GiysiTurleri
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string giysiTuruAd { get; set; }
    }
}
