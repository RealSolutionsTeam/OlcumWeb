namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PArrays
    {
        public int Id { get; set; }

        public int? KontrolId { get; set; }

        public int? No { get; set; }

        public bool? Value { get; set; }
    }
}
