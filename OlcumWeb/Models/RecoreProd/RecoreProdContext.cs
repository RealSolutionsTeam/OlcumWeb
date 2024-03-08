using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace OlcumWeb.Models.RecoreProd
{
    public partial class RecoreProdContext : DbContext
    {
        public RecoreProdContext()
            : base("name=RecoreProdContext")
        {
        }

        public virtual DbSet<BRC_Barcodes> BRC_Barcodes { get; set; }
        public virtual DbSet<RC_WorkOrders> RC_WorkOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
