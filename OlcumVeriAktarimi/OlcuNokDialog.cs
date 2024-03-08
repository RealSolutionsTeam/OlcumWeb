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
    public partial class OlcuNokDialog : Form
    {
        AtolyeContext dbAtolye = new AtolyeContext();
        OlcumContext dbOlcum = new OlcumContext();
        RaporlamaContext dbOlcumRaporlama = new RaporlamaContext();

        public int seilenOlcuNok { get; set; }

        public OlcuNokDialog(string olcumNoktasi, int otBedenID, int id, int tur)
        {
            InitializeComponent();

            label1.Text = "Aranan Ölçü Noktası  =  " + olcumNoktasi;

            if (tur == 1)
            {
                label2.Text = "OlcuTablosuBeden Id  =  " + otBedenID.ToString();
                label3.Text = "OlcuTabloDetay Id  =  " + id.ToString();
            }
            else if (tur == 2)
            {
                label2.Text = "OlcuFormuBeden2 Id  =  " + otBedenID.ToString();
                label3.Text = "NumuneDetay Id  =  " + id.ToString();
            }
            else if (tur == 3)
            {
                label2.Text = "OlcuFormuBeden Id  =  " + otBedenID.ToString();
                label3.Text = "KazanDetay Id  =  " + id.ToString();
            }
            else if (tur == 4)
            {
                label2.Text = "OlcuFormuBeden3 Id  =  " + otBedenID.ToString();
                label3.Text = "CoğaltmaDetay Id  =  " + id.ToString();
            }
        }

        private void OlcuNokDialog_Load(object sender, EventArgs e)
        {
            List<OlcuNoktalari> list = dbOlcum.OlcuNoktalari.ToList();

            foreach (var item in list)
            {
                comboBox1.Items.Add(item.olcuNoktasi);
            }

            //Test süresinde sormasın diye hızlı seçim
            //seilenOlcuNok = 1;
            //this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            seilenOlcuNok = dbOlcum.OlcuNoktalari.Where(x => x.olcuNoktasi == comboBox1.SelectedItem.ToString()).FirstOrDefault().id;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int n))
            {
                button1.Enabled = true;
                seilenOlcuNok = Convert.ToInt32(textBox1.Text);
            }
            else
            {
                button1.Enabled = false;
                seilenOlcuNok = 0;
            }
        }
    }
}
