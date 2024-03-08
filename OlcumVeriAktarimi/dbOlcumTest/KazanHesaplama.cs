namespace OlcumVeriAktarimi.dbOlcumTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KazanHesaplama")]
    public partial class KazanHesaplama
    {
        public int id { get; set; }

        public int kazanDetayID { get; set; }

        public int olcuNoktasiID { get; set; }

        public double ortalamaDeger { get; set; }

        public double yoOlculen { get; set; }

        public double gerceklesenTolCekme { get; set; }

        public double oncekiTablo { get; set; }

        public double uygulananTolCekme { get; set; }

        public double asilTablo { get; set; }

        public virtual KazanDetay KazanDetay { get; set; }
    }
}
