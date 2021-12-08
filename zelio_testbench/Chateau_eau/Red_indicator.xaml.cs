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
    /// Logique d'interaction pour Red_indicator.xaml
    /// </summary>
    public partial class Red_indicator : UserControl
    {
        public Red_indicator()
        {
            InitializeComponent();
        }

        public void Set_state(bool value)
        {
            if (value != light_red.IsChecked.Value) light_red.IsChecked = value;
        }
    }
}
