namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NumuneAra")]
    public partial class NumuneAra
    {
        public int id { get; set; }

        public int numuneDetayID { get; set; }

        public int olcuNoktaID { get; set; }

        public int bedenID { get; set; }

        public double? kalipOlcusu { get; set; }

        public double? uygulananTolerans { get; set; }

        public double? yikamaOncesiTablo { get; set; }

        public double? uygulananCekme { get; set; }

        public double? yikamaSonrasiTablo { get; set; }

        public double? gerceklesenTolerans { get; set; }

        public double? yikamaOncesiOlculen { get; set; }

        public double? gerceklesenCekme { get; set; }

        public double? yikamaSonrasiOlculen { get; set; }

        public double? yikamaOncesiFark { get; set; }

        public double? yikamaSonrasiFark { get; set; }

        public bool mudahaleMi { get; set; }

        public virtual Bedenler Bedenler { get; set; }

        public virtual NumuneDetay NumuneDetay { get; set; }

        public virtual NumuneDetay NumuneDetay1 { get; set; }

        public virtual OlcuNoktalari OlcuNoktalari { get; set; }
    }
}
