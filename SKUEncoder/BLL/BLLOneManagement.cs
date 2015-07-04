using SKUEncoder.DAL;
using SKUEncoder.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;

namespace SKUEncoder.BLL
{
    /// <summary>
    /// 一级目录管理业务逻辑层
    /// </summary>
    public class BLLOneManagement
    {
        private DALOneManagement _dal;

        public BLLOneManagement()
        {
            _dal = new DALOneManagement();
        }

        public List<SKUCGY> GetOneList()
        {
            List<SKUCGY> result = null;
            try
            {
                DataTable dt = _dal.GetOneList();
                if(dt != null && dt.Rows.Count > 0)
                {
                    result = new List<SKUCGY>();
                    SKUCGY cgy;
                    foreach(DataRow dr in dt.Rows)
                    {
                        Guid ID = dr.GetFieldValue<Guid>("ID");
                        string code = dr.GetFieldValue<string>("CODE");
                        string name = dr.GetFieldValue<string>("NAME");
                        Guid PID = dr.GetFieldValue<Guid>("PID");
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
            catch(Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("获取一级目录列表出错:BLLOneManagement.GetOneList()", e);
            }

            return result;
        }

        public bool AddOne(SKUCGY cgy)
        {
            bool result = false;
            try
            {
                int count = _dal.AddOne(cgy);
                if(count > 0)
                {
                    result = true;
                }
            }
            catch(Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("判断一级Code是否存在出错:BLLOneManagement.AddOne(SKUCGY cgy)", e);
            }
            return result;
        }

        public bool UpdateOne(SKUCGY cgy)
        {
            bool result = false;
            try
            {
                int count = _dal.UpdateOne(cgy);
                if(count > 0)
                {
                    result = true;
                }
            }
            catch(Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("更新一级Code是否存在出错:BLLOneManagement.UpdateOne(SKUCGY cgy)", e);
            }
            return result;
        }

        public bool DeleteOnes(List<Guid> IDs)
        {
            bool result = false;
            try
            {
                //int count = _dal.DeleteOnes(IDs);
                //if(count > 0)
                //{
                //    result = true;
                //}
                result = _dal.DeleteOnes(IDs);
            }
            catch(Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("删除一级出错:BLLOneManagement.DeleteOnes(List<Guid> IDs)", e);
            }
            return result;
        }

        public bool ImportOnes(List<SKUCGY> cgys)
        {
            bool result = false;
            try
            {
                result = _dal.ImportOnes(cgys);
            }
            catch(Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("批量添加一级出错:BLLOneManagement.ImportOnes(List<SKUCGY> cgys)", e);
            }
            return result;
        }

        public bool IsOneCodeExits(string Code)
        {
            bool isOneCodeExists = false;
            try
            {
                object obj = _dal.IsOneCodeExits(Code);
                if(obj != null)
                {
                    int count;
                    int.TryParse(obj.ToString(), out count);
                    if(count > 0)
                    {
                        isOneCodeExists = true;
                    }
                }
            }
            catch(Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("判断一级Code是否存在出错:BLLOneManagement.IsOneCodeExits(string Code)", e);
            }
            return isOneCodeExists;
        }
    }
}
