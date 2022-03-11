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
    public partial class sıparısmenu : Form
    {
        public sıparısmenu()
        {
            InitializeComponent();
        }

        //global değişkenler
        MuhasebeEntities2 db = new MuhasebeEntities2();
        public int index = cari.m_index;
        public string[] no_dizi;
        public static string secilen_no;


        //yüklendiğinde
        private void sıparısmenu_Load(object sender, EventArgs e)
        {
            var query = db.SIPARISLER.Where(x=>x.CARİID == index && x.ONAY == null).Select(x=> new{x.BAŞLIK,x.TABLEMUSTERİ.AD,x.TABLEMUSTERİ.SOYAD,x.TUTAR});

            no_dizi = db.SIPARISLER.Where(x => x.CARİID == index).Select(x => x.NO).ToArray();   
            dataGridView1.DataSource = query.ToList();
            label2.Text = db.SIPARISLER.Where(x => x.CARİID == index && x.ONAY == null).Sum(x=>x.TUTAR).ToString();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int no_index = dataGridView1.CurrentRow.Index;
            secilen_no = no_dizi[no_index].ToString();
            this.Hide();
            sıpeditor sp = new sıpeditor();
            sp.Show();
        }


        //sipariş iptal
        private void button1_Click(object sender, EventArgs e)
        {
            var s = db.SIPARISLER.Find(secilen_no);

            var que = db.TABLESIPARIS.Where(x=>x.SİPARİŞNO == secilen_no).Select(x=>x.URUNID);

            foreach (var item in que)
            {
                var ts = db.TABLESIPARIS.Find(item);
                ts.URUN_ADEDİ = 0;
            }
            db.SaveChanges();
            db.SIPARISLER.Remove(s);
            foreach (var item in que)
            {
                var ts = db.TABLESIPARIS.Find(item);
                db.TABLESIPARIS.Remove(ts);
            }
            db.SaveChanges();
            var query = db.SIPARISLER.Where(x => x.CARİID == index).Select(x => new { x.BAŞLIK, x.TABLEMUSTERİ.AD, x.TABLEMUSTERİ.SOYAD, x.TUTAR });
            dataGridView1.DataSource = query.ToList();
            label2.Text = db.SIPARISLER.Where(x => x.CARİID == index && x.ONAY == null).Sum(x => x.TUTAR).ToString();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int no_index = dataGridView1.CurrentRow.Index;
            secilen_no = no_dizi[no_index].ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            cari c = new cari();
            c.Show();
        }
    }
}
