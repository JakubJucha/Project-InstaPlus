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
    /// Logika interakcji dla klasy OdpowiedziKomentarze.xaml
    /// </summary>
    public partial class OdpowiedziKomentarze : Window
    {
        public int IdKomentarza { get; set; }
        public string TreśćKomenta { get; set; }
        public int IdU { get; set; }
        public OdpowiedziKomentarze()
        {
            InitializeComponent();
        }
        public OdpowiedziKomentarze(string tresc, int id, int idU) : this()
        {
            IdKomentarza = id;
            TreśćKomenta = tresc;
            IdU = idU;
            lbxKomentarz.Items.Add(TreśćKomenta);
            czyśćKomentarze();
            wyświetlOdpowiedziKomentarze();
        }

        private void btnDodajOdpowiedz_Click(object sender, RoutedEventArgs e)
        {
            int idUżytkownika = IdU;
            var dodajOdp = new DodawanieOdpowiedzi(idUżytkownika, IdKomentarza);
            dodajOdp.ShowDialog();
            czyśćKomentarze();
            wyświetlOdpowiedziKomentarze();
        }
        public void czyśćKomentarze()
        {
            lbxOdpowiedzi.Items.Clear();
        }

        public void wyświetlOdpowiedziKomentarze()
        {
            List<int> idKomentujących = new List<int>();
            List<string> nazwyKomentujących = new List<string>();
            List<string> treśćKomentarzy = new List<string>();
            List<string> dataKomentarza = new List<string>();
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand pobierzKomentarze = new SqlCommand($"SELECT idUżytkownika from OdpowiedziKom where id_odp = '{IdKomentarza}'", polaczenie);
                SqlDataReader idUżytkowników = pobierzKomentarze.ExecuteReader();
                while (idUżytkowników.Read())
                {
                    idKomentujących.Add(Convert.ToInt32(idUżytkowników["idUżytkownika"]));
                }
                polaczenie.Close();

                for (int i = 0; i < idKomentujących.Count; i++)
                {
                    polaczenie.Open();

                    SqlCommand pobierzNazwyKomentujących = new SqlCommand($"SELECT nazwaUżytkownika from Użytkownik where IdUżytkownika = '{idKomentujących[i]}'", polaczenie);
                    SqlDataReader nazwaKomentującego = pobierzNazwyKomentujących.ExecuteReader();
                    while (nazwaKomentującego.Read())
                    {
                        nazwyKomentujących.Add(Convert.ToString(nazwaKomentującego["nazwaUżytkownika"]));
                    }
                    polaczenie.Close();
                }

                List<string> listId = new List<string>();

                polaczenie.Open();
                SqlCommand pobierzIdKomentarzy = new SqlCommand($"SELECT idodpowiedziKom from OdpowiedziKom where id_odp = '{IdKomentarza}'", polaczenie);
                SqlDataReader IdKomentarzy = pobierzIdKomentarzy.ExecuteReader();
                while (IdKomentarzy.Read())
                {
                    listId.Add(Convert.ToString(IdKomentarzy["idodpowiedziKom"]));
                }

                polaczenie.Close();

                for (int i = 0; i < idKomentujących.Count; i++)
                {
                    polaczenie.Open();

                    SqlCommand pobierzTreśćKomentarzy = new SqlCommand($"SELECT treść from OdpowiedziKom where idUżytkownika = '{idKomentujących[i]}' AND id_odp = '{IdKomentarza}' AND idodpowiedziKom = '{listId[i]}'", polaczenie);
                    SqlDataReader treśćKomentarza = pobierzTreśćKomentarzy.ExecuteReader();
                    while (treśćKomentarza.Read())
                    {
                        treśćKomentarzy.Add(Convert.ToString(treśćKomentarza["treść"]));
                    }
                    polaczenie.Close();
                }

                for (int i = 0; i < idKomentujących.Count; i++)
                {
                    polaczenie.Open();

                    SqlCommand pobierzDateKomentarzy = new SqlCommand($"SELECT dataDodania from OdpowiedziKom where idUżytkownika = '{idKomentujących[i]}' AND id_odp = '{IdKomentarza}'  AND idodpowiedziKom = '{listId[i]}'", polaczenie);
                    SqlDataReader dataKometarza = pobierzDateKomentarzy.ExecuteReader();
                    while (dataKometarza.Read())
                    {
                        dataKomentarza.Add(Convert.ToString(dataKometarza["dataDodania"]));
                    }
                    polaczenie.Close();
                }
            }


            for (int i = 0; i < idKomentujących.Count; i++)
            {
                string komentarz = $"👦🏼: " + nazwyKomentujących[i] + "       🕒: " + dataKomentarza[i] + "\n💬: " + treśćKomentarzy[i] + "\n————————————————————————";
                lbxOdpowiedzi.Items.Add(komentarz);
            }
        }
     
        public int IdAktualnegoKomentarza()
        {


            string komentarzyk = lbxOdpowiedzi.SelectedItem.ToString();
            string nazwaKomentującego = "";
            char znak = 'x';

            int i = 6;
            while (znak != ' ')
            {

                znak = komentarzyk[i];
                if (znak == ' ') break;
                nazwaKomentującego = nazwaKomentującego + znak;
                i++;
            }
            znak = 'x';
            string treśćKoma = "";
            //i = i + 7 + 6 + 2 + 19 + 1 + 6 + 2;
            //while (znak != ' ')
            //{

            //    znak = komentarzyk[i];
            //    if (znak == ' ') break;
            //    nazwaKomentującego = nazwaKomentującego + znak;
            //    i++;
            //}
            //string emotka = "";

            while (znak != '\udcac')
            {

                // if (emotka == "💬") break;
                znak = komentarzyk[i];
                i++;
            }
            i = i + 2;

            znak = 'x';
            while (znak != '\n')
            {

                znak = komentarzyk[i];
                if (znak == '\n') break;
                treśćKoma = treśćKoma + znak;
                i++;
            }

            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {

                polaczenie.Open();
                SqlCommand pobierzIdKomentującego = new SqlCommand($"SELECT IdUżytkownika from Użytkownik where nazwaUżytkownika = '{nazwaKomentującego}'", polaczenie);
                int idKomentujacego = (int)pobierzIdKomentującego.ExecuteScalar();
                polaczenie.Close();
                polaczenie.Open();
                SqlCommand pobierzIdKomentarza = new SqlCommand($"SELECT idodpowiedziKom from OdpowiedziKom where treść = '{treśćKoma}' AND idUżytkownika = '{idKomentujacego}'", polaczenie);
                SqlDataReader czytnik = pobierzIdKomentarza.ExecuteReader();
                //int idKomenta = (int)pobierzIdKomentarza.ExecuteScalar();
                int idKomenta = 0;
                while (czytnik.Read())
                {
                    idKomenta = Convert.ToInt32(czytnik["idodpowiedziKom"]);
                }
                polaczenie.Close();

                return idKomenta;
            }


        }

        private void btnZgłośOdpowiedz_Click(object sender, RoutedEventArgs e)
        {
            if (lbxOdpowiedzi.SelectedIndex != -1)
            {
                int idOdpowiedzi = IdAktualnegoKomentarza();
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zgłosić ten komentarz?", "Zgłaszanie", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                    {
                        polaczenie.Open();
                        SqlCommand ZgłoszenieKomentarza = new SqlCommand($"UPDATE OdpowiedziKom SET czyZgłoszony = '{1}' where idodpowiedziKom = '{idOdpowiedzi}'", polaczenie);
                        ZgłoszenieKomentarza.ExecuteNonQuery();
                        polaczenie.Close();
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                }
            }
            else MessageBox.Show("Nie wybrano komentarza!");


        }
    }
}
