using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AracKiralama
{
    public partial class Rezervasyon : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        public String getTc;
        public Rezervasyon()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";
        }

        private void Rezervasyon_Load(object sender, EventArgs e)
        {
            rezerv();
        }

        public void rezerv()
        {
            DateTime bugun = DateTime.Now; // DateTime.Now komutu anlık tarih bilgisini verir.
            cn.Open();
            /* DönüsTarihi > @bugun koşulu sayesinde ileriki döneme ait işlemlerin rezervasyon 
             * bilgisi olarak alınması sağlanmıştır.
            */
            cm = new SqlCommand("select * from Sözlesme where tcno = @getTc and DönüsTarihi > @bugun ", cn);
            cm.Parameters.AddWithValue("@getTc", getTc);
            cm.Parameters.AddWithValue("@bugun", bugun);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.BackgroundColor = SystemColors.ActiveCaption;
            cn.Close();
        }

     
    }
}
