using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Timers;


namespace zelio_testbench.Chateau_eau
{
    /// <summary>
    /// Logique d'interaction pour Thermometer.xaml
    /// </summary>
    public partial class Thermometer : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private const double temps_speed_per_sec = 2;
        //-30C to 50C -> 80
        private readonly Timer TT_temp;


        private bool isCelsius = true;
        public bool IsCelsius
        {
            get => isCelsius;
            set
            {
                isCelsius = value;
                NotifyPropertyChanged(nameof(MinTemperatureStr));
                NotifyPropertyChanged(nameof(MaxTemperatureStr));
                NotifyPropertyChanged(nameof(TemperatureText));
            }
        }

        private double minTemp = -30.0;
        public double MinTemperature
        {
            get => minTemp;
            set
            {
                minTemp = value;
                temperatureStep = (temperatureTube.ActualHeight - (bulb.ActualHeight / 2)) / (maxTemp - minTemp);
                NotifyPropertyChanged(nameof(TemperatureHeight));
                NotifyPropertyChanged(nameof(MinTemperatureStr));
            }
        }

        public string MinTemperatureStr
        {
            get => $"{(int)minTemp}°" + (isCelsius ? "C" : "F");
        }

        private double maxTemp = 50.0;
        public double MaxTemperature
        {
            get => maxTemp;
            set
            {
                maxTemp = value;
                temperatureStep = (temperatureTube.ActualHeight - (bulb.ActualHeight / 2)) / (maxTemp - minTemp);
                NotifyPropertyChanged(nameof(TemperatureHeight));
                NotifyPropertyChanged(nameof(MaxTemperatureStr));
            }
        }
        public string MaxTemperatureStr
        {
            get => $"{(int)maxTemp}°" + (isCelsius ? "C" : "F");
        }

        private double temperatureStep = 1;
        public double TemperatureHeight
        {
            get => bulb != null ? ((Temperature - minTemp) * temperatureStep) + (bulb.ActualHeight / 2) : ((Temperature - minTemp) * temperatureStep);
        }

        public string TemperatureText
        {
            get => $"{(int)Temperature}°" + (isCelsius ? "C" : "F");
        }

        private double Temperature
        {
            get {
                    return (double)GetValue(TemperatureProperty);
            
            }
            set {
                    SetValue(TemperatureProperty, value);
            }
        }

        public double Desired_Temperature
        {
            get
            {
                return (double)GetValue(Desired_TemperatureProperty);

            }
            set
            {

                SetValue(Desired_TemperatureProperty, value);
                
              
            }
        }

        // Using a DependencyProperty as the backing store for NeedleAngle.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty TemperatureProperty =
            DependencyProperty.Register("Temperature", typeof(double), typeof(Thermometer), new PropertyMetadata(8.0));

        public static readonly DependencyProperty Desired_TemperatureProperty =
            DependencyProperty.Register("Desired_Temperature", typeof(double), typeof(Thermometer), new PropertyMetadata(8.0));

        public Thermometer()
        {
            this.DataContext = this;
            //every 20ms we update
            TT_temp = new System.Timers.Timer(1000);
            TT_temp.Elapsed += Update_loop;
            TT_temp.AutoReset = true;
            InitializeComponent();
         
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            switch (e.Property.Name)
            {
                case "ActualHeight":
                    {
                        temperatureStep = (temperatureTube.ActualHeight - (bulb.ActualHeight / 2)) / (maxTemp - minTemp);
                        NotifyPropertyChanged(nameof(TemperatureHeight));
                    }
                    break;
                case "Temperature":
                    NotifyPropertyChanged(nameof(TemperatureHeight));
                    NotifyPropertyChanged(nameof(TemperatureText));
                    break;
                case "Desired_Temperature":
                    if (Temperature != Desired_Temperature) TT_temp.Enabled = true ;
                    break;
            }
        }

        private void Update_loop(Object source, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (Desired_Temperature > Temperature)
                {

                    Temperature += Math.Min(Desired_Temperature - Temperature, temps_speed_per_sec);

                }
                else if (Desired_Temperature < Temperature)
                {

                    Temperature -= Math.Max(Desired_Temperature - Temperature, temps_speed_per_sec);

                }
                else
                {
                    TT_temp.Enabled = false;
                }
            });
        }
    }
}

