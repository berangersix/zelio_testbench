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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace zelio_testbench.Chateau_eau
{
    /// <summary>
    /// Logique d'interaction pour Flotteur_niveau_haut.xaml
    /// </summary>
    public partial class Flotteur_niveau_haut : UserControl
    {
        public Flotteur_niveau_haut()
        {
            InitializeComponent();
        }

        public void Set_state(bool value)
        {
            Floteur_Haut.IsChecked = value;
        }
        public bool Get_state()
        {
            return Floteur_Haut.IsChecked.Value;
        }
    }
}
