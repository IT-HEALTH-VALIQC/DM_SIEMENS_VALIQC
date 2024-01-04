using DM_SIEMENS_VALIQC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DM_SIEMENS_VALIQC.Forms
{
    public partial class Dashboard : Form
    {
        Resultados terminal = new Resultados();
        Config config = new Config();

        public Dashboard()
        {
            InitializeComponent();

            #region Proceso para cargar la primera ventana
            //Se crea una instancia del formulario secundario
            terminal = new Resultados()
            {
                //Se asignan propiedades al fomulario
                TopLevel = false,
                TopMost = true,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            //Se establece el panel contenedor como padre del formulario secundario
            panelDashContenedor.Controls.Add(terminal);
            //Se muestra el formulario
            terminal.Show();

            btnResultados.BackgroundImage = Properties.Resources.Resul_True;
            btnResultados.BackgroundImageLayout = ImageLayout.Zoom;

            btnConfig.BackgroundImage = Properties.Resources.Config_False;
            btnConfig.BackgroundImageLayout = ImageLayout.Zoom;
            #endregion

            this.Text = $"DM_SIEMENS_VALIQC v{Application.ProductVersion}";
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            //Banderas para cambio de icono en los botones
            InterfaceConfig.banderaConfig = true;
            btnResultados.Enabled = false;
        }

        //Eventos al posicionar el mouse en config
        private void btnConfig_MouseEnter(object sender, EventArgs e)
        {
            if (btnConfig.Enabled)
            {
                btnConfig.BackgroundImage = Properties.Resources.Config_True;
                btnConfig.BackgroundImageLayout = ImageLayout.Zoom;

                btnResultados.BackgroundImage = Properties.Resources.Resul_False;
                btnResultados.BackgroundImageLayout = ImageLayout.Zoom;
            }
        }

        //Eventos al posicionar el mouse en config
        private void btnConfig_MouseLeave(object sender, EventArgs e)
        {
            if (btnConfig.Enabled)
            {
                btnResultados.BackgroundImage = Properties.Resources.Resul_True;
                btnResultados.BackgroundImageLayout = ImageLayout.Zoom;

                btnConfig.BackgroundImage = Properties.Resources.Config_False;
                btnConfig.BackgroundImageLayout = ImageLayout.Zoom;
            }
        }

        //Eventos al posicionar el mouse en resultados
        private void btnResultados_MouseEnter(object sender, EventArgs e)
        {
            if (btnResultados.Enabled)
            {
                btnResultados.BackgroundImage = Properties.Resources.Resul_True;
                btnResultados.BackgroundImageLayout = ImageLayout.Zoom;

                btnConfig.BackgroundImage = Properties.Resources.Config_False;
                btnConfig.BackgroundImageLayout = ImageLayout.Zoom;
            }
        }

        //Eventos al posicionar el mouse en resultados
        private void btnResultados_MouseLeave(object sender, EventArgs e)
        {
            if (btnResultados.Enabled)
            {
                btnConfig.BackgroundImage = Properties.Resources.Config_True;
                btnConfig.BackgroundImageLayout = ImageLayout.Zoom;

                btnResultados.BackgroundImage = Properties.Resources.Resul_False;
                btnResultados.BackgroundImageLayout = ImageLayout.Zoom;
            }
        }

        private void btnResultados_Click(object sender, EventArgs e)
        {
            //Cambio de icono boton
            InterfaceConfig.banderaConfig = true;
            btnConfig.Enabled = true;

            if (InterfaceConfig.banderaTerminal)
            {
                btnResultados.BackgroundImage = Properties.Resources.Resul_True;
                btnResultados.BackgroundImageLayout = ImageLayout.Zoom;

                btnConfig.BackgroundImage = Properties.Resources.Config_False;
                btnConfig.BackgroundImageLayout = ImageLayout.Zoom;
            }

            ///Cargue del form Terminal
            //Se liberan resursos de algun formulario abierto anteriormente
            config.Dispose();
            //Se cierra cualquier control existente en el panel contenedor
            panelDashContenedor.Controls.Clear();

            //Se crea una instancia del formulario secundario
            terminal = new Resultados()
            {
                //Se asignan propiedades al fomulario
                TopLevel = false,
                TopMost = true,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            //Se establece el panel contenedor como padre del formulario secundario
            panelDashContenedor.Controls.Add(terminal);
            //Se muestra el formulario
            terminal.Show();

            btnResultados.Enabled = false;
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            //Cambio de icono boton
            InterfaceConfig.banderaTerminal = true;
            btnResultados.Enabled = true;

            if (InterfaceConfig.banderaConfig)
            {
                btnConfig.BackgroundImage = Properties.Resources.Config_True;
                btnConfig.BackgroundImageLayout = ImageLayout.Zoom;

                btnResultados.BackgroundImage = Properties.Resources.Resul_False;
                btnResultados.BackgroundImageLayout = ImageLayout.Zoom;
            }

            ///Cargue del form Config
            //Se liberan resursos de algun formulario abierto anteriormente
            terminal.Dispose();
            //Se cierra cualquier control existente en el panel contenedor
            panelDashContenedor.Controls.Clear();

            //Se crea una instancia del formulario secundario
            config = new Config()
            {
                //Se asignan propiedades al fomulario
                TopLevel = false,
                TopMost = true,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            //Se establece el panel contenedor como padre del formulario secundario
            panelDashContenedor.Controls.Add(config);
            //Se muestra el formulario
            config.Show();

            btnConfig.Enabled = false;
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result;
            using (var msFomr = new FormMessageBox("¿Desea cerrar la interfaz?", "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                result = msFomr.ShowDialog();
            }

            if (result.Equals(DialogResult.Yes))
            {
                Dispose();
                Environment.Exit(1);
            }
            else e.Cancel = true;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
