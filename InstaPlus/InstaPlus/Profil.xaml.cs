using Microsoft.Win32;
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
using System.Drawing;
using System.Data;


namespace PBD_działające__y_
{
    /// <summary>
    /// Logika interakcji dla klasy Profil.xaml
    /// </summary>
    public partial class Profil : Window
    {
        public string Login { set; get; }
        public string LoginZalogowanego { set; get; }
        public bool czyObserwujesz = false;
        public bool czyNastepnyProfil = false;
        public Profil()
        {
            InitializeComponent();
        }

        private static BitmapImage ConvertByteToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public void kimJestem(string login)
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();

                SqlCommand czyModerator = new SqlCommand($"SELECT moderator from Użytkownik WHERE login = '{login}'", polaczenie);
                bool? czyMod = (bool?)czyModerator.ExecuteScalar();
                if (czyMod == true)
                {
                    reklama3.Visibility = Visibility.Hidden;
                }
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand czyVip = new SqlCommand($"SELECT vip from Użytkownik WHERE login = '{login}'", polaczenie);
                bool? VIP = (bool?)czyVip.ExecuteScalar();
                if (VIP == true && czyMod == false)
                {
                    reklama3.Visibility = Visibility.Hidden;
                }
                polaczenie.Close();

                if (czyMod == false && VIP == false)
                {
                    reklama3.Visibility = Visibility.Visible;

                }
            }
        }
        public Profil(string login) : this()
        {
            this.Login = login;
            kimJestem(login);
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand imieee = new SqlCommand($"SELECT imię from Użytkownik WHERE login = '{Login}'", polaczenie);
                string imie = (string)imieee.ExecuteScalar();
                polaczenie.Close();
                polaczenie.Open();
                SqlCommand nazwiskoo = new SqlCommand($"SELECT nazwisko from Użytkownik WHERE login = '{Login}'", polaczenie);
                string nazwisko = (string)nazwiskoo.ExecuteScalar();
                polaczenie.Close();
                lblImieNazwiskoNaProfilu.Content = $"{imie} {nazwisko}";
                polaczenie.Open();
                SqlCommand nazwaa = new SqlCommand($"SELECT nazwaUżytkownika from Użytkownik WHERE login = '{Login}'", polaczenie);
                string nazwa = (string)nazwaa.ExecuteScalar();
                polaczenie.Close();
                lblNzwaUzNaProfilu.Content = $"{nazwa}";
                polaczenie.Open();

                SqlCommand bio = new SqlCommand($"SELECT bio from Użytkownik WHERE login = '{Login}'", polaczenie);
                try
                {
                    string opis = (string)bio.ExecuteScalar();
                    lblBio.Text = opis;
                }
                catch (Exception) { }
                polaczenie.Close();
                polaczenie.Open();

                try
                {
                    SqlCommand zdjecie = new SqlCommand($"SELECT profilowe from Użytkownik WHERE login = '{Login}'", polaczenie);
                    byte[] profilowe = (byte[])zdjecie.ExecuteScalar();
                    imgZdjecie.Source = ConvertByteToBitmapImage(profilowe);
                    
                }
                catch (Exception) { }
                polaczenie.Close();
                polaczenie.Open();
                try
                {
                    SqlCommand iloscOsobKtoreObserwujaProfil = new SqlCommand($"SELECT COUNT(*) from Obserwujący where idUżytkownika = {getId(Login)}", polaczenie);
                    int ilosc = (int)iloscOsobKtoreObserwujaProfil.ExecuteScalar();
                    lblIlośćOsóbKtóreObserwująTenProfil.Content = $"Obserwujący: {ilosc}";
                    
                }catch(Exception ex) { lblIlośćOsóbKtóreObserwująTenProfil.Content = $"Obserwujący: 0"; }
                polaczenie.Close();
            }
        }
        public Profil(string login, string loginzalogowanego) : this(login)
        {
            this.LoginZalogowanego = loginzalogowanego;
            kimJestem(LoginZalogowanego);
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand Obserwowany = new SqlCommand($"SELECT COUNT(*) from Obserwowani where IdObserwującego ='{getId(Login)}' and idUżytkownika = '{getId(loginzalogowanego)}'", polaczenie);
                int obserwowany = (int)Obserwowany.ExecuteScalar();
                if (obserwowany == 0)
                    czyObserwujesz = false;
                else czyObserwujesz = true;
                polaczenie.Close();
            }
            if(czyObserwujesz == false)
            {
                btnObserwuj.Content = "Obserwuj";
                btnObserwuj.Background = System.Windows.Media.Brushes.Gray;
            }
            else
            {
                btnObserwuj.Content = "Obserwujesz";
                btnObserwuj.Background = System.Windows.Media.Brushes.Green;
            }
        }

        private void btnDodajZdj_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
           
            open.Filter = "Image Files (*.JPG)|*.JPG|Image Files (*PNG)|*.PNG";

            BitmapImage bitmapa = new BitmapImage();
            if (open.ShowDialog() == true)
            {
                bitmapa.BeginInit();
                bitmapa.UriSource = new Uri(open.FileName);
                bitmapa.EndInit();
            }
            var zmianaProfilowki = new ZmianaProfilówki(bitmapa, Login);
            zmianaProfilowki.ShowDialog();
            
   
                if (zmianaProfilowki.czyTAK)
            {
                imgZdjecie.Source = bitmapa;
            }
            
        }

        private void btnWrocNaGłówną_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        private void btnBio_Click(object sender, RoutedEventArgs e)
        {
            var oknoZmianyBio = new ZmianaBio(Login);
            oknoZmianyBio.ShowDialog();

            if(oknoZmianyBio.czyZmieniamy)
            {
                lblBio.Text = oknoZmianyBio.NoweBio;
            }
        }

        public int getId(String login)
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand getid = new SqlCommand($"Select IdUżytkownika from Użytkownik where login = '{login}'", polaczenie);
                int id = (int)getid.ExecuteScalar();
                polaczenie.Close();
                return id;
            }
        }

        private void btnObserwuj_Click(object sender, RoutedEventArgs e)
        {
            if(czyObserwujesz == false)
            {
                btnObserwuj.Content = "Obserwujesz";
                btnObserwuj.Background = System.Windows.Media.Brushes.Green;
                czyObserwujesz = true;
                using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    polaczenie.Open();
                    SqlCommand Obserwowany = new SqlCommand("INSERT INTO Obserwowani(idUżytkownika,idObserwującego)VALUES(@użytkownik,@obserwowany)", polaczenie);
                    Obserwowany.Parameters.Add("użytkownik", System.Data.SqlDbType.Int).Value = getId(LoginZalogowanego);
                    Obserwowany.Parameters.Add("obserwowany", System.Data.SqlDbType.Int).Value = getId(Login);
                    Obserwowany.ExecuteNonQuery();
                    polaczenie.Close();
                    polaczenie.Open();
                    SqlCommand Obserwujący = new SqlCommand("INSERT INTO Obserwujący(idUżytkownika,idObserwatora)VALUES(@użytkownik,@obserwator)", polaczenie);
                    Obserwujący.Parameters.Add("użytkownik", System.Data.SqlDbType.Int).Value = getId(Login);
                    Obserwujący.Parameters.Add("obserwator", System.Data.SqlDbType.Int).Value = getId(LoginZalogowanego);
                    Obserwujący.ExecuteNonQuery();
                    polaczenie.Close();
                }
            }
            else
            {
                czyObserwujesz = false;
                btnObserwuj.Content = "Obserwuj";
                btnObserwuj.Background = System.Windows.Media.Brushes.Gray;
                using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    polaczenie.Open();
                    SqlCommand CofniecieObserwacji = new SqlCommand($"DELETE FROM Obserwowani where idObserwującego = '{getId(Login)}' and idUżytkownika = '{getId(LoginZalogowanego)}'", polaczenie);
                    CofniecieObserwacji.ExecuteNonQuery();
                    polaczenie.Close();
                    polaczenie.Open();
                    SqlCommand CofniecieObseracji2 = new SqlCommand($"DELETE FROM Obserwujący where idObserwatora = '{getId(LoginZalogowanego)}' and idUżytkownika = '{getId(Login)}'", polaczenie);
                    CofniecieObseracji2.ExecuteNonQuery();
                    polaczenie.Close();
                }
            }
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                    SqlCommand iloscOsobKtoreObserwujaProfil = new SqlCommand($"SELECT COUNT(*) from Obserwujący where idUżytkownika = {getId(Login)}", polaczenie);
                    int ilosc = (int)iloscOsobKtoreObserwujaProfil.ExecuteScalar();
                    lblIlośćOsóbKtóreObserwująTenProfil.Content = $"Obserwujący: {ilosc}";
                polaczenie.Close();
                
            }
        }

        private void btnNastepnyUzytkownikZTagiem_Click(object sender, RoutedEventArgs e)
        {
            czyNastepnyProfil = true;
            this.Close();
        }
    }
}
