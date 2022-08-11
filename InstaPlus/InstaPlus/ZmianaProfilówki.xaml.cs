using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Data;

namespace PBD_działające__y_
{
    /// <summary>
    /// Logika interakcji dla klasy ZmianaProfilówki.xaml
    /// </summary>
    public partial class ZmianaProfilówki : Window
    {
        public BitmapImage Profilowe {get;set;}
        public string Login { set; get; }

        public bool czyTAK;
        public ZmianaProfilówki()
        {
            InitializeComponent();
        }

        public ZmianaProfilówki(BitmapImage zdjecie, string login):this()
        {
            imgZdjeciePotwierdzeniei.Source = zdjecie;
            this.Profilowe = zdjecie;
            this.Login = login;
        }

        private void btnNIE_Click(object sender, RoutedEventArgs e)
        {
            czyTAK = false;
            this.Close();
        }

       
        private byte[] BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                return outStream.ToArray();
            }
        }

        private void btnTAK_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                czyTAK = true;
                polaczenie.Open();
                string polecenie = $"UPDATE Użytkownik SET profilowe = @obrazek WHERE login = '{Login}' ";

                SqlCommand dodanieZdjecia = new SqlCommand(polecenie, polaczenie);
                SqlParameter imageParameter = dodanieZdjecia.Parameters.Add("@obrazek", SqlDbType.Binary);
                var content = BitmapImage2Bitmap(Profilowe);
                imageParameter.Value = content;
                imageParameter.Size = content.Length;
                dodanieZdjecia.ExecuteNonQuery();
                polaczenie.Close();
            }
            this.Close();
        }
    }
}
