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
    /// Logique d'interaction pour Two_pos_switch.xaml
    /// </summary>
    public partial class Two_pos_switch : UserControl
    {
        public Two_pos_switch()
        {
            InitializeComponent();
        }

        private void Check_event(object sender, RoutedEventArgs e)
        {

        }

        private void Uncheck_event(object sender, RoutedEventArgs e)
        {

        }

        public bool Get_state()
        {
            return two_pos_sw.IsChecked.Value;
        }
    }
}
