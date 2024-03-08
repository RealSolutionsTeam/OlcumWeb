namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PersonelTablo")]
    public partial class PersonelTablo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PersonelTablo()
        {
            CogaltmaDetay = new HashSet<CogaltmaDetay>();
            HazirTabloDetay = new HashSet<HazirTabloDetay>();
            KazanDetay = new HashSet<KazanDetay>();
            KesimFisiDetay = new HashSet<KesimFisiDetay>();
            NumuneDetay = new HashSet<NumuneDetay>();
            OlcuTabloDetay = new HashSet<OlcuTabloDetay>();
            RaporDetay = new HashSet<RaporDetay>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string personelAd { get; set; }

        [StringLength(50)]
        public string personelSoyad { get; set; }

        [StringLength(75)]
        public string personelDepartman { get; set; }

        [StringLength(75)]
        public string personelGorev { get; set; }

        [StringLength(25)]
        public string personelkAdi { get; set; }

        [StringLength(25)]
        public string personelSifre { get; set; }

        public bool? personelAdminmi { get; set; }

        public int rolId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CogaltmaDetay> CogaltmaDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HazirTabloDetay> HazirTabloDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KazanDetay> KazanDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KesimFisiDetay> KesimFisiDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumuneDetay> NumuneDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OlcuTabloDetay> OlcuTabloDetay { get; set; }

        public virtual Roller Roller { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporDetay> RaporDetay { get; set; }
    }
}
