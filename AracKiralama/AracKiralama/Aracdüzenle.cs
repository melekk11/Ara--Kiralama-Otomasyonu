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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AracKiralama
{
    public partial class Aracdüzenle : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        public Aracdüzenle()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";

            //ComboBox yapılarına veri ekleme 
            cbxMarka.Items.Add("OPEL");
            cbxMarka.Items.Add("MERCEDES-BENZ");
            cbxMarka.Items.Add("BMV");
            cbxMarka.Items.Add("RENOULT");
            cbxMarka.Items.Add("FORD");

            cbxYakit.Items.Add("Benzin");
            cbxYakit.Items.Add("Dizel");
            cbxYakit.Items.Add("LPG");
            cbxYakit.Items.Add("Elektrik");

            comboBox1.Items.Add("İzmir otogar");
            comboBox1.Items.Add("İzmir havalimanı");
            comboBox1.Items.Add("Ankara otogar");
            comboBox1.Items.Add("Ankara havalimanı");
            comboBox1.Items.Add("İstanbul otogar");
            comboBox1.Items.Add("İstanbul havalimanı");

        }

        private void Aracdüzenle_Load(object sender, EventArgs e)
        {  //aracdüzenle adlı panel yüklenirken otomatik olarak çalışan metoddur.
            Arac_Listele();
        }
      
        public void Arac_Listele()
        {
            cn.Open();
            cm = new SqlCommand("select * from araclar5 ", cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cn.Close();
        }

        public void Arac_Guncelle()
        {
            //Araç bilgilerinin güncelleneceği metod 
            cn.Open();
            cm = new SqlCommand("Update Araclar5 set Marka=@marka,Seri=@seri,Model=@model,Renk=@renk,KM=@km,Yakİt=@yakit,KiraUcret=@ücret,Vites=@vites where Plaka=@plaka", cn);
            cm.Parameters.AddWithValue("@plaka", txtPlaka.Text);
            cm.Parameters.AddWithValue("@marka", cbxSeri.Text);
            cm.Parameters.AddWithValue("@seri",  cbxSeri.Text);
            cm.Parameters.AddWithValue("@model", txtModel.Text);
            cm.Parameters.AddWithValue("@renk",  txtRenk.Text);
            cm.Parameters.AddWithValue("@km",    txtKm.Text);
            cm.Parameters.AddWithValue("@yakit", cbxYakit.Text);
            cm.Parameters.AddWithValue("@ücret", txtÜcret.Text);
            cm.Parameters.AddWithValue("@konum", comboBox1.Text);
            cm.Parameters.AddWithValue("@vites", txtvites.Text);

            cm.ExecuteNonQuery();
            cn.Close();
            Arac_Listele();//güncelleme işleminden sonra güncel veriler tekrardan çekilir
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            //seçilen plaka değeri delete komutuna parametre olarak verilerek sime işlemi sağlanır.
                cn.Open();
                cm = new SqlCommand("Delete from Araclar5 where Plaka='" + dataGridView1.CurrentRow.Cells["Plaka"].Value.ToString() + "'", cn);
                cm.ExecuteNonQuery();
                cn.Close();
                Arac_Listele();//güncelleme işleminden sonra güncel veriler tekrardan çekilir
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
                Arac_Guncelle();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {//dataGridView1_CellContentClick: dataGridView1 nesnesinde tıklama olayını tutan metodur.

            //dataGridView1.CurrentRow; tıklanan satırı tutan yapıdır.
            //dataGridView1.CurrentRow.Cells[1].Value sayseinde 1.sütunun değeri alınır
            txtPlaka.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            cbxMarka.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            cbxSeri.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtModel.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            txtRenk.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            txtKm.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            cbxYakit.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            txtÜcret.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[11].Value.ToString();
            txtvites.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString();
         
        }

        private void cbxMarka_SelectedIndexChanged(object sender, EventArgs e)
        {/*Marka değerini tutan combobox değeri değiştirilince Seri verilerini tutan
           ComboBox un içeriği de dinamik olarak değiştirecektir
         */
            cbxSeri.Items.Clear(); //seçim değeri sıfırlanır 
            if (cbxMarka.SelectedItem == "OPEL")
            {//Marka değeri opel seçilmişse cbxSeri yapısına opel markasının modelleri eklenir. 
                cbxSeri.Items.Add("Corsa");
                cbxSeri.Items.Add("Astra");
                cbxSeri.Items.Add("İnsignia");

            }
            else if (cbxMarka.SelectedItem == "MERCEDES-BENZ")
            {
                cbxSeri.Items.Add("A-200");
                cbxSeri.Items.Add("C63");
                cbxSeri.Items.Add("S-400");
            }
            else if (cbxMarka.SelectedItem == "BMV")
            {
                cbxSeri.Items.Add("M 520");
                cbxSeri.Items.Add("M4 competition");
                cbxSeri.Items.Add("M 63");
            }
            else if (cbxMarka.SelectedItem == "RENOULT")
            {
                cbxSeri.Items.Add("Megane 6");
                cbxSeri.Items.Add("Clıo");
                cbxSeri.Items.Add("Fluence");
            }
            else if (cbxMarka.SelectedItem == "FORD")
            {
                cbxSeri.Items.Add("courier");
                cbxSeri.Items.Add("GT-500");
                cbxSeri.Items.Add("Mustang");
            }
        }
    }
}
