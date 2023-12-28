using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using APP_ADM_SIEMENS_VALIQC.Data;
using APP_ADM_SIEMENS_VALIQC.Properties;
using APP_ADM_SIEMENS_VALIQC.Utils;
using CustomControls.RJControls;

namespace APP_ADM_SIEMENS_VALIQC.Forms
{
    public partial class Resultados : Form
    {
        DbContext.ResultadoQuery resultadoQuery = new DbContext.ResultadoQuery();
        DbContext.ResultadoStatement resultadoStatement = new DbContext.ResultadoStatement();
        DbQuery dbQuery = new DbQuery();
        RegistroLog log = new RegistroLog();

        public string nombreLog = InterfaceConfig.nombreLog;
        public bool booleanTimer = false;
        public const char CR = '\r';

        public enum EnumEstados { Ok, Info, Process, Warning, Error }

        public Resultados()
        {
            InitializeComponent();
            InterfaceConfig.InitializeConfig();
        }

        private void timerIntervalos_Tick(object sender, EventArgs e)
        {
            int intervalo = InterfaceConfig.intervalo;

            timerIntervalos.Interval = intervalo * 60000;

            if (timerIntervalos.Enabled)
            {
                if (lblIntervalos.ForeColor == Color.Black)
                {
                    lblIntervalos.ForeColor = Color.Red;
                }
                else
                {
                    lblIntervalos.ForeColor = Color.Black;
                }
            }

            //banderas para evitar que se pueda abrir el form Config
            //en mitad del proceso de lectura de archivos planos
            InterfaceConfig.banderaConfig = false;
            LecturaArchivosPlanos();
            InterfaceConfig.banderaConfig = true;
        }

        private void Terminal_Load(object sender, EventArgs e)
        {
            timerIntervalos.Start();
            lblIntervalos.Text = "Intervalo";
            lblIntervalos.ForeColor = Color.Red;
            InterfaceConfig.InitializeConfig();

            MensajesFlowLP($"Interfaz iniciada", EnumEstados.Ok);
        }

