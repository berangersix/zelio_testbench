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
    /// Logique d'interaction pour Yellow_light_bulb.xaml
    /// </summary>
    public partial class Yellow_light_bulb : UserControl
    {
        public Yellow_light_bulb()
        {
            InitializeComponent();
        }

        public void Update_bulb(bool value)
        {
            this.Yellow_red_bulb.IsChecked = value;
        }

        public void Set_callback(Output_zelio oz)
        {
            oz.Add_callback(Update_bulb);
        }
    }
}

