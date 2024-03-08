namespace OlcumWeb.Recipe
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RECIPE")]
    public partial class RECIPE
    {
        public int ID { get; set; }

        public int? RECIPE_TYPE { get; set; }

        [StringLength(50)]
        public string RECIPE_TYPE_TEXT { get; set; }

        public int? DESIGNER { get; set; }

        public int? PIECE { get; set; }

        [StringLength(50)]
        public string CODE { get; set; }

        public int? CODE_SUFFIX { get; set; }

        [StringLength(50)]
        public string ORDER_NO { get; set; }

        [StringLength(500)]
        public string ORIGINAL_CRITICS { get; set; }

        [StringLength(500)]
        public string TRIAL_CRITICS { get; set; }

        [StringLength(100)]
        public string CUSTOMER { get; set; }

        [StringLength(50)]
        public string MATARIAL { get; set; }

        [StringLength(50)]
        public string MODEL_NAME { get; set; }

        public bool? STATUS { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public DateTime? LAST_UPDATED_DATE { get; set; }

        public DateTime? ORDER_INPUT { get; set; }

        public DateTime? ORDER_OUTPUT { get; set; }

        public DateTime? ORDER_DEADLINE { get; set; }

        [StringLength(6)]
        public string CODE_DATE { get; set; }

        [StringLength(4)]
        public string CODE_USER { get; set; }

        public int? URUN_SIRA_KODU { get; set; }

        [StringLength(50)]
        public string GUID { get; set; }

        public double? WEIGHT { get; set; }

        [StringLength(500)]
        public string MATERIAL_NAME { get; set; }

        public bool? IS_PLANNING_RECIPE { get; set; }

        public bool? ONAY_DURUMU { get; set; }

        [StringLength(255)]
        public string SERIAL_NO { get; set; }

        [StringLength(250)]
        public string WASHING_CODE { get; set; }

        [StringLength(50)]
        public string KAZAN_DURUMU { get; set; }

        [StringLength(50)]
        public string ORDER_NO_YEDEK { get; set; }

        public virtual RECIPE_TYPES RECIPE_TYPES { get; set; }
    }
}
