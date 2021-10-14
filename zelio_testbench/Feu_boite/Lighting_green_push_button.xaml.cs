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
    /// Logique d'interaction pour lighting_green_push_button.xaml
    /// </summary>
    public partial class Lighting_green_push_button : UserControl
    {
        private  Input_zelio attached_input = null;

        public Lighting_green_push_button()
        {
            InitializeComponent();

        }

        public void Set_light_on()
        {
            push_button_light.Style = (Style)FindResource("Style_Geen_ind_light_on");
        }

        public void Set_light_off()
        {
            push_button_light.Style = (Style)FindResource("Style_Geen_ind_light_off");
        }

        public void Update_bulb(bool value)
        {
            if (value) Set_light_on();
            else Set_light_off();
        }

        public void Set_callback(Output_zelio oz)
        {
            oz.Add_callback(Update_bulb);
        }


        public void Set_input(Input_zelio iz)
        {
            attached_input = iz;
        }

        private void Check_event(object sender, RoutedEventArgs e)
        {
            attached_input.Set_value(true);
        }

        private void Uncheck_event(object sender, RoutedEventArgs e)
        {
            attached_input.Set_value(false);
        }
    }
}

