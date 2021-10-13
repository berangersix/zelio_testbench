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
using System.Windows.Shapes;

namespace zelio_testbench.select_TP
{
    /// <summary>
    /// Logique d'interaction pour Window_select_TP.xaml
    /// A windows for selecting a TP
    /// </summary>
    public partial class Window_select_TP : Window
    {
        public Window_select_TP()
        {
            InitializeComponent();
        }

        public string Get_selected_item()
        {
            return List_choix.SelectedItem.ToString();
        }

        private void Select_clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
