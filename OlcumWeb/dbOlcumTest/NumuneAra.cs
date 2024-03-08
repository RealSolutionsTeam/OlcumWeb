namespace OlcumWeb.dbOlcumTest
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

        public double? imalat_Kalip { get; set; }

        public double? imalat_Tol { get; set; }

        public double? imalat_YO { get; set; }

        public double? imalat_Cekme { get; set; }

        public double? kalipOlcusu { get; set; }

        public double? kalipFark { get; set; }

        public double? yikOncesi { get; set; }

        public double? yikCekme { get; set; }

        public double? yikSonrasi { get; set; }

        public double? yikamaFark { get; set; }

        public double? ysonTablo { get; set; }

        public double? ortTolerans { get; set; }

        public double? ortCekme { get; set; }

        public bool mudahaleMi { get; set; }
    }
}
