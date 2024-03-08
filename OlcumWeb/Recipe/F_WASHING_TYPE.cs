namespace OlcumWeb.Recipe
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class F_WASHING_TYPE
    {
        public int ID { get; set; }

        public int? UNIT_ID { get; set; }

        [StringLength(50)]
        public string NAME { get; set; }

        public bool? IS_WET { get; set; }

        public int? STATUS { get; set; }

        public bool? IS_TIME { get; set; }

        public bool? IS_TEMPRATURE { get; set; }

        public bool? IS_LR { get; set; }

        public bool? IS_SIKMA_SANIYESI { get; set; }

        public bool? IS_KURUTMA_SICAKLIK { get; set; }

        public bool? IS_KURUTMA_DAKIKA { get; set; }

        public bool? IS_BOLGE { get; set; }

        public bool? IS_TUR_SAYISI { get; set; }

        public bool? IS_FIRIN_1_DERECE { get; set; }

        public bool? IS_FIRIN_2_DERECE { get; set; }

        public bool? IS_FIRIN_1_DAKIKA { get; set; }

        public bool? IS_FIRIN_2_DAKIKA { get; set; }

        public bool? IS_PROJE { get; set; }

        public bool? IS_YAKMA_DERECESI { get; set; }

        public bool? IS_YAKMA_SURESI { get; set; }

        public bool? IS_DPI { get; set; }

        public bool? IS_ZIMPARA_DERECESI { get; set; }

        public bool? IS_MACHINE { get; set; }

        public bool? IS_SUBMACHINE { get; set; }

        public bool? IS_SARJ_ADEDI { get; set; }

        public bool? IS_PH { get; set; }

        public bool? IS_HAVLU_MIKTARI { get; set; }

        public bool? IS_HAVLU_BOYUT { get; set; }

        public bool? IS_HAVLU_DONME_DK { get; set; }

        public bool? IS_HAVLU_SIKMA_DK { get; set; }

        public bool? IS_RANDOM_DK { get; set; }

        public bool? SIKMA_DEVRI { get; set; }

        public bool? KOVA_TAS_MIKTARI { get; set; }

        public bool? FILE_TIPI { get; set; }

        public bool? IP_TIPI { get; set; }

        public bool? CUVAL_TAS_MIKTARI { get; set; }

        public bool? PANTOLON_SIKMA_DK { get; set; }

        public bool? IS_ACIKLAMA { get; set; }

        public bool? IS_FIRIN_3_DAKIKA { get; set; }

        public bool? IS_FIRIN_3_DERECE { get; set; }

        public bool? IS_1_LT_PANT_ADET { get; set; }

        public bool? IS_SON_AGIRLIK { get; set; }

        public bool? IS_HAM_AGIRLIK { get; set; }

        public bool? IS_BASKI_KUVVETI { get; set; }

        public bool? IS_BASKI_SURESI { get; set; }

        public bool? IS_TOPLAM_SURE { get; set; }

        public bool? IS_SIKMA_SURE { get; set; }

        public bool? IS_NEM { get; set; }

        public bool? IS_SU { get; set; }

        public bool? IS_GUC { get; set; }

        public bool? IS_AGIRLIK { get; set; }

        public bool? IS_AKIS_HIZI { get; set; }

        public bool? IS_NBP { get; set; }

        public bool? IS_SIFIR_TAS_MIKTARI { get; set; }

        public bool? IS_ESKI_TAS_MIKTARI { get; set; }

        public int? ROW_ORDER_NO { get; set; }

        public bool? IS_ADET { get; set; }

        public bool? IS_BOS_DONDURME { get; set; }
    }
}
