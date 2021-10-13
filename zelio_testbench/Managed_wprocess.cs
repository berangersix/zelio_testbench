using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Jupiter;

namespace zelio_testbench
{
    /// <summary>
    /// Some method to find, and write into zelio mem
    /// </summary>
    class Managed_wprocess
    {

        /// <summary> Find first windows that contain the given title text </summary>
        /// <param name="titleText"> The text that the window title must contain. </param>
        public static Process Find_Process(string titleText)
        {
            foreach (Process pr in Process.GetProcesses())
            {
               
                if (pr.ProcessName.Contains(titleText)) {
                   
                    return pr;
                };
            }
            Debug.WriteLine("process " + titleText + " non trouvé");
            return null;
        }

        /// <summary>
        /// Equivalent to GetModuleBase in C++, it gives the basemodule Adress + an offset
        /// </summary>
        /// <param name="pr"></param>
        /// <param name="modName"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static IntPtr Find_adress(Process pr, String modName, int offset)
        {
            IntPtr addr = IntPtr.Zero;
            
            foreach (ProcessModule m in pr.Modules)
            {
                Debug.WriteLine("Module " + m.ModuleName);
                if (m.ModuleName == modName)
                {
                 
                   addr = m.BaseAddress;
                    break;
                }
            }
            return (addr + offset);
        }

        /// <summary>
        /// Write an int16 value in process pr memory,
        /// </summary>
        /// <param name="pr">process where we write</param>
        /// <param name="adress">adress of ptr</param>
        /// <returns>value read</returns>
        public static Int16 ReadMem(Process pr, IntPtr adress)
        {
            var memoryModule = new MemoryModule(pr.Id);
            return memoryModule.ReadVirtualMemory<Int16>(adress);
        }

        /// <summary>
        /// Write an int16 value in process pr memory,
        /// </summary>
        /// <param name="pr">process where we write</param>
        /// <param name="adress">adress of ptr</param>
        /// <param name="value">the value</param>
        public static void WriteMem(Process pr, IntPtr adress, Int16 value)
        {
            var memoryModule = new MemoryModule(pr.Id);
            memoryModule.WriteVirtualMemory<Int16>(adress, value);
        }
    }
}
