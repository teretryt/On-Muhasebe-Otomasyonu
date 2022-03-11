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
    public partial class stok : Form
    {
        public stok()
        {
            InitializeComponent();
        }

        MuhasebeEntities2 db = new MuhasebeEntities2();

        private void pros2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.SORGU();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                dataGridView1.DataSource = db.SORGU().Where(x => x.CINSADI == comboBox1.SelectedItem.ToString()).ToList();
                return;
            }
            dataGridView1.DataSource = db.SORGU().Where(x => x.CINSADI == comboBox1.SelectedItem.ToString() & x.URUNADI == comboBox2.SelectedItem.ToString()).ToList();
        }

        private void stok_Load(object sender, EventArgs e)
        {
            var cont = from item in db.TABLOKATEG
                       select item.CINSADI;
            comboBox1.DataSource = cont.ToList();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            stokeditor se = new stokeditor();
            se.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ciurn cu = new ciurn();
            cu.Show();
        }
    }
}
