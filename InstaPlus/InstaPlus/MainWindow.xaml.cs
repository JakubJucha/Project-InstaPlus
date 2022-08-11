using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PBD_działające__y_
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var oknoRejestracji = new Rejestracja();
            oknoRejestracji.ShowDialog();
            
        }


        private void btnZaloguj_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLoginLogowanie.Text;
            string haslo = txtHasloLogowanie.Password;

            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
              
               SqlCommand zbanowany = new SqlCommand($"SELECT COUNT(*) from Użytkownik WHERE login = '{login}' and czyZbanowany = '{1}'", polaczenie);
               int toZbanowanyCzyNie = (int)zbanowany.ExecuteScalar();
                polaczenie.Close();
                polaczenie.Open();
                SqlCommand logowanie = new SqlCommand($"SELECT * FROM Użytkownik WHERE login = '{login}' and hasło = '{haslo}'", polaczenie);
                SqlDataReader czytnik = logowanie.ExecuteReader();
                if (toZbanowanyCzyNie == 0)
                {
                    if (czytnik.Read())
                    {
                        czytnik.Close();
                        instagram oknoGlowne = new instagram(login);
                        oknoGlowne.Show();
                        this.Close();

                    }
                    else
                        MessageBox.Show("Błędny login lub hasło!");
                }
                else { MessageBox.Show("To konto zostało zbanowane!"); }
            }


        }

     

        private void lblRejestracja_MouseEnter(object sender, MouseEventArgs e)
        {
            lblRejestracja.FontWeight = FontWeights.UltraBold;
        }

        private void lblRejestracja_MouseLeave(object sender, MouseEventArgs e)
        {
            lblRejestracja.FontWeight = FontWeights.Normal;
        }


        private void txtLoginLogowanie_GotFocus(object sender, RoutedEventArgs e)
        {
            txtLoginLogowanie.Text = "";
        }

        private void txtHasloLogowanie_GotFocus(object sender, RoutedEventArgs e)
        {
            txtHasloLogowanie.Password = "";
        }
    }
}
