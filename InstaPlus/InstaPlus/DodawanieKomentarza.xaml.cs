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
    /// Logika interakcji dla klasy DodawanieKomentarza.xaml
    /// </summary>
    public partial class DodawanieKomentarza : Window
    {

        public int IdUżytkownika { set; get; }
        public int IdPosta { set; get; }
        public DodawanieKomentarza()
        {
            InitializeComponent();
        }

        public DodawanieKomentarza(int idUż, int idPosta) : this()
        {
            this.IdUżytkownika = idUż;
            this.IdPosta = idPosta;

        }

        public List<string> WykryjTag()
        {
            string trescOpisu = txtTrescKomentarza.Text;
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
            //WykryjTag();

            string komentarz = txtTrescKomentarza.Text;

            if (!string.IsNullOrEmpty(komentarz))
            {
                SqlDateTime myDateTime = DateTime.Now;

                using (SqlConnection polaczenie = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=INSTAGRAM-;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    polaczenie.Open();
                    SqlCommand dodanieKomentarzaDoBazy = new SqlCommand("INSERT into Komentarze(idPosta,treść,idUżytkownika,dataDodania) VALUES(@idpost,@tresc,@iduzytkownika,@data)", polaczenie);
                    dodanieKomentarzaDoBazy.Parameters.Add("idpost", System.Data.SqlDbType.Int).Value = IdPosta;
                    dodanieKomentarzaDoBazy.Parameters.Add("tresc", System.Data.SqlDbType.VarChar).Value = komentarz;
                    dodanieKomentarzaDoBazy.Parameters.Add("iduzytkownika", System.Data.SqlDbType.Int).Value = IdUżytkownika;
                    dodanieKomentarzaDoBazy.Parameters.Add("data", System.Data.SqlDbType.DateTime).Value = myDateTime;
                    dodanieKomentarzaDoBazy.ExecuteNonQuery();
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
