namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KimyasalEkipLiders
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Ad { get; set; }

        public int? KimyasalEkipId { get; set; }
    }
}
