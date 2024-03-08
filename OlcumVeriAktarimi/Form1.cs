using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OlcumVeriAktarimi.dbAtolye;
//using OlcumVeriAktarimi.dbOlcum;
using OlcumVeriAktarimi.dbOlcumRaporlama;
using OlcumVeriAktarimi.dbOlcumTest;
using System.Configuration;

//52783la başladık en son - 54599 son kaydedilen

namespace OlcumVeriAktarimi
{
    public partial class Form1 : Form
    {
        AtolyeContext dbAtolye = new AtolyeContext();
        OlcumContext dbOlcum = new OlcumContext();
        RaporlamaContext dbOlcumRaporlama = new RaporlamaContext();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //List<Users> listUser = dbAtolye.Users.Where(x => (x.Rol == 2 || x.Rol == 1 || x.Rol == 3) && x.Sifre != "").OrderBy(y => y.Rol).ToList();

            //foreach (var item in listUser)
            //{
            //    string ad = item.KullaniciAd.Split('.')[0], soyad = item.KullaniciAd.Split('.')[1];

            //    if (!dbOlcum.PersonelTablo.Any(x => x.personelAd == ad && x.personelSoyad == soyad))
            //    {
            //        dbOlcumTest.PersonelTablo addobj = new dbOlcumTest.PersonelTablo();
            //        addobj.personelAd = ad;
            //        addobj.personelSoyad = soyad;

            //        if (item.Rol == 1)//fason Takipcisi
            //        {
            //            addobj.personelDepartman = "Fason";
            //            addobj.personelGorev = "Fason Takipçisi";
            //            addobj.personelAdminmi = false;
            //            addobj.rolId = dbOlcum.Roller.Where(x => x.RolAd == "Fason Takipçisi").FirstOrDefault().Id;
            //        }
            //        else if (item.Rol == 2) //yönetici
            //        {
            //            addobj.personelDepartman = "Düzce";
            //            addobj.personelGorev = "";
            //            addobj.personelAdminmi = true;
            //            addobj.rolId = dbOlcum.Roller.Where(x => x.RolAd == "Yönetici").FirstOrDefault().Id;
            //        }
            //        else//3 : raporKontrolcü
            //        {
            //            addobj.personelDepartman = "Düzce";
            //            addobj.personelGorev = "";
            //            addobj.personelAdminmi = false;
            //            addobj.rolId = dbOlcum.Roller.Where(x => x.RolAd == "Modelist").FirstOrDefault().Id;
            //        }

            //        addobj.personelkAdi = item.KullaniciAd;
            //        addobj.personelSifre = item.Sifre;

            //        dbOlcum.PersonelTablo.Add(addobj);
            //    }

            //}

            //dbOlcum.SaveChanges();

            //MessageBox.Show("Kayıt Başarılı");
        }

