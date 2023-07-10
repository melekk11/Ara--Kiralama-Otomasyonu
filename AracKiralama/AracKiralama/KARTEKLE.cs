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
    public partial class KARTEKLE : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        public string setTc;
        public string setisim;
        public string setKno;
        public KARTEKLE()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = "Data Source=localhost;Initial Catalog=AracKiralama;Integrated Security=True";

        }

        private void button1_Click(object sender, EventArgs e)
        {//tüm parametre alanların dolu olması and işlemi ile sağlanıf if bloğuna koşul olarak verilmiştir
            if (textBox7.Text != "" && 
                textBox1.Text != "" && 
                textBox6.Text != "" && 
                textBox3.Text != "" &&
                setisim       != "" &&
                textBox5.Text != "" &&
                setTc         != "" )
            {
                cn.Open();
                cm = new SqlCommand("Insert Into kART Values (@KARTNO, @SSK, @CCV, @SİFRE, @KARTİSİM, @BANKAAD,@BAKİYE , @TCNO   )", cn);
                cm.Parameters.AddWithValue("@KARTNO",   textBox7.Text);
                cm.Parameters.AddWithValue("@SSK"   ,   textBox1.Text);
                cm.Parameters.AddWithValue("@CCV",      textBox6.Text);
                cm.Parameters.AddWithValue("@SİFRE",    textBox3.Text);
                cm.Parameters.AddWithValue("@KARTİSİM", setisim);
                cm.Parameters.AddWithValue("@BANKAAD",  textBox5.Text);
                cm.Parameters.AddWithValue("@BAKİYE",   10000);
                cm.Parameters.AddWithValue("@TCNO",     setTc);

                cm.ExecuteNonQuery();
                cn.Close();
                setKno = textBox7.Text;
                MessageBox.Show("Kart başarılı bir şekilde kaydedildi.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Boş alanları doldurun.");
            }


        }
    }
}
