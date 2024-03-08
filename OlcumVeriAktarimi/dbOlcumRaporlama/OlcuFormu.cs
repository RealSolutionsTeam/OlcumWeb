namespace OlcumVeriAktarimi.dbOlcumRaporlama
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OlcuFormu")]
    public partial class OlcuFormu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OlcuFormu()
        {
            OlcuFormuBeden = new HashSet<OlcuFormuBeden>();
        }

        [Key]
        public int oftabloID { get; set; }

        [Required]
        [StringLength(200)]
        public string orderNo { get; set; }

        [StringLength(70)]
        public string dikimAtolyesi { get; set; }

        [StringLength(70)]
        public string yikamaYeri { get; set; }

        [StringLength(70)]
        public string yikamaSorumlu { get; set; }

        [Column(TypeName = "date")]
        public DateTime? tarih { get; set; }

        [StringLength(25)]
        public string olcuTuru { get; set; }

        [StringLength(25)]
        public string giysiTuru { get; set; }

        public int? oftabloTuru { get; set; }

        public int? karsitNo { get; set; }

        public string aciklama { get; set; }

        public int? olcuTuruNo { get; set; }

        [StringLength(25)]
        public string aparat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OlcuFormuBeden> OlcuFormuBeden { get; set; }
    }
}
