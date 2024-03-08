using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace OlcumVeriAktarimi.dbOlcumTest
{
    public partial class OlcumContext : DbContext
    {
        public OlcumContext()
            : base("name=OlcumContext")
        {
        }

        public virtual DbSet<Bedenler> Bedenler { get; set; }
        public virtual DbSet<Boylar> Boylar { get; set; }
        public virtual DbSet<CiktiHtml> CiktiHtml { get; set; }
        public virtual DbSet<CogaltmaAra> CogaltmaAra { get; set; }
        public virtual DbSet<CogaltmaDetay> CogaltmaDetay { get; set; }
        public virtual DbSet<GiysiTurleri> GiysiTurleri { get; set; }
        public virtual DbSet<HazirTabloAra> HazirTabloAra { get; set; }
        public virtual DbSet<HazirTabloDetay> HazirTabloDetay { get; set; }
        public virtual DbSet<KazanAra> KazanAra { get; set; }
        public virtual DbSet<KazanDetay> KazanDetay { get; set; }
        public virtual DbSet<KazanHesaplama> KazanHesaplama { get; set; }
        public virtual DbSet<KesimFisiAra> KesimFisiAra { get; set; }
        public virtual DbSet<KesimFisiDetay> KesimFisiDetay { get; set; }
        public virtual DbSet<KumasKarakteri> KumasKarakteri { get; set; }
        public virtual DbSet<Modeller> Modeller { get; set; }
        public virtual DbSet<Musteriler> Musteriler { get; set; }
        public virtual DbSet<NumuneAra> NumuneAra { get; set; }
        public virtual DbSet<NumuneDetay> NumuneDetay { get; set; }
        public virtual DbSet<NumuneHazirTabloAra> NumuneHazirTabloAra { get; set; }
        public virtual DbSet<NumuneHazirTabloDetay> NumuneHazirTabloDetay { get; set; }
        public virtual DbSet<OlcuNoktalari> OlcuNoktalari { get; set; }
        public virtual DbSet<OlcuTabloAra> OlcuTabloAra { get; set; }
        public virtual DbSet<OlcuTabloDetay> OlcuTabloDetay { get; set; }
        public virtual DbSet<OlcuTabloHesaplama> OlcuTabloHesaplama { get; set; }
        public virtual DbSet<OlcuTurleri> OlcuTurleri { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderBaglanti> OrderBaglanti { get; set; }
        public virtual DbSet<PersonelTablo> PersonelTablo { get; set; }
        public virtual DbSet<RaporAnaCekme> RaporAnaCekme { get; set; }
        public virtual DbSet<RaporAra> RaporAra { get; set; }
        public virtual DbSet<RaporBagliOrder> RaporBagliOrder { get; set; }
        public virtual DbSet<RaporDetay> RaporDetay { get; set; }
        public virtual DbSet<RaporHesaplama> RaporHesaplama { get; set; }
        public virtual DbSet<RaporlamaOlcuNoktalari> RaporlamaOlcuNoktalari { get; set; }
        public virtual DbSet<RaporOrtalama> RaporOrtalama { get; set; }
        public virtual DbSet<Roller> Roller { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bedenler>()
                .HasMany(e => e.CogaltmaAra)
                .WithRequired(e => e.Bedenler)
                .HasForeignKey(e => e.bedenID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bedenler>()
                .HasMany(e => e.HazirTabloAra)
                .WithRequired(e => e.Bedenler)
                .HasForeignKey(e => e.bedenID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bedenler>()
                .HasMany(e => e.KazanAra)
                .WithRequired(e => e.Bedenler)
                .HasForeignKey(e => e.bedenID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bedenler>()
                .HasMany(e => e.KesimFisiAra)
                .WithOptional(e => e.Bedenler)
                .HasForeignKey(e => e.BedenId);

            modelBuilder.Entity<Bedenler>()
                .HasMany(e => e.NumuneAra)
                .WithRequired(e => e.Bedenler)
                .HasForeignKey(e => e.bedenID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bedenler>()
                .HasMany(e => e.OlcuTabloAra)
                .WithRequired(e => e.Bedenler)
                .HasForeignKey(e => e.bedenID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Boylar>()
                .HasMany(e => e.KesimFisiAra)
                .WithOptional(e => e.Boylar)
                .HasForeignKey(e => e.BoyId);

            modelBuilder.Entity<CogaltmaDetay>()
                .HasMany(e => e.CogaltmaAra)
                .WithRequired(e => e.CogaltmaDetay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GiysiTurleri>()
                .HasMany(e => e.CogaltmaDetay)
                .WithRequired(e => e.GiysiTurleri)
                .HasForeignKey(e => e.giysiTuruID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GiysiTurleri>()
                .HasMany(e => e.KazanDetay)
                .WithOptional(e => e.GiysiTurleri)
                .HasForeignKey(e => e.giysiTuruID);

            modelBuilder.Entity<GiysiTurleri>()
                .HasMany(e => e.KesimFisiDetay)
                .WithRequired(e => e.GiysiTurleri)
                .HasForeignKey(e => e.GiysiTuruId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GiysiTurleri>()
                .HasMany(e => e.NumuneDetay)
                .WithRequired(e => e.GiysiTurleri)
                .HasForeignKey(e => e.giysiTuruID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GiysiTurleri>()
                .HasMany(e => e.NumuneDetay1)
                .WithRequired(e => e.GiysiTurleri1)
                .HasForeignKey(e => e.giysiTuruID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GiysiTurleri>()
                .HasMany(e => e.NumuneHazirTabloDetay)
                .WithRequired(e => e.GiysiTurleri)
                .HasForeignKey(e => e.GiysiTuruId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GiysiTurleri>()
                .HasMany(e => e.OlcuTabloDetay)
                .WithRequired(e => e.GiysiTurleri)
                .HasForeignKey(e => e.giysiTuruID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HazirTabloDetay>()
                .HasMany(e => e.HazirTabloAra)
                .WithRequired(e => e.HazirTabloDetay)
                .HasForeignKey(e => e.hazirtabloID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KazanDetay>()
                .HasOptional(e => e.CiktiHtml)
                .WithRequired(e => e.KazanDetay);

            modelBuilder.Entity<KazanDetay>()
                .HasMany(e => e.KazanAra)
                .WithRequired(e => e.KazanDetay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KazanDetay>()
                .HasMany(e => e.KazanHesaplama)
                .WithRequired(e => e.KazanDetay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KesimFisiDetay>()
                .HasMany(e => e.KesimFisiAra)
                .WithRequired(e => e.KesimFisiDetay)
                .HasForeignKey(e => e.KesimFisiId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Modeller>()
                .HasMany(e => e.HazirTabloDetay)
                .WithRequired(e => e.Modeller)
                .HasForeignKey(e => e.modelID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Musteriler>()
                .HasMany(e => e.Modeller)
                .WithRequired(e => e.Musteriler)
                .HasForeignKey(e => e.musteriID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Musteriler>()
                .HasMany(e => e.NumuneHazirTabloDetay)
                .WithRequired(e => e.Musteriler)
                .HasForeignKey(e => e.MusteriId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NumuneDetay>()
                .HasMany(e => e.NumuneAra)
                .WithRequired(e => e.NumuneDetay)
                .HasForeignKey(e => e.numuneDetayID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NumuneDetay>()
                .HasMany(e => e.NumuneAra1)
                .WithRequired(e => e.NumuneDetay1)
                .HasForeignKey(e => e.numuneDetayID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NumuneHazirTabloDetay>()
                .HasMany(e => e.NumuneHazirTabloAra)
                .WithRequired(e => e.NumuneHazirTabloDetay)
                .HasForeignKey(e => e.HazirTabloId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuNoktalari>()
                .HasMany(e => e.CogaltmaAra)
                .WithRequired(e => e.OlcuNoktalari)
                .HasForeignKey(e => e.olcuNoktaID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuNoktalari>()
                .HasMany(e => e.HazirTabloAra)
                .WithRequired(e => e.OlcuNoktalari)
                .HasForeignKey(e => e.olcuNoktaID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuNoktalari>()
                .HasMany(e => e.KazanAra)
                .WithRequired(e => e.OlcuNoktalari)
                .HasForeignKey(e => e.olcuNoktaID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuNoktalari>()
                .HasMany(e => e.KesimFisiAra)
                .WithOptional(e => e.OlcuNoktalari)
                .HasForeignKey(e => e.OlcuNoktaId);

            modelBuilder.Entity<OlcuNoktalari>()
                .HasMany(e => e.NumuneAra)
                .WithRequired(e => e.OlcuNoktalari)
                .HasForeignKey(e => e.olcuNoktaID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuNoktalari>()
                .HasMany(e => e.NumuneHazirTabloAra)
                .WithRequired(e => e.OlcuNoktalari)
                .HasForeignKey(e => e.olcuNoktaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuNoktalari>()
                .HasMany(e => e.OlcuTabloAra)
                .WithRequired(e => e.OlcuNoktalari)
                .HasForeignKey(e => e.olcuNoktaID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuNoktalari>()
                .HasMany(e => e.OlcuTabloHesaplama)
                .WithRequired(e => e.OlcuNoktalari)
                .HasForeignKey(e => e.olcuNoktaID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuNoktalari>()
                .HasMany(e => e.RaporAnaCekme)
                .WithRequired(e => e.OlcuNoktalari)
                .HasForeignKey(e => e.OlcuNoktaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuNoktalari>()
                .HasMany(e => e.RaporHesaplama)
                .WithRequired(e => e.OlcuNoktalari)
                .HasForeignKey(e => e.olcuNoktaID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuNoktalari>()
                .HasMany(e => e.RaporOrtalama)
                .WithRequired(e => e.OlcuNoktalari)
                .HasForeignKey(e => e.olcuNoktaID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuTurleri>()
                .HasMany(e => e.CogaltmaDetay)
                .WithRequired(e => e.OlcuTurleri)
                .HasForeignKey(e => e.olcuTuruID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuTurleri>()
                .HasMany(e => e.KazanDetay)
                .WithRequired(e => e.OlcuTurleri)
                .HasForeignKey(e => e.olcuTuruID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuTurleri>()
                .HasMany(e => e.KesimFisiDetay)
                .WithRequired(e => e.OlcuTurleri)
                .HasForeignKey(e => e.OlcuTuruId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuTurleri>()
                .HasMany(e => e.NumuneDetay)
                .WithRequired(e => e.OlcuTurleri)
                .HasForeignKey(e => e.olcuTuruID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuTurleri>()
                .HasMany(e => e.OlcuTabloDetay)
                .WithRequired(e => e.OlcuTurleri)
                .HasForeignKey(e => e.olcuTuruID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OlcuTurleri>()
                .HasMany(e => e.RaporAra)
                .WithRequired(e => e.OlcuTurleri)
                .HasForeignKey(e => e.olcuTuruID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.CogaltmaDetay)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.KazanDetay)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.KesimFisiDetay)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.NumuneDetay)
                .WithRequired(e => e.Order)
                .HasForeignKey(e => e.orderID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.NumuneDetay1)
                .WithRequired(e => e.Order1)
                .HasForeignKey(e => e.orderID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OlcuTabloDetay)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.RaporAra)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.RaporBagliOrder)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonelTablo>()
                .HasMany(e => e.CogaltmaDetay)
                .WithRequired(e => e.PersonelTablo)
                .HasForeignKey(e => e.kullaniciID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonelTablo>()
                .HasMany(e => e.HazirTabloDetay)
                .WithRequired(e => e.PersonelTablo)
                .HasForeignKey(e => e.kullanıcıID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonelTablo>()
                .HasMany(e => e.KazanDetay)
                .WithRequired(e => e.PersonelTablo)
                .HasForeignKey(e => e.kullaniciID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonelTablo>()
                .HasMany(e => e.KesimFisiDetay)
                .WithRequired(e => e.PersonelTablo)
                .HasForeignKey(e => e.KullaniciId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonelTablo>()
                .HasMany(e => e.NumuneDetay)
                .WithRequired(e => e.PersonelTablo)
                .HasForeignKey(e => e.kullaniciID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonelTablo>()
                .HasMany(e => e.OlcuTabloDetay)
                .WithRequired(e => e.PersonelTablo)
                .HasForeignKey(e => e.kullaniciID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonelTablo>()
                .HasMany(e => e.RaporDetay)
                .WithOptional(e => e.PersonelTablo)
                .HasForeignKey(e => e.kullaniciID);

            modelBuilder.Entity<RaporAra>()
                .HasMany(e => e.RaporHesaplama)
                .WithRequired(e => e.RaporAra)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RaporDetay>()
                .HasMany(e => e.RaporAnaCekme)
                .WithRequired(e => e.RaporDetay)
                .HasForeignKey(e => e.RaporId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RaporDetay>()
                .HasMany(e => e.RaporAra)
                .WithRequired(e => e.RaporDetay)
                .HasForeignKey(e => e.raporID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RaporDetay>()
                .HasMany(e => e.RaporBagliOrder)
                .WithRequired(e => e.RaporDetay)
                .HasForeignKey(e => e.raporId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RaporDetay>()
                .HasMany(e => e.RaporHesaplama)
                .WithRequired(e => e.RaporDetay)
                .HasForeignKey(e => e.raporID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RaporDetay>()
                .HasMany(e => e.RaporOrtalama)
                .WithRequired(e => e.RaporDetay)
                .HasForeignKey(e => e.raporID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roller>()
                .HasMany(e => e.PersonelTablo)
                .WithRequired(e => e.Roller)
                .HasForeignKey(e => e.rolId)
                .WillCascadeOnDelete(false);
        }
    }
}
