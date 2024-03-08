namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GiysiTurleri")]
    public partial class GiysiTurleri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GiysiTurleri()
        {
            CogaltmaDetay = new HashSet<CogaltmaDetay>();
            KazanDetay = new HashSet<KazanDetay>();
            KesimFisiDetay = new HashSet<KesimFisiDetay>();
            NumuneDetay = new HashSet<NumuneDetay>();
            NumuneDetay1 = new HashSet<NumuneDetay>();
            NumuneHazirTabloDetay = new HashSet<NumuneHazirTabloDetay>();
            OlcuTabloDetay = new HashSet<OlcuTabloDetay>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string giysiTuruAd { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CogaltmaDetay> CogaltmaDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KazanDetay> KazanDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KesimFisiDetay> KesimFisiDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumuneDetay> NumuneDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumuneDetay> NumuneDetay1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumuneHazirTabloDetay> NumuneHazirTabloDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OlcuTabloDetay> OlcuTabloDetay { get; set; }
    }
}
