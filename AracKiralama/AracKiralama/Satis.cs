using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AracKiralama
{
    public partial class Satis : Form
    {

        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        public Satis()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";
        }

        private void Satis_Load_1(object sender, EventArgs e)
        {
            yükle(); //satış paneli yüklenirken satış verilerinin alındığı metod çağırılmıştır.
        }

        public void yükle()
        {
            cn.Open();
            cm = new SqlCommand("Select * From Satis", cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cn.Close();
        }

        private void button1_Click(object sender, EventArgs e)//silme butonu
        {
            /*dataGridView1.CurrentRow komutu ile tıklanan satır verileri alınmıştır.
             Alınan veriler delete komutunda kullanılmak üzre ilgili değişkenlere aatanmıştır.
             */
            var tcno = dataGridView1.CurrentRow.Cells["Tcno"].Value.ToString();
            var plaka = dataGridView1.CurrentRow.Cells["Plaka"].Value.ToString();
            var cikis = dataGridView1.CurrentRow.Cells["CikisTarihi"].Value;
            var donus = dataGridView1.CurrentRow.Cells["DönüsTarihi"].Value;

            cn.Open();
            cm = new SqlCommand("Delete from Satis where Tcno = @getTc and Plaka = @plaka and CikisTarihi = @cikis and DönüsTarihi = @donus ", cn);
            cm.Parameters.AddWithValue("@getTc", tcno);
            cm.Parameters.AddWithValue("@plaka", plaka);
            cm.Parameters.AddWithValue("@cikis", cikis);
            cm.Parameters.AddWithValue("@donus", donus);

            cm.ExecuteNonQuery();
            cn.Close();
            dataGridView1.Controls.Clear();
            yükle();//güncel veriler yüklr() metodu ile tekrardan alınmıştır.


        }
    }
}