        private void LecturaArchivosPlanos()
        {
            try
            {
                log.RegistraEnLog("-----Empieza Ciclo de Busqueda de archivos-----", nombreLog);
                MensajesFlowLP($"Buscando archivos...", EnumEstados.Process);


                string[] archivos = Directory.GetFiles(InterfaceConfig.rutaArchivos, "*.txt");

                int cantidadArchivos = archivos.Length;
                string nombreArchivoERROR = string.Empty; //Se obtiene el nombre del archivo por si genera algun error

                List<string> listLineasError = new List<string>();
                string nombreArchivo = string.Empty; //Se obtinene el nombre incluido el [.txt]
                string nombreArchivo2 = string.Empty; //Se obtiene el nombre sin incluir el [.txt]

                if (cantidadArchivos > 0)
                {
                    foreach (var archivo in archivos)
                    {
                        try
                        {
                            using (StreamReader reader = new StreamReader(archivo, false))
                            {
                                //Se lee la información del .txt
                                string contenido = reader.ReadToEnd();
                                string[] arrNombreArchivo = archivo.Split('\\');

                                //Proceso para obtener el nombre del archivo
                                foreach (var nomArchivo in arrNombreArchivo)
                                {
                                    if (nomArchivo.Contains('.'))
                                    {
                                        nombreArchivo = nomArchivo;
                                    }
                                }
                                nombreArchivo2 = nombreArchivo.Split('.')[0];
                                nombreArchivoERROR = nombreArchivo;

                                //Inicio del procesamiento de la información del archivo plano
                                if (contenido.Length > 0)
                                {
                                    MensajesFlowLP($"Procesando resultados...", EnumEstados.Warning);

                                    try
                                    {
                                        log.RegistraEnLog($"-----Se inicia procesamiento del archivo {nombreArchivo}-----", nombreLog);

                                        //Se ajusta en una lista la información del .dat
                                        string strArchivoResultado = string.Empty;
                                        strArchivoResultado = contenido + strArchivoResultado;
                                        string[] arrDatosArchivo = strArchivoResultado.Replace("\n", "").Split(CR);

                                        string[] estructuraLinea = { "Instrumento", "Lote", "Prueba", "Fecha", "Hora", "Nivel", "Resultado" }; //Arreglo que contiene la estructura de la linea.

                                        //--------------------------Posiciones de cada linea del arrDatosArchivo--------------------------
                                        //Instrumento = [0]
                                        //Lote de control = [1]
                                        //Prueba = [2]
                                        //Fecha = [3]
                                        //Hora = [4]
                                        //Nivel = [5]
                                        //Resultado = [6]

                                        foreach (var lineaDatosArchivo in arrDatosArchivo)
                                        {
                                            log.RegistraEnLog($"{CR}", nombreLog);

                                            string[] arrLineaArchvo = lineaDatosArchivo.Split(',');

                                            //Datos
                                            string instrumento = arrLineaArchvo[0];

                                            //string loteInicial = arrLineaArchvo[1];
                                            string loteInicial = arrLineaArchvo[1]; //Lote inicial, se usara dependiendo de que se responda frente a la situación que genera.

                                            //Se elimina el ultimo caracter del lote inicial, al parecer este incluye el nivel en esta posición
                                            //lo cual afecta en que nunca se actualicen los niveles en un registro, por que se estaria
                                            //creando un registro nuevo ya que el lote cambiaria en esta posición haciendolo un lote distinto.
                                            string loteFinal = loteInicial.Substring(0, loteInicial.Length - 1); 

                                            string prueba = arrLineaArchvo[2];
                                            string fecha = arrLineaArchvo[3];
                                            string hora = arrLineaArchvo[4];
                                            string nivel = arrLineaArchvo[5];
                                            //string nivelOpcion2 = lote.Substring(lote.Length - 1);
                                            string resultado = arrLineaArchvo[6];
                                            //Eliminar caracteres adicionales del resultado
                                            if (resultado.Contains("<") || resultado.Contains(">"))
                                            {
                                                resultado = resultado.Replace("<", "");
                                                resultado = resultado.Replace(">", "");
                                            }

                                            int idRegistro = 0;
                                            string nivelHomologado = string.Empty;

                                            #region Validación de campos vacios
                                            if (string.IsNullOrEmpty(arrLineaArchvo[0]) || //Instrumento = [0]
                                                string.IsNullOrEmpty(arrLineaArchvo[1]) ||     //Lote de control = [1]
                                                string.IsNullOrEmpty(arrLineaArchvo[2]) ||     //Prueba = [2]
                                                string.IsNullOrEmpty(arrLineaArchvo[3]) ||     //Fecha = [3]
                                                string.IsNullOrEmpty(arrLineaArchvo[5]) ||     //Nivel = [5]
                                                string.IsNullOrEmpty(arrLineaArchvo[6]))       //Resultado = [6]
                                            {
                                                log.RegistraEnLog($"Error en linea [{lineaDatosArchivo}], no se procesara ya que contiene algun valor necesario vacio.{CR}", nombreLog);
                                                MensajesFlowLP($"Error procesando Linea[{lineaDatosArchivo}]", EnumEstados.Error);
                                                listLineasError.Add(lineaDatosArchivo);
                                                continue;
                                            }
                                            #endregion

                                            //Consulta si ya existe registro en la tabla de resultados 
                                            resultadoQuery = dbQuery.ConsultaResultados(0, instrumento, loteFinal, prueba, nivel);
                                            if (resultadoQuery.Tabla.Rows.Count == 0)
                                            {
                                                //Se inserta el registro en la base de datos
                                                resultadoStatement = dbQuery.InsertResultados(instrumento, loteFinal, prueba, fecha, nivel, resultado);
                                                if (resultadoStatement.filasAfectadas > 0)
                                                {
                                                    log.RegistraEnLog($"Registro para la prueba[{prueba}], instrumento[{instrumento}], lote[{loteFinal}], creado correctamente en el esquema de resultados para ValiQC.", nombreLog);
                                                }
                                                else
                                                {
                                                    log.RegistraEnLog($"Error insertando linea[{lineaDatosArchivo}] en el esquema de resultados para ValiQC ", nombreLog);
                                                    MensajesFlowLP($"Error procesando Linea[{lineaDatosArchivo}]", EnumEstados.Error);
                                                    listLineasError.Add(lineaDatosArchivo);
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                //Id del registro encontrado en la tabla de resultados
                                                idRegistro = Convert.ToInt32(resultadoQuery.Tabla.Rows[0]["id"]);

                                                //Consulta si el estado continua en N
                                                resultadoQuery = dbQuery.ConsultaEstado(idRegistro);
                                                if (resultadoQuery.Tabla.Rows.Count > 0)
                                                {
                                                    #region Con Homologacion
                                                    //Se consula homologación para el nivel
                                                    log.RegistraEnLog($"-----Busqueda de homologación de nivel-----{CR}", nombreLog);

                                                    resultadoQuery = dbQuery.ConsultaNivel(prueba);
                                                    if (resultadoQuery.Tabla.Rows.Count > 0)
                                                    {
                                                        nivelHomologado = resultadoQuery.Tabla.Rows[0]["examen_cod"].ToString();
                                                    }
                                                    else
                                                    {
                                                        log.RegistraEnLog($"No se encuentra homologación de nivel definida para la prueba[{prueba}], linea[{lineaDatosArchivo}]{CR}", nombreLog);
                                                        MensajesFlowLP($"Error procesando Linea[{lineaDatosArchivo}]", EnumEstados.Error);
                                                        listLineasError.Add(lineaDatosArchivo);
                                                        continue;
                                                    }

                                                    //Se valida que el nivel a actualizar no supere el nivel homologado
                                                    if (Convert.ToInt32(nivel) <= Convert.ToInt32(nivelHomologado))
                                                    {
                                                        resultadoStatement = dbQuery.ActualizaEstado(idRegistro, nivel, resultado);
                                                        if (resultadoStatement.filasAfectadas > 0)
                                                        {
                                                            log.RegistraEnLog($"Actualización de resultado de nivel[{nivel}], para la prueba[{prueba}], instrumento[{instrumento}], lote[{loteFinal}], en el esquema de resultados para ValiQC.", nombreLog);
                                                        }
                                                        else
                                                        {
                                                            log.RegistraEnLog($"Error en actualización de nivel para la prueba[{prueba}], linea[{lineaDatosArchivo}]", nombreLog);
                                                            MensajesFlowLP($"Error procesando Linea[{lineaDatosArchivo}]", EnumEstados.Error);
                                                            listLineasError.Add(lineaDatosArchivo);
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        log.RegistraEnLog($@"El nivel[{nivel}] a actualizar supera el nivel definido en la homologación para la prueba[{prueba}], por lo tanto no se procesara{CR}la linea[{lineaDatosArchivo}], del archivo[{nombreArchivo2}], se adicionara en el archivo con errores, si no se ajusta{CR}la homologación, este proceso se seguira repitiendo.", nombreLog);
                                                        MensajesFlowLP($"Error procesando Linea[{lineaDatosArchivo}]", EnumEstados.Error);
                                                        listLineasError.Add(lineaDatosArchivo);
                                                        continue;
                                                    }
                                                    #endregion

                                                    #region Sin Homologacion
                                                    resultadoStatement = dbQuery.ActualizaEstado(idRegistro, nivel, resultado);
                                                    //if (resultadoStatement.filasAfectadas > 0)
                                                    //{
                                                    //    log.RegistraEnLog($"Se actualiza resultado de nivel[{nivel}], en la tabla de resultados para la prueba[{prueba}], instrumento[{instrumento}], lote[{loteFinal}]", nombreLog);
                                                    //}
                                                    //else
                                                    //{
                                                    //    log.RegistraEnLog($"Error en actualización de nivel para la prueba[{prueba}], linea[{lineaDatosArchivo}]", nombreLog);
                                                    //    listLineasError.Add(lineaDatosArchivo);
                                                    //    continue;
                                                    //}
                                                    #endregion
                                                }
                                                //else
                                                //{
                                                //    //Ya se encuentra el estado en S
                                                //    log.RegistraEnLog($"Ya se encuentra el estado en S para la prueba[{prueba}], instrumento[{instrumento}], lote[{loteFinal}], en la tabla de resultados del esquema para ValiQC.", nombreLog);
                                                //    continue;
                                                //}
                                            }

                                            log.RegistraEnLog($"Linea[{lineaDatosArchivo}], procesada correctamente.{CR}", nombreLog);
                                            MensajesFlowLP($"Resultado procesado correctamente, Linea[{lineaDatosArchivo}]", EnumEstados.Ok);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        log.RegistraEnLog($"Error en procesamiento del archivo[{nombreArchivo}]", nombreLog);
                                        MensajesFlowLP($"Error en la interpretación", EnumEstados.Error);
                                    }
                                }
                            }

                            #region Proceso para mover los archivos
                            //Se mueven los archivos .txt de la ruta de entrada a la ruta de procesados
                            log.RegistraEnLog($"-----Se inicia proceso de cambio de ruta del archivo [{nombreArchivo}] a la ruta de archivosOK definida en el .config-----", nombreLog);

                            string rutaArchivos = InterfaceConfig.rutaArchivos + "\\" + nombreArchivo;
                            string rutaArchivosOK = InterfaceConfig.rutaArchivosOK + "\\" + nombreArchivo;
                            try
                            {
                                File.Move(rutaArchivos, rutaArchivosOK);
                            }
                            catch (Exception e)
                            {
                                log.RegistraEnLog($"Error al mover el archivo[{nombreArchivo}] a la ruta[{rutaArchivosOK}],[{e.Message}]", nombreLog);
                                MensajesFlowLP($"Error en el procesamiento", EnumEstados.Error);
                            }

                            log.RegistraEnLog($"-----Fin de proceso para cambio de ruta del archivo {nombreArchivo}-----{CR}", nombreLog);
                            #endregion

                            #region Proceso para crear los archivos
                            if (listLineasError.Count > 0)
                            {
                                //Crear el archivo con las lineas que no pudieron ser procesadas en la ruta de error
                                log.RegistraEnLog($"-----Se inicia proceso para generar el archivo [{nombreArchivo2}(ERROR).txt] con las lineas que presentaron errores en su procesamiento-----", nombreLog);

                                string rutaArchivosError = $"{InterfaceConfig.rutaArchivosError}\\{nombreArchivo2}(ERROR).txt";
                                try
                                {
                                    using (StreamWriter txt = File.AppendText(rutaArchivosError))//se crea el archivo
                                    {
                                        for (var x = 0; x < listLineasError.Count; x++)
                                        {
                                            txt.WriteLine(listLineasError[x].ToString());
                                            //txt.WriteLine(listLineasError[x].ToString() + CR);
                                        }
                                        txt.Close();
                                    }
                                }
                                catch (Exception)
                                {
                                    log.RegistraEnLog($"Error al crear el archivo[{nombreArchivo}] con las lineas sin procesar, en la ruta[{rutaArchivosError}]", nombreLog);
                                    MensajesFlowLP($"Error en el procesamiento", EnumEstados.Error);
                                }

                                log.RegistraEnLog($"-----Fin de proceso para generar el archivo [{nombreArchivo2}(ERROR).txt]-----{CR}", nombreLog);

                            }
                            #endregion
                        }
                        catch (Exception)
                        {
                            log.RegistraEnLog($"Error en lectura de archivo en la ruta[{InterfaceConfig.rutaArchivos}]", nombreLog);
                            MensajesFlowLP($"Error en el procesamiento", EnumEstados.Error);
                        }
                    }
                }
                else
                {
                    log.RegistraEnLog($"No hay archivos por procesar en la ruta[{InterfaceConfig.rutaArchivos}]", nombreLog);
                    MensajesFlowLP($"No hay archivos por procesar", EnumEstados.Warning);
                }
            }
            catch (Exception)
            {
                log.RegistraEnLog($"Error de consulta de archivos en la ruta[{InterfaceConfig.rutaArchivos}]", nombreLog);
                MensajesFlowLP($"Error en el procesamiento", EnumEstados.Error);
            }
        }

        //Metodo para mostrar los mensajes en el FlowLayoutPanel
        public void MensajesFlowLP(string msg, EnumEstados estado)
        {
            Button nuevoButton = new Button();
            string fechaActual = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            //nuevoButton.Dock = DockStyle.Top;
            nuevoButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            nuevoButton.ImageAlign = ContentAlignment.MiddleLeft;
            nuevoButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            nuevoButton.Width = 225;
            nuevoButton.Height = 10;
            nuevoButton.FlatStyle = FlatStyle.Flat;
            nuevoButton.FlatAppearance.BorderColor = Color.White;
            nuevoButton.FlatAppearance.BorderSize = 0;
            nuevoButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            nuevoButton.FlatAppearance.MouseOverBackColor = Color.Transparent;

            switch (estado)
            {
                case EnumEstados.Ok:
                    nuevoButton.Image = Resources.OkM;
                    nuevoButton.AutoSize = true;
                    nuevoButton.Text = $" {fechaActual}: " + msg;
                    nuevoButton.ForeColor = Color.Black;
                    nuevoButton.TextAlign = ContentAlignment.MiddleLeft; // Alinea el texto a la izquierda
                    break;

                case EnumEstados.Info:
                    nuevoButton.Image = Resources.Null;
                    nuevoButton.AutoSize = true;
                    nuevoButton.Text = msg;
                    nuevoButton.ForeColor = Color.Black;
                    nuevoButton.TextAlign = ContentAlignment.MiddleLeft; // Alinea el texto a la izquierda
                    break;

                case EnumEstados.Process:
                    nuevoButton.Image = Resources.InterpretandoM;
                    nuevoButton.AutoSize = true;
                    nuevoButton.Text = $" {fechaActual}: " + msg;
                    nuevoButton.ForeColor = Color.Black;
                    break;

                case EnumEstados.Warning:
                    nuevoButton.Image = Resources.EsperandoM;
                    nuevoButton.AutoSize = true;
                    nuevoButton.Text = $" {fechaActual}: " + msg;
                    nuevoButton.ForeColor = Color.Black;
                    break;

                case EnumEstados.Error:
                    nuevoButton.Image = Resources.ErrorM;
                    nuevoButton.AutoSize = true;
                    nuevoButton.Text = $" {fechaActual}: " + msg;
                    nuevoButton.ForeColor = Color.Black;
                    break;

                default:
                    break;
            }

            flpContenedorResul.Invoke(new EventHandler(delegate
            {
                flpContenedorResul.Controls.Add(nuevoButton);

                //Hacer scroll hacia abajo para mostrar el contenido más reciente
                flpContenedorResul.AutoScrollPosition = new Point(0, flpContenedorResul.VerticalScroll.Maximum);
            }));
        }

    }
}
