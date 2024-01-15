using DM_SIEMENS_VALIQC.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DM_SIEMENS_VALIQC.Forms.Resultados;

namespace DM_SIEMENS_VALIQC.Utils
{
    class RegistroLog
    {
        private static bool logIniciado = false;
        public string logName = "Log_";
        //public string logActivo = InterfaceConfig.activaLog;
        //public string RutaLog = InterfaceConfig.rutaLog;
        private bool catchEjecutado = false;

        public void InicializaLog(string p_equipo)
        {
            string RutaLog = InterfaceConfig.rutaLog;

            try
            {
                logName = RutaLog + "/Log_" + p_equipo + "_v" + Application.ProductVersion + "_" + DateTime.Now.ToString("ddMMyyyy");
                using (StreamWriter w = File.AppendText(logName + ".txt"))
                {
                    w.Write("\r\nLog " + p_equipo + " v" + Application.ProductVersion + " : ");
                    w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                        DateTime.Now.ToLongDateString());
                    w.WriteLine("------------------------------------------------------------------------------------------------");
                }
                if (p_equipo != "Desconocido") logIniciado = true;
            }
            catch (Exception)
            {
                DialogResult result;
                using (var msFomr = new FormMessageBox($"Error en procesamiento de log, verifique que la ruta definida exista.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error))
                    result = msFomr.ShowDialog();
            }
        }

        public void RegistraEnLog(string logMessage, string p_equipo)
        {
            string logActivo = InterfaceConfig.activaLog;
            string RutaLog = InterfaceConfig.rutaLog;

            try
            {
                if (logActivo.Equals("S"))
                {
                    logName = RutaLog + "/Log_" + p_equipo + "_v" + Application.ProductVersion + "_" + DateTime.Now.ToString("ddMMyyyy");
                    if (!logIniciado) InicializaLog(p_equipo);
                    using (StreamWriter w = File.AppendText(logName + ".txt"))
                    {
                        w.WriteLine(DateTime.Now + "  :  {0}", logMessage);
                    }
                }
            }
            catch (Exception)
            {
                if (!catchEjecutado)
                {
                    DialogResult result;
                    using (var msFomr = new FormMessageBox($"Error en procesamiento de log, verifique que la ruta definida exista.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error))
                        result = msFomr.ShowDialog();
                    
                    catchEjecutado = true;
                }                
            }
        }
    }
}
