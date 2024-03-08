namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HazirTabloDetay")]
    public partial class HazirTabloDetay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HazirTabloDetay()
        {
            HazirTabloAra = new HashSet<HazirTabloAra>();
        }

        public int id { get; set; }

        public int modelID { get; set; }

        public int kullanıcıID { get; set; }

        public DateTime? tarih { get; set; }

        [StringLength(250)]
        public string tabloAd { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HazirTabloAra> HazirTabloAra { get; set; }

        public virtual Modeller Modeller { get; set; }

        public virtual PersonelTablo PersonelTablo { get; set; }
    }
}
