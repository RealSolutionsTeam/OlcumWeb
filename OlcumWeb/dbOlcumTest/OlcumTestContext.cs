namespace OlcumWeb.dbOlcumTest
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OlcumTestContext : DbContext
    {
        public OlcumTestContext()
            : base("name=OlcumTestContext")
        {
        }

        public virtual DbSet<Bedenler> Bedenlers { get; set; }
        public virtual DbSet<CiktiHtml> CiktiHtmls { get; set; }
        public virtual DbSet<CogaltmaAra> CogaltmaAras { get; set; }
        public virtual DbSet<CogaltmaDetay> CogaltmaDetays { get; set; }
        public virtual DbSet<GiysiTurleri> GiysiTurleris { get; set; }
        public virtual DbSet<HazirTabloAra> HazirTabloAras { get; set; }
        public virtual DbSet<HazirTabloDetay> HazirTabloDetays { get; set; }
        public virtual DbSet<KazanAra> KazanAras { get; set; }
        public virtual DbSet<KazanDetay> KazanDetays { get; set; }
        public virtual DbSet<KazanHesaplama> KazanHesaplamas { get; set; }
        public virtual DbSet<KumasDetay> KumasDetays { get; set; }
        public virtual DbSet<Modeller> Modellers { get; set; }
        public virtual DbSet<Musteriler> Musterilers { get; set; }
        public virtual DbSet<NumuneAra> NumuneAras { get; set; }
        public virtual DbSet<NumuneDetay> NumuneDetays { get; set; }
        public virtual DbSet<NumuneKesimDetay> NumuneKesimDetays { get; set; }
        public virtual DbSet<OlcuNoktalari> OlcuNoktalaris { get; set; }
        public virtual DbSet<OlcuTabloAra> OlcuTabloAras { get; set; }
        public virtual DbSet<OlcuTabloDetay> OlcuTabloDetays { get; set; }
        public virtual DbSet<OlcuTabloHesaplama> OlcuTabloHesaplamas { get; set; }
        public virtual DbSet<OlcuTurleri> OlcuTurleris { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderBaglanti> OrderBaglantis { get; set; }
        public virtual DbSet<PersonelTablo> PersonelTabloes { get; set; }
        public virtual DbSet<Roller> Rollers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OlcuTurleri>()
                .HasMany(e => e.KazanDetays)
                .WithRequired(e => e.OlcuTurleri)
                .HasForeignKey(e => e.olcuTabloOlcuTurId)
                .WillCascadeOnDelete(false);
        }
    }
}
