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
    public partial class yonetim : Form
    {
        public yonetim()
        {
            InitializeComponent();
        }

        MuhasebeEntities2 db = new MuhasebeEntities2();
        Random rnd = new Random();

        private void yonetim_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = db.TABLOKATEG.Select(x => x.CINSADI).ToList();
            dataGridView1.DataSource = db.GELİRTABLO().ToList();
            double toplam;
            toplam = (double)db.GELİRTABLO().Sum(x => x.KAZANÇ);
            var que = db.GELİRTABLO().Select(x => x.KAZANÇ).ToList();

            chart1.ChartAreas[0].AxisY.Maximum = 100;
            chart1.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
            int i = 0;
            foreach (var item in que)
            {
                double yüzde = (double)(item * 100) / toplam;
                var liste = db.GELİRTABLO().Select(x => x.URUNADI).ToList();
                string uadı = liste[i].ToString();
                chart1.Series.Add(uadı);
                chart1.Series[uadı].Points.AddY(yüzde);
                chart1.Series[uadı].Label = "%" + Math.Round(yüzde, 1).ToString();
                i++;
            }
            Bilgileri_Getir();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            double toplam;
            toplam = (double)db.GELİRTABLO().Sum(x => x.KAZANÇ);
            var que = db.TABLOKATEG.Select(x=>x.CINSADI).ToList(); 
            chart1.Series.Clear();

            foreach (var item in que)
            {
                var ique = db.GELİRTABLO().Where(x=>x.CINSADI == item).Select(x=>x.URUNADI).ToList();
                double toplam2 = (double)db.GELİRTABLO().Where(x => x.CINSADI == item).Sum(x=>x.KAZANÇ);
                double yüzde = (double)(toplam2 * 100) / toplam;
                chart1.Series.Add(item);
                chart1.Series[item].Points.AddY(yüzde);
                chart1.Series[item].LabelBorderWidth = 4;
                chart1.Series[item].Label = "%" + Math.Round(yüzde, 1).ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            if (checkBox1.Checked == true)
            {
                string cins = comboBox1.SelectedItem.ToString();
                var que = db.TABLOKATEG.SingleOrDefault(x => x.CINSADI == cins);


                int toplam = (int)db.GELİRTABLO().Where(x => x.CINSADI == cins).Sum(x => x.SATILAN_ÜRÜN);

                var que2 = db.GELİRTABLO().Where(x => x.CINSADI == cins).Select(x => x.URUNADI).ToList();

                chart1.Series.Add(que.CINSADI);

                int i = 0;
                foreach (var item in que2)
                {
                    int toplam2 = (int)db.GELİRTABLO().Where(x => x.URUNADI == item).Sum(x => x.SATILAN_ÜRÜN);
                    int yüzde = (int)(toplam2 * 100) / toplam;
                    chart1.Series[que.CINSADI].Points.AddXY(item, yüzde);
                    chart1.Series[que.CINSADI].Points[i].Label = toplam2.ToString();
                    i++;
                }
            }
            else
            {
                string cins = comboBox1.SelectedItem.ToString();
                var que = db.TABLOKATEG.SingleOrDefault(x => x.CINSADI == cins);


                double toplam = (double)db.GELİRTABLO().Where(x => x.CINSADI == cins).Sum(x => x.KAZANÇ);

                var que2 = db.GELİRTABLO().Where(x => x.CINSADI == cins).Select(x => x.URUNADI).ToList();

                chart1.Series.Add(que.CINSADI);

                int i = 0;
                foreach (var item in que2)
                {
                    double toplam2 = (double)db.GELİRTABLO().Where(x => x.URUNADI == item).Sum(x => x.KAZANÇ);
                    double yüzde = (double)(toplam2 * 100) / toplam;
                    chart1.Series[que.CINSADI].Points.AddXY(item, yüzde);
                    chart1.Series[que.CINSADI].Points[i].Label = "%" + Math.Round(yüzde, 1).ToString();
                    i++;
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            comboBox1.DataSource = db.TABLOKATEG.Select(x => x.CINSADI).ToList();
            double toplam;
            toplam = (double)db.GELİRTABLO().Sum(x => x.KAZANÇ);
            var que = db.GELİRTABLO().Select(x => x.KAZANÇ == null ? x.KAZANÇ = 0 : x.KAZANÇ).ToList();

            int i = 0;
            foreach (var item in que)
            {
                double yüzde = (double)(item * 100) / toplam;
                var liste = db.GELİRTABLO().Select(x => x.URUNADI).ToList();
                string uadı = liste[i].ToString();
                chart1.Series.Add(uadı);
                chart1.Series[uadı].Points.AddY(yüzde);
                chart1.Series[uadı].Label = "%" + Math.Round(yüzde, 1).ToString();
                i++;
            }
        }

        private void rad_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            int toplam = (int)db.GELİRTABLO().Sum(x => x.SATILAN_ÜRÜN);

            var que2 = db.GELİRTABLO().Select(x => x.URUNADI).ToList();

            chart1.Series.Add("ÜrünAdetleri");
            int i = 0;
            foreach (var item in que2)
            {
                int toplam2 = (int)db.GELİRTABLO().Where(x => x.URUNADI == item).Sum(x => x.SATILAN_ÜRÜN);
                int yüzde = (int)(toplam2 * 100) / toplam;
                chart1.Series[0].Points.AddXY(item,yüzde);
                chart1.Series[0].Points[i].Label = toplam2.ToString();
                i++;
            }
        }

        void Bilgileri_Getir() 
        {
            //toplam cins sayısı////////////////////
            int toplamcins = db.TABLOKATEG.Count();
            label8.Text = toplamcins.ToString();
            ////////////////////////////////////////
            //
            //toplam ürün sayısı////////////////////
            int toplamurun = db.URUNİSİMLERİ.Count();
            label9.Text = toplamurun.ToString();
            ////////////////////////////////////////
            //
            //stoktaki toplam ürün sayısı///////////
            int stokurun = (int)db.GELİRTABLO().Sum(x => x.STOK_ADEDİ);
            label10.Text = stokurun.ToString();
            ////////////////////////////////////////
            //
            //satılan toplam ürün sayısı///////////
            int satilanurun = (int)db.GELİRTABLO().Sum(x => x.SATILAN_ÜRÜN);
            label11.Text = satilanurun.ToString();
            ////////////////////////////////////////
            //
            //kalan toplam ürün sayısı///////////
            int stokta_kalan = (int)db.GELİRTABLO().Sum(x => x.STOK_ADEDİ);
            int kalanurun = stokta_kalan - satilanurun;
            label15.Text = kalanurun.ToString();
            ////////////////////////////////////////
            //
            //en çok satılan ürün sayısı///////////
            int eua = (int)db.GELİRTABLO().Max(x => x.SATILAN_ÜRÜN);
            var enurun = db.GELİRTABLO().FirstOrDefault(x => x.SATILAN_ÜRÜN == eua);
            label12.Text = enurun.URUNADI;
            ////////////////////////////////////////
            //
            //toplam kazanç///////////
            double kazanc = (double)db.GELİRTABLO().Sum(x => x.KAZANÇ);
            label13.Text = kazanc.ToString() + " ₺";
            ////////////////////////////////////////
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            var query = from item in db.TABLESIPARIS
                        join item2 in db.TABLEMUSTERİ on item.MUSTERI equals item2.ID
                        join item3 in db.TABLOKATEG on item.URUNCINS equals item3.CINSID
                        join item4 in db.URUNİSİMLERİ on item.URUNUN_ADI equals item4.URUNADI_ID

                        where item.SIPARISLER.ONAY == true && item.TARİH >= dateTimePicker1.Value && item.TARİH <= dateTimePicker2.Value
                        
                        group item4 by item4.URUNADI into grup
                        select new
                        {
                            URUNCINSI = grup.FirstOrDefault(x=>x.TABLOKATEG.CINSID == x.CINS).TABLOKATEG.CINSADI,
                            URUNADI = grup.Key,
                            BİRİMFİYAT = grup.FirstOrDefault(x => x.URUNADI == grup.Key).BİRİM_FİYAT,
                            SATILAN = grup.FirstOrDefault(x => x.URUNADI == grup.Key).TABLESIPARIS.Where(x =>x.SIPARISLER.ONAY == true && x.TARİH >= dateTimePicker1.Value && x.TARİH <= dateTimePicker2.Value).Sum(x => x.URUN_ADEDİ),
                            KAZANÇ = grup.FirstOrDefault(x => x.URUNADI == grup.Key).TABLESIPARIS.Where(x => x.SIPARISLER.ONAY == true && x.TARİH >= dateTimePicker1.Value && x.TARİH <= dateTimePicker2.Value).Sum(x => x.URUN_ADEDİ) *
                            grup.FirstOrDefault(x => x.URUNADI == grup.Key).BİRİM_FİYAT
                        };                

            try
            {
                int toplam = (int)query.Sum(x => x.KAZANÇ);

                var que = query.Select(x => x.KAZANÇ).ToList();

                int i = 0;
                foreach (var item in que)
                {
                    double yüzde = (double)(item * 100) / toplam;
                    var liste = query.Select(x => x.URUNADI).ToList();
                    string uadı = liste[i].ToString();
                    chart1.Series.Add(uadı);
                    chart1.Series[uadı].Points.AddY(yüzde);
                    chart1.Series[uadı].Label = "%" + Math.Round(yüzde, 1).ToString();
                    //
                    i++;
                }
            }
            catch (InvalidOperationException)//toplam null dönerse bir uyarı döndürür.
            {
                MessageBox.Show("Satış bulunamadı");
                return;
            }


   
            dataGridView1.DataSource = query.ToList();
        }
    }
}