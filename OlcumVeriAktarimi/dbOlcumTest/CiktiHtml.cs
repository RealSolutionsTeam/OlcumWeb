namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CiktiHtml")]
    public partial class CiktiHtml
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int kazanDetayID { get; set; }

        [Required]
        public string stringHtml { get; set; }

        public virtual KazanDetay KazanDetay { get; set; }
    }
}
