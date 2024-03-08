namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KazanBarkod")]
    public partial class KazanBarkod
    {
        public int Id { get; set; }

        public int KazanDetayId { get; set; }

        public int? BedenId { get; set; }

        [StringLength(128)]
        public string BarcodeNo { get; set; }

        [StringLength(50)]
        public string TopNo { get; set; }

        [StringLength(20)]
        public string EnBoyCekme { get; set; }

        public int? PantNo { get; set; }

        public int? BedenPantNo { get; set; }

        public virtual Bedenler Bedenler { get; set; }

        public virtual KazanDetay KazanDetay { get; set; }
    }
}
