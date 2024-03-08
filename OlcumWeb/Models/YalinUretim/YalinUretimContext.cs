using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace OlcumWeb.Models.YalinUretim
{
    public partial class YalinUretimContext : DbContext
    {
        public YalinUretimContext()
            : base("name=YalinUretimContext")
        {
        }

        public virtual DbSet<IsEmriAlt> IsEmriAlt { get; set; }
        public virtual DbSet<OrderTakipKimlikBilgiler> OrderTakipKimlikBilgiler { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
