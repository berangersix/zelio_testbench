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
    /// Logique d'interaction pour tab_feu_tricolore.xaml
    /// </summary>
    public partial class Tab_feu_tricolore : UserControl, ITP_grid
    {
        public Tab_feu_tricolore()
        {
            InitializeComponent();
        }


        /// <summary>
        /// set reference to Zelio button to child
        /// </summary>
        /// <param name="dic_o"></param>
        /// <param name="dic_i"></param>
        public void Set_dic(Dictionary<int, Output_zelio> dic_o, Dictionary<int, Input_zelio> dic_i)
        {
            if (dic_o.ContainsKey(8)) button_pieton_A.Set_callback(dic_o[8]);
            if (dic_i.ContainsKey(3)) button_pieton_A.Set_input(dic_i[3]);
            if (dic_i.ContainsKey(4)) button_pieton_B.Set_input(dic_i[4]);
            if (dic_o.ContainsKey(7)) button_pieton_B.Set_callback(dic_o[7]);

            
            if (dic_o.ContainsKey(1)) Green_light_A.Set_callback(dic_o[1]);
            if (dic_o.ContainsKey(2)) Yellow_light_A.Set_callback(dic_o[2]);
            if (dic_o.ContainsKey(3)) Red_light_A.Set_callback(dic_o[3]);

            if (dic_o.ContainsKey(4)) Green_light_B.Set_callback(dic_o[4]);
            if (dic_o.ContainsKey(5)) Yellow_light_B.Set_callback(dic_o[5]);
            if (dic_o.ContainsKey(6)) Red_light_B.Set_callback(dic_o[6]);

            if (dic_i.ContainsKey(1) && dic_i.ContainsKey(2)) sw_nigth_day.Set_input(dic_i[1], dic_i[2]);
        }

        //event when button played is clicked
        public delegate void play_event(object sender, RoutedEventArgs e);
        public event play_event Played_event;
        public delegate void pause_event(object sender, RoutedEventArgs e);
        public event pause_event Paused_event;
        /// <summary>
        /// method called when played button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play_event(object sender, RoutedEventArgs e)
        {
            Played_event?.Invoke(sender, e);
        }

        /// <summary>
        /// method called when played button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pause_event(object sender, RoutedEventArgs e)
        {
            Paused_event?.Invoke(sender, e);
        }

        /// <summary>
        /// set the playbutton to a specific state
        /// </summary>
        /// <param name="value"></param>
        public void Set_played_state(bool value)
        {
            TP_button_play.button_play_template.IsChecked = value;
        }



    }
}
