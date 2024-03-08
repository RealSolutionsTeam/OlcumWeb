namespace OlcumWeb.dbOlcum
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
            CogaltmaDetay = new HashSet<CogaltmaDetay>();
            KazanDetay = new HashSet<KazanDetay>();
            KesimFisiDetay = new HashSet<KesimFisiDetay>();
            NumuneDetay = new HashSet<NumuneDetay>();
            OlcuTabloDetay = new HashSet<OlcuTabloDetay>();
            RaporAra = new HashSet<RaporAra>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(25)]
        public string olcuTuruAd { get; set; }

        [StringLength(25)]
        public string tabloTuru { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CogaltmaDetay> CogaltmaDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KazanDetay> KazanDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KesimFisiDetay> KesimFisiDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumuneDetay> NumuneDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OlcuTabloDetay> OlcuTabloDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporAra> RaporAra { get; set; }
    }
}
