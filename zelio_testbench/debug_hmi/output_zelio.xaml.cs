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
using System.Reflection;

namespace zelio_testbench.debug_hmi


{

    /// <summary>
    /// Logique d'interaction pour output_zelio.xaml
    /// A button attached to a specific boolean Zelio output
    /// The zelio input is under zelio soft at position index. Exemple for O3, index=3
    /// This button display zelio value, but you could alse attached a delegate to it via Add_callback
    /// When output value changed in Zelio, it raise the callback
    /// </summary>
    public partial class Output_zelio : UserControl
    {

        public static readonly DependencyProperty IndexContentProperty = DependencyProperty.Register("Index", typeof(int), typeof(Output_zelio));

        public int Index
        {
            get { return (int)GetValue(IndexContentProperty); }
            set { SetValue(IndexContentProperty, value); }
        }


        private readonly Sync_with_zelio sync_zelio = Sync_with_zelio.GetInstance();
        private readonly List<del_output_changed> list_callback = new();
        public Output_zelio()
        {
            InitializeComponent();
            Loaded += (o, e) => Initialize();

        }

        void Initialize()
        {
            sync_zelio.Set_callback(Index, Callback_zelio_out_has_changed);
            button_output_template_label.Content = "O" + Index;
           
        }

        public void Add_callback(del_output_changed cback)
        {
            list_callback.Add(cback);
        }
        public void Callback_zelio_out_has_changed(bool value)
        {
            if (value)
            {
                Dispatcher.Invoke(() =>
                {
                    button_output_template.IsChecked = true;
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    button_output_template.IsChecked = false;
                });
            }

            foreach(del_output_changed callback in list_callback)
            {
                Dispatcher.Invoke(() =>
                {
                    callback(value);
                });

            }
        }
    }
}
