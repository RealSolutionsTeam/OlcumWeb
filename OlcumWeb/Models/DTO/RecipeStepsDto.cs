using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Models.DTO
{
    public class RecipeStepsDto
    {
        public int ID { get; set; }

        public int? RECIPE_ID { get; set; }

        
        public string GUID { get; set; }

        public string TYPE { get; set; }

        public int? PARAM_ID { get; set; }

        public string PARAM_NAME { get; set; }

        public int? PERSONEL { get; set; }

        public string WASHING_DETAIL { get; set; }

        public int? USER_ID { get; set; }

        
        public string USER_NAME { get; set; }

        public int? MACHINE_ID { get; set; }

        
        public string MACHINE_NAME { get; set; }

        public bool? IS_HAVE_WASHING_TYPE { get; set; }

        
        public string WASHING_TYPE_NAME { get; set; }

        public double? TIME { get; set; }

        
        public string TEMPERATURE { get; set; }

        
        public string LR { get; set; }

        
        public string SIKMA_SANIYESI { get; set; }

        
        public string KURUTMA_SICAKLIK { get; set; }

        
        public string KURUTMA_DAKIKA { get; set; }

        
        public string BOLGE { get; set; }

        
        public string TUR_SAYISI { get; set; }

        
        public string FIRIN_1_DERECE { get; set; }

        
        public string FIRIN_2_DERECE { get; set; }

        
        public string FIRIN_1_DAKIKA { get; set; }

        
        public string FIRIN_2_DAKIKA { get; set; }

        
        public string PROJE { get; set; }

        
        public string YAKMA_DERECESI { get; set; }

        
        public string YAKMA_DERECESI2 { get; set; }

        
        public string YAKMA_SURESI { get; set; }

        
        public string DPI { get; set; }

        
        public string DPI2 { get; set; }

        
        public string ZIMPARA_DERECESI { get; set; }

        
        public string SARJ_ADEDI { get; set; }

        
        public string PH { get; set; }

        
        public string HAVLU_MIKTARI { get; set; }

        
        public string HAVLU_BOYUT { get; set; }

        
        public string HAVLU_DONME_DK { get; set; }

        
        public string HAVLU_SIKMA_DK { get; set; }

        
        public string RANDOM_DK { get; set; }

        
        public string SIKMA_DEVRI { get; set; }

        
        public string KOVA_TAS_MIKTARI { get; set; }

        public string FILE_TIPI { get; set; }

        public string IP_TIPI { get; set; }

        public string CUVAL_TAS_MIKTARI { get; set; }

        public string PANTOLON_SIKMA_DK { get; set; }

        public string ACIKLAMA { get; set; }

        public int? WASHING_TYPE_ID { get; set; }

        public int? SUB_MACHINE_ID { get; set; }

        public string SUB_MACHINE_NAME { get; set; }

        public bool? IS_USING_PLANNING { get; set; }

        public int? YURUTME_ADEDI { get; set; }

        public double? HAM_AGIRLIK { get; set; }

        public double? IS_FIRIN_3_DERECE { get; set; }

        public double? FIRIN_3_DAKIKA { get; set; }

        public double? LT_PANT_ADET { get; set; }

        public double? SON_AGIRLIK { get; set; }

        public double? KATLAMA_SAYISI { get; set; }

        public double? KIVRILMA_SAYISI { get; set; }

        public int? TEXTBOX_PARAMETER_ID { get; set; }

        public double? BASKI_KUVVETI { get; set; }

        public double? BASKI_SURESI { get; set; }

        public double? TOPLAM_SURE { get; set; }

        public double? SIKMA_SURE { get; set; }

        public double? NEM { get; set; }

        public double? SU { get; set; }

        public double? GUC { get; set; }

        public double? AGIRLIK { get; set; }

        public double? AKIS_HIZI { get; set; }

        public double? NBP { get; set; }

        public double? SIFIR_TAS_MIKTARI { get; set; }

        public double? ESKI_TAS_MIKTARI { get; set; }

        public string BOS_DONDURME { get; set; }

        public string ADET { get; set; }
    }
}