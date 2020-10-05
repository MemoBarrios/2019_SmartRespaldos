using _2019_Respaldos.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace _2019_Respaldos.Data
{
    public class FuncionesGenerales
    {
        private static SqlConnection GetDbConnection(string SC)
        {
            //SC = SC + "application name=" + clsGlobalVar.AppName + ";";
            SqlConnection _conn = new SqlConnection(SC);
            _conn.Open();
            return _conn;
        }

        //LLena un DataTable enbase a una instruccion SQL
        public static DataTable LLenaDataTableSQL(string SQLConexion, string SQL)
        {
            DataTable dt = new DataTable();
            //SQLConexion = SQLConexion + "application name=" + clsGlobalVar.AppName + ";";
            using (SqlConnection conn = GetDbConnection(SQLConexion))
            {
                using (SqlCommand cmd = new SqlCommand(SQL, conn))
                {
                    //cmd.CommandTimeout = clsGlobalVar.TEspera;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        //llena un DataTable en base a un SP con conexion y sin parametros
        public static DataTable LlenaDataTableSpSQL(string SQLConexion, string SP)
        {
            DataTable dt = new DataTable();
            //SQLConexion = SQLConexion + "application name=" + clsGlobalVar.AppName + ";";
            using (SqlConnection conn = GetDbConnection(SQLConexion))
            {
                using (SqlCommand cmd = new SqlCommand(SP, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandTimeout = clsGlobalVar.TEspera;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            da.Fill(dt);
                        }
                        catch (SqlException e)
                        {
                            throw e;
                        }
                    }
                }
            }
            return dt;
        }

        //llena un DataTable en base a un SP con conexion y parametros
        public static DataTable LlenaDataTableSpSQL(string SQLConexion, string SP, params object[] Params)
        {
            DataTable dt = new DataTable();
            //SQLConexion = SQLConexion + "application name=" + clsGlobalVar.AppName + ";";
            using (SqlConnection conn = GetDbConnection(SQLConexion))
            {
                using (SqlCommand cmd = new SqlCommand(SP, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandTimeout = clsGlobalVar.TEspera;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    for (int i = 1; i < cmd.Parameters.Count; i++)
                    {
                        cmd.Parameters[i].Value = Params[i - 1];
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            da.Fill(dt);
                        }
                        catch (SqlException e)
                        {
                            throw e;
                        }
                    }
                }
            }
            return dt;
        }

        //llena un DataTable en base a un SP sin parametros
        /* public static DataTable LlenaDataTableSP(string SP)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = GetDbConnection(clsGlobalVar.SQLCon))
            {
                using (SqlCommand cmd = new SqlCommand(SP, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = clsGlobalVar.TEspera;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            da.Fill(dt);
                        }
                        catch (SqlException e)
                        {
                            throw e;
                        }
                    }
                }
            }
            return dt;
        }*/

        //llena un DataTable en base a un SP con parametros
        /* public static DataTable LlenaDataTableSP(string SP, params object[] Params)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetDbConnection(clsGlobalVar.SQLCon))
            {
                using (SqlCommand cmd = new SqlCommand(SP, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = clsGlobalVar.TEspera;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    for (int i = 1; i < cmd.Parameters.Count; i++)
                    {
                        cmd.Parameters[i].Value = Params[i - 1];
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            da.Fill(dt);
                        }
                        catch (SqlException e)
                        {
                            throw e;
                        }
                    }
                }
            }
            return dt;
        }*/

        /* public static void EXE_SP(string SP, params object[] Params)
        {
            using (SqlConnection conn = GetDbConnection(clsGlobalVar.SQLCon))
            {
                using (SqlCommand cmd = new SqlCommand(SP, conn))
                {
                    cmd.CommandTimeout = clsGlobalVar.TEspera;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);
                    for (int i = 1; i < cmd.Parameters.Count; i++)
                    {
                        cmd.Parameters[i].Value = Params[i - 1];
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }*/
    }
}
