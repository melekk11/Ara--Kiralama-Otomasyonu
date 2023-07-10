using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace AracKiralama
{
    public partial class kartlarım : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        SqlDataReader dr2;
        public string getTc;
        public string getİsim;
        public string kno;

        public kartlarım()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";
         
        }
        public void Get_kart()
        {/*getTc değişkeninden gelen değere göre select işlemi yaılmıştır.
          * Bu sayede ilgili tc numarasına kayıtlı tüm kartlar dr okuyucusuna çekilmiştir.
          */

            cn.Open();
            cm = new SqlCommand("select * from kart where Tcno = @tcno ", cn);
            cm.Parameters.AddWithValue("@tcno", getTc);
            dr = cm.ExecuteReader();

            if (dr.Read() == false)
            {//eğer tc bilgisine kayıtlı kart bulunmuyor ise panel dinamik olarak değiştirilir
          
                label4.Visible = false;
                comboBox1.Visible = false;
                panel1.Visible = false;
                panel2.Visible = false;
            }
            else
            {
                //kart bilgisi bulundu ise kart numarası comboBox1 nesnesine eklenir
                label3.Visible = false;
                    comboBox1.Items.Add(dr["KartNo"]);
            }
            cn.Close();
        }


        public void Get_data()//seçilen kart bilgilerinin getirilmesi
        {
            cn.Open();
            cm = new SqlCommand("select * from kart where KartNo = @kno ", cn);
            cm.Parameters.AddWithValue("@kno", comboBox1.SelectedItem.ToString());
            dr = cm.ExecuteReader();

            while (dr.Read())
            {
               //seçilen kart bilgileri kartlıktaki parametre alanlarına doldurulur 
                textBox1.Text = dr["KartNo"].ToString();
                txtbankaad.Text = dr["BankaAd"].ToString();
                txtskt.Text = dr["SSK"].ToString();
                txtcvv.Text = dr["CCV"].ToString();
                txtkartisim.Text = dr["Kartisim"].ToString();
            }
            cn.Close();
        }

        private void combokartno_SelectedIndexChanged(object sender, EventArgs e)
        {/*kart numarası seçimi değiştirildiğinde  Get_data(); metodu çağırılarak
         *kartlık alanı dinamik olarak değişmesi sağlanır */
            panel1.Visible = true;
            panel2.Visible = true;
            Get_data();
        }

        private void button1_Click(object sender, EventArgs e)  //KART EKLEME PANELİ
        { 
            KARTEKLE KARTEKLE = new KARTEKLE();
            KARTEKLE.setTc = getTc;
            KARTEKLE.setisim = getİsim;
            KARTEKLE.ShowDialog();
            if (KARTEKLE.setKno != null)
            {
                comboBox1.Items.Add(KARTEKLE.setKno);
            }
            comboBox1.Visible = true;
            label3.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)//kart sil butonu 
        {
            if (comboBox1.SelectedItem != null)
            {   
                cn.Open();
                cm = new SqlCommand("Delete from Kart where KartNo = @kno  ", cn);
                cm.Parameters.AddWithValue("@kno", comboBox1.SelectedItem);
                cm.ExecuteNonQuery();
                comboBox1.Items.Remove(comboBox1.SelectedItem);
                cn.Close();
                panel1.Visible = false; 
                panel2.Visible = false;
                MessageBox.Show("Kart bilgileri silindi.");
            }
            else
            {
                MessageBox.Show("Kart numarası seçilmedi.");
            }

            
           

        }
    }

   
}
