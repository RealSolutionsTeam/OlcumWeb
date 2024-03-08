namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KesimFisiDetay")]
    public partial class KesimFisiDetay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KesimFisiDetay()
        {
            KesimFisiAra = new HashSet<KesimFisiAra>();
        }

        public int Id { get; set; }

        public int OrderId { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        public int KullaniciId { get; set; }

        public int GiysiTuruId { get; set; }

        public int OlcuTuruId { get; set; }

        [StringLength(450)]
        public string Garni1 { get; set; }

        [StringLength(450)]
        public string Garni2 { get; set; }

        [StringLength(450)]
        public string Garni3 { get; set; }

        [StringLength(450)]
        public string Tela { get; set; }

        [StringLength(100)]
        public string Cekme { get; set; }

        [StringLength(100)]
        public string KumasEni { get; set; }

        [StringLength(100)]
        public string TopNo { get; set; }

        [StringLength(100)]
        public string Baski { get; set; }

        [StringLength(100)]
        public string Nakis { get; set; }

        public string aciklama { get; set; }

        public virtual GiysiTurleri GiysiTurleri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KesimFisiAra> KesimFisiAra { get; set; }

        public virtual OlcuTurleri OlcuTurleri { get; set; }

        public virtual Order Order { get; set; }

        public virtual PersonelTablo PersonelTablo { get; set; }
    }
}
