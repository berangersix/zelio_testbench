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
    /// Logique d'interaction pour Flotteur_50E3.xaml
    /// </summary>
    public partial class Flotteur_50E3 : UserControl
    {
        public Flotteur_50E3()
        {
            InitializeComponent();
        }

        public void Set_state(bool value)
        {
            Floteur_50E3.IsChecked = value;
        }
        public bool Get_state()
        {
            return Floteur_50E3.IsChecked.Value;
        }
    }
}
