using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    [Serializable]
    public class DtoNumuneAra
    {
        public int Id { get; set; }
        public int NumuneDetayId { get; set; }
        public int OlcuNoktaId { get; set; }
        public string OlcuNoktaAd { get; set; }
        public int BedenId { get; set; }
        public string BedenAd { get; set; }
        public double KalipOlcusu { get; set; }
        public double UygulananTolerans { get; set; }
        public double YikamaOncesiTablo { get; set; }
        public double UygulananCekme { get; set; }
        public double YikamaSonrasiTablo { get; set; }
        public double GerceklesenTolerans { get; set; }

        public double YikamaOncesiOlculen { get; set; }
        public double GerceklesenCekme { get; set; }
        public double YikamaSonrasiOlculen { get; set; }
        public double YikamaSonrasiFark { get; set; }
        public double YikamaOncesiFark { get; set; }
        public bool MudahaleMi { get; set; }
        public int IsNumuneReference { get; set; }

    }
}