using APP_ADM_SIEMENS_VALIQC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace APP_ADM_SIEMENS_VALIQC.Forms
{
    public partial class Config : Form
    {
        public string conexion = InterfaceConfig.StrCadenaConeccion;
        private string pathConfig;
        private string condicional;

        public Config()
        {
            InitializeComponent();

            #region Cargar datos
            //Conexión
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.config");
            pathConfig = files[0];
            var datosConexion = conexion.Split(';');

            txtDireccionIP.Text = datosConexion[0].Split('=')[1].ToString();
            txtPuerto.Text = datosConexion[1].Split('=')[1].ToString();
            txtNombreDB.Text = datosConexion[2].Split('=')[1].ToString();
            txtUsuario.Text = datosConexion[3].Split('=')[1].ToString();
            txtPass.Text = datosConexion[4].Split('=')[1].ToString();
            txtIntentosReCo.Text = InterfaceConfig.intentosReconexionDB.ToString();

            //Parametrización
            txtNombreEquipo.Text = InterfaceConfig.nombreEquipo.ToString();
            txtIntervalo.Text = InterfaceConfig.intervalo.ToString();
            txtNombreLog.Text = InterfaceConfig.nombreLog.ToString();

            List<String> valores1 = new List<String> { "SI", "NO" };
            List<String> valores2 = new List<String> { "SI", "NO" };
            //cbActivarLog.DataSource = valores1;
            //cbImpQuerys.DataSource = valores2;
            cbActivarLog.Checked = InterfaceConfig.activaLog == "S" ? true : false;
            cbImpQuerys.Checked = InterfaceConfig.imprimirQueriesDBLog == "S" ? true : false;

            //Rutas
            txtRutaArchivos.Text = InterfaceConfig.rutaArchivos.ToString();
            txtRutaArchivosOK.Text = InterfaceConfig.rutaArchivosOK.ToString();
            txtRutaArchivosERROR.Text = InterfaceConfig.rutaArchivosError.ToString();
            txtRutaLog.Text = InterfaceConfig.rutaLog.ToString();
            #endregion

            #region Proceso para cargar la primera ventana
            //Color
            btnConexion.ForeColor = Color.FromArgb(64, 81, 252);
            panelConexion.BackColor = Color.FromArgb(64, 81, 252);

            btnParametrizacion.ForeColor = Color.Gray;
            panelParametrizacion.BackColor = Color.Gray;

            btnRuta.ForeColor = Color.Gray;
            panelRuta.BackColor = Color.Gray;

            //Comportamiento
            panelConexion2.Visible = true;
            panelConexion2.Dock = DockStyle.Fill;

            panelParametrizacion2.Visible = false;
            panelRuta2.Visible = false;
            #endregion
        }

        private void Config_Load(object sender, EventArgs e)
        {

        }

        private void btnConexion_Click(object sender, EventArgs e)
        {
            //Color
            btnConexion.ForeColor = Color.FromArgb(64, 81, 252);
            panelConexion.BackColor = Color.FromArgb(64, 81, 252);

            btnParametrizacion.ForeColor = Color.Gray;
            panelParametrizacion.BackColor = Color.Gray;

            btnRuta.ForeColor = Color.Gray;
            panelRuta.BackColor = Color.Gray;

            //Comportamiento
            panelConexion2.Visible = true;
            panelConexion2.Dock = DockStyle.Fill;

            panelParametrizacion2.Visible = false;
            panelRuta2.Visible = false;
        }

        private void btnParametrizacion_Click(object sender, EventArgs e)
        {
            //Color
            btnParametrizacion.ForeColor = Color.FromArgb(64, 81, 252);
            panelParametrizacion.BackColor = Color.FromArgb(64, 81, 252);

            btnConexion.ForeColor = Color.Gray;
            panelConexion.BackColor = Color.Gray;

            btnRuta.ForeColor = Color.Gray;
            panelRuta.BackColor = Color.Gray;


            //Comportamiento
            panelParametrizacion2.Visible = true;
            panelParametrizacion2.Dock = DockStyle.Fill;

            panelConexion2.Visible = false;
            panelRuta2.Visible = false;
        }

        private void btnRuta_Click(object sender, EventArgs e)
        {
            //Color
            btnRuta.ForeColor = Color.FromArgb(64, 81, 252);
            panelRuta.BackColor = Color.FromArgb(64, 81, 252);

            btnConexion.ForeColor = Color.Gray;
            panelConexion.BackColor = Color.Gray;

            btnParametrizacion.ForeColor = Color.Gray;
            panelParametrizacion.BackColor = Color.Gray;

            //Comportamiento
            panelRuta2.Visible = true;
            panelRuta2.Dock = DockStyle.Fill;

            panelConexion2.Visible = false;
            panelParametrizacion2.Visible = false;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Proceso conexión
            if (panelConexion2.Visible)
            {
                #region Conexión
                if (!string.IsNullOrEmpty(txtDireccionIP.Text) &&
                !string.IsNullOrEmpty(txtPuerto.Text) &&
                !string.IsNullOrEmpty(txtNombreDB.Text) &&
                !string.IsNullOrEmpty(txtUsuario.Text) &&
                !string.IsNullOrEmpty(txtPass.Text) &&
                !string.IsNullOrEmpty(txtIntentosReCo.Text)
                )
                {
                    try
                    {
                        var cadenaNueva = $@"Server={txtDireccionIP.Text}; Port={txtPuerto.Text}; Database={txtNombreDB.Text}; User Id={txtUsuario.Text}; Password={txtPass.Text};";
                        UpdateConfigKey("StrCadenaConeccion", cadenaNueva, 1);
                        UpdateConfigKey("intentosReconexionDB", txtIntentosReCo.Text, 1);

                        DialogResult result;
                        using (var msFomr = new FormMessageBox("Conexion guardada correctamente. ", "OK", MessageBoxButtons.OK, MessageBoxIcon.None))
                            result = msFomr.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        DialogResult result;
                        using (var msFomr = new FormMessageBox($"No se puedo guardar correctamente. {ex}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error))
                            result = msFomr.ShowDialog();
                    }
                }
                else
                {
                    DialogResult result;
                    using (var msFomr = new FormMessageBox($"No puede enviar campos vacios. ", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information))
                        result = msFomr.ShowDialog();
                }
                #endregion
            }

            //Proceso parametrización
            if (panelParametrizacion2.Visible)
            {
                #region Parametrización
                if (!string.IsNullOrEmpty(txtNombreEquipo.Text) &&
                !string.IsNullOrEmpty(txtIntervalo.Text) &&
                !string.IsNullOrEmpty(txtNombreLog.Text)
                )
                {
                    try
                    {
                        UpdateConfigKey("nombreEquipo", txtNombreEquipo.Text, 2);
                        UpdateConfigKey("intervalo", txtIntervalo.Text, 2);
                        UpdateConfigKey("nombreLog", txtNombreLog.Text, 2);
                        UpdateConfigKey("activaLog", cbActivarLog.Checked == true ? "S" : "N", 2);
                        UpdateConfigKey("imprimirQueriesDBLog", cbImpQuerys.Checked == true ? "S" : "N", 2);

                        DialogResult result;
                        using (var msFomr = new FormMessageBox("Parametros guardados correctamente. ", "OK", MessageBoxButtons.OK, MessageBoxIcon.None))
                            result = msFomr.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        DialogResult result;
                        using (var msFomr = new FormMessageBox($"No se puedo guardar correctamente. {ex}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error))
                            result = msFomr.ShowDialog();
                    }
                }
                else
                {
                    DialogResult result;
                    using (var msFomr = new FormMessageBox($"No puede enviar campos vacios. ", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information))
                        result = msFomr.ShowDialog();
                }
                #endregion
            }

            //Proceso ruta
            if (panelRuta2.Visible)
            {
                #region Rutas
                if (!string.IsNullOrEmpty(txtRutaArchivos.Text) &&
                !string.IsNullOrEmpty(txtRutaArchivosOK.Text) &&
                !string.IsNullOrEmpty(txtRutaArchivosERROR.Text) &&
                !string.IsNullOrEmpty(txtRutaLog.Text)
                )
                {
                    try
                    {
                        UpdateConfigKey("rutaArchivos", txtRutaArchivos.Text, 2);
                        UpdateConfigKey("rutaArchivosOK", txtRutaArchivosOK.Text, 2);
                        UpdateConfigKey("rutaArchivosError", txtRutaArchivosERROR.Text, 2);
                        UpdateConfigKey("rutaLog", txtRutaLog.Text, 2);

                        DialogResult result;
                        using (var msFomr = new FormMessageBox("Parametros guardados correctamente. ", "OK", MessageBoxButtons.OK, MessageBoxIcon.None))
                            result = msFomr.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        DialogResult result;
                        using (var msFomr = new FormMessageBox($"No se puedo guardar correctamente. {ex}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error))
                            result = msFomr.ShowDialog();
                    }
                }
                else
                {
                    DialogResult result;
                    using (var msFomr = new FormMessageBox($"No puede enviar campos vacios. ", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information))
                        result = msFomr.ShowDialog();
                }
                #endregion
            }
        }

        public void UpdateConfigKey(string strKey, string newValue, int seccion) //Actuliza el valor de app.config por llave
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathConfig);

            if (!ConfigKeyExists(strKey, seccion))
            {
                throw new ArgumentNullException("Key", "<" + strKey + "> not find in the configuration.");
            }

            if (seccion == 1)
            {
                XmlNode connectionStrings = xmlDoc.SelectSingleNode("configuration/connectionStrings");

                foreach (XmlNode childNode in connectionStrings)
                {
                    condicional = childNode.NodeType.ToString();

                    if (condicional == "Comment")
                    {
                        continue;
                    }
                    else if (childNode.Attributes["name"].Value == strKey)
                    {
                        childNode.Attributes["connectionString"].Value = newValue;
                    }
                }
            }
            else
            {
                XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");

                foreach (XmlNode childNode in appSettingsNode)
                {
                    condicional = childNode.NodeType.ToString();

                    if (condicional == "Comment")
                    {
                        continue;
                    }
                    else if (childNode.Attributes["key"].Value == strKey)
                    {
                        childNode.Attributes["value"].Value = newValue;
                    }
                }
            }

            xmlDoc.Save(pathConfig);
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            if (seccion == 1)
            {
                ConfigurationManager.RefreshSection("connectionStrings");
            }
            else
            {
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        public bool ConfigKeyExists(string strKey, int seccion) //Verifica que exista la llave
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathConfig);

            if (seccion == 1)
            {
                XmlNode connectionStringsNode = xmlDoc.SelectSingleNode("configuration/connectionStrings");

                foreach (XmlNode childNode in connectionStringsNode)
                {
                    condicional = childNode.NodeType.ToString();

                    if (condicional == "Comment")
                    {
                        continue;
                    }
                    else if (childNode.Attributes["name"].Value == strKey)
                    {
                        return true;
                    }
                }
            }
            else
            {
                XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");

                foreach (XmlNode childNode in appSettingsNode)
                {
                    condicional = childNode.NodeType.ToString();

                    if (condicional == "Comment")
                    {
                        continue;
                    }
                    else if (childNode.Attributes["key"].Value == strKey)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
