namespace OlcumWeb.dbOlcum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KumasKarakteri")]
    public partial class KumasKarakteri
    {
        public int id { get; set; }

        [StringLength(250)]
        public string kumasKodu { get; set; }

        [StringLength(250)]
        public string kumasAdı { get; set; }

        public double? tamEnMin { get; set; }

        public double? tamEnMax { get; set; }

        public double? tamEnOrtalama { get; set; }

        public double? kesilebilirEnMin { get; set; }

        public double? kesilebilirEnMax { get; set; }

        public double? kesilebilirEnOrtalama { get; set; }

        public double? elastikiyetAtkiMin { get; set; }

        public double? elastikiyetAtkiMax { get; set; }

        public double? elastikiyetAtkiOrtalama { get; set; }

        public double? elastikiyetCozguMin { get; set; }

        public double? elastikiyetCozguMax { get; set; }

        public double? elastikiyetCozguOrtalama { get; set; }

        [StringLength(250)]
        public string konstrüksiyon { get; set; }

        [StringLength(250)]
        public string konstrüksiyonParametre1 { get; set; }

        public double? konstrüksiyonDeger1 { get; set; }

        [StringLength(250)]
        public string konstrüksiyonParametre2 { get; set; }

        public double? konstrüksiyonDeger2 { get; set; }

        [StringLength(250)]
        public string konstrüksiyonParametre3 { get; set; }

        public double? konstrüksiyonDeger3 { get; set; }

        [StringLength(250)]
        public string konstrüksiyonParametre4 { get; set; }

        public double? konstrüksiyonDeger4 { get; set; }

        [StringLength(250)]
        public string konstrüksiyonParametre5 { get; set; }

        public double? konstrüksiyonDeger5 { get; set; }

        [StringLength(250)]
        public string konstrüksiyonParametre6 { get; set; }

        public double? konstrüksiyonDeger6 { get; set; }

        public double? kumasEnCekmeMin { get; set; }

        public double? kumasEnCekmeMax { get; set; }

        public double? kumasEnCekmeOrtalama { get; set; }

        public double? kumasBoyCekmeMin { get; set; }

        public double? kumasBoyCekmeMax { get; set; }

        public double? kumasBoyCekmeOrtalama { get; set; }
    }
}
