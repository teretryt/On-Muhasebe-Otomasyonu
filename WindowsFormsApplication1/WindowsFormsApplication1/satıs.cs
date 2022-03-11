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
    public partial class satıs : Form
    {
        public satıs()
        {
            InitializeComponent();
        }

        MuhasebeEntities2 db = new MuhasebeEntities2();
        public int m_id = 0;
        public int[] sip_id;
        public double tt;

        private void satıs_Load(object sender, EventArgs e)
        {
            var query = db.TABLEMUSTERİ.Select(x => new { 
                x.ID,
                x.AD,
                x.SOYAD,
                x.TELEFON
            });
            dataGridView1.DataSource = query.ToList();
            aktif();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var query = db.TABLEMUSTERİ.Where(x => (x.AD + " " + x.SOYAD).Contains(textBox1.Text)).Select(x => new
            {
                x.ID,
                x.AD,
                x.SOYAD,
                x.TELEFON
            });
            dataGridView1.DataSource = query.ToList();
            aktif();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.ColumnCount == 3)
            {
                int datagrid_index = dataGridView1.CurrentRow.Index;
                double tutar = (double)dataGridView1.CurrentRow.Cells[2].Value;
                int secilecek_siparis = sip_id[datagrid_index];
                string siparis_no = db.SIPARISLER.Single(x => x.CARİID == m_id && x.SIPARISID == secilecek_siparis).NO;
                var item = db.SIPARISLER.Find(siparis_no);
                var müs = db.TABLEMUSTERİ.Find(item.CARİID);


                item.TUTAR = tt;
                müs.ALACAK -= tt;
                if (müs.ALACAK < 0)
                {
                    müs.BAKİYE += (double)-müs.ALACAK;
                    müs.ALACAK = 0;
                }
                if (müs.BAKİYE < 0)
                {
                    müs.ALACAK += (double)-müs.BAKİYE;
                    müs.BAKİYE = 0;
                }
                item.ONAY = true;
                db.SaveChanges();    
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            aktif();
            if (dataGridView1.ColumnCount == 3) 
            {
                textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                return;
            }
            if (dataGridView1.ColumnCount == 4)
            {
                m_id = (int)dataGridView1.CurrentRow.Cells[0].Value;
                var que = db.SIPARISLER.Where(x => x.ONAY == null && x.CARİID == (int)m_id).Select(x => new
                {
                    x.BAŞLIK,
                    ADSOYAD = x.TABLEMUSTERİ.AD + " " + x.TABLEMUSTERİ.SOYAD,
                    x.TUTAR
                });
                sip_id = db.SIPARISLER.Where(x => x.ONAY == null && x.CARİID == (int)m_id).Select(x => x.SIPARISID).ToArray();
                dataGridView1.DataSource = que.ToList();
            }
        }

        private void aktif()
        {
            if (dataGridView1.ColumnCount == 3)
            {   
                foreach (RadioButton item in groupBox1.Controls.OfType<RadioButton>().ToList())
                {
                    item.Enabled = true;
                    button1.Enabled = true;
                }
            }
            else if (dataGridView1.ColumnCount > 3)
            {
                foreach (RadioButton item in groupBox1.Controls.OfType<RadioButton>().ToList())
                {
                    item.Enabled = false;
                    button1.Enabled = false;
                }
            }
        }


        //%1 KDV
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            double tutar = Convert.ToDouble(textBox2.Text);
            double kdv = (tutar * 1) / 100;

            double toplam_tutar = tutar + kdv;
            label6.Text = toplam_tutar.ToString();
            tt = toplam_tutar;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            double tutar = Convert.ToDouble(textBox2.Text);
            double kdv = (tutar * 8) / 100;

            double toplam_tutar = tutar + kdv;
            label6.Text = toplam_tutar.ToString();
            tt = toplam_tutar;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            double tutar = Convert.ToDouble(textBox2.Text);
            double kdv = (tutar * 18) / 100;

            double toplam_tutar = tutar + kdv;
            label6.Text = toplam_tutar.ToString();
            tt = toplam_tutar;
        }
    }
}
