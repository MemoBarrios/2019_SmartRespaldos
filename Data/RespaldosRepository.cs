using _2019_Respaldos.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _2019_Respaldos.Data
{
    public class RespaldosRepository
    {
        private string conexionDB;
        private List<Sucursal> Sucursales = new List<Sucursal>();

        public RespaldosRepository(IConfiguration configuration)
        {
            conexionDB = configuration.GetConnectionString("TIENDA");
        }

        public async Task<List<Respaldo>> GetAllRespaldos(string ip)
        {
            conexionDB = "Data Source=" + ip + ";Initial Catalog=TIENDA;User ID=analisis;Password=analisis20120203;";
            using (SqlConnection conn = new SqlConnection(conexionDB))
            {
                using (SqlCommand cmd = new SqlCommand("TSP_Respaldos", conn))
                {
                    //int opcion = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Opcion", 2));
                    var response = new List<Respaldo>();
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToRespaldo(reader));
                        }
                    }
                    return response;
                }
            }

        }

        public async Task<List<Sucursal>> GetRespaldoSucursales(string sucursales)
        {
            //sucursal = sucursal.OrderBy(o => o.Clave).ToList();
            await GetSucursales();
            if (sucursales != null && sucursales != "")
            {
                DataSet dsRespaldos = new DataSet("Respaldos");
                DataTable dtRespaldo;
                string resultado = "";
                int[] sucJson = JsonConvert.DeserializeObject<int[]>(sucursales);

                foreach (var suc in sucursales)
                {
                    Sucursal sucursal = Sucursales.Find(x => x.Clave.Equals(suc));
                    conexionDB = "Data Source=" + sucursal.Ip + ";Initial Catalog=" + sucursal.Db + ";User ID=analisis;Password=analisis20120203;";
                    using (SqlConnection conn = new SqlConnection(conexionDB))
                    {
                        using (SqlCommand cmd = new SqlCommand("TSP_Respaldos", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@Opcion", 1));
                            sucursal.Respaldos = new List<Respaldo>();
                            await conn.OpenAsync();
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    sucursal.Respaldos.Add(MapToRespaldo(reader));
                                    //response.Add(MapToRespaldo(reader));
                                }
                            }
                        }
                    }

                }
            }
            return new List<Sucursal>();
        }

        //public async Task<string> GetJobsRespaldos(string fecha, string sucursales)
        //{
        //    await GetSucursales();
        //    if (sucursales != null && sucursales != "")
        //    {
        //        DataSet dsRespaldos = new DataSet("Respaldos");
        //        DataTable dtRespaldo;
        //        string resultado = "";
        //        int[] sucJson = JsonConvert.DeserializeObject<int[]>(sucursales);
        //        DateTime fechaQ = DateTime.Parse(fecha); 

        //        foreach (var suc in sucJson)
        //        {
        //            dtRespaldo = new DataTable("" + suc + "");
        //            dtRespaldo.Columns.Add("sucursal", typeof(string));
        //            dtRespaldo.Columns.Add("jobName", typeof(string));
        //            dtRespaldo.Columns.Add("runDate", typeof(string));
        //            dtRespaldo.Columns.Add("Estatus", typeof(string));
        //            dtRespaldo.Columns.Add("Mensaje", typeof(string));

        //            Sucursal sucursal = Sucursales.Find(x => x.Clave.Equals(suc));

        //            conexionDB = "Data Source=" + sucursal.Ip + ";Initial Catalog=" + sucursal.Db + ";User ID=analisis;Password=analisis20120203;";
        //            try
        //            {
        //                using (SqlConnection conn = new SqlConnection(conexionDB))
        //                {
        //                    using (SqlCommand cmd = new SqlCommand("TSP_Respaldos", conn))
        //                    {
        //                        cmd.CommandType = CommandType.StoredProcedure;
        //                        cmd.Parameters.Add(new SqlParameter("@Opcion", 4));
        //                        cmd.Parameters.Add(new SqlParameter("@Fecha", fechaQ));

        //                        await conn.OpenAsync();
        //                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //                        {
        //                            await Task.Run(() => da.Fill(dtRespaldo));
        //                        }
        //                    }

        //                    dsRespaldos.Tables.Add(dtRespaldo);
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //                Console.WriteLine(ex);
        //            }
        //        }
        //        resultado = JsonConvert.SerializeObject(dsRespaldos, Formatting.Indented);
        //        return resultado;
        //    }
        //    else
        //    {
        //        return "No se encontraron sucursales disponibles.";
        //    }
        //}

        public async Task<IEnumerable<Servidor>> GetRutasRespaldos(string sucursales, List<Sucursal> catalogoSucs)
        {
            List<Servidor> lstServidores = new List<Servidor>();

            if (sucursales != null && sucursales != "")
            {
                DataSet dsRutasResp = new DataSet("Respaldos");
                DataTable dtRutasResp;
                string resultado = "";
                int[] sucJson = JsonConvert.DeserializeObject<int[]>(sucursales);

                foreach (var suc in sucJson)
                {
                    Servidor _servidor = new Servidor();                    
                    dtRutasResp = new DataTable("" + suc + "");
                    dtRutasResp.Columns.Add("Tipo", typeof(string));
                    dtRutasResp.Columns.Add("BD", typeof(string));
                    dtRutasResp.Columns.Add("NumeroSemana", typeof(int));
                    dtRutasResp.Columns.Add("RutaActual", typeof(string));
                    dtRutasResp.Columns.Add("RutaAnterior", typeof(string));

                    Sucursal sucursal = catalogoSucs.Find(x => x.Clave.Equals(suc));
                    _servidor.Sucursal = sucursal;

                    conexionDB = "Data Source=" + sucursal.Ip + ";Initial Catalog=" + sucursal.Db + ";User ID=analisis;Password=analisis20120203;";
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(conexionDB))
                        {
                            using (SqlCommand cmd = new SqlCommand("TSP_Respaldos", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@Opcion", 2));
                                List<RutaRespaldo> lstRutasRespaldos = new List<RutaRespaldo>();
                                try
                                {
                                    await conn.OpenAsync();
                                    using (var reader = await cmd.ExecuteReaderAsync())
                                    {
                                        while (await reader.ReadAsync())
                                        {
                                            lstRutasRespaldos.Add(MapToRutaRespaldo(reader));
                                        }
                                    }
                                    //using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                    //{
                                    //    await Task.Run(() => da.Fill(dtRutasResp));
                                    //    dsRutasResp.Tables.Add(dtRutasResp);
                                    //}
                                }
                                catch (SqlException ex)
                                {
                                    //SI SE MANDA UNA EXCEPCION DERIVADO DE QUE LA SUCURSAL NO TENGA CONEXION, SE AGREGAR AL DATASET PARA MOSTRARLO COMO RESULTADO
                                    if (ex.Number == 53)
                                    {
                                        DataRow row = dtRutasResp.NewRow();
                                        row["Mensaje"] = "No se pudo conectar con el servidor de la sucursal " + suc;
                                        row["Estatus"] = "SC";
                                        dtRutasResp.Rows.Add(row);
                                        dsRutasResp.Tables.Add(dtRutasResp);
                                        continue;
                                    }
                                }
                                _servidor.RutasRespaldos = lstRutasRespaldos;
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    lstServidores.Add(_servidor);
                }
                return lstServidores;
            }
            else
            {
                return lstServidores;
            }
        }

        public async Task<string> GetJobsRespaldos(string fecha, string sucursales, string tipoConsulta, List<Sucursal> catalogoSucs)
        {
            if (sucursales != null && sucursales != "")
            {
                DataSet dsRespaldos = new DataSet("Respaldos");
                DataTable dtRespaldo;
                string resultado = "";
                int[] sucJson = JsonConvert.DeserializeObject<int[]>(sucursales);
                DateTime fechaQ = DateTime.Parse(fecha);
                bool bLunes = false;
                bool bLog, bDistri, bTienda, bMaster, bModel, bMsdb;
                bLog = bDistri = bTienda = bMaster = bModel = bMsdb = false;

                if (fechaQ.DayOfWeek == DayOfWeek.Monday)
                {
                    bLunes = true;
                }

                foreach (var suc in sucJson)
                {
                    dtRespaldo = new DataTable("" + suc + "");
                    dtRespaldo.Columns.Add("sucursal", typeof(string));
                    dtRespaldo.Columns.Add("jobName", typeof(string));
                    dtRespaldo.Columns.Add("runDate", typeof(string));
                    dtRespaldo.Columns.Add("Estatus", typeof(string));
                    dtRespaldo.Columns.Add("Mensaje", typeof(string));

                    Sucursal sucursal = catalogoSucs.Find(x => x.Clave.Equals(suc));

                    conexionDB = "Data Source=" + sucursal.Ip + ";Initial Catalog=" + sucursal.Db + ";User ID=analisis;Password=analisis20120203;";
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(conexionDB))
                        {
                            using (SqlCommand cmd = new SqlCommand("TSP_Respaldos", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@Opcion", 4));
                                cmd.Parameters.Add(new SqlParameter("@Fecha", fechaQ));

                                try
                                {
                                    await conn.OpenAsync();
                                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                    {
                                        await Task.Run(() => da.Fill(dtRespaldo));
                                    }
                                }
                                catch(SqlException ex)
                                {
                                    //SI SE MANDA UNA EXCEPCION DERIVADO DE QUE LA SUCURSAL NO TENGA CONEXION, SE AGREGAR AL DATASET PARA MOSTRARLO COMO RESULTADO
                                    if (ex.Number == 53)
                                    {
                                        DataRow row = dtRespaldo.NewRow();
                                        row["Mensaje"] = "No se pudo conectar con el servidor de la sucursal " + suc;
                                        row["Estatus"] = "SC";
                                        dtRespaldo.Rows.Add(row);
                                        dsRespaldos.Tables.Add(dtRespaldo);
                                        continue;
                                    }
                                }                                                         
                            }

                            if (tipoConsulta == "Consulta")
                            {
                                dsRespaldos.Tables.Add(dtRespaldo);
                            }
                            else if (tipoConsulta == "Revision")
                            {
                                if (dtRespaldo.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtRespaldo.Rows.Count; i++)
                                    {
                                        // VALIDACIONES //
                                        // CUANDO UN REGISTRO VIENE CON ESTATUS DE ERROR SE AGREGA AL DATASET //
                                        if (dtRespaldo.Rows[i]["Estatus"].ToString() != "Exito")
                                        {
                                            dsRespaldos.Tables.Add(dtRespaldo);
                                            break;
                                        }

                                        // SE REVISA QUE SE EJECUTEN TODOS LOS RESPALDOS DEPENDIENDO, SI SON LOS SEMANALES O LOS DIARIOS //
                                        if (bLunes && dtRespaldo.Rows[i]["Estatus"].ToString() == "Exito")
                                        {
                                            switch (dtRespaldo.Rows[i][""].ToString())
                                            {
                                                case "Respaldo Semanal de Distribution":
                                                    bDistri = true;
                                                    continue;
                                                case "Respaldo Semanal de Master":
                                                    bMaster = true;
                                                    continue;
                                                case "Respaldo Semanal de Model":
                                                    bModel = true;
                                                    continue;
                                                case "Respaldo Semanal de Msdb":
                                                    bMsdb = true;
                                                    continue;
                                                case "Respaldo Semanal de Tienda":
                                                    bTienda = true;
                                                    continue;
                                                default:
                                                    continue;
                                            }
                                        }
                                        else if (bLunes == false && dtRespaldo.Rows[i]["Estatus"].ToString() == "Exito")
                                        {
                                            switch (dtRespaldo.Rows[i]["jobName"].ToString())
                                            {
                                                case "Respaldo Diario de Log Tienda":
                                                    bLog = true;
                                                    continue;
                                                case "Respaldo Diferencial de Distribution":
                                                    bDistri = true;
                                                    continue;
                                                case "Respaldo Diferencial de Tienda":
                                                    bTienda = true;
                                                    continue;
                                                default:
                                                    continue;
                                            }
                                        }
                                    }

                                    //SI FALTO DE EJECUTARSE ALGUN RESPALDO SE AGREGA AL DATASET PARA MOSTRARLO EN LOS RESULTADOS
                                    if (bLunes && (bDistri == false || bMaster == false || bModel == false || bMsdb == false || bTienda == false))
                                    {
                                        dsRespaldos.Tables.Add(dtRespaldo);
                                    }
                                    else if (bLunes == false && (bLog == false || bDistri == false || bTienda == false))
                                    {
                                        dsRespaldos.Tables.Add(dtRespaldo);
                                    }

                                }
                                else
                                {
                                    dsRespaldos.Tables.Add(dtRespaldo);
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                resultado = JsonConvert.SerializeObject(dsRespaldos, Formatting.Indented);
                return resultado;
            }
            else
            {
                return "No se encontraron sucursales disponibles.";
            }
        }


        public async Task<List<Sucursal>> GetSucursales()
        {
            using (SqlConnection conn = new SqlConnection(conexionDB))
            {
                using (SqlCommand cmd = new SqlCommand("TSP_Respaldos", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Opcion", 1));
                    Sucursales = new List<Sucursal>();
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Sucursales.Add(MapToSucursal(reader));
                        }
                    }
                    return Sucursales;
                }
            }
        }

        private Sucursal MapToSucursal(SqlDataReader reader)
        {
            return new Sucursal()
            {
                Clave = (int)reader["NOSUC"],
                Nombre = reader["NOMBRE"].ToString(),
                Ip = reader["IP"].ToString(),
                Db = reader["DB"].ToString(),
                Seleccionado = (bool)reader["SELECCIONADO"]
            };
        }

        private Respaldo MapToRespaldo(SqlDataReader reader)
        {
            return new Respaldo()
            {
                Tipo = reader["Tipo"].ToString(),
                DB = reader["BD"].ToString(),
                NumeroSemana = (byte)reader["NumeroSemana"],
                RutaActual = reader["RutaActual"].ToString(),
                RutaAnterior = reader["RutaAnterior"].ToString()
            };
        }

        private RutaRespaldo MapToRutaRespaldo(SqlDataReader reader)
        {
            return new RutaRespaldo()
            {
                Tipo = reader["Tipo"].ToString(),
                DB = reader["BD"].ToString(),
                NumeroSemana = (byte)reader["NumeroSemana"],
                RutaActual = reader["RutaActual"].ToString(),
                RutaAnterior = reader["RutaAnterior"].ToString()
            };
        }

    }
}
