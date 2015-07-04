using Microsoft.Practices.EnterpriseLibrary.Data;
using SKUEncoder.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.DAL
{
    /// <summary>
    /// 一级目录管理数据访问层
    /// </summary>
    public class DALOneManagement
    {
        private Database _database;

        public DALOneManagement()
        {
            _database = DatabaseFactory.CreateDatabase();
        }

        public DataTable GetOneList()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT * 
                           FROM SKUCGY 
                           WHERE LEVELINDEX = 1
                           ORDER BY CODE";
            using (IDataReader reader = _database.ExecuteReader(CommandType.Text, sql))
            {
                dt.Load(reader);
            }
            return dt;
        }

        public int AddOne(SKUCGY cgy)
        {
            int result = -1;
            string sql = @"INSERT INTO SKUCGY
                           (ID, CODE, NAME, PID, LEVELINDEX)
                           VALUES(@ID, @CODE, @NAME, @PID, @LEVELINDEX)";
            using(DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@ID", DbType.Guid, cgy.ID);
                _database.AddInParameter(cmd, "@CODE", DbType.String, cgy.Code);
                _database.AddInParameter(cmd, "@NAME", DbType.String, cgy.Name);
                _database.AddInParameter(cmd, "@PID", DbType.Guid, cgy.PID);
                _database.AddInParameter(cmd, "@LEVELINDEX", DbType.Int16, cgy.LevelIndex);
                result = _database.ExecuteNonQuery(cmd);
            }
            return result;
        }

        public int UpdateOne(SKUCGY cgy)
        {
            int result = -1;
            string sql = @"UPDATE SKUCGY 
                           SET NAME = @NAME
                           WHERE ID = @ID";
            using(DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@NAME", DbType.String, cgy.Name);
                _database.AddInParameter(cmd, "@ID", DbType.Guid, cgy.ID);
                result = _database.ExecuteNonQuery(cmd);
            }
            return result;
        }

        public bool DeleteOnes(List<Guid> IDs)
        {
            #region 完善前删除一级
            //int result = -1;
            //StringBuilder sbSql = new StringBuilder();
            //sbSql.Append("DELETE FROM SKUCGY WHERE ID IN (");
            //IDs.ForEach(id =>
            //{
            //    sbSql.Append(string.Format("'{0}',", id));
            //});
            //sbSql.Remove(sbSql.Length - 1, 1).Append(")");
            //using (DbCommand cmd = _database.GetSqlStringCommand(sbSql.ToString()))
            //{
            //    result = _database.ExecuteNonQuery(cmd);
            //}
            //return result;
            #endregion 

            #region 完善后删除一级
            bool result = false;
            string sql = @"DELETE FROM SKUCGY WHERE ID = @ID";
            string sqlCasecade = @"DELETE FROM SKUCGY WHERE PID = @PID";
            using (DbConnection con = _database.CreateConnection())
            {
                con.Open();
                using (DbTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        foreach (var ID in IDs)
                        {
                            using (DbCommand cmd = _database.GetSqlStringCommand(sql))
                            {
                                _database.AddInParameter(cmd, "@ID", DbType.Guid, ID);
                                _database.ExecuteNonQuery(cmd);
                            }
                            using(DbCommand cmd = _database.GetSqlStringCommand(sqlCasecade))
                            {
                                _database.AddInParameter(cmd, "@PID", DbType.Guid, ID);
                                _database.ExecuteNonQuery(cmd);
                            }
                        }
                        trans.Commit();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                        Trace.TraceError(excepMsg);
                        throw new Exception("删除一级出错:DALOneManagement.DeleteOnes(List<Guid> IDs)", e);
                    }
                }
                con.Close();
            }
            return result;
            #endregion
        }

        public bool ImportOnes(List<SKUCGY> cgys)
        {
            bool result = false;
            string sql = @"INSERT INTO SKUCGY
                           (ID, CODE, NAME, PID, LEVELINDEX)
                           VALUES(@ID, @CODE, @NAME, @PID, @LEVELINDEX)";
            using(DbConnection con = _database.CreateConnection())
            {
                con.Open();
                using(DbTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        foreach (var cgy in cgys)
                        {
                            using (DbCommand cmd = _database.GetSqlStringCommand(sql))
                            {
                                _database.AddInParameter(cmd, "@ID", DbType.Guid, cgy.ID);
                                _database.AddInParameter(cmd, "@CODE", DbType.String, cgy.Code);
                                _database.AddInParameter(cmd, "@NAME", DbType.String, cgy.Name);
                                _database.AddInParameter(cmd, "@PID", DbType.Guid, cgy.PID);
                                _database.AddInParameter(cmd, "@LEVELINDEX", DbType.Int16, cgy.LevelIndex);
                                _database.ExecuteNonQuery(cmd);
                            }
                        }
                        trans.Commit();
                        result = true;
                    }
                    catch(Exception e)
                    {
                        trans.Rollback();
                        string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                        Trace.TraceError(excepMsg);
                        throw new Exception("批量添加一级出错:DALOneManagement.ImportOnes(List<SKUCGY> cgys)", e);
                    }
                }
                con.Close();
            }
            return result;
        }

        public object IsOneCodeExits(string Code)
        {
            object obj = null;
            string sql = @"SELECT COUNT(*) 
                           FROM SKUCGY 
                           WHERE LEVELINDEX = 1
                           AND CODE = @CODE ";
            using(DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "CODE", DbType.String, Code);
                obj = _database.ExecuteScalar(cmd);
            }
            return obj;
        }
    }
}
