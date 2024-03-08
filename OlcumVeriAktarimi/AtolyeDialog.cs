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
    public partial class AtolyeDialog : Form
    {

        AtolyeContext dbAtolye = new AtolyeContext();
        OlcumContext dbOlcum = new OlcumContext();
        RaporlamaContext dbOlcumRaporlama = new RaporlamaContext();

        public int seilenAtolyeId { get; set; }

        public AtolyeDialog(int oftabloID2, string dikimAtolyesi, int tur)
        {
            InitializeComponent();
            label1.Text = "Aranan Atolye  =  " + dikimAtolyesi;

            if(tur == 1)
            {
                label2.Text = "OlcuFormu2 Id  =  " + oftabloID2.ToString();
            }
            else if(tur == 2)
            {
                label2.Text = "OlcuFormu Id  =  " + oftabloID2.ToString();
            }else if(tur == 3)
            {
                label2.Text = "OlcuFormu3 Id  =  " + oftabloID2.ToString();
            }
        }

        private void AtolyeDialog_Load(object sender, EventArgs e)
        {
            List<Atolyes> list = dbAtolye.Atolyes.ToList();

            foreach (var item in list)
            {
                comboBox1.Items.Add(item.AtolyeAd);
            }

            //Test amaçlı hızlı seçim yapıldı
            //seilenAtolyeId = 1;
            //this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            seilenAtolyeId = dbAtolye.Atolyes.Where(x => x.AtolyeAd == comboBox1.SelectedItem.ToString()).FirstOrDefault().Id;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int n))
            {
                button1.Enabled = true;
                seilenAtolyeId = Convert.ToInt32(textBox1.Text);
            }
            else
            {
                button1.Enabled = false;
                seilenAtolyeId = 0;
            }
        }
    }
}
