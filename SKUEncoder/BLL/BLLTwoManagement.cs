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
    public class BLLTwoManagement
    {
        private DALTwoManagement _dal;

        public BLLTwoManagement()
        {
            _dal = new DALTwoManagement();
        }

        public List<SKUCGY> GetTwoList(Guid pid)
        {
            List<SKUCGY> result = null;
            try
            {
                DataTable dt = _dal.GetTwoList(pid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new List<SKUCGY>();
                    SKUCGY cgy;
                    foreach (DataRow dr in dt.Rows)
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
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("获取二级目录列表出错:BLLTwoManagement.GetTwoList(Guid pid)", e);
            }

            return result;
        }

        public bool AddTwo(SKUCGY cgy)
        {
            bool result = false;
            try
            {
                int count = _dal.AddTwo(cgy);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("添加二级出错:BLLTwoManagement.AddTwo(SKUCGY cgy)", e);
            }
            return result;
        }

        public bool UpdateTwo(SKUCGY cgy)
        {
            bool result = false;
            try
            {
                int count = _dal.UpdateTwo(cgy);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("更新二级出错:BLLTwoManagement.UpdateOne(SKUCGY cgy)", e);
            }
            return result;
        }

        public bool DeleteTwos(List<Guid> IDs)
        {
            bool result = false;
            try
            {
                int count = _dal.DeleteTwos(IDs);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("删除二级出错:BLLTwoManagement.DeleteTwos(List<Guid> IDs)", e);
            }
            return result;
        }

        public bool ImportTwos(List<SKUCGY> cgys)
        {
            bool result = false;
            try
            {
                result = _dal.ImportTwos(cgys);
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("批量导入二级出错:BLLTwoManagement.ImportTwos(List<SKUCGY> cgys)", e);
            }
            return result;
        }

        public bool IsTwoCodeExits(Guid pid, string Code)
        {
            bool isTwoCodeExists = false;
            try
            {
                object obj = _dal.IsTwoCodeExits(pid, Code);
                if (obj != null)
                {
                    int count;
                    int.TryParse(obj.ToString(), out count);
                    if (count > 0)
                    {
                        isTwoCodeExists = true;
                    }
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("判断二级Code是否存在出错:BLLTwoManagement.IsTwoCodeExits(Guid pid, string Code)", e);
            }
            return isTwoCodeExists;
        }
    }
}
