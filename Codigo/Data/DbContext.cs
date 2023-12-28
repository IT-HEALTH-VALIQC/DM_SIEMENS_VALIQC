using APP_ADM_SIEMENS_VALIQC.Utils;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP_ADM_SIEMENS_VALIQC.Data
{
    public class DbContext
    {
        RegistroLog log = new RegistroLog();
        string ERROR = "ERROR";

        public class ResultadoQuery
        {
            public DataTable Tabla { get; set; }
            public string Resultado { get; set; }
            public string ResultadoMensaje { get; set; }
        }

        public class ResultadoStatement
        {
            public string Resultado { get; set; }
            public string ResultadoMensaje { get; set; }

            public int filasAfectadas { get; set; }
        }

        public ResultadoQuery RunQuery(string sqlStr, List<NpgsqlParameter> ParametersList, CommandType type)
        {
            ResultadoQuery resultadoQuery = new ResultadoQuery();
            resultadoQuery.Tabla = new DataTable();
            resultadoQuery.Resultado = "";
            resultadoQuery.ResultadoMensaje = "";

            bool respuestaPersistencia = false;
            int intentosConexion = 0;

            while (respuestaPersistencia == false)
            {
                NpgsqlConnection connection = new NpgsqlConnection(InterfaceConfig.StrCadenaConeccion);
                NpgsqlCommand command = new NpgsqlCommand();
                intentosConexion += 1;

                try
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        connection.Open();
                    }
                    else
                    {
                        connection.Open();
                    }

                    command = connection.CreateCommand();
                    command.CommandType = type;
                    command.CommandText = sqlStr;

                    if (ParametersList != null)
                    {
                        foreach (var parameter in ParametersList)
                        {
                            command.Parameters.Add(new NpgsqlParameter(parameter.ParameterName, parameter.Value));
                        }
                    }

                    DataTable dt = new DataTable();
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                    da.Fill(dt);
                    connection.Close();

                    respuestaPersistencia = true;
                    resultadoQuery.Tabla = dt;
                }
                catch (Exception ex)
                {
                    //REGISTRAR LOG
                    log.RegistraEnLog("Error consultando la base de datos --> Mensaje[" + ex.Message + "]", InterfaceConfig.nombreLog);
                    log.RegistraEnLog("Sentencia ejecutada --> [" + sqlStr + "]", InterfaceConfig.nombreLog);
                    respuestaPersistencia = false;
                    resultadoQuery.Resultado = ERROR;
                    resultadoQuery.ResultadoMensaje = ex.Message;
                }
                finally
                {
                    connection.Dispose();
                    connection.Close();
                }

                if (respuestaPersistencia == false && intentosConexion == InterfaceConfig.intentosReconexionDB)
                {
                    //SALE DEL CICLO PARA DEVOLVER EL ERROR
                    respuestaPersistencia = true;
                }

                if (InterfaceConfig.imprimirQueriesDBLog.Equals("S"))
                {
                    log.RegistraEnLog("Sentencia ejecutada --> [" + sqlStr + "]", InterfaceConfig.nombreLog);
                }
            }

            return resultadoQuery;
        }

        public ResultadoStatement RunStatement(string sqlStr, List<NpgsqlParameter> ParametersList, CommandType type)
        {
            ResultadoStatement resultadoStatement = new ResultadoStatement();
            resultadoStatement.Resultado = "";
            resultadoStatement.ResultadoMensaje = "";

            bool respuestaPersistencia = false;
            int intentosConexion = 0;

            while (respuestaPersistencia == false)
            {
                NpgsqlConnection connection = new NpgsqlConnection(InterfaceConfig.StrCadenaConeccion);
                NpgsqlCommand command = new NpgsqlCommand();
                intentosConexion += 1;

                try
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        connection.Open();
                    }
                    else
                    {
                        connection.Open();
                    }

                    command = connection.CreateCommand();
                    command.CommandType = type;
                    command.CommandText = sqlStr;

                    if (ParametersList != null)
                    {
                        foreach (var parameter in ParametersList)
                        {
                            command.Parameters.Add(new NpgsqlParameter(parameter.ParameterName, parameter.Value));
                        }
                    }

                    resultadoStatement.filasAfectadas = command.ExecuteNonQuery();
                    connection.Close();
                    respuestaPersistencia = true;
                    resultadoStatement.Resultado = "OK";
                }
                catch (Exception ex)
                {
                    //REGISTRAR LOG
                    log.RegistraEnLog("Error ejecutando en la base de datos --> Mensaje[" + ex.Message + "]", InterfaceConfig.nombreLog);
                    log.RegistraEnLog("Sentencia ejecutada --> [" + sqlStr + "]", InterfaceConfig.nombreLog);
                    respuestaPersistencia = false;
                    resultadoStatement.Resultado = ERROR;
                    resultadoStatement.ResultadoMensaje = ex.Message;
                }
                finally
                {
                    connection.Dispose();
                    connection.Close();
                }

                if (respuestaPersistencia == false && intentosConexion == InterfaceConfig.intentosReconexionDB)
                {
                    //SALE DEL CICLO PARA DEVOLVER EL ERROR
                    respuestaPersistencia = true;
                }

                if (InterfaceConfig.imprimirQueriesDBLog.Equals("S"))
                {
                    log.RegistraEnLog("Sentencia ejecutada --> [" + sqlStr + "]", InterfaceConfig.nombreLog);
                }
            }
            return resultadoStatement;
        }
    }
}
