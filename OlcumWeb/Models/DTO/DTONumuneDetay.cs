using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DTONumuneDetay
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public string OrderNo { get; set; }

        public int? AtolyeId { get; set; } = 65;
        public string AtolyeAd { get; set; } 


        public int OlcuTuruId { get; set; } 
        public string OlcuTuruAd { get; set; } 

        public int? tabloTuru { get; set; }

        public DateTime? YikamayaGidis { get; set; } 

        public DateTime? YikamadanGelis { get; set; } 

        public string KalipAdi { get; set; } 

        public string TopNo { get; set; } 

        public string KID { get; set; } 

        public string ElliListe { get; set; } 

        public string ElliNumune { get; set; }

        public string DikimNedeni { get; set; } 

        public int? OlcumNo { get; set; } 

        public string Aciklama { get; set; }

        public DateTime Tarih { get; set; } 

        public int KullaniciId { get; set; } 
        public string KullaniciAd { get; set; } 

        public string ReceteKod { get; set; } 

        public bool? aktarimMi { get; set; } 

        public int? UpdatedUserId { get; set; }
        public string UpdatedUserAd { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? BagliOrderId { get; set; }

        public bool? HasPreviousOrder { get; set; }
        public int ReferansOrderId { get; set; }

        public string Kumas { get; set; }

        public string Musteri { get; set; } 

        public string ModelAd { get; set; }


    }
}