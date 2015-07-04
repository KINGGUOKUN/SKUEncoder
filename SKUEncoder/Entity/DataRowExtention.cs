using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.Entity
{
    /// <summary>
    /// DataRow扩展类
    /// </summary>
    public static class DataRowExtention
    {
        /// <summary>
        /// 从DataRow中获取制定字段名的字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T GetFieldValue<T>(this DataRow dr, string fieldName)
        {
            return dr.IsNull(fieldName) ? default(T) : dr.Field<T>(fieldName);
        }
    }
}
