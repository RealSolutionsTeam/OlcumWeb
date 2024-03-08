namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RaporOkundus
    {
        public int Id { get; set; }

        public int? KontrolId { get; set; }

        public int? UserId { get; set; }
    }
}
