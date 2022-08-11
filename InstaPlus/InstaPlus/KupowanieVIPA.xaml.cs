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

namespace PBD_działające__y_
{
    /// <summary>
    /// Logika interakcji dla klasy KupowanieVIPA.xaml
    /// </summary>
    public partial class KupowanieVIPA : Window
    {

        public string Login { set; get; }

        public KupowanieVIPA()
        {
            InitializeComponent();
        }

        public KupowanieVIPA(string uzytkownik):this()
        {
            Login = uzytkownik;
        }


        public enum RodzajVIPA { miesiąc, pół_roku, rok };

        private void btnVIPnaMiesiac_Click(object sender, RoutedEventArgs e)
        {
            RodzajVIPA m = RodzajVIPA.rok;
            var kupnoVipa = new ZapłataZaVIP(m,Login);
            kupnoVipa.ShowDialog();
            this.Close();

        }

        private void btnAnulujZakup_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnVIPnaPółRoku_Click(object sender, RoutedEventArgs e)
        {
            RodzajVIPA m = RodzajVIPA.pół_roku;
            var kupnoVipa = new ZapłataZaVIP(m,Login);
            kupnoVipa.ShowDialog();
            this.Close();
        }

        private void btnVIPnaMiesiac_Click_1(object sender, RoutedEventArgs e)
        {
            RodzajVIPA m = RodzajVIPA.miesiąc;
            var kupnoVipa = new ZapłataZaVIP(m,Login);
            kupnoVipa.ShowDialog();
            this.Close();
        }
    }
}
