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

namespace zelio_testbench.debug_hmi
{
    /// <summary>
    /// Logique d'interaction pour Play_button.xaml
    /// </summary>
    public partial class Play_button : UserControl
    {

        public delegate void play_event(object sender, RoutedEventArgs e);
        public event play_event Played_event;
        public delegate void pause_event(object sender, RoutedEventArgs e);
        public event pause_event Paused_event;

        public Play_button()
        {
            
            InitializeComponent();
        }

        private void Play_event(object sender, RoutedEventArgs e)
        {
            Played_event?.Invoke(sender, e);
        }

        private void Pause_event(object sender, RoutedEventArgs e)
        {
            Paused_event?.Invoke(sender, e);
        }
    }
}
