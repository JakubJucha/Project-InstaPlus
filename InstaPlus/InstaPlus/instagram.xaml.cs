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
using Klasy;

namespace PBD_działające__y_
{
    /// <summary>
    /// Logika interakcji dla klasy instagram.xaml
    /// </summary>
    public partial class instagram : Window
    {

        public string Login { set; get; }
        public bool czyMojePosty = false;
        public struct Post
        {
            public int id_Posta;
            public BitmapImage zdjecie_w_Poscie;
            public string opis_Posta;
            public DateTime data_Posta;
            public int id_Autora_Posta;
            public bool czy_Zgloszony_Post;
        }

        public struct Komentarz
        {
            public int id_Komentarza;
            public int id_Posta;
            public string tresc_koemntarza;
            public DateTime data_Komentarza;
            public int id_Autora_Komentarza;
            public bool czy_Zgloszony_Komentarz;
        }
        public List<Komentarz> listaKomentarzy = new List<Komentarz>();
        public List<Post> listaPostow = new List<Post>();
        public static int indeksator = 0;

        public instagram()
        {
            InitializeComponent();
        }

        public void wypelnijKomentarze()
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                listaKomentarzy.Clear();
                polaczenie.Open();
                SqlCommand zawartoscPosta = new SqlCommand($"SELECT * from Komentarze", polaczenie);
                SqlDataReader czytnik = zawartoscPosta.ExecuteReader();
                while (czytnik.Read())
                {

                    Komentarz k = new Komentarz();
                    k.id_Komentarza = Convert.ToInt32(czytnik["idKomentarze"]);
                    k.id_Posta = Convert.ToInt32(czytnik["idPosta"]);
                    k.data_Komentarza = Convert.ToDateTime(czytnik["dataDodania"]);
                    k.id_Autora_Komentarza = Convert.ToInt32(czytnik["idUżytkownika"]);
                    k.tresc_koemntarza = Convert.ToString(czytnik["treść"]);
                    k.czy_Zgloszony_Komentarz = Convert.ToBoolean(czytnik["czyZgłoszony"]);
                    listaKomentarzy.Add(k);
                }
                polaczenie.Close();
            }
        }

        public void wyświetlKomentarze()
        {
            List<int> idKomentujących = new List<int>();
            List<string> nazwyKomentujących = new List<string>();
            List<string> treśćKomentarzy = new List<string>();
            List<string> dataKomentarza = new List<string>();
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {

                if (!czyAdmin())
                {
                    polaczenie.Open();
                    SqlCommand pobierzKomentarze = new SqlCommand($"SELECT idUżytkownika from Komentarze where idPosta = '{IdAktualnegoPosta()}'", polaczenie);
                    SqlDataReader idUżytkowników = pobierzKomentarze.ExecuteReader();
                    while (idUżytkowników.Read())
                    {
                        idKomentujących.Add(Convert.ToInt32(idUżytkowników["idUżytkownika"]));
                    }
                    polaczenie.Close();
                }
                else
                {
                    polaczenie.Open();
                    SqlCommand pobierzKomentarze = new SqlCommand($"SELECT idUżytkownika from Komentarze where czyZgłoszony = '{1}'", polaczenie);
                    SqlDataReader idUżytkowników = pobierzKomentarze.ExecuteReader();
                    while (idUżytkowników.Read())
                    {
                        idKomentujących.Add(Convert.ToInt32(idUżytkowników["idUżytkownika"]));
                    }
                    polaczenie.Close();
                }


                if (!czyAdmin())
                {
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
                }
                else
                {
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
                }

                List<string> listId = new List<string>();
                if (!czyAdmin())
                {
                    polaczenie.Open();
                    SqlCommand pobierzIdKomentarzy = new SqlCommand($"SELECT idKomentarze from Komentarze where idPosta = '{IdAktualnegoPosta()}'", polaczenie);
                    SqlDataReader IdKomentarzy = pobierzIdKomentarzy.ExecuteReader();
                    while (IdKomentarzy.Read())
                    {
                        listId.Add(Convert.ToString(IdKomentarzy["idKomentarze"]));
                    }

                    polaczenie.Close();
                }
                else
                {

                    polaczenie.Open();
                    SqlCommand pobierzIdKomentarzy = new SqlCommand($"SELECT idKomentarze from Komentarze where czyZgłoszony = {1}", polaczenie);
                    SqlDataReader IdKomentarzy = pobierzIdKomentarzy.ExecuteReader();
                    while (IdKomentarzy.Read())
                    {
                        listId.Add(Convert.ToString(IdKomentarzy["idKomentarze"]));

                    }
                    polaczenie.Close();
                }


                if (!czyAdmin())
                {
                    for (int i = 0; i < idKomentujących.Count; i++)
                    {
                        polaczenie.Open();

                        SqlCommand pobierzDateKomentarzy = new SqlCommand($"SELECT dataDodania from Komentarze where idUżytkownika = '{idKomentujących[i]}' AND idPosta = '{IdAktualnegoPosta()}' AND idKomentarze = '{listId[i]}'", polaczenie);
                        SqlDataReader dataKometarza = pobierzDateKomentarzy.ExecuteReader();
                        while (dataKometarza.Read())
                        {
                            dataKomentarza.Add(Convert.ToString(dataKometarza["dataDodania"]));
                        }
                        polaczenie.Close();
                    }
                }
                else
                {
                    for (int i = 0; i < idKomentujących.Count; i++)
                    {
                        polaczenie.Open();

                        SqlCommand pobierzDateKomentarzy = new SqlCommand($"SELECT dataDodania from Komentarze where idUżytkownika = '{idKomentujących[i]}' AND czyZgłoszony = {1} AND idKomentarze = '{listId[i]}'", polaczenie);
                        SqlDataReader dataKometarza = pobierzDateKomentarzy.ExecuteReader();
                        while (dataKometarza.Read())
                        {
                            dataKomentarza.Add(Convert.ToString(dataKometarza["dataDodania"]));
                        }
                        polaczenie.Close();
                    }
                }



                if (!czyAdmin())
                {
                    for (int i = 0; i < idKomentujących.Count; i++)
                    {
                        polaczenie.Open();
                        SqlCommand pobierzTreśćKomentarzy = new SqlCommand($"SELECT treść from Komentarze where idUżytkownika = '{idKomentujących[i]}' AND idPosta = '{IdAktualnegoPosta()}' AND idKomentarze = '{listId[i]}'", polaczenie);
                        SqlDataReader treśćKomentarza = pobierzTreśćKomentarzy.ExecuteReader();
                        while (treśćKomentarza.Read())
                        {
                            treśćKomentarzy.Add(Convert.ToString(treśćKomentarza["treść"]));
                        }

                        polaczenie.Close();
                    }
                }
                else
                {
                    for (int i = 0; i < idKomentujących.Count; i++)
                    {
                        polaczenie.Open();

                        SqlCommand pobierzTreśćKomentarzy = new SqlCommand($"SELECT treść from Komentarze where idUżytkownika = '{idKomentujących[i]}' AND czyZgłoszony = '{1}' AND idKomentarze = '{listId[i]}'", polaczenie);
                        SqlDataReader treśćKomentarza = pobierzTreśćKomentarzy.ExecuteReader();
                        while (treśćKomentarza.Read())
                        {
                            treśćKomentarzy.Add(Convert.ToString(treśćKomentarza["treść"]));
                        }
                        polaczenie.Close();
                    }
                }

            }
            for (int i = 0; i < idKomentujących.Count; i++)
            {
                string komentarz = $"👦🏼: " + nazwyKomentujących[i] + "       🕒: " + dataKomentarza[i] + "\n💬: " + treśćKomentarzy[i] + "\n————————————————————————";
                lbxKomentarzeDoPosta.Items.Add(komentarz);
            }
            if (czyAdmin()) { wyświetlOdpowiedziDlaAdmina(); }
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


        public void interfejs()
        {
            if (czyAdmin())
            {
                btnUsunMojPost.Visibility = Visibility.Hidden;
                btnDodajPost.Visibility = Visibility.Hidden;
                btnZglosPost.Visibility = Visibility.Hidden;
                btnNastepnyPost.Visibility = Visibility.Hidden;
                btnPoprzedniPost.Visibility = Visibility.Hidden;
                btnPolubieniePosta.Visibility = Visibility.Hidden;
                BtnDodajKomentarz.Visibility = Visibility.Hidden;

                btnZbanujUzytkownika.Visibility = Visibility.Visible;
                btnUsunPost.Visibility = Visibility.Visible;
                btnZachowajPost.Visibility = Visibility.Visible;
                btnMojePosty.Visibility = Visibility.Hidden;
                btnZbanujKomentujacego.Visibility = Visibility.Visible;
                btnUsunKom.Visibility = Visibility.Visible;
                btnZachowajKom.Visibility = Visibility.Visible;
                btnRaport.Visibility = Visibility.Visible;

            }
            else
            {
                btnMojePosty.Visibility = Visibility.Visible;
                btnUsunMojPost.Visibility = Visibility.Hidden;
                if (czyMojePosty == true)
                {
                    if (!(postyDoWyswietlenia().Count <= 0)) btnUsunMojPost.Visibility = Visibility.Visible;
                }
                if (postyDoWyswietlenia().Count <= 0)
                {
                    BtnDodajKomentarz.IsEnabled= false;
                    btnPolubieniePosta.IsEnabled= false;
                    btnZglosPost.IsEnabled = false;
                }else
                {
                    BtnDodajKomentarz.IsEnabled = true;
                    btnPolubieniePosta.IsEnabled = true;
                    btnZglosPost.IsEnabled = true;
                }

                btnZglosPost.Visibility = Visibility.Visible;

                btnDodajPost.Visibility = Visibility.Visible;
                btnNastepnyPost.Visibility = Visibility.Visible;
                btnPoprzedniPost.Visibility = Visibility.Visible;
                btnPolubieniePosta.Visibility = Visibility.Visible;
                BtnDodajKomentarz.Visibility = Visibility.Visible;

                btnZbanujUzytkownika.Visibility = Visibility.Hidden;
                btnUsunPost.Visibility = Visibility.Hidden;
                btnZachowajPost.Visibility = Visibility.Hidden;
                lbxKomentarzeDoPosta.Visibility = Visibility.Visible;
                btnZbanujKomentujacego.Visibility = Visibility.Hidden;
                btnUsunKom.Visibility = Visibility.Hidden;
                btnZachowajKom.Visibility = Visibility.Hidden;
                btnRaport.Visibility = Visibility.Hidden;

                if (czyZgloszony())
                {
                    btnZglosPost.Background = System.Windows.Media.Brushes.Red;
                    btnZglosPost.Content = "Zgłoszony";
                }
                else
                {
                    btnZglosPost.Background = System.Windows.Media.Brushes.Blue;
                    btnZglosPost.Content = "Zgłoś post";
                }
            }
        }

        public void wypelnijPosty()
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                listaPostow.Clear();
                polaczenie.Open();
                SqlCommand zawartoscPosta = new SqlCommand($"SELECT * from Post", polaczenie);
                SqlDataReader czytnik = zawartoscPosta.ExecuteReader();
                while (czytnik.Read())
                {

                    Post p = new Post();
                    p.id_Posta = Convert.ToInt32(czytnik["idPosta"]);
                    byte[] zdjeciePost = (byte[])czytnik["zdjęcie"];
                    p.zdjecie_w_Poscie = ConvertByteToBitmapImage(zdjeciePost);
                    p.opis_Posta = Convert.ToString(czytnik["opis"]);
                    p.data_Posta = Convert.ToDateTime(czytnik["dataDodania"]);
                    p.id_Autora_Posta = Convert.ToInt32(czytnik["idAutora"]);
                    p.czy_Zgloszony_Post = Convert.ToBoolean(czytnik["czyZgłoszony"]);
                    listaPostow.Add(p);
                }
                polaczenie.Close();
                listaPostow.Reverse();
            }
        }


        public List<Post> postyDoWyswietlenia()
        {
            if (RbPost.IsChecked == true)
            {
                return SzukanyTagWPoscie();
            }
            else if(czyMojePosty == true)
            {
                return mojePosty();
            }else
            {
                List<Post> postyDoWys = new List<Post>();
                int naszeId = getId(Login);

                using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    polaczenie.Open();
                    SqlCommand idZbanowanychUzytkownikow = new SqlCommand($"SELECT IdUżytkownika from Użytkownik WHERE czyZbanowany = '{1}'", polaczenie);
                    List<int> listaZbanowanychUzytkownikow = new List<int>();
                    SqlDataReader zbanowani = idZbanowanychUzytkownikow.ExecuteReader();
                    while (zbanowani.Read())
                    {
                        listaZbanowanychUzytkownikow.Add(Convert.ToInt32(zbanowani["IdUżytkownika"]));
                    }
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlCommand idLudziKtorychObserwujemy = new SqlCommand($"SELECT idUżytkownika from Obserwujący where idObserwatora = '{naszeId}'", polaczenie);
                    List<int> idLudziKtorychPostyNamSieMajaWyswietlac = new List<int>();
                    SqlDataReader czytnik = idLudziKtorychObserwujemy.ExecuteReader();
                    while (czytnik.Read())
                    {
                        idLudziKtorychPostyNamSieMajaWyswietlac.Add(Convert.ToInt32(czytnik["idUżytkownika"]));
                    }
                    polaczenie.Close();
                    idLudziKtorychPostyNamSieMajaWyswietlac.Add(naszeId);

                    foreach (var ban in listaZbanowanychUzytkownikow)
                    {
                        for (int id = 0; id < idLudziKtorychPostyNamSieMajaWyswietlac.Count; id++)
                        {
                            if (ban == idLudziKtorychPostyNamSieMajaWyswietlac.ElementAt(id))
                            {
                                idLudziKtorychPostyNamSieMajaWyswietlac.Remove(idLudziKtorychPostyNamSieMajaWyswietlac.ElementAt(id));
                            }
                        }
                    }


                    for (int id = 0; id < idLudziKtorychPostyNamSieMajaWyswietlac.Count; id++)
                    {
                        foreach (var post in listaPostow)
                        {
                            if (idLudziKtorychPostyNamSieMajaWyswietlac.ElementAt(id) == post.id_Autora_Posta)
                            {
                                postyDoWys.Add(post);
                            }
                        }
                    }
                }
                return postyDoWys;
            }

        }

        public List<Post> postyDoWyswietleniaDlaAdmina()
        {
            List<Post> postyDoWys = new List<Post>();
            //int naszeId = getId(Login);

            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand idPostówZgłoszonych = new SqlCommand($"SELECT idPosta from Post where czyZgłoszony = '{1}'", polaczenie);
                List<int> idPostówKtóreSieMajaWyswietlac = new List<int>();
                SqlDataReader czytnik = idPostówZgłoszonych.ExecuteReader();
                while (czytnik.Read())
                {
                    idPostówKtóreSieMajaWyswietlac.Add(Convert.ToInt32(czytnik["idPosta"]));
                }
                polaczenie.Close();
                for (int id = 0; id < idPostówKtóreSieMajaWyswietlac.Count; id++)
                {
                    foreach (var post in listaPostow)
                    {
                        if (idPostówKtóreSieMajaWyswietlac.ElementAt(id) == post.id_Posta)
                        {
                            postyDoWys.Add(post);
                        }
                    }
                }
            }
            return postyDoWys;

        }
        public bool czyAdmin()
        {

            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand czyAdmin = new SqlCommand($"SELECT moderator from Użytkownik where login = '{Login}'", polaczenie);
                bool? Admin = (bool?)czyAdmin.ExecuteScalar();
                polaczenie.Close();
                if (Admin != false)
                {
                    return true;
                }
                else
                    return false;

            }

        }
        public void wyswietlPostyNaGlownej(int indeks)
        {
            
            if (czyAdmin())
            {
                List<Post> wyswietlanePosty = postyDoWyswietleniaDlaAdmina();

                if (wyswietlanePosty.Count == 0) 
                { 
                    lblNieMaPostówXD.Visibility = Visibility.Visible;
                    btnZbanujUzytkownika.IsEnabled = false;
                    btnUsunPost.IsEnabled = false;
                    btnZachowajPost.IsEnabled = false;
                }
                else
                {
                  
                    btnZbanujUzytkownika.IsEnabled = true;
                    btnUsunPost.IsEnabled = true;
                    btnZachowajPost.IsEnabled = true;
                    lblNieMaPostówXD.Visibility = Visibility.Hidden;
                    var temp = wyswietlanePosty.OrderByDescending(item => item.data_Posta);
                    wyswietlanePosty = temp.ToList();
                    try
                    {

                        imgZdjecieWPoscie.Source = wyswietlanePosty[indeks].zdjecie_w_Poscie;
                        lblDataDodaniaPosta.Content = wyswietlanePosty[indeks].data_Posta;
                        txtOpisPosta.Text = wyswietlanePosty[indeks].opis_Posta;
                        using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                        {
                            polaczenie.Open();
                            SqlCommand daneOpa = new SqlCommand($"SELECT nazwaUżytkownika, profilowe from Użytkownik WHERE IdUżytkownika = '{wyswietlanePosty[indeks].id_Autora_Posta}'", polaczenie);
                            SqlDataReader czytnik = daneOpa.ExecuteReader();

                            while (czytnik.Read())
                            {
                                lblNazwaOPa.Content = czytnik.GetString(0);
                                byte[] profiloweOpa = (byte[])czytnik["profilowe"];
                                imgProfiloweOPA.Source = ConvertByteToBitmapImage(profiloweOpa);
                            }
                            polaczenie.Close();
                        }
                    }
                    catch (Exception ex) { }


                }
            }
            else if (RbPost.IsChecked == true)
            {


                WyswietlanieLiczbyPolubien();
                List<Post> wyswietlanePosty = SzukanyTagWPoscie();

                if (wyswietlanePosty.Count() > indeksator+1) { btnNastepnyPost.IsEnabled = true; } else btnNastepnyPost.IsEnabled = false;
                if(indeksator == 0) { btnPoprzedniPost.IsEnabled = false; } else { btnPoprzedniPost.IsEnabled = true; }
                
                lblNieMaPostówXD.Visibility = Visibility.Hidden;
                var temp = wyswietlanePosty.OrderByDescending(item => item.data_Posta);
                wyswietlanePosty = temp.ToList();
                if (wyswietlanePosty.Count == 0) { lblNieMaPostówXD.Visibility = Visibility.Visible; }
                else
                {

                    try
                    {
                        imgZdjecieWPoscie.Source = wyswietlanePosty[indeks].zdjecie_w_Poscie;
                        lblDataDodaniaPosta.Content = wyswietlanePosty[indeks].data_Posta;
                        txtOpisPosta.Text = wyswietlanePosty[indeks].opis_Posta;
                        using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                        {
                            polaczenie.Open();
                            SqlCommand daneOpa = new SqlCommand($"SELECT nazwaUżytkownika, profilowe from Użytkownik WHERE IdUżytkownika = '{wyswietlanePosty[indeks].id_Autora_Posta}'", polaczenie);
                            SqlDataReader czytnik = daneOpa.ExecuteReader();

                            while (czytnik.Read())
                            {
                                lblNazwaOPa.Content = czytnik.GetString(0);
                                byte[] profiloweOpa = (byte[])czytnik["profilowe"];
                                imgProfiloweOPA.Source = ConvertByteToBitmapImage(profiloweOpa);
                            }
                            polaczenie.Close();
                        }
                    }
                    catch (Exception ex) { }
                }

            }
            else
            {
                WyswietlanieLiczbyPolubien();
                List<Post> wyswietlanePosty = postyDoWyswietlenia();
                if (wyswietlanePosty.Count == 0) { lblNieMaPostówXD.Visibility = Visibility.Visible; }
                else
                {
                    lblNieMaPostówXD.Visibility = Visibility.Hidden;
                    var temp = wyswietlanePosty.OrderByDescending(item => item.data_Posta);
                    wyswietlanePosty = temp.ToList();

                    try
                    {
                        imgZdjecieWPoscie.Source = wyswietlanePosty[indeks].zdjecie_w_Poscie;
                        lblDataDodaniaPosta.Content = wyswietlanePosty[indeks].data_Posta;
                        txtOpisPosta.Text = wyswietlanePosty[indeks].opis_Posta;
                        using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                        {
                            polaczenie.Open();
                            SqlCommand daneOpa = new SqlCommand($"SELECT nazwaUżytkownika, profilowe from Użytkownik WHERE IdUżytkownika = '{wyswietlanePosty[indeks].id_Autora_Posta}'", polaczenie);
                            SqlDataReader czytnik = daneOpa.ExecuteReader();

                            while (czytnik.Read())
                            {
                                lblNazwaOPa.Content = czytnik.GetString(0);
                                byte[] profiloweOpa = (byte[])czytnik["profilowe"];
                                imgProfiloweOPA.Source = ConvertByteToBitmapImage(profiloweOpa);
                            }
                            polaczenie.Close();
                        }
                    }
                    catch (Exception ex) { }
                }
            }
            czyśćKomentarze();
            wyświetlKomentarze();
        }


        void odwiezStatus()
        {
            wypelnijPosty();

            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();

                SqlCommand czyModerator = new SqlCommand($"SELECT moderator from Użytkownik WHERE login = '{Login}'", polaczenie);
                bool? czyMod = (bool?)czyModerator.ExecuteScalar();
                if (czyMod == true)
                {
                    lblWitajTEst.Content = $"Moderator";
                    btnKupVIP.Visibility = Visibility.Hidden;
                    btnKupVIP.IsEnabled = false;
                    imgReklama1.Visibility = Visibility.Hidden;
                    imgReklama2.Visibility = Visibility.Hidden;
                    btnZglosKomentarz.Visibility = Visibility.Hidden;
                }
                polaczenie.Close();


                polaczenie.Open();
                SqlCommand czyVip = new SqlCommand($"SELECT vip from Użytkownik WHERE login = '{Login}'", polaczenie);
                bool? VIP = (bool?)czyVip.ExecuteScalar();
                if (VIP == true && czyMod == false)
                {
                    lblWitajTEst.Content = $"Konto VIP";

                    btnKupVIP.Visibility = Visibility.Hidden;
                    btnKupVIP.IsEnabled = false;
                    imgReklama1.Visibility = Visibility.Hidden;
                    imgReklama2.Visibility = Visibility.Hidden;
                    btnZglosKomentarz.Visibility = Visibility.Visible;
                }
                polaczenie.Close();

                if (czyMod == false && VIP == false)
                {
                    lblWitajTEst.Content = $"Konto zwykłe";
                    imgReklama1.Visibility = Visibility.Visible;
                    imgReklama2.Visibility = Visibility.Visible;
                    btnZglosKomentarz.Visibility = Visibility.Visible;

                }
            }
        }
        public instagram(string login) : this()
        {
            this.Login = login;

            indeksator = 0;
            odswiezenieOknaGlownego();
            wyswietlPostyNaGlownej(0);
            WyswietlanieLiczbyPolubien();
            interfejs();
            RbPost.IsChecked = false;
            if (indeksator == 0) { btnPoprzedniPost.IsEnabled = false; }
            List<Post> pomoc = postyDoWyswietlenia();
            if (pomoc.Count == 1 || pomoc.Count == 0) { btnNastepnyPost.IsEnabled = false; }
        }

        public void odswiezenieOknaGlownego()
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                odwiezStatus();

                polaczenie.Open();
                SqlCommand imieee = new SqlCommand($"SELECT imię from Użytkownik WHERE login = '{Login}'", polaczenie);
                string imie = (string)imieee.ExecuteScalar();
                polaczenie.Close();
                polaczenie.Open();
                SqlCommand nazwiskoo = new SqlCommand($"SELECT nazwisko from Użytkownik WHERE login = '{Login}'", polaczenie);
                string nazwisko = (string)nazwiskoo.ExecuteScalar();
                polaczenie.Close();
                lblImieNazwisko.Content = $"{imie} {nazwisko}";

                polaczenie.Open();
                try
                {
                    SqlCommand zdjecie = new SqlCommand($"SELECT profilowe from Użytkownik WHERE login = '{Login}'", polaczenie);
                    byte[] profilowe = (byte[])zdjecie.ExecuteScalar();
                    imgZdjecie.Source = ConvertByteToBitmapImage(profilowe);
                    polaczenie.Close();
                }
                catch (Exception) { }
            }
        }
        private void btnKupVIP_Click(object sender, RoutedEventArgs e)
        {
            var oknoKupowaniaVipa = new KupowanieVIPA(Login);
            oknoKupowaniaVipa.ShowDialog();
            odwiezStatus();
        }

        private void btnZgłoszone_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Banowanie uzytkownikow");
        }

        private void btnWyloguj_Click(object sender, RoutedEventArgs e)
        {
            var logowanie = new MainWindow();
            logowanie.Show();
            this.Close();
        }

        private void btnProfil_Click(object sender, RoutedEventArgs e)
        {
            var profil = new Profil(Login);
            this.Hide();
            profil.ShowDialog();
            
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                try
                {
                    polaczenie.Open();
                    SqlCommand zdjecie = new SqlCommand($"SELECT profilowe from Użytkownik WHERE login = '{Login}'", polaczenie);
                    byte[] profilowe = (byte[])zdjecie.ExecuteScalar();
                    imgZdjecie.Source = ConvertByteToBitmapImage(profilowe);
                    polaczenie.Close();
                }
                catch (Exception) { }
            }
            interfejs();
            this.Show();

        }

        public string getLogin(int id)
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand getLogin = new SqlCommand($"Select login from Użytkownik where IdUżytkownika = '{id}'", polaczenie);
                string login = (string)getLogin.ExecuteScalar();
                polaczenie.Close();
                return login;
            }
        }
        public void SzukanyTagWUzytkowniku()
        {
            string tag1 = txtWyszukiwarka.Text;
            string tag = tag1.TrimStart('#');
            List<int> ids = new List<int>();

            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand IdUzytkownikaZtagiem = new SqlCommand($"SELECT idTagu from Tagi WHERE treść = '{tag}' AND użytkownik = '{1}'", polaczenie);
                SqlDataReader czyIstnieje = IdUzytkownikaZtagiem.ExecuteReader();

                if (czyIstnieje.Read())
                {
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlDataReader wypisanieTagow = IdUzytkownikaZtagiem.ExecuteReader();
                    while (wypisanieTagow.Read())
                    {
                        ids.Add(Convert.ToInt32(wypisanieTagow["idTagu"]));
                    }
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlCommand getlogin = new SqlCommand($"Select login from Użytkownik where IdUżytkownika = '{ids[0]}'", polaczenie);
                    string login = (string)getlogin.ExecuteScalar();
                    polaczenie.Close();

                    List<string> loginyOsobZTagami = new List<string>();
                    foreach (var s in ids)
                    {
                        loginyOsobZTagami.Add(getLogin(s));
                    }

                    for (int i = 0; i < loginyOsobZTagami.Count; i++)
                    {
                        var profilSzukanegoUzytkownika = new Profil(loginyOsobZTagami[i], Login);
                        this.Hide();
                        if (this.Login != login)
                        {
                            profilSzukanegoUzytkownika.btnBio.Visibility = Visibility.Hidden;
                            profilSzukanegoUzytkownika.btnDodajZdj.Visibility = Visibility.Hidden;
                            profilSzukanegoUzytkownika.btnObserwuj.Visibility = Visibility.Visible;
                          //  profilSzukanegoUzytkownika.btnNastepnyUzytkownikZTagiem.Visibility = Visibility.Visible;
                        }
                        profilSzukanegoUzytkownika.btnNastepnyUzytkownikZTagiem.Visibility = Visibility.Visible;
                        profilSzukanegoUzytkownika.ShowDialog();
                        if (profilSzukanegoUzytkownika.czyNastepnyProfil == false) break;
                    }
                    interfejs();
                    this.Show();
                }
                else MessageBox.Show("Nie ma użytkownika z takim tagiem!");

            }
        }
        public List<Post> SzukanyTagWPoscie()
        {
            string tag1 = txtWyszukiwarka.Text;
            string tag = tag1.TrimStart('#');
            List<int> ids = new List<int>();
            List<Post> postyDoWys = new List<Post>();
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand IdPostaZtagiem = new SqlCommand($"SELECT idTagu from Tagi WHERE treść = '{tag}' AND post = '{1}'", polaczenie);
                SqlDataReader czyIstnieje = IdPostaZtagiem.ExecuteReader();

                if (czyIstnieje.Read())
                {
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlDataReader wypisanieTagow = IdPostaZtagiem.ExecuteReader();
                    while (wypisanieTagow.Read())
                    {
                        ids.Add(Convert.ToInt32(wypisanieTagow["idTagu"]));
                    }
                    polaczenie.Close();


                    foreach (var id in ids)
                    {
                        foreach (var post in listaPostow)
                        {
                            if (id == post.id_Posta)
                            {
                                postyDoWys.Add(post);
                            }
                        }
                    }

                }
                else MessageBox.Show("Nie ma posta z takim tagiem!");

                return postyDoWys;
            }

        }

        public List<Post> mojePosty()
        {
            List<int> ids = new List<int>();
            List<Post> postyDoWys = new List<Post>();
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand mp = new SqlCommand($"SELECT idPosta from Post where idAutora = '{getId(Login)}'",polaczenie);
                SqlDataReader czytnik = mp.ExecuteReader();

                while (czytnik.Read())
                {
                    ids.Add(Convert.ToInt32(czytnik["idPosta"]));
                }
                polaczenie.Close();
            }
            foreach (var id in ids)
            {
                foreach (var post in listaPostow)
                {
                    if (id == post.id_Posta)
                    {
                        postyDoWys.Add(post);
                    }
                }
            }
            return postyDoWys;
        }
        public void wyswietlPostyPoTagu()
        {
            int indeks = 0;
            WyswietlanieLiczbyPolubien();
            List<Post> wyswietlanePosty = SzukanyTagWPoscie();
           
            
            if (wyswietlanePosty.Count == 0) { lblNieMaPostówXD.Visibility = Visibility.Visible; }
            else
            {
                lblNieMaPostówXD.Visibility = Visibility.Hidden;
                var temp = wyswietlanePosty.OrderByDescending(item => item.data_Posta);
                wyswietlanePosty = temp.ToList();

                try
                {
                    imgZdjecieWPoscie.Source = wyswietlanePosty[indeks].zdjecie_w_Poscie;
                    lblDataDodaniaPosta.Content = wyswietlanePosty[indeks].data_Posta;
                    txtOpisPosta.Text = wyswietlanePosty[indeks].opis_Posta;
                    using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                    {
                        polaczenie.Open();
                        SqlCommand daneOpa = new SqlCommand($"SELECT nazwaUżytkownika, profilowe from Użytkownik WHERE IdUżytkownika = '{wyswietlanePosty[indeks].id_Autora_Posta}'", polaczenie);
                        SqlDataReader czytnik = daneOpa.ExecuteReader();

                        while (czytnik.Read())
                        {
                            lblNazwaOPa.Content = czytnik.GetString(0);
                            byte[] profiloweOpa = (byte[])czytnik["profilowe"];
                            imgProfiloweOPA.Source = ConvertByteToBitmapImage(profiloweOpa);
                        }
                        polaczenie.Close();
                    }
                }
                catch (Exception ex) { }
            }
            czyśćKomentarze();
            wyświetlKomentarze();
        }

        private void btnWyszukiwarka_Click(object sender, RoutedEventArgs e)
        {
            if (RbUzytkownik.IsChecked == true)
            {
                SzukanyTagWUzytkowniku();

            }
            else
            if (RbPost.IsChecked == true)
            {
                List<Post> temp = new List<Post>();
                temp = SzukanyTagWPoscie();
                if (temp.Count > 0)
                {
                    List<Post> pomoc = postyDoWyswietlenia();
                    if (pomoc.Count == 1 || pomoc.Count == 0) { btnNastepnyPost.IsEnabled = false; }
                    else btnNastepnyPost.IsEnabled = true;
                   
                    indeksator = 0;
                    wyswietlPostyNaGlownej(indeksator);
                }


            }
            else
            {
                string szukanyUzytkownik = txtWyszukiwarka.Text;
                txtWyszukiwarka.Text = "";
                using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    polaczenie.Open();
                    SqlCommand zbanowany = new SqlCommand($"SELECT COUNT(*) from Użytkownik WHERE nazwaUżytkownika = '{szukanyUzytkownik}' and czyZbanowany = '{1}'", polaczenie);
                    int toZbanowanyCzyNie = (int)zbanowany.ExecuteScalar();
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlCommand wyszukiwanieUzytkownika = new SqlCommand($"SELECT login from Użytkownik WHERE nazwaUżytkownika = '{szukanyUzytkownik}'", polaczenie);
                    SqlDataReader sprawdzenieCzyUzytkownikIstnieje = wyszukiwanieUzytkownika.ExecuteReader();
                    if (sprawdzenieCzyUzytkownikIstnieje.Read())
                    {
                        polaczenie.Close();
                        polaczenie.Open();
                        string loginSzukanegoUzytkownika = (string)wyszukiwanieUzytkownika.ExecuteScalar();
                        if (toZbanowanyCzyNie == 0)
                        {
                            var profilSzukanegoUzytkownika = new Profil(loginSzukanegoUzytkownika, Login);
                            this.Hide();

                            if (this.Login != profilSzukanegoUzytkownika.Login)
                            {
                                profilSzukanegoUzytkownika.btnBio.Visibility = Visibility.Hidden;
                                profilSzukanegoUzytkownika.btnDodajZdj.Visibility = Visibility.Hidden;
                                profilSzukanegoUzytkownika.btnObserwuj.Visibility = Visibility.Visible;
                            }
                            profilSzukanegoUzytkownika.ShowDialog();
                            List<Post> pomoc = postyDoWyswietlenia();
                            if (pomoc.Count == 1 || pomoc.Count == 0) { btnNastepnyPost.IsEnabled = false; }
                            else btnNastepnyPost.IsEnabled = true;
                            indeksator = 0;
                            odswiezenieOknaGlownego();
                            wyswietlPostyNaGlownej(0);
                            interfejs();
                            this.Show();
                            polaczenie.Close();
                        }
                        else MessageBox.Show("Ten użytkownik został zbanowany!");
                    }
                    else
                        MessageBox.Show("Taki użytkownik nie istnieje!");

                    polaczenie.Close();
                }
            }
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
        private void btnDodajPost_Click(object sender, RoutedEventArgs e)
        {
            var wstawianiePosta = new WstawianiePosta(Login);
            wstawianiePosta.ShowDialog();
            
            indeksator = 0;
            List<Post> temp = postyDoWyswietlenia();
            if (temp.Count() > 0) { btnNastepnyPost.IsEnabled = true; }
            
            odswiezenieOknaGlownego();
            wyswietlPostyNaGlownej(0);
            interfejs();
        }

        private void btnPoprzedniPost_Click(object sender, RoutedEventArgs e)
        {

            if (indeksator == 1)
            {
                btnPoprzedniPost.IsEnabled = false;
            }
            btnNastepnyPost.IsEnabled = true;
            indeksator--;
            wyswietlPostyNaGlownej(indeksator);
            if (czyZgloszony())
            {
                btnZglosPost.Background = System.Windows.Media.Brushes.Red;
                btnZglosPost.Content = "Zgłoszony";
            }
            else
            {
                btnZglosPost.Background = System.Windows.Media.Brushes.Blue;
                btnZglosPost.Content = "Zgłoś post";
            }
        }

        private void btnNastepnyPost_Click(object sender, RoutedEventArgs e)
        {
            List<Post> pomoc = new List<Post>();
            btnPoprzedniPost.IsEnabled = true;
            if (RbPost.IsChecked == true)
            { pomoc = SzukanyTagWPoscie(); }
            else if(czyMojePosty == true)
            {
                pomoc = mojePosty();
            } 
            else
            {
                pomoc = postyDoWyswietlenia();
            }

            if (indeksator == pomoc.Count - 2)
            {
                btnNastepnyPost.IsEnabled = false;
            }
            indeksator++;
            wyswietlPostyNaGlownej(indeksator);
            if (czyZgloszony())
            {
                btnZglosPost.Background = System.Windows.Media.Brushes.Red;
                btnZglosPost.Content = "Zgłoszony";
            }
            else
            {
                btnZglosPost.Background = System.Windows.Media.Brushes.Blue;
                btnZglosPost.Content = "Zgłoś post";
            }
        }


        public bool czyPolubione()
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand istnieje = new SqlCommand($"SELECT COUNT(*) from Polubienia where idPosta = '{IdAktualnegoPosta()}' and idUżytkownika = '{getId(Login)}'", polaczenie);
                int toIstniejeCzynie = (int)istnieje.ExecuteScalar();
                bool noJakToJest;
                if (toIstniejeCzynie == 0)
                {
                    noJakToJest = false;
                }
                else noJakToJest = true;

                polaczenie.Close();
                return noJakToJest;
            }
        }

        public void WyswietlanieLiczbyPolubien()
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand ilosclajkowposta = new SqlCommand($"SELECT COUNT(*) from Polubienia where idPosta = '{IdAktualnegoPosta()}'", polaczenie);
                int iloscwString = (int)ilosclajkowposta.ExecuteScalar();
                btnPolubieniePosta.Content = $"❤  {iloscwString}";
                polaczenie.Close();

            }
        }
        public int IdAktualnegoPosta()
        {

            if (czyAdmin())
            {
                List<Post> wyswietlanePosty = postyDoWyswietleniaDlaAdmina();
                var temp = wyswietlanePosty.OrderByDescending(item => item.data_Posta);
                wyswietlanePosty = temp.ToList();
                try
                {
                    int idPosta = wyswietlanePosty[indeksator].id_Posta;
                    return idPosta;
                }
                catch (Exception ex) { return 0; }
            }
            else
            {
                List<Post> wyswietlanePosty = postyDoWyswietlenia();
                var temp = wyswietlanePosty.OrderByDescending(item => item.data_Posta);
                wyswietlanePosty = temp.ToList();
                try
                {
                    int idPosta = wyswietlanePosty[indeksator].id_Posta;
                    return idPosta;
                }
                catch (Exception ex) { return 0; }
            }

        }

        private void btnPolubieniePosta_Click(object sender, RoutedEventArgs e)
        {
            int idLajkującego = getId(Login);

            int idPosta = IdAktualnegoPosta();


            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                if (czyPolubione())
                {
                    polaczenie.Open();
                    SqlCommand CofniecieLajka = new SqlCommand($"DELETE FROM Polubienia where idPosta = '{idPosta}' and idUżytkownika = '{idLajkującego}'", polaczenie);
                    CofniecieLajka.ExecuteNonQuery();
                    polaczenie.Close();

                }
                else
                {
                    polaczenie.Open();
                    SqlCommand Lajkowanie = new SqlCommand("INSERT INTO Polubienia(idPosta,idUżytkownika)VALUES(@post,@użytkownik)", polaczenie);
                    Lajkowanie.Parameters.Add("post", System.Data.SqlDbType.Int).Value = idPosta;
                    Lajkowanie.Parameters.Add("użytkownik", System.Data.SqlDbType.Int).Value = idLajkującego;
                    Lajkowanie.ExecuteNonQuery();
                    polaczenie.Close();
                }
                WyswietlanieLiczbyPolubien();
            }
        }


        public bool czyZgloszony()
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand zgloszony = new SqlCommand($"SELECT COUNT(*) from Post where idPosta = '{IdAktualnegoPosta()}' and czyZgłoszony = '{1}'", polaczenie);
                int toZgloszonyCzynie = (int)zgloszony.ExecuteScalar();
                bool noJakToJest;
                if (toZgloszonyCzynie == 0)
                {
                    noJakToJest = false;
                }
                else noJakToJest = true;

                polaczenie.Close();
                return noJakToJest;
            }
        }

        private void btnZglosPost_Click(object sender, RoutedEventArgs e)
        {
            int idPosta = IdAktualnegoPosta();
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zgłosić ten post?", "Zgłaszanie", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {

                    using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                    {

                        polaczenie.Open();
                        SqlCommand Zglaszanie = new SqlCommand($"UPDATE Post SET czyZgłoszony = '{1}' where idPosta = '{idPosta}'", polaczenie);
                        Zglaszanie.ExecuteNonQuery();
                        polaczenie.Close();
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                }
                WyswietlanieLiczbyPolubien();

        }

        private void btnUsunPost_Click(object sender, RoutedEventArgs e)
        {
            int id = IdAktualnegoPosta();
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand czySaZgloszoneJeszcze = new SqlCommand($"SELECT COUNT(*) from Post where czyZgłoszony = '{1}'", polaczenie);
                int temp = (int)czySaZgloszoneJeszcze.ExecuteScalar();
                polaczenie.Close();
                if (temp > 0)
                {
                    polaczenie.Open();
                    SqlCommand UsuwanieL = new SqlCommand($"DELETE from Polubienia where idPosta = {id}", polaczenie);
                    UsuwanieL.ExecuteNonQuery();
                    polaczenie.Close();
                    polaczenie.Open();
                    SqlCommand UsuwanieK = new SqlCommand($"DELETE from Komentarze where idPosta = {id}", polaczenie);
                    UsuwanieK.ExecuteNonQuery();
                    polaczenie.Close();
                   
                    polaczenie.Open();
                    SqlCommand Usuwanie = new SqlCommand($"DELETE from Post where idPosta = {id}", polaczenie);
                    Usuwanie.ExecuteNonQuery();
                    polaczenie.Close();
                    indeksator = 0;
                    wyswietlPostyNaGlownej(0);
                }
                else
                {
                    lblNieMaPostówXD.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnZachowajPost_Click(object sender, RoutedEventArgs e)
        {
            int id = IdAktualnegoPosta();

            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand czySaZgloszoneJeszcze = new SqlCommand($"SELECT COUNT(*) from Post where czyZgłoszony = '{1}'", polaczenie);
                int temp = (int)czySaZgloszoneJeszcze.ExecuteScalar();
                polaczenie.Close();
                if (temp > 0)
                {
                    polaczenie.Open();
                    SqlCommand CofniecieZgloszenia = new SqlCommand($"UPDATE Post SET czyZgłoszony = '{0}' where idPosta = '{id}'", polaczenie);
                    CofniecieZgloszenia.ExecuteNonQuery();
                    polaczenie.Close();
                    indeksator = 0;
                    wyswietlPostyNaGlownej(0);
                }
                else
                {
                    lblNieMaPostówXD.Visibility = Visibility.Visible;
                }
            }
        }
      
        private void btnZbanujUzytkownika_Click(object sender, RoutedEventArgs e)
        {
            int idPosta = IdAktualnegoPosta();

            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand zdobycieIdUzytkownikaDoZbanowania = new SqlCommand($"SELECT idAutora from Post where idPosta = '{idPosta}'", polaczenie);
                int idUzytkownika = (int)zdobycieIdUzytkownikaDoZbanowania.ExecuteScalar();
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand czySaZgloszoneJeszcze = new SqlCommand($"SELECT COUNT(*) from Post where czyZgłoszony = '{1}'", polaczenie);
                int temp = (int)czySaZgloszoneJeszcze.ExecuteScalar();
                polaczenie.Close();

                if (temp > 0)
                {
                    polaczenie.Open();
                    SqlCommand CofniecieZgloszenia = new SqlCommand($"UPDATE Post SET czyZgłoszony = '{0}' where idPosta = '{idPosta}'", polaczenie);
                    CofniecieZgloszenia.ExecuteNonQuery();
                    polaczenie.Close();
                    polaczenie.Open();
                    SqlCommand banowanieUzytkownika = new SqlCommand($"UPDATE Użytkownik SET czyZbanowany = '{1}' where IdUżytkownika = '{idUzytkownika}'", polaczenie);
                    banowanieUzytkownika.ExecuteNonQuery();
                    polaczenie.Close();
                    indeksator = 0;
                    wyswietlPostyNaGlownej(0);
                    usunRzeczyZbanowanego(idUzytkownika);
                }
                else
                {
                    lblNieMaPostówXD.Visibility = Visibility.Visible;
                }
            }

        }

        public List<string> WykryjTag()
        {
            string trescOpisu = txtOpisPosta.Text;
            List<string> tagi = new List<string>();
            string tag = "";
            char znak = 'x';
            int i = 0;

            while (true)
            {
                if (i == trescOpisu.Length - 1) break;
                while (znak != '#')
                {
                    znak = trescOpisu[i];
                    i++;
                    if (i == trescOpisu.Length - 1) break;
                }
                while (znak != ' ' || znak != '\n' || znak != '\t')
                {
                    znak = trescOpisu[i];
                    if (znak == ' ' || znak == '\n' || znak == '\t') { tagi.Add(tag); tag = ""; break; }
                    else if (i == trescOpisu.Length - 1) { tag = tag + znak; tagi.Add(tag); tag = ""; break; }
                    tag = tag + znak;
                    i++;
                }

            }
            return tagi;
        }
        private void BtnDodajKomentarz_Click(object sender, RoutedEventArgs e)
        {
            int idPosta = IdAktualnegoPosta();
            int idUżytkownika = getId(Login);
            var dodajKom = new DodawanieKomentarza(idUżytkownika, idPosta);
            dodajKom.ShowDialog();
            czyśćKomentarze();
            wyświetlKomentarze();


        }


        public void wyświetlOdpowiedziDlaAdmina()
        {
            List<int> idKomentujących = new List<int>();
            List<string> nazwyKomentujących = new List<string>();
            List<string> treśćKomentarzy = new List<string>();
            List<string> dataKomentarza = new List<string>();
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {


                polaczenie.Open();
                SqlCommand pobierzKomentarze = new SqlCommand($"SELECT idUżytkownika from OdpowiedziKom where czyZgłoszony = '{1}'", polaczenie);
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
                SqlCommand pobierzIdKomentarzy = new SqlCommand($"SELECT idodpowiedziKom from OdpowiedziKom where czyZgłoszony = {1}", polaczenie);
                SqlDataReader IdKomentarzy = pobierzIdKomentarzy.ExecuteReader();
                while (IdKomentarzy.Read())
                {
                    listId.Add(Convert.ToString(IdKomentarzy["idodpowiedziKom"]));

                }
                polaczenie.Close();

                for (int i = 0; i < idKomentujących.Count; i++)
                {
                    polaczenie.Open();

                    SqlCommand pobierzTreśćKomentarzy = new SqlCommand($"SELECT treść from OdpowiedziKom where idUżytkownika = '{idKomentujących[i]}' AND czyZgłoszony = '{1}'  AND idodpowiedziKom = '{listId[i]}'", polaczenie);
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

                    SqlCommand pobierzDateKomentarzy = new SqlCommand($"SELECT dataDodania from OdpowiedziKom where idUżytkownika = '{idKomentujących[i]}'  AND idodpowiedziKom = '{listId[i]}'", polaczenie);
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
                string komentarz = $"👦🏼: " + nazwyKomentujących[i] + "       🕒: " + dataKomentarza[i] + "\n💬: " + treśćKomentarzy[i] + "↪" + "\n————————————————————————";
                lbxKomentarzeDoPosta.Items.Add(komentarz);
            }

            if (lbxKomentarzeDoPosta.Items.Count == 0)
            {
                btnZbanujKomentujacego.IsEnabled = false;
                btnUsunKom.IsEnabled = false;
                btnZachowajKom.IsEnabled = false;
            }
            else
            {
                btnZbanujKomentujacego.IsEnabled = true;
                btnUsunKom.IsEnabled = true;
                btnZachowajKom.IsEnabled = true;
            }
        }
        //public void wyświetlOdpowiedziDlaAdmina()
        //{
        //    List<int> idKomentujących = new List<int>();
        //    List<string> nazwyKomentujących = new List<string>();
        //    List<string> treśćKomentarzy = new List<string>();
        //    List<string> dataKomentarza = new List<string>();
        //    using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
        //    {


        //            polaczenie.Open();
        //            SqlCommand pobierzKomentarze = new SqlCommand($"SELECT idUżytkownika from OdpowiedziKom where czyZgłoszony = '{1}'", polaczenie);
        //            SqlDataReader idUżytkowników = pobierzKomentarze.ExecuteReader();
        //            while (idUżytkowników.Read())
        //            {
        //                idKomentujących.Add(Convert.ToInt32(idUżytkowników["idUżytkownika"]));
        //            }
        //            polaczenie.Close();



        //        for (int i = 0; i < idKomentujących.Count; i++)
        //        {
        //            polaczenie.Open();

        //            SqlCommand pobierzNazwyKomentujących = new SqlCommand($"SELECT nazwaUżytkownika from Użytkownik where IdUżytkownika = '{idKomentujących[i]}'", polaczenie);
        //            SqlDataReader nazwaKomentującego = pobierzNazwyKomentujących.ExecuteReader();
        //            while (nazwaKomentującego.Read())
        //            {
        //                nazwyKomentujących.Add(Convert.ToString(nazwaKomentującego["nazwaUżytkownika"]));
        //            }
        //            polaczenie.Close();
        //        }



        //            for (int i = 0; i < idKomentujących.Count; i++)
        //            {
        //                polaczenie.Open();

        //                SqlCommand pobierzTreśćKomentarzy = new SqlCommand($"SELECT treść from OdpowiedziKom where idUżytkownika = '{idKomentujących[i]}' AND czyZgłoszony = '{1}'", polaczenie);
        //                SqlDataReader treśćKomentarza = pobierzTreśćKomentarzy.ExecuteReader();
        //                while (treśćKomentarza.Read())
        //                {
        //                    treśćKomentarzy.Add(Convert.ToString(treśćKomentarza["treść"]));
        //                }
        //                polaczenie.Close();
        //            }



        //            for (int i = 0; i < idKomentujących.Count; i++)
        //            {
        //                polaczenie.Open();

        //                SqlCommand pobierzDateKomentarzy = new SqlCommand($"SELECT dataDodania from OdpowiedziKom where idUżytkownika = '{idKomentujących[i]}'", polaczenie);
        //                SqlDataReader dataKometarza = pobierzDateKomentarzy.ExecuteReader();
        //                while (dataKometarza.Read())
        //                {
        //                    dataKomentarza.Add(Convert.ToString(dataKometarza["dataDodania"]));
        //                }
        //                polaczenie.Close();
        //            }



        //    }


        //    for (int i = 0; i < idKomentujących.Count; i++)
        //    {
        //        string komentarz = $"👦🏼: " + nazwyKomentujących[i] + "       🕒: " + dataKomentarza[i] + "\n💬: " + treśćKomentarzy[i] + "↪" + "\n————————————————————————";
        //        lbxKomentarzeDoPosta.Items.Add(komentarz);
        //    }

        //    if (lbxKomentarzeDoPosta.Items.Count == 0)
        //    {
        //        btnZbanujKomentujacego.IsEnabled = false;
        //        btnUsunKom.IsEnabled = false;
        //        btnZachowajKom.IsEnabled = false;
        //    }
        //    else
        //    {
        //        btnZbanujKomentujacego.IsEnabled = true;
        //        btnUsunKom.IsEnabled = true;
        //        btnZachowajKom.IsEnabled = true;
        //    }
        //}

        public void czyśćKomentarze()
        {
            lbxKomentarzeDoPosta.Items.Clear();
        }
        public int IdAktualnegoKomentarza()
        {
                string komentarzyk = lbxKomentarzeDoPosta.SelectedItem.ToString();
                string nazwaKomentującego = "";
                char znak = 'x';
                bool czyOdp = false;
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
                while (znak != '\udcac')
                {
                    znak = komentarzyk[i];
                    i++;
                }
                i = i + 2;

                znak = 'x';
                while (znak != '\n')
                {

                    znak = komentarzyk[i];
                    if (znak == '\n') break;
                if (znak == Convert.ToChar("↪")) { czyOdp = true; break; }
                    treśćKoma = treśćKoma + znak;
                    i++;
                }

                using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                if (czyOdp)
                {
                    polaczenie.Open();
                    SqlCommand pobierzIdKomentującego = new SqlCommand($"SELECT IdUżytkownika from Użytkownik where nazwaUżytkownika = '{nazwaKomentującego}'", polaczenie);
                    int idKomentujacego = (int)pobierzIdKomentującego.ExecuteScalar();
                    polaczenie.Close();
                    polaczenie.Open();
                    SqlCommand pobierzIdKomentarza = new SqlCommand($"SELECT idodpowiedziKom from OdpowiedziKom where treść = '{treśćKoma}' AND idUżytkownika = '{idKomentujacego}'", polaczenie);
                    SqlDataReader czytnik = pobierzIdKomentarza.ExecuteReader();
                    int idKomenta = 0;
                    while (czytnik.Read())
                    {
                        idKomenta = Convert.ToInt32(czytnik["idodpowiedziKom"]);
                    }
                    polaczenie.Close();

                    return idKomenta;
                }
                else
                {
                    polaczenie.Open();
                    SqlCommand pobierzIdKomentującego = new SqlCommand($"SELECT IdUżytkownika from Użytkownik where nazwaUżytkownika = '{nazwaKomentującego}'", polaczenie);
                    int idKomentujacego = (int)pobierzIdKomentującego.ExecuteScalar();
                    polaczenie.Close();
                    polaczenie.Open();
                    SqlCommand pobierzIdKomentarza = new SqlCommand($"SELECT idKomentarze from Komentarze where treść = '{treśćKoma}' AND idUżytkownika = '{idKomentujacego}'", polaczenie);
                    SqlDataReader czytnik = pobierzIdKomentarza.ExecuteReader();
                    int idKomenta = 0;
                    while (czytnik.Read())
                    {
                        idKomenta = Convert.ToInt32(czytnik["idKomentarze"]);
                    }
                    polaczenie.Close();

                    return idKomenta;
                }
                }
            

        }
      
        
        private void btnZglosKomentarz_Click(object sender, RoutedEventArgs e)
        {
            int idKomentarza = 0;
            if (lbxKomentarzeDoPosta.SelectedIndex != -1)
            {
               
                idKomentarza = IdAktualnegoKomentarza();

                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zgłosić ten komentarz?", "Zgłaszanie", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {

                    using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                    {

                        polaczenie.Open();
                        SqlCommand ZgłoszenieKomentarza = new SqlCommand($"UPDATE Komentarze SET czyZgłoszony = '{1}' where idKomentarze = '{idKomentarza}'", polaczenie);
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



        private void lbxKomentarzeDoPosta_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!czyAdmin())
            { 
            string komentarz = lbxKomentarzeDoPosta.SelectedItem.ToString();
            int idKomentarza = IdAktualnegoKomentarza();
            int index = lbxKomentarzeDoPosta.SelectedIndex;
            int idUż = getId(Login);
            var dodawanieOdp = new OdpowiedziKomentarze(komentarz, idKomentarza, idUż);
            dodawanieOdp.ShowDialog();
            }
           
        }

        private void txtWyszukiwarka_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtWyszukiwarka.Text.Length > 0)
            {
                if (txtWyszukiwarka.Text[0] == '#')
                {
                   
                    RbPost.Visibility = Visibility.Visible;
                    RbUzytkownik.Visibility = Visibility.Visible;
                    if(RbPost.IsChecked == true)
                    {
                        btnWyszukiwarka.IsEnabled = true;
                    }
                    else btnWyszukiwarka.IsEnabled = false;
                }
                else
                {
                   
                    RbPost.Visibility = Visibility.Hidden;
                    RbUzytkownik.Visibility = Visibility.Hidden;
                    btnWyszukiwarka.IsEnabled = true;
                    
                    RbPost.IsChecked = false;
                    RbUzytkownik.IsChecked = false;
                }
            }
            else {
                RbPost.Visibility = Visibility.Hidden;
                RbUzytkownik.Visibility = Visibility.Hidden;
                RbPost.IsChecked = false;
                RbUzytkownik.IsChecked = false;
                indeksator = 0;
                List<Post> pomoc = postyDoWyswietlenia();
                if (pomoc.Count == 1 || pomoc.Count == 0) { btnNastepnyPost.IsEnabled = false; }
                else btnNastepnyPost.IsEnabled = true;
                btnPoprzedniPost.IsEnabled = false;
                wyswietlPostyNaGlownej(indeksator); }
        }

        private void RbUzytkownik_Checked(object sender, RoutedEventArgs e)
        {
            btnWyszukiwarka.IsEnabled = true;
        }

        private void RbPost_Checked(object sender, RoutedEventArgs e)
        {
            btnWyszukiwarka.IsEnabled = true;
        }

        private void RbKomentarz_Checked(object sender, RoutedEventArgs e)
        {
            btnWyszukiwarka.IsEnabled = true;
        }

        private void btnUsunKom_Click(object sender, RoutedEventArgs e)
        {

            int idKomentarza = 0;
            if (lbxKomentarzeDoPosta.SelectedIndex != -1)
            {
                string komentarzyk = lbxKomentarzeDoPosta.SelectedItem.ToString();
                if (komentarzyk.ElementAt(komentarzyk.Length - 26) == (Convert.ToChar("↪")))
                {
                    idKomentarza = IdAktualnegoKomentarza();

                    using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                    {

                        polaczenie.Open();
                        SqlCommand UsuniecieKomentarza = new SqlCommand($"DELETE from OdpowiedziKom where idodpowiedziKom = '{idKomentarza}'", polaczenie);
                        UsuniecieKomentarza.ExecuteNonQuery();
                        polaczenie.Close();
                    }
                    lbxKomentarzeDoPosta.Items.RemoveAt(lbxKomentarzeDoPosta.SelectedIndex);
                    if (lbxKomentarzeDoPosta.Items.Count == 0)
                    {
                        btnZbanujKomentujacego.IsEnabled = false;
                        btnUsunKom.IsEnabled = false;
                        btnZachowajKom.IsEnabled = false;
                    }
                    else
                    {
                        btnZbanujKomentujacego.IsEnabled = true;
                        btnUsunKom.IsEnabled = true;
                        btnZachowajKom.IsEnabled = true;
                    }
                }
                else
                {
                    idKomentarza = IdAktualnegoKomentarza();

                    using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                    {

                        polaczenie.Open();
                        SqlCommand UsuniecieKomentarza = new SqlCommand($"DELETE from Komentarze where idKomentarze = '{idKomentarza}'", polaczenie);
                        UsuniecieKomentarza.ExecuteNonQuery();
                        polaczenie.Close();
                    }
                    lbxKomentarzeDoPosta.Items.RemoveAt(lbxKomentarzeDoPosta.SelectedIndex);
                    if (lbxKomentarzeDoPosta.Items.Count == 0)
                    {
                        btnZbanujKomentujacego.IsEnabled = false;
                        btnUsunKom.IsEnabled = false;
                        btnZachowajKom.IsEnabled = false;
                    }
                    else
                    {
                        btnZbanujKomentujacego.IsEnabled = true;
                        btnUsunKom.IsEnabled = true;
                        btnZachowajKom.IsEnabled = true;
                    }
                }
               
            }
            else MessageBox.Show("Nie wybrano komentarza!");
        }

        private void btnZbanujKomentujacego_Click(object sender, RoutedEventArgs e)
        {

            int idKomentarza = 0;

            if (lbxKomentarzeDoPosta.SelectedIndex != -1)
            {
                
                string komentarzyk = lbxKomentarzeDoPosta.SelectedItem.ToString();
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
                int idKomentujacego = 0;
                using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {

                    polaczenie.Open();
                    SqlCommand pobierzIdKomentującego = new SqlCommand($"SELECT IdUżytkownika from Użytkownik where nazwaUżytkownika = '{nazwaKomentującego}'", polaczenie);
                    idKomentujacego = (int)pobierzIdKomentującego.ExecuteScalar();
                    polaczenie.Close();
                }

                using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
               
                polaczenie.Open();
                SqlCommand banowanieUzytkownika = new SqlCommand($"UPDATE Użytkownik SET czyZbanowany = '{1}' where IdUżytkownika = '{idKomentujacego}'", polaczenie);
                banowanieUzytkownika.ExecuteNonQuery();
                polaczenie.Close();
                lbxKomentarzeDoPosta.Items.RemoveAt(lbxKomentarzeDoPosta.SelectedIndex);

                }
                usunRzeczyZbanowanego(idKomentujacego);
                if (lbxKomentarzeDoPosta.Items.Count == 0)
                {
                    btnZbanujKomentujacego.IsEnabled = false;
                    btnUsunKom.IsEnabled = false;
                    btnZachowajKom.IsEnabled = false;
                }
                else
                {
                    btnZbanujKomentujacego.IsEnabled = true;
                    btnUsunKom.IsEnabled = true;
                    btnZachowajKom.IsEnabled = true;
                }
            }
            else MessageBox.Show("Nie wybrano komentarza!");

        }

        private void btnZachowajKom_Click(object sender, RoutedEventArgs e)
        {

            
            if (lbxKomentarzeDoPosta.SelectedIndex != -1)
            {
                int idKomentarza = IdAktualnegoKomentarza();
                string komentarzyk = lbxKomentarzeDoPosta.SelectedItem.ToString();
            lbxKomentarzeDoPosta.Items.RemoveAt(lbxKomentarzeDoPosta.SelectedIndex);

            if (komentarzyk.ElementAt(komentarzyk.Length - 26) == (Convert.ToChar("↪")))
            {
                using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    polaczenie.Open();
                    SqlCommand UsuniecieKomentarza = new SqlCommand($"UPDATE OdpowiedziKom SET czyZgłoszony = '{0}' where idodpowiedziKom = '{idKomentarza}'", polaczenie);
                    UsuniecieKomentarza.ExecuteNonQuery();
                    polaczenie.Close();
                }
                if (lbxKomentarzeDoPosta.Items.Count == 0)
                {
                    btnZbanujKomentujacego.IsEnabled = false;
                    btnUsunKom.IsEnabled = false;
                    btnZachowajKom.IsEnabled = false;
                }
                else
                {
                    btnZbanujKomentujacego.IsEnabled = true;
                    btnUsunKom.IsEnabled = true;
                    btnZachowajKom.IsEnabled = true;
                }
            }
            else
            {
                using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {

                    polaczenie.Open();
                    SqlCommand UsuniecieKomentarza = new SqlCommand($"UPDATE Komentarze SET czyZgłoszony = '{0}' where idKomentarze = '{idKomentarza}'", polaczenie);
                    UsuniecieKomentarza.ExecuteNonQuery();
                    polaczenie.Close();
                }
                if (lbxKomentarzeDoPosta.Items.Count == 0)
                {
                    btnZbanujKomentujacego.IsEnabled = false;
                    btnUsunKom.IsEnabled = false;
                    btnZachowajKom.IsEnabled = false;
                }
                else
                {
                    btnZbanujKomentujacego.IsEnabled = true;
                    btnUsunKom.IsEnabled = true;
                    btnZachowajKom.IsEnabled = true;
                }
            }
            }
            else MessageBox.Show("Nie wybrano komentarza!");
        }

        public void usunRzeczyZbanowanego(int id)
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {

                polaczenie.Open();
                SqlCommand UsuniecieKomentarza = new SqlCommand($"DELETE from Komentarze where idUżytkownika = '{id}'", polaczenie);
                UsuniecieKomentarza.ExecuteNonQuery();
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand UsuniecieOdp = new SqlCommand($"DELETE from OdpowiedziKom where idUżytkownika = '{id}'", polaczenie);
                UsuniecieOdp.ExecuteNonQuery();
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand postyZbanowanego = new SqlCommand($"SELECT idPosta from Post where idAutora = '{id}'", polaczenie);
                List<int> idPostów = new List<int>();
                SqlDataReader czytnik = postyZbanowanego.ExecuteReader();
                while(czytnik.Read())
                {
                    idPostów.Add(Convert.ToInt32(czytnik["idPosta"]));
                }
                polaczenie.Close();

                for (int i = 0; i < idPostów.Count; i++)
                {
                    polaczenie.Open();
                    SqlCommand UsunieciePostów = new SqlCommand($"DELETE from Tagi where idTagu = '{idPostów[i]}' AND post = '{1}'", polaczenie);
                    UsunieciePostów.ExecuteNonQuery();
                    polaczenie.Close();
                }
            }
            usunTagProfilu(id);
        }
        public void usunTagProfilu(int idU)
        {
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand UsuniecieTagu = new SqlCommand($"DELETE from Tagi where idTagu = '{idU}' AND użytkownik = '{1}'", polaczenie);
                UsuniecieTagu.ExecuteNonQuery();
                polaczenie.Close();
            }
        }

        private void btnMojePosty_Click(object sender, RoutedEventArgs e)
        {
            if (czyMojePosty == true) { czyMojePosty = false; btnMojePosty.Background = System.Windows.Media.Brushes.Blue;
                btnUsunMojPost.Visibility = Visibility.Hidden;
            }
            else if (czyMojePosty == false) { czyMojePosty = true; btnMojePosty.Background = System.Windows.Media.Brushes.LightGreen;
                btnUsunMojPost.Visibility = Visibility.Visible;
            }
            indeksator = 0;
            odswiezenieOknaGlownego();
            wyswietlPostyNaGlownej(0);
            btnPoprzedniPost.IsEnabled = false;

            List<Post> pomoc = postyDoWyswietlenia();
            if (pomoc.Count == 1 || pomoc.Count == 0) { btnNastepnyPost.IsEnabled = false; }
            else btnNastepnyPost.IsEnabled = true;
            if (czyZgloszony())
            {
                btnZglosPost.Background = System.Windows.Media.Brushes.Red;
                btnZglosPost.Content = "Zgłoszony";
            }
            else
            {
                btnZglosPost.Background = System.Windows.Media.Brushes.Blue;
                btnZglosPost.Content = "Zgłoś post";
            }

        }

        private void btnUsunMojPost_Click(object sender, RoutedEventArgs e)
        {
            int id = IdAktualnegoPosta();
            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand UsuwanieL = new SqlCommand($"DELETE from Polubienia where idPosta = {id}", polaczenie);
                UsuwanieL.ExecuteNonQuery();
                polaczenie.Close();
                polaczenie.Open();
                SqlCommand UsuwanieK = new SqlCommand($"DELETE from Komentarze where idPosta = {id}", polaczenie);
                UsuwanieK.ExecuteNonQuery();
                polaczenie.Close();


                polaczenie.Open();
                SqlCommand UsunieciePosta = new SqlCommand($"DELETE from Post where idPosta = '{id}' AND idAutora = '{getId(Login)}'", polaczenie);
                UsunieciePosta.ExecuteNonQuery();
                polaczenie.Close();
            }
            wypelnijPosty();
            indeksator = 0;
            btnPoprzedniPost.IsEnabled = false;
            odswiezenieOknaGlownego();
            interfejs();
            wyswietlPostyNaGlownej(0);
            List<Post> pomoc = postyDoWyswietlenia();
            if (pomoc.Count == 1 || pomoc.Count == 0) { btnNastepnyPost.IsEnabled = false; }
            else btnNastepnyPost.IsEnabled = true;

        }

        private void btnRaport_Click(object sender, RoutedEventArgs e)
        {
            int liczbaNajwiecejKomntarzyDoJednegoPosta = 0, liczbaLikow = 0, NajLik = 0, najKom = 0;
            DateTime? najstarszyV = null, najnowszyV = null, najstarszyPost = null, najstarszydodanyKom = null, najnowszyKom = null, najnowszyPost = null;
            string pol = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using (SqlConnection polaczenie = new SqlConnection(pol))
            {
                polaczenie.Open();
                SqlCommand listaUzytkownikow = new SqlCommand($"SELECT COUNT(*) from Użytkownik  ", polaczenie);
                int liczbaUzytkownikow = (Int32)listaUzytkownikow.ExecuteScalar();
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand listaPostow = new SqlCommand($"SELECT COUNT(*) from Post  ", polaczenie);
                int liczbaPostow = (Int32)listaPostow.ExecuteScalar();
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand listaTagow = new SqlCommand($"SELECT COUNT(*) from Tagi  ", polaczenie);
                int liczbaTagow = (Int32)listaTagow.ExecuteScalar();
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand listaKomentarzy = new SqlCommand($"SELECT COUNT(*) from Komentarze  ", polaczenie);
                int liczbaKomentarzy = (Int32)listaKomentarzy.ExecuteScalar();
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand listaOdpKomentarzy = new SqlCommand($"SELECT COUNT(*) from OdpowiedziKom  ", polaczenie);
                int liczbaOdpKomentarzy = (Int32)listaOdpKomentarzy.ExecuteScalar();
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand listaVipow = new SqlCommand($"SELECT COUNT(*) from SubVip  ", polaczenie);
                int liczbaVipow = (Int32)listaVipow.ExecuteScalar();
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand listaPolubień = new SqlCommand($"SELECT COUNT(*) from Polubienia  ", polaczenie);
                int liczbaPolubień = (Int32)listaPolubień.ExecuteScalar();
                polaczenie.Close();

                polaczenie.Open();
                SqlCommand listaObserwacji = new SqlCommand($"SELECT COUNT(*) from Obserwujący  ", polaczenie);
                int liczbaObserwacji = (Int32)listaObserwacji.ExecuteScalar();
                polaczenie.Close();


                if (liczbaKomentarzy > 0)
                {
                    polaczenie.Open();
                    SqlCommand listaNajwiecejKomentarzyDoJednegoPosta = new SqlCommand($"SELECT COUNT(idPosta) from Komentarze GROUP BY idPosta ORDER BY COUNT(idPosta) DESC  ", polaczenie);
                     liczbaNajwiecejKomntarzyDoJednegoPosta = (Int32)listaNajwiecejKomentarzyDoJednegoPosta.ExecuteScalar();
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlCommand najnowszyDodanyKom = new SqlCommand($"SELECT dataDodania from Komentarze WHERE dataDodania = (SELECT MAX(dataDodania) from Komentarze) ", polaczenie);
                     najnowszyKom = (DateTime)najnowszyDodanyKom.ExecuteScalar();
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlCommand najstarszyDodanyKom = new SqlCommand($"SELECT dataDodania from Komentarze WHERE dataDodania = (SELECT MIN(dataDodania) from Komentarze) ", polaczenie);
                     najstarszydodanyKom = (DateTime)najstarszyDodanyKom.ExecuteScalar();
                    polaczenie.Close();
                }
                if (liczbaPolubień > 0)
                {
                    polaczenie.Open();
                    SqlCommand listaNajwiecejLikeDoJednegoPosta = new SqlCommand($"SELECT COUNT(idPosta) from Polubienia GROUP BY idPosta ORDER BY COUNT(idPosta) DESC  ", polaczenie);
                     liczbaLikow = (Int32)listaNajwiecejLikeDoJednegoPosta.ExecuteScalar();
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlCommand opZNajwiekszailosciaLike = new SqlCommand($"SELECT COUNT(idUżytkownika) from Polubienia GROUP BY idUżytkownika ORDER BY COUNT(idUżytkownika) DESC ", polaczenie);
                     NajLik = (Int32)opZNajwiekszailosciaLike.ExecuteScalar();
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlCommand opZNajwiekszailosciaKom = new SqlCommand($"SELECT COUNT(idUżytkownika) from Komentarze GROUP BY idUżytkownika ORDER BY COUNT(idUżytkownika) DESC ", polaczenie);
                     najKom = (Int32)opZNajwiekszailosciaKom.ExecuteScalar();
                    polaczenie.Close();
                }

                if (liczbaVipow > 0)
                {
                    polaczenie.Open();
                    SqlCommand najstarszyVip = new SqlCommand($"SELECT * from SubVip WHERE dataZakupu = (SELECT MIN(dataZakupu) from SubVip)", polaczenie);
                     najstarszyV = (DateTime)najstarszyVip.ExecuteScalar();
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlCommand najnowszyVip = new SqlCommand($"SELECT * from SubVip WHERE dataZakupu = (SELECT MAX(dataZakupu) from SubVip) ", polaczenie);
                     najnowszyV = (DateTime)najnowszyVip.ExecuteScalar();
                    polaczenie.Close();
                }


                if (liczbaPostow > 0)
                {
                    polaczenie.Open();
                    SqlCommand najnowszyDodanyPost = new SqlCommand($"SELECT dataDodania from Post WHERE dataDodania = (SELECT MAX(dataDodania) from Post) ", polaczenie);
                     najnowszyPost = (DateTime)najnowszyDodanyPost.ExecuteScalar();
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlCommand najstarszyDodanyPost = new SqlCommand($"SELECT dataDodania from Post WHERE dataDodania = (SELECT MIN(dataDodania) from Post) ", polaczenie);
                     najstarszyPost = (DateTime)najstarszyDodanyPost.ExecuteScalar();
                    polaczenie.Close();
                }
                
                FileStream fs = new FileStream(@"raport.txt", FileMode.Create, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine("Data sworzenia raportu: " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss") + "\n");
                sw.WriteLine("Aktualna liczba użytkowników to: " + liczbaUzytkownikow + "\n");
                sw.WriteLine("Aktualna liczba postów to: " + liczbaPostow + "\n");
                sw.WriteLine("Aktualna liczba użyć tagów to: " + liczbaTagow + "\n");
                sw.WriteLine("Aktualna liczba komentarzy to: " + liczbaKomentarzy + "\n");
                sw.WriteLine("Aktualna liczba odpowiedzi do komentarzy: " + liczbaOdpKomentarzy + "\n");
                sw.WriteLine("Aktualna liczba posiadaczy VIPa: " + liczbaVipow + "\n");
                sw.WriteLine("Aktualna liczba polubień: " + liczbaPolubień + "\n");
                sw.WriteLine("Największa ilość komentarzy do jednego posta: " + liczbaNajwiecejKomntarzyDoJednegoPosta + "\n");
                sw.WriteLine("Największa ilość like do jednego posta: " + liczbaLikow + "\n");
                if (liczbaVipow > 0)
                {
                    sw.WriteLine("Najstarszy Vip został zakupiony: " + najstarszyV + "\n");
                    sw.WriteLine("Najnowszy Vip został zakupiony: " + najnowszyV + "\n");
                }
                if (liczbaKomentarzy > 0)
                {
                    sw.WriteLine("Najstarszy dodany komentarz: " + najstarszydodanyKom + "\n");
                    sw.WriteLine("Najnowszy dodany komentarz: " + najnowszyKom + "\n");
                }
                if (liczbaPostow > 0)
                {
                    sw.WriteLine("Najstarszy dodany post: " + najstarszyPost + "\n");
                    sw.WriteLine("Najnowszy dodany post: " + najnowszyPost + "\n");
                }
                if (liczbaLikow > 0)
                {
                    sw.WriteLine("Największa liczba like zostawiona przez jednego użytkownika: " + NajLik + "\n");
                    sw.WriteLine("Największa liczba komentarzy zostawiona przez jednego użytkownika: " + najKom + "\n");
                }
                sw.Close();


            }
        }
    }
}