        private void button2_Click(object sender, EventArgs e)
        {

            List<OlcuTablosu> listOlcuTablosu = dbOlcumRaporlama.OlcuTablosu.Where(x => ( x.ottabloID == 53918 || x.ottabloID == 53919 || x.ottabloID == 53921) && x.ottabloTuru != 2).ToList();
            //6628 > 6627

            List<dbOlcumTest.Order> NewOrderList = dbOlcum.Order.ToList();
            List<GiysiTurleri> newGiysiTurleri = dbOlcum.GiysiTurleri.ToList();
            List<PersonelTablo> newPersonelTablo = dbOlcum.PersonelTablo.ToList(); 
            List<Bedenler> newBedenler = dbOlcum.Bedenler.ToList();

            List<OlcuTurleri> newOlcuTurleri = dbOlcum.OlcuTurleri.ToList();
            List<OlcuNoktalari> newOlcuNoktalari = dbOlcum.OlcuNoktalari.ToList();

            List<OlcuTablosuBeden> olcuTablosuBeden = dbOlcumRaporlama.OlcuTablosuBeden.Where(x => (x.ottabloID == 53918 || x.ottabloID == 53919 || x.ottabloID == 53921)).ToList();

            foreach (var item in listOlcuTablosu)
            {
                OlcuTabloDetay addObj = new OlcuTabloDetay();
                addObj.oldOlcuTabloId = item.ottabloID;

                //Order
                if (NewOrderList.Any(x => x.orderNo == item.orderNo.ToUpper()))
                {
                    addObj.orderID = NewOrderList.Where(x => x.orderNo == item.orderNo.ToUpper()).FirstOrDefault().ID;
                }
                else
                {
                    //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "    Order=" + item.orderNo + " bulunamadı");
                    addObj.orderID = 1;
                    addObj.hataOldu = true;
                }


                //Giysi Türleri
                if (newGiysiTurleri.Any(x => x.giysiTuruAd == item.giysiTuru))
                {
                    addObj.giysiTuruID = newGiysiTurleri.Where(x => x.giysiTuruAd == item.giysiTuru).FirstOrDefault().id;
                }
                else
                {
                    //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "     Giysi Turu=" + item.giysiTuru + " bulunamadı");
                    addObj.giysiTuruID = 1;
                    addObj.hataOldu = true;
                }


                //KULLANICI ID
                if (item.modelistAd.IndexOf("/") != -1)
                {
                    var ad = item.modelistAd.Trim();

                    if (newPersonelTablo.Any(x => x.personelAd.Contains(ad)))
                    {
                        if (ad == "elif")
                        {
                            addObj.kullaniciID = newPersonelTablo.Where(x => x.personelAd.Contains(ad) && x.personelSoyad == "Şahin").FirstOrDefault().id;
                        }
                        else
                        {
                            addObj.kullaniciID = newPersonelTablo.Where(x => x.personelAd.Contains(ad)).FirstOrDefault().id;
                        }
                    }
                    else
                    {

                        //using (UserDialog form2 = new UserDialog(item.modelistAd, item.ottabloID, 1))
                        //{
                        //    if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        //    {
                        //        addObj.kullaniciID = form2.seilenUserId;
                        //    }
                        //}

                        addObj.hataOldu = true;
                        addObj.kullaniciID = 1;
                    }
                }
                else
                {
                    var AdArray = item.modelistAd.Split('/');
                    var ModelisAd = AdArray[AdArray.Length - 1].Trim();

                    if (newPersonelTablo.Any(x => x.personelAd.Contains(ModelisAd)))
                    {
                        if (ModelisAd == "elif")
                        {
                            addObj.kullaniciID = newPersonelTablo.Where(x => x.personelAd.Contains(ModelisAd) && x.personelSoyad == "Şahin").FirstOrDefault().id;
                        }
                        else
                        {
                            addObj.kullaniciID = newPersonelTablo.Where(x => x.personelAd.Contains(ModelisAd)).FirstOrDefault().id;
                        }
                    }
                    else
                    {
                        //using (UserDialog form2 = new UserDialog(item.modelistAd, item.ottabloID, 1))
                        //{
                        //    if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        //    {
                        //        addObj.kullaniciID = form2.seilenUserId;
                        //    }
                        //}
                        addObj.hataOldu = true;
                        addObj.kullaniciID = 1;
                    }
                }



                //EnBoy Çekme
                addObj.enBoyCekme = item.enBoyCekme;


                //Tarih
                addObj.tarih = item.tarih;


                //AnaBeden
                OlcuTablosuBeden bedenList = olcuTablosuBeden.Where(x => x.ottabloID == item.ottabloID).OrderBy(y => y.satirIndexi).FirstOrDefault();
                List<string> bedenlerList = new List<string>();

                if (bedenList.olcuBedeni != null) { bedenlerList.Add(bedenList.olcuBedeni); }
                if (bedenList.olcuBedeni2 != null) { bedenlerList.Add(bedenList.olcuBedeni2); }
                if (bedenList.olcuBedeni3 != null) { bedenlerList.Add(bedenList.olcuBedeni3); }
                if (bedenList.olcuBedeni4 != null) { bedenlerList.Add(bedenList.olcuBedeni4); }
                if (bedenList.olcuBedeni5 != null) { bedenlerList.Add(bedenList.olcuBedeni5); }
                if (bedenList.olcuBedeni6 != null) { bedenlerList.Add(bedenList.olcuBedeni6); }
                if (bedenList.olcuBedeni7 != null) { bedenlerList.Add(bedenList.olcuBedeni7); }
                if (bedenList.olcuBedeni8 != null) { bedenlerList.Add(bedenList.olcuBedeni8); }
                if (bedenList.olcuBedeni9 != null) { bedenlerList.Add(bedenList.olcuBedeni9); }
                if (bedenList.olcuBedeni10 != null) { bedenlerList.Add(bedenList.olcuBedeni10); }
                if (bedenList.olcuBedeni11 != null) { bedenlerList.Add(bedenList.olcuBedeni11); }
                if (bedenList.olcuBedeni12 != null) { bedenlerList.Add(bedenList.olcuBedeni12); }
                if (bedenList.olcuBedeni13 != null) { bedenlerList.Add(bedenList.olcuBedeni13); }
                if (bedenList.olcuBedeni14 != null) { bedenlerList.Add(bedenList.olcuBedeni14); }
                if (bedenList.olcuBedeni15 != null) { bedenlerList.Add(bedenList.olcuBedeni15); }

                int anaBedenIndex = Convert.ToInt32(item.anaBeden) - 2;

                if (bedenlerList.Count() > anaBedenIndex)
                {
                    string anaBeden = bedenlerList[anaBedenIndex].Split('/')[0];

                    if (newBedenler.Any(x => x.beden == anaBeden))
                    {
                        addObj.anaBedenID = newBedenler.Where(x => x.beden == anaBeden).FirstOrDefault().ID;
                    }
                    else
                    {
                        //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "    Ana Beden=" + anaBeden + " bulunamadı");
                        addObj.anaBedenID = 1;
                        addObj.hataOldu = true;
                    }
                }
                else
                {
                    if (bedenlerList.Count() == 1)
                    {
                        string anaBeden = bedenlerList[0];

                        if (newBedenler.Any(x => x.beden == anaBeden))
                        {
                            addObj.anaBedenID = newBedenler.Where(x => x.beden == anaBeden).FirstOrDefault().ID;
                        }
                        else
                        {
                            //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "     Aranan Ana Beden=" + anaBeden + " bulunamadı");
                            addObj.anaBedenID = 1;
                            addObj.hataOldu = true;
                        }
                    }
                    else
                    {
                        //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "    Ana Beden=" + item.anaBeden + "     birden fazla beden var fakatanabedenIndex liste dışı geldi");
                        addObj.anaBedenID = 1;
                        addObj.hataOldu = true;
                    }

                }


                //TabloTur
                addObj.tabloTuru = item.ottabloTuru;


                //OlcuTuru
                if (newOlcuTurleri.Any(x => x.olcuTuruAd == item.olcuTuru))
                {
                    if (item.olcuTuru == "İmalat")
                    {
                        addObj.olcuTuruID = newOlcuTurleri.Where(x => x.olcuTuruAd == item.olcuTuru && x.tabloTuru == "Ölçü Tablosu").FirstOrDefault().id;
                    }
                    else
                    {
                        addObj.olcuTuruID = newOlcuTurleri.Where(x => x.olcuTuruAd == item.olcuTuru).FirstOrDefault().id;
                    }
                }
                else
                {
                    //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "    Ölçü Tablo Türü=" + item.olcuTuru + "   bulunamadı");
                    addObj.olcuTuruID = 1;
                    addObj.hataOldu = true;
                }


                //Aciklama
                addObj.aciklama = item.aciklama;
                addObj.aktarimMi = true;
                dbOlcum.OlcuTabloDetay.Add(addObj);

                dbOlcum.SaveChanges();

                List<OlcuTablosuBeden> degerlerList = olcuTablosuBeden.Where(x => x.ottabloID == item.ottabloID).OrderBy(x => x.satirIndexi).ToList();

                foreach (var itemDeger in degerlerList)
                {

                    if (itemDeger.olcuBedeni != null)
                    {

                        OlcuTabloAra addAra = new OlcuTabloAra();
                        addAra.olcuTabloID = addObj.id;


                        if (newOlcuNoktalari.Any(x => x.olcuNoktasi == itemDeger.olcumNoktasi.Trim().Replace("\0","")))
                        {
                            addAra.olcuNoktaID = newOlcuNoktalari.Where(x => x.olcuNoktasi == itemDeger.olcumNoktasi.Trim().Replace("\0", "")).FirstOrDefault().id;
                        }
                        else
                        {
                            //using (OlcuNokDialog form2 = new OlcuNokDialog(itemDeger.olcumNoktasi, itemDeger.otBedenID, addObj.id, 1))
                            //{
                            //    if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            //    {
                            //        addAra.olcuNoktaID = form2.seilenOlcuNok;
                            //    }
                            //}

                            addAra.olcuNoktaID = -1;
                            addAra.orijinalOlcuNok = itemDeger.olcumNoktasi.Trim().Replace("\0", "");
                            addAra.hataOldu = true;
                        }

                        if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni.Replace(" ", "")))
                        {
                            addAra.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni).FirstOrDefault().ID;
                        }
                        else
                        {
                            //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni + " bulunamadı");
                            addAra.bedenID = 1;
                            addAra.hataOldu = true;
                        }


                        addAra.deger = (double)itemDeger.olcu1;
                        dbOlcum.OlcuTabloAra.Add(addAra);
                        //dbOlcum.SaveChanges();

                        OlcuTabloHesaplama olcuHesaplama = new OlcuTabloHesaplama();

                        olcuHesaplama.olcuTabloID = addObj.id;
                        olcuHesaplama.olcuNoktaID = addAra.olcuNoktaID;

                        if (!newOlcuNoktalari.Any(x => x.olcuNoktasi == itemDeger.olcumNoktasi.Trim().Replace("\0", "")))
                        {
                            olcuHesaplama.orijinalOlcuNok = itemDeger.olcumNoktasi.Trim().Replace("\0", "");
                            olcuHesaplama.hataOldu = true;
                        }



                        olcuHesaplama.tolerans = itemDeger.tolerans;
                        olcuHesaplama.cekme = itemDeger.cekme;
                        olcuHesaplama.oran = itemDeger.oran;

                        dbOlcum.OlcuTabloHesaplama.Add(olcuHesaplama);
                        //dbOlcum.SaveChanges();


                        if (itemDeger.olcuBedeni2 != null)
                        {

                            OlcuTabloAra addAra2 = new OlcuTabloAra();
                            addAra2.olcuTabloID = addObj.id;
                            addAra2.olcuNoktaID = addAra.olcuNoktaID;



                            if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni2))
                            {
                                addAra2.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni2).FirstOrDefault().ID;
                            }
                            else
                            {
                                //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni2 + " bulunamadı");
                                addAra2.bedenID = 1;
                                addAra2.hataOldu = true;
                            }


                            addAra2.deger = (double)itemDeger.olcu2;
                            dbOlcum.OlcuTabloAra.Add(addAra2);



                            if (itemDeger.olcuBedeni3 != null)
                            {
                                OlcuTabloAra addAra3 = new OlcuTabloAra();
                                addAra3.olcuTabloID = addObj.id;
                                addAra3.olcuNoktaID = addAra.olcuNoktaID;


                                if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni3))
                                {
                                    addAra3.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni3).FirstOrDefault().ID;
                                }
                                else
                                {
                                    //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni3 + " bulunamadı");
                                    addAra3.bedenID = 1;
                                    addAra3.hataOldu = true;
                                }


                                addAra3.deger = (double)itemDeger.olcu3;
                                dbOlcum.OlcuTabloAra.Add(addAra3);
                                //dbOlcum.SaveChanges();

                                OlcuTabloHesaplama olcuHesaplama3 = new OlcuTabloHesaplama();


                                if (itemDeger.olcuBedeni4 != null)
                                {

                                    OlcuTabloAra addAra4 = new OlcuTabloAra();
                                    addAra4.olcuTabloID = addObj.id;
                                    addAra4.olcuNoktaID = addAra.olcuNoktaID;


                                    if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni4))
                                    {
                                        addAra4.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni4).FirstOrDefault().ID;
                                    }
                                    else
                                    {
                                        //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni4 + " bulunamadı");
                                        addAra4.bedenID = 1;
                                        addAra4.hataOldu = true;
                                    }


                                    addAra4.deger = (double)itemDeger.olcu4;
                                    dbOlcum.OlcuTabloAra.Add(addAra4);


                                    if (itemDeger.olcuBedeni5 != null)
                                    {
                                        OlcuTabloAra addAra5 = new OlcuTabloAra();
                                        addAra5.olcuTabloID = addObj.id;
                                        addAra5.olcuNoktaID = addAra.olcuNoktaID;


                                        if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni5))
                                        {
                                            addAra5.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni5).FirstOrDefault().ID;
                                        }
                                        else
                                        {
                                            //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni5 + " bulunamadı");
                                            addAra5.bedenID = 1;

                                            addAra5.hataOldu = true;
                                        }


                                        addAra5.deger = (double)itemDeger.olcu5;
                                        dbOlcum.OlcuTabloAra.Add(addAra5);


                                        if (itemDeger.olcuBedeni6 != null)
                                        {
                                            OlcuTabloAra addAra6 = new OlcuTabloAra();
                                            addAra6.olcuTabloID = addObj.id;
                                            addAra6.olcuNoktaID = addAra.olcuNoktaID;


                                            if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni6))
                                            {
                                                addAra6.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni6).FirstOrDefault().ID;
                                            }
                                            else
                                            {
                                                //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni6 + " bulunamadı");
                                                addAra6.bedenID = 1;
                                                addAra6.hataOldu = true;
                                            }


                                            addAra6.deger = (double)itemDeger.olcu6;
                                            dbOlcum.OlcuTabloAra.Add(addAra6);

                                            if (itemDeger.olcuBedeni7 != null)
                                            {

                                                OlcuTabloAra addAra7 = new OlcuTabloAra();
                                                addAra7.olcuTabloID = addObj.id;
                                                addAra7.olcuNoktaID = addAra.olcuNoktaID;


                                                if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni7))
                                                {
                                                    addAra7.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni7).FirstOrDefault().ID;
                                                }
                                                else
                                                {
                                                    //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni7 + " bulunamadı");
                                                    addAra7.bedenID = 1;
                                                    addAra7.hataOldu = true;
                                                }


                                                addAra7.deger = (double)itemDeger.olcu7;
                                                dbOlcum.OlcuTabloAra.Add(addAra7);


                                                if (itemDeger.olcuBedeni8 != null)
                                                {
                                                    OlcuTabloAra addAra8 = new OlcuTabloAra();
                                                    addAra8.olcuTabloID = addObj.id;
                                                    addAra8.olcuNoktaID = addAra.olcuNoktaID;


                                                    if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni8))
                                                    {
                                                        addAra8.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni8).FirstOrDefault().ID;
                                                    }
                                                    else
                                                    {
                                                        //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni8 + " bulunamadı");
                                                        addAra8.bedenID = 1;
                                                        addAra8.hataOldu = true;
                                                    }


                                                    addAra8.deger = (double)itemDeger.olcu8;
                                                    dbOlcum.OlcuTabloAra.Add(addAra8);


                                                    if (itemDeger.olcuBedeni9 != null)
                                                    {
                                                        OlcuTabloAra addAra9 = new OlcuTabloAra();
                                                        addAra9.olcuTabloID = addObj.id;
                                                        addAra9.olcuNoktaID = addAra.olcuNoktaID;

                                                        if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni9))
                                                        {
                                                            addAra9.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni9).FirstOrDefault().ID;
                                                        }
                                                        else
                                                        {
                                                            //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni9 + " bulunamadı");
                                                            addAra9.bedenID = 1;
                                                            addAra9.hataOldu = true;

                                                        }


                                                        addAra9.deger = (double)itemDeger.olcu9;
                                                        dbOlcum.OlcuTabloAra.Add(addAra9);



                                                        if (itemDeger.olcuBedeni10 != null)
                                                        {
                                                            OlcuTabloAra addAra10 = new OlcuTabloAra();
                                                            addAra10.olcuTabloID = addObj.id;
                                                            addAra10.olcuNoktaID = addAra.olcuNoktaID;


                                                            if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni10))
                                                            {
                                                                addAra10.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni10).FirstOrDefault().ID;
                                                            }
                                                            else
                                                            {
                                                                //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni10 + " bulunamadı");
                                                                addAra10.bedenID = 1;
                                                                addAra10.hataOldu = true;

                                                            }


                                                            addAra10.deger = (double)itemDeger.olcu10;
                                                            dbOlcum.OlcuTabloAra.Add(addAra10);



                                                            if (itemDeger.olcuBedeni11 != null)
                                                            {
                                                                OlcuTabloAra addAra11 = new OlcuTabloAra();
                                                                addAra11.olcuTabloID = addObj.id;
                                                                addAra11.olcuNoktaID = addAra.olcuNoktaID;

                                                                if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni11))
                                                                {
                                                                    addAra11.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni11).FirstOrDefault().ID;
                                                                }
                                                                else
                                                                {
                                                                    //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni11 + " bulunamadı");
                                                                    addAra11.bedenID = 1;
                                                                    addAra11.hataOldu = true;
                                                                }


                                                                addAra11.deger = (double)itemDeger.olcu11;
                                                                dbOlcum.OlcuTabloAra.Add(addAra11);


                                                                if (itemDeger.olcuBedeni12 != null)
                                                                {

                                                                    OlcuTabloAra addAra12 = new OlcuTabloAra();
                                                                    addAra12.olcuTabloID = addObj.id;
                                                                    addAra12.olcuNoktaID = addAra.olcuNoktaID;


                                                                    if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni12))
                                                                    {
                                                                        addAra12.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni12).FirstOrDefault().ID;
                                                                    }
                                                                    else
                                                                    {
                                                                        //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni12 + " bulunamadı");
                                                                        addAra12.bedenID = 1;
                                                                        addAra12.hataOldu = true;
                                                                    }


                                                                    addAra12.deger = (double)itemDeger.olcu12;
                                                                    dbOlcum.OlcuTabloAra.Add(addAra12);


                                                                    if (itemDeger.olcuBedeni13 != null)
                                                                    {

                                                                        OlcuTabloAra addAra13 = new OlcuTabloAra();
                                                                        addAra13.olcuTabloID = addObj.id;
                                                                        addAra13.olcuNoktaID = addAra.olcuNoktaID;


                                                                        if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni13))
                                                                        {
                                                                            addAra13.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni13).FirstOrDefault().ID;
                                                                        }
                                                                        else
                                                                        {
                                                                            //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni13 + " bulunamadı");
                                                                            addAra13.bedenID = 1;
                                                                            addAra13.hataOldu = true;
                                                                        }


                                                                        addAra13.deger = (double)itemDeger.olcu13;
                                                                        dbOlcum.OlcuTabloAra.Add(addAra13);


                                                                        if (itemDeger.olcuBedeni14 != null)
                                                                        {
                                                                            OlcuTabloAra addAra14 = new OlcuTabloAra();
                                                                            addAra14.olcuTabloID = addObj.id;
                                                                            addAra14.olcuNoktaID = addAra.olcuNoktaID;




                                                                            if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni14))
                                                                            {
                                                                                addAra14.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni14).FirstOrDefault().ID;
                                                                            }
                                                                            else
                                                                            {
                                                                                //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni14 + " bulunamadı");
                                                                                addAra14.bedenID = 1;
                                                                                addAra14.hataOldu = true;
                                                                            }


                                                                            addAra14.deger = (double)itemDeger.olcu14;
                                                                            dbOlcum.OlcuTabloAra.Add(addAra14);



                                                                            if (itemDeger.olcuBedeni15 != null)
                                                                            {
                                                                                OlcuTabloAra addAra15 = new OlcuTabloAra();
                                                                                addAra15.olcuTabloID = addObj.id;
                                                                                addAra15.olcuNoktaID = addAra.olcuNoktaID;



                                                                                if (newBedenler.Any(x => x.beden == itemDeger.olcuBedeni15))
                                                                                {
                                                                                    addAra15.bedenID = newBedenler.Where(x => x.beden == itemDeger.olcuBedeni15).FirstOrDefault().ID;
                                                                                }
                                                                                else
                                                                                {
                                                                                    //MessageBox.Show("OlcuTablosu Id=" + item.ottabloID.ToString() + "  OlcuTablosuBedenId=" + itemDeger.otBedenID + "   Beden=" + itemDeger.olcuBedeni15 + " bulunamadı");
                                                                                    addAra15.bedenID = 1;
                                                                                    addAra15.hataOldu = true;
                                                                                }


                                                                                addAra15.deger = (double)itemDeger.olcu15;
                                                                                dbOlcum.OlcuTabloAra.Add(addAra15);


                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                dbOlcum.SaveChanges();
            }
            MessageBox.Show("Kayıt Başarılı");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //List<OlcuFormu2> listOlcuTablosu = dbOlcumRaporlama.OlcuFormu2.Where(x => x.oftabloID2 > 107).OrderBy(y => y.oftabloID2).Skip(1000).Take(50).ToList();

            //List<dbOlcumTest.Order> newOrderList = dbOlcum.Order.ToList();
            //List<OlcuTurleri> newOlcuTurleriList = dbOlcum.OlcuTurleri.ToList(); 
            //List<GiysiTurleri> newGiysiTurleriList = dbOlcum.GiysiTurleri.ToList();
            //List<Atolyes> newAtolyeList = dbAtolye.Atolyes.ToList(); 

            //List<PersonelTablo> newPersonelList = dbOlcum.PersonelTablo.ToList();
            //List<OlcuNoktalari> newOlcuNokList = dbOlcum.OlcuNoktalari.ToList();
            //List<Bedenler> newBedenlerList = dbOlcum.Bedenler.ToList();


            //foreach (var item in listOlcuTablosu)
            //{
            //    NumuneDetay addObjNumuneDetay = new NumuneDetay();
            //    label1.Text = "OlcuFormu2 Id=" + item.oftabloID2.ToString();

            //    //Order
            //    if (newOrderList.Any(x => x.orderNo == item.orderNo))
            //    {
            //        addObjNumuneDetay.orderID = newOrderList.Where(x => x.orderNo == item.orderNo).FirstOrDefault().ID;

            //        addObjNumuneDetay.yikamayaGidis = item.yikamayaGidis;
            //        addObjNumuneDetay.yikamadanGelis = item.yikamadanGelis;
            //        addObjNumuneDetay.kalipAdi = item.kalipAdi;
            //        addObjNumuneDetay.topNo = item.topNo;
            //        addObjNumuneDetay.receteKod = item.receteKod;
            //        addObjNumuneDetay.elliListe = item.elliListe;
            //        addObjNumuneDetay.elliNumune = item.elliNumune;
            //        addObjNumuneDetay.dikimNedeni = item.dikimNedeni;

            //        //OlcuTuru
            //        if (newOlcuTurleriList.Any(c => c.olcuTuruAd == item.olcuTuru))
            //        {
            //            addObjNumuneDetay.olcuTuruID = newOlcuTurleriList.Where(c => c.olcuTuruAd == item.olcuTuru).FirstOrDefault().id;
            //        }
            //        else
            //        {
            //            //MessageBox.Show("OlcuForm2 Id=" + item.oftabloID2.ToString() + " Aranan Olçü Türü=" + item.olcuTuru + " bulunamadı");
            //            addObjNumuneDetay.olcuTuruID = 1;
            //        }


            //        //GiysiTuru
            //        if (newGiysiTurleriList.Any(c => c.giysiTuruAd == item.giysiTuru))
            //        {
            //            addObjNumuneDetay.giysiTuruID = newGiysiTurleriList.Where(c => c.giysiTuruAd == item.giysiTuru).FirstOrDefault().id;
            //        }
            //        else
            //        {
            //            //MessageBox.Show("OlcuForm2 Id=" + item.oftabloID2.ToString() + " Aranan Giysi Türü=" + item.giysiTuru + "  bulunamadı");
            //            addObjNumuneDetay.giysiTuruID = 1;
            //        }


            //        //Atolye
            //        if (item.dikimAtolyesi != null)
            //        {
            //            if (newAtolyeList.Any(x => x.AtolyeAd == item.dikimAtolyesi))
            //            {
            //                addObjNumuneDetay.atolyeID = newAtolyeList.Where(x => x.AtolyeAd == item.dikimAtolyesi).FirstOrDefault().Id;
            //            }
            //            else
            //            {
            //                using (AtolyeDialog form3 = new AtolyeDialog(item.oftabloID2, item.dikimAtolyesi, 1))
            //                {
            //                    if (form3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                    {
            //                        addObjNumuneDetay.atolyeID = form3.seilenAtolyeId;
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            addObjNumuneDetay.atolyeID = 66;// Atölye Belirtilmemiş
            //        }


            //        addObjNumuneDetay.tarih = item.tarih;


            //        addObjNumuneDetay.aciklama = item.aciklama;
            //        addObjNumuneDetay.olcumNo = item.olcuTuruNo;
            //        addObjNumuneDetay.KID = item.KID;


            //        //KullanıcıID
            //        if (item.modelistAd != null)
            //        {
            //            if (newPersonelList.Any(x => x.personelAd == item.modelistAd))
            //            {
            //                if (item.modelistAd == "Elif")
            //                {
            //                    addObjNumuneDetay.kullanıcıID = 7;// Elif Akgün

            //                }
            //                else if (item.modelistAd == "Nevin")
            //                {
            //                    addObjNumuneDetay.kullanıcıID = 10;// Nevin Deveci
            //                }
            //                else
            //                {
            //                    addObjNumuneDetay.kullanıcıID = newPersonelList.Where(x => x.personelAd == item.modelistAd).FirstOrDefault().id;
            //                }


            //            }
            //            else
            //            {
            //                using (UserDialog form2 = new UserDialog(item.modelistAd, item.oftabloID2, 2))
            //                {
            //                    if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                    {
            //                        addObjNumuneDetay.kullanıcıID = form2.seilenUserId;
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            addObjNumuneDetay.kullanıcıID = 18;//Nagihan Polat
            //        }


            //        dbOlcum.NumuneDetay.Add(addObjNumuneDetay);
            //        dbOlcum.SaveChanges();

            //        List<OlcuFormuBeden2> listOlcuFormuBeden2 = dbOlcumRaporlama.OlcuFormuBeden2.Where(x => x.oftabloID2 == item.oftabloID2).ToList();


            //        foreach (var itemOlcuFormuBeden2 in listOlcuFormuBeden2)
            //        {

            //            if (itemOlcuFormuBeden2.olcuNoktasi != null)
            //            {
            //                NumuneAra ObjNumuneAra = new NumuneAra();


            //                //Olcu Noktalari
            //                if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi))
            //                {
            //                    ObjNumuneAra.olcuNoktaID = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi).FirstOrDefault().id;
            //                }
            //                else
            //                {
            //                    using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden2.olcuNoktasi, itemOlcuFormuBeden2.ofBedenID2, addObjNumuneDetay.id, 2))
            //                    {
            //                        if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                        {
            //                            ObjNumuneAra.olcuNoktaID = form2.seilenOlcuNok;
            //                        }
            //                    }
            //                }


            //                //Bedenler
            //                if (itemOlcuFormuBeden2.olcuBedeni.Contains("Müdahale"))
            //                {
            //                    var BedenArray = itemOlcuFormuBeden2.olcuBedeni.Split('-');
            //                    var bedenString = BedenArray[0].Trim();

            //                    if (newBedenlerList.Any(x => x.beden == bedenString))
            //                    {
            //                        ObjNumuneAra.bedenID = newBedenlerList.Where(x => x.beden == bedenString).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + bedenString + " bulunamadı");
            //                        ObjNumuneAra.bedenID = 1;
            //                    }

            //                    ObjNumuneAra.mudahaleMi = true;
            //                }
            //                else
            //                {

            //                    if (newBedenlerList.Any(x => x.beden == itemOlcuFormuBeden2.olcuBedeni))
            //                    {
            //                        ObjNumuneAra.bedenID = newBedenlerList.Where(x => x.beden == itemOlcuFormuBeden2.olcuBedeni).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + itemOlcuFormuBeden2.olcuBedeni + " bulunamadı");
            //                        ObjNumuneAra.bedenID = 1;
            //                    }

            //                    ObjNumuneAra.mudahaleMi = false;

            //                }


            //                ObjNumuneAra.imalat_Kalip = itemOlcuFormuBeden2.imlt_Kalip1;
            //                ObjNumuneAra.imalat_Tol = itemOlcuFormuBeden2.imlt_Tol1;
            //                ObjNumuneAra.imalat_YO = itemOlcuFormuBeden2.imlt_YO1;
            //                ObjNumuneAra.imalat_Cekme = itemOlcuFormuBeden2.imlt_Cekme1;
            //                ObjNumuneAra.kalipOlcusu = itemOlcuFormuBeden2.kalıpOlcusu;
            //                ObjNumuneAra.kalipFark = itemOlcuFormuBeden2.kalipFark;
            //                ObjNumuneAra.yikOncesi = itemOlcuFormuBeden2.yikOncesi;
            //                ObjNumuneAra.yikCekme = itemOlcuFormuBeden2.yikCekme;
            //                ObjNumuneAra.yikSonrasi = itemOlcuFormuBeden2.yikSonrasi;
            //                ObjNumuneAra.yikamaFark = itemOlcuFormuBeden2.yikamaFark;
            //                ObjNumuneAra.ysonTablo = itemOlcuFormuBeden2.ysonTablo;

            //                ObjNumuneAra.numuneDetayID = addObjNumuneDetay.id;

            //                ObjNumuneAra.ortCekme = itemOlcuFormuBeden2.ortCekme;
            //                ObjNumuneAra.ortTolerans = itemOlcuFormuBeden2.ortTolerans;


            //                dbOlcum.NumuneAra.Add(ObjNumuneAra);
            //                dbOlcum.SaveChanges();

            //            }

            //            if (itemOlcuFormuBeden2.olcuNoktasi2 != null)
            //            {
            //                NumuneAra ObjNumuneAra2 = new NumuneAra();


            //                //Olcu Noktalari
            //                if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi2))
            //                {
            //                    ObjNumuneAra2.olcuNoktaID = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi2).FirstOrDefault().id;
            //                }
            //                else
            //                {
            //                    using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden2.olcuNoktasi2, itemOlcuFormuBeden2.ofBedenID2, addObjNumuneDetay.id, 2))
            //                    {
            //                        if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                        {
            //                            ObjNumuneAra2.olcuNoktaID = form2.seilenOlcuNok;
            //                        }
            //                    }
            //                }


            //                //Bedenler
            //                if (itemOlcuFormuBeden2.olcuBedeni2.Contains("Müdahale"))
            //                {
            //                    var BedenArray = itemOlcuFormuBeden2.olcuBedeni2.Split('-');
            //                    var bedenString = BedenArray[0].Trim();

            //                    if (newBedenlerList.Any(x => x.beden == bedenString))
            //                    {
            //                        ObjNumuneAra2.bedenID = newBedenlerList.Where(x => x.beden == bedenString).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + bedenString + " bulunamadı");
            //                        ObjNumuneAra2.bedenID = 1;
            //                    }
            //                }
            //                else
            //                {

            //                    if (newBedenlerList.Any(x => x.beden == itemOlcuFormuBeden2.olcuBedeni2))
            //                    {
            //                        ObjNumuneAra2.bedenID = newBedenlerList.Where(x => x.beden == itemOlcuFormuBeden2.olcuBedeni2).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + itemOlcuFormuBeden2.olcuBedeni2 + " bulunamadı");
            //                        ObjNumuneAra2.bedenID = 1;
            //                    }
            //                }


            //                ObjNumuneAra2.imalat_Kalip = itemOlcuFormuBeden2.imlt_Kalip2;
            //                ObjNumuneAra2.imalat_Tol = itemOlcuFormuBeden2.imlt_Tol2;
            //                ObjNumuneAra2.imalat_YO = itemOlcuFormuBeden2.imlt_YO2;
            //                ObjNumuneAra2.imalat_Cekme = itemOlcuFormuBeden2.imlt_Cekme2;
            //                ObjNumuneAra2.kalipOlcusu = itemOlcuFormuBeden2.kalipOlcusu2;
            //                ObjNumuneAra2.kalipFark = itemOlcuFormuBeden2.kalipFark2;
            //                ObjNumuneAra2.yikOncesi = itemOlcuFormuBeden2.yikOncesi2;
            //                ObjNumuneAra2.yikCekme = itemOlcuFormuBeden2.yikCekme2;
            //                ObjNumuneAra2.yikSonrasi = itemOlcuFormuBeden2.yikSonrasi2;
            //                ObjNumuneAra2.yikamaFark = itemOlcuFormuBeden2.yikamaFark2;
            //                ObjNumuneAra2.ysonTablo = itemOlcuFormuBeden2.ysonTablo2;


            //                ObjNumuneAra2.numuneDetayID = addObjNumuneDetay.id;

            //                ObjNumuneAra2.ortCekme = itemOlcuFormuBeden2.ortCekme;
            //                ObjNumuneAra2.ortTolerans = itemOlcuFormuBeden2.ortTolerans;

            //                dbOlcum.NumuneAra.Add(ObjNumuneAra2);
            //                dbOlcum.SaveChanges();
            //            }

            //            if (itemOlcuFormuBeden2.olcuNoktasi3 != null)
            //            {
            //                NumuneAra ObjNumuneAra3 = new NumuneAra();


            //                //Olcu Noktalari
            //                if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi3))
            //                {
            //                    ObjNumuneAra3.olcuNoktaID = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi3).FirstOrDefault().id;
            //                }
            //                else
            //                {
            //                    using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden2.olcuNoktasi3, itemOlcuFormuBeden2.ofBedenID2, addObjNumuneDetay.id, 2))
            //                    {
            //                        if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                        {
            //                            ObjNumuneAra3.olcuNoktaID = form2.seilenOlcuNok;
            //                        }
            //                    }
            //                }


            //                //Bedenler
            //                if (itemOlcuFormuBeden2.olcuBedeni3.Contains("Müdahale"))
            //                {
            //                    var BedenArray = itemOlcuFormuBeden2.olcuBedeni3.Split('-');
            //                    var bedenString = BedenArray[0].Trim();

            //                    if (newBedenlerList.Any(x => x.beden == bedenString))
            //                    {
            //                        ObjNumuneAra3.bedenID = newBedenlerList.Where(x => x.beden == bedenString).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + bedenString + " bulunamadı");
            //                        ObjNumuneAra3.bedenID = 1;
            //                    }
            //                }
            //                else
            //                {

            //                    if (newBedenlerList.Any(x => x.beden == itemOlcuFormuBeden2.olcuBedeni3))
            //                    {
            //                        ObjNumuneAra3.bedenID = newBedenlerList.Where(x => x.beden == itemOlcuFormuBeden2.olcuBedeni3).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + itemOlcuFormuBeden2.olcuBedeni3 + " bulunamadı");
            //                        ObjNumuneAra3.bedenID = 1;
            //                    }
            //                }


            //                ObjNumuneAra3.imalat_Kalip = itemOlcuFormuBeden2.imlt_Kalip3;
            //                ObjNumuneAra3.imalat_Tol = itemOlcuFormuBeden2.imlt_Tol3;
            //                ObjNumuneAra3.imalat_YO = itemOlcuFormuBeden2.imlt_YO3;
            //                ObjNumuneAra3.imalat_Cekme = itemOlcuFormuBeden2.imlt_Cekme3;
            //                ObjNumuneAra3.kalipOlcusu = itemOlcuFormuBeden2.kalipOlcusu3;
            //                ObjNumuneAra3.kalipFark = itemOlcuFormuBeden2.kalipFark3;
            //                ObjNumuneAra3.yikOncesi = itemOlcuFormuBeden2.yikOncesi3;
            //                ObjNumuneAra3.yikCekme = itemOlcuFormuBeden2.yikCekme3;
            //                ObjNumuneAra3.yikSonrasi = itemOlcuFormuBeden2.yikSonrasi3;
            //                ObjNumuneAra3.yikamaFark = itemOlcuFormuBeden2.yikamaFark3;
            //                ObjNumuneAra3.ysonTablo = itemOlcuFormuBeden2.ysonTablo3;


            //                ObjNumuneAra3.numuneDetayID = addObjNumuneDetay.id;

            //                ObjNumuneAra3.ortCekme = itemOlcuFormuBeden2.ortCekme;
            //                ObjNumuneAra3.ortTolerans = itemOlcuFormuBeden2.ortTolerans;


            //                dbOlcum.NumuneAra.Add(ObjNumuneAra3);
            //                dbOlcum.SaveChanges();
            //            }


            //            if (itemOlcuFormuBeden2.olcuNoktasi4 != null)
            //            {
            //                NumuneAra ObjNumuneAra4 = new NumuneAra();


            //                //Olcu Noktalari
            //                if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi4))
            //                {
            //                    ObjNumuneAra4.olcuNoktaID = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi4).FirstOrDefault().id;
            //                }
            //                else
            //                {
            //                    using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden2.olcuNoktasi4, itemOlcuFormuBeden2.ofBedenID2, addObjNumuneDetay.id, 2))
            //                    {
            //                        if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                        {
            //                            ObjNumuneAra4.olcuNoktaID = form2.seilenOlcuNok;
            //                        }
            //                    }
            //                }


            //                //Bedenler
            //                if (itemOlcuFormuBeden2.olcuBedeni4.Contains("Müdahale"))
            //                {
            //                    var BedenArray = itemOlcuFormuBeden2.olcuBedeni4.Split('-');
            //                    var bedenString = BedenArray[0].Trim();

            //                    if (newBedenlerList.Any(x => x.beden == bedenString))
            //                    {
            //                        ObjNumuneAra4.bedenID = newBedenlerList.Where(x => x.beden == bedenString).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + bedenString + " bulunamadı");
            //                        ObjNumuneAra4.bedenID = 1;
            //                    }
            //                }
            //                else
            //                {

            //                    if (newBedenlerList.Any(x => x.beden == itemOlcuFormuBeden2.olcuBedeni4))
            //                    {
            //                        ObjNumuneAra4.bedenID = newBedenlerList.Where(x => x.beden == itemOlcuFormuBeden2.olcuBedeni4).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + itemOlcuFormuBeden2.olcuBedeni4 + " bulunamadı");
            //                        ObjNumuneAra4.bedenID = 1;
            //                    }
            //                }


            //                ObjNumuneAra4.imalat_Kalip = itemOlcuFormuBeden2.imlt_Kalip4;
            //                ObjNumuneAra4.imalat_Tol = itemOlcuFormuBeden2.imlt_Tol4;
            //                ObjNumuneAra4.imalat_YO = itemOlcuFormuBeden2.imlt_YO4;
            //                ObjNumuneAra4.imalat_Cekme = itemOlcuFormuBeden2.imlt_Cekme4;
            //                ObjNumuneAra4.kalipOlcusu = itemOlcuFormuBeden2.kalipOlcusu4;
            //                ObjNumuneAra4.kalipFark = itemOlcuFormuBeden2.kalipFark4;
            //                ObjNumuneAra4.yikOncesi = itemOlcuFormuBeden2.yikOncesi4;
            //                ObjNumuneAra4.yikCekme = itemOlcuFormuBeden2.yikCekme4;
            //                ObjNumuneAra4.yikSonrasi = itemOlcuFormuBeden2.yikSonrasi4;
            //                ObjNumuneAra4.yikamaFark = itemOlcuFormuBeden2.yikamaFark4;
            //                ObjNumuneAra4.ysonTablo = itemOlcuFormuBeden2.ysonTablo4;


            //                ObjNumuneAra4.numuneDetayID = addObjNumuneDetay.id;

            //                ObjNumuneAra4.ortCekme = itemOlcuFormuBeden2.ortCekme;
            //                ObjNumuneAra4.ortTolerans = itemOlcuFormuBeden2.ortTolerans;

            //                dbOlcum.NumuneAra.Add(ObjNumuneAra4);
            //                dbOlcum.SaveChanges();
            //            }


            //            if (itemOlcuFormuBeden2.olcuNoktasi5 != null)
            //            {
            //                NumuneAra ObjNumuneAra5 = new NumuneAra();


            //                //Olcu Noktalari
            //                if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi5))
            //                {
            //                    ObjNumuneAra5.olcuNoktaID = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi5).FirstOrDefault().id;
            //                }
            //                else
            //                {
            //                    using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden2.olcuNoktasi5, itemOlcuFormuBeden2.ofBedenID2, addObjNumuneDetay.id, 2))
            //                    {
            //                        if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                        {
            //                            ObjNumuneAra5.olcuNoktaID = form2.seilenOlcuNok;
            //                        }
            //                    }
            //                }


            //                //Bedenler
            //                if (itemOlcuFormuBeden2.olcuBedeni5.Contains("Müdahale"))
            //                {
            //                    var BedenArray = itemOlcuFormuBeden2.olcuBedeni5.Split('-');
            //                    var bedenString = BedenArray[0].Trim();

            //                    if (newBedenlerList.Any(x => x.beden == bedenString))
            //                    {
            //                        ObjNumuneAra5.bedenID = newBedenlerList.Where(x => x.beden == bedenString).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + bedenString + " bulunamadı");
            //                        ObjNumuneAra5.bedenID = 1;
            //                    }
            //                }
            //                else
            //                {

            //                    if (newBedenlerList.Any(x => x.beden == itemOlcuFormuBeden2.olcuBedeni5))
            //                    {
            //                        ObjNumuneAra5.bedenID = newBedenlerList.Where(x => x.beden == itemOlcuFormuBeden2.olcuBedeni5).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + itemOlcuFormuBeden2.olcuBedeni5 + " bulunamadı");
            //                        ObjNumuneAra5.bedenID = 1;
            //                    }
            //                }


            //                ObjNumuneAra5.imalat_Kalip = itemOlcuFormuBeden2.imlt_Kalip5;
            //                ObjNumuneAra5.imalat_Tol = itemOlcuFormuBeden2.imlt_Tol5;
            //                ObjNumuneAra5.imalat_YO = itemOlcuFormuBeden2.imlt_YO5;
            //                ObjNumuneAra5.imalat_Cekme = itemOlcuFormuBeden2.imlt_Cekme5;
            //                ObjNumuneAra5.kalipOlcusu = itemOlcuFormuBeden2.kalipOlcusu5;
            //                ObjNumuneAra5.kalipFark = itemOlcuFormuBeden2.kalipFark5;
            //                ObjNumuneAra5.yikOncesi = itemOlcuFormuBeden2.yikOncesi5;
            //                ObjNumuneAra5.yikCekme = itemOlcuFormuBeden2.yikCekme5;
            //                ObjNumuneAra5.yikSonrasi = itemOlcuFormuBeden2.yikSonrasi5;
            //                ObjNumuneAra5.yikamaFark = itemOlcuFormuBeden2.yikamaFark5;
            //                ObjNumuneAra5.ysonTablo = itemOlcuFormuBeden2.ysonTablo5;


            //                ObjNumuneAra5.numuneDetayID = addObjNumuneDetay.id;

            //                ObjNumuneAra5.ortCekme = itemOlcuFormuBeden2.ortCekme;
            //                ObjNumuneAra5.ortTolerans = itemOlcuFormuBeden2.ortTolerans;


            //                dbOlcum.NumuneAra.Add(ObjNumuneAra5);
            //                dbOlcum.SaveChanges();
            //            }

            //            if (itemOlcuFormuBeden2.olcuNoktasi6 != null)
            //            {
            //                NumuneAra ObjNumuneAra6 = new NumuneAra();


            //                //Olcu Noktalari
            //                if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi6))
            //                {
            //                    ObjNumuneAra6.olcuNoktaID = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi6).FirstOrDefault().id;
            //                }
            //                else
            //                {
            //                    using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden2.olcuNoktasi6, itemOlcuFormuBeden2.ofBedenID2, addObjNumuneDetay.id, 2))
            //                    {
            //                        if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                        {
            //                            ObjNumuneAra6.olcuNoktaID = form2.seilenOlcuNok;
            //                        }
            //                    }
            //                }


            //                //Bedenler
            //                if (itemOlcuFormuBeden2.olcuBedeni6.Contains("Müdahale"))
            //                {
            //                    var BedenArray = itemOlcuFormuBeden2.olcuBedeni6.Split('-');
            //                    var bedenString = BedenArray[0].Trim();

            //                    if (newBedenlerList.Any(x => x.beden == bedenString))
            //                    {
            //                        ObjNumuneAra6.bedenID = newBedenlerList.Where(x => x.beden == bedenString).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + bedenString + " bulunamadı");
            //                        ObjNumuneAra6.bedenID = 1;
            //                    }
            //                }
            //                else
            //                {

            //                    if (newBedenlerList.Any(x => x.beden == itemOlcuFormuBeden2.olcuBedeni6))
            //                    {
            //                        ObjNumuneAra6.bedenID = newBedenlerList.Where(x => x.beden == itemOlcuFormuBeden2.olcuBedeni6).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + itemOlcuFormuBeden2.olcuBedeni6 + " bulunamadı");
            //                        ObjNumuneAra6.bedenID = 1;
            //                    }
            //                }


            //                ObjNumuneAra6.imalat_Kalip = itemOlcuFormuBeden2.imlt_Kalip6;
            //                ObjNumuneAra6.imalat_Tol = itemOlcuFormuBeden2.imlt_Tol6;
            //                ObjNumuneAra6.imalat_YO = itemOlcuFormuBeden2.imlt_YO6;
            //                ObjNumuneAra6.imalat_Cekme = itemOlcuFormuBeden2.imlt_Cekme6;
            //                ObjNumuneAra6.kalipOlcusu = itemOlcuFormuBeden2.kalipOlcusu6;
            //                ObjNumuneAra6.kalipFark = itemOlcuFormuBeden2.kalipFark6;
            //                ObjNumuneAra6.yikOncesi = itemOlcuFormuBeden2.yikOncesi6;
            //                ObjNumuneAra6.yikCekme = itemOlcuFormuBeden2.yikCekme6;
            //                ObjNumuneAra6.yikSonrasi = itemOlcuFormuBeden2.yikSonrasi6;
            //                ObjNumuneAra6.yikamaFark = itemOlcuFormuBeden2.yikamaFark6;
            //                ObjNumuneAra6.ysonTablo = itemOlcuFormuBeden2.ysonTablo6;


            //                ObjNumuneAra6.numuneDetayID = addObjNumuneDetay.id;

            //                ObjNumuneAra6.ortCekme = itemOlcuFormuBeden2.ortCekme;
            //                ObjNumuneAra6.ortTolerans = itemOlcuFormuBeden2.ortTolerans;


            //                dbOlcum.NumuneAra.Add(ObjNumuneAra6);
            //                dbOlcum.SaveChanges();
            //            }


            //            if (itemOlcuFormuBeden2.olcuNoktasi7 != null)
            //            {
            //                NumuneAra ObjNumuneAra7 = new NumuneAra();


            //                //Olcu Noktalari
            //                if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi7))
            //                {
            //                    ObjNumuneAra7.olcuNoktaID = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi7).FirstOrDefault().id;
            //                }
            //                else
            //                {
            //                    using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden2.olcuNoktasi7, itemOlcuFormuBeden2.ofBedenID2, addObjNumuneDetay.id, 2))
            //                    {
            //                        if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                        {
            //                            ObjNumuneAra7.olcuNoktaID = form2.seilenOlcuNok;
            //                        }
            //                    }
            //                }


            //                //Bedenler
            //                if (itemOlcuFormuBeden2.olcuBedeni7.Contains("Müdahale"))
            //                {
            //                    var BedenArray = itemOlcuFormuBeden2.olcuBedeni7.Split('-');
            //                    var bedenString = BedenArray[0].Trim();

            //                    if (newBedenlerList.Any(x => x.beden == bedenString))
            //                    {
            //                        ObjNumuneAra7.bedenID = newBedenlerList.Where(x => x.beden == bedenString).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + bedenString + " bulunamadı");
            //                        ObjNumuneAra7.bedenID = 1;
            //                    }
            //                }
            //                else
            //                {

            //                    if (newBedenlerList.Any(x => x.beden == itemOlcuFormuBeden2.olcuBedeni7))
            //                    {
            //                        ObjNumuneAra7.bedenID = newBedenlerList.Where(x => x.beden == itemOlcuFormuBeden2.olcuBedeni7).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + itemOlcuFormuBeden2.olcuBedeni7 + " bulunamadı");
            //                        ObjNumuneAra7.bedenID = 1;
            //                    }
            //                }


            //                ObjNumuneAra7.imalat_Kalip = itemOlcuFormuBeden2.imlt_Kalip7;
            //                ObjNumuneAra7.imalat_Tol = itemOlcuFormuBeden2.imlt_Tol7;
            //                ObjNumuneAra7.imalat_YO = itemOlcuFormuBeden2.imlt_YO7;
            //                ObjNumuneAra7.imalat_Cekme = itemOlcuFormuBeden2.imlt_Cekme7;
            //                ObjNumuneAra7.kalipOlcusu = itemOlcuFormuBeden2.kalipOlcusu7;
            //                ObjNumuneAra7.kalipFark = itemOlcuFormuBeden2.kalipFark7;
            //                ObjNumuneAra7.yikOncesi = itemOlcuFormuBeden2.yikOncesi7;
            //                ObjNumuneAra7.yikCekme = itemOlcuFormuBeden2.yikCekme7;
            //                ObjNumuneAra7.yikSonrasi = itemOlcuFormuBeden2.yikSonrasi7;
            //                ObjNumuneAra7.yikamaFark = itemOlcuFormuBeden2.yikamaFark7;
            //                ObjNumuneAra7.ysonTablo = itemOlcuFormuBeden2.ysonTablo7;


            //                ObjNumuneAra7.numuneDetayID = addObjNumuneDetay.id;

            //                ObjNumuneAra7.ortCekme = itemOlcuFormuBeden2.ortCekme;
            //                ObjNumuneAra7.ortTolerans = itemOlcuFormuBeden2.ortTolerans;


            //                dbOlcum.NumuneAra.Add(ObjNumuneAra7);
            //                dbOlcum.SaveChanges();
            //            }

            //            if (itemOlcuFormuBeden2.olcuNoktasi8 != null)
            //            {
            //                NumuneAra ObjNumuneAra8 = new NumuneAra();


            //                //Olcu Noktalari
            //                if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi8))
            //                {
            //                    ObjNumuneAra8.olcuNoktaID = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi8).FirstOrDefault().id;
            //                }
            //                else
            //                {
            //                    using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden2.olcuNoktasi8, itemOlcuFormuBeden2.ofBedenID2, addObjNumuneDetay.id, 2))
            //                    {
            //                        if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                        {
            //                            ObjNumuneAra8.olcuNoktaID = form2.seilenOlcuNok;
            //                        }
            //                    }
            //                }


            //                //Bedenler
            //                if (itemOlcuFormuBeden2.olcuBedeni8.Contains("Müdahale"))
            //                {
            //                    var BedenArray = itemOlcuFormuBeden2.olcuBedeni8.Split('-');
            //                    var bedenString = BedenArray[0].Trim();

            //                    if (newBedenlerList.Any(x => x.beden == bedenString))
            //                    {
            //                        ObjNumuneAra8.bedenID = newBedenlerList.Where(x => x.beden == bedenString).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + bedenString + " bulunamadı");
            //                        ObjNumuneAra8.bedenID = 1;
            //                    }
            //                }
            //                else
            //                {

            //                    if (newBedenlerList.Any(x => x.beden == itemOlcuFormuBeden2.olcuBedeni8))
            //                    {
            //                        ObjNumuneAra8.bedenID = newBedenlerList.Where(x => x.beden == itemOlcuFormuBeden2.olcuBedeni8).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + itemOlcuFormuBeden2.olcuBedeni8 + " bulunamadı");
            //                        ObjNumuneAra8.bedenID = 1;
            //                    }
            //                }


            //                ObjNumuneAra8.imalat_Kalip = itemOlcuFormuBeden2.imlt_Kalip8;
            //                ObjNumuneAra8.imalat_Tol = itemOlcuFormuBeden2.imlt_Tol8;
            //                ObjNumuneAra8.imalat_YO = itemOlcuFormuBeden2.imlt_YO8;
            //                ObjNumuneAra8.imalat_Cekme = itemOlcuFormuBeden2.imlt_Cekme8;
            //                ObjNumuneAra8.kalipOlcusu = itemOlcuFormuBeden2.kalipOlcusu8;
            //                ObjNumuneAra8.kalipFark = itemOlcuFormuBeden2.kalipFark8;
            //                ObjNumuneAra8.yikOncesi = itemOlcuFormuBeden2.yikOncesi8;
            //                ObjNumuneAra8.yikCekme = itemOlcuFormuBeden2.yikCekme8;
            //                ObjNumuneAra8.yikSonrasi = itemOlcuFormuBeden2.yikSonrasi8;
            //                ObjNumuneAra8.yikamaFark = itemOlcuFormuBeden2.yikamaFark8;
            //                ObjNumuneAra8.ysonTablo = itemOlcuFormuBeden2.ysonTablo8;


            //                ObjNumuneAra8.numuneDetayID = addObjNumuneDetay.id;

            //                ObjNumuneAra8.ortCekme = itemOlcuFormuBeden2.ortCekme;
            //                ObjNumuneAra8.ortTolerans = itemOlcuFormuBeden2.ortTolerans;


            //                dbOlcum.NumuneAra.Add(ObjNumuneAra8);
            //                dbOlcum.SaveChanges();
            //            }

            //            if (itemOlcuFormuBeden2.olcuNoktasi9 != null)
            //            {
            //                NumuneAra ObjNumuneAra9 = new NumuneAra();


            //                //Olcu Noktalari
            //                if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi9))
            //                {
            //                    ObjNumuneAra9.olcuNoktaID = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi9).FirstOrDefault().id;
            //                }
            //                else
            //                {
            //                    using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden2.olcuNoktasi9, itemOlcuFormuBeden2.ofBedenID2, addObjNumuneDetay.id, 2))
            //                    {
            //                        if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                        {
            //                            ObjNumuneAra9.olcuNoktaID = form2.seilenOlcuNok;
            //                        }
            //                    }
            //                }


            //                //Bedenler
            //                if (itemOlcuFormuBeden2.olcuBedeni9.Contains("Müdahale"))
            //                {
            //                    var BedenArray = itemOlcuFormuBeden2.olcuBedeni9.Split('-');
            //                    var bedenString = BedenArray[0].Trim();

            //                    if (newBedenlerList.Any(x => x.beden == bedenString))
            //                    {
            //                        ObjNumuneAra9.bedenID = newBedenlerList.Where(x => x.beden == bedenString).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + bedenString + " bulunamadı");
            //                        ObjNumuneAra9.bedenID = 1;
            //                    }
            //                }
            //                else
            //                {

            //                    if (newBedenlerList.Any(x => x.beden == itemOlcuFormuBeden2.olcuBedeni9))
            //                    {
            //                        ObjNumuneAra9.bedenID = newBedenlerList.Where(x => x.beden == itemOlcuFormuBeden2.olcuBedeni9).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + itemOlcuFormuBeden2.olcuBedeni9 + " bulunamadı");
            //                        ObjNumuneAra9.bedenID = 1;
            //                    }
            //                }


            //                ObjNumuneAra9.imalat_Kalip = itemOlcuFormuBeden2.imlt_Kalip9;
            //                ObjNumuneAra9.imalat_Tol = itemOlcuFormuBeden2.imlt_Tol9;
            //                ObjNumuneAra9.imalat_YO = itemOlcuFormuBeden2.imlt_YO9;
            //                ObjNumuneAra9.imalat_Cekme = itemOlcuFormuBeden2.imlt_Cekme9;
            //                ObjNumuneAra9.kalipOlcusu = itemOlcuFormuBeden2.kalipOlcusu9;
            //                ObjNumuneAra9.kalipFark = itemOlcuFormuBeden2.kalipFark9;
            //                ObjNumuneAra9.yikOncesi = itemOlcuFormuBeden2.yikOncesi9;
            //                ObjNumuneAra9.yikCekme = itemOlcuFormuBeden2.yikCekme9;
            //                ObjNumuneAra9.yikSonrasi = itemOlcuFormuBeden2.yikSonrasi9;
            //                ObjNumuneAra9.yikamaFark = itemOlcuFormuBeden2.yikamaFark9;
            //                ObjNumuneAra9.ysonTablo = itemOlcuFormuBeden2.ysonTablo9;


            //                ObjNumuneAra9.numuneDetayID = addObjNumuneDetay.id;

            //                ObjNumuneAra9.ortCekme = itemOlcuFormuBeden2.ortCekme;
            //                ObjNumuneAra9.ortTolerans = itemOlcuFormuBeden2.ortTolerans;


            //                dbOlcum.NumuneAra.Add(ObjNumuneAra9);
            //                dbOlcum.SaveChanges();
            //            }

            //            if (itemOlcuFormuBeden2.olcuNoktasi10 != null)
            //            {
            //                NumuneAra ObjNumuneAra10 = new NumuneAra();


            //                //Olcu Noktalari
            //                if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi10))
            //                {
            //                    ObjNumuneAra10.olcuNoktaID = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden2.olcuNoktasi10).FirstOrDefault().id;
            //                }
            //                else
            //                {
            //                    using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden2.olcuNoktasi10, itemOlcuFormuBeden2.ofBedenID2, addObjNumuneDetay.id, 2))
            //                    {
            //                        if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                        {
            //                            ObjNumuneAra10.olcuNoktaID = form2.seilenOlcuNok;
            //                        }
            //                    }
            //                }


            //                //Bedenler
            //                if (itemOlcuFormuBeden2.olcuBedeni10.Contains("Müdahale"))
            //                {
            //                    var BedenArray = itemOlcuFormuBeden2.olcuBedeni10.Split('-');
            //                    var bedenString = BedenArray[0].Trim();

            //                    if (newBedenlerList.Any(x => x.beden == bedenString))
            //                    {
            //                        ObjNumuneAra10.bedenID = newBedenlerList.Where(x => x.beden == bedenString).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + bedenString + " bulunamadı");
            //                        ObjNumuneAra10.bedenID = 1;
            //                    }
            //                }
            //                else
            //                {

            //                    if (newBedenlerList.Any(x => x.beden == itemOlcuFormuBeden2.olcuBedeni10))
            //                    {
            //                        ObjNumuneAra10.bedenID = newBedenlerList.Where(x => x.beden == itemOlcuFormuBeden2.olcuBedeni10).FirstOrDefault().ID;
            //                    }
            //                    else
            //                    {
            //                        //MessageBox.Show("OlcuFormuBeden2 Id=" + itemOlcuFormuBeden2.ofBedenID2.ToString() + "  Aranan Beden=" + itemOlcuFormuBeden2.olcuBedeni10 + " bulunamadı");
            //                        ObjNumuneAra10.bedenID = 1;
            //                    }
            //                }


            //                ObjNumuneAra10.imalat_Kalip = itemOlcuFormuBeden2.imlt_Kalip10;
            //                ObjNumuneAra10.imalat_Tol = itemOlcuFormuBeden2.imlt_Tol10;
            //                ObjNumuneAra10.imalat_YO = itemOlcuFormuBeden2.imlt_YO10;
            //                ObjNumuneAra10.imalat_Cekme = itemOlcuFormuBeden2.imlt_Cekme10;
            //                ObjNumuneAra10.kalipOlcusu = itemOlcuFormuBeden2.kalipOlcusu10;
            //                ObjNumuneAra10.kalipFark = itemOlcuFormuBeden2.kalipFark10;
            //                ObjNumuneAra10.yikOncesi = itemOlcuFormuBeden2.yikOncesi10;
            //                ObjNumuneAra10.yikCekme = itemOlcuFormuBeden2.yikCekme10;
            //                ObjNumuneAra10.yikSonrasi = itemOlcuFormuBeden2.yikSonrasi10;
            //                ObjNumuneAra10.yikamaFark = itemOlcuFormuBeden2.yikamaFark10;
            //                ObjNumuneAra10.ysonTablo = itemOlcuFormuBeden2.ysonTablo10;


            //                ObjNumuneAra10.numuneDetayID = addObjNumuneDetay.id;

            //                ObjNumuneAra10.ortCekme = itemOlcuFormuBeden2.ortCekme;
            //                ObjNumuneAra10.ortTolerans = itemOlcuFormuBeden2.ortTolerans;


            //                dbOlcum.NumuneAra.Add(ObjNumuneAra10);
            //                dbOlcum.SaveChanges();
            //            }

            //        }


            //    }
            //    else
            //    {
            //        //MessageBox.Show("OlcuForm2 Id=" + item.oftabloID2.ToString() + " Aranan Order=" + item.orderNo + "/n Order bulunamadığı için kayıt edilmeden geçilrdi");
            //    }
            //}

            ////MessageBox.Show("Kayıt Başarılı");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<OlcuFormu> ListOlcuForum = dbOlcumRaporlama.OlcuFormu.Where(x =>  x.oftabloID == 5197).ToList();
            List<OlcuFormuBeden> listOFBeden = dbOlcumRaporlama.OlcuFormuBeden.Where(x => x.oftabloID == 5197).ToList();

            List<dbOlcumTest.Order> newOrderList = dbOlcum.Order.ToList();
            List<OlcuTurleri> newOlcuTurleriList = dbOlcum.OlcuTurleri.ToList();
            List<GiysiTurleri> newGiysiTurleriList = dbOlcum.GiysiTurleri.ToList();
            List<Atolyes> newAtolyeList = dbAtolye.Atolyes.ToList();

            List<OlcuNoktalari> newOlcuNokList = dbOlcum.OlcuNoktalari.ToList();
            List<Bedenler> newBedenlerList = dbOlcum.Bedenler.ToList();

            List<KazanHesaplama> newKazanHesaplamaList = dbOlcum.KazanHesaplama.ToList();

            foreach (var item in ListOlcuForum)
            {
                KazanDetay addKazanDetay = new KazanDetay();
      

                if (newOrderList.Any(x => x.orderNo == item.orderNo.ToUpper()))
                {
                    addKazanDetay.orderID = newOrderList.Where(x => x.orderNo == item.orderNo.ToUpper()).FirstOrDefault().ID;
                }
                else
                {
                    //MessageBox.Show("OlcuFormu Id=" + item.oftabloID.ToString() + "  aranan Order=" + item.orderNo + "  bulunamadı.");
                    addKazanDetay.orderID = 1;
                }

                if (item.dikimAtolyesi != null && item.dikimAtolyesi != "")
                {
                    if (newAtolyeList.Any(x => x.AtolyeAd == item.dikimAtolyesi))
                    {
                        addKazanDetay.atolyeID = newAtolyeList.Where(x => x.AtolyeAd == item.dikimAtolyesi).FirstOrDefault().Id;
                    }
                    else
                    {
                        addKazanDetay.atolyeID = 1;

                    }
                }
                else
                {
                    addKazanDetay.atolyeID = 66;// Atölye Belirtilmemiş
                }


                addKazanDetay.yikamaSorumlu = item.yikamaSorumlu;
                addKazanDetay.yikamaYeri = item.yikamaYeri;
                addKazanDetay.tarih = (DateTime)item.tarih;

                if (newOlcuTurleriList.Any(x => x.olcuTuruAd == item.olcuTuru))
                {
                    addKazanDetay.olcuTuruID = newOlcuTurleriList.Where(x => x.olcuTuruAd == item.olcuTuru).FirstOrDefault().id;
                }
                else
                {
                    //MessageBox.Show("OlcuFormu Id=" + item.oftabloID.ToString() + "  aranan ÖlçüTürü=" + item.olcuTuru + "  bulunamadı. Olcuturu=Proto, OlcuTurId = 1 olarak ayarlandı");
                    addKazanDetay.olcuTuruID = 1;
                }



                if (newGiysiTurleriList.Any(x => x.giysiTuruAd == item.giysiTuru))
                {
                    addKazanDetay.giysiTuruID = newGiysiTurleriList.Where(x => x.giysiTuruAd == item.giysiTuru).FirstOrDefault().id;
                }
                else
                {
                    //MessageBox.Show("OlcuFormu Id=" + item.oftabloID.ToString() + "  aranan GiysiTür=" + item.giysiTuru + "  bulunamadı. Giysitur=Pantolon, GiysiturId = 1 olarak ayarlandı");
                    addKazanDetay.giysiTuruID = 1;
                }

                addKazanDetay.tabloTuru = (int)item.oftabloTuru;

                addKazanDetay.aciklama = item.aciklama ?? "";

                addKazanDetay.olcumNo = item.olcuTuruNo;

                addKazanDetay.aparatliMi = item.aparat == "Aparatlı";
                addKazanDetay.utuPaketID = 0;
                addKazanDetay.durum = true;
                addKazanDetay.olcuTabloOlcuTurId = 0;
                addKazanDetay.kaliteInspectionId = 0;
                addKazanDetay.kaliteKontrolId = 0;


                //Kullanıcı Id
                addKazanDetay.kullaniciID = 19;//Nurşen Karadeniz


                dbOlcum.KazanDetay.Add(addKazanDetay);
                dbOlcum.SaveChanges();


                foreach (var itemOlcuFormuBeden in listOFBeden.Where(x=>x.oftabloID == item.oftabloID))
                {

                    #region 
                    Boolean hesaplamaKaydet = true;
                    int GenelOlcuNokId = 0;


                    int pantNo = 0;
                    int BedenIdForOlcuBedeni1 = 0;

                    if (itemOlcuFormuBeden.olcuBedeni1 != null)
                    {
                        ///Beden
                        string boy = "";

                        string olcuBeden1Trim = itemOlcuFormuBeden.olcuBedeni1.Trim();
                        if (newBedenlerList.Any(x => x.beden == olcuBeden1Trim))
                        {
                            BedenIdForOlcuBedeni1 = newBedenlerList.Where(x => x.beden == olcuBeden1Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden1Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden1Trim.Split('/');
                                var BedenString = BedenArray[0];
                                BedenIdForOlcuBedeni1 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy = BedenArray[1];
                            }
                            else if (olcuBeden1Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden1Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni1 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden1Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni1 = 1;
                            }
                        }


                        //ÖlçüNoktasi

                        int olcuNokId = 0;

                        if (boy != "")
                        {
                            hesaplamaKaydet = false;

                            if (itemOlcuFormuBeden.olcuNoktasi == "İÇ BOY" || itemOlcuFormuBeden.olcuNoktasi == "YAN BOY")
                            {
                                olcuNokId = newOlcuNokList.Where(x => x.olcuNoktasi.Contains(itemOlcuFormuBeden.olcuNoktasi) && x.olcuNoktasi.Contains(boy)).FirstOrDefault().id;
                            }
                            else
                            {
                                if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden.olcuNoktasi.Replace("\\0", "")))
                                {
                                    olcuNokId = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden.olcuNoktasi.Replace("\\0", "")).FirstOrDefault().id;
                                }
                                else
                                {
                                    olcuNokId = 1;
                                }
                            }

                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == olcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = olcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }


                        }
                        else
                        {
                            if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden.olcuNoktasi))
                            {
                                olcuNokId = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden.olcuNoktasi).FirstOrDefault().id;
                            }
                            else
                            {
                                using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden.olcuNoktasi, itemOlcuFormuBeden.ofBedenID, addKazanDetay.id, 3))
                                {
                                    if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        olcuNokId = form2.seilenOlcuNok;
                                    }
                                }
                            }


                            GenelOlcuNokId = olcuNokId;
                        }



                        if (itemOlcuFormuBeden.olcum11 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra11 = new KazanAra();
                            objKazanAra11.olcuNoktaID = olcuNokId;
                            objKazanAra11.kazanDetayID = addKazanDetay.id;
                            objKazanAra11.bedenID = BedenIdForOlcuBedeni1;
                            objKazanAra11.deger = itemOlcuFormuBeden.olcum11;
                            objKazanAra11.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra11);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum12 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra12 = new KazanAra();
                            objKazanAra12.olcuNoktaID = olcuNokId;
                            objKazanAra12.kazanDetayID = addKazanDetay.id;
                            objKazanAra12.bedenID = BedenIdForOlcuBedeni1;
                            objKazanAra12.deger = itemOlcuFormuBeden.olcum12;
                            objKazanAra12.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra12);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum13 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra13 = new KazanAra();
                            objKazanAra13.olcuNoktaID = olcuNokId;
                            objKazanAra13.kazanDetayID = addKazanDetay.id;
                            objKazanAra13.bedenID = BedenIdForOlcuBedeni1;
                            objKazanAra13.deger = itemOlcuFormuBeden.olcum13;
                            objKazanAra13.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra13);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum14 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra14 = new KazanAra();
                            objKazanAra14.olcuNoktaID = olcuNokId;
                            objKazanAra14.kazanDetayID = addKazanDetay.id;
                            objKazanAra14.bedenID = BedenIdForOlcuBedeni1;
                            objKazanAra14.deger = itemOlcuFormuBeden.olcum14;
                            objKazanAra14.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra14);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum10 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra10 = new KazanAra();
                            objKazanAra10.olcuNoktaID = olcuNokId;
                            objKazanAra10.kazanDetayID = addKazanDetay.id;
                            objKazanAra10.bedenID = BedenIdForOlcuBedeni1;
                            objKazanAra10.deger = itemOlcuFormuBeden.olcum10;
                            objKazanAra10.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra10);
                            dbOlcum.SaveChanges();
                        }
                    }

                    if (itemOlcuFormuBeden.olcuBedeni2 != null)
                    {

                        ///Beden
                        int BedenIdForOlcuBedeni2 = 0;
                        string boy2 = "";
                        string olcuBeden2Trim = itemOlcuFormuBeden.olcuBedeni2.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden2Trim))
                        {
                            BedenIdForOlcuBedeni2 = newBedenlerList.Where(x => x.beden == olcuBeden2Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden2Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden2Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni2 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy2 = BedenArray[1];
                            }
                            else if (olcuBeden2Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden2Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni2 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy2 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden2Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni2 = 1;
                            }
                        }


                        //ÖlçüNoktasi

                        if (boy2 != "")
                        {
                            hesaplamaKaydet = false;

                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }
                        }
                        else
                        {

                        }


                        if (itemOlcuFormuBeden.olcum21 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra21 = new KazanAra();
                            objKazanAra21.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra21.kazanDetayID = addKazanDetay.id;
                            objKazanAra21.bedenID = BedenIdForOlcuBedeni2;
                            objKazanAra21.deger = itemOlcuFormuBeden.olcum21;
                            objKazanAra21.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra21);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum22 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra22 = new KazanAra();
                            objKazanAra22.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra22.kazanDetayID = addKazanDetay.id;
                            objKazanAra22.bedenID = BedenIdForOlcuBedeni2;
                            objKazanAra22.deger = itemOlcuFormuBeden.olcum22;
                            objKazanAra22.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra22);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum23 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra23 = new KazanAra();
                            objKazanAra23.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra23.kazanDetayID = addKazanDetay.id;
                            objKazanAra23.bedenID = BedenIdForOlcuBedeni2;
                            objKazanAra23.deger = itemOlcuFormuBeden.olcum23;
                            objKazanAra23.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra23);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum24 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra24 = new KazanAra();
                            objKazanAra24.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra24.kazanDetayID = addKazanDetay.id;
                            objKazanAra24.bedenID = BedenIdForOlcuBedeni2;
                            objKazanAra24.deger = itemOlcuFormuBeden.olcum24;
                            objKazanAra24.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra24);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum20 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra20 = new KazanAra();
                            objKazanAra20.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra20.kazanDetayID = addKazanDetay.id;
                            objKazanAra20.bedenID = BedenIdForOlcuBedeni2;
                            objKazanAra20.deger = itemOlcuFormuBeden.olcum20;
                            objKazanAra20.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra20);
                            dbOlcum.SaveChanges();
                        }


                    }

                    if (itemOlcuFormuBeden.olcuBedeni3 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni3 = 0;
                        string boy3 = "";
                        string olcuBeden3Trim = itemOlcuFormuBeden.olcuBedeni3.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden3Trim))
                        {
                            BedenIdForOlcuBedeni3 = newBedenlerList.Where(x => x.beden == olcuBeden3Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden3Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden3Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni3 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy3 = BedenArray[1];
                            }
                            else if (olcuBeden3Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden3Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni3 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy3 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden3Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni3 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy3 != "")
                        {
                            hesaplamaKaydet = false;


                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }



                        }
                        else
                        {
 
                        }

                        if (itemOlcuFormuBeden.olcum31 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra31 = new KazanAra();
                            objKazanAra31.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra31.kazanDetayID = addKazanDetay.id;
                            objKazanAra31.bedenID = BedenIdForOlcuBedeni3;
                            objKazanAra31.deger = itemOlcuFormuBeden.olcum31;
                            objKazanAra31.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra31);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum32 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra32 = new KazanAra();
                            objKazanAra32.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra32.kazanDetayID = addKazanDetay.id;
                            objKazanAra32.bedenID = BedenIdForOlcuBedeni3;
                            objKazanAra32.deger = itemOlcuFormuBeden.olcum32;
                            objKazanAra32.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra32);
                            dbOlcum.SaveChanges();
                        }


                        if (itemOlcuFormuBeden.olcum33 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra33 = new KazanAra();
                            objKazanAra33.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra33.kazanDetayID = addKazanDetay.id;
                            objKazanAra33.bedenID = BedenIdForOlcuBedeni3;
                            objKazanAra33.deger = itemOlcuFormuBeden.olcum33;
                            objKazanAra33.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra33);
                            dbOlcum.SaveChanges();
                        }


                        if (itemOlcuFormuBeden.olcum34 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra34 = new KazanAra();
                            objKazanAra34.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra34.kazanDetayID = addKazanDetay.id;
                            objKazanAra34.bedenID = BedenIdForOlcuBedeni3;
                            objKazanAra34.deger = itemOlcuFormuBeden.olcum34;
                            objKazanAra34.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra34);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum30 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra30 = new KazanAra();
                            objKazanAra30.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra30.kazanDetayID = addKazanDetay.id;
                            objKazanAra30.bedenID = BedenIdForOlcuBedeni3;
                            objKazanAra30.deger = itemOlcuFormuBeden.olcum30;
                            objKazanAra30.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra30);
                            dbOlcum.SaveChanges();
                        }


                    }

                    if (itemOlcuFormuBeden.olcuBedeni4 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni4 = 0;
                        string boy4 = "";
                        string olcuBeden4Trim = itemOlcuFormuBeden.olcuBedeni4.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden4Trim))
                        {
                            BedenIdForOlcuBedeni4 = newBedenlerList.Where(x => x.beden == olcuBeden4Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden4Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden4Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni4 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy4 = BedenArray[1];
                            }
                            else if (olcuBeden4Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden4Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni4 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy4 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden4Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni4 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy4 != "")
                        {
                            hesaplamaKaydet = false;


                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }

                        }
                        else
                        {

                        }

                        if (itemOlcuFormuBeden.olcum41 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra41 = new KazanAra();
                            objKazanAra41.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra41.kazanDetayID = addKazanDetay.id;
                            objKazanAra41.bedenID = BedenIdForOlcuBedeni4;
                            objKazanAra41.deger = itemOlcuFormuBeden.olcum41;
                            objKazanAra41.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra41);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum42 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra42 = new KazanAra();
                            objKazanAra42.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra42.kazanDetayID = addKazanDetay.id;
                            objKazanAra42.bedenID = BedenIdForOlcuBedeni4;
                            objKazanAra42.deger = itemOlcuFormuBeden.olcum42;
                            objKazanAra42.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra42);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum43 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra43 = new KazanAra();
                            objKazanAra43.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra43.kazanDetayID = addKazanDetay.id;
                            objKazanAra43.bedenID = BedenIdForOlcuBedeni4;
                            objKazanAra43.deger = itemOlcuFormuBeden.olcum43;
                            objKazanAra43.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra43);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum44 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra44 = new KazanAra();
                            objKazanAra44.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra44.kazanDetayID = addKazanDetay.id;
                            objKazanAra44.bedenID = BedenIdForOlcuBedeni4;
                            objKazanAra44.deger = itemOlcuFormuBeden.olcum44;
                            objKazanAra44.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra44);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum40 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra40 = new KazanAra();
                            objKazanAra40.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra40.kazanDetayID = addKazanDetay.id;
                            objKazanAra40.bedenID = BedenIdForOlcuBedeni4;
                            objKazanAra40.deger = itemOlcuFormuBeden.olcum40;
                            objKazanAra40.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra40);
                            dbOlcum.SaveChanges();
                        }

                    }

                    if (itemOlcuFormuBeden.olcuBedeni5 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni5 = 0;
                        string boy5 = "";
                        string olcuBeden5Trim = itemOlcuFormuBeden.olcuBedeni5.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden5Trim))
                        {
                            BedenIdForOlcuBedeni5 = newBedenlerList.Where(x => x.beden == olcuBeden5Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden5Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden5Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni5 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy5 = BedenArray[1];
                            }
                            else if (olcuBeden5Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden5Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni5 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy5 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden5Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni5 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy5 != "")
                        {
                            hesaplamaKaydet = false;

                           

                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }


                        }
                        else
                        {

                        }

                        if (itemOlcuFormuBeden.olcum51 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra51 = new KazanAra();
                            objKazanAra51.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra51.kazanDetayID = addKazanDetay.id;
                            objKazanAra51.bedenID = BedenIdForOlcuBedeni5;
                            objKazanAra51.deger = itemOlcuFormuBeden.olcum51;
                            objKazanAra51.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra51);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum52 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra52 = new KazanAra();
                            objKazanAra52.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra52.kazanDetayID = addKazanDetay.id;
                            objKazanAra52.bedenID = BedenIdForOlcuBedeni5;
                            objKazanAra52.deger = itemOlcuFormuBeden.olcum52;
                            objKazanAra52.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra52);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum53 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra53 = new KazanAra();
                            objKazanAra53.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra53.kazanDetayID = addKazanDetay.id;
                            objKazanAra53.bedenID = BedenIdForOlcuBedeni5;
                            objKazanAra53.deger = itemOlcuFormuBeden.olcum53;
                            objKazanAra53.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra53);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum54 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra54 = new KazanAra();
                            objKazanAra54.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra54.kazanDetayID = addKazanDetay.id;
                            objKazanAra54.bedenID = BedenIdForOlcuBedeni5;
                            objKazanAra54.deger = itemOlcuFormuBeden.olcum54;
                            objKazanAra54.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra54);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum50 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra50 = new KazanAra();
                            objKazanAra50.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra50.kazanDetayID = addKazanDetay.id;
                            objKazanAra50.bedenID = BedenIdForOlcuBedeni5;
                            objKazanAra50.deger = itemOlcuFormuBeden.olcum50;
                            objKazanAra50.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra50);
                            dbOlcum.SaveChanges();
                        }





                    }

                    if (itemOlcuFormuBeden.olcuBedeni6 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni6 = 0;
                        string boy6 = "";
                        string olcuBeden6Trim = itemOlcuFormuBeden.olcuBedeni6.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden6Trim))
                        {
                            BedenIdForOlcuBedeni6 = newBedenlerList.Where(x => x.beden == olcuBeden6Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden6Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden6Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni6 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy6 = BedenArray[1];
                            }
                            else if (olcuBeden6Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden6Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni6 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy6 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden6Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni6 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy6 != "")
                        {
                            hesaplamaKaydet = false;


                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }

                        }
                        else
                        {

                        }

                        if (itemOlcuFormuBeden.olcum61 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra61 = new KazanAra();
                            objKazanAra61.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra61.kazanDetayID = addKazanDetay.id;
                            objKazanAra61.bedenID = BedenIdForOlcuBedeni6;
                            objKazanAra61.deger = itemOlcuFormuBeden.olcum61;
                            objKazanAra61.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra61);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum62 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra62 = new KazanAra();
                            objKazanAra62.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra62.kazanDetayID = addKazanDetay.id;
                            objKazanAra62.bedenID = BedenIdForOlcuBedeni6;
                            objKazanAra62.deger = itemOlcuFormuBeden.olcum62;
                            objKazanAra62.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra62);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum63 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra63 = new KazanAra();
                            objKazanAra63.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra63.kazanDetayID = addKazanDetay.id;
                            objKazanAra63.bedenID = BedenIdForOlcuBedeni6;
                            objKazanAra63.deger = itemOlcuFormuBeden.olcum63;
                            objKazanAra63.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra63);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum64 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra64 = new KazanAra();
                            objKazanAra64.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra64.kazanDetayID = addKazanDetay.id;
                            objKazanAra64.bedenID = BedenIdForOlcuBedeni6;
                            objKazanAra64.deger = itemOlcuFormuBeden.olcum64;
                            objKazanAra64.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra64);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum60 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra60 = new KazanAra();
                            objKazanAra60.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra60.kazanDetayID = addKazanDetay.id;
                            objKazanAra60.bedenID = BedenIdForOlcuBedeni6;
                            objKazanAra60.deger = itemOlcuFormuBeden.olcum60;
                            objKazanAra60.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra60);
                            dbOlcum.SaveChanges();
                        }

                    }

                    if (itemOlcuFormuBeden.olcuBedeni7 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni7 = 0;
                        string boy7 = "";
                        string olcuBeden7Trim = itemOlcuFormuBeden.olcuBedeni7.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden7Trim))
                        {
                            BedenIdForOlcuBedeni7 = newBedenlerList.Where(x => x.beden == olcuBeden7Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden7Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden7Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni7 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy7 = BedenArray[1];
                            }
                            else if (olcuBeden7Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden7Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni7 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy7 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden7Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni7 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy7 != "")
                        {
                            hesaplamaKaydet = false;


                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }

                        }
                        else
                        {

                        }

                        if (itemOlcuFormuBeden.olcum71 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra71 = new KazanAra();
                            objKazanAra71.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra71.kazanDetayID = addKazanDetay.id;
                            objKazanAra71.bedenID = BedenIdForOlcuBedeni7;
                            objKazanAra71.deger = itemOlcuFormuBeden.olcum71;
                            objKazanAra71.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra71);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum72 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra72 = new KazanAra();
                            objKazanAra72.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra72.kazanDetayID = addKazanDetay.id;
                            objKazanAra72.bedenID = BedenIdForOlcuBedeni7;
                            objKazanAra72.deger = itemOlcuFormuBeden.olcum72;
                            objKazanAra72.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra72);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum73 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra73 = new KazanAra();
                            objKazanAra73.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra73.kazanDetayID = addKazanDetay.id;
                            objKazanAra73.bedenID = BedenIdForOlcuBedeni7;
                            objKazanAra73.deger = itemOlcuFormuBeden.olcum73;
                            objKazanAra73.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra73);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum74 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra74 = new KazanAra();
                            objKazanAra74.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra74.kazanDetayID = addKazanDetay.id;
                            objKazanAra74.bedenID = BedenIdForOlcuBedeni7;
                            objKazanAra74.deger = itemOlcuFormuBeden.olcum74;
                            objKazanAra74.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra74);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum70 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra70 = new KazanAra();
                            objKazanAra70.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra70.kazanDetayID = addKazanDetay.id;
                            objKazanAra70.bedenID = BedenIdForOlcuBedeni7;
                            objKazanAra70.deger = itemOlcuFormuBeden.olcum70;
                            objKazanAra70.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra70);
                            dbOlcum.SaveChanges();
                        }





                    }

                    if (itemOlcuFormuBeden.olcuBedeni8 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni8 = 0;
                        string boy8 = "";
                        string olcuBeden8Trim = itemOlcuFormuBeden.olcuBedeni8.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden8Trim))
                        {
                            BedenIdForOlcuBedeni8 = newBedenlerList.Where(x => x.beden == olcuBeden8Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden8Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden8Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni8 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy8 = BedenArray[1];
                            }
                            else if (olcuBeden8Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden8Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni8 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy8 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden8Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni8 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy8 != "")
                        {
                            hesaplamaKaydet = false;



                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }

                        }
                        else
                        {

                        }

                        if (itemOlcuFormuBeden.olcum81 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra81 = new KazanAra();
                            objKazanAra81.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra81.kazanDetayID = addKazanDetay.id;
                            objKazanAra81.bedenID = BedenIdForOlcuBedeni8;
                            objKazanAra81.deger = itemOlcuFormuBeden.olcum81;
                            objKazanAra81.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra81);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum82 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra82 = new KazanAra();
                            objKazanAra82.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra82.kazanDetayID = addKazanDetay.id;
                            objKazanAra82.bedenID = BedenIdForOlcuBedeni8;
                            objKazanAra82.deger = itemOlcuFormuBeden.olcum82;
                            objKazanAra82.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra82);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum83 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra83 = new KazanAra();
                            objKazanAra83.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra83.kazanDetayID = addKazanDetay.id;
                            objKazanAra83.bedenID = BedenIdForOlcuBedeni8;
                            objKazanAra83.deger = itemOlcuFormuBeden.olcum83;
                            objKazanAra83.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra83);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum84 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra84 = new KazanAra();
                            objKazanAra84.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra84.kazanDetayID = addKazanDetay.id;
                            objKazanAra84.bedenID = BedenIdForOlcuBedeni8;
                            objKazanAra84.deger = itemOlcuFormuBeden.olcum84;
                            objKazanAra84.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra84);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum80 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra80 = new KazanAra();
                            objKazanAra80.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra80.kazanDetayID = addKazanDetay.id;
                            objKazanAra80.bedenID = BedenIdForOlcuBedeni8;
                            objKazanAra80.deger = itemOlcuFormuBeden.olcum80;
                            objKazanAra80.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra80);
                            dbOlcum.SaveChanges();
                        }

                    }

                    if (itemOlcuFormuBeden.olcuBedeni9 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni9 = 0;
                        string boy9 = "";
                        string olcuBeden9Trim = itemOlcuFormuBeden.olcuBedeni9.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden9Trim))
                        {
                            BedenIdForOlcuBedeni9 = newBedenlerList.Where(x => x.beden == olcuBeden9Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden9Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden9Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni9 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy9 = BedenArray[1];
                            }
                            else if (olcuBeden9Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden9Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni9 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy9 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden9Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni9 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy9 != "")
                        {
                            hesaplamaKaydet = false;




                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }


                        }
                        else
                        {

                        }

                        if (itemOlcuFormuBeden.olcum91 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra91 = new KazanAra();
                            objKazanAra91.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra91.kazanDetayID = addKazanDetay.id;
                            objKazanAra91.bedenID = BedenIdForOlcuBedeni9;
                            objKazanAra91.deger = itemOlcuFormuBeden.olcum91;
                            objKazanAra91.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra91);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum92 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra92 = new KazanAra();
                            objKazanAra92.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra92.kazanDetayID = addKazanDetay.id;
                            objKazanAra92.bedenID = BedenIdForOlcuBedeni9;
                            objKazanAra92.deger = itemOlcuFormuBeden.olcum92;
                            objKazanAra92.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra92);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum93 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra93 = new KazanAra();
                            objKazanAra93.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra93.kazanDetayID = addKazanDetay.id;
                            objKazanAra93.bedenID = BedenIdForOlcuBedeni9;
                            objKazanAra93.deger = itemOlcuFormuBeden.olcum93;
                            objKazanAra93.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra93);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum94 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra94 = new KazanAra();
                            objKazanAra94.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra94.kazanDetayID = addKazanDetay.id;
                            objKazanAra94.bedenID = BedenIdForOlcuBedeni9;
                            objKazanAra94.deger = itemOlcuFormuBeden.olcum94;
                            objKazanAra94.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra94);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum90 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra90 = new KazanAra();
                            objKazanAra90.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra90.kazanDetayID = addKazanDetay.id;
                            objKazanAra90.bedenID = BedenIdForOlcuBedeni9;
                            objKazanAra90.deger = itemOlcuFormuBeden.olcum90;
                            objKazanAra90.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra90);
                            dbOlcum.SaveChanges();
                        }





                    }

                    if (itemOlcuFormuBeden.olcuBedeni10 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni10 = 0;
                        string boy10 = "";
                        string olcuBeden10Trim = itemOlcuFormuBeden.olcuBedeni10.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden10Trim))
                        {
                            BedenIdForOlcuBedeni10 = newBedenlerList.Where(x => x.beden == olcuBeden10Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden10Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden10Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni10 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy10 = BedenArray[1];
                            }
                            else if (olcuBeden10Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden10Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni10 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy10 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden10Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni10 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy10 != "")
                        {
                            hesaplamaKaydet = false;


                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }

                        }
                        else
                        {

                        }

                        if (itemOlcuFormuBeden.olcum101 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra101 = new KazanAra();
                            objKazanAra101.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra101.kazanDetayID = addKazanDetay.id;
                            objKazanAra101.bedenID = BedenIdForOlcuBedeni10;
                            objKazanAra101.deger = itemOlcuFormuBeden.olcum101;
                            objKazanAra101.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra101);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum102 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra102 = new KazanAra();
                            objKazanAra102.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra102.kazanDetayID = addKazanDetay.id;
                            objKazanAra102.bedenID = BedenIdForOlcuBedeni10;
                            objKazanAra102.deger = itemOlcuFormuBeden.olcum102;
                            objKazanAra102.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra102);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum103 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra103 = new KazanAra();
                            objKazanAra103.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra103.kazanDetayID = addKazanDetay.id;
                            objKazanAra103.bedenID = BedenIdForOlcuBedeni10;
                            objKazanAra103.deger = itemOlcuFormuBeden.olcum103;
                            objKazanAra103.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra103);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum104 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra104 = new KazanAra();
                            objKazanAra104.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra104.kazanDetayID = addKazanDetay.id;
                            objKazanAra104.bedenID = BedenIdForOlcuBedeni10;
                            objKazanAra104.deger = itemOlcuFormuBeden.olcum104;
                            objKazanAra104.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra104);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum100 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra100 = new KazanAra();
                            objKazanAra100.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra100.kazanDetayID = addKazanDetay.id;
                            objKazanAra100.bedenID = BedenIdForOlcuBedeni10;
                            objKazanAra100.deger = itemOlcuFormuBeden.olcum100;
                            objKazanAra100.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra100);
                            dbOlcum.SaveChanges();
                        }


                    }

                    if (itemOlcuFormuBeden.olcuBedeni11 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni11 = 0;
                        string boy11 = "";
                        string olcuBeden11Trim = itemOlcuFormuBeden.olcuBedeni11.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden11Trim))
                        {
                            BedenIdForOlcuBedeni11 = newBedenlerList.Where(x => x.beden == olcuBeden11Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden11Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden11Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni11 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy11 = BedenArray[1];
                            }
                            else if (olcuBeden11Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden11Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni11 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy11 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden11Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni11 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy11 != "")
                        {
                            hesaplamaKaydet = false;


                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }

                        }
                        else
                        {
 
                        }

                        if (itemOlcuFormuBeden.olcum111 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra111 = new KazanAra();
                            objKazanAra111.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra111.kazanDetayID = addKazanDetay.id;
                            objKazanAra111.bedenID = BedenIdForOlcuBedeni11;
                            objKazanAra111.deger = itemOlcuFormuBeden.olcum111;
                            objKazanAra111.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra111);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum112 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra112 = new KazanAra();
                            objKazanAra112.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra112.kazanDetayID = addKazanDetay.id;
                            objKazanAra112.bedenID = BedenIdForOlcuBedeni11;
                            objKazanAra112.deger = itemOlcuFormuBeden.olcum112;
                            objKazanAra112.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra112);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum113 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra113 = new KazanAra();
                            objKazanAra113.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra113.kazanDetayID = addKazanDetay.id;
                            objKazanAra113.bedenID = BedenIdForOlcuBedeni11;
                            objKazanAra113.deger = itemOlcuFormuBeden.olcum113;
                            objKazanAra113.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra113);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum114 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra114 = new KazanAra();
                            objKazanAra114.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra114.kazanDetayID = addKazanDetay.id;
                            objKazanAra114.bedenID = BedenIdForOlcuBedeni11;
                            objKazanAra114.deger = itemOlcuFormuBeden.olcum114;
                            objKazanAra114.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra114);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum110 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra110 = new KazanAra();
                            objKazanAra110.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra110.kazanDetayID = addKazanDetay.id;
                            objKazanAra110.bedenID = BedenIdForOlcuBedeni11;
                            objKazanAra110.deger = itemOlcuFormuBeden.olcum110;
                            objKazanAra110.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra110);
                            dbOlcum.SaveChanges();
                        }

                    }

                    if (itemOlcuFormuBeden.olcuBedeni12 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni12 = 0;
                        string boy12 = "";
                        string olcuBeden12Trim = itemOlcuFormuBeden.olcuBedeni12.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden12Trim))
                        {
                            BedenIdForOlcuBedeni12 = newBedenlerList.Where(x => x.beden == olcuBeden12Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden12Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden12Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni12 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy12 = BedenArray[1];
                            }
                            else if (olcuBeden12Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden12Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni12 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy12 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden12Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni12 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy12 != "")
                        {
                            hesaplamaKaydet = false;

                           

                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }

                        }
                        else
                        {
     
                        }

                        if (itemOlcuFormuBeden.olcum121 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra121 = new KazanAra();
                            objKazanAra121.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra121.kazanDetayID = addKazanDetay.id;
                            objKazanAra121.bedenID = BedenIdForOlcuBedeni12;
                            objKazanAra121.deger = itemOlcuFormuBeden.olcum121;
                            objKazanAra121.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra121);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum122 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra122 = new KazanAra();
                            objKazanAra122.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra122.kazanDetayID = addKazanDetay.id;
                            objKazanAra122.bedenID = BedenIdForOlcuBedeni12;
                            objKazanAra122.deger = itemOlcuFormuBeden.olcum122;
                            objKazanAra122.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra122);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum123 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra123 = new KazanAra();
                            objKazanAra123.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra123.kazanDetayID = addKazanDetay.id;
                            objKazanAra123.bedenID = BedenIdForOlcuBedeni12;
                            objKazanAra123.deger = itemOlcuFormuBeden.olcum123;
                            objKazanAra123.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra123);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum124 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra124 = new KazanAra();
                            objKazanAra124.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra124.kazanDetayID = addKazanDetay.id;
                            objKazanAra124.bedenID = BedenIdForOlcuBedeni12;
                            objKazanAra124.deger = itemOlcuFormuBeden.olcum124;
                            objKazanAra124.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra124);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum120 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra120 = new KazanAra();
                            objKazanAra120.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra120.kazanDetayID = addKazanDetay.id;
                            objKazanAra120.bedenID = BedenIdForOlcuBedeni12;
                            objKazanAra120.deger = itemOlcuFormuBeden.olcum120;
                            objKazanAra120.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra120);
                            dbOlcum.SaveChanges();
                        }





                    }

                    if (itemOlcuFormuBeden.olcuBedeni13 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni13 = 0;
                        string boy13 = "";
                        string olcuBeden13Trim = itemOlcuFormuBeden.olcuBedeni13.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden13Trim))
                        {
                            BedenIdForOlcuBedeni13 = newBedenlerList.Where(x => x.beden == olcuBeden13Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden13Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden13Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni13 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy13 = BedenArray[1];
                            }
                            else if (olcuBeden13Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden13Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni13 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy13 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden13Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni13 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy13 != "")
                        {
                            hesaplamaKaydet = false;

                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }

                        }
                        else
                        {

                        }

                        if (itemOlcuFormuBeden.olcum131 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra131 = new KazanAra();
                            objKazanAra131.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra131.kazanDetayID = addKazanDetay.id;
                            objKazanAra131.bedenID = BedenIdForOlcuBedeni13;
                            objKazanAra131.deger = itemOlcuFormuBeden.olcum131;
                            objKazanAra131.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra131);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum132 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra132 = new KazanAra();
                            objKazanAra132.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra132.kazanDetayID = addKazanDetay.id;
                            objKazanAra132.bedenID = BedenIdForOlcuBedeni13;
                            objKazanAra132.deger = itemOlcuFormuBeden.olcum132;
                            objKazanAra132.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra132);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum133 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra133 = new KazanAra();
                            objKazanAra133.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra133.kazanDetayID = addKazanDetay.id;
                            objKazanAra133.bedenID = BedenIdForOlcuBedeni13;
                            objKazanAra133.deger = itemOlcuFormuBeden.olcum133;
                            objKazanAra133.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra133);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum134 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra134 = new KazanAra();
                            objKazanAra134.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra134.kazanDetayID = addKazanDetay.id;
                            objKazanAra134.bedenID = BedenIdForOlcuBedeni13;
                            objKazanAra134.deger = itemOlcuFormuBeden.olcum134;
                            objKazanAra134.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra134);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum130 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra130 = new KazanAra();
                            objKazanAra130.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra130.kazanDetayID = addKazanDetay.id;
                            objKazanAra130.bedenID = BedenIdForOlcuBedeni13;
                            objKazanAra130.deger = itemOlcuFormuBeden.olcum130;
                            objKazanAra130.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra130);
                            dbOlcum.SaveChanges();
                        }





                    }

                    if (itemOlcuFormuBeden.olcuBedeni14 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni14 = 0;
                        string boy14 = "";
                        string olcuBeden14Trim = itemOlcuFormuBeden.olcuBedeni14.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden14Trim))
                        {
                            BedenIdForOlcuBedeni14 = newBedenlerList.Where(x => x.beden == olcuBeden14Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden14Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden14Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni14 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy14 = BedenArray[1];
                            }
                            else if (olcuBeden14Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden14Trim.Split('-');
                                var BedenString = BedenArray[0];


                                BedenIdForOlcuBedeni14 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy14 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden14Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni14 = 1;
                            }
                        }

                        //ÖlçüNoktasi
             

                        if (boy14 != "")
                        {
                            hesaplamaKaydet = false;


                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }

                        }
                        else
                        {
                        
                        }

                        if (itemOlcuFormuBeden.olcum141 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra141 = new KazanAra();
                            objKazanAra141.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra141.kazanDetayID = addKazanDetay.id;
                            objKazanAra141.bedenID = BedenIdForOlcuBedeni14;
                            objKazanAra141.deger = itemOlcuFormuBeden.olcum141;
                            objKazanAra141.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra141);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum142 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra142 = new KazanAra();
                            objKazanAra142.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra142.kazanDetayID = addKazanDetay.id;
                            objKazanAra142.bedenID = BedenIdForOlcuBedeni14;
                            objKazanAra142.deger = itemOlcuFormuBeden.olcum142;
                            objKazanAra142.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra142);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum143 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra143 = new KazanAra();
                            objKazanAra143.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra143.kazanDetayID = addKazanDetay.id;
                            objKazanAra143.bedenID = BedenIdForOlcuBedeni14;
                            objKazanAra143.deger = itemOlcuFormuBeden.olcum143;
                            objKazanAra143.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra143);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum144 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra144 = new KazanAra();
                            objKazanAra144.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra144.kazanDetayID = addKazanDetay.id;
                            objKazanAra144.bedenID = BedenIdForOlcuBedeni14;
                            objKazanAra144.deger = itemOlcuFormuBeden.olcum144;
                            objKazanAra144.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra144);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum140 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra140 = new KazanAra();
                            objKazanAra140.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra140.kazanDetayID = addKazanDetay.id;
                            objKazanAra140.bedenID = BedenIdForOlcuBedeni14;
                            objKazanAra140.deger = itemOlcuFormuBeden.olcum140;
                            objKazanAra140.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra140);
                            dbOlcum.SaveChanges();
                        }





                    }

                    if (itemOlcuFormuBeden.olcuBedeni15 != null)
                    {
                        ///Beden
                        int BedenIdForOlcuBedeni15 = 0;
                        string boy15 = "";
                        string olcuBeden15Trim = itemOlcuFormuBeden.olcuBedeni15.Trim();

                        if (newBedenlerList.Any(x => x.beden == olcuBeden15Trim))
                        {
                            BedenIdForOlcuBedeni15 = newBedenlerList.Where(x => x.beden == olcuBeden15Trim).FirstOrDefault().ID;
                        }
                        else
                        {
                            string[] BedenArray;

                            if (olcuBeden15Trim.Contains("/"))
                            {
                                BedenArray = olcuBeden15Trim.Split('/');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni15 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy15 = BedenArray[1];
                            }
                            else if (olcuBeden15Trim.Contains("-"))
                            {

                                BedenArray = olcuBeden15Trim.Split('-');
                                var BedenString = BedenArray[0];

                                BedenIdForOlcuBedeni15 = newBedenlerList.Where(x => x.beden == BedenString).FirstOrDefault().ID;
                                boy15 = BedenArray[1];
                            }
                            else
                            {
                                //MessageBox.Show("olcuFormuBeden Id=" + itemOlcuFormuBeden.ofBedenID.ToString() + "  Bulunamayan Beden=" + olcuBeden15Trim + " bulunamadı BedenId=1 ayarlandı");
                                BedenIdForOlcuBedeni15 = 1;
                            }
                        }

                        //ÖlçüNoktasi

                        if (boy15 != "")
                        {
                            hesaplamaKaydet = false;


                            if (!newKazanHesaplamaList.Any(x => x.kazanDetayID == addKazanDetay.id && x.olcuNoktasiID == GenelOlcuNokId))
                            {
                                KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                                addKazanHesaplama.asilTablo = 0;
                                addKazanHesaplama.gerceklesenTolCekme = 0;
                                addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                                addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                                addKazanHesaplama.oncekiTablo = 0;
                                addKazanHesaplama.ortalamaDeger = 0;
                                addKazanHesaplama.uygulananTolCekme = 0;
                                addKazanHesaplama.yoOlculen = 0;

                                dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                                dbOlcum.SaveChanges();
                            }

                        }
                        else
                        {

                        }

                        if (itemOlcuFormuBeden.olcum151 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra151 = new KazanAra();
                            objKazanAra151.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra151.kazanDetayID = addKazanDetay.id;
                            objKazanAra151.bedenID = BedenIdForOlcuBedeni15;
                            objKazanAra151.deger = itemOlcuFormuBeden.olcum151;
                            objKazanAra151.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra151);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum152 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra152 = new KazanAra();
                            objKazanAra152.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra152.kazanDetayID = addKazanDetay.id;
                            objKazanAra152.bedenID = BedenIdForOlcuBedeni15;
                            objKazanAra152.deger = itemOlcuFormuBeden.olcum152;
                            objKazanAra152.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra152);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum153 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra153 = new KazanAra();
                            objKazanAra153.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra153.kazanDetayID = addKazanDetay.id;
                            objKazanAra153.bedenID = BedenIdForOlcuBedeni15;
                            objKazanAra153.deger = itemOlcuFormuBeden.olcum153;
                            objKazanAra153.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra153);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum154 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra154 = new KazanAra();
                            objKazanAra154.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra154.kazanDetayID = addKazanDetay.id;
                            objKazanAra154.bedenID = BedenIdForOlcuBedeni15;
                            objKazanAra154.deger = itemOlcuFormuBeden.olcum154;
                            objKazanAra154.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra154);
                            dbOlcum.SaveChanges();
                        }

                        if (itemOlcuFormuBeden.olcum150 != null)
                        {
                            pantNo++;

                            KazanAra objKazanAra150 = new KazanAra();
                            objKazanAra150.olcuNoktaID = GenelOlcuNokId;
                            objKazanAra150.kazanDetayID = addKazanDetay.id;
                            objKazanAra150.bedenID = BedenIdForOlcuBedeni15;
                            objKazanAra150.deger = itemOlcuFormuBeden.olcum150;
                            objKazanAra150.pantNo = pantNo;

                            dbOlcum.KazanAra.Add(objKazanAra150);
                            dbOlcum.SaveChanges();
                        }

                    }


                    #endregion

                    if (hesaplamaKaydet)
                    {
                        KazanHesaplama addKazanHesaplama = new KazanHesaplama();
                        addKazanHesaplama.asilTablo = itemOlcuFormuBeden.asılTablo ?? 0;
                        addKazanHesaplama.gerceklesenTolCekme = itemOlcuFormuBeden.gerceklesenTolCekme ?? 0;
                        addKazanHesaplama.kazanDetayID = addKazanDetay.id;
                        addKazanHesaplama.olcuNoktasiID = GenelOlcuNokId;
                        addKazanHesaplama.oncekiTablo = itemOlcuFormuBeden.oncekiTablo ?? 0;
                        addKazanHesaplama.ortalamaDeger = itemOlcuFormuBeden.ortalamaDeger ?? 0;
                        addKazanHesaplama.uygulananTolCekme = itemOlcuFormuBeden.uygulananTolCekme ?? 0;
                        addKazanHesaplama.yoOlculen = itemOlcuFormuBeden.yoOlculen ?? 0;

                        dbOlcum.KazanHesaplama.Add(addKazanHesaplama);
                        dbOlcum.SaveChanges();
                    }

                }

            }

            //MessageBox.Show("Kayıt başarılı");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //List<OlcuFormu3> ListOlcuFormu3 = dbOlcumRaporlama.OlcuFormu3.Where(s => s.oftabloID3 > 22).ToList();

            //List<dbOlcumTest.Order> newOrderList = dbOlcum.Order.ToList();
            //List<OlcuTurleri> newOlcuTurleriList = dbOlcum.OlcuTurleri.ToList();
            //List<GiysiTurleri> newGiysiTurleriList = dbOlcum.GiysiTurleri.ToList();
            //List<Atolyes> newAtolyeList = dbAtolye.Atolyes.ToList();

            //List<OlcuNoktalari> newOlcuNokList = dbOlcum.OlcuNoktalari.ToList();
            //List<Bedenler> newBedenlerList = dbOlcum.Bedenler.ToList();
            //List<PersonelTablo> newPersonelList = dbOlcum.PersonelTablo.ToList();

            //foreach (var item in ListOlcuFormu3)
            //{
            //    CogaltmaDetay addCogaltma = new CogaltmaDetay();


            //    //Order
            //    if (newOrderList.Any(x => x.orderNo == item.orderNo))
            //    {
            //        addCogaltma.orderID = newOrderList.Where(x => x.orderNo == item.orderNo).FirstOrDefault().ID;
            //    }
            //    else
            //    {
            //        //MessageBox.Show("OlcuFormu3 Id=" + item.oftabloID3.ToString() + "  aranan Order=" + item.orderNo + "  bulunamadı.OrderId 1 ayarlandı");
            //        addCogaltma.orderID = 1;
            //    }


            //    //Atolye
            //    if (item.dikimAtolyesi != null && item.dikimAtolyesi != "")
            //    {
            //        if (newAtolyeList.Any(x => x.AtolyeAd == item.dikimAtolyesi))
            //        {
            //            addCogaltma.atolyeID = newAtolyeList.Where(x => x.AtolyeAd == item.dikimAtolyesi).FirstOrDefault().Id;
            //        }
            //        else
            //        {
            //            using (AtolyeDialog form3 = new AtolyeDialog(item.oftabloID3, item.dikimAtolyesi, 3))
            //            {
            //                if (form3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                {
            //                    addCogaltma.atolyeID = form3.seilenAtolyeId;
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        addCogaltma.atolyeID = 66;// Atölye Belirtilmemiş
            //    }


            //    //GiysiTuru
            //    if (newGiysiTurleriList.Any(x => x.giysiTuruAd == item.giysiTuru))
            //    {
            //        addCogaltma.giysiTuruID = newGiysiTurleriList.Where(x => x.giysiTuruAd == item.giysiTuru).FirstOrDefault().id;
            //    }
            //    else
            //    {
            //        //MessageBox.Show("OlcuFormu3 Id=" + item.oftabloID3.ToString() + "  aranan GiysiTür=" + item.giysiTuru + "  bulunamadı. Giysitur=Pantolon, GiysiturId = 1 olarak ayarlandı");
            //        addCogaltma.giysiTuruID = 1;
            //    }



            //    //OlcuTuru
            //    if (newOlcuTurleriList.Any(x => x.olcuTuruAd == item.olcuTuru))
            //    {
            //        addCogaltma.olcuTuruID = newOlcuTurleriList.Where(x => x.olcuTuruAd == item.olcuTuru).FirstOrDefault().id;
            //    }
            //    else
            //    {
            //        //MessageBox.Show("OlcuFormu3 Id=" + item.oftabloID3.ToString() + "  aranan ÖlçüTürü=" + item.olcuTuru + "  bulunamadı. Olcuturu=Proto, OlcuTurId = 1 olarak ayarlandı");
            //        addCogaltma.olcuTuruID = 1;
            //    }


            //    addCogaltma.kesimeGidis = item.kesimeVeris;
            //    addCogaltma.dikimeGidis = item.dikimeVeris;
            //    addCogaltma.yikamadanGidis = item.yikamadanGelis;
            //    addCogaltma.yikamayaGidis = item.yikamayaGidis;
            //    addCogaltma.kalipAdi = item.enBoyCekme;
            //    addCogaltma.topNo = item.topNo;
            //    addCogaltma.olcumNo = item.olcuTuruNo;
            //    addCogaltma.aciklama = item.aciklama;
            //    addCogaltma.tarih = item.tarih;
            //    addCogaltma.receteKod = item.receteKod;

            //    //KullanıcıId
            //    if (item.modelistAd != null && item.modelistAd != "")
            //    {
            //        var modelistTrim = item.modelistAd.Trim();
            //        List<string> ModelistArray;
            //        var arananModelisy = "";

            //        if (modelistTrim.Contains("/"))
            //        {
            //            ModelistArray = modelistTrim.Split('/').ToList();
            //            arananModelisy = ModelistArray[ModelistArray.Count() - 1].Trim();
            //        }
            //        else if (modelistTrim.Contains("-"))
            //        {
            //            ModelistArray = modelistTrim.Split('-').ToList();
            //            arananModelisy = ModelistArray[ModelistArray.Count() - 1].Trim();
            //        }
            //        else
            //        {
            //            arananModelisy = modelistTrim;
            //        }




            //        if (newPersonelList.Any(x => x.personelAd == arananModelisy))
            //        {
            //            if (arananModelisy == "Elif")
            //            {
            //                addCogaltma.kullaniciID = 3;// Elif Şahin

            //            }
            //            else if (arananModelisy == "Nevin")
            //            {
            //                addCogaltma.kullaniciID = 10;// Nevin Deveci
            //            }
            //            else
            //            {
            //                addCogaltma.kullaniciID = newPersonelList.Where(x => x.personelAd == arananModelisy).FirstOrDefault().id;
            //            }


            //        }
            //        else
            //        {
            //            using (UserDialog form2 = new UserDialog(arananModelisy, item.oftabloID3, 3))
            //            {
            //                if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                {
            //                    addCogaltma.kullaniciID = form2.seilenUserId;
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        addCogaltma.kullaniciID = 18;//Nagihan Polat
            //    }



            //    try
            //    {
            //        dbOlcum.CogaltmaDetay.Add(addCogaltma);
            //        dbOlcum.SaveChanges();
            //    }
            //    catch (DbEntityValidationException s)
            //    {

            //        foreach (var eve in s.EntityValidationErrors)
            //        {
            //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //            foreach (var ve in eve.ValidationErrors)
            //            {
            //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                    ve.PropertyName, ve.ErrorMessage);
            //            }
            //        }
            //        throw;
            //    }


            //    List<OlcuFormuBeden3> listBeden3 = dbOlcumRaporlama.OlcuFormuBeden3.Where(x => x.oftabloID3 == item.oftabloID3).ToList();

            //    foreach (var itemOlcuFormuBeden3 in listBeden3)
            //    {

            //        if (itemOlcuFormuBeden3.olcuNoktasi != null)
            //        {
            //            int OlcuNokId1 = 0;

            //            if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden3.olcuNoktasi))
            //            {
            //                OlcuNokId1 = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden3.olcuNoktasi).FirstOrDefault().id;
            //            }
            //            else
            //            {
            //                using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden3.olcuNoktasi, itemOlcuFormuBeden3.ofBedenID3, addCogaltma.id, 4))
            //                {
            //                    if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                    {
            //                        OlcuNokId1 = form2.seilenOlcuNok;
            //                    }
            //                }
            //            }

            //            var beden1 = "";
            //            var bedenId1 = 0;


            //            if (itemOlcuFormuBeden3.olcuBedeni1.Contains("-"))
            //            {
            //                beden1 = itemOlcuFormuBeden3.olcuBedeni1.Split('-')[0];
            //            }
            //            else
            //            {
            //                beden1 = itemOlcuFormuBeden3.olcuBedeni1;
            //            }

            //            if (newBedenlerList.Any(x => x.beden == beden1))
            //            {
            //                bedenId1 = newBedenlerList.Where(x => x.beden == beden1).FirstOrDefault().ID;
            //            }
            //            else
            //            {
            //                //MessageBox.Show("OlcuFormuBeden3 Id=" + itemOlcuFormuBeden3.ofBedenID3.ToString() + "  Aranadan beden=" + itemOlcuFormuBeden3.olcuBedeni1 + " bulunamadı");
            //                bedenId1 = 1;
            //            }


            //            CogaltmaAra addCogaltmaAra1 = new CogaltmaAra();
            //            addCogaltmaAra1.bedenID = bedenId1;
            //            addCogaltmaAra1.cekme = itemOlcuFormuBeden3.cekme1;
            //            addCogaltmaAra1.cogaltmaDetayID = addCogaltma.id;
            //            addCogaltmaAra1.kalipOlcusu = itemOlcuFormuBeden3.kalipOlcu1;
            //            addCogaltmaAra1.olcuNoktaID = OlcuNokId1;
            //            addCogaltmaAra1.tolerans = itemOlcuFormuBeden3.tolerans1;
            //            addCogaltmaAra1.yoFark = itemOlcuFormuBeden3.yikOncesi1;
            //            addCogaltmaAra1.yoOlcu = itemOlcuFormuBeden3.yoOlcu1;
            //            addCogaltmaAra1.ysFark = itemOlcuFormuBeden3.ysFark1;
            //            addCogaltmaAra1.ysOlcu = itemOlcuFormuBeden3.ysOlcu1;
            //            addCogaltmaAra1.ysOlculen = itemOlcuFormuBeden3.ysOlculen1;


            //            dbOlcum.CogaltmaAra.Add(addCogaltmaAra1);
            //            dbOlcum.SaveChanges();

            //        }

            //        if (itemOlcuFormuBeden3.olcuNoktasi2 != null)
            //        {
            //            int OlcuNokId2 = 0;

            //            if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden3.olcuNoktasi2))
            //            {
            //                OlcuNokId2 = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden3.olcuNoktasi2).FirstOrDefault().id;
            //            }
            //            else
            //            {
            //                using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden3.olcuNoktasi2, itemOlcuFormuBeden3.ofBedenID3, addCogaltma.id, 4))
            //                {
            //                    if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                    {
            //                        OlcuNokId2 = form2.seilenOlcuNok;
            //                    }
            //                }
            //            }

            //            var beden2 = "";
            //            var bedenId2 = 0;


            //            if (itemOlcuFormuBeden3.olcuBedeni2.Contains("-"))
            //            {
            //                beden2 = itemOlcuFormuBeden3.olcuBedeni2.Split('-')[0];
            //            }
            //            else
            //            {
            //                beden2 = itemOlcuFormuBeden3.olcuBedeni2;
            //            }

            //            if (newBedenlerList.Any(x => x.beden == beden2))
            //            {
            //                bedenId2 = newBedenlerList.Where(x => x.beden == beden2).FirstOrDefault().ID;
            //            }
            //            else
            //            {
            //                //MessageBox.Show("OlcuFormuBeden3 Id=" + itemOlcuFormuBeden3.ofBedenID3.ToString() + "  Aranadan beden=" + itemOlcuFormuBeden3.olcuBedeni2 + " bulunamadı");
            //                bedenId2 = 1;
            //            }


            //            CogaltmaAra addCogaltmaAra2 = new CogaltmaAra();
            //            addCogaltmaAra2.bedenID = bedenId2;
            //            addCogaltmaAra2.cekme = itemOlcuFormuBeden3.cekme2;
            //            addCogaltmaAra2.cogaltmaDetayID = addCogaltma.id;
            //            addCogaltmaAra2.kalipOlcusu = itemOlcuFormuBeden3.kalipOlcu2;
            //            addCogaltmaAra2.olcuNoktaID = OlcuNokId2;
            //            addCogaltmaAra2.tolerans = itemOlcuFormuBeden3.tolerans2;
            //            addCogaltmaAra2.yoFark = itemOlcuFormuBeden3.yikOncesi2;
            //            addCogaltmaAra2.yoOlcu = itemOlcuFormuBeden3.yoOlcu2;
            //            addCogaltmaAra2.ysFark = itemOlcuFormuBeden3.ysFark2;
            //            addCogaltmaAra2.ysOlcu = itemOlcuFormuBeden3.ysOlcu2;
            //            addCogaltmaAra2.ysOlculen = itemOlcuFormuBeden3.ysOlculen2;


            //            dbOlcum.CogaltmaAra.Add(addCogaltmaAra2);
            //            dbOlcum.SaveChanges();

            //        }


            //        if (itemOlcuFormuBeden3.olcuNoktasi3 != null)
            //        {
            //            int OlcuNokId3 = 0;

            //            if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden3.olcuNoktasi3))
            //            {
            //                OlcuNokId3 = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden3.olcuNoktasi3).FirstOrDefault().id;
            //            }
            //            else
            //            {
            //                using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden3.olcuNoktasi3, itemOlcuFormuBeden3.ofBedenID3, addCogaltma.id, 4))
            //                {
            //                    if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                    {
            //                        OlcuNokId3 = form2.seilenOlcuNok;
            //                    }
            //                }
            //            }

            //            var beden3 = "";
            //            var bedenId3 = 0;


            //            if (itemOlcuFormuBeden3.olcuBedeni3.Contains("-"))
            //            {
            //                beden3 = itemOlcuFormuBeden3.olcuBedeni3.Split('-')[0];
            //            }
            //            else
            //            {
            //                beden3 = itemOlcuFormuBeden3.olcuBedeni3;
            //            }

            //            if (newBedenlerList.Any(x => x.beden == beden3))
            //            {
            //                bedenId3 = newBedenlerList.Where(x => x.beden == beden3).FirstOrDefault().ID;
            //            }
            //            else
            //            {
            //                //MessageBox.Show("OlcuFormuBeden3 Id=" + itemOlcuFormuBeden3.ofBedenID3.ToString() + "  Aranadan beden=" + itemOlcuFormuBeden3.olcuBedeni3 + " bulunamadı");
            //                bedenId3 = 1;
            //            }


            //            CogaltmaAra addCogaltmaAra3 = new CogaltmaAra();
            //            addCogaltmaAra3.bedenID = bedenId3;
            //            addCogaltmaAra3.cekme = itemOlcuFormuBeden3.cekme3;
            //            addCogaltmaAra3.cogaltmaDetayID = addCogaltma.id;
            //            addCogaltmaAra3.kalipOlcusu = itemOlcuFormuBeden3.kalipOlcu3;
            //            addCogaltmaAra3.olcuNoktaID = OlcuNokId3;
            //            addCogaltmaAra3.tolerans = itemOlcuFormuBeden3.tolerans3;
            //            addCogaltmaAra3.yoFark = itemOlcuFormuBeden3.yikOncesi3;
            //            addCogaltmaAra3.yoOlcu = itemOlcuFormuBeden3.yoOlcu3;
            //            addCogaltmaAra3.ysFark = itemOlcuFormuBeden3.ysFark3;
            //            addCogaltmaAra3.ysOlcu = itemOlcuFormuBeden3.ysOlcu3;
            //            addCogaltmaAra3.ysOlculen = itemOlcuFormuBeden3.ysOlculen3;


            //            dbOlcum.CogaltmaAra.Add(addCogaltmaAra3);
            //            dbOlcum.SaveChanges();

            //        }


            //        if (itemOlcuFormuBeden3.olcuNoktasi4 != null)
            //        {
            //            int OlcuNokId4 = 0;

            //            if (newOlcuNokList.Any(x => x.olcuNoktasi == itemOlcuFormuBeden3.olcuNoktasi4))
            //            {
            //                OlcuNokId4 = newOlcuNokList.Where(x => x.olcuNoktasi == itemOlcuFormuBeden3.olcuNoktasi4).FirstOrDefault().id;
            //            }
            //            else
            //            {
            //                using (OlcuNokDialog form2 = new OlcuNokDialog(itemOlcuFormuBeden3.olcuNoktasi4, itemOlcuFormuBeden3.ofBedenID3, addCogaltma.id, 4))
            //                {
            //                    if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                    {
            //                        OlcuNokId4 = form2.seilenOlcuNok;
            //                    }
            //                }
            //            }

            //            var beden4 = "";
            //            var bedenId4 = 0;


            //            if (itemOlcuFormuBeden3.olcuBedeni4.Contains("-"))
            //            {
            //                beden4 = itemOlcuFormuBeden3.olcuBedeni4.Split('-')[0];
            //            }
            //            else
            //            {
            //                beden4 = itemOlcuFormuBeden3.olcuBedeni4;
            //            }

            //            if (newBedenlerList.Any(x => x.beden == beden4))
            //            {
            //                bedenId4 = newBedenlerList.Where(x => x.beden == beden4).FirstOrDefault().ID;
            //            }
            //            else
            //            {
            //                //MessageBox.Show("OlcuFormuBeden3 Id=" + itemOlcuFormuBeden3.ofBedenID3.ToString() + "  Aranadan beden=" + itemOlcuFormuBeden3.olcuBedeni4 + " bulunamadı");
            //                bedenId4 = 1;
            //            }


            //            CogaltmaAra addCogaltmaAra4 = new CogaltmaAra();
            //            addCogaltmaAra4.bedenID = bedenId4;
            //            addCogaltmaAra4.cekme = itemOlcuFormuBeden3.cekme4;
            //            addCogaltmaAra4.cogaltmaDetayID = addCogaltma.id;
            //            addCogaltmaAra4.kalipOlcusu = itemOlcuFormuBeden3.kalipOlcu4;
            //            addCogaltmaAra4.olcuNoktaID = OlcuNokId4;
            //            addCogaltmaAra4.tolerans = itemOlcuFormuBeden3.tolerans4;
            //            addCogaltmaAra4.yoFark = itemOlcuFormuBeden3.yikOncesi4;
            //            addCogaltmaAra4.yoOlcu = itemOlcuFormuBeden3.yoOlcu4;
            //            addCogaltmaAra4.ysFark = itemOlcuFormuBeden3.ysFark4;
            //            addCogaltmaAra4.ysOlcu = itemOlcuFormuBeden3.ysOlcu4;
            //            addCogaltmaAra4.ysOlculen = itemOlcuFormuBeden3.ysOlculen4;


            //            dbOlcum.CogaltmaAra.Add(addCogaltmaAra4);
            //            dbOlcum.SaveChanges();

            //        }




            //    }
            //}

            ////MessageBox.Show("Kayıt Başarılı");

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                var conString = ConfigurationManager.ConnectionStrings["RaporlamaContext"]?.ConnectionString;
                SqlConnection conn = new SqlConnection(conString);
                conn.Open();
                using (conn)
                {
                    SqlCommand cmd = new SqlCommand();
                    string identitycmdOn = "SET IDENTITY_INSERT OlcumRaporlamaTest.dbo.OlcuTablosu ON ";

                    string OlcuTablosuInsert = "INSERT INTO OlcumRaporlamaTest.dbo.OlcuTablosu(ottabloID,orderNo,orderArtikel,giysiTuru,modelistAd,enBoyCekme,tarih,anaBeden,ottabloTuru,olcuTuru,karsitNo,aciklama,olcuTuruNo,satirSayisi) " +
                        "select ottabloID,orderNo,orderArtikel,giysiTuru,modelistAd,enBoyCekme,tarih,anaBeden,ottabloTuru,olcuTuru,karsitNo,aciklama,olcuTuruNo,satirSayisi from OlcumRaporlama.dbo.OlcuTablosu where  ottabloID = 55088 or ottabloID = 55089 ";

                    string identitycmdOff = "SET IDENTITY_INSERT OlcumRaporlamaTest.dbo.OlcuTablosu OFF ";

                    string identitycmdOn2 = "SET IDENTITY_INSERT OlcumRaporlamaTest.dbo.OlcuTablosuBeden ON ";

                    string olcuTablosuBedenInsert = "INSERT INTO OlcumRaporlamaTest.dbo.OlcuTablosuBeden(otBedenID,ottabloID,olcumNoktasi,olcuBedeni,olcu1,olcuBedeni2,olcu2,olcuBedeni3,olcu3,olcuBedeni4,olcu4,olcuBedeni5,olcu5,olcuBedeni6,olcu6," +
    "olcuBedeni7,olcu7,olcuBedeni8,olcu8,olcuBedeni9,olcu9,olcuBedeni10,olcu10,olcuBedeni11,olcu11,olcuBedeni12,olcu12,olcuBedeni13,olcu13,olcuBedeni14,olcu14,olcuBedeni15,olcu15,tolerans,cekme,oran,satirIndexi) " +
    "SELECT otBedenID,ottabloID,olcumNoktasi,olcuBedeni,olcu1,olcuBedeni2,olcu2,olcuBedeni3,olcu3,olcuBedeni4,olcu4,olcuBedeni5,olcu5,olcuBedeni6,olcu6," +
    "olcuBedeni7,olcu7,olcuBedeni8,olcu8,olcuBedeni9,olcu9,olcuBedeni10,olcu10,olcuBedeni11,olcu11,olcuBedeni12,olcu12,olcuBedeni13,olcu13,olcuBedeni14,olcu14,olcuBedeni15,olcu15,tolerans,cekme,oran,satirIndexi FROM" +
    " OlcumRaporlama.dbo.OlcuTablosuBeden where ottabloID = 55088 or ottabloID = 55089 ";

                    string identitycmdOff2 = "SET IDENTITY_INSERT OlcumRaporlamaTest.dbo.OlcuTablosuBeden OFF ";


                    cmd.Connection = conn;

                    cmd.CommandText = identitycmdOn;
                    cmd.CommandText += OlcuTablosuInsert;
                    cmd.CommandText += identitycmdOff;
                    cmd.CommandText = identitycmdOn2;
                    cmd.CommandText += olcuTablosuBedenInsert;
                    cmd.CommandText += identitycmdOff2;


                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("İşlem başarılı");

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            List<dbOlcumTest.Order> newOrderList = dbOlcum.Order.ToList();
            List<OlcuTurleri> newOlcuTurleriList = dbOlcum.OlcuTurleri.ToList();
            List<GiysiTurleri> newGiysiTurleriList = dbOlcum.GiysiTurleri.ToList();
            List<Atolyes> newAtolyeList = dbAtolye.Atolyes.ToList();

            List<KazanDetay> kazanDetayList = dbOlcum.KazanDetay.Where(x => x.orderID == 1 && x.id != 1942).ToList();

            foreach (var item in kazanDetayList)
            {
                int atolyeID = item.atolyeID;

                string atolyeAd = newAtolyeList.Where(x => x.Id == atolyeID).Select(x => x.AtolyeAd).FirstOrDefault();

                if (atolyeAd == "Atölye Belirtilmemiş")
                {
                    atolyeAd = "";
                }

                int olcuturuID = item.olcuTuruID;
                string olcuTuruAd = newOlcuTurleriList.Where(x => x.id == olcuturuID).Select(x => x.olcuTuruAd).FirstOrDefault();

                int? giysiTuruID = item.giysiTuruID;
                string giysiTuruAd = newGiysiTurleriList.Where(x => x.id == giysiTuruID).Select(x => x.giysiTuruAd).FirstOrDefault();

                DateTime? tarih = item.tarih;



                string aciklama = item.aciklama;

                bool aparat = item.aparatliMi;

                string aparatliMi = "";

                if (aparat)
                {
                    aparatliMi = "Aparatlı";
                }
                else
                {
                    aparatliMi = "Aparatsız";
                }

                int tabloTuru = item.tabloTuru;

                List<OlcuFormu> orderNo = dbOlcumRaporlama.OlcuFormu.Where(x => x.aciklama == aciklama && x.tarih == tarih && x.aparat == aparatliMi && x.olcuTuru == olcuTuruAd && x.giysiTuru == giysiTuruAd && x.dikimAtolyesi == atolyeAd && x.oftabloTuru == tabloTuru)
                    .ToList();

                if (orderNo.Count > 1)
                {
                    int kazanDetayID = item.id;
                    List<KazanAra> kazanAras = dbOlcum.KazanAra.Where(x => x.kazanDetayID == kazanDetayID).Take(10).ToList();
                    if (kazanAras.Count > 0)
                    {
                        foreach (var item2 in orderNo)
                        {
                            int oftabloID = item2.oftabloID;
                            OlcuFormuBeden olcuFormuBedens = dbOlcumRaporlama.OlcuFormuBeden.Where(x => x.oftabloID == oftabloID).FirstOrDefault();

                            if (olcuFormuBedens != null)
                            {
                                if (kazanAras[0].deger == olcuFormuBedens.olcum11)
                                {
                                    string alinanOrder = item2.orderNo.Trim();

                                    item.orderID = newOrderList.Where(x => x.orderNo == alinanOrder).Select(x => x.ID).FirstOrDefault();
                                    dbOlcum.SaveChanges();

                                    break;
                                }
                            }
                        }
                    }


                }
                else if (orderNo.Count == 1)
                {
                    string alinanOrder = orderNo[0].orderNo.Trim();

                    item.orderID = newOrderList.Where(x => x.orderNo == alinanOrder).Select(x => x.ID).FirstOrDefault();
                    dbOlcum.SaveChanges();
                }

            }

            MessageBox.Show("Güncelleme Başarılı");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                //List<OlcuNoktalari> olcuNoktalaris = dbOlcum.OlcuNoktalari.ToList();

                //List<OlcuTabloAra> olcuTabloAras = dbOlcum.OlcuTabloAra.Where(x => x.orijinalOlcuNok != null && x.hataOldu == true && x.olcuNoktaID == -1 &&  x.OlcuTabloDetay.tabloTuru != 2 && x.OlcuTabloDetay.orderID == 10367).ToList();

                //List<OlcuTabloHesaplama> olcuTabloHesaplamas = dbOlcum.OlcuTabloHesaplama.Where(x => x.olcuNoktaID == 1 && x.OlcuTabloDetay.tabloTuru != 2 && x.OlcuTabloDetay.orderID == 10205).ToList();

                //foreach (var item in olcuTabloAras)
                //{
                //    if (!String.IsNullOrEmpty(item.orijinalOlcuNok))
                //    {

                //        int olcutabloID = item.olcuTabloID;

                //        SqlParameter param1 = new SqlParameter("@olcuTabloID", olcutabloID);

                //        var cmd = dbOlcum.Database.Connection.CreateCommand();
                //        cmd.CommandText = "select top 1 COUNT(*) from OlcuTabloAra " +
                //        "where olcuTabloID = @olcuTabloID and olcuNoktaID != 1" +
                //        "group by olcuTabloID,olcuNoktaID";
                //        cmd.Parameters.Add(param1);

                //        dbOlcum.Database.Connection.Open();

                //        var reader = cmd.ExecuteReader();

                //        reader.Read();
                //        int bedenSayi = reader.GetInt32(0);
                //        reader.Close();

                //        string hataliOlcuNoktasi = item.orijinalOlcuNok.Trim();

                //        if (hataliOlcuNoktasi.Contains("\\0"))
                //        {
                //            hataliOlcuNoktasi.Replace("\\0", "");
                //        }

                //        int olcuNoktaID = dbOlcum.OlcuNoktalari.Where(x => x.olcuNoktasi == hataliOlcuNoktasi).Select(x => x.id).FirstOrDefault();

                //        if (olcuNoktaID == 0)
                //        {

                //        }
                //        else
                //        {
                //            int id = item.id;

                //            for (int i = 0; i < bedenSayi; i++)
                //            {
                //                OlcuTabloAra olcuTabloAra = dbOlcum.OlcuTabloAra.Where(x => x.id == id).FirstOrDefault();

                //                olcuTabloAra.olcuNoktaID = olcuNoktaID;
                //                olcuTabloAra.orijinalOlcuNok = null;

                //                dbOlcum.SaveChanges();

                //                id++;
                //            }
                //        }

                //        dbOlcum.Database.Connection.Close();

                //    }

                //}

                //foreach (var item2 in olcuTabloHesaplamas)
                //{
                    
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            List<dbOlcumTest.Order> newOrderList = dbOlcum.Order.ToList();
            List<OlcuTurleri> newOlcuTurleriList = dbOlcum.OlcuTurleri.ToList();
            List<GiysiTurleri> newGiysiTurleriList = dbOlcum.GiysiTurleri.ToList();

            List<OlcuTabloDetay> olcuDetayList = dbOlcum.OlcuTabloDetay.Where(x => x.orderID == 1).ToList();

            foreach (var item in olcuDetayList)
            {
                int oldOlcuTabloID = (int)item.oldOlcuTabloId;

                OlcuTablosu tablo = dbOlcumRaporlama.OlcuTablosu.Where(x => x.ottabloID == oldOlcuTabloID).FirstOrDefault();

                if (tablo != null)
                {
                    string orderNo = tablo.orderNo.ToUpper();

                    int orderID = dbOlcum.Order.Where(x => x.orderNo == orderNo).Select(x => x.ID).FirstOrDefault();

                    if (orderID != 0)
                    {
                        item.orderID = orderID;

                        dbOlcum.SaveChanges();
                    }
                }

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                List<KazanHesaplama> kazanHesaplamas = dbOlcum.KazanHesaplama.Where(x => x.KazanDetay.aktarimMi == null).ToList();

                List<Order> orders = dbOlcum.Order.ToList();

                List<OlcuTurleri> olcuTurleri = dbOlcum.OlcuTurleri.ToList();

                List<GiysiTurleri> giysiTurleris = dbOlcum.GiysiTurleri.ToList();

                List<OlcuNoktalari> olcuNoktalaris = dbOlcum.OlcuNoktalari.ToList();


                List<OlcuFormu> olcuFormus = dbOlcumRaporlama.OlcuFormu.ToList();

                List<OlcuFormuBeden> olcuFormuBedens = dbOlcumRaporlama.OlcuFormuBeden.ToList();

                foreach (var item in kazanHesaplamas)
                {
                    int orderID = item.KazanDetay.orderID;
                    string orderNo = orders.Where(x => x.ID == orderID).Select(x => x.orderNo).FirstOrDefault();
                    int olcuturuID = item.KazanDetay.olcuTuruID;
                    string olcuTuru = olcuTurleri.Where(x => x.id == olcuturuID).Select(x => x.olcuTuruAd).FirstOrDefault();
                    int? giysiTuruID = item.KazanDetay.giysiTuruID;
                    string giysiTuru = giysiTurleris.Where(x => x.id == giysiTuruID).Select(x => x.giysiTuruAd).FirstOrDefault();
                    int? olcumNo = item.KazanDetay.olcumNo;

                    string aciklama = item.KazanDetay.aciklama;

                    string aparat = "";

                    if (item.KazanDetay.aparatliMi == true)
                    {
                        aparat = "Aparatlı";
                    }
                    else
                    {
                        aparat = "Aparatsız";
                    }

                    int oftabloID = olcuFormus.Where(x => x.orderNo == orderNo && x.giysiTuru == giysiTuru && x.olcuTuru == olcuTuru && x.olcuTuruNo == olcumNo && x.aciklama == aciklama && x.aparat == aparat).Select(x => x.oftabloID).FirstOrDefault();

                    if (oftabloID != 0)
                    {
                        int olcuNoktaID = item.olcuNoktasiID;

                        string olcuNoktasi = olcuNoktalaris.Where(x => x.id == olcuNoktaID).Select(x => x.olcuNoktasi).FirstOrDefault();

                        if (!String.IsNullOrEmpty(olcuNoktasi))
                        {
                            OlcuFormuBeden olcuFormuBedens1 = olcuFormuBedens.Where(x => x.oftabloID == oftabloID && x.olcuNoktasi == olcuNoktasi).FirstOrDefault();

                            if (olcuFormuBedens1 != null)
                            {
                                double ortalamaDeger = olcuFormuBedens1.ortalamaDeger ?? 0;

                                double yoOlculen = olcuFormuBedens1.yoOlculen ?? 0;

                                double gerceklesenTolCekme = olcuFormuBedens1.gerceklesenTolCekme ?? 0;

                                double oncekiTablo = olcuFormuBedens1.oncekiTablo ?? 0;

                                double uygulananTolCekme = olcuFormuBedens1.uygulananTolCekme ?? 0;

                                double asilTablo = olcuFormuBedens1.asılTablo ?? 0;

                                item.ortalamaDeger = ortalamaDeger;
                                item.yoOlculen = yoOlculen;
                                item.gerceklesenTolCekme = gerceklesenTolCekme;
                                item.oncekiTablo = oncekiTablo;
                                item.uygulananTolCekme = uygulananTolCekme;
                                item.asilTablo = asilTablo;

                                dbOlcum.SaveChanges();
                            }


                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                List<KazanHesaplama> kazanHesaplamas = dbOlcum.KazanHesaplama.Where(x => x.kazanDetayID == 6685).ToList();

                foreach (var item in kazanHesaplamas)
                {
                    int tabloTuru = item.KazanDetay.tabloTuru;

                    int kazanDetayID = item.kazanDetayID;
                    int olcuNoktaID = item.olcuNoktasiID;
                    int orderID = item.KazanDetay.orderID;




                    if (tabloTuru == 0)
                    {

                        SqlParameter param1 = new SqlParameter("@KazanDetayID", kazanDetayID);
                        SqlParameter param2 = new SqlParameter("@olcuNoktaID", olcuNoktaID);

                        var cmd2 = dbOlcum.Database.Connection.CreateCommand();
                        cmd2.CommandText = "[dbo].[OrtalamaDegerBulmaYO] @KazanDetayID, @olcuNoktaID";
                        cmd2.Parameters.Add(param1);
                        cmd2.Parameters.Add(param2);

                        dbOlcum.Database.Connection.Open();

                        var reader2 = cmd2.ExecuteReader();

                        reader2.Read();
                        item.ortalamaDeger = Math.Round(reader2.GetDouble(0), 2, MidpointRounding.AwayFromZero);
                        reader2.Close();

                        dbOlcum.Database.Connection.Close();

                        SqlParameter param11 = new SqlParameter("@kazanDetayID", kazanDetayID);
                        SqlParameter param21 = new SqlParameter("@olcuNoktaID", olcuNoktaID);
                        SqlParameter param3 = new SqlParameter("@OrderID", orderID);


                        var cmd = dbOlcum.Database.Connection.CreateCommand();
                        cmd.CommandText = "exec [dbo].[YikamaOncesiKalip] @OrderID,@olcuNoktaID,@kazandetayID";
                        cmd.Parameters.Add(param3);
                        cmd.Parameters.Add(param21);
                        cmd.Parameters.Add(param11);


                        dbOlcum.Database.Connection.Open();

                        var reader = cmd.ExecuteReader();


                        double oncekiTablo = 0;
                        double uygulananTolCekme = 0;
                        double asilTablo = 0;

                        if (reader.HasRows)
                        {
                            reader.Read();

                            if (!Convert.IsDBNull(reader.GetValue(0)))
                            {
                                oncekiTablo = reader.GetDouble(0);
                            }


                            if (!Convert.IsDBNull(reader.GetValue(1)))
                            {
                                uygulananTolCekme = reader.GetDouble(1);
                            }


                            if (!Convert.IsDBNull(reader.GetValue(2)))
                            {
                                asilTablo = reader.GetDouble(2);
                            }
                            reader.Close();

                            item.oncekiTablo = oncekiTablo;
                            item.uygulananTolCekme = uygulananTolCekme;
                            item.asilTablo = asilTablo;

                            item.yoOlculen = Math.Round(item.ortalamaDeger + asilTablo, 2, MidpointRounding.AwayFromZero);

                            item.gerceklesenTolCekme = Math.Round(oncekiTablo - item.yoOlculen, 2, MidpointRounding.AwayFromZero);

                            dbOlcum.SaveChanges();

                        }

                        dbOlcum.Database.Connection.Close();

                    }
                    else
                    {
                        int? giysiTuruID = item.KazanDetay.giysiTuruID;


                        int olcuTuruID = item.KazanDetay.olcuTuruID;


                        SqlParameter param1 = new SqlParameter("@KazanDetayID", kazanDetayID);
                        SqlParameter param2 = new SqlParameter("@olcuNoktaID", olcuNoktaID);

                        var cmd2 = dbOlcum.Database.Connection.CreateCommand();
                        cmd2.CommandText = "[dbo].[OrtalamaDegerBulma] @KazanDetayID, @olcuNoktaID";
                        cmd2.Parameters.Add(param1);
                        cmd2.Parameters.Add(param2);

                        dbOlcum.Database.Connection.Open();

                        var reader2 = cmd2.ExecuteReader();

                        reader2.Read();
                        if (!reader2.IsDBNull(0))
                        {
                            item.ortalamaDeger = Math.Round(reader2.GetDouble(0), 2, MidpointRounding.AwayFromZero);

                        }
                        else
                        {
                            item.ortalamaDeger = 0;
                        }
                        reader2.Close();

                        dbOlcum.Database.Connection.Close();

                        if (Convert.ToInt32(item.KazanDetay.olcuTuruID) == 18 || Convert.ToInt32(item.KazanDetay.olcuTuruID) == 19)
                        {
                            olcuTuruID = 17;

                        }
                        else
                        {
                            olcuTuruID = item.KazanDetay.olcuTuruID;
                        }


                        int? olcumNo = item.KazanDetay.olcumNo;
                        int kazanDetayIDYO = dbOlcum.KazanDetay.Where(x => x.orderID == orderID && x.giysiTuruID == giysiTuruID && x.olcuTuruID == olcuTuruID && x.olcumNo == olcumNo && x.tabloTuru == 0).Select(x => x.id).FirstOrDefault();

                        if (kazanDetayIDYO != 0)
                        {
                            SqlParameter param11 = new SqlParameter("@kazanDetayID", kazanDetayID);
                            SqlParameter param21 = new SqlParameter("@olcuNoktaID", olcuNoktaID);
                            SqlParameter param3 = new SqlParameter("@orderID", orderID);
                            SqlParameter param4 = new SqlParameter("@kazanDetayIDYO", kazanDetayIDYO);

                            var cmd = dbOlcum.Database.Connection.CreateCommand();
                            cmd.CommandText = "exec [dbo].[YikamaSonrasiCekme] @kazandetayID,@kazandetayIDYO,@olcuNoktaID,@orderID";
                            cmd.Parameters.Add(param3);
                            cmd.Parameters.Add(param4);
                            cmd.Parameters.Add(param21);
                            cmd.Parameters.Add(param11);


                            dbOlcum.Database.Connection.Open();

                            var reader = cmd.ExecuteReader();

                            double yoOlculen = 0;
                            double uygulananTolCekme = 0;
                            double asilTablo = 0;
                            if (reader.HasRows)
                            {
                                reader.Read();
                                if (!Convert.IsDBNull(reader.GetValue(0)))
                                {
                                    yoOlculen = reader.GetDouble(0);
                                }

                                if (!Convert.IsDBNull(reader.GetValue(1)))
                                {
                                    uygulananTolCekme = reader.GetDouble(1);
                                }

                                if (!Convert.IsDBNull(reader.GetValue(2)))
                                {
                                    asilTablo = reader.GetDouble(2);
                                }
                                reader.Close();

                                item.yoOlculen = yoOlculen;
                                item.uygulananTolCekme = uygulananTolCekme;
                                item.asilTablo = asilTablo;
                                item.oncekiTablo = Math.Round(asilTablo + item.ortalamaDeger, 2, MidpointRounding.AwayFromZero);

                                if (yoOlculen != 0)
                                {
                                    item.gerceklesenTolCekme = Math.Round(((yoOlculen - item.oncekiTablo) / yoOlculen) * 100, 2, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    item.gerceklesenTolCekme = 0;
                                }

                                dbOlcum.SaveChanges();

                            }

                            dbOlcum.Database.Connection.Close();

                        }

                    }
                }

                MessageBox.Show("Başarılı");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                dbOlcum.Database.Connection.Close();

            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata:" + ex.ToString());
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                int kazanDetayID = Convert.ToInt32(textBox1.Text);
                KazanDetay dbKazanDetay = dbOlcum.KazanDetay.Where(x => x.id == kazanDetayID).FirstOrDefault();

                List<KazanAra> dbKazanAra = dbOlcum.KazanAra.Where(x => x.kazanDetayID == kazanDetayID).ToList();

                foreach (var item in dbKazanAra.Select(x => x.olcuNoktaID).Distinct())
                {
                    double ortalamaDeger = 0;
                    double yoOlculen = 0;
                    double gerceklesenTolCekme = 0;
                    double oncekiTablo = 0;
                    double uygulananTolCekme = 0;
                    double asilTablo = 0;

                    //
                    KazanHesaplama newKazanHesaplama = new KazanHesaplama();
                    newKazanHesaplama.kazanDetayID = kazanDetayID;
                    newKazanHesaplama.olcuNoktasiID = Convert.ToInt32(item);


                    if (dbKazanDetay.tabloTuru == 0)
                    {

                        SqlParameter param1 = new SqlParameter("@KazanDetayID", kazanDetayID);
                        SqlParameter param2 = new SqlParameter("@olcuNoktaID", Convert.ToInt32(item));

                        var cmd = dbOlcum.Database.Connection.CreateCommand();
                        cmd.CommandText = "[dbo].[OrtalamaDegerBulmaYO] @KazanDetayID, @olcuNoktaID";
                        cmd.Parameters.Add(param1);
                        cmd.Parameters.Add(param2);

                        dbOlcum.Database.Connection.Open();

                        var reader = cmd.ExecuteReader();

                        reader.Read();
                        if (!Convert.IsDBNull(reader.GetValue(0)))
                        {
                            ortalamaDeger = Math.Round(reader.GetDouble(0), 2, MidpointRounding.AwayFromZero);
                            newKazanHesaplama.ortalamaDeger = ortalamaDeger;
                        }
                        reader.Close();


                        SqlParameter yoParam = new SqlParameter("@kazandetayID", kazanDetayID);
                        SqlParameter yoParam1 = new SqlParameter("@OrderID", dbKazanDetay.orderID);
                        SqlParameter yoParam2 = new SqlParameter("@olcuNoktaID", item);

                        var cmd2 = dbOlcum.Database.Connection.CreateCommand();
                        cmd2.CommandText = "[dbo].[YikamaOncesiKalip] @OrderID,@olcuNoktaID,@kazanDetayID";

                        cmd2.Parameters.Add(yoParam1);
                        cmd2.Parameters.Add(yoParam2);
                        cmd2.Parameters.Add(yoParam);

                        var reader2 = cmd2.ExecuteReader();
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {
                                if (!Convert.IsDBNull(reader2.GetValue(0)))
                                {
                                    oncekiTablo = Math.Round(reader2.GetDouble(0), 2, MidpointRounding.AwayFromZero);
                                }
                                newKazanHesaplama.oncekiTablo = oncekiTablo;

                                if (!Convert.IsDBNull(reader2.GetValue(1)))
                                {
                                    uygulananTolCekme = Math.Round(reader2.GetDouble(1), 2, MidpointRounding.AwayFromZero);
                                }

                                newKazanHesaplama.uygulananTolCekme = uygulananTolCekme;

                                if (!Convert.IsDBNull(reader2.GetValue(2)))
                                {
                                    asilTablo = Math.Round(reader2.GetDouble(2), 2, MidpointRounding.AwayFromZero);
                                }
                                newKazanHesaplama.asilTablo = asilTablo;
                            }
                            reader2.Close();
                        }
                        else
                        {
                            newKazanHesaplama.oncekiTablo = oncekiTablo;
                            newKazanHesaplama.uygulananTolCekme = uygulananTolCekme;
                            newKazanHesaplama.asilTablo = asilTablo;
                            reader2.Close();

                        }


                        yoOlculen = Math.Round(ortalamaDeger + asilTablo, 2, MidpointRounding.AwayFromZero);
                        newKazanHesaplama.yoOlculen = yoOlculen;

                        gerceklesenTolCekme = Math.Round(oncekiTablo - yoOlculen, 2, MidpointRounding.AwayFromZero);
                        newKazanHesaplama.gerceklesenTolCekme = gerceklesenTolCekme;


                    }
                    else if (dbKazanDetay.tabloTuru == 1)
                    {
                        int olcuturuID = 0;

                        int yoKazanDetayID = 0;

                        SqlParameter param1 = new SqlParameter("@KazanDetayID", kazanDetayID);
                        SqlParameter param2 = new SqlParameter("@olcuNoktaID", Convert.ToInt32(item));

                        var cmd = dbOlcum.Database.Connection.CreateCommand();
                        cmd.CommandText = "[dbo].[OrtalamaDegerBulma] @KazanDetayID, @olcuNoktaID";
                        cmd.Parameters.Add(param1);
                        cmd.Parameters.Add(param2);

                        dbOlcum.Database.Connection.Open();

                        var reader = cmd.ExecuteReader();

                        reader.Read();
                        if (!reader.IsDBNull(0))
                        {
                            ortalamaDeger = Math.Round(reader.GetDouble(0), 2, MidpointRounding.AwayFromZero);

                        }
                        else
                        {
                            ortalamaDeger = 0;
                        }
                        newKazanHesaplama.ortalamaDeger = ortalamaDeger;
                        reader.Close();


                        if (Convert.ToInt32(dbKazanDetay.olcuTuruID) == 18 || Convert.ToInt32(dbKazanDetay.olcuTuruID) == 19 || Convert.ToInt32(dbKazanDetay.olcuTuruID) == 31 || Convert.ToInt32(dbKazanDetay.olcuTuruID) == 33 || Convert.ToInt32(dbKazanDetay.olcuTuruID) == 32 || Convert.ToInt32(dbKazanDetay.olcuTuruID) == 35 || Convert.ToInt32(dbKazanDetay.olcuTuruID) == 17)
                        {
                            olcuturuID = 17;

                            yoKazanDetayID = dbOlcum.KazanDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == olcuturuID && x.tabloTuru == 0 && (x.durum != null || x.durum != false)).Select(x => x.id).FirstOrDefault();

                            if (yoKazanDetayID == 0)
                            {
                                yoKazanDetayID = dbOlcum.KazanDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == 16  && x.tabloTuru == 0 && x.KazanAra.FirstOrDefault() != null && (x.durum != null || x.durum != false)).Select(x => x.id).FirstOrDefault();
                            }
                        }
                        else
                        {
                            olcuturuID = dbKazanDetay.olcuTuruID;

                            yoKazanDetayID = dbOlcum.KazanDetay.Where(x => x.orderID == dbKazanDetay.orderID && x.olcuTuruID == olcuturuID && x.tabloTuru == 0 && x.KazanAra.FirstOrDefault() != null && (x.durum != null || x.durum != false)).Select(x => x.id).FirstOrDefault();
                        }

                        var cmd2 = dbOlcum.Database.Connection.CreateCommand();
                        SqlParameter ysParam1 = new SqlParameter("@kazanDetayID", dbKazanDetay.id);
                        SqlParameter ysParam2 = new SqlParameter("@kazanDetayIDYO", yoKazanDetayID);
                        SqlParameter ysParam3 = new SqlParameter("@olcuNoktaID", item);
                        SqlParameter ysParam4 = new SqlParameter("@orderID", dbKazanDetay.orderID);

                        cmd2.CommandText = "[dbo].[YikamaSonrasiCekme] @kazanDetayID,@kazanDetayIDYO,@olcuNoktaID,@orderID";

                        cmd2.Parameters.Add(ysParam1);
                        cmd2.Parameters.Add(ysParam2);
                        cmd2.Parameters.Add(ysParam3);
                        cmd2.Parameters.Add(ysParam4);

                        var reader2 = cmd2.ExecuteReader();

                        while (reader2.Read())
                        {
                            if (!Convert.IsDBNull(reader2.GetValue(0)))
                            {
                                yoOlculen = Math.Round(reader2.GetDouble(0), 2, MidpointRounding.AwayFromZero);
                            }

                            newKazanHesaplama.yoOlculen = yoOlculen;

                            if (!Convert.IsDBNull(reader2.GetValue(1)))
                            {
                                uygulananTolCekme = Math.Round(reader2.GetDouble(1), 2, MidpointRounding.AwayFromZero);
                            }

                            newKazanHesaplama.uygulananTolCekme = uygulananTolCekme;

                            if (!Convert.IsDBNull(reader2.GetValue(2)))
                            {
                                asilTablo = Math.Round(reader2.GetDouble(2), 2, MidpointRounding.AwayFromZero);
                            }

                            newKazanHesaplama.asilTablo = asilTablo;
                        }
                        reader2.Close();

                        oncekiTablo = Math.Round(asilTablo + ortalamaDeger, 2, MidpointRounding.AwayFromZero);
                        newKazanHesaplama.oncekiTablo = oncekiTablo;

                        if (yoOlculen != 0)
                        {
                            gerceklesenTolCekme = Math.Round(((yoOlculen - oncekiTablo) / yoOlculen) * 100, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            gerceklesenTolCekme = 0;
                        }
                        newKazanHesaplama.gerceklesenTolCekme = gerceklesenTolCekme;
                    }

                    dbOlcum.KazanHesaplama.Add(newKazanHesaplama);
                    dbOlcum.SaveChanges();
                    dbOlcum.Database.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            List<OlcuTabloDetay> detayList = dbOlcum.OlcuTabloDetay.ToList();

            List<OlcuTabloAra> araList = dbOlcum.OlcuTabloAra.ToList();

            List<OlcuTabloHesaplama> hesaplamaList = dbOlcum.OlcuTabloHesaplama.ToList();

            for(int i = 0; i < detayList.Count; i++)
            {
                int olcutabloID = detayList[i].id;

                List<OlcuTabloAra> araListDetay = araList.Where(x => x.olcuTabloID == olcutabloID).ToList();

                List<OlcuTabloHesaplama> hesaplamaListDetay = hesaplamaList.Where(x=>x.olcuTabloID == olcutabloID).ToList();


                for (int j = 0; j < araListDetay.Count; j++)
                {

                    var kayit = araListDetay[j];
                    if (j>0)
                    {
                        var kayitOnceki = araListDetay[j-1];
                        if (kayit.olcuNoktaID!=kayitOnceki.olcuNoktaID)
                        {
                            if (kayitOnceki.OlcuNoktalari.tabanID==7  && kayit.OlcuNoktalari.tabanID==85)
                            {
                                int araListDetayId = (int)kayit.id;
                                int araListDetayOlcuNoktaId = (int)kayit.olcuNoktaID;
                                var sonrakiler = araListDetay.Where(x => x.id >= araListDetayId && x.olcuNoktaID== araListDetayOlcuNoktaId).ToList();
                                foreach (var item in sonrakiler)
                                {
                                    string olcuNoktasi = item.OlcuNoktalari.olcuNoktasi;

                                    olcuNoktasi = olcuNoktasi.Replace("BALDIR", "DİZ");

                                    bool checkExists = dbOlcum.OlcuNoktalari.Any(x => x.olcuNoktasi == olcuNoktasi);

                                    if (checkExists)
                                    {
                                        int olcuNoktaID = dbOlcum.OlcuNoktalari.Where(x => x.olcuNoktasi == olcuNoktasi).Select(x => x.id).FirstOrDefault();
                                        item.olcuNoktaID = olcuNoktaID;
                                        dbOlcum.SaveChanges();
                                    }
                                    else
                                    {
                                        OlcuNoktalari newOlcuNoktasi = new OlcuNoktalari();

                                        newOlcuNoktasi.giysiTuruID = null;
                                        newOlcuNoktasi.olcuNoktasi = olcuNoktasi;
                                        newOlcuNoktasi.tabanID = 88;
                                        newOlcuNoktasi.anaNoktami = false;
                                        newOlcuNoktasi.enBoy = false;
                                        newOlcuNoktasi.siraNo = 13;

                                        dbOlcum.OlcuNoktalari.Add(newOlcuNoktasi);
                                        dbOlcum.SaveChanges();

                                        item.olcuNoktaID = newOlcuNoktasi.id;
                                        dbOlcum.SaveChanges();
                                    }
                                }
                            }

                        }

                    }
                }

                for(int j = 0; j < hesaplamaListDetay.Count; j++)
                {
                    var kayit = hesaplamaListDetay[j];

                    if(j > 0)
                    {
                        var kayitOnceki = hesaplamaListDetay[j - 1];

                        if (kayitOnceki.OlcuNoktalari.tabanID == 7 && kayit.OlcuNoktalari.tabanID == 85)
                        {
                            string olcuNoktasi = kayit.OlcuNoktalari.olcuNoktasi;

                            olcuNoktasi = olcuNoktasi.Replace("BALDIR", "DİZ");

                            bool checkExists = dbOlcum.OlcuNoktalari.Any(x => x.olcuNoktasi == olcuNoktasi);

                            if (checkExists)
                            {
                                int olcuNoktaID = dbOlcum.OlcuNoktalari.Where(x => x.olcuNoktasi == olcuNoktasi).Select(x => x.id).FirstOrDefault();
                                kayit.olcuNoktaID = olcuNoktaID;
                                dbOlcum.SaveChanges();
                            }
                            else
                            {
                                OlcuNoktalari newOlcuNoktasi = new OlcuNoktalari();

                                newOlcuNoktasi.giysiTuruID = null;
                                newOlcuNoktasi.olcuNoktasi = olcuNoktasi;
                                newOlcuNoktasi.tabanID = 88;
                                newOlcuNoktasi.anaNoktami = false;
                                newOlcuNoktasi.enBoy = false;
                                newOlcuNoktasi.siraNo = 13;

                                dbOlcum.OlcuNoktalari.Add(newOlcuNoktasi);
                                dbOlcum.SaveChanges();

                                kayit.olcuNoktaID = newOlcuNoktasi.id;
                                dbOlcum.SaveChanges();
                            }
                        }
                    }
                }

            }
        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                List<KazanDetay> kazanDetays = dbOlcum.KazanDetay.Where(x=>x.id > 6400 && x.KazanAra.Any(y=>y.olcuNoktaID == 231 || y.olcuNoktaID == 156)).OrderByDescending(x=>x.id).ToList();
                List<KazanAra> kazanAras = dbOlcum.KazanAra.Where(x=>(x.olcuNoktaID == 231 || x.olcuNoktaID == 156) && x.kazanDetayID > 5656 ).ToList();
                List<KazanHesaplama> kazanHesaplamas = dbOlcum.KazanHesaplama.Where(x=>(x.olcuNoktasiID == 231 || x.olcuNoktasiID == 156) && x.kazanDetayID > 5656).ToList();

                foreach (var item in kazanDetays)
                {
                    List<KazanAra> kazanAraList = kazanAras.Where(x => x.kazanDetayID == item.id).ToList();
                    List<KazanHesaplama> kazanHesaplamaList = kazanHesaplamas.Where(x => x.kazanDetayID == item.id).ToList();

                    if(kazanAraList.Any(x=>x.olcuNoktaID == 231) || kazanAraList.Any(x=>x.olcuNoktaID == 156))
                    {
                        var olcuTuruId = item.olcuTuruID;
                        var orderId = item.orderID;
                        
                        if(olcuTuruId == 22 || olcuTuruId == 51)
                        {
                            NumuneDetay numuneDetay = dbOlcum.NumuneDetay.Where(x => (x.olcuTuruID == 23 || x.olcuTuruID == 24) && x.orderID == orderId).FirstOrDefault();

                            if(numuneDetay != null)
                            {
                                var numuneDetayId = numuneDetay.id;

                                if(dbOlcum.NumuneAra.Any(x=>x.numuneDetayID == numuneDetayId && x.olcuNoktaID  == 157))
                                {
                                    List<KazanAra> tempKazanAras = kazanAraList.Where(x => x.olcuNoktaID == 231).ToList();
                                    List<KazanHesaplama> tempKazanHesaplamas = kazanHesaplamaList.Where(x => x.olcuNoktasiID == 231).ToList();

                                    foreach (var kazanAra in tempKazanAras)
                                    {
                                        kazanAra.olcuNoktaID = 157;
                                    }

                                    foreach (var kazanHesaplama in tempKazanHesaplamas)
                                    {
                                        kazanHesaplama.olcuNoktasiID = 157;
                                    }

                                }

                                if (dbOlcum.NumuneAra.Any(x => x.numuneDetayID == numuneDetayId && x.olcuNoktaID == 152))
                                {

                                    List<KazanAra> tempKazanAras = kazanAraList.Where(x => x.olcuNoktaID == 156).ToList();
                                    List<KazanHesaplama> tempKazanHesaplamas = kazanHesaplamaList.Where(x => x.olcuNoktasiID == 156).ToList();

                                    foreach (var kazanAra in tempKazanAras)
                                    {
                                        kazanAra.olcuNoktaID = 152;
                                    }

                                    foreach (var kazanHesaplama in tempKazanHesaplamas)
                                    {
                                        kazanHesaplama.olcuNoktasiID = 152;
                                    }

                                }
                            }
                        }
                        else
                        {
                            OlcuTabloDetay olcuTabloDetay = dbOlcum.OlcuTabloDetay.Where(x => x.orderID == orderId).FirstOrDefault();

                            if(olcuTabloDetay != null)
                            {
                                var olcuTabloId = olcuTabloDetay.id;

                                if(dbOlcum.OlcuTabloAra.Any(x=>x.olcuTabloID == olcuTabloId && x.olcuNoktaID == 157))
                                {

                                    List<KazanAra> tempKazanAras = kazanAraList.Where(x => x.olcuNoktaID == 231).ToList();
                                    List<KazanHesaplama> tempKazanHesaplamas = kazanHesaplamaList.Where(x => x.olcuNoktasiID == 231).ToList();

                                    foreach (var kazanAra in tempKazanAras)
                                    {
                                        kazanAra.olcuNoktaID = 157;
                                    }

                                    foreach (var kazanHesaplama in tempKazanHesaplamas)
                                    {
                                        kazanHesaplama.olcuNoktasiID = 157;
                                    }

                                }

                                if (dbOlcum.OlcuTabloAra.Any(x => x.olcuTabloID == olcuTabloId && x.olcuNoktaID == 152))
                                {

                                    List<KazanAra> tempKazanAras = kazanAraList.Where(x => x.olcuNoktaID == 156).ToList();
                                    List<KazanHesaplama> tempKazanHesaplamas = kazanHesaplamaList.Where(x => x.olcuNoktasiID == 156).ToList();

                                    foreach (var kazanAra in tempKazanAras)
                                    {
                                        kazanAra.olcuNoktaID = 152;
                                    }

                                    foreach (var kazanHesaplama in tempKazanHesaplamas)
                                    {
                                        kazanHesaplama.olcuNoktasiID = 152;
                                    }

                                }

                            }
                        }
                    }



                }
                dbOlcum.SaveChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

