using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;



namespace zelio_testbench
{      
    //callback when something change
    public delegate void del_output_changed(Dictionary<int, bool> output_has_changed);

    /// <summary>
    /// A singleton class to communicate with the Zelio software
    /// </summary>
    class Sync_with_zelio
    {

        private const string ZELIO_WNAME = "Zelio2";
      
        private Process hWnd_Zelio;
        //position
        private IntPtr address_TOR_input;
        private IntPtr address_TOR_output;
        private IntPtr address_TOR_analog_input;
        private static Timer TT;
 
        private del_output_changed callback_del;
        /// <summary>
        /// Singleton so the consturctor is private
        /// </summary>
        private Sync_with_zelio() {
            //initalise table
            analog_input = new Int16[6];

        }

        /// <summary>
        /// start the timer scan
        /// </summary>
        /// <param name="timer_time"></param>
        public void start_process_scan(int timer_time, del_output_changed callback)
        {
            //set the callback when something changed
            callback_del = callback;
            //get windows handle
            hWnd_Zelio = Managed_wprocess.Find_Process(ZELIO_WNAME);
            //set all base adress get value as "Zelio2.exe", 0x375678 thanks to CheatEngine. It seems static
            address_TOR_input = Managed_wprocess.Find_adress(hWnd_Zelio, "Zelio2.exe", 0x375678);
            address_TOR_analog_input = Managed_wprocess.Find_adress(hWnd_Zelio, "Zelio2.exe", 0x375678 + 0x5);
            address_TOR_output = Managed_wprocess.Find_adress(hWnd_Zelio, "Z2Dc4c_Interf.dll", 0x382BC);

            //every 20ms we update
            TT = new System.Timers.Timer(timer_time);
            TT.Elapsed += update_loop;
            TT.AutoReset = true;
            TT.Enabled = true;

        }

        /// <summary>
        /// stop the timer san
        /// </summary>
        public void stop_process_scan()
        {
            TT.Stop();
            TT.Dispose();
            TT = null;
            //set all to initial value, and wait timer callback is finish
            System.Threading.Thread.Sleep(1000);
              lock (_sync_TOR_output)
                {
                    hWnd_Zelio.Dispose();
                    hWnd_Zelio = null;
                    address_TOR_input = IntPtr.Zero;
                    address_TOR_analog_input = IntPtr.Zero;
                    address_TOR_output = IntPtr.Zero;
                    callback_del = null;
                }

        }

        /// <summary>
        /// ref to singleton
        /// </summary>
        private static Sync_with_zelio _instance;
        /// <summary>
        /// mutex that will be used to prevent every change multithreaded
        /// </summary>
        private static readonly object _sync_get = new object();
        private static readonly object _sync_TOR_input = new object();
        private static readonly object _sync_analog_input = new object();
        private static readonly object _sync_TOR_output = new object();
        private static readonly object _sync_loop = new object();


