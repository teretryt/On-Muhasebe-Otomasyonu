using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ciurn : Form
    {
        public ciurn()
        {
            InitializeComponent();
        }

        MuhasebeEntities2 db = new MuhasebeEntities2();

        private void cins_getir() 
        {
            comboBox1.DataSource = db.TABLOKATEG.Select(x => x.CINSADI).ToList();
            comboBox2.DataSource = db.TABLOKATEG.Select(x => x.CINSADI).ToList();
            comboBox3.DataSource = db.URUNİSİMLERİ.Select(x => x.URUNADI).ToList();
        }

        private void ciurn_Load(object sender, EventArgs e)
        {
            cins_getir();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string deger = textBox1.Text;

            TABLOKATEG t1 = new TABLOKATEG();
            t1.CINSADI = deger.ToUpper();
            db.TABLOKATEG.Add(t1);
            db.SaveChanges();
            cins_getir();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            string deger = textBox1.Text;

            var item = db.TABLOKATEG.Find(index + 1);
            item.CINSADI = deger.ToUpper();
            db.SaveChanges();
            cins_getir();
        }

        
       

        private void button5_Click(object sender, EventArgs e)
        {
            var item = db.URUNİSİMLERİ.FirstOrDefault(x => x.URUNADI.Contains(textBox3.Text.ToUpper()));

            if (item == null)
                {
                    MessageBox.Show("Böyle bir ürün bulunamadı");
                    return;
                }

            string cins = item.TABLOKATEG.CINSADI;
            int cins_id = comboBox2.Items.IndexOf(cins);
            textBox4.Text = item.URUNADI.ToString();
            comboBox2.SelectedIndex = cins_id;
            textBox5.Text = item.BİRİM_FİYAT.ToString();
            int ürün = comboBox3.Items.IndexOf(item.URUNADI);
            comboBox3.SelectedIndex = ürün;
            textBox3.Clear();
                    
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string deger = comboBox3.SelectedItem.ToString();

            var que = db.URUNİSİMLERİ.FirstOrDefault(x => deger == x.URUNADI);

            string cins = que.TABLOKATEG.CINSADI;
            int cins_index = comboBox2.Items.IndexOf(cins);
            comboBox2.SelectedIndex = cins_index;
            textBox4.Text = que.URUNADI;
            textBox5.Text = que.BİRİM_FİYAT.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string cins = comboBox2.SelectedItem.ToString();
            string ürün = comboBox3.SelectedItem.ToString();
            int index = db.TABLOKATEG.Single(x=>x.CINSADI == cins).CINSID;//cins
            int deger = db.URUNİSİMLERİ.Single(x => x.URUNADI == ürün).URUNADI_ID; ;//ürün


            var que = db.URUNİSİMLERİ.Find(deger);
            que.CINS = index;
            que.BİRİM_FİYAT = Convert.ToDouble(textBox5.Text);

            var ara = db.TABLESIPARIS.Where(x => x.URUNUN_ADI == deger).Select(x => x.URUNID).ToList();
            foreach (var item in ara)
            {
                var que_1 = db.TABLESIPARIS.Find(item);
                que_1.URUNCINS = index;
            }

            db.SaveChanges();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            URUNİSİMLERİ Uİ = new URUNİSİMLERİ();
            var que = db.URUNİSİMLERİ.FirstOrDefault(x=>x.URUNADI == textBox4.Text.ToUpper());
            if (que == null)
            {
                Uİ.URUNADI = textBox4.Text.ToUpper();
                Uİ.CINS = comboBox2.SelectedIndex + 1;
                Uİ.BİRİM_FİYAT = Convert.ToInt32(textBox5.Text);
                Uİ.STOK_ADEDİ = 0;
                db.URUNİSİMLERİ.Add(Uİ);
                db.SaveChanges();
                MessageBox.Show(Uİ.URUNADI+" EKLENDİ...");
                return;
            }
            if (que.URUNADI == textBox4.Text.ToUpper())
            {
                MessageBox.Show("Böyle ürün zaten stokta bulunmakta...");
                return;
            }
        }


        //BU İKİ ÖZELLİK KALDIRILDI
        /* private void button3_Click(object sender, EventArgs e)
        {
            string deger = comboBox1.SelectedItem.ToString();
            int index = db.TABLOKATEG.FirstOrDefault(x=>x.CINSADI == deger).CINSID;

            var list = db.TABLESIPARIS.Select(x=>x.URUNCINS).ToList();

            if (list.Contains(index))
            {
                MessageBox.Show("BU KATEGORİ KALDIRILAMAZ ÇÜNKÜ BU KATEGORİYE AİT ÜRÜNLER SİPARİŞ LİSTENİZDE BULUNMAKTADIR", "DİKKAT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var item = db.TABLOKATEG.Find(index);
            db.TABLOKATEG.Remove(item);
            db.SaveChanges();
        }
        */
       /* private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bu işlem ardından bu ürün adına yapılmış tüm siparişler iptal edilecektir eminmisiniz ? ", "Ürün Silme",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                string deger = comboBox3.SelectedItem.ToString();
                var que = db.URUNİSİMLERİ.SingleOrDefault(x=>x.URUNADI == deger);
                if (que == null)
                {
                    MessageBox.Show("Bu ürün stoktan kaldırılmış lütfen sayfayı yenileyip deneyiniz...");
                    return;
                }
                var item = db.URUNİSİMLERİ.Find(que.URUNADI_ID);

                var query = db.TABLESIPARIS.Where(x => x.URUNUN_ADI == que.URUNADI_ID).Select(x=>x.URUNID).ToList();
                foreach (var item1 in query)
                {
                    var que_1 = db.TABLESIPARIS.Find(item1);
                    db.TABLESIPARIS.Remove(que_1);
                }    
                db.URUNİSİMLERİ.Remove(item);
                db.SaveChanges();
            }
            else
            {
                return;
            }
        }
        */

    }
}











/*

*/