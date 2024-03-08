namespace OlcumVeriAktarimi.dbOlcumRaporlama
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OlcuTablosuBeden")]
    public partial class OlcuTablosuBeden
    {
        [Key]
        [Column(Order = 0)]
        public int otBedenID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ottabloID { get; set; }

        [StringLength(500)]
        public string olcumNoktasi { get; set; }

        [StringLength(30)]
        public string olcuBedeni { get; set; }

        public double? olcu1 { get; set; }

        [StringLength(30)]
        public string olcuBedeni2 { get; set; }

        public double? olcu2 { get; set; }

        [StringLength(30)]
        public string olcuBedeni3 { get; set; }

        public double? olcu3 { get; set; }

        [StringLength(30)]
        public string olcuBedeni4 { get; set; }

        public double? olcu4 { get; set; }

        [StringLength(30)]
        public string olcuBedeni5 { get; set; }

        public double? olcu5 { get; set; }

        [StringLength(30)]
        public string olcuBedeni6 { get; set; }

        public double? olcu6 { get; set; }

        [StringLength(30)]
        public string olcuBedeni7 { get; set; }

        public double? olcu7 { get; set; }

        [StringLength(30)]
        public string olcuBedeni8 { get; set; }

        public double? olcu8 { get; set; }

        [StringLength(30)]
        public string olcuBedeni9 { get; set; }

        public double? olcu9 { get; set; }

        [StringLength(30)]
        public string olcuBedeni10 { get; set; }

        public double? olcu10 { get; set; }

        [StringLength(30)]
        public string olcuBedeni11 { get; set; }

        public double? olcu11 { get; set; }

        [StringLength(30)]
        public string olcuBedeni12 { get; set; }

        public double? olcu12 { get; set; }

        [StringLength(30)]
        public string olcuBedeni13 { get; set; }

        public double? olcu13 { get; set; }

        [StringLength(30)]
        public string olcuBedeni14 { get; set; }

        public double? olcu14 { get; set; }

        [StringLength(30)]
        public string olcuBedeni15 { get; set; }

        public double? olcu15 { get; set; }

        public double? tolerans { get; set; }

        public double? cekme { get; set; }

        public double? oran { get; set; }

        public int? satirIndexi { get; set; }
    }
}
