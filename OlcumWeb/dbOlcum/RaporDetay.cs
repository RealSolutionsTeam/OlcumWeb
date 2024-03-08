namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RaporDetay")]
    public partial class RaporDetay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RaporDetay()
        {
            RaporAnaCekme = new HashSet<RaporAnaCekme>();
            RaporAra = new HashSet<RaporAra>();
            RaporBagliOrder = new HashSet<RaporBagliOrder>();
            RaporHesaplama = new HashSet<RaporHesaplama>();
            RaporOrtalama = new HashSet<RaporOrtalama>();
        }

        public int id { get; set; }

        [StringLength(150)]
        public string raporAdi { get; set; }

        public int? tabloTuru { get; set; }

        public DateTime? tarih { get; set; }

        public int? kullaniciID { get; set; }

        public int? kalipNo { get; set; }

        public int? anaCekmeVarMi { get; set; }

        public int? anaCekmeRaporId { get; set; }

        public string raporNotu { get; set; }

        public virtual PersonelTablo PersonelTablo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporAnaCekme> RaporAnaCekme { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporAra> RaporAra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporBagliOrder> RaporBagliOrder { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporHesaplama> RaporHesaplama { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporOrtalama> RaporOrtalama { get; set; }
    }
}
