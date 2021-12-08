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
    /// Logique d'interaction pour Flotteur_niveau_bas.xaml
    /// </summary>
    public partial class Flotteur_niveau_bas : UserControl
    {
        public Flotteur_niveau_bas()
        {
            InitializeComponent();
        }

        public void Set_state(bool value)
        {
            Flotteur_Bas.IsChecked = value;
        }
        public bool Get_state()
        {
            return Flotteur_Bas.IsChecked.Value;
        }
    }
}
