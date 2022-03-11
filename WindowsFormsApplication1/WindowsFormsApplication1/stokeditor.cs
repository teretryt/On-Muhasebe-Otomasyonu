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
    public partial class stokeditor : Form
    {
        public stokeditor()
        {
            InitializeComponent();
        }

        MuhasebeEntities2 db = new MuhasebeEntities2();

        private void stokeditor_Load_1(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.SORGU();
            var cont = from item in db.TABLOKATEG
                       select item.CINSADI;
            comboBox1.DataSource = cont.ToList();
            comboBox3.DataSource = cont.ToList();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (textBox3.Text.Contains(" "))
            {
                MessageBox.Show("Arama çubuğu boş karakterle doldurulamaz.");
                return;
            }
            if (comboBox2.SelectedIndex == 0 && textBox3.Text == "")
            {
                dataGridView1.DataSource = db.SORGU().Where(x => x.CINSADI == comboBox1.SelectedItem.ToString()).ToList();
                return;
            }
            else if (textBox3.Text.Length >= 1)
            {
                string isim = db.URUNİSİMLERİ.FirstOrDefault(x => x.URUNADI.Contains(textBox3.Text.ToUpper())).URUNADI;
                dataGridView1.DataSource = db.SORGU().Where(x => x.URUNADI == isim ).ToList();
                return;
            }
            dataGridView1.DataSource = db.SORGU().Where(x => x.CINSADI == comboBox1.SelectedItem.ToString() & x.URUNADI == comboBox2.SelectedItem.ToString()).ToList();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int index_1 = comboBox3.Items.IndexOf(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            comboBox3.SelectedIndex = index_1;
            int index = comboBox4.Items.IndexOf(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            comboBox4.SelectedIndex = index;
            textBox2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();           
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            var aranan = comboBox1.SelectedItem.ToString();
            var cont = from item in db.URUNİSİMLERİ
                       where aranan == item.TABLOKATEG.CINSADI
                       select item.URUNADI;
            foreach (var item in cont)
            {
                comboBox2.Items.Add(item);
            }
            comboBox2.Items.Insert(0, "HEPSİ");
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string aranan = comboBox4.SelectedItem.ToString();
            var que = db.URUNİSİMLERİ.SingleOrDefault(x=> x.URUNADI == aranan);
            int adet = Convert.ToInt32(textBox2.Text);
            var que_1 = db.URUNİSİMLERİ.Find(que.URUNADI_ID);
            que_1.STOK_ADEDİ = adet;
            db.SaveChanges();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            var aranan = comboBox3.SelectedItem.ToString();
            var cont = from item in db.URUNİSİMLERİ
                       where aranan == item.TABLOKATEG.CINSADI
                       select item.URUNADI;
            foreach (var item in cont)
            {
                comboBox4.Items.Add(item);
            }
            comboBox4.SelectedIndex = 0;
        }
    }
}
