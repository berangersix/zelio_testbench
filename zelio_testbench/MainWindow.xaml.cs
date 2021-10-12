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

namespace zelio_testbench
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Sync_with_zelio sync_zelio = Sync_with_zelio.GetInstance();
        private bool is_simu_started = false;
        public MainWindow()
        {
            sync_zelio.Set_log_callback(Callback_log_info);
        }

        private void Callback_log_info(String text)
        {
            Dispatcher.Invoke(() =>
            {
                debug_info.Text = text;
            });
        }

        private void Update_toggle_button()
        {
            debug_button_play.button_play_template.IsChecked = is_simu_started;
            TP_button_play.button_play_template.IsChecked = is_simu_started;
        }


        private void Start(object sender, RoutedEventArgs e)
        {
            if (!is_simu_started)
            {
                is_simu_started = sync_zelio.Start_process_scan();
                Update_toggle_button();
            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            if (is_simu_started)
            {
                sync_zelio.Stop_process_scan();
                Update_toggle_button();
            }
        }
    }
}
