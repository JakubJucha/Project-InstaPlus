using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Data.SqlTypes;

namespace PBD_działające__y_
{
    /// <summary>
    /// Logika interakcji dla klasy WstawianiePosta.xaml
    /// </summary>
    public partial class WstawianiePosta : Window
    {
        public string Login { set; get; }
        public BitmapImage ZdjecieWPoscie { get; set; }
        
        public WstawianiePosta(String login)
        {
            InitializeComponent();
            this.Login = login;
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
        private void btnDodajZdjecie_Click(object sender, RoutedEventArgs e)
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
            imgZdjeciePosta.Source = bitmapa;
            ZdjecieWPoscie = bitmapa;
        }


        public List<string> WykryjTag()
        {
            string trescOpisu = txtOpisPosta.Text;
            List<string> tagi = new List<string>();
            string tag = "";
            char znak = 'x';
            int i = 0;

            if (trescOpisu.Length > 0)
            {
                while (true)
                {
                    if (i == trescOpisu.Length - 1) break;
                    while (znak != '#')
                    {
                        znak = trescOpisu[i];
                        i++;
                        if (i == trescOpisu.Length - 1) break;
                    }
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
            return tagi;
            
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
        private void btnDodajPost_Click(object sender, RoutedEventArgs e)
        {
            List<string> tagiWPoscie = WykryjTag();
            int licznikTagow = tagiWPoscie.Count;

            if (imgZdjeciePosta.Source != null)
            {
                using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    polaczenie.Open();
                    SqlCommand idAutora = new SqlCommand($"SELECT IdUżytkownika from Użytkownik where login = '{Login}'", polaczenie);
                    int id = Convert.ToInt32(idAutora.ExecuteScalar());
                    polaczenie.Close();

                    polaczenie.Open();
                    SqlCommand dodaniePostaDoBazy = new SqlCommand("INSERT into Post(opis,dataDodania,idAutora) VALUES(@opis,@dataDodania,@idAutora)", polaczenie);
                    dodaniePostaDoBazy.Parameters.Add("opis", System.Data.SqlDbType.VarChar).Value = txtOpisPosta.Text.ToString();

                    SqlDateTime myDateTime = DateTime.Now;
                    dodaniePostaDoBazy.Parameters.Add("dataDodania", System.Data.SqlDbType.DateTime).Value = myDateTime;
                    dodaniePostaDoBazy.Parameters.Add("idAutora", System.Data.SqlDbType.Int).Value = id;
                    dodaniePostaDoBazy.ExecuteNonQuery();
                    polaczenie.Close();

                    polaczenie.Open();
                    string polecenie = $"UPDATE Post SET zdjęcie = @obrazek WHERE idAutora = '{id}' and zdjęcie is null ";
                    SqlCommand dodanieZdjecia = new SqlCommand(polecenie, polaczenie);
                    SqlParameter imageParameter = dodanieZdjecia.Parameters.Add("@obrazek", SqlDbType.Binary);

                    var content = BitmapImage2Bitmap(ZdjecieWPoscie);

                    imageParameter.Value = content;
                    imageParameter.Size = content.Length;
                    dodanieZdjecia.ExecuteNonQuery();
                    polaczenie.Close();

                    if (licznikTagow > 0)
                    {
                        polaczenie.Open();
                        SqlCommand pobranieIdPostaDoKtoregoPrzypisanyJestTag = new SqlCommand($"SELECT idPosta from Post where opis = '{txtOpisPosta.Text.ToString()}' and idAutora = '{id}'", polaczenie);
                        int idPostaZTagiem = Convert.ToInt32(pobranieIdPostaDoKtoregoPrzypisanyJestTag.ExecuteScalar());
                        polaczenie.Close();

                        for (int i = 0; i < licznikTagow; i++)
                        {
                            string trescTaga = tagiWPoscie[i].ToString();
                            polaczenie.Open();
                            SqlCommand dodanieTagaDoBazy = new SqlCommand("INSERT into Tagi(idTagu,treść,post) VALUES(@idtagu,@tresc,@post)", polaczenie);
                            dodanieTagaDoBazy.Parameters.Add("idtagu", System.Data.SqlDbType.Int).Value = idPostaZTagiem;
                            dodanieTagaDoBazy.Parameters.Add("tresc", System.Data.SqlDbType.VarChar).Value = trescTaga;
                            dodanieTagaDoBazy.Parameters.Add("post", System.Data.SqlDbType.Bit).Value = 1;
                            dodanieTagaDoBazy.ExecuteNonQuery();
                            polaczenie.Close();
                        }
                    }
                }
                this.Close();
            }
            else MessageBox.Show("Musisz wybrać zdjęcie!");
        }
    }
}
