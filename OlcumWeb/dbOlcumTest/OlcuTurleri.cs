namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OlcuTurleri")]
    public partial class OlcuTurleri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OlcuTurleri()
        {
            KazanDetays = new HashSet<KazanDetay>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(25)]
        public string olcuTuruAd { get; set; }

        [StringLength(25)]
        public string tabloTuru { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KazanDetay> KazanDetays { get; set; }
    }
}
