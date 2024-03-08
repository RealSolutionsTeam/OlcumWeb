namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PHatas
    {
        public int Id { get; set; }

        public string Operasyonad { get; set; }

        public string Hataad { get; set; }

        public int? PArraysId { get; set; }
    }
}
