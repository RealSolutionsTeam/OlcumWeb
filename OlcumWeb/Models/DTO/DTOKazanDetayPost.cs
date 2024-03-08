using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class DTOKazanDetayPost
    {
        public int kullaniciId { get; set; }
        public int orderID { get; set; }
        public int atolyeID { get; set; }
        public int giysiTuruID { get; set; }
        public int olcuTuruID { get; set; }
        public int tabloTurId { get; set; }
        public int utuId { get; set; }
        public int oTolcuTurId { get; set; }
        public int kontrolId { get; set; }
        public int inspectionId { get; set; }
        public bool aparatliMi { get; set; }
        public bool utuluMu { get; set; }
        public int urunKabulId { get; set; }

    }
}