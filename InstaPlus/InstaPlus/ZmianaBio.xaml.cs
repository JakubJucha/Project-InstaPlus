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
using System.Windows.Shapes;

namespace PBD_działające__y_
{
    /// <summary>
    /// Logika interakcji dla klasy ZmianaBio.xaml
    /// </summary>
    public partial class ZmianaBio : Window
    {

        public string Login { set; get; }
        public bool czyZmieniamy;
        public string NoweBio { set; get; }
        public ZmianaBio()
        {
            InitializeComponent();
        }

        public ZmianaBio(string uzytkownik):this()
        {
            this.Login = uzytkownik;
        }

        private void btnAnuluj_Click(object sender, RoutedEventArgs e)
        {
            czyZmieniamy = false;
            this.Close();
        }

        public List<string> WykryjTag()
        {
            string trescOpisu = txtBio.Text;
            List<string> tagi = new List<string>();
            string tag = "";
            char znak = 'x';
            int i = 0;

            while (true)
            {
                if (i == trescOpisu.Length - 1) break;
                try
                {
                     while (znak != '#')
                     {
                    
                        znak = trescOpisu[i];
                        i++;
                        if (i == trescOpisu.Length - 1) break;
                   
                     }
                }
                catch (Exception ex) { break; }
                if (znak == '#')
                {
                    while (znak != ' ' || znak != '\n' || znak != '\t')
                    {
                        znak = trescOpisu[i];
                        if (znak == ' ' || znak == '\n' || znak == '\t') { tagi.Add(tag); tag = ""; break; }
                        else if (i == trescOpisu.Length - 1) { tag = tag + znak; tagi.Add(tag); tag = ""; break; }
                        tag = tag + znak;
                        i++;
                    }
                }
            }
            return tagi;
        }
        public int getId(string login)
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
        private void btnZapiszBio_Click(object sender, RoutedEventArgs e)
        {
            List<string> tagiWPoscie = WykryjTag();
            int licznikTagow = tagiWPoscie.Count;

            string noweBio = txtBio.Text;
            NoweBio = noweBio;
            czyZmieniamy = true;
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand usuniecieStarychTAgow = new SqlCommand($"DELETE from Tagi where idTagu = '{getId(Login)}'", polaczenie);
                usuniecieStarychTAgow.ExecuteNonQuery();
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand dodanieBio = new SqlCommand($"UPDATE Użytkownik SET bio = '{noweBio}' WHERE login = '{Login}' ", polaczenie);
                dodanieBio.ExecuteNonQuery();
                polaczenie.Close();

                if (licznikTagow > 0)
                {
                    polaczenie.Open();
                    SqlCommand pobranieIdUzytkownikaDoKtoregoPrzypisanyJestTag = new SqlCommand($"SELECT IdUżytkownika from Użytkownik where login = '{Login}'", polaczenie);
                    int idUzytkownikaZTagiem = Convert.ToInt32(pobranieIdUzytkownikaDoKtoregoPrzypisanyJestTag.ExecuteScalar());
                    polaczenie.Close();

                    for (int i = 0; i < licznikTagow; i++)
                    {
                        string trescTaga = tagiWPoscie[i].ToString();
                        polaczenie.Open();
                        SqlCommand dodanieTagaDoBazy = new SqlCommand("INSERT into Tagi(idTagu,treść,użytkownik) VALUES(@idtagu,@tresc,@uz)", polaczenie);
                        dodanieTagaDoBazy.Parameters.Add("idtagu", System.Data.SqlDbType.Int).Value = idUzytkownikaZTagiem;
                        dodanieTagaDoBazy.Parameters.Add("tresc", System.Data.SqlDbType.VarChar).Value = trescTaga;
                        dodanieTagaDoBazy.Parameters.Add("uz", System.Data.SqlDbType.Bit).Value = 1;
                        dodanieTagaDoBazy.ExecuteNonQuery();
                        polaczenie.Close();
                    }
                }
            }
            this.Close();
        }
    }
}
