namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Modeller")]
    public partial class Modeller
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Modeller()
        {
            HazirTabloDetay = new HashSet<HazirTabloDetay>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(400)]
        public string modelAd { get; set; }

        public int musteriID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HazirTabloDetay> HazirTabloDetay { get; set; }

        public virtual Musteriler Musteriler { get; set; }
    }
}
