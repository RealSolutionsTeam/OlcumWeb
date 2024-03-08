namespace OlcumVeriAktarimi.dbOlcumRaporlama
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OlcuFormuBeden")]
    public partial class OlcuFormuBeden
    {
        [Key]
        public int ofBedenID { get; set; }

        public int? oftabloID { get; set; }

        [StringLength(150)]
        public string olcuNoktasi { get; set; }

        [StringLength(37)]
        public string olcuBedeni1 { get; set; }

        public double? olcum11 { get; set; }

        public double? olcum12 { get; set; }

        public double? olcum13 { get; set; }

        public double? olcum14 { get; set; }

        public double? olcum10 { get; set; }

        [StringLength(37)]
        public string olcuBedeni2 { get; set; }

        public double? olcum21 { get; set; }

        public double? olcum22 { get; set; }

        public double? olcum23 { get; set; }

        public double? olcum24 { get; set; }

        public double? olcum20 { get; set; }

        [StringLength(37)]
        public string olcuBedeni3 { get; set; }

        public double? olcum31 { get; set; }

        public double? olcum32 { get; set; }

        public double? olcum33 { get; set; }

        public double? olcum34 { get; set; }

        public double? olcum30 { get; set; }

        [StringLength(37)]
        public string olcuBedeni4 { get; set; }

        public double? olcum41 { get; set; }

        public double? olcum42 { get; set; }

        public double? olcum43 { get; set; }

        public double? olcum44 { get; set; }

        public double? olcum40 { get; set; }

        [StringLength(37)]
        public string olcuBedeni5 { get; set; }

        public double? olcum51 { get; set; }

        public double? olcum52 { get; set; }

        public double? olcum53 { get; set; }

        public double? olcum54 { get; set; }

        public double? olcum50 { get; set; }

        [StringLength(37)]
        public string olcuBedeni6 { get; set; }

        public double? olcum61 { get; set; }

        public double? olcum62 { get; set; }

        public double? olcum63 { get; set; }

        public double? olcum64 { get; set; }

        public double? olcum60 { get; set; }

        [StringLength(37)]
        public string olcuBedeni7 { get; set; }

        public double? olcum71 { get; set; }

        public double? olcum72 { get; set; }

        public double? olcum73 { get; set; }

        public double? olcum74 { get; set; }

        public double? olcum70 { get; set; }

        [StringLength(37)]
        public string olcuBedeni8 { get; set; }

        public double? olcum81 { get; set; }

        public double? olcum82 { get; set; }

        public double? olcum83 { get; set; }

        public double? olcum84 { get; set; }

        public double? olcum80 { get; set; }

        [StringLength(37)]
        public string olcuBedeni9 { get; set; }

        public double? olcum91 { get; set; }

        public double? olcum92 { get; set; }

        public double? olcum93 { get; set; }

        public double? olcum94 { get; set; }

        public double? olcum90 { get; set; }

        [StringLength(37)]
        public string olcuBedeni10 { get; set; }

        public double? olcum101 { get; set; }

        public double? olcum102 { get; set; }

        public double? olcum103 { get; set; }

        public double? olcum104 { get; set; }

        public double? olcum100 { get; set; }

        [StringLength(37)]
        public string olcuBedeni11 { get; set; }

        public double? olcum111 { get; set; }

        public double? olcum112 { get; set; }

        public double? olcum113 { get; set; }

        public double? olcum114 { get; set; }

        public double? olcum110 { get; set; }

        [StringLength(37)]
        public string olcuBedeni12 { get; set; }

        public double? olcum121 { get; set; }

        public double? olcum122 { get; set; }

        public double? olcum123 { get; set; }

        public double? olcum124 { get; set; }

        public double? olcum120 { get; set; }

        [StringLength(37)]
        public string olcuBedeni13 { get; set; }

        public double? olcum131 { get; set; }

        public double? olcum132 { get; set; }

        public double? olcum133 { get; set; }

        public double? olcum134 { get; set; }

        public double? olcum130 { get; set; }

        [StringLength(37)]
        public string olcuBedeni14 { get; set; }

        public double? olcum141 { get; set; }

        public double? olcum142 { get; set; }

        public double? olcum143 { get; set; }

        public double? olcum144 { get; set; }

        public double? olcum140 { get; set; }

        [StringLength(37)]
        public string olcuBedeni15 { get; set; }

        public double? olcum151 { get; set; }

        public double? olcum152 { get; set; }

        public double? olcum153 { get; set; }

        public double? olcum154 { get; set; }

        public double? olcum150 { get; set; }

        public double? ortalamaDeger { get; set; }

        public double? yoOlculen { get; set; }

        public double? gerceklesenTolCekme { get; set; }

        public double? oncekiTablo { get; set; }

        public double? uygulananTolCekme { get; set; }

        public double? asılTablo { get; set; }

        public virtual OlcuFormu OlcuFormu { get; set; }
    }
}
