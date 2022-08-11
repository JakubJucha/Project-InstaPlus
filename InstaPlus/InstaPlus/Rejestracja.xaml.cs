using System;
using System.Collections.Generic;
using System.Data;
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

namespace PBD_działające__y_
{
    /// <summary>
    /// Logika interakcji dla klasy Rejestracja.xaml
    /// </summary>
    public partial class Rejestracja : Window
    {
        public Rejestracja()
        {
            InitializeComponent();
        }
        string Login;
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        private void btnZarejestruj_Click(object sender, RoutedEventArgs e)
        {
            string nazwaUz = txtNazwaUz.Text;
            string login = txtLogin.Text;
            string haslo = txtHaslo.Password;
            string hasloPotw = txtHasloPotw.Password;
            string imie = txtImie.Text;
            string nazwisko = txtNazwisko.Text;
            string email = txtEmail.Text;
            bool poprawnyEmail = IsValidEmail(email);

            if(txtHaslo.Password == "")
            {
                MessageBox.Show("Hasło nie może być puste!");
            }
            else if(!haslo.Equals(hasloPotw))
            {
                MessageBox.Show("Hasła nie są identyczne!");
                txtHaslo.Clear();
                txtHasloPotw.Clear();
            }
            else if(czyNiePuste(txtEmail) || czyNiePuste(txtImie) || czyNiePuste(txtLogin) || czyNiePuste(txtNazwaUz) || czyNiePuste(txtNazwisko))
            {
                MessageBox.Show("Żadne pole nie może być puste!");
            }
            else if(poprawnyEmail == false)
            {
                MessageBox.Show("Niepoprawny format adresu E-mail!");
            }
            else
            {
                using(SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False") )
                {
                    polaczenie.Open();

                    SqlCommand sprawdzeniePowtarzalnosci = new SqlCommand($"SELECT * FROM Użytkownik WHERE nazwaUżytkownika = '{nazwaUz}' OR login = '{login}' OR email = '{email}'", polaczenie);
                    SqlDataReader czytnik = sprawdzeniePowtarzalnosci.ExecuteReader();
                    if (czytnik.Read())
                    {
                        MessageBox.Show("Już istnieje taki użytkownik!\n Zmień dane i spróbuj ponownie!");
                    }
                    else
                    {
                        polaczenie.Close();
                        polaczenie.Open();
                        SqlCommand dodanieDoBazy = new SqlCommand("INSERT into Użytkownik(nazwaUżytkownika,login,hasło,imię,nazwisko,email) VALUES(@nazwa,@login,@haslo,@imie,@nazwi,@mail)", polaczenie);
                        dodanieDoBazy.Parameters.Add("nazwa", System.Data.SqlDbType.NVarChar).Value = nazwaUz;
                        dodanieDoBazy.Parameters.Add("login", System.Data.SqlDbType.NVarChar).Value = login;
                        dodanieDoBazy.Parameters.Add("haslo", System.Data.SqlDbType.NVarChar).Value = haslo;
                        dodanieDoBazy.Parameters.Add("imie", System.Data.SqlDbType.NVarChar).Value = imie;
                        dodanieDoBazy.Parameters.Add("nazwi", System.Data.SqlDbType.NVarChar).Value = nazwisko;
                        dodanieDoBazy.Parameters.Add("mail", System.Data.SqlDbType.NVarChar).Value = email;
                        dodanieDoBazy.ExecuteNonQuery();
                        polaczenie.Close();
                        Login = login;
                        nadanieDomyślnegoProfilowego();
                        this.Close();

                    }

                }
            }
            
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
        public void nadanieDomyślnegoProfilowego()
        {
            BitmapImage myBitmapImage = new BitmapImage();
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "profilowe.jpg");
            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            Uri resourceUri = new Uri(path, UriKind.Relative);
            BitmapImage profilowe = new BitmapImage(resourceUri); 
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
               
                polaczenie.Open();
                string polecenie = $"UPDATE Użytkownik SET profilowe = @obrazek WHERE login = '{Login}' ";

                SqlCommand dodanieZdjecia = new SqlCommand(polecenie, polaczenie);
                SqlParameter imageParameter = dodanieZdjecia.Parameters.Add("@obrazek", SqlDbType.Binary);
                var content = BitmapImage2Bitmap(profilowe);
                imageParameter.Value = content;
                imageParameter.Size = content.Length;
                dodanieZdjecia.ExecuteNonQuery();
                polaczenie.Close();
            }
            this.Close();
        }
        public bool czyNiePuste(TextBox txt)
        {
            if (txt.Text == "")
                return true;
            else
                return false;
        }

        
        void Skasowanie(TextBox T)
        {
            T.Text = "";
        }

        private void txtNazwaUz_GotFocus(object sender, RoutedEventArgs e)
        {
            Skasowanie(txtNazwaUz);
        }

        private void txtLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            Skasowanie(txtLogin);
        }

        private void txtHaslo_GotFocus(object sender, RoutedEventArgs e)
        {
            txtHaslo.Password = "";
        }

        private void txtHasloPotw_GotFocus(object sender, RoutedEventArgs e)
        {
            txtHasloPotw.Password = "";
        }

        private void txtImie_GotFocus(object sender, RoutedEventArgs e)
        {
            Skasowanie(txtImie);
        }

        private void txtNazwisko_GotFocus(object sender, RoutedEventArgs e)
        {
            Skasowanie(txtNazwisko);
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            Skasowanie(txtEmail);
        }
    }
}
