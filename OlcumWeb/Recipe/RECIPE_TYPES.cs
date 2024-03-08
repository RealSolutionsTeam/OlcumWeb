namespace OlcumWeb.Recipe
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RECIPE_TYPES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RECIPE_TYPES()
        {
            RECIPE = new HashSet<RECIPE>();
        }

        public int ID { get; set; }

        [StringLength(100)]
        public string NAME { get; set; }

        public int? NO { get; set; }

        [StringLength(50)]
        public string CHOOSE_TYPE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RECIPE> RECIPE { get; set; }
    }
}
