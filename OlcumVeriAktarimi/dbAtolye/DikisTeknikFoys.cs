namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DikisTeknikFoys
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string OrderNumarasi { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Tarih { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RevizyonTarih { get; set; }

        public string RevizyonNo { get; set; }

        public string Musteri { get; set; }

        public string ModelAd { get; set; }

        public string Kumas { get; set; }

        public string Yikama { get; set; }

        public string Modelist { get; set; }

        public string UstIpRengi { get; set; }

        public string AltIpRengi { get; set; }

        public string OverlokIpRengi { get; set; }

        public string OncepAstarIpRengi { get; set; }

        public string ArkaCepTakimAltIpRengi { get; set; }

        public string CepIcDikisIpRengi { get; set; }

        public string PacaIcZincirDikisIpRengi { get; set; }

        public string KemerIcZincirDIR { get; set; }

        public string PonterizIR { get; set; }

        public string ButunGozdeIIR { get; set; }

        public string YikamaTalimati { get; set; }

        public string UlkeEtiketi { get; set; }

        public string BedenliBoyEtiketi { get; set; }

        public string FirmaEtiketi { get; set; }

        public string FitEtiket { get; set; }

        public string KagitJakron { get; set; }

        public string Nakis { get; set; }

        public string IpTelaCalismasi { get; set; }

        public string LikraliTelaCalismasi { get; set; }

        public string OnemliUyariKritik { get; set; }

        public string AdimAyari { get; set; }

        public string OnCalismasi { get; set; }

        public string ArkaCalismasi { get; set; }

        public string BirlestirmeCalismasi { get; set; }

        public string BitimCalismasi { get; set; }

        public string ModelistKritik { get; set; }

        public string FermuarAcikPatleteDikimIR { get; set; }

        public string YanBoyOverlokDikisIR { get; set; }

        public string KemerGozdeilikIR { get; set; }

        public string PacaDisveIcDikisIR { get; set; }

        public string IcBoyCimaAltZincirIR { get; set; }

        public string YanBoyDortIplikOverlokDIR { get; set; }

        public string YikamaSonrasiYamaveZikzakDIR { get; set; }

        public string KemerOnIcDuzilikIR { get; set; }

        public string YanAcmaOverlokIR { get; set; }

        public string YikamadaCitlayanDikislerinUstveAltIR { get; set; }

        public string OnAgBaglamaveArkaCepTakimAltIR { get; set; }

        public string PatletDonusOnAgBaglamaveArkaCepTakimAltIR { get; set; }

        public string icBoyBesIplikZincirIR { get; set; }

        public string FermuarlarinDikimIR { get; set; }

        public string OnOrtaAltveArkaCepTakimAltIR { get; set; }

        public string YanCatimOverlokIR { get; set; }

        public string EtekDisveIcDikisIR { get; set; }

        public string AcikKapaliPatletYanAcmaOverlokIR { get; set; }

        public string OncepAstarRecmeDIR { get; set; }

        public string OncepAstarAltGazeDIR { get; set; }

        public string ilikliveKapaliPatletYanAcmaDarOverlokIR { get; set; }

        public string CeplerinIcDIR { get; set; }

        public string KopruCiftigneDIR { get; set; }

        public string icBoyCiftIgneAltZincirDIR { get; set; }

        public string BedenEtiketi { get; set; }

        public string YapistirmaEtiket { get; set; }

        public string Jakron { get; set; }

        public string MetalPlaka { get; set; }

        public string ilikliGenisLastik { get; set; }

        public string DikmeDugme { get; set; }

        public string KancaliDugme { get; set; }

        public string italianMadeFabric { get; set; }

        public string DantelPayetBoncuk { get; set; }

        public string italyanEtiket { get; set; }

        public string Dantel { get; set; }

        public string DantelUzeriBoncuk { get; set; }

        public string OnlySusEtiket { get; set; }

        public string BayrakEtiket { get; set; }

        public string SusEtiket { get; set; }

        public string DeriJakron { get; set; }

        public string YapistirmaFirmaEtiketi { get; set; }

        public string YapistirmaFirEtiket { get; set; }

        public string UyariEtiketi { get; set; }

        public string MadeinTurkeyEtiketi { get; set; }

        public string BedenliBaski { get; set; }

        public string Boncuk { get; set; }

        public string BaskiveNakis { get; set; }

        public string SusElDikis { get; set; }

        public string BezTelaCalismasi { get; set; }

        public string MansetveKemerIcZincirDIR { get; set; }

        public string BedenPulu { get; set; }

        public string KolCalismasi { get; set; }

        public string YakaCalismasi { get; set; }

        public string MansetveKemerCalismasi { get; set; }

        public string YanBoyAcmaZincirDIR { get; set; }

        public string yanBoyBesIplikZincirIR { get; set; }

        public string icBoyCimaAltZincirDIR { get; set; }

        public string Baski { get; set; }

        public string YoBaski { get; set; }

        public string YsBaski { get; set; }

        public string YoSilikonBaski { get; set; }

        public string YsSilikonBaski { get; set; }

        public string SeritTela { get; set; }

        public string PatletIcBiyeIR { get; set; }

        public string KemerIcBiyeIR { get; set; }

        public string icBiyeIR { get; set; }

        public string KopruZikzakIR { get; set; }

        public string EkRapor { get; set; }

        public string MarkaEtiketi { get; set; }

        public string YanAcmaSusZincirIR { get; set; }

        public string OnAgBaglamaAltIR { get; set; }

        public string ArkaCepAstarIR { get; set; }

        public string YanAcmaDortIplikIR { get; set; }

        public string ArkaCepAltParcaCatimOverlokZincirIR { get; set; }

        public string Etiket { get; set; }

        public string DokumaEtiket { get; set; }

        public string DokumaSusEtiket { get; set; }

        public string DokumaFitEtiketi { get; set; }

        public string PatletGozdeIlikIR { get; set; }

        public string ArkaCepAltSingerCatimIR { get; set; }

        public string ilikveKapalipatletOverlokIR { get; set; }

        public string SicakBaskiFitEtiketi { get; set; }

        public string MetalKistirmaRilet { get; set; }

        public string KotonKulupEtiket { get; set; }

        public string PremiumQualityOlitEtiket { get; set; }

        public string DokumaFirmaEtiket { get; set; }

        public string KulupMemberEtiket { get; set; }

        public string GenisLastik { get; set; }

        public string OnYanAcmaOverlokIR { get; set; }

        public string ArkaYanAcmaOverlokIR { get; set; }

        public string KanvasJakron { get; set; }

        public string PatletDonusAltIR { get; set; }

        public string PacaDisDIR { get; set; }

        public string PacaIcDIR { get; set; }

        public string GeciciBedenEtiketi { get; set; }

        public string PresEtiket { get; set; }

        public string PresFirmaEtiketi { get; set; }

        public string ilikliveKapaliPatletYanAcmaOverlokIR { get; set; }

        public string CakmakveArkaCepAgziGazeIR { get; set; }

        public int? KullaniciId { get; set; }

        public bool? Durum { get; set; }

        public string YikamaSonrasiYanDikisIR { get; set; }

        public string KemerTakimGizliDikisIR { get; set; }

        public int? RenkId { get; set; }

        public string KemerDuzIlikIR { get; set; }

        public string KemerLastikDuzIlikIR { get; set; }

        public string KemerKordonDuzIlikIR { get; set; }

        public string ArkaRobaIcIR { get; set; }

        public string ArkaAgIcBiyeIR { get; set; }

        public string KemerIcDIR { get; set; }

        public string CakmakCepAgziveArkaKopruZIR { get; set; }

        public string AcikveKapaliPatletOverlokIR { get; set; }

        public string isimEtiketi { get; set; }

        public string ContaCiftIgneAltZIR { get; set; }

        public string ArkaAgCiftIgneAltZIR { get; set; }

        public string ContaCiftDikisAltZIR { get; set; }

        public string ArkaAgCiftDikisAltZIR { get; set; }

        public string ContaCimaAltZIR { get; set; }

        public string ArkaAgCimaAltZIR { get; set; }

        public string ContaUcIgneAltZIR { get; set; }

        public string ArkaAgUcIgneAltZIR { get; set; }

        public string icBoyCiftIgneAltZIR { get; set; }

        public string YanBoyCiftIgneAltZIR { get; set; }

        public string icBoyCiftDikisAltZIR { get; set; }

        public string YanBoyCiftDikisAltZIR { get; set; }

        public string icBoyCimaAltZIR { get; set; }

        public string YanBoyUcIgneAltZIR { get; set; }

        public string icBoyUcIgneZIR { get; set; }

        public string YanBoyCimaAltIR { get; set; }

        public string CitCit { get; set; }

        public string YakaTakimIR { get; set; }

        public string MansetTakimIR { get; set; }

        public string KemerTakimIR { get; set; }

        public string Papagan { get; set; }

        public string PapaganSusZincir { get; set; }

        public string SusDeriEtiket { get; set; }

        public string ArkaOrtaCiftIgneAltZIR { get; set; }

        public string ArkaOrtaCimaAltZIR { get; set; }

        public string ArkaOrtaUcIgneAltZIR { get; set; }

        public string GeciciJAkBBYaziEtiket { get; set; }

        public string YanPacaBiyeIR { get; set; }

        public string Fitil { get; set; }

        public string ikiCmBalikSirtiEkstrafor { get; set; }

        public string HazirKemerTelasi { get; set; }

        public string SagOnCepAstarIR { get; set; }

        public string SolOnCepAstarIR { get; set; }

        public string OnContaCiftIgneAltIR { get; set; }

        public string OnContaCimaAltIR { get; set; }

        public string OnContaUcIgneAltIR { get; set; }

        public string AcikKapaliPatletDortIOIR { get; set; }

        public string ilikliKapaliPatletDortIOIR { get; set; }

        public string OnCepTakimAltIR { get; set; }

        public string Kordon { get; set; }

        public string YanYarimDAIR { get; set; }

        public string FirmaEtiketliBedenli { get; set; }

        public string GeciciJaponAkmazBedenEtiketi { get; set; }

        public string Agraf { get; set; }
    }
}
