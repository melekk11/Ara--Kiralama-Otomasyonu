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

namespace AracKiralama
{
    public partial class ÜyeGeçmiş : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        public String getTc;
        public ÜyeGeçmiş()
        {
     
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";

        }
        private void ÜyeGeçmiş_Load(object sender, EventArgs e)
        {
            gecmiş();

        }

        public void gecmiş() {
            cn.Open();
            cm = new SqlCommand("select * from Satis where Tcno = @getTc  ", cn);
            cm.Parameters.AddWithValue("@getTc", getTc);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.BackgroundColor = SystemColors.ActiveCaption;
            cn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //seçili satış işleminin sislinmesi için satış id değeri alınır 
            var id = dataGridView1.CurrentRow.Cells["SatisID"].Value.ToString();
            cn.Open();
            //alınan id değeri üzerinde delete işlemi yapılarak seçili satırın silinmesi sağlanır.
            cm = new SqlCommand("Delete from Satis where SatisID = @id ", cn);
            cm.Parameters.AddWithValue("@id", id);
            cm.ExecuteNonQuery();
            cn.Close();
            gecmiş();
        }
    }
}
