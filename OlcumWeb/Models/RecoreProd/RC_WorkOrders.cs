namespace OlcumWeb.Models.RecoreProd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RC_WorkOrders
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int WorkOrderTypeId { get; set; }

        public int CustomerMarkId { get; set; }

        public int ClothTypeId { get; set; }

        public string ClothTypeName { get; set; }

        public int? CountryId { get; set; }

        public int RecipeId { get; set; }

        public int? ReferenceWorkOrderId { get; set; }

        public int InventoryId { get; set; }

        [Required]
        [StringLength(250)]
        public string WorkOrderNo { get; set; }

        [Required]
        [StringLength(250)]
        public string WorkOrderModelName { get; set; }

        [Required]
        [StringLength(50)]
        public string WorkOrderModelShortName { get; set; }

        public int Quantity { get; set; }

        public int? PackageQuantity { get; set; }

        public int CuttingQuantity { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime WorkOrderDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DeliveryDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PackingDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ShipmentDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CuttingApproveDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? SewingFinishDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? WashingApproveDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? WashingFinishDate { get; set; }

        public bool? IsApproved { get; set; }

        public int? ApprovedBy { get; set; }

        public bool? IsLocked { get; set; }

        public int? LockedBy { get; set; }

        public string LockedExplanation { get; set; }

        public bool? IsClosed { get; set; }

        public string ClosedExplanation { get; set; }

        public bool? IsCancelled { get; set; }

        public string CancelledExplanation { get; set; }

        public string RootId { get; set; }

        public int? RC_MarkId { get; set; }

        public int CreatedUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        public int LastUpdatedUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LastUpdatedDate { get; set; }

        public bool Status { get; set; }

        public bool isDeleted { get; set; }

        public int? ErpRecId { get; set; }

        public bool? IsTransferred { get; set; }

        public int? PlannedCuttingQuantity { get; set; }

        public string WorkOrderModelCode { get; set; }

        public string CategoryTypeName { get; set; }
    }
}
