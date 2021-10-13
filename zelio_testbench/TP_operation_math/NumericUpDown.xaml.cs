using System;
using System.Collections;
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

namespace zelio_testbench.TP_operation
{
    /// <summary>
    /// Logique d'interaction pour NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
                   "Value", typeof(int), typeof(NumericUpDown), new PropertyMetadata(default(int)));

        private int offset =0;
        private Dictionary<int, Input_zelio> dic_input = null;

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            /// <summary>
            /// update value for all textbox
            /// </summary>
            set
            {
                SetValue(ValueProperty, value);
                txtNum_dec.Text = value.ToString();
                txtNum_hex.Text = value.ToString("X"); 
                string bin = Convert.ToString(value, 2);
                while (bin.Length < 4)
                {
                    bin = "0" + bin;
                };

                txtNum_bin.Text = bin;
                Update_value_zelio(bin);




            }
        }
        /// <summary>
        /// Update valut to Zelio Soft
        /// </summary>
        /// <param name="bin"></param>
        private void Update_value_zelio(string bin)
        {
            if (dic_input != null) { 
                for (int i = 0; i < 4; i++)
                {
                    if (dic_input.ContainsKey(i+1+ offset)) { 
                        if (bin[3-i] == '1') dic_input[i+1+offset].Set_value(true);
                        else dic_input[i+1+ offset].Set_value(false);
                    }
                }
            }
        }

        public NumericUpDown()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Set reference to zelio elem
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="offset">From which byte we start</param>
        public void Set_dic_input(Dictionary<int, Input_zelio> dic, int offset)
        {
            dic_input = dic;
            this.offset = offset;
        }

        private void CmdUp_Click(object sender, RoutedEventArgs e)
        {
            if ( Value<15 ) Value++;
        }

        private void CmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (Value > 0) Value--;
        }

       
    }
}
