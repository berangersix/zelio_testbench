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
using zelio_testbench.debug_hmi;

namespace zelio_testbench.Feu_boite
{
    /// <summary>
    /// Logique d'interaction pour two_position_switch.xaml
    /// </summary>
    public partial class two_position_switch : UserControl
    {
        private Input_zelio attached_input_1 = null;
        private Input_zelio attached_input_2 = null;
        public two_position_switch()
        {
            InitializeComponent();
        }

        public void Set_input(Input_zelio iz1, Input_zelio iz2)
        {
            attached_input_1 = iz1;
            attached_input_2 = iz2;
        }

        private void Check_event(object sender, RoutedEventArgs e)
        {
            //mode nuit
            attached_input_1.Set_value(false);
            attached_input_2.Set_value(true);
        }

        private void Uncheck_event(object sender, RoutedEventArgs e)
        {
            //mode jour
            attached_input_1.Set_value(true);
            attached_input_2.Set_value(false);
        }
    }
}
