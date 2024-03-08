using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class KazanBarkodDto
    {
        public int Id { get; set; }
        public int KazanDetayId { get; set; }
        public int? BedenId { get; set; }
        public string BedenAd { get; set; }
        public string BarcodeNo { get; set; }
        public string TopNo { get; set; }
        public string EnBoyCekme { get; set; }
        public int? PantNo { get; set; }
        public int? BedenPantNo { get; set; }
    }
}