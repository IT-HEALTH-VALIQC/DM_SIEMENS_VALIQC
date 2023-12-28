using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using APP_ADM_SIEMENS_VALIQC.Forms;
using APP_ADM_SIEMENS_VALIQC.Utils;

namespace APP_ADM_SIEMENS_VALIQC
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            InterfaceConfig.InitializeConfig();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Dashboard());
        }
    }
}
