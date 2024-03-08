namespace OlcumWeb.Models.RecoreProd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BRC_Barcodes
    {
        public int Id { get; set; }

        public int? DepartmentId { get; set; }

        [Required]
        public string Barcode { get; set; }

        public string Lot { get; set; }

        public string Shrink { get; set; }

        public string MarkerFoldRange { get; set; }

        public string Height { get; set; }

        public string MarkerNumber { get; set; }

        public string Size { get; set; }

        public int CreatedUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        public int LastUpdatedUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LastUpdatedDate { get; set; }

        public bool Status { get; set; }

        public bool isDeleted { get; set; }

        public string CuttingNumber { get; set; }

        public string DocNumber { get; set; }

        public string DrawingNumber { get; set; }

        public string FabricRollLineNumber { get; set; }

        public string FabricRollNumber { get; set; }

        public int WorkOrderId { get; set; }

        public bool? IsPrinted { get; set; }
    }
}
