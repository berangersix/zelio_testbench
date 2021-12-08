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
    /// Logique d'interaction pour tank.xaml
    /// </summary>
    public partial class Tank : UserControl
    {
        public Tank()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty Niveau_eauContentProperty = DependencyProperty.Register("Niveau_eau", typeof(double), typeof(Tank));

        public double Niveau_eau
        {
            get { return (double)GetValue(Niveau_eauContentProperty); }
            set { SetValue(Niveau_eauContentProperty, value); }
        }


    }
}
