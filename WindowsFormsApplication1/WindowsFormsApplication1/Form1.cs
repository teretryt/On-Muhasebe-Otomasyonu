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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MuhasebeEntities2 db = new MuhasebeEntities2();

        private void button1_Click(object sender, EventArgs e)
        {
            cari c = new cari();
            c.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("calc.exe");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stok s = new stok();
            s.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            yonetim y = new yonetim();
            y.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            satıs s = new satıs();
            s.Show();
        }

       
        //SqlConnection conn = new SqlConnection("Data Source=LG-BILGISAYAR;Initial Catalog=Muhasebe;Integrated Security=True");

    }
}
