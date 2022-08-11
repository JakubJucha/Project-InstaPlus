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
    /// Logika interakcji dla klasy ZapłataZaVIP.xaml
    /// </summary>
    public partial class ZapłataZaVIP : Window
    {

        public string Login { get; set; }
        public ZapłataZaVIP()
        {
            InitializeComponent();
        }


        public string rodzajSubskrypcji;
        public decimal cena;
        enum MetodaPlatnosci{ blik, karta};
        private MetodaPlatnosci metoda;

        public ZapłataZaVIP(KupowanieVIPA.RodzajVIPA rodzaj, string uzytkownik):this()
        {
            Login = uzytkownik;
           switch(rodzaj)
            {
                case KupowanieVIPA.RodzajVIPA.miesiąc:
                    lblRodzajVIPA.Content = "1 miesiąc";
                    rodzajSubskrypcji = KupowanieVIPA.RodzajVIPA.miesiąc.ToString();
                    cena = 91.99m;
                    break;
                case KupowanieVIPA.RodzajVIPA.pół_roku:
                    lblRodzajVIPA.Content = "6 miesięcy";
                    rodzajSubskrypcji = KupowanieVIPA.RodzajVIPA.pół_roku.ToString();
                    cena = 274.98m;
                    break;
                case KupowanieVIPA.RodzajVIPA.rok:
                    lblRodzajVIPA.Content = "12 miesięcy";
                    rodzajSubskrypcji = KupowanieVIPA.RodzajVIPA.rok.ToString();
                    cena = 369.96m;
                    break;
            }


        }

        private void btnKARTA_Click(object sender, RoutedEventArgs e)
        {
            txtNumerKarty.Text = "";

            lblNapiszPrzyKupowaniuKartą.Visibility = Visibility.Visible;
            txtNumerKarty.Visibility = Visibility.Visible;

            lblNapisPrzyKupowaniuBlika.Visibility = Visibility.Hidden;
            txtNumerBLIK.Visibility = Visibility.Hidden;

            metoda = MetodaPlatnosci.karta;
        }

        private void btnBLIK_Click(object sender, RoutedEventArgs e)
        {
            txtNumerBLIK.Text = "";

            lblNapisPrzyKupowaniuBlika.Visibility = Visibility.Visible;
            txtNumerBLIK.Visibility = Visibility.Visible;

            lblNapiszPrzyKupowaniuKartą.Visibility = Visibility.Hidden;
            txtNumerKarty.Visibility = Visibility.Hidden;

            metoda = MetodaPlatnosci.blik;
        }


        private void btnFinalizacjaZakupu_Click(object sender, RoutedEventArgs e)
        {
            int liczbaPoprawnych = 0;
            switch (metoda)
            {
                case MetodaPlatnosci.blik:
                    liczbaPoprawnych = 0;
                    foreach (var c in txtNumerBLIK.Text)
                    {
                        if (c >= 48 && c <= 57)
                            liczbaPoprawnych++;
                    }

                    if (txtNumerBLIK.Text.Length == 6 && liczbaPoprawnych == 6)
                    {
                        using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                        {
                            polaczenie.Open();
                            SqlCommand nadanieVipa = new SqlCommand($"UPDATE Użytkownik SET vip = '{true}' WHERE login = '{Login}' ", polaczenie);
                            nadanieVipa.ExecuteNonQuery();
                            polaczenie.Close();
                            polaczenie.Open();
                            SqlCommand pobranieID = new SqlCommand($"SELECT IdUżytkownika from Użytkownik WHERE login = '{Login}' ", polaczenie);
                            int id = (int)pobranieID.ExecuteScalar();
                            polaczenie.Close();
                            polaczenie.Open();
                            SqlCommand nowyVIP = new SqlCommand($"INSERT INTO SubVip(dataZakupu,rodzaj,cena,idUżytkownika) VALUES(@data,@rodzaj,@cena,@id)", polaczenie);
                            nowyVIP.Parameters.Add("data", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                            nowyVIP.Parameters.Add("rodzaj", System.Data.SqlDbType.NVarChar).Value = this.rodzajSubskrypcji;
                            nowyVIP.Parameters.Add("cena", System.Data.SqlDbType.Decimal).Value = this.cena;
                            nowyVIP.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id;
                            nowyVIP.ExecuteNonQuery();
                            polaczenie.Close();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Niepoprawny kod BLIK!");
                    }
                    break;

                case MetodaPlatnosci.karta:
                    liczbaPoprawnych = 0;
                    foreach (var c in txtNumerKarty.Text)
                    {
                        if (c >= 48 && c <= 57)
                            liczbaPoprawnych++;
                    }

                    if (txtNumerKarty.Text.Length == 16 && liczbaPoprawnych == 16)
                    {
                        using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                        {
                            polaczenie.Open();
                            SqlCommand nadanieVipa = new SqlCommand($"UPDATE Użytkownik SET vip = '{true}' WHERE login = '{Login}' ", polaczenie);
                            nadanieVipa.ExecuteNonQuery();
                            polaczenie.Close();
                            polaczenie.Open();
                            SqlCommand pobranieID = new SqlCommand($"SELECT IdUżytkownika from Użytkownik WHERE login = '{Login}' ", polaczenie);
                            int id = (int)pobranieID.ExecuteScalar();
                            polaczenie.Close();
                            polaczenie.Open();
                            SqlCommand nowyVIP = new SqlCommand($"INSERT INTO SubVip(dataZakupu,rodzaj,cena,idUżytkownika) VALUES(@data,@rodzaj,@cena,@id)", polaczenie);
                            nowyVIP.Parameters.Add("data", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                            nowyVIP.Parameters.Add("rodzaj", System.Data.SqlDbType.NVarChar).Value = this.rodzajSubskrypcji;
                            nowyVIP.Parameters.Add("cena", System.Data.SqlDbType.Decimal).Value = this.cena;
                            nowyVIP.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id;
                            nowyVIP.ExecuteNonQuery();
                            polaczenie.Close();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Niepoprawny numer karty!");
                    }
                    break;
            }

            
        }
    }
}
