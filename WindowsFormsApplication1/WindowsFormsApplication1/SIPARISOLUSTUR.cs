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
    public partial class SIPARISOLUSTUR : Form
    {
        public SIPARISOLUSTUR()
        {
            InitializeComponent();
        }

        MuhasebeEntities2 db = new MuhasebeEntities2();
        List<TABLESIPARIS> siparişler = new List<TABLESIPARIS>();
        Random rd = new Random();
        public int index = cari.m_index;
        public string ad = cari.m_ad;
        public string soyad = cari.m_soyad;
        public double toplam_tutar;

        private void SIPARISOLUSTUR_Load(object sender, EventArgs e)
        {
            MessageBox.Show(siparişler.Count.ToString());
            textBox8.Text = ad;
            textBox9.Text = soyad;
            comboBox1.DataSource = db.TABLOKATEG.Select(x=>x.CINSADI).ToList();
        }

        //siparişno oluştur
        private void button1_Click(object sender, EventArgs e)
        {
            string diyez = "#";
            int sayi = rd.Next(100000, 999999);
            string siparis_no = diyez + sayi.ToString();
            var kontrol = db.TABLESIPARIS.Select(x => x.SİPARİŞNO).ToList();
            foreach (var item in kontrol)
            {
                if (item == siparis_no)
                {
                    sayi = rd.Next(100000, 999999);
                    siparis_no = diyez + sayi.ToString();
                }
            }
            textBox1.Text = siparis_no;
            groupBox1.Enabled = true;
            button3.Enabled = true;
        }


        //siparişe ürün ekleme
        private void button3_Click(object sender, EventArgs e)
        {
            string siparisno = textBox1.Text;
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
            var müs = db.TABLEMUSTERİ.Find(index);

            t.SİPARİŞNO = textBox1.Text;
            t.MUSTERI = index;
            t.URUNCINS = urun_cinsi;
            t.URUNUN_ADI = ürünün_adı;
            t.URUN_ADEDİ = Convert.ToInt32(textBox2.Text);
            t.TARİH = dateTimePicker1.Value;

            double tutar = (double)t.URUN_ADEDİ * birim_fiyat;

            t.TUTAR = tutar;
            toplam_tutar += (double)t.TUTAR;

            if (t.URUN_ADEDİ != 0)
            {
                int sayi = (int)db.SORGU().SingleOrDefault(x => ua == x.URUNADI).STOK_ADEDİ;
                if (t.URUN_ADEDİ > sayi)
                {
                    MessageBox.Show("BU ÜRÜNDEN STOKTA BU KADAR YOK KALAN SON ADET SAYISI : " + sayi
                    , "STOK AŞILDI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                siparişler.Add(t);
            }
            else
            {
                MessageBox.Show("0 GİRİLEMEZ");
            }
            button4.Enabled = true;
            var query = from item in siparişler
                        join müsteri in db.TABLEMUSTERİ on item.MUSTERI equals müsteri.ID
                        join übil in db.URUNİSİMLERİ on item.URUNUN_ADI equals übil.URUNADI_ID
                        where item.SİPARİŞNO == t.SİPARİŞNO
                        select new
                        {
                            ADSOYAD = müsteri.AD + müsteri.SOYAD,
                            übil.TABLOKATEG.CINSADI,
                            übil.URUNADI,
                            item.URUN_ADEDİ,
                            TUTAR = item.URUN_ADEDİ * übil.BİRİM_FİYAT
                        };
            dataGridView1.DataSource = query.ToList();
            label15.Text = toplam_tutar.ToString();
        }

        //ürün adedine sadece sayı girdirme
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var aranan = comboBox1.SelectedItem.ToString();
            var cont = from item in db.URUNİSİMLERİ
                       where aranan == item.TABLOKATEG.CINSADI
                       select item.URUNADI;
            comboBox2.DataSource = cont.ToList();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ürün = comboBox2.SelectedItem.ToString();
            var özellikler = db.URUNİSİMLERİ.Single(x=>x.URUNADI == ürün);
            textBox4.Text = özellikler.TABLOKATEG.CINSADI;
            textBox5.Text = özellikler.URUNADI;
            textBox6.Text = özellikler.STOK_ADEDİ.ToString();
            textBox7.Text = özellikler.BİRİM_FİYAT.ToString(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SIPARISLER S = new SIPARISLER();
            S.CARİID = index;
            S.NO = textBox1.Text;
            S.BAŞLIK = textBox10.Text;
            S.TUTAR = toplam_tutar;
            db.SIPARISLER.Add(S);
            foreach (var item in siparişler)
            {
                db.TABLESIPARIS.Add(item);
            }
            db.SaveChanges();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string aranan = textBox3.Text.ToUpper();
            var özellikler = db.URUNİSİMLERİ.FirstOrDefault(x => x.URUNADI.Contains(aranan));
            if (özellikler == null)
            {
                MessageBox.Show("Böyle bir ürün bulunmamaktadır...");
                return;
            }
            textBox4.Text = özellikler.TABLOKATEG.CINSADI;
            textBox5.Text = özellikler.URUNADI;
            textBox6.Text = özellikler.STOK_ADEDİ.ToString();
            textBox7.Text = özellikler.BİRİM_FİYAT.ToString();
        }

    }
}

//EKLEME
/*
 
            

           


//cins listele
/*

*/


