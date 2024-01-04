using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM_SIEMENS_VALIQC.Utils;

namespace DM_SIEMENS_VALIQC.Data
{
    public class DbQuery
    {
        DbContext _context = new DbContext();

        string nombreEquipo = InterfaceConfig.nombreEquipo;

        //string query = $@"";
        //
        //return _context.RunQuery(query, null, CommandType.Text);

        public DbContext.ResultadoQuery ConsultaResultados(int id, string instrumento, string lote, string prueba, string nivel)
        {
            string query = $@"SELECT *
                              FROM valiqc.resultados
                              WHERE analito = '{prueba}'
                                AND numlote = '{lote}'
                                AND analizador = '{instrumento}'
                                AND estado = 'N'
                                AND equipo_cod = '{InterfaceConfig.nombreEquipo}'";

            return _context.RunQuery(query, null, CommandType.Text);
        }

        public DbContext.ResultadoStatement InsertResultados(string instrumento, string lote, string prueba, string fecha, string nivel, string resultado)
        {
            //Producción
            string query = $@"INSERT INTO valiqc.resultados (fecha, analito, resulnivel{nivel}, numlote, analizador, estado, equipo_cod)
                              VALUES('{fecha}','{prueba}','{resultado}','{lote}','{instrumento}','N','{InterfaceConfig.nombreEquipo}')";

            ////Pruebas
            //string query = $@"INSERT INTO valiqc.resultados (fecha, analito, resulnivel{nivel}, numlote, analizador, comentario, estado, equipo_cod)
            //                  VALUES('{fecha}','{prueba}','{resultado}','{lote}','{instrumento}','PRUEBAS','N','{InterfaceConfig.nombreEquipo}')";


            return _context.RunStatement(query, null, CommandType.Text);
        }

        public DbContext.ResultadoQuery ConsultaNivel(string prueba)
        {
            string query = $@"SELECT *
                              FROM homologacion
                              WHERE equipo_cod = '{nombreEquipo}'
                                AND examen_cod = '{prueba}'";

            return _context.RunQuery(query, null, CommandType.Text);
        }

        public DbContext.ResultadoStatement ActualizaEstado(int idRegistro, string nivel, string resultado)
        {
            string query = $@"UPDATE valiqc.resultados
                              SET resulnivel{nivel} = '{resultado}'
                              WHERE id = {idRegistro}
                                AND estado = 'N'";

            return _context.RunStatement(query, null, CommandType.Text);
        }

        public DbContext.ResultadoQuery ConsultaEstado(int idRegistro)
        {
            string query = $@"SELECT *
                           FROM valiqc.resultados
                           WHERE id = {idRegistro}
                             AND estado = 'N'
                             ";

            return _context.RunQuery(query, null, CommandType.Text);
        }
    }
}
