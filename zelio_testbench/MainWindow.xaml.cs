using System;
using System.Collections.Generic;
using System.Diagnostics;
using zelio_testbench.debug_hmi;
using System.Linq;
using System.Text;
using System.Threading;
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
using zelio_testbench.TP_operation_math;
using zelio_testbench.select_TP;

namespace zelio_testbench
{
    /// <summary>
    /// Every TP have its own grid, this grid must be a child of this interface
    /// </summary>
    interface ITP_grid
    {
        /// <summary>
        /// Change the state of play button
        /// </summary>
        /// <param name="value"></param>
        void Set_played_state(bool value);
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Sync_with_zelio sync_zelio = Sync_with_zelio.GetInstance();
        private bool is_simu_started = false;

        // we store in a dic every output, input, and analog input button
        private Dictionary<int, Input_zelio> dic_input;
        private Dictionary<int, Output_zelio> dic_output;
        private Dictionary<int, Analog_in_zelio> dic_analog;

        // a pointer to tp grid
        ITP_grid tp_grid = null;
        public MainWindow()
        {

            InitializeComponent();
            Set_dic();
            sync_zelio.Set_log_callback(Callback_log_info);
            Selection_TP();


        }

        /// <summary>
        /// Chosse a TP via a listbox and load it
        /// </summary>
        private void Selection_TP()
        {
            Window_select_TP w_selec = new();
            w_selec.ShowDialog();


            String choice = w_selec.Get_selected_item();
            if (choice.Contains("Operateur Mathematique")) { 
                Tab_operation gr_op = new();
                gr_op.Set_dic(dic_output, dic_input);
                TP_tab_main_grid.Children.Add(gr_op);
                gr_op.Played_event += Start;
                gr_op.Paused_event += Stop;
                tp_grid = gr_op;
            }
        }

        /// <summary>
        /// Set all dic to dictionnary attributes
        /// </summary>
        private void Set_dic()
        {
            dic_input = new() { { 1, I1 }, { 2, I2 }, { 3, I3 }, { 4, I4 }, { 5, I5 }, { 6, I6 }, { 7, I7 }, { 8, I8 }, { 9, I9 }, { 10, I10 }, 
                 { 11, I11 }, { 12, I12 }, { 13, I13 }, { 14, I14 }, { 15, I15 }
            };
            dic_output = new() { { 1, O1 }, { 2, O2 }, { 3, O3 }, { 4, O4 }, { 5, O5 }, { 6, O6 }, { 7, O7 }, { 8, O8 }, { 9, O9 }, { 10, O10 }, 
                 { 11, O11 }, { 12, O12 }, { 13, O13 }, { 14, O14 }, { 15, O15 }
            };
            dic_analog = new() { { 1, AI1 }, { 2, AI2 }, { 3, AI3 }, { 4, AI4 }, { 5, AI5 } };

        }
        /// <summary>
        /// Display log in a textbox place in debug
        /// </summary>
        /// <param name="text"></param>
        private void Callback_log_info(String text)
        {
            Dispatcher.Invoke(() =>
            {
                debug_info.Text = text;
            });
        }

        /// <summary>
        /// Update play button in TP and debug
        /// </summary>
        private void Update_Playbutton_button()
        {
            debug_button_play.button_play_template.IsChecked = is_simu_started;
            if(tp_grid != null) tp_grid.Set_played_state(is_simu_started);
        }

        /// <summary>
        /// Start process scan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start(object sender, RoutedEventArgs e)
        {
            if (!is_simu_started)
            {
                is_simu_started = sync_zelio.Start_process_scan();
                Update_Playbutton_button();
            }
        }

        /// <summary>
        /// Stop process scan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop(object sender, RoutedEventArgs e)
        {
            if (is_simu_started)
            {
                sync_zelio.Stop_process_scan();
                is_simu_started = false;
                Update_Playbutton_button();
            }
        }
    }
}
