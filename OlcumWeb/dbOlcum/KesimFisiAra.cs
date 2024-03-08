namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KesimFisiAra")]
    public partial class KesimFisiAra
    {
        public int Id { get; set; }

        public int KesimFisiId { get; set; }

        public int? BedenId { get; set; }

        public int? BoyId { get; set; }

        public int? Adet { get; set; }

        public int? OlcuNoktaId { get; set; }

        [StringLength(150)]
        public string OlcuNoktaAd { get; set; }

        public virtual Bedenler Bedenler { get; set; }

        public virtual Boylar Boylar { get; set; }

        public virtual KesimFisiDetay KesimFisiDetay { get; set; }

        public virtual OlcuNoktalari OlcuNoktalari { get; set; }
    }
}
