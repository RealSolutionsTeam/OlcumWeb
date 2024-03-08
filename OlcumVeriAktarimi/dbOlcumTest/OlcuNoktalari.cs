namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OlcuNoktalari")]
    public partial class OlcuNoktalari
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OlcuNoktalari()
        {
            CogaltmaAra = new HashSet<CogaltmaAra>();
            HazirTabloAra = new HashSet<HazirTabloAra>();
            KazanAra = new HashSet<KazanAra>();
            KesimFisiAra = new HashSet<KesimFisiAra>();
            NumuneAra = new HashSet<NumuneAra>();
            NumuneHazirTabloAra = new HashSet<NumuneHazirTabloAra>();
            OlcuTabloAra = new HashSet<OlcuTabloAra>();
            OlcuTabloHesaplama = new HashSet<OlcuTabloHesaplama>();
            RaporAnaCekme = new HashSet<RaporAnaCekme>();
            RaporHesaplama = new HashSet<RaporHesaplama>();
            RaporOrtalama = new HashSet<RaporOrtalama>();
        }

        public int id { get; set; }

        public int? giysiTuruID { get; set; }

        [Required]
        [StringLength(150)]
        public string olcuNoktasi { get; set; }

        public int tabanID { get; set; }

        public bool? anaNoktami { get; set; }

        public bool? enBoy { get; set; }

        public double? siraNo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CogaltmaAra> CogaltmaAra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HazirTabloAra> HazirTabloAra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KazanAra> KazanAra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KesimFisiAra> KesimFisiAra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumuneAra> NumuneAra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumuneHazirTabloAra> NumuneHazirTabloAra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OlcuTabloAra> OlcuTabloAra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OlcuTabloHesaplama> OlcuTabloHesaplama { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporAnaCekme> RaporAnaCekme { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporHesaplama> RaporHesaplama { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporOrtalama> RaporOrtalama { get; set; }
    }
}
