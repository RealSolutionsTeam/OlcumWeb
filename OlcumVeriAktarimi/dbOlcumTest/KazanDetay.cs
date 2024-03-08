namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KazanDetay")]
    public partial class KazanDetay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KazanDetay()
        {
            KazanAra = new HashSet<KazanAra>();
            KazanHesaplama = new HashSet<KazanHesaplama>();
        }

        public int id { get; set; }

        public int orderID { get; set; }

        public int atolyeID { get; set; }

        public int? giysiTuruID { get; set; }

        public int olcuTuruID { get; set; }

        [StringLength(50)]
        public string yikamaYeri { get; set; }

        [StringLength(50)]
        public string yikamaSorumlu { get; set; }

        public int tabloTuru { get; set; }

        public int? olcumNo { get; set; }

        public string aciklama { get; set; }

        public DateTime tarih { get; set; }

        public int kullaniciID { get; set; }

        public bool aparatliMi { get; set; }

        public bool? aktarimMi { get; set; }

        public int utuPaketID { get; set; }

        public bool? durum { get; set; }

        public int olcuTabloOlcuTurId { get; set; }

        public int kaliteKontrolId { get; set; }

        public int kaliteInspectionId { get; set; }

        public int? kDerece { get; set; }

        public int? kDakika { get; set; }

        public bool? utuluMu { get; set; }

        public int? kaliteUrunKabulId { get; set; }

        public virtual CiktiHtml CiktiHtml { get; set; }

        public virtual GiysiTurleri GiysiTurleri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KazanAra> KazanAra { get; set; }

        public virtual OlcuTurleri OlcuTurleri { get; set; }

        public virtual Order Order { get; set; }

        public virtual PersonelTablo PersonelTablo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KazanHesaplama> KazanHesaplama { get; set; }
    }
}
