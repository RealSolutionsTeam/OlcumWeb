namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            CogaltmaDetay = new HashSet<CogaltmaDetay>();
            KazanDetay = new HashSet<KazanDetay>();
            KesimFisiDetay = new HashSet<KesimFisiDetay>();
            NumuneDetay = new HashSet<NumuneDetay>();
            NumuneDetay1 = new HashSet<NumuneDetay>();
            OlcuTabloDetay = new HashSet<OlcuTabloDetay>();
            RaporAra = new HashSet<RaporAra>();
            RaporBagliOrder = new HashSet<RaporBagliOrder>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string orderNo { get; set; }

        [StringLength(100)]
        public string musteri { get; set; }

        [StringLength(150)]
        public string modelAd { get; set; }

        [StringLength(100)]
        public string modelNo { get; set; }

        [StringLength(100)]
        public string realModelNo { get; set; }

        [StringLength(100)]
        public string kumas { get; set; }

        [StringLength(250)]
        public string fit { get; set; }

        [StringLength(250)]
        public string giysiTuru { get; set; }

        [StringLength(50)]
        public string orderOlcuTuru { get; set; }

        [StringLength(50)]
        public string orderYikamaYeri { get; set; }

        [StringLength(50)]
        public string orderReceteKodu { get; set; }

        public bool? isNumune { get; set; }

        [StringLength(30)]
        public string orderSafhasi { get; set; }

        [StringLength(30)]
        public string orderTuru { get; set; }

        [StringLength(30)]
        public string orderTipi { get; set; }

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
        public virtual ICollection<OlcuTabloDetay> OlcuTabloDetay { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporAra> RaporAra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaporBagliOrder> RaporBagliOrder { get; set; }
    }
}
