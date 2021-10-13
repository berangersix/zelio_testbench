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
    /// Logique d'interaction pour analog_in_zelio.xaml
    ///  A button attached to a specific boolean Zelio input
    /// The zelio input is under zelio soft at position index. Exemple for IB, index=1
    /// </summary>

    public partial class Analog_in_zelio : UserControl
    {
        private readonly Sync_with_zelio sync_zelio = Sync_with_zelio.GetInstance();
        public Analog_in_zelio()
        {
            InitializeComponent();
            Loaded += (o, e) => Initialize();
        }

        /// <summary>
        /// For analog input index=1 mean IB in zelio soft
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string Index_to_zelioname(int index)
        {
            if (index == 1) return "B";
            else if (index == 2) return "C";
            else if (index == 3) return "D";
            else if (index == 4) return "E";
            else if (index == 5) return "F";
            else if (index == 6) return "G";
            else return "0";
        }
        void Initialize()
        {
            Label_name_AI.Content = "AI" + Index_to_zelioname(Index);
            Label_Voltage.Content = "0.0V";
        }

        public static readonly DependencyProperty IndexContentProperty = DependencyProperty.Register("Index", typeof(int), typeof(Analog_in_zelio));

        public int Index
        {
            get { return (int)GetValue(IndexContentProperty); }
            set { SetValue(IndexContentProperty, value); }
        }

        public void Set_value(double value)
        {
            int numerical_convert = (int) value *(255/10);
            Label_Voltage.Content = value.ToString("0.#") + "V";
            sync_zelio.Write_analog_input(Index, numerical_convert);

        }

    }
}
