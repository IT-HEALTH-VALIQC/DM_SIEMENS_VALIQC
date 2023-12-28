using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace APP_ADM_SIEMENS_VALIQC.Utils
{
    public class InterfaceConfig
    {
        //Configuración Interfaz
        static internal string nombreEquipo;
        static internal int intervalo;
        //Configuración Rutas archivos
        static internal string rutaArchivos;
        static internal string rutaArchivosOK;
        static internal string rutaArchivosError;
        //Configuracion Log
        static internal string activaLog;
        static internal string rutaLog;
        static internal string nombreLog;
        static internal string imprimirQueriesDBLog;
        //Configuracion Conexión a Base de Datos
        static internal int intentosReconexionDB;
        static internal string StrCadenaConeccion;

        //bandera para controlar procesos en el dashboard
        static internal bool banderaTerminal = false;
        static internal bool banderaConfig = false;

        static internal void InitializeConfig()
        {
            //Configuración Interfaz
            nombreEquipo = ConfigurationManager.AppSettings["nombreEquipo"];
            intervalo = Convert.ToInt32(ConfigurationManager.AppSettings["intervalo"]);
            ////Configuración Rutas archivos
            rutaArchivos = ConfigurationManager.AppSettings["rutaArchivos"];
            rutaArchivosOK = ConfigurationManager.AppSettings["rutaArchivosOK"];
            rutaArchivosError = ConfigurationManager.AppSettings["rutaArchivosError"];
            //Configuracion Log
            activaLog = ConfigurationManager.AppSettings["activaLog"];
            rutaLog = ConfigurationManager.AppSettings["rutaLog"];
            nombreLog = ConfigurationManager.AppSettings["nombreLog"];
            imprimirQueriesDBLog = ConfigurationManager.AppSettings["imprimirQueriesDBLog"];
            //Configuracion Conexión a Base de Datos
            intentosReconexionDB = Convert.ToInt32(ConfigurationManager.ConnectionStrings["intentosReconexionDB"].ConnectionString);
            StrCadenaConeccion = ConfigurationManager.ConnectionStrings["StrCadenaConeccion"].ConnectionString;
        }
    }
}
