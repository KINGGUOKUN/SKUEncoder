using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using SKUEncoder.Entity;
using System.Data;

namespace SKUEncoder.DAL
{
    public class DALSKUEncodeManagement
    {
        private Database _database;

        public DALSKUEncodeManagement()
        {
            _database = DatabaseFactory.CreateDatabase();
        }

        public DataTable GetSKUEncodeByCondition(SKUSearchParams param)
        {
            DataTable result = new DataTable();
            StringBuilder sbSql = new StringBuilder();
//            sbSql.Append(@"SELECT C.*, A.SKUCID
//                           FROM SKUENCODE C
//                           LEFT JOIN SKUATT A ON C.ATT3ID = A.ID 
//                           WHERE 1 = 1 ");
            //if(param.IDTwo != Guid.Empty)
            //{
            //    sbSql.Append(@" AND SKUCID = @SKUCID ");
            //}
            sbSql.Append(@"WITH SUBQRY(ID)
                        	AS 
                        	(
                        		SELECT ID 
                        		FROM SKUCGY
                        		WHERE ID = @SelectedSKUCID
                        		UNION ALL
                        		SELECT SKUCGY.ID
                        		FROM SKUCGY, SUBQRY
                        		WHERE SKUCGY.PID = SUBQRY.ID
                        	)
                             SELECT C.*, A.SKUCID
                             FROM SKUENCODE C
                             LEFT JOIN SKUATT A ON C.ATT3ID = A.ID 
                             WHERE A.SKUCID IN
                             (
                             	SELECT ID FROM SUBQRY
                             ) ");
            if(param.Att3 != Guid.Empty)
            {
                sbSql.Append(@" AND ATT3ID = @ATT3ID ");
            }
            if (param.Att4 != Guid.Empty)
            {
                sbSql.Append(@" AND ATT4ID = @ATT4ID ");
            }
            if (param.Att5 != Guid.Empty)
            {
                sbSql.Append(@" AND ATT5ID = @ATT5ID ");
            }
            if (param.Att6 != Guid.Empty)
            {
                sbSql.Append(@" AND ATT6ID = @ATT6ID ");
            }
            if (param.Att7 != Guid.Empty)
            {
                sbSql.Append(@" AND ATT7ID = @ATT7ID ");
            }
            if(!string.IsNullOrWhiteSpace(param.Keyword))
            {
                //sbSql.Append(@" AND C.CODE + C.NAME LIKE '%@Keyword%' ");
                sbSql.Append(string.Format(" AND C.CODE + C.NAME LIKE '%{0}%' ", param.Keyword));
            }
            using (DbCommand cmd = _database.GetSqlStringCommand(sbSql.ToString()))
            {
                //if (param.IDTwo != Guid.Empty)
                //{
                //    _database.AddInParameter(cmd, "@SKUCID", DbType.Guid, param.IDTwo);
                //}
                _database.AddInParameter(cmd, "@SelectedSKUCID", DbType.Guid, param.SelectedSKUCID);
                if (param.Att3 != Guid.Empty)
                {
                    _database.AddInParameter(cmd, "@ATT3ID", DbType.Guid, param.Att3);
                }
                if (param.Att4 != Guid.Empty)
                {
                    _database.AddInParameter(cmd, "@ATT4ID", DbType.Guid, param.Att4);
                }
                if (param.Att5 != Guid.Empty)
                {
                    _database.AddInParameter(cmd, "@ATT5ID", DbType.Guid, param.Att5);
                }
                if (param.Att6 != Guid.Empty)
                {
                    _database.AddInParameter(cmd, "@ATT6ID", DbType.Guid, param.Att6);
                }
                if (param.Att7 != Guid.Empty)
                {
                    _database.AddInParameter(cmd, "@ATT7ID", DbType.Guid, param.Att7);
                }
                //if (!string.IsNullOrWhiteSpace(param.Keyword))
                //{
                //    _database.AddInParameter(cmd, "@Keyword", DbType.String, param.Keyword);
                //}
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
        public int AddSKUEncode(SKUEncode encode)
        {
            int result = -1;
            string sql = @"INSERT INTO SKUENCODE
                           (ID, CODE, NAME, ATT3ID, ATT4ID, ATT5ID, ATT6ID, ATT7ID)
                           VALUES(@ID, @CODE, @NAME, @ATT3ID, @ATT4ID, @ATT5ID, @ATT6ID, @ATT7ID)";
            using (DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@ID", DbType.Guid, encode.ID);
                _database.AddInParameter(cmd, "@CODE", DbType.String, encode.Code);
                _database.AddInParameter(cmd, "@NAME", DbType.String, encode.Name);
                _database.AddInParameter(cmd, "@ATT3ID", DbType.Guid, encode.Att3ID);
                _database.AddInParameter(cmd, "@ATT4ID", DbType.Guid, encode.Att4ID);
                _database.AddInParameter(cmd, "@ATT5ID", DbType.Guid, encode.Att5ID);
                _database.AddInParameter(cmd, "@ATT6ID", DbType.Guid, encode.Att6ID);
                _database.AddInParameter(cmd, "@ATT7ID", DbType.Guid, encode.Att7ID);
                result = _database.ExecuteNonQuery(cmd);
            }
            return result;
        }

        /// <summary>
        /// 更新属性名称
        /// </summary>
        /// <param name="att"></param>
        /// <returns></returns>
        public int UpdateSKUEncode(SKUEncode encode)
        {
            int result = -1;
            string sql = @"UPDATE SKUENCODE 
                           SET NAME = @NAME
                           WHERE ID = @ID";
            using (DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@NAME", DbType.String, encode.Name);
                _database.AddInParameter(cmd, "@ID", DbType.Guid, encode.ID);
                result = _database.ExecuteNonQuery(cmd);
            }
            return result;
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public int DeleteSKUEncodes(List<Guid> IDs)
        {
            int result = -1;
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("DELETE FROM SKUENCODE WHERE ID IN (");
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
    }
}
