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
    public partial class sıpeditor : Form
    {
        public sıpeditor()
        {
            InitializeComponent();
        }


        //global değişkenler
        MuhasebeEntities2 db = new MuhasebeEntities2();
        public string sip_no = sıparısmenu.secilen_no;
        public int index;
        public int[] sid_dizi;
        public double toplam_tutar;

        private void sıpeditor_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = db.TABLOKATEG.Select(x=>x.CINSADI).ToList();
            sid_dizi = db.TABLESIPARIS.Where(x => x.SİPARİŞNO == sip_no).Select(x => x.URUNID).ToArray();
            listele();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string uc = comboBox1.SelectedItem.ToString();
            string ürün_Ad = comboBox2.SelectedItem.ToString();
            int ürün_cins_index = db.TABLOKATEG.Single(x=>x.CINSADI == uc).CINSID;
            int ürün_ad_index = db.URUNİSİMLERİ.Single(x=>x.URUNADI == ürün_Ad).URUNADI_ID;
            int birim_fiyat = (int)db.URUNİSİMLERİ.SingleOrDefault(x => x.URUNADI == ürün_Ad).BİRİM_FİYAT;
            var f2 = db.TABLESIPARIS.Find(sid_dizi[index]);
            var müs = db.TABLEMUSTERİ.Find(f2.MUSTERI);

                double tutar = (double)f2.URUN_ADEDİ * birim_fiyat;
                toplam_tutar -= tutar;

                f2.URUNCINS = ürün_cins_index;
                f2.URUNUN_ADI = ürün_ad_index;
                f2.URUN_ADEDİ = 0;
                f2.SIPARISLER.TUTAR -= tutar;
                db.SaveChanges();
                db.TABLESIPARIS.Remove(f2);
                db.SaveChanges();
                listele();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index = dataGridView1.CurrentRow.Index;
            int c = comboBox1.Items.IndexOf(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            comboBox1.SelectedIndex = c;
            int ü = comboBox2.Items.IndexOf(dataGridView1.CurrentRow.Cells[2].Value.ToString());
            comboBox2.SelectedIndex = ü;
            textBox2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var aranan = comboBox1.SelectedItem.ToString();
            var cont = from item in db.URUNİSİMLERİ
                       where aranan == item.TABLOKATEG.CINSADI
                       select item.URUNADI;
            comboBox2.DataSource = cont.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            sıparısmenu sp = new sıparısmenu();
            sp.Show();
        }


        //güncelle
        private void button3_Click(object sender, EventArgs e)
        {
            string ua = comboBox2.SelectedItem.ToString();
            int ürünün_adı = db.URUNİSİMLERİ.SingleOrDefault(x => x.URUNADI == ua).URUNADI_ID;
            string uc = comboBox1.SelectedItem.ToString();
            int ürünün_cinsi = db.TABLOKATEG.Single(x => x.CINSADI == uc).CINSID;
            if (string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Ürün adedi boş geçilemez...");
                return;
            }
            int sid = sid_dizi[index];
            var f2 = db.TABLESIPARIS.Find(sid);
            var müs = db.TABLEMUSTERİ.Find(f2.MUSTERI);
            double birim_fiyat = (double)f2.URUNİSİMLERİ.BİRİM_FİYAT;
            double birim_fiyat2 = (double)db.URUNİSİMLERİ.SingleOrDefault(x => x.URUNADI == ua).BİRİM_FİYAT;
            double eski_miktar = (double)f2.URUN_ADEDİ * birim_fiyat;
            double yeni_miktar = Convert.ToDouble(textBox2.Text) * birim_fiyat2;
            double fark = eski_miktar - yeni_miktar;



            f2.URUNCINS = ürünün_cinsi;
            f2.URUNUN_ADI = ürünün_adı;
            f2.TUTAR = yeni_miktar;
            f2.URUN_ADEDİ = Convert.ToInt32(textBox2.Text);
            if (yeni_miktar < eski_miktar)
            {
                f2.SIPARISLER.TUTAR -= fark;
            }
            else if (yeni_miktar > eski_miktar)
            {
                f2.SIPARISLER.TUTAR += -fark;
            }
            db.SaveChanges();
            listele();
        }

        private void listele()
        {
            var query = db.TABLESIPARIS.Where(x => x.SİPARİŞNO == sip_no).Select(x => new
            {
                x.TARİH,
                x.TABLOKATEG.CINSADI,
                x.URUNİSİMLERİ.URUNADI,
                x.URUN_ADEDİ,
                x.TUTAR
            });
            dataGridView1.DataSource = query.ToList();
            toplam_tutar = (double)db.TABLESIPARIS.Where(x => x.SİPARİŞNO == sip_no).Sum(x=>x.TUTAR);
            label6.Text = toplam_tutar.ToString();
        }

        //ekle
        private void button2_Click(object sender, EventArgs e)
        {
            int m_index = (int)db.SIPARISLER.Single(X => X.NO == sip_no).CARİID;
            string siparisno = sip_no;
            string uc = comboBox1.SelectedItem.ToString();
            string ua = comboBox2.SelectedItem.ToString();
            int urun_cinsi = db.TABLOKATEG.Single(x => x.CINSADI == uc).CINSID;
            int ürünün_adı = db.URUNİSİMLERİ.SingleOrDefault(x => x.URUNADI == ua).URUNADI_ID;
            double birim_fiyat = (double)db.URUNİSİMLERİ.SingleOrDefault(x => x.URUNADI == ua).BİRİM_FİYAT;
            int uzunluk = textBox2.Text.Length;

            if (uzunluk <= 0)
            {
                MessageBox.Show("Miktar giriniz...");
                return;
            }

            TABLESIPARIS t = new TABLESIPARIS();
            var müs = db.TABLEMUSTERİ.Find(m_index);
            int adet = (int)db.URUNİSİMLERİ.FirstOrDefault(x=>x.URUNADI_ID == ürünün_adı).STOK_ADEDİ;

            t.SİPARİŞNO = sip_no;
            t.MUSTERI = m_index;
            t.URUNCINS = urun_cinsi;
            t.URUNUN_ADI = ürünün_adı;
            t.URUN_ADEDİ = Convert.ToInt32(textBox2.Text);
            if (t.URUN_ADEDİ > adet)
            {
                MessageBox.Show("Stokta bu üründen son "+adet+" kaldı");
                return;
            }
            t.TARİH = dateTimePicker1.Value;

            double tutar = (double)t.URUN_ADEDİ * birim_fiyat;

            t.TUTAR = tutar;

            if (t.URUN_ADEDİ != 0)
            {
                int sayi = (int)db.SORGU().SingleOrDefault(x => ua == x.URUNADI).STOK_ADEDİ;
                if (t.URUN_ADEDİ > sayi)
                {
                    MessageBox.Show("BU ÜRÜNDEN STOKTA BU KADAR YOK KALAN SON ADET SAYISI : " + sayi
                    , "STOK AŞILDI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("0 GİRİLEMEZ");
            }
            db.TABLESIPARIS.Add(t);
            t.SIPARISLER.TUTAR += t.TUTAR;
            db.SaveChanges();
            listele();
        }
    }
}
