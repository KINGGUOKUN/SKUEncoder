using SKUEncoder.DAL;
using SKUEncoder.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.BLL
{
    /// <summary>
    /// 属性管理
    /// </summary>
    public class BLLAttManagement
    {
        private DALAttManagement _dal;

        public BLLAttManagement()
        {
            _dal = new DALAttManagement();
        }

        public List<SKUCGY> GetAllSKUCGY()
        {
            List<SKUCGY> result = null;
            try
            {
                DataTable dt = _dal.GetAllSKUCGY();
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new List<SKUCGY>();
                    SKUCGY cgy;
                    foreach (DataRow dr in dt.Rows)
                    {
                        Guid ID = dr.GetFieldValue<Guid>("ID");
                        string code = dr.GetFieldValue<string>("CODE");
                        string name = dr.GetFieldValue<string>("NAME");
                        Guid? PID = dr.GetFieldValue<Guid?>("PID");
                        short levelIndex = dr.GetFieldValue<short>("LEVELINDEX");
                        cgy = new SKUCGY(ID)
                        {
                            Code = code,
                            Name = name,
                            PID = PID,
                            LevelIndex = levelIndex
                        };
                        result.Add(cgy);
                    }
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("获取一级目录列表出错:BLLAttManagement.GetAllSKUCGY()", e);
            }

            return result;
        }

        public List<ATTType> GetAllAttType()
        {
            List<ATTType> result = null;
            try
            {
                DataTable dt = _dal.GetAllAttType();
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new List<ATTType>();
                    ATTType attType;
                    foreach (DataRow dr in dt.Rows)
                    {
                        short ID = dr.GetFieldValue<short>("ID");
                        string name = dr.GetFieldValue<string>("NAME");
                        attType = new ATTType(ID, name);
                        result.Add(attType);
                    }
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("获取属性列表出错:BLLAttManagement.GetAllAttType()", e);
            }

            return result;
        }

        public List<SKUATT> GetSKUATTByCondition(Guid SKUCID, short ATTTypeID)
        {
            List<SKUATT> result = null;
            try
            {
                DataTable dt = _dal.GetSKUATTByCondition(SKUCID, ATTTypeID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new List<SKUATT>();
                    SKUATT attType;
                    foreach (DataRow dr in dt.Rows)
                    {
                        Guid ID = dr.GetFieldValue<Guid>("ID");
                        string code = dr.GetFieldValue<string>("CODE");
                        string name = dr.GetFieldValue<string>("NAME");
                        short attTypeID = dr.GetFieldValue<short>("ATTTYPE");
                        Guid skucid = dr.GetFieldValue<Guid>("SKUCID");
                        attType = new SKUATT(ID)
                        {
                            Code = code,
                            Name = name,
                            ATTType = attTypeID,
                            SKUID = skucid,
                        };
                        result.Add(attType);
                    }
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("获取属性列表出错:BLLAttManagement.GetSKUATTBySKUID(Guid SKUCID)", e);
            }

            return result;
        }

        /// <summary>
        /// 根据SKUCID获取所有属性
        /// </summary>
        /// <param name="SKUCID"></param>
        /// <returns></returns>
        public List<SKUATT> GetSKUATTByCondition(Guid SKUCID)
        {
            List<SKUATT> result = null;
            try
            {
                DataTable dt = _dal.GetSKUATTByCondition(SKUCID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new List<SKUATT>();
                    SKUATT attType;
                    foreach (DataRow dr in dt.Rows)
                    {
                        Guid ID = dr.GetFieldValue<Guid>("ID");
                        string code = dr.GetFieldValue<string>("CODE");
                        string name = dr.GetFieldValue<string>("NAME");
                        short attTypeID = dr.GetFieldValue<short>("ATTTYPE");
                        Guid skucid = dr.GetFieldValue<Guid>("SKUCID");
                        attType = new SKUATT(ID)
                        {
                            Code = code,
                            Name = name,
                            ATTType = attTypeID,
                            SKUID = skucid,
                        };
                        result.Add(attType);
                    }
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("获取属性列表出错:BLLAttManagement.GetSKUATTBySKUID(Guid SKUCID)", e);
            }

            return result;
        }

        public bool AddATT(SKUATT att)
        {
            bool result = false;
            try
            {
                int count = _dal.AddATT(att);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("添加属性出错:BLLAttManagement.AddATT(SKUATT att)", e);
            }
            return result;
        }

        public bool UpdateATT(SKUATT att)
        {
            bool result = false;
            try
            {
                int count = _dal.UpdateATT(att);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("更新属性出错:BLLAttManagement.UpdateATT(SKUATT att)", e);
            }
            return result;
        }

        public bool DeleteATTs(List<Guid> IDs)
        {
            bool result = false;
            try
            {
                int count = _dal.DeleteATTs(IDs);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("删除属性出错:BLLAttManagement.DeleteTwos(List<Guid> IDs)", e);
            }
            return result;
        }

        public bool ImportATTs(List<SKUATT> atts)
        {
            bool result = false;
            try
            {
                result = _dal.ImportATTs(atts);
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("批量导入属性出错:BLLAttManagement.ImportATTs(List<SKUATT> atts)", e);
            }
            return result;
        }

        public bool IsATTCodeExits(short attTypeID, Guid skucid, string Code)
        {
            bool isATTCodeExists = false;
            try
            {
                object obj = _dal.IsATTCodeExits(attTypeID, skucid, Code);
                if (obj != null)
                {
                    int count;
                    int.TryParse(obj.ToString(), out count);
                    if (count > 0)
                    {
                        isATTCodeExists = true;
                    }
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("判断属性Code是否存在出错:BLLAttManagement.IsATTCodeExits(short attTypeID, Guid skucid, string Code)", e);
            }
            return isATTCodeExists;
        }

    }
}
