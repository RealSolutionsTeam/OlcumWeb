namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CogaltmaAra")]
    public partial class CogaltmaAra
    {
        public int id { get; set; }

        public int cogaltmaDetayID { get; set; }

        public int olcuNoktaID { get; set; }

        public int bedenID { get; set; }

        public double? kalipOlcusu { get; set; }

        public double? tolerans { get; set; }

        public double? yoOlcu { get; set; }

        public double? yoFark { get; set; }

        public double? yoOlculen { get; set; }

        public double? cekme { get; set; }

        public double? ysOlculen { get; set; }

        public double? ysFark { get; set; }

        public double? ysOlcu { get; set; }

        public bool mudahaleMi { get; set; }

        public virtual Bedenler Bedenler { get; set; }

        public virtual CogaltmaDetay CogaltmaDetay { get; set; }

        public virtual OlcuNoktalari OlcuNoktalari { get; set; }
    }
}
