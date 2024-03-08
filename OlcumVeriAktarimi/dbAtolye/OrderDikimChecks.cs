namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDikimChecks
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public bool? CheckKontrol { get; set; }
    }
}
