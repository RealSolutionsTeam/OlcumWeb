namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NumuneDetay")]
    public partial class NumuneDetay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NumuneDetay()
        {
            NumuneAra = new HashSet<NumuneAra>();
            NumuneAra1 = new HashSet<NumuneAra>();
        }

        public int id { get; set; }

        public int orderID { get; set; }

        public int? atolyeID { get; set; }

        public int giysiTuruID { get; set; }

        public int olcuTuruID { get; set; }

        public int? tabloTuru { get; set; }

        [StringLength(100)]
        public string yikamaYeri { get; set; }

        [Column(TypeName = "date")]
        public DateTime? yikamayaGidis { get; set; }

        [Column(TypeName = "date")]
        public DateTime? yikamadanGelis { get; set; }

        [StringLength(100)]
        public string kalipAdi { get; set; }

        [StringLength(100)]
        public string topNo { get; set; }

        [StringLength(50)]
        public string KID { get; set; }

        [StringLength(50)]
        public string elliListe { get; set; }

        [StringLength(50)]
        public string elliNumune { get; set; }

        [StringLength(100)]
        public string dikimNedeni { get; set; }

        public int? olcumNo { get; set; }

        public string aciklama { get; set; }

        public DateTime tarih { get; set; }

        public int kullaniciID { get; set; }

        [StringLength(50)]
        public string receteKod { get; set; }

        public bool? aktarimMi { get; set; }

        public int? updatedUserId { get; set; }

        public DateTime? updatedDate { get; set; }

        public int? bagliOrderId { get; set; }

        public bool? hasPreviousOrder { get; set; }

        public int? oldOfTabloId { get; set; }

        public int? referansOrderId { get; set; }

        public virtual GiysiTurleri GiysiTurleri { get; set; }

        public virtual GiysiTurleri GiysiTurleri1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumuneAra> NumuneAra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumuneAra> NumuneAra1 { get; set; }

        public virtual OlcuTurleri OlcuTurleri { get; set; }

        public virtual Order Order { get; set; }

        public virtual Order Order1 { get; set; }

        public virtual PersonelTablo PersonelTablo { get; set; }
    }
}
