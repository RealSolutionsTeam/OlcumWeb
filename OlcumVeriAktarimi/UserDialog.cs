using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OlcumVeriAktarimi.dbAtolye;
//using OlcumVeriAktarimi.dbOlcum;
using OlcumVeriAktarimi.dbOlcumRaporlama;
using OlcumVeriAktarimi.dbOlcumTest;

namespace OlcumVeriAktarimi
{
    public partial class UserDialog : Form
    {
        AtolyeContext dbAtolye = new AtolyeContext();
        OlcumContext dbOlcum = new OlcumContext();
        RaporlamaContext dbOlcumRaporlama = new RaporlamaContext();

        public int seilenUserId { get; set; }

        public UserDialog(string arananUser, int id, int tur)
        {
            InitializeComponent();

            label1.Text = "Aranan User  =  " + arananUser;


            if (tur == 1)
            {
                label2.Text = "OlcuTablosu Id=  " + id.ToString();

            }
            else if (tur == 2)
            {
                label2.Text = "OlcuForm2 Id=  " + id.ToString(); 

            }
            else if (tur == 3)
            {

                label2.Text = "OlcuForm3 Id=  " + id.ToString();
            }

        }

        private void UserDialog_Load(object sender, EventArgs e)
        {

            List<dbOlcumTest.PersonelTablo> list = dbOlcum.PersonelTablo.ToList();


            foreach (var item in list)
            {
                comboBox1.Items.Add(item.personelAd);
            }

            //Test amaçlı hızlı seçim yapıldı
            //seilenUserId = 1;
            //this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            seilenUserId = dbOlcum.PersonelTablo.Where(x => x.personelAd == comboBox1.SelectedItem.ToString()).FirstOrDefault().id;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if(int.TryParse(textBox1.Text, out int n))
            {
                button1.Enabled = true;
                seilenUserId = Convert.ToInt32(textBox1.Text);
            }
            else
            {
                button1.Enabled = false;
                seilenUserId = 0;

            }
        }
    }
}
