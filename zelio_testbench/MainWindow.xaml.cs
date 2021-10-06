using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public MainWindow()
        {
            InitializeComponent();
            Sync_with_zelio sync_zelio = Sync_with_zelio.GetInstance();
            sync_zelio.start_process_scan(200, Callback_zelio_out_has_changed);



        }

        public void Callback_zelio_out_has_changed(Dictionary<int, bool> output_has_changed)
        {
            foreach (int index in output_has_changed.Keys)
            {
                Debug.WriteLine("Changed b" + index + ": " + output_has_changed[index]);
            }
        }
    }
}
