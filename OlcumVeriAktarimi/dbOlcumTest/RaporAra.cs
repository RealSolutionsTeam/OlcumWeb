namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RaporAra")]
    public partial class RaporAra
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RaporAra()
        {
            RaporHesaplama = new HashSet<RaporHesaplama>();
        }

        public int id { get; set; }

        public int raporID { get; set; }

        public int orderID { get; set; }

        [Required]
        [StringLength(25)]
        public string orderNo { get; set; }

        public int olcuTuruID { get; set; }

        [Required]
        [StringLength(50)]
        public string olcuTuruAd { get; set; }

        [StringLength(10)]
        public string enBoyCekme { get; set; }

        public int? kazanDetayID { get; set; }

        public int? raporTuru { get; set; }

        [StringLength(50)]
        public string beden { get; set; }

        [StringLength(50)]
        public string receteKod { get; set; }

        public virtual OlcuTurleri OlcuTurleri { get; set; }

        public virtual Order Order { get; set; }

        public virtual RaporDetay RaporDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporHesaplama> RaporHesaplama { get; set; }
    }
}
