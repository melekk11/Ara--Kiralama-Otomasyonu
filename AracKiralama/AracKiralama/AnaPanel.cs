using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AracKiralama
{
    public partial class AnaPanel : Form
    {
        public AnaPanel()
        {
            InitializeComponent();
        }

        //Kurumsal işlemlerin bulunduğu AnasayfaFRM adlı panele erişim için ilgili buton eventi eklendi.
        private void button1_Click(object sender, EventArgs e)
        {
            KurumsalPanel AnasayfaFRM = new KurumsalPanel(); //kurumsal panelin nesnesi oluşturuldu.
            AnasayfaFRM.ShowDialog();                        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MusteriGiris mgiris = new MusteriGiris();
            mgiris.ShowDialog();
           
        }

        private int imageNumber = 1;

        private void loadİmage()
        {
            if (imageNumber == 7)
            {
                imageNumber = 1;
            }
            pictureBox5.ImageLocation = string.Format(@"images\{0}.png", imageNumber);
            pictureBox5.BackgroundImageLayout = ImageLayout.Stretch;
            imageNumber++;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            loadİmage();
        }
    }
}
