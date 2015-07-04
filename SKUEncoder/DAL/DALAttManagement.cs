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
    /// 属性管理DAL
    /// </summary>
    public class DALAttManagement
    {
        private Database _database; 

        public DALAttManagement()
        {
            _database = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 获取所有一级和二级
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllSKUCGY()
        {
            DataTable result = new DataTable();
            string sql = @"SELECT * FROM SKUCGY ORDER BY CODE";
            using (IDataReader reader = _database.ExecuteReader(CommandType.Text, sql))
            {
                result.Load(reader);
            }
            return result;
        }

        public DataTable GetAllAttType()
        {
            DataTable result = new DataTable();
            string sql = @"SELECT * FROM ATTTYPE ORDER BY ID";
            using(IDataReader reader = _database.ExecuteReader(CommandType.Text, sql))
            {
                result.Load(reader);
            }
            return result;
        }

        public DataTable GetSKUATTByCondition(Guid SKUCID, short attTypeID)
        {
            DataTable result = new DataTable();
            string sql = @"SELECT * FROM SKUATT 
                           WHERE SKUCID = @SKUCID 
                           AND ATTTYPE = @ATTTYPE
                           ORDER BY CODE";
            using(DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@SKUCID", DbType.Guid, SKUCID);
                _database.AddInParameter(cmd, "@ATTTYPE", DbType.Int16, attTypeID);
                using(IDataReader reader = _database.ExecuteReader(cmd))
                {
                    result.Load(reader);
                }
            }

            return result;
        }

        public DataTable GetSKUATTByCondition(Guid SKUCID)
        {
            DataTable result = new DataTable();
            string sql = @"SELECT * FROM SKUATT 
                           WHERE SKUCID = @SKUCID 
                           ORDER BY CODE";
            using (DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@SKUCID", DbType.Guid, SKUCID);
                using (IDataReader reader = _database.ExecuteReader(cmd))
                {
                    result.Load(reader);
                }
            }

            return result;
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="cgy"></param>
        /// <returns></returns>
        public int AddATT(SKUATT att)
        {
            int result = -1;
            string sql = @"INSERT INTO SKUATT
                           (ID, CODE, NAME, ATTTYPE, SKUCID)
                           VALUES(@ID, @CODE, @NAME, @ATTTYPE, @SKUCID)";
            using (DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@ID", DbType.Guid, att.ID);
                _database.AddInParameter(cmd, "@CODE", DbType.String, att.Code);
                _database.AddInParameter(cmd, "@NAME", DbType.String, att.Name);
                _database.AddInParameter(cmd, "@ATTTYPE", DbType.Int16, att.ATTType);
                _database.AddInParameter(cmd, "@SKUCID", DbType.Guid, att.SKUID);
                result = _database.ExecuteNonQuery(cmd);
            }
            return result;
        }

        /// <summary>
        /// 更新属性名称
        /// </summary>
        /// <param name="att"></param>
        /// <returns></returns>
        public int UpdateATT(SKUATT att)
        {
            int result = -1;
            string sql = @"UPDATE SKUATT 
                           SET NAME = @NAME
                           WHERE ID = @ID";
            using (DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@NAME", DbType.String, att.Name);
                _database.AddInParameter(cmd, "@ID", DbType.Guid, att.ID);
                result = _database.ExecuteNonQuery(cmd);
            }
            return result;
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public int DeleteATTs(List<Guid> IDs)
        {
            int result = -1;
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("DELETE FROM SKUATT WHERE ID IN (");
            IDs.ForEach(id =>
            {
                sbSql.Append(string.Format("'{0}',", id));
            });
            sbSql.Remove(sbSql.Length - 1, 1).Append(")");
            using (DbCommand cmd = _database.GetSqlStringCommand(sbSql.ToString()))
            {
                result = _database.ExecuteNonQuery(cmd);
            }
            return result;
        }

        public bool ImportATTs(List<SKUATT> atts)
        {
            bool result = false;
            string sql = @"INSERT INTO SKUATT
                           (ID, CODE, NAME, ATTTYPE, SKUCID)
                           VALUES(@ID, @CODE, @NAME, @ATTTYPE, @SKUCID)";
            using (DbConnection con = _database.CreateConnection())
            {
                con.Open();
                using (DbTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        foreach (var att in atts)
                        {
                            using (DbCommand cmd = _database.GetSqlStringCommand(sql))
                            {
                                _database.AddInParameter(cmd, "@ID", DbType.Guid, att.ID);
                                _database.AddInParameter(cmd, "@CODE", DbType.String, att.Code);
                                _database.AddInParameter(cmd, "@NAME", DbType.String, att.Name);
                                _database.AddInParameter(cmd, "@ATTTYPE", DbType.Int16, att.ATTType);
                                _database.AddInParameter(cmd, "@SKUCID", DbType.Guid, att.SKUID);
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
                        throw new Exception("批量添加属性出错:DALAttManagement.ImportATTs(List<SKUATT> atts)", e);
                    }
                }
                con.Close();
            }
            return result;
        }

        public object IsATTCodeExits(short attTypeID, Guid skucid, string Code)
        {
            object obj = null;
            string sql = @"SELECT COUNT(*) 
                           FROM SKUATT 
                           WHERE SKUCID = @SKUCID
                           AND ATTTYPE = @ATTTYPE
                           AND CODE = @CODE ";
            using (DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@SKUCID", DbType.Guid, skucid);
                _database.AddInParameter(cmd, "@ATTTYPE", DbType.Int16, attTypeID);
                _database.AddInParameter(cmd, "CODE", DbType.String, Code);
                obj = _database.ExecuteScalar(cmd);
            }
            return obj;
        }

    }
}
