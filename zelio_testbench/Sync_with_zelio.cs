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
    public delegate void del_output_changed(bool value);

    //callback to send information 
    public delegate void log_del(String text);
    /// <summary>
    /// A singleton class to communicate with the Zelio software
    /// </summary>
    class Sync_with_zelio
    {

        private const string ZELIO_WNAME = "Zelio2";
        private const int timer_time = 20;


        //version of zelio soft
        private int major_Version;
        private int minor_Version;
        private int build_Version;


        private Process hWnd_Zelio;
        //position
        private IntPtr address_TOR_input;
        private IntPtr address_TOR_output;
        private IntPtr address_TOR_analog_input;
        private static Timer TT;

        private readonly Dictionary<int, del_output_changed> call_back_dic =new();
        private log_del log;
        private bool istarted=false;


        private void Default_log(String text)
        {
            Debug.WriteLine(text);
        }
        /// <summary>
        /// Singleton so the consturctor is private
        /// </summary>
        private Sync_with_zelio() {
            //initalise table
            analog_input = new Int16[6];
            Set_log_callback(Default_log);
        }

        //log mutext
        private static readonly object _sync_log = new();
        /// <summary>
        /// Export output log to a callback
        /// </summary>
        /// <param name="text">log text</param>
        private void Local_log(String text)
        {
            lock (_sync_log) {
                log.Invoke(text);
            }
        }
        /// <summary>
        /// Set a delegate for log
        /// </summary>
        /// <param name="log_call"></param>
        public void Set_log_callback(log_del log_call)
        {
            lock (_sync_log)
            {
                log = log_call;
            }
        }

        //log mutext
        private static readonly object _sync_callback = new();
        /// <summary>
        /// When output at index change , delegate callback is called
        /// </summary>
        /// <param name="index"></param>
        /// <param name="callback"></param>
        public void Set_callback(int index, del_output_changed callback)
        {
            lock (_sync_callback)
            {
                //set the callback when something changed
                call_back_dic.TryAdd(index, callback);
               
            }
        }
        /// <summary>
        /// Main loop scan process call this method to raide callback attached to every output
        /// </summary>
        /// <param name="output_has_changed"></param>
        private void Local_callback(Dictionary<int, bool> output_has_changed)
        {
            lock (_sync_callback)
            {
                foreach (int index in output_has_changed.Keys)
                {
                    if (call_back_dic.ContainsKey(index))
                    {

                        
                        //set the callback when something changed
                        call_back_dic[index].Invoke(output_has_changed[index]);
                    }
                }

            }
        }
        /// <summary>
        /// start the timer scan
        /// </summary>
        /// <returns>true simu start, else false</returns>
        /// <param name="timer_time"></param>
        public bool Start_process_scan()
        {
            if (istarted)
            {
                Local_log("Process already start");
                return false;

            }
            //get windows handle
            hWnd_Zelio = Managed_wprocess.Find_Process(ZELIO_WNAME);
           
            if (hWnd_Zelio == null)
            {
                Local_log("Process not found");
                return false;
            }

            major_Version = hWnd_Zelio.MainModule.FileVersionInfo.FileMajorPart;
            minor_Version = hWnd_Zelio.MainModule.FileVersionInfo.FileMinorPart;
            build_Version = hWnd_Zelio.MainModule.FileVersionInfo.FileBuildPart;

            if (major_Version == 5 && minor_Version==4 && build_Version==0)
            {
                //set all base adress get value as "Zelio2.exe", 0x375678 thanks to CheatEngine. It seems static
                address_TOR_input = Managed_wprocess.Find_adress(hWnd_Zelio, "Zelio2.exe", 0x375678);
                address_TOR_analog_input = Managed_wprocess.Find_adress(hWnd_Zelio, "Zelio2.exe", 0x375678 + 0x5);
                address_TOR_output = Managed_wprocess.Find_adress(hWnd_Zelio, "Z2Dc4c_Interf.dll", 0x382BC);
       
            }
            else if (major_Version == 5 && minor_Version == 4 && build_Version == 1)
            {
                //set all base adress get value as "Zelio2.exe", 0x375678 thanks to CheatEngine. It seems static
                address_TOR_input = Managed_wprocess.Find_adress(hWnd_Zelio, "Zelio2.exe", 0x394010);
                address_TOR_analog_input = Managed_wprocess.Find_adress(hWnd_Zelio, "Zelio2.exe", 0x394010 + 0x5);
                address_TOR_output = Managed_wprocess.Find_adress(hWnd_Zelio, "Z2Dc4c_Interf.dll", 0x382BC + 0x1080);
            }
            else
            {
                Local_log("Version not found");
                return false;
            }
     
            if (address_TOR_input == IntPtr.Zero || address_TOR_analog_input == IntPtr.Zero || address_TOR_output == IntPtr.Zero)
            {
                Local_log("Symbol not found");
                return false;
            }

            //every 20ms we update
            TT = new System.Timers.Timer(timer_time);
            TT.Elapsed += Update_loop;
            TT.AutoReset = true;
            TT.Enabled = true;
            //set log
            Local_log("Start process scan every " + timer_time + "ms");
            return true;
           
        }

        /// <summary>
        /// stop the timer san
        /// </summary>
        public void Stop_process_scan()
        {   
            
            if (TT != null) {
                TT.Stop();
                TT.Dispose();
                TT = null;
            }
            //set all to initial value, and wait timer callback is finish
            System.Threading.Thread.Sleep(1000);
              lock (_sync_TOR_output)
                {

                if (hWnd_Zelio != null) hWnd_Zelio.Dispose();
                    hWnd_Zelio = null;
                    address_TOR_input = IntPtr.Zero;
                    address_TOR_analog_input = IntPtr.Zero;
                    address_TOR_output = IntPtr.Zero;
                }
            istarted = false;
            Local_log("Stop process");
        }

        /// <summary>
        /// ref to singleton
        /// </summary>
        private static Sync_with_zelio _instance;
        /// <summary>
        /// mutex that will be used to prevent every change multithreaded
        /// </summary>
        private static readonly object _sync_get = new();
        private static readonly object _sync_TOR_input = new();
        private static readonly object _sync_analog_input = new();
        private static readonly object _sync_TOR_output = new();
        private static readonly object _sync_loop = new();


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
        private bool Get_bit_from_int16(Int16 var, int index)
        {
            if (index >= 16 || index < 0) { Local_log("Index Out of bound"); return false; }; // prevent override put error here
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
        private bool Set_bit_from_int16(ref Int16 var, int index,bool value)
        {
            if (index >= 16 || index < 0) { Local_log("Index Out of bound"); return false; }; // prevent override put error here

            if (Get_bit_from_int16(var, index) != value)
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
        private readonly Int16[] analog_input;

        /// <summary>
        /// write specific bool in TOR input
        /// </summary>
        /// <param name="index">position of TOR from 1 to 16</param>
        /// <param name="value">boolean value</param>
        public void Write_TOR_input(int index, bool value)
        {
            index--;//byte is store from 0 to 15, whereas zelio is display from 1 to 16
            if (index >= 16 || index < 0) { Local_log("Index Out of bound"); return; }; // prevent override put error here
            lock (_sync_TOR_input)
            {
                Set_bit_from_int16(ref TOR_input, index, value);
            }
        }

        /// <summary>
        /// read specific bool in TOR input
        /// </summary>
        /// <param name="index">position of TOR from 1 to 16</param>
        /// <returns>boolean value</returns>
        public bool Read_TOR_input(int index)
        {
            bool output;
            index--;//byte is store from 0 to 15, whereas zelio is display from 1 to 16
            if (index >= 16 || index < 0) { Local_log("Index Out of bound"); return false; }; // prevent override put error here
            lock (_sync_TOR_input)
            {
                output = Get_bit_from_int16(TOR_input, index);
            }
            return output;
        }

        /// <summary>
        ///  write specific analog input
        /// </summary>
        /// <param name="index">position of analog input start at IB =1</param>
        /// <param name="value">new value</param>
        public void Write_analog_input(int index, int value)
        {
            index--;//byte is store from 0 to 15, whereas zelio is display from 1 to 16
            if (index > 5 || index < 0) { Local_log("Index Out of bound"); return; }; // prevent override put error here
            if (value > 255 || value < 0) { Local_log("Value Out of bound"); return; }; // prevent override put error here

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
        public int Read_analog_input(int index)
        {
            int output;
            index--;//byte is store from 0 to 15, whereas zelio is display from 1 to 16
            if (index > 5 || index < 0) { Local_log("Index Out of bound"); return 0; }; // prevent override put error here
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
        public bool Read_TOR_output(int index)
        {

            bool output;
            index--;//we could only see output from 1 to 8
            if (index >= 8 || index < 0) { Local_log("Index Out of bound"); return false; }; // prevent override put error here
            lock (_sync_TOR_output)
            {
                output = Get_bit_from_int16(TOR_output, index);
            }
            return output;
        }

        /// <summary>
        /// This loop read and write data in zelio soft, if an output changed it also call delegate associate to it (previously vi setcallback method)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void Update_loop(Object source, ElapsedEventArgs e)
        {
            Dictionary<int, bool> output_changed = new();
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
                            bool new_bool = Get_bit_from_int16(read_output, i);
                            if (new_bool  != Get_bit_from_int16(TOR_output, i))
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
                Local_callback(output_changed);
            
            }
        }

    }
}
