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
    /// Logique d'interaction pour Green_indicator.xaml
    /// </summary>
    public partial class Green_indicator : UserControl
    {
        public Green_indicator()
        {
            InitializeComponent();
        }
        public void Set_state(bool value)
        {
            if (value != light_green.IsChecked.Value) light_green.IsChecked = value;
        }
    }
}
