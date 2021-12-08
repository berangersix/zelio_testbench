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
    /// Logique d'interaction pour input_zelio.xaml
    /// A button attached to a specific boolean Zelio input
    /// The zelio input is under zelio soft at position index. Exemple for I3, index=3
    /// </summary>
    public partial class Input_zelio : UserControl
    {
        private readonly Sync_with_zelio sync_zelio = Sync_with_zelio.GetInstance();
        public Input_zelio()
        {
            InitializeComponent();
            Loaded += (o, e) => Initialize();
        }

        void Initialize()
        {
            button_input_template_label.Content = "I" + Index;
        }

        public static readonly DependencyProperty IndexContentProperty = DependencyProperty.Register("Index", typeof(int), typeof(Input_zelio));

        public int Index
        {
            get { return (int)GetValue(IndexContentProperty); }
            set { SetValue(IndexContentProperty, value); }
        }


        private void Set_true(object sender, RoutedEventArgs e)
        {
            sync_zelio.Write_TOR_input(Index, true);
        }

        private void Set_false(object sender, RoutedEventArgs e)
        {
            sync_zelio.Write_TOR_input(Index, false);
        }

        public void Set_value(bool value)
        {
            if (value) button_input_template.IsChecked = true;
            else button_input_template.IsChecked = false;
        }

        public bool Get_value()
        {
            return button_input_template.IsChecked.Value;
        }
    }
}
