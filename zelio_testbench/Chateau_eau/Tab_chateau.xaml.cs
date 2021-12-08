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
using System.Timers;


namespace zelio_testbench.Chateau_eau
{
    /// <summary>
    /// Logique d'interaction pour Tab_chateau.xaml
    /// </summary>
    public partial class Tab_chateau : UserControl, ITP_grid
    {


        ///all environment variable
        bool alim_ok = false;
        bool enough_water=false;
        double  water_level =0;
        
        const int enough_water_level = 5;
        const int water_level_bas = 20;
        const int water_level_haut = 90;
        const int water_level_50E3 = 77;
        const int fill_water_speed = 2;//in percent per second
        const int unfill_water_speed = 1;//in percent per second

        bool I1, I2, I3, IC, ID;
        // we store in a dic every output, input, and analog input button
        private Dictionary<int, Input_zelio> dic_input;
        private Dictionary<int, Output_zelio> dic_output;
        private Dictionary<int, Analog_in_zelio> dic_analog;
        //100ms timer for infinite loop
        private static Timer TT;
        private const double timer_time = 50;
      

        public Tab_chateau()
        {
            InitializeComponent();


            //every 100ms we update process
            TT = new System.Timers.Timer(timer_time);
            TT.Elapsed += Update_loop;
            TT.AutoReset = true;
            TT.Enabled = true;
        }



        /// <summary>
        /// set reference to Zelio button to child
        /// </summary>
        /// <param name="dic_o">zelio output</param>
        /// <param name="dic_i">zelio input</param>
        /// <param name="dic_analog">zelio analog</param>
        public void Set_dic(Dictionary<int, Output_zelio> dic_o, Dictionary<int, Input_zelio> dic_i, Dictionary<int, Analog_in_zelio> dic_ai)
        {
            dic_input = dic_i;
            dic_output = dic_o;
            dic_analog = dic_ai;


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


        /// <summary>
        /// Update every 100ms
        /// </summary>
        private void Update_loop(Object source, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                alim_ok = (!Emerg_button.Emerg_button.IsChecked.Value) && On_off.Get_state();

                //now we set flotteur
                I1 = alim_ok && Flotteur_bas.Get_state()    && !Err_flot_bas.Get_state();
                I2 = alim_ok && Flotteur_haut.Get_state()   && !Err_flot_haut.Get_state();
                I3 = alim_ok && Floteur_50E3.Get_state()    && !Err_flot_50E3.Get_state();
                if (I1 != dic_input[1].Get_value()) dic_input[1].Set_value(I1);
                if (I2 != dic_input[2].Get_value()) dic_input[2].Set_value(I2);
                if (I2 != dic_input[3].Get_value()) dic_input[3].Set_value(I3);

                //Read motor value
                IC = Motor1.Get_state() && alim_ok;
                ID = Motor2.Get_state() && alim_ok;
                if (IC != dic_input[6].Get_value()) dic_input[6].Set_value(IC);
                if (ID != dic_input[7].Get_value()) dic_input[7].Set_value(ID);

                //temperature sensor

                //set motor state
                Motor1.Set_state(dic_output[1].Get_state() && alim_ok && !Err_Mot1.Get_state());
                Motor2.Set_state(dic_output[2].Get_state() && alim_ok && !Err_Mot2.Get_state());

                if (Motor1.Get_state() || Motor2.Get_state()) water_level = Math.Min(water_level + fill_water_speed * (timer_time / 1000), 100);

                //voyant
                Green_Ind.Set_state(dic_output[3].Get_state() && alim_ok);
                Red_Ind.Set_state(dic_output[4].Get_state() && alim_ok);


                //set waterlevel
                enough_water = water_level > enough_water_level;
                if (enough_water) filling_pipe.Fill = Brushes.LightBlue;
                else filling_pipe.Fill = Brushes.White ;

                //faucet_water white
                if (enough_water && Robinet.Get_state()) {
                    faucet_water.Fill = Brushes.LightBlue;
                    water_level = Math.Max(water_level - unfill_water_speed * (timer_time / 1000), 0);
                }
                else faucet_water.Fill = Brushes.White;

                tank_to_fill.Niveau_eau = water_level;
                //set water indicator
                Flotteur_bas.Set_state(water_level > water_level_bas);
                Floteur_50E3.Set_state(water_level > water_level_50E3);
                Flotteur_haut.Set_state(water_level > water_level_haut);

            });

        }
    }
}
