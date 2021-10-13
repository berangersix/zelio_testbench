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
    /// Logique d'interaction pour Numerical_display.xaml
    /// </summary>
    public partial class Numerical_display : UserControl
    {

        private readonly bool[] value_in_bool = new bool[4];
        public Numerical_display()
        {
            InitializeComponent();
        }
        /// <summary>
        /// update value for all textbox
        /// </summary>
        public void Update_value()
        {
            int value = (value_in_bool[0] ? 1 : 0) + (value_in_bool[1] ? 1 : 0) * 2 + (value_in_bool[2] ? 1 : 0) * 4 + (value_in_bool[3] ? 1 : 0) * 8;//convert bool to int
            txtNum_dec.Text = value.ToString();
            txtNum_hex.Text = value.ToString("X");
            txtNum_bin.Text = Convert.ToString(value, 2);

            string bin = Convert.ToString(value, 2);
            while (bin.Length < 4)
            {
                bin = "0" + bin;
            };
            txtNum_bin.Text = bin;
        }
        
        public void Update_b1(bool value)
        {
            value_in_bool[0] = value;
            Update_value();
        }
        public void Update_b2(bool value)
        {
            value_in_bool[1] = value;
            Update_value();
        }
        public void Update_b3(bool value)
        {
            value_in_bool[2] = value;
            Update_value();
        }
        public void Update_b4(bool value)
        {
            value_in_bool[3] = value;
            Update_value();
        }
        public void Set_dic_output(Dictionary<int, Output_zelio> dic)
        {
            if (dic != null)
            {
                if (dic.ContainsKey(1)) dic[1].Add_callback(Update_b1);
                if (dic.ContainsKey(2)) dic[2].Add_callback(Update_b2);
                if (dic.ContainsKey(3)) dic[3].Add_callback(Update_b3);
                if (dic.ContainsKey(4)) dic[4].Add_callback(Update_b4);
            }

        }
    }
}