        public static Sync_with_zelio GetInstance()
        {
            // This conditional is needed to prevent threads stumbling over the
            // lock once the instance is ready.
            if (_instance == null)
            {

                // lock and will proceed further, while the rest will wait here.
                lock (_sync_get)
                {
                    // The first thread to acquire the lock, reaches this
                    // conditional, goes inside and creates the Singleton
                    // instance.
                    if (_instance == null)
                    {
                        _instance = new Sync_with_zelio();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// get sepecific bit in byte , retur a boolean
        /// </summary>
        /// <param name="var">int16 where we search for val</param>
        /// <param name="index">pos of bit</param>
        /// <returns></returns>
        private bool get_bit_from_int16(Int16 var, int index)
        {
            if (index >= 16 || index<0 ) return false; // prevent override put error here
            int bitNumber = index % 8;
            byte b = BitConverter.GetBytes(var)[index/8];
            bool ret = ((b >> bitNumber) & 1) != 0;
            return ret;
        }

        /// <summary>
        /// set sepecific bit in byte , retur a boolean
        /// </summary>
        /// <param name="var">int16 where we set new bit</param>
        /// <param name="index">pos of bit</param>
        /// <param name="value">new value</param>
        /// <returns>true if values has changed</returns>
        private bool set_bit_from_int16(ref Int16 var, int index,bool value)
        {
            if (index >= 16 || index < 0) return false; // prevent override put error here

            if (get_bit_from_int16(var, index) != value)
            {
                int bitNumber = index % 8;
                int pos = index / 8;
                byte[] b = BitConverter.GetBytes(var);

                if (value)
                {
                    //left-shift 1, then bitwise OR
                    b[pos] = (byte)(b[pos] | (1 << bitNumber));
                }
                else
                {
                    //left-shift 1, then take complement, then bitwise AND
                    b[pos] = (byte)(b[pos] & ~(1 << bitNumber));
                }

                var = BitConverter.ToInt16(b, 0);
                return true;
            }
            else
            {
                //nothing change
                return false;
            }


       
        }


        private Int16 TOR_input; //input list (each bit is a value)
        private Int16 TOR_output;//input list (each bit is a value)
        private Int16[] analog_input;

        /// <summary>
        /// write specific bool in TOR input
        /// </summary>
        /// <param name="index">position of TOR from 1 to 16</param>
        /// <param name="value">boolean value</param>
        public void write_TOR_input(int index, bool value)
        {
            index--;//byte is store from 0 to 15, whereas zelio is display from 1 to 16
            if (index >= 16 || index < 0) return; // prevent override put error here
            lock (_sync_TOR_input)
            {
                set_bit_from_int16(ref TOR_input, index, value);
            }
        }

        /// <summary>
        /// read specific bool in TOR input
        /// </summary>
        /// <param name="index">position of TOR from 1 to 16</param>
        /// <returns>boolean value</returns>
        public bool read_TOR_input(int index)
        {
            bool output;
            index--;//byte is store from 0 to 15, whereas zelio is display from 1 to 16
            if (index >= 16 || index < 0) return false; // prevent override put error here
            lock (_sync_TOR_input)
            {
                output = get_bit_from_int16(TOR_input, index);
            }
            return output;
        }

        /// <summary>
        ///  write specific analog input
        /// </summary>
        /// <param name="index">position of analog input start at IB =1</param>
        /// <param name="value">new value</param>
        public void write_analog_input(int index, int value)
        {
            index--;//byte is store from 0 to 15, whereas zelio is display from 1 to 16
            if (index > 5 || index < 0 ) return; // prevent override put error here
            if (value > 255 || value < 0) return; // it is maximum in Zelio soft

            lock (_sync_analog_input)
            {
                if (analog_input[index] != value)
                {
                    analog_input[index] = Convert.ToInt16(value);
                }

            }
        }

        /// <summary>
        /// read specific analog input
        /// </summary>
        /// <param name="index">position of analog input start at IB =1</param>
        /// <returns>value</returns>
        public int read_analog_input(int index)
        {
            int output;
            index--;//byte is store from 0 to 15, whereas zelio is display from 1 to 16
            if (index > 5 || index < 0) return 0; // prevent override put error here
            lock (_sync_analog_input)
            {
                    output = Convert.ToInt32 (analog_input[index]);
            }
            return output;
        }


        /// <summary>
        ///  read specific bool in TOR output
        /// </summary>
        /// <param name="index">position of TOR from 1 to 16</param>
        /// <returns>the value</returns>
        public bool read_TOR_output(int index)
        {

            bool output;
            index--;//we could only see output from 1 to 8
            if (index >= 8 || index < 0) return false;
            lock (_sync_TOR_output)
            {
                output = get_bit_from_int16(TOR_output, index);
            }
            return output;
        }

        private void update_loop(Object source, ElapsedEventArgs e)
        {
            Dictionary<int, bool> output_changed = new Dictionary<int, bool>();
            lock (_sync_loop)
            {
                lock (_sync_TOR_input)
                {
                   Int16 read_input =  Managed_wprocess.ReadMem(hWnd_Zelio, address_TOR_input);
                    if (read_input != TOR_input) Managed_wprocess.WriteMem(hWnd_Zelio, address_TOR_input, TOR_input);
                   
                }
                lock (_sync_analog_input)
                {
                    for (int i=0; i<=5; i++)
                    {
                        IntPtr adress_analog = address_TOR_analog_input + 2*i;
                        Int16 read_input = Managed_wprocess.ReadMem(hWnd_Zelio, adress_analog);
                        if (read_input != analog_input[i]) Managed_wprocess.WriteMem(hWnd_Zelio, adress_analog, analog_input[i]);
                    }
                   
                }

                lock (_sync_TOR_output)
                {
                   Int16 read_output = Managed_wprocess.ReadMem(hWnd_Zelio, address_TOR_output);
                    if (TOR_output != read_output)
                    {
                  
                        //add diff to  a dic for event
                        for (int i=0; i<=7; i++)
                        {
                            bool new_bool = get_bit_from_int16(read_output, i);
                            if (new_bool  != get_bit_from_int16(TOR_output, i))
                            {
                                output_changed.Add(i + 1, new_bool);
                            }
                        }
                        TOR_output = read_output;
                    }
                  
                }

            }
            if (output_changed.Count > 0)
            {
                //raise delegate
                callback_del.Invoke(output_changed);
            }
        }

    }
}
