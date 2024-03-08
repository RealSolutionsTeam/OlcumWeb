namespace OlcumVeriAktarimi.dbOlcumRaporlama
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RaporlamaContext : DbContext
    {
        public RaporlamaContext()
            : base("name=RaporlamaContext")
        {
        }

        public virtual DbSet<OlcuFormu> OlcuFormu { get; set; }
        public virtual DbSet<OlcuFormuBeden> OlcuFormuBeden { get; set; }
        public virtual DbSet<OlcuTablosu> OlcuTablosu { get; set; }
        public virtual DbSet<OlcuTablosuBeden> OlcuTablosuBeden { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OlcuTablosu>()
                .Property(e => e.olcuTuru)
                .IsUnicode(false);
        }
    }
}
