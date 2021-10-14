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
    /// Logique d'interaction pour Tab_chateau.xaml
    /// </summary>
    public partial class Tab_chateau : UserControl, ITP_grid
    {
        public Tab_chateau()
        {
            InitializeComponent();
        }

        /// <summary>
        /// set the playbutton to a specific state
        /// </summary>
        /// <param name="value"></param>
        public void Set_played_state(bool value)
        {
            //TP_button_play.button_play_template.IsChecked = value;
        }
    }
}
