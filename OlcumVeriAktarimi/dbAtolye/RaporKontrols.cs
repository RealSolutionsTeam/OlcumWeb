namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RaporKontrols
    {
        public int Id { get; set; }

        public int? KontrolId { get; set; }

        public string KontrolEdenler { get; set; }
    }
}
