namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AtolyeContext : DbContext
    {
        public AtolyeContext()
            : base("name=AtolyeContext")
        {
        }

        public virtual DbSet<Atolyes> Atolyes { get; set; }
        public virtual DbSet<BaslangicKontrol2s> BaslangicKontrol2s { get; set; }
        public virtual DbSet<DikisTeknikFoys> DikisTeknikFoys { get; set; }
        public virtual DbSet<Emails> Emails { get; set; }
        public virtual DbSet<FinalKontrol2s> FinalKontrol2s { get; set; }
        public virtual DbSet<HataAciklamaTamirs> HataAciklamaTamirs { get; set; }
        public virtual DbSet<Hatas> Hatas { get; set; }
        public virtual DbSet<InlineKontrol2s> InlineKontrol2s { get; set; }
        public virtual DbSet<KimyasalBirims> KimyasalBirims { get; set; }
        public virtual DbSet<KimyasalEkipLiders> KimyasalEkipLiders { get; set; }
        public virtual DbSet<KimyasalEkips> KimyasalEkips { get; set; }
        public virtual DbSet<Kontrols> Kontrols { get; set; }
        public virtual DbSet<MakinaciAds> MakinaciAds { get; set; }
        public virtual DbSet<Makinas> Makinas { get; set; }
        public virtual DbSet<Musteris> Musteris { get; set; }
        public virtual DbSet<OrderDikimChecks> OrderDikimChecks { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<PArrays> PArrays { get; set; }
        public virtual DbSet<PHatas> PHatas { get; set; }
        public virtual DbSet<RaporKontrols> RaporKontrols { get; set; }
        public virtual DbSet<RaporOkundus> RaporOkundus { get; set; }
        public virtual DbSet<Rapors> Rapors { get; set; }
        public virtual DbSet<Renks> Renks { get; set; }
        public virtual DbSet<Resims> Resims { get; set; }
        public virtual DbSet<Spreycis> Spreycis { get; set; }
        public virtual DbSet<TrendOperasyon> TrendOperasyon { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UtuPakets> UtuPakets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .Property(e => e.KullaniciName)
                .IsUnicode(false);
        }
    }
}
