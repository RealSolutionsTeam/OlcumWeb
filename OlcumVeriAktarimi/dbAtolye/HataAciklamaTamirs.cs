namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HataAciklamaTamirs
    {
        public int Id { get; set; }

        public int? HataId { get; set; }

        public string Aciklama { get; set; }

        public int? TurId { get; set; }
    }
}
