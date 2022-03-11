using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Configuration;

namespace WindowsFormsApplication1
{
    public partial class cari : Form
    {
        public cari()
        {
            InitializeComponent();
        }

        //KODLAR
        #region
        MuhasebeEntities2 me = new MuhasebeEntities2();
        Random rd = new Random();
        public static int m_index;
        public static string m_ad,m_soyad;
        //SqlConnection conn = new SqlConnection("Data Source=LG-BILGISAYAR;Initial Catalog=Muhasebe;Integrated Security=True");
        
        
        //müşteri listeleme ✓
        private void button4_Click(object sender, EventArgs e)
        {
            var musteriler = from item in me.TABLEMUSTERİ
                             select new
                             {
                                 item.ID,
                                 item.AD,
                                 item.SOYAD,
                                 item.TELEFON,
                                 item.ALACAK,
                                 item.BAKİYE
                             };
            dataGridView1.DataSource = musteriler.ToList();
        }


        //form yuklendiğinde combobox a urun cinslerini aktarma ✓
        private void cari_Load(object sender, EventArgs e)
        {

        }


        //buton aktfiliği
        void Aktif() 
        {
            //aktif et
            button2.Enabled = true;
            button3.Enabled = true;
            button6.Enabled = true;
        }


        //datagrid verilerini form kontrollerine aktarma işlemi ✓
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Aktif();
            m_index = (int)dataGridView1.CurrentRow.Cells[0].Value;

            if (dataGridView1.ColumnCount == 6)
            {
                textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                textBox7.Text = Convert.ToString(0);
                m_ad = textBox1.Text;
                m_soyad = textBox2.Text;
            }
           }


        //müşteri ekleme ✓
        private void button1_Click(object sender, EventArgs e)
        {
            TABLEMUSTERİ t = new TABLEMUSTERİ();
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Lütfen gereken alanları doldurunuz.");
                return;
            }
            t.AD = textBox1.Text.ToUpper();
            t.SOYAD = textBox2.Text.ToUpper();
            t.TELEFON = textBox3.Text;
            if (string.IsNullOrEmpty(textBox7.Text) || string.IsNullOrWhiteSpace(textBox7.Text))
            {
                t.ALACAK = 0;
            }
            else
            {
                t.ALACAK = double.Parse(textBox7.Text);
            }
            t.BAKİYE = 0;
            me.TABLEMUSTERİ.Add(t);
            me.SaveChanges(); 
        }


        //müşteri silme ✓
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("BU SEÇENEK MÜŞTERİYE AİT TÜM SİPARİŞLERİDE SİLER", "DİKKAT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                    int id = (int)dataGridView1.CurrentRow.Cells[0].Value;
                    var f2 = me.TABLEMUSTERİ.Find(id);
                    var que = from item in me.TABLESIPARIS
                              where item.MUSTERI == id
                              select item.URUNID;
                    var que2 = from item in me.SIPARISLER
                               where item.CARİID == id
                               select item.NO;

                    int[] dizi = que.ToArray();
                    string[] dizi2 = que2.ToArray();

                    foreach (var item in dizi2)
                    {
                        var f1 = me.SIPARISLER.Find(item);
                        me.SIPARISLER.Remove(f1);
                    }
                    foreach (var item in dizi)
                    {
                        var f1 = me.TABLESIPARIS.Find(item);
                        me.TABLESIPARIS.Remove(f1);
                    }
                    me.TABLEMUSTERİ.Remove(f2);
                    me.SaveChanges();
            }
            else
            {
                return;
            }
           
        }


        //güncelleme ✓
        private void button3_Click(object sender, EventArgs e)
        {
                int id = (int)dataGridView1.CurrentRow.Cells[0].Value;
                var f1 = me.TABLEMUSTERİ.Find(id);
                f1.AD = textBox1.Text.ToUpper();
                f1.SOYAD = textBox2.Text.ToUpper();
                f1.TELEFON = textBox3.Text;
                double alacak = Convert.ToDouble(textBox7.Text);
                if (alacak + f1.ALACAK > f1.BAKİYE)
                {
                    f1.ALACAK += alacak;
                    f1.ALACAK = f1.ALACAK - f1.BAKİYE;
                    f1.BAKİYE = 0;
                }
                else if(alacak + f1.ALACAK < f1.BAKİYE)
                {
                    f1.BAKİYE -= alacak + f1.ALACAK;
                    f1.ALACAK = 0;
                }
                me.SaveChanges();
        }


        //sipariş ekleme ✓
        private void button6_Click(object sender, EventArgs e)
        {
            SIPARISOLUSTUR f = new SIPARISOLUSTUR();
            f.Show();
        }


        //müşteri siparişleri listeleme
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var query = me.SIPARISLER.Where(x => x.CARİID == m_index && x.ONAY == null).Select(x => new { x.BAŞLIK, x.TABLEMUSTERİ.AD, x.TABLEMUSTERİ.SOYAD, x.TUTAR });
            if (query.ToList().Count == 0)
            {
                MessageBox.Show("Bu müşterinin herhangi bir sipariş kaydı bulunmamaktadır...");
                return;
            }
            this.Hide();
            sıparısmenu s = new sıparısmenu();
            s.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sıparısmenu sm = new sıparısmenu();
            sm.Show();
        }

    }
        #endregion 
}



