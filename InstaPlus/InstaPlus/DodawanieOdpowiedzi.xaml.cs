using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
    /// Logika interakcji dla klasy DodawanieOdpowiedzi.xaml
    /// </summary>
    public partial class DodawanieOdpowiedzi : Window
    {
        public int IdUżytkownika { set; get; }
        public int IdKomentarza { set; get; }
        public DodawanieOdpowiedzi()
        {
            InitializeComponent();
        }
        public DodawanieOdpowiedzi(int idUż, int idKom) : this()
        {
            this.IdUżytkownika = idUż;
            this.IdKomentarza = idKom;
        }

        public List<string> WykryjTag()
        {
            string trescOpisu = txtTrescOdpowiedzi.Text;
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
            MessageBox.Show(tagi[0] + " " + tagi[1]);
            return tagi;
        }
        private void btnDodajKom_Click(object sender, RoutedEventArgs e)
        {
           // WykryjTag();
            string komentarz = txtTrescOdpowiedzi.Text;
            if (!string.IsNullOrEmpty(komentarz))
            {
                SqlDateTime myDateTime = DateTime.Now;

            using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                polaczenie.Open();
                SqlCommand dodanieOdpowiedziDoBazy = new SqlCommand("INSERT into OdpowiedziKom(id_odp,treść,idUżytkownika,dataDodania) VALUES(@idodp,@tresc,@iduzytkownika,@data)", polaczenie);
                dodanieOdpowiedziDoBazy.Parameters.Add("idodp", System.Data.SqlDbType.Int).Value = IdKomentarza;
                dodanieOdpowiedziDoBazy.Parameters.Add("tresc", System.Data.SqlDbType.VarChar).Value = komentarz;
                dodanieOdpowiedziDoBazy.Parameters.Add("iduzytkownika", System.Data.SqlDbType.Int).Value = IdUżytkownika;
                dodanieOdpowiedziDoBazy.Parameters.Add("data", System.Data.SqlDbType.DateTime).Value = myDateTime;
                dodanieOdpowiedziDoBazy.ExecuteNonQuery();
                polaczenie.Close();

            }
            this.Close();
            }
            else MessageBox.Show("Komentarz nie może być pusty!");
        }

        private void btnAnuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
