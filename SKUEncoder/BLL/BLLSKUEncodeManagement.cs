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
    public class BLLSKUEncodeManagement
    {
        private DALSKUEncodeManagement _dal;

        public BLLSKUEncodeManagement()
        {
            _dal = new DALSKUEncodeManagement();
        }

        public List<SKUEncode> GetSKUEncodeByCondition(SKUSearchParams param)
        {
            List<SKUEncode> result = null;
            try
            {
                DataTable dt = _dal.GetSKUEncodeByCondition(param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new List<SKUEncode>();
                    SKUEncode encode;
                    foreach (DataRow dr in dt.Rows)
                    {
                        Guid ID = dr.GetFieldValue<Guid>("ID");
                        string code = dr.GetFieldValue<string>("CODE");
                        string name = dr.GetFieldValue<string>("NAME");
                        Guid ATT3ID = dr.GetFieldValue<Guid>("ATT3ID");
                        Guid ATT4ID = dr.GetFieldValue<Guid>("ATT4ID");
                        Guid ATT5ID = dr.GetFieldValue<Guid>("ATT5ID");
                        Guid ATT6ID = dr.GetFieldValue<Guid>("ATT6ID");
                        Guid ATT7ID = dr.GetFieldValue<Guid>("ATT7ID");
                        encode = new SKUEncode(ID)
                        {
                            Code = code,
                            Name = name,
                            Att3ID = ATT3ID,
                            Att4ID = ATT4ID,
                            Att5ID = ATT5ID,
                            Att6ID = ATT6ID,
                            Att7ID = ATT7ID,
                        };
                        result.Add(encode);
                    }
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("获取SKU编码列表出错:BLLSKUEncodeManagement.GetSKUEncodeByCondition(SKUSearchParams param)", e);
            }

            return result;
        }

        public bool AddSKUEncode(SKUEncode encode)
        {
            bool result = false;
            try
            {
                int count = _dal.AddSKUEncode(encode);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("添加SKU编码出错:BLLSKUEncodeManagement.AddSKUEncode(SKUEncode encode)", e);
            }
            return result;
        }

        public bool UpdateSKUEncode(SKUEncode encode)
        {
            bool result = false;
            try
            {
                int count = _dal.UpdateSKUEncode(encode);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("更新SKU编码出错:BLLSKUEncodeManagement.UpdateSKUEncode(SKUEncode encode)", e);
            }
            return result;
        }

        public bool DeleteSKUEncodes(List<Guid> IDs)
        {
            bool result = false;
            try
            {
                int count = _dal.DeleteSKUEncodes(IDs);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                string excepMsg = string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message);
                Trace.TraceError(excepMsg);
                throw new Exception("删除属性出错:BLLSKUEncodeManagement.DeleteTwos(List<Guid> IDs)", e);
            }
            return result;
        }


    }
}
