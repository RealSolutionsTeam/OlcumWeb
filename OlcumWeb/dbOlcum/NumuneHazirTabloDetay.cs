namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NumuneHazirTabloDetay")]
    public partial class NumuneHazirTabloDetay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NumuneHazirTabloDetay()
        {
            NumuneHazirTabloAra = new HashSet<NumuneHazirTabloAra>();
        }

        public int Id { get; set; }

        public int MusteriId { get; set; }

        public int GiysiTuruId { get; set; }

        public virtual GiysiTurleri GiysiTurleri { get; set; }

        public virtual Musteriler Musteriler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NumuneHazirTabloAra> NumuneHazirTabloAra { get; set; }
    }
}
