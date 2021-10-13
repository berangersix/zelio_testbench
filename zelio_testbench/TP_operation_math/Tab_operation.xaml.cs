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

namespace zelio_testbench.TP_operation_math
{
    /// <summary>
    /// Student do a mathematic operation in ZElio beetween E1 and E2, and see the Result under S
    /// E1 is input byte 1 to 4 in ZElio
    /// E2  is input byte 5 to 8 in ZElio
    /// S is output byte 1 to 4 in ZElio
    /// </summary>
    public partial class Tab_operation : UserControl, ITP_grid
    {
        //event when button played is clicked
        public delegate void play_event(object sender, RoutedEventArgs e);
        public event play_event Played_event;
        public delegate void pause_event(object sender, RoutedEventArgs e);
        public event pause_event Paused_event;
        public Tab_operation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// set reference to Zelio button to child
        /// </summary>
        /// <param name="dic_o"></param>
        /// <param name="dic_i"></param>
        public void Set_dic (Dictionary<int, Output_zelio> dic_o, Dictionary<int, Input_zelio> dic_i)
        {
            S.Set_dic_output(dic_o);
            E1.Set_dic_input(dic_i, 0);
            E2.Set_dic_input(dic_i, 4);
        }

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
