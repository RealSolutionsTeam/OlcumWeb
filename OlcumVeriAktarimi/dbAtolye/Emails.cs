namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Emails
    {
        public int Id { get; set; }

        public string EmailAddress { get; set; }

        public int? Type { get; set; }
    }
}
