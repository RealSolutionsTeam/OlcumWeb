namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rapors
    {
        public int Id { get; set; }

        public int? KontrolId { get; set; }

        public int? KontrolAdet { get; set; }

        public int? SaglamAdet { get; set; }

        public int? HatalıAdet { get; set; }

        public int? HataNokAdet { get; set; }

        public byte[] Resim { get; set; }
    }
}